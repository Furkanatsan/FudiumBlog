using FudiumBlog.Data.Abstract;
using FudiumBlog.Data.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FudiumBlogContext _context;
        private EfArticleRepository _articleRepository;//interfacelerin(concrete) somut hallerini ekledik
        private EfCategoryRepository _categoryRepository;
        private EfCommentRepository _commentRepository;
   


        public UnitOfWork(FudiumBlogContext context)
        {
            _context = context;
        }
        //eger elimizde bir _articleRepisitory varsa dönüyoruz yoksa yeni bir articlerepository newleyerek kullanıcıya döndürüyoruz.
        public IArticleRepository Articles => _articleRepository ?? new EfArticleRepository(_context);

        public ICategoryRepository Categories => _categoryRepository ?? new EfCategoryRepository(_context);

        public ICommentRepository Comments => _commentRepository ?? new EfCommentRepository(_context);


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync(); 
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
