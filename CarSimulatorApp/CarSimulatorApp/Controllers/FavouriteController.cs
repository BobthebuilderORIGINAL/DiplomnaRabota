using CarSimulatorApp.Data;
using CarSimulatorApp.Infrastructure.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSimulatorApp.Controllers
{
    [Authorize]
    public class FavouriteController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouriteController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Favourite
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var favourites = await _db.Favourites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(favourites);
        }

        // POST: /Favourite/Toggle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int productId)
        {
            var userId = _userManager.GetUserId(User);

            var existing = await _db.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            bool isFavourited;

            if (existing != null)
            {
                _db.Favourites.Remove(existing);
                isFavourited = false;
            }
            else
            {
                _db.Favourites.Add(new Favourites { UserId = userId, ProductId = productId });
                isFavourited = true;
            }

            await _db.SaveChangesAsync();
            return Json(new { isFavourited });
        }

        // POST: /Favourite/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = _userManager.GetUserId(User);

            var fav = await _db.Favourites
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);

            if (fav != null)
            {
                _db.Favourites.Remove(fav);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
