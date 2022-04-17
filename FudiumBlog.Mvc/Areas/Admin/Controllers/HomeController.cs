using FudiumBlog.Entities.Concrete;
using FudiumBlog.Mvc.Areas.Admin.Models;
using FudiumBlog.Services.Abstract;
using FudiumBlog.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FudiumBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(ICategoryService categoryService, IArticleService articleService, ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }
       
        public async Task<IActionResult> Index()
        {
            var categoryCountResult = await _categoryService.CountByNonDeletedAsync();
            var articlesCountResult = await _articleService.CountByNonDeletedAsync();
            var commentsCountResult = await _commentService.CountByNonDeletedAsync();
            var usersCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();
            if (categoryCountResult.ResultStatus==ResultStatus.Success && articlesCountResult.ResultStatus==ResultStatus.Success && commentsCountResult.ResultStatus==ResultStatus.Success && usersCount>-1 && articlesResult.ResultStatus==ResultStatus.Success)
            {
                return View(new DashboardViewModel {
                    CategoriesCount=categoryCountResult.Data,
                    ArticlesCount=articlesCountResult.Data,
                    CommentsCount=commentsCountResult.Data,
                    UsersCount=usersCount,
                    Articles=articlesResult.Data
                
                });

            }
            return NotFound();
        }
    }
}
