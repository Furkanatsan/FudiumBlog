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
        Task<IDataResult<ArticleDto>> Get(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();//tümünü getir
        Task<IDataResult<ArticleListDto>> GetAllByNonDeleted();//silinmemişleri getirir.
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive();//silinmemiş ve aktif makaleleri göstermek için//anasayfada
        Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);//kategoriye göre makaleleri getir.
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> Delete(int articleId, string modifiedByName);//isDeleted i true yapar
        Task<IResult> HardDelete(int articleId);//gerçekten siler.

    }
}
