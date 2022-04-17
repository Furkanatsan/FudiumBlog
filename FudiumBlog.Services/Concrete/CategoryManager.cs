using AutoMapper;
using FudiumBlog.Data.Abstract;
using FudiumBlog.Entities.Concrete;
using FudiumBlog.Entities.Dtos;
using FudiumBlog.Services.Abstract;
using FudiumBlog.Shared.Utilities.Results.Abstract;
using FudiumBlog.Shared.Utilities.Results.ComplexTypes;
using FudiumBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)//yeni kategori ekleme
        {

            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory=await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir.",new CategoryDto
            { 
                Category=addedCategory,
                ResultStatus=ResultStatus.Success,
                Message= $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir."
            });
            //await _unitOfWork.Categories.AddAsync(new Category
            //{
            //    Name = categoryAddDto.Name,
            //    Description=categoryAddDto.Description,
            //    Note=categoryAddDto.Note,
            //    IsActive=categoryAddDto.IsActive,
            //    CreatedByName=createdByName,
            //    CreatedDate=DateTime.Now,
            //    ModifiedByName=createdByName,//ilk kez oluşturduğumuz için düzenleyici ismini veriyoruz
            //    ModifiedDate=DateTime.Now,
            //    IsDeleted=false
            //}).ContinueWith(t=>_unitOfWork.SaveAsync());//alttakı save işlemine göre daha hızlı,performanslı 
            ////await _unitOfWork.SaveAsync();

            //return new Result(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir.");

        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var categoriesCount = await _unitOfWork.Categories.CountAsync();
            if (categoriesCount>-1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()//silinmemiş olanların sayısı
        {
            var categoriesCount = await _unitOfWork.Categories.CountAsync(c=>!c.IsDeleted);
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category!=null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();//delete ve save işlemi
                return new DataResult<CategoryDto>(ResultStatus.Success, $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir.", new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir."
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, $"Böyle bir kategori bulunamamıştır.", new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = $"Böyle bir kategori bulunamamıştır."
            });
        }

        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);//id ye göre kategorileri getiricek

            if (category!=null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto { 
                Category=category,
                ResultStatus=ResultStatus.Success
                }); 
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı", new CategoryDto 
            {
                Category=null,
                ResultStatus=ResultStatus.Error,
                Message= "Böyle bir kategori bulunamadı"
            });;
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null);//tüm kategorileri getiricek
            if (categories.Count>-1)//0 kategori olabilir
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto 
                { 
                Categories=categories,
                ResultStatus=ResultStatus.Success

                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiç bir kategori bulunamadı.",new CategoryListDto { 
            Categories=null,
            ResultStatus=ResultStatus.Error,
            Message= "Hiç bir kategori bulunamadı."
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => c.IsDeleted == false);//silinmemiş olanları getiricek
            if (categories.Count > -1)//0 kategori olabilir
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success

                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiç bir kategori bulunamadı.", new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Hiç bir kategori bulunamadı."
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c =>!c.IsDeleted&&c.IsActive);//silinmemiş ve aktif olanları getiricek
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Hiç bir kategori bulunamadı.", null);

        }

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı.", null);
            }
        }

        public async Task<IResult> HardDeleteAsync(int categoryId)//silme işlemi
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{category.Name } adlı kategori başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)//update işlemi
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);//Dto Da olmayan değerleri de almış olacağız.
            category.ModifiedByName = modifiedByName;

            var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();//update ve save işlemi
            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.",new CategoryDto 
            {
                Category=updatedCategory,
                ResultStatus=ResultStatus.Success,
                Message= $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir."
            });
        
        //var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
        //if (category!=null)
        //{
        //    category.Name = categoryUpdateDto.Name;
        //    category.Description = categoryUpdateDto.Description;
        //    category.Note = categoryUpdateDto.Note;
        //    category.IsActive = categoryUpdateDto.IsActive;
        //    category.IsDeleted = categoryUpdateDto.IsDeleted;
        //    category.ModifiedByName = modifiedByName;
        //    category.ModifiedDate = DateTime.Now;
        //    await _unitOfWork.Categories.UpdateAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());//update ve save işlemi
        //    return new Result(ResultStatus.Success, $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.");
        //}
        //return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı.");
    }

     
    }
}
