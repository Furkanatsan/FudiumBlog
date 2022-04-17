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
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;//iç içe objelerde hata almamamýzý saðlar.
            });//mvc uygulamasý gibi çalýþ.

            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile));//Derlenme esnasýnda automapperin burdaki sýnýflarý taramasýný saðlar.ýmapper,profile buluyor ve ekliyor.
            services.LoadMyServices(connectionString:Configuration.GetConnectionString("localDB"));//servicesi kullanabilmek için
            services.ConfigureApplicationCookie(options => {

                options.LoginPath = new PathString("/Admin/User/Login");//kullanýcý giriþi yapmadan admin areaya eriþmek istersen bu yola yönlendirilir.
                options.LogoutPath = new PathString("/Admin/User/Logout");
                options.Cookie = new CookieBuilder { 
                Name="FudiumBlog",
                HttpOnly=true,// cookie  iþlemlerini sadece http üzerinden gönderilmesini saðlar.güvenlik.
                SameSite=SameSiteMode.Strict,//cookie bilgilerinin nereden geldiðini kontrol eder.güvenlik. 
                SecurePolicy=CookieSecurePolicy.SameAsRequest//güvenlik always olarak býrakýlmalý 
                };

                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);//oturumun açýk kalma süresi yarý yarýya uzar
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");//giriþ yapmýþ  kullanýcý yetkisi olmayan bir yere giriþ yapmaya çalýþýrsa gönderileceði yol

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
