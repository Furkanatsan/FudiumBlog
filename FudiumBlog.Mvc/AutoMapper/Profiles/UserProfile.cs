using AutoMapper;
using FudiumBlog.Entities.Concrete;
using FudiumBlog.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FudiumBlog.Mvc.AutoMapper.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();//useradddto yu user sınıfına map ediyoruz.
            CreateMap<User,UserUpdateDto>();//update user
            CreateMap<UserUpdateDto, User>();//updatedto verip user alıyoruz.
        }
    }
}
