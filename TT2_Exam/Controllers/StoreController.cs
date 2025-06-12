using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;
using TT2_Exam.Utility;

namespace TT2_Exam.Controllers
{
    public class StoreController(
        AppDbContext context,
        IMarkdownFormatter markdownFormatter,
        UserManager<UserModel> userManager)
        : Controller
    {
        private readonly IMarkdownFormatter _markdownFormatter = markdownFormatter;

        // GET: StoreController
        [IgnoreAntiforgeryToken]
        public IActionResult Index(string searchQuery, List<int> selectedCategoryIds, string sortBy)
        {
            var gamesQuery = context.VideoGames
                .AsNoTracking()
                .Include(g => g.GameSpecificCategories)
                .ThenInclude(vc => vc.Category)
                .AsQueryable();

            gamesQuery = ApplySearchFilter(gamesQuery, searchQuery);
            gamesQuery = ApplyCategoryFilter(gamesQuery, selectedCategoryIds);
            gamesQuery = ApplySorting(gamesQuery, sortBy);

            var model = new StoreViewModel
            {
                Games = gamesQuery.ToList(),
                SearchQuery = searchQuery,
                SortBy = sortBy,
                SelectedCategoryIds = selectedCategoryIds ?? [],
                AvailableCategories = context.Categories.ToList()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_GamesPartial", model);
            }

            return View(model);
        }


        
        public async Task<IActionResult>  Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await context.VideoGames
                .Include(v => v.GameSpecificCategories)
                    .ThenInclude(gc => gc.Category)
                .Include(v => v.Publisher)
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (game == null)
            {
                return NotFound();
            }
            
            var userId = userManager.GetUserId(User);
            var ownsGame = await context.UserLibrary
                .AnyAsync(l => l.UserId == userId && l.VideoGameId == id);
            
            var reviews = await context.Reviews
                .Include(r => r.User)
                .Where(r => r.VideoGameId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var userReview = await context.Reviews
                .Include(r => r.User)
                .Where(r => r.VideoGameId == id)
                .FirstOrDefaultAsync(r => r.UserId == userId);

            var videoGameDetails = new StoreVideoGameDetailsViewModel()
            {
                VideoGame = game,
                Reviews = reviews,
                UserOwnsGame = ownsGame && User.Identity is { IsAuthenticated: true },
                UserReview = userReview,
            };

            return View(videoGameDetails);
        }
        
        // Filter functions
        
        private static IQueryable<VideoGameModel> ApplySearchFilter(IQueryable<VideoGameModel> query, string searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(g => g.Title.Contains(searchQuery));
            }
            return query;
        }

        private static IQueryable<VideoGameModel> ApplyCategoryFilter(IQueryable<VideoGameModel> query, List<int> categoryIds)
        {
            if (categoryIds is { Count: > 0 })
            {
                query = query
                    .Where(g =>
                        g.GameSpecificCategories
                            .Count(gc => categoryIds.Contains(gc.CategoryId)) == categoryIds.Count);
            }
            return query;
        }

        private static IQueryable<VideoGameModel> ApplySorting(IQueryable<VideoGameModel> query, string sortBy)
        {
            return sortBy switch
            {
                "PriceAsc" => query.OrderBy(g => g.Price),
                "PriceDesc" => query.OrderByDescending(g => g.Price),
                "Title" => query.OrderBy(g => g.Title),
                _ => query
            };
        }

    }
    
}
