using FudiumBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Entities.Concrete
{
    public class Category:EntityBase,IEntity //sen entityBaseden türüyorsun ve I entityi implemente ediyorsun.
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
