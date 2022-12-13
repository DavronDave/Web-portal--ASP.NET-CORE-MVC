using DataLayer.Models;

namespace CourseProject.ViewModels.User
{
    public class ReviewViewModel
    {
        public IQueryable<Review> Reviews { get; set; }
        public string Name { get; set; }
    }
}
