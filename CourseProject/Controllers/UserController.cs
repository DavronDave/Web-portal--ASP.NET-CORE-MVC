using CourseProject.ViewModels;
using CourseProject.ViewModels.User;
using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly UserDbContext _dbContext;

        public UserController(UserManager<UserApplication> userManager, UserDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users.OrderBy(x => x.UserName);
            return View(users);
        }
        
        [HttpGet]
        public IActionResult Categories()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.id= userId.ToString();
            CategoriesViewModel categories = new CategoriesViewModel()
            {
                ReviewCategories = _dbContext.ReviewCategories.Where(x => x.UserId == userId)
            };

            return View(categories);
        }

        [HttpPost]
        public IActionResult Categories(ReviewCategory category)
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

            return RedirectToAction();
        }

        [HttpGet]
        public IActionResult UserCategories(string id)
        {
            var categories = _dbContext.ReviewCategories.Where(x => x.UserId == id);
            ViewBag.id = id;
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateReview(int id, string userId)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ReviewViewModel model = new ReviewViewModel()
            {
                Reviews = _dbContext.Reviews.Where(x => x.ReviewCategoryId==id),
                IsEqual= user==userId ? true : false
            };
            return View(model);
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
                };
                _dbContext.Reviews.Add(review1);
                _dbContext.SaveChanges();
            }

            return RedirectToAction();
        }

        [HttpGet]
        public IActionResult ReviewDetails(int id)
        {
            var review = _dbContext.Reviews.Where(x => x.Id == id).FirstOrDefault();
            return View(review);
        }

        [HttpGet]
        public IActionResult EditReview(int id)
        {
            var review = _dbContext.Reviews.Where(x => x.Id == id).FirstOrDefault();
            return View(review);
        }
        [HttpPost]
        public async Task<IActionResult> EditReview(Review review)
        {
            var deletedReview = await _dbContext.Reviews.Where(x => x.Id == review.Id).FirstOrDefaultAsync();
           
            if(deletedReview != null)
            {
                var review1 = new Review()
                {
                    Name = review.Name,
                    ReviewCategoryId = deletedReview.ReviewCategoryId
                };
                _dbContext.Reviews.Remove(deletedReview);
                _dbContext.Reviews.Add(review1);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("CreateReview", new { id = review1.ReviewCategoryId });
            }

            return RedirectToAction();
        }

        [HttpPost]
        public IActionResult DeleteReview(int id)
        {
            var deletedReview = _dbContext.Reviews.Where(x => x.Id==id).FirstOrDefault();

           if( deletedReview != null)
            {
                _dbContext.Reviews.Remove(deletedReview);
                _dbContext.SaveChanges();

                return RedirectToAction("CreateReview", new { id = deletedReview.ReviewCategoryId });
            }

            return RedirectToAction();
        }
    }
}
