using FudiumBlog.Shared.Data.Abstract;
using FudiumBlog.Shared.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Shared.Data.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()//IEntityRepository implement edildi ve içindeki metodlar buraya geldi
    {
        protected readonly DbContext _context;
        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
           await _context.Set<TEntity>().AddAsync(entity);//TEntitye abone oluyoruz TEntity ve gelen entity comment,user veya article olabilir.
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate=null)
        {
            return await (predicate==null ? _context.Set<TEntity>().CountAsync(): _context.Set<TEntity>().CountAsync(predicate));
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); }); //async olmadığı için task işlemini kendimiz oluşturduk.
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();//burdaki işlemleri include propertyde de kullancağımız için oluşturduk
            if (predicate!=null)//filtre null değilse
            {
                query = query.Where(predicate);

            }
            if (includeProperties.Any())//bu dizinin içinde herhangi bir değer var mı rgrt varsa querye includelari ekliyoruz
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

            }
            return await query.ToListAsync();//bize gelen değerlere göre  query oluşturup kullanıcıya liste olarak dönüyoruz. 
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();//burdaki işlemleri include propertyde de kullancağımız için oluşturduk
                query = query.Where(predicate);

            if (includeProperties.Any())//bu dizinin içinde herhangi bir değer var mı rgrt varsa querye includelari ekliyoruz
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

            }
            return await query.SingleOrDefaultAsync(); //tek nesne dönüyoruz
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Run(() => { _context.Set<TEntity>().Update(entity);});
            return entity;
        }
    }
}
