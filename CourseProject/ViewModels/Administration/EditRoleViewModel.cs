namespace CourseProject.ViewModels.Administration
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            this.Users = new List<string>();
        }
        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
