using DataLayer.Models;

namespace CourseProject.ViewModels.User
{
    public class CategoriesViewModel
    {
        public IQueryable<ReviewCategory> ReviewCategories { get; set; }
        public string Name { get; set; }
    }
}
