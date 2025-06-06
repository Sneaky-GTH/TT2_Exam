using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;

namespace TT2_Exam.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }
        // GET: StoreController
        public IActionResult Index(string searchQuery, List<int> selectedCategoryIds, string sortBy)
        {
            var gamesQuery = _context.VideoGames
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
                SelectedCategoryIds = selectedCategoryIds,
                AvailableCategories = _context.Categories.ToList()
            };

            return View(model);
        }
        
        public async Task<IActionResult>  Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoGameModel = await _context.VideoGames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGameModel == null)
            {
                return NotFound();
            }

            return View(videoGameModel);
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
                query = query.Where(g =>
                    g.GameSpecificCategories.Any(vc => categoryIds.Contains(vc.CategoryId)));
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
