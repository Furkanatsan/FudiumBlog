using FudiumBlog.Mvc.AutoMapper.Profiles;
using FudiumBlog.Services.AutoMapper.Profiles;
using FudiumBlog.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FudiumBlog.Mvc
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> 
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;//i� i�e objelerde hata almamam�z� sa�lar.
            });//mvc uygulamas� gibi �al��.

            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile));//Derlenme esnas�nda automapperin burdaki s�n�flar� taramas�n� sa�lar.�mapper,profile buluyor ve ekliyor.
            services.LoadMyServices(connectionString:Configuration.GetConnectionString("localDB"));//servicesi kullanabilmek i�in
            services.ConfigureApplicationCookie(options => {

                options.LoginPath = new PathString("/Admin/User/Login");//kullan�c� giri�i yapmadan admin areaya eri�mek istersen bu yola y�nlendirilir.
                options.LogoutPath = new PathString("/Admin/User/Logout");
                options.Cookie = new CookieBuilder { 
                Name="FudiumBlog",
                HttpOnly=true,// cookie  i�lemlerini sadece http �zerinden g�nderilmesini sa�lar.g�venlik.
                SameSite=SameSiteMode.Strict,//cookie bilgilerinin nereden geldi�ini kontrol eder.g�venlik. 
                SecurePolicy=CookieSecurePolicy.SameAsRequest//g�venlik always olarak b�rak�lmal� 
                };

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);//oturumun a��k kalma s�resi yar� yar�ya uzar
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");//giri� yapm��  kullan�c� yetkisi olmayan bir yere giri� yapmaya �al���rsa g�nderilece�i yol

            });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();//resimler,css,js

            app.UseRouting();//yol

            app.UseAuthentication();//kimlik

            app.UseAuthorization();//yetki

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
