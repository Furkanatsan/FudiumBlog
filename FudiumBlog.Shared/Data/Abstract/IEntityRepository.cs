using FudiumBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Shared.Data.Abstract
{//burada tüm entityler için dal classlarında kullanıcagımız metodları oluşturduk.
    public interface IEntityRepository<T>where T:class,IEntity,new()//bu generic kısma sadece veri tabanı nesmelerimizin gelebileceğini belirttik. //ortak metodlar burada bulunur //Generic //aslında data katmanına ekleyebilirdik ama başka yerlerde de kullabiliriz diye shared içine ekledik.<T>comment verirsek comment gelicek user verirsek user gelicek.predicate=filtreleyici linkq sorgu
    {
        Task<T> GetAsync(Expression<Func<T,bool>> predicate,params Expression<Func<T,object>>[] includeProperties);//tekli sorgu
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate=null, params Expression<Func<T, object>>[] includeProperties);  //çoklu sorgu listeye ihtiyacımız olduğu için 
        Task<T> AddAsync(T entity);//bir kategori eklemiş isek geriye bir kategori dönücek.
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);//örn:Böyle bir kullanıcı var mı kontrolu
        Task<int> CountAsync(Expression<Func<T, bool>> predicate=null);//total kaç makale ver kaç yotum var vs

    }
}
