﻿using FudiumBlog.Shared.Entities.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Entities.Concrete
{
    public class User:IdentityUser<int>
    {

        public string Picture { get; set; }
        
        public ICollection<Article> Articles { get; set; }//BİR KULLANICININ BİRDEN FAZLA MAKALESİ OLABİLİR.
    }
}
