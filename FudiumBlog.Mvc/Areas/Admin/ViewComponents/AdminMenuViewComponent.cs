using FudiumBlog.Entities.Concrete;
using FudiumBlog.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FudiumBlog.Mvc.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent:ViewComponent
    {
        private readonly UserManager<User> _userManager;
        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public ViewViewComponentResult Invoke()
        {
            //veritabanıyla iletişim kurabilir
            //viewi return etmeden önce  model kullanma izni verir.
            var user = _userManager.GetUserAsync(HttpContext.User).Result;//hangi kullanıcının girdiği bilgisi Httpcontext.user
            var roles = _userManager.GetRolesAsync(user).Result;//rolleri aldık
            return View(new UserWithRolesViewModel
            {
                User=user,
                Roles=roles
            });
        }
    }
}
