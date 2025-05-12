using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT2_Exam.Data;

namespace TT2_Exam.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        // GET: StoreController
        public async Task<IActionResult>  Index()
        {
            return View(await _context.VideoGames.ToListAsync());
        }
        
        public async Task<IActionResult>  Details()
        {
            return View();
        }

    }
}
