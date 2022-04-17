using FudiumBlog.Entities.Concrete;
using FudiumBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Entities.Dtos
{
    public class UserDto:DtoGetBase
    {
        public User User { get; set; }
    }
}
