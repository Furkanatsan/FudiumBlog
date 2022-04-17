using FudiumBlog.Entities.Concrete;
using FudiumBlog.Entities.Dtos;
using FudiumBlog.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDto>> Get(int categoryId);
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAll();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeleted();//silinmemişleri getirir.
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive();//silinmemişleri getirir.
        Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName);//veri eklediğimizde veya güncellediğimizde geriye kateggoriDto dönmüş olacagız.bu sayede eklediğimiz yada güncellediğimiz verinin son hali elimizde olucak ve bunuda tablomuza ve toastera eklemek için kullanıyor olacagız.
        Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
        Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedByName);//isDeleted i true yapar
        Task<IResult> HardDelete(int categoryId);//gerçekten siler.
    }
}
