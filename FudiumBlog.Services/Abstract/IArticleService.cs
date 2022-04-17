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
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();//tümünü getir
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();//silinmemişleri getirir.
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();//silinmemiş ve aktif makaleleri göstermek için//anasayfada
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);//kategoriye göre makaleleri getir.
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);//isDeleted i true yapar
        Task<IResult> HardDeleteAsync(int articleId);//gerçekten siler.
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();


    }
}
