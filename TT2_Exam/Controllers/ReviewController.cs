using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;
using TT2_Exam.Utility;

namespace TT2_Exam.Controllers;

[Authorize]
public class ReviewController(AppDbContext context, UserManager<UserModel> userManager) : Controller
{
    private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();

    // GET: Review/Create/5
    public async Task<IActionResult> Create(int gameId)
    {
        var game = await context.VideoGames.FindAsync(gameId);
        if (game == null) return NotFound();

        var model = new ReviewCreateViewModel
        {
            VideoGameId = gameId,
        };
        return View(model);
    }

    // POST: Review/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewCreateViewModel model)
    {
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }
    
        Console.WriteLine(model.VideoGameId);
        
        var ownsGame = await context.UserLibrary
            .AnyAsync(ul => ul.UserId == user.Id && ul.VideoGameId == model.VideoGameId);

        if (!ownsGame)
        {
            ModelState.AddModelError("", "You must own this game to leave a review.");
            return View(model);
        }

        // Prevent multiple reviews
        bool alreadyReviewed = await context.Reviews
            .AnyAsync(r => r.UserId == user.Id && r.VideoGameId == model.VideoGameId);

        if (alreadyReviewed)
        {
            ModelState.AddModelError("", "You have already reviewed this game.");
            return View(model);
        }

        var review = new ReviewModel
        {
            UserId = user.Id,
            VideoGameId = model.VideoGameId,
            Rating = model.Rating,
            Comment = Sanitizer.Sanitize(model.Comment),
            CreatedAt = DateTime.UtcNow
        };

        context.Reviews.Add(review);
        await context.SaveChangesAsync();

        return RedirectToAction("Details", "Store", new { id = model.VideoGameId });
    }
    
    // GET: Review/Edit/5
    public async Task<IActionResult> Edit(int gameId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.VideoGameId == gameId);

        if (review == null) return NotFound();

        var model = new ReviewCreateViewModel
        {
            VideoGameId = review.VideoGameId,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return View(model);
    }

    // POST: Review/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ReviewCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.VideoGameId == model.VideoGameId);

        if (review == null) return NotFound();

        review.Comment = model.Comment;
        review.Rating = model.Rating;
        review.CreatedAt = DateTime.UtcNow;

        context.Reviews.Update(review);
        await context.SaveChangesAsync();

        return RedirectToAction("Details", "Store", new { id = model.VideoGameId });
    }


    // GET: Review/Delete/5
    public async Task<IActionResult> Delete(int gameId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var review = await context.Reviews
            .Include(r => r.VideoGame)
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.VideoGameId == gameId);

        if (review == null) return NotFound();

        return View(review);
    }


    // POST: Review/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int gameId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.VideoGameId == gameId);

        if (review == null) return NotFound();

        context.Reviews.Remove(review);
        await context.SaveChangesAsync();

        return RedirectToAction("Details", "Store", new { id = gameId });
    }
    
    // POST: Review/AdminDelete/5
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
    public async Task<IActionResult> AdminDelete(int gameId, string userId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        
        if (!User.IsInRole("Admin")) return NotFound();

        var review = await context.Reviews
            .Include(r => r.VideoGame)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VideoGameId == gameId);

        if (review == null) return NotFound();

        return View(review);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
    public async Task<IActionResult> AdminDeleteConfirmed(int gameId, string userId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        
        if (!User.IsInRole("Admin")) return NotFound();

        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.VideoGameId == gameId);

        if (review == null) return NotFound();

        context.Reviews.Remove(review);
        await context.SaveChangesAsync();

        return RedirectToAction("Details", "Store", new { id = gameId });
    }
    

}
