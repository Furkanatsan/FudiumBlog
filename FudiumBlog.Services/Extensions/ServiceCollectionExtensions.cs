using FudiumBlog.Data.Abstract;
using FudiumBlog.Data.Concrete;
using FudiumBlog.Data.Concrete.EntityFramework.Contexts;
using FudiumBlog.Entities.Concrete;
using FudiumBlog.Services.Abstract;
using FudiumBlog.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Services.Extensions
{
    //mvc katmanı direkt olarak data katmanına erişmesin diye böyle bir extension sınıf yarattık.services katmanını mvc katmanı referans etti.services katmanı bir köprü görevi gördü.
    //yani services katmanı bilgileri data katmanından alır kendi içinde işler vemvc katmanına taşır.
    public static class ServiceCollectionExtensions//static olmalı
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<FudiumBlogContext>();//db contextimizi kaydettik
            serviceCollection.AddIdentity<User,Role>(options=> 
            {
                //password options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;//kaç özel karakter olmalı
                options.Password.RequireNonAlphanumeric = false;//özel karakterlerin eklenip eklenmeyeceği
                options.Password.RequireLowercase = false;//küçük harf zorunluluğu
                options.Password.RequireUppercase=false;//büyük harf bulunması
                //username and email options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";//kullanıcı adı oluştuturken izin verilen özel karakterler
                options.User.RequireUniqueEmail =true;//unique email


            }).AddEntityFrameworkStores<FudiumBlogContext>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();//eger biri senden ıunitofwork isterse unitofwork ver.Yapılan her requestte nesne tekrar oluşur.
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();//eger biri senden ıcategoryservice  isterse categorymanager ver.
            serviceCollection.AddScoped<IArticleService, ArticleManager>();//eger biri senden ıArticleservice isterse artivlemanager ver.
            return serviceCollection;
        } 
    }
}
