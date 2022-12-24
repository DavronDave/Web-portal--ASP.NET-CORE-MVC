using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models
{
    public class User : IdentityUser
    {
        public ICollection<ReviewCategory>? ReviewCategories { get; set; }
        //public bool IsActive { get; set; }
    }
}
