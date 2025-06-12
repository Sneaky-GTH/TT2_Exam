using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TT2_Exam.Data;
using TT2_Exam.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace TT2_Exam.Controllers;

[Authorize]
public class LibraryController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<UserModel> _userManager;

    public LibraryController(AppDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var libraryItems = await _context.UserLibrary
            .Include(c => c.VideoGame)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return View(libraryItems);
    }
    
}