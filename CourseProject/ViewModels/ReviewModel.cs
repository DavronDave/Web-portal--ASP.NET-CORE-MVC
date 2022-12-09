using DataLayer.Models;

namespace CourseProject.ViewModels
{
    public class ReviewModel
    {
        public string Name { get; set; }
        public int ReviewCategoryId { get; set; }
        public ReviewCategory Category { get; set; }
    }
}
