using CarSimulatorApp.Core.Contracts;
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
        private readonly IFavouriteService _favouriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouriteController(IFavouriteService favouriteService, UserManager<ApplicationUser> userManager)
        {
            _favouriteService = favouriteService;
            _userManager = userManager;
        }
        // GET: /Favourite
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var favourites = _favouriteService.GetFavourites(userId);
            return View(favourites);
        }

        // POST: /Favourite/Toggle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int productId)
        {
            var userId = _userManager.GetUserId(User);
            bool isFavourited = _favouriteService.Toggle(userId, productId);
            return Json(new { isFavourited });
        }

        // POST: /Favourite/Remove/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var userId = _userManager.GetUserId(User);
            _favouriteService.Remove(userId, id);
            return RedirectToAction(nameof(Index));

        }
    }
}