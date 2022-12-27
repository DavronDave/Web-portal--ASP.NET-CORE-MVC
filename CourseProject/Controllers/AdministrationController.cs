using CourseProject.ViewModels.Administration;
using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserApplication> _userManager;
        private readonly UserDbContext _dbContext;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<UserApplication> userManager, UserDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = viewModel.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var userRole = _roleManager.FindByIdAsync(id).Result;
            var model = new EditRoleViewModel
            {
                Id = id,
                RoleName = userRole.Name
            };

            foreach (var user in _userManager.Users.OrderBy(x => x.UserName))
            {
                if (await _userManager.IsInRoleAsync(user, userRole.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var userRole = await _roleManager.FindByIdAsync(model.Id);

            if (userRole == null)
            {
                ViewBag.ErrorMessage = "Role not found";
            }
            else
            {
                userRole.Name = model.RoleName;
                var role = await _roleManager.UpdateAsync(userRole);
                if (role.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (var error in role.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return RedirectToAction();
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            ListRolesViewModel model = new ListRolesViewModel()
            {
                IdentityRoles = _roleManager.Roles,
                Names = _userManager.Users.Select(x => x.UserName)
            };
            // var roles = _roleManager.Roles;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = "Not found";
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.OrderBy(x => x.UserName))
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsActive = user.IsActive
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> models, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = "Role cannot be found";
            }

            for (int i = 0; i < models.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(models[i].UserId);

                IdentityResult result = null;

                if (models[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!models[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (models.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string roleId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {

            }
            else
            {
                var userId = _dbContext.ReviewCategories.Where(x => x.UserId == id).FirstOrDefault();
                if (userId != null)
                {
                    _dbContext.ReviewCategories.Remove(userId);
                }
                var result = await _userManager.DeleteAsync(user);
                _dbContext.SaveChanges();
                if (result.Succeeded)
                {
                    return RedirectToAction("EditUsersInRole","Administration", new {roleId = roleId});
                }

            }
            return RedirectToAction("EditUsersInRole");
        }

        [HttpPost]
        public async Task<IActionResult> Block(string id, string roleId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("EditUsersInRole", "Administration", new { roleId = roleId });
        }

        [HttpPost]
        public async Task<IActionResult> UnBlock(string id, string roleId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("EditUsersInRole", "Administration", new { roleId = roleId });
        }

    }
}
