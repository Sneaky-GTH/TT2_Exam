using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;
using TT2_Exam.Utility;

namespace TT2_Exam.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMarkdownFormatter _markdownFormatter;

        public StoreController(AppDbContext context, IMarkdownFormatter markdownFormatter)
        {
            _context = context;
            _markdownFormatter = markdownFormatter;
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
                .Include(v => v.GameSpecificCategories)
                .ThenInclude(gc => gc.Category)
                .FirstOrDefaultAsync(v => v.Id == id);
            
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
