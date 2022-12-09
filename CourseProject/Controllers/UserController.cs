using CourseProject.ViewModels;
using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly UserDbContext _dbContext;

        public UserController(UserManager<User> userManager, UserDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users;
            return View(users);
        }
        [HttpGet]
        public IActionResult Categories()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var categories = _dbContext.ReviewCategories.Where(x => x.UserId == userId);
            return View(categories);
        }

        [HttpGet]
        public IActionResult UserCategories(string id)
        {
            var categories = _dbContext.ReviewCategories.Where(x => x.UserId == id);
            return View(categories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateCategory(ReviewCategory category)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var cat = new ReviewCategory()
                {
                    Name = category.Name,
                    UserId = userId
                };

                _dbContext.ReviewCategories.Add(cat);
                _dbContext.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateReview()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateReview(Review review)
        {
            if (ModelState.IsValid)
            {
                var review1 = new Review()
                {
                    Name = review.Name,
                    ReviewCategoryId = review.Id
                    //Category=review.Category
                };
                _dbContext.Reviews.Add(review1);
                _dbContext.SaveChanges();
            }

            return View(review);
        }
    }
}
