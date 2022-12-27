using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models
{
    public class UserApplication : IdentityUser
    {
        public ICollection<ReviewCategory>? ReviewCategories { get; set; }
        public bool IsActive { get; set; }
    }
}
