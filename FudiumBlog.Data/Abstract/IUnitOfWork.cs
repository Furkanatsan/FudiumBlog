using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Data.Abstract
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        //UnitOfWork sayesinde tüm repositoryler bir yerden yönetilir.repoların interfaceleri verilir.Ef ye bağımlığımız yok.

        IArticleRepository Articles { get; }
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }
       

        Task<int> SaveAsync();
    }
}
