using System.Security.Claims;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;
using TT2_Exam.Utility;
using System.Linq;

namespace TT2_Exam.Areas.Admin.Controllers;

[Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
[Area("Admin")]
public class UserController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    
    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        var model = new UserDetailsViewModel
        {
            User = user,
            CurrentRoles = roles,
            AllRoles = allRoles
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoles(string id, List<string> selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);

        var rolesToAdd = selectedRoles.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(selectedRoles);

        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        await _userManager.AddToRolesAsync(user, rolesToAdd);

        return RedirectToAction("Details", new { id });
    }
}
