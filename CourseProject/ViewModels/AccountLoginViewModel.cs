using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        // [EmailAddress]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
