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
public class CartController(AppDbContext context, UserManager<UserModel> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = userManager.GetUserId(User);

        var cartItems = await context.CartItems
            .Include(c => c.VideoGame)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int gameId)
    {
        var userId = userManager.GetUserId(User);

        var alreadyExists = await context.CartItems
            .AnyAsync(c => c.UserId == userId && c.VideoGameId == gameId);

        if (alreadyExists) return RedirectToAction("Index", "Store");
        if (userId != null)
            context.CartItems.Add(new CartItemModel
            {
                UserId = userId,
                VideoGameId = gameId
            });

        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Store");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var item = await context.CartItems.FindAsync(id);
        if (item != null)
        {
            context.CartItems.Remove(item);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var userId = userManager.GetUserId(User);

        var cartItems = await context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();

        foreach (var item in cartItems)
        {
            // Check if already owned
            bool alreadyOwned = await context.UserLibrary
                .AnyAsync(l => l.UserId == userId && l.VideoGameId == item.VideoGameId);

            if (alreadyOwned) continue;
            if (userId != null)
                context.UserLibrary.Add(new UserLibraryItemModel
                {
                    UserId = userId,
                    VideoGameId = item.VideoGameId
                });
        }

        context.CartItems.RemoveRange(cartItems);
        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Library");
    }
}
