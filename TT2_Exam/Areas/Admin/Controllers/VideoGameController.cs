using System.Security.Claims;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<UserModel>  _userManager;
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();


        public VideoGameController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/VideoGame
        [Authorize(Policy = AuthorizationPolicies.RequirePublisher)]
        public async Task<IActionResult> Index()
        {   
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Admin"))
            {
                var allGames = await _context.VideoGames
                    .Include(v => v.Publisher)
                    .ToListAsync();

                return View(allGames);
            }
            else
            {
                var publisherGames = await _context.VideoGames
                    .Include(v => v.Publisher)
                    .Where(v => v.PublisherId == userId)
                    .ToListAsync();

                return View(publisherGames);
            }
        }

        // GET: Admin/VideoGame/Details/5
        [Authorize(Policy = "IsPublisher")]
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
        [Authorize(Policy = AuthorizationPolicies.RequirePublisher)]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();

            var viewModel = new VideoGameCreateViewModel
            {
                Categories = categories.Select(c => new CategoryCheckboxViewModel
                    {
                        CategoryId = c.Id,
                        Name = c.Name,
                        IsSelected = false
                    })
                    .ToList(),
                VideoGame = null
            };

            return View(viewModel);
        }

        // POST: Admin/VideoGame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.RequirePublisher)]
        public async Task<IActionResult> Create(VideoGameCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload category data for redisplay
                var allCategories = await _context.Categories.ToListAsync();

                viewModel.Categories = allCategories
                    .Select(c => new CategoryCheckboxViewModel
                    {
                        CategoryId = c.Id,
                        Name = c.Name,
                        IsSelected = viewModel.Categories != null && viewModel.Categories.Any(vc => vc.CategoryId == c.Id && vc.IsSelected)
                    })
                    .ToList();
            }

            // Get current user (publisher)
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            try
            {
                if (viewModel.VideoGame != null)
                    viewModel.VideoGame.ThumbnailPath = await SaveThumbnailAsync(viewModel.ThumbnailUpload);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("ThumbnailUpload", ex.Message);
                return View(viewModel);
            }

            var videoGame = new VideoGameModel
            {
                Title = Sanitizer.Sanitize(viewModel.VideoGame.Title),
                Price = viewModel.VideoGame.Price,
                ShortDescription = Sanitizer.Sanitize(viewModel.VideoGame.ShortDescription),
                Description = Sanitizer.Sanitize(viewModel.VideoGame.Description),
                ThumbnailPath = Sanitizer.Sanitize(viewModel.VideoGame.ThumbnailPath),
                ReleaseDate = DateTime.Today,

                PublisherId = user.Id,
                Publisher = user,

                GameSpecificCategories = viewModel.Categories
                    .Where(c => c.IsSelected)
                    .Select(c => new GameSpecificCategoryModel
                    {
                        CategoryId = c.CategoryId
                    }).ToList()
            };

            _context.VideoGames.Add(videoGame);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Admin/VideoGame/Edit/5
        [Authorize(Policy = AuthorizationPolicies.IsRightfulPublisher)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var videoGame = await _context.VideoGames
                .Include(v => v.GameSpecificCategories)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (videoGame == null) return NotFound();

            var allCategories = await _context.Categories.ToListAsync();

            var viewModel = new VideoGameCreateViewModel
            {
                VideoGame = videoGame,
                Categories = allCategories.Select(c => new CategoryCheckboxViewModel
                {
                    CategoryId = c.Id,
                    Name = c.Name,
                    IsSelected = videoGame.GameSpecificCategories.Any(gc => gc.CategoryId == c.Id)
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Admin/VideoGame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationPolicies.IsRightfulPublisher)]
        public async Task<IActionResult> Edit(int id, VideoGameCreateViewModel viewModel)
        {
            if (id != viewModel.VideoGame.Id) return NotFound();

            var videoGame = await _context.VideoGames
                .Include(v => v.GameSpecificCategories)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (videoGame == null) return NotFound();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Log errors or inspect in debugger
                // For example, temporarily add:
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                
                await ReloadCategories(viewModel);
                return View(viewModel);
            }

            // Handle thumbnail upload
            if (viewModel.ThumbnailUpload != null)
            {
                // Delete old file if one exists
                if (!string.IsNullOrEmpty(videoGame.ThumbnailPath))
                {
                    var relativePath = videoGame.ThumbnailPath.Replace("~", "").TrimStart('/','\\');
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                try
                {
                    videoGame.ThumbnailPath = await SaveThumbnailAsync(viewModel.ThumbnailUpload);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("ThumbnailUpload", ex.Message);
                    await ReloadCategories(viewModel);
                    return View(viewModel);
                }
            }

            // Update fields
            videoGame.Title = Sanitizer.Sanitize(viewModel.VideoGame.Title);
            videoGame.Description = Sanitizer.Sanitize(viewModel.VideoGame.Description);
            videoGame.ShortDescription = Sanitizer.Sanitize(viewModel.VideoGame.ShortDescription);
            videoGame.ReleaseDate = viewModel.VideoGame.ReleaseDate;
            videoGame.Price = viewModel.VideoGame.Price;

            // Update categories
            videoGame.GameSpecificCategories.Clear();
            foreach (var category in viewModel.Categories.Where(c => c.IsSelected))
            {
                videoGame.GameSpecificCategories.Add(new GameSpecificCategoryModel
                {
                    VideoGameId = videoGame.Id,
                    CategoryId = category.CategoryId
                });
            }

            try
            {
                _context.Update(videoGame);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoGameModelExists(videoGame.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/VideoGame/Delete/5
        [Authorize(Policy = "IsPublisher")]
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
        [Authorize(Policy = "IsPublisher")]
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
        
        private async Task<string?> SaveThumbnailAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Unsupported file type. Only .jpg and .png are allowed.");

            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
                throw new InvalidOperationException("File size exceeds 2MB limit.");

            var fileName = Path.GetRandomFileName() + ext;
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);

            await using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "~/img/" + fileName;
        }
        
        private async Task ReloadCategories(VideoGameCreateViewModel viewModel)
        {
            var allCategories = await _context.Categories.ToListAsync();

            viewModel.Categories = allCategories.Select(c => new CategoryCheckboxViewModel
            {
                CategoryId = c.Id,
                Name = c.Name,
                IsSelected = viewModel.Categories?.Any(vc => vc.CategoryId == c.Id && vc.IsSelected) == true
            }).ToList();
        }

        
        
    }
}
