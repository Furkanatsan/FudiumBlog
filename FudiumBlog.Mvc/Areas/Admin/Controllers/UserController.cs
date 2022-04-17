using AutoMapper;
using FudiumBlog.Entities.Concrete;
using FudiumBlog.Entities.Dtos;
using FudiumBlog.Mvc.Areas.Admin.Models;
using FudiumBlog.Shared.Utilities.Extensions;
using FudiumBlog.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FudiumBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env; //wwwroot un dosya yolunu dinamik olarak almak için 
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _signInManager = signInManager;
        }
        [Authorize(Roles ="Admin")]//bu actiona sadece admin rolüne sahip kullanıcı girebilir.
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();//tüm kullanıcıları liste halinde dönecek
            return View(new UserListDto{
            Users=users,
            ResultStatus=ResultStatus.Success
            });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");//action adı ile view uyuşmadığından returnda belirttik.
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user!=null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlış");
                        return View("UserLogin");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresiniz veya şifreniz yanlış");
                    return View("UserLogin");
                }
            }
            else
            {
                return View("UserLogin");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });//kullanıcı çıkış yapınca blog sayfa ındexine yönlendirilir
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();//tüm kullanıcıları liste halinde dönecek
            var userListDto= JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            },new JsonSerializerOptions { 
            ReferenceHandler=ReferenceHandler.Preserve//tüm değerleri jsona cevirebiliriz.
            
            });
            return Json(userListDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]//ekleme işlemi
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName,userAddDto.PictureFile);
               var user=_mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto=new UserDto
                        {
                            ResultStatus =ResultStatus.Success,
                            Message=$"{user.UserName} adlı kullanıcı başarıyla eklenmiştir.",
                            User=user
                        },
                        UserAddPartial=await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
                    });
                    return Json(userAddAjaxModel);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel {
                        UserAddDto = userAddDto,
                        UserAddPartial=await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
                    });
                    return Json(userAddAjaxErrorModel);
                }
            }
            var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorModel);


        }

        //silme işlemi
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir.",
                    User = user
                });
                return Json(deletedUser);

            }
            else
            {
                string errorMessages = string.Empty;
                foreach (var error in result.Errors)
                {
                   errorMessages=$"*{error.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto {
                ResultStatus=ResultStatus.Error,
                Message= $"{user.UserName} adlı kullanıcı silinirken hata oluştu.\n{errorMessages}",
                User=user
                });
                return Json(deletedUserErrorModel);
            }

        }

        //update işlemi
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);//partialview i modeli ile return ediyoruz.
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile!=null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    isNewPictureUploaded = true;

                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded==true)//yeni bir resim eklendi mi
                    {
                        ImageDelete(oldUserPicture);
                    }
                    var userUpdateViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel {
                    UserDto=new UserDto
                    {
                        ResultStatus=ResultStatus.Success,
                        Message =$"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.",
                        User=updatedUser
                    },
                    UserUpdatePartial=await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)//partiali userUpdateDto modeli ile göndermek için
                    
                    });
                    return Json(userUpdateViewModal);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto=userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)//partiali userUpdateDto modeli ile göndermek için

                    });
                    return Json(userUpdateErrorViewModal);
                }


            }
            else
            {
                var userUpdateModelStateErrorViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)//partiali userUpdateDto modeli ile göndermek için

                });
                return Json(userUpdateModelStateErrorViewModal);

            }
        }

        //kullanıcı bilgilerinin güncellenmesi ayarlanması
        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ChangeDetails()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);//şu anki kullanıcının bilgileri
            var updateDto = _mapper.Map<UserUpdateDto>(user);//useri userupdatedtoya ceviriyoruz
            return View(updateDto);

        }

        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    if (oldUserPicture!="defaultUser.png")
                    {
                    isNewPictureUploaded = true;

                    }

                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded == true)//yeni bir resim eklendi mi
                    {
                        ImageDelete(oldUserPicture);
                    }
                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto);
                }


            }
            else
            {
                return View(userUpdateDto);

            }

        }
        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);//kullanıcının yakın zamanda önemli bir bilgi güncellediğini bilmek ister.
                        await _signInManager.SignOutAsync();//kullanıcıya çıkış yaptırılır.
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                        TempData.Add("SuccessMessage", $"Şifreniz başarıyla güncellenmiştir.");
                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen Girmiş olduğunuz şifrenizi kontrol ediniz.");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
            }
            
        }



        [HttpGet]//Yetkisiz istek
        public ViewResult AccessDenied()
        {
            return View();
        }


        //resim upload metodu
        [Authorize(Roles = "Admin,Editor")]
        public async Task<string> ImageUpload(string userName,IFormFile pictureFile)
        {
            string wwwroot = _env.WebRootPath;//~/img/user.Picture
            //string fileName = Path.GetFileNameWithoutExtension(userAddDto.pictureFile.FileName);//dosya adını uzantısız aldırır.
            string fileExtension = Path.GetExtension(pictureFile.FileName);//uzantıyı alır(.jpeg,png)
            DateTime dateTime = DateTime.Now;
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderScore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/img", fileName);//filenameyi img ye ekledik
            await using (var stream = new FileStream(path, FileMode.Create)) {
                await pictureFile.CopyToAsync(stream);
            }
            return fileName;
        }

        //delete method
        [Authorize(Roles = "Admin,Editor")]
        public bool ImageDelete(string pictureName) 
        {
            string wwwroot = _env.WebRootPath;
            var fileToDelete = Path.Combine($"{wwwroot}/img", pictureName);
            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
