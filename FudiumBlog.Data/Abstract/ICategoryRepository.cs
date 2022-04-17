using FudiumBlog.Entities.Concrete;
using FudiumBlog.Shared.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Data.Abstract
{
    public interface ICategoryRepository:IEntityRepository<Category>
    {
    }
}
