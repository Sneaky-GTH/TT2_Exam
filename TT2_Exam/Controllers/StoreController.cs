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
        public IActionResult Index(string searchQuery, List<string> selectedCategories, string sortBy)
        {
            var allGames = _context.VideoGames.AsQueryable();

            // Filter by search
            if (!string.IsNullOrEmpty(searchQuery))
                allGames = allGames.Where(g => g.Title.Contains(searchQuery));

            // Filter by categories

            // Sorting
            allGames = sortBy switch
            {
                "PriceAsc" => allGames.OrderBy(g => g.Price),
                "PriceDesc" => allGames.OrderByDescending(g => g.Price),
                "Title" => allGames.OrderBy(g => g.Title),
                _ => allGames
            };

            var model = new StoreViewModel
            {
                Games = allGames.ToList(),
                SearchQuery = searchQuery,
                SortBy = sortBy
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

    }
}
