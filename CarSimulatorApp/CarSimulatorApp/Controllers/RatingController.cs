using CarSimulatorApp.Core.Contracts;
using CarSimulatorApp.Infrastructure.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSimulatorApp.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(IRatingService ratingService, UserManager<ApplicationUser> userManager)
        {
            _ratingService = ratingService;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rate(int productId, int stars)
        {
            var userId = _userManager.GetUserId(User);

            if (!_ratingService.HasOrdered(userId, productId))
                return Forbid();

            if (stars < 1 || stars > 5)
                return BadRequest();

            _ratingService.Rate(userId, productId, stars);
            return RedirectToAction("Details", "Product", new { id = productId });
        }
    }
}