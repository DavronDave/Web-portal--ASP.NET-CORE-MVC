using System.ComponentModel.DataAnnotations;

namespace CourseProject.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage ="Password and Confirm password do not match")]
        public string? ConfirmPassword { get; set; }

    }
}
