using FudiumBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Entities.Concrete
{
    public class Comment: EntityBase, IEntity
    {
        public string Text { get; set; }//yorum yazısı
        public int ArticleId { get; set; }//ait oldugu makaleId
        public Article Article { get; set; }//navigation prop
    }
}
