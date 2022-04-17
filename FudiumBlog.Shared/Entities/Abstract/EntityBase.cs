using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Shared.Entities.Abstract
{
    public abstract class EntityBase//başka sınıflarda override edilebilmesi için ****En Temel propertyler burada her sınıfta olabilecek proplar****
    {
        public virtual int Id { get; set; }//değiştirme ihtiyacımız olabilir
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;//başka sınıflarda override edilebilmesi için
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;//Düzenlenme tarihi
        public virtual bool IsDeleted { get; set; } = false;//entitiler için silindimi kontrolunu yapmak için kullandık
        public virtual bool IsActive { get; set; } = true;//entitiler için aktif mi kontrolü
        public virtual string CreatedByName { get; set; } = "Admin";//default name
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string Note { get; set; }//entitilerle ilgili admin panelinde not tutmak isteyebiliriz(yorum kullanıcı vs)

    }
}
