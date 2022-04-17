using FudiumBlog.Data.Abstract;
using FudiumBlog.Entities.Concrete;
using FudiumBlog.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Data.Concrete
{
    public class EfArticleRepository:EfEntityRepositoryBase<Article>,IArticleRepository
    {
        public EfArticleRepository(DbContext context):base(context)
        {

        }
        
    }
}
