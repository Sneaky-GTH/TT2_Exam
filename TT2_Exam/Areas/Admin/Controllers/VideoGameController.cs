using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;
using TT2_Exam.Models;
using TT2_Exam.Utility;

namespace TT2_Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoGameController : Controller
    {
        private readonly AppDbContext _context;

        public VideoGameController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/VideoGame
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoGames.ToListAsync());
        }

        // GET: Admin/VideoGame/Details/5
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Details(int? id)
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

        // GET: Admin/VideoGame/Create
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/VideoGame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ReleaseDate,Price")] VideoGameModel videoGameModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoGameModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoGameModel);
        }

        // GET: Admin/VideoGame/Edit/5
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoGameModel = await _context.VideoGames.FindAsync(id);
            if (videoGameModel == null)
            {
                return NotFound();
            }
            return View(videoGameModel);
        }

        // POST: Admin/VideoGame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ReleaseDate,Price")] VideoGameModel videoGameModel)
        {
            if (id != videoGameModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGameModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGameModelExists(videoGameModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(videoGameModel);
        }

        // GET: Admin/VideoGame/Delete/5
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/VideoGame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.RequireAdmin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoGameModel = await _context.VideoGames.FindAsync(id);
            if (videoGameModel != null)
            {
                _context.VideoGames.Remove(videoGameModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGameModelExists(int id)
        {
            return _context.VideoGames.Any(e => e.Id == id);
        }
        
        
    }
}
