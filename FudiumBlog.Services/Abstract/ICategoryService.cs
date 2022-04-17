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
        Task<IDataResult<CategoryDto>> GetAsync(int categoryId);
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAllAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync();//silinmemişleri getirir.
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync();//silinmemişleri getirir.
        Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName);//veri eklediğimizde veya güncellediğimizde geriye kateggoriDto dönmüş olacagız.bu sayede eklediğimiz yada güncellediğimiz verinin son hali elimizde olucak ve bunuda tablomuza ve toastera eklemek için kullanıyor olacagız.
        Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
        Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName);//isDeleted i true yapar
        Task<IResult> HardDeleteAsync(int categoryId);//gerçekten siler.
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
