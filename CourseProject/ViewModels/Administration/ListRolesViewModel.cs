using Microsoft.AspNetCore.Identity;
using DataLayer.Models;

namespace CourseProject.ViewModels.Administration
{
    public class ListRolesViewModel
    {
        public IEnumerable<IdentityRole>? IdentityRoles { get; set; }
        public IQueryable<string>? Names { get; set; }

    }
}
