using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSimulatorApp.Core.Contracts;
using CarSimulatorApp.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;

namespace CarSimulatorApp.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool HasOrdered(string userId, int productId)
        {
            return _context.Orders.Any(o => o.UserId == userId && o.ProductId == productId);
        }

        public bool HasRated(string userId, int productId)
        {
            return _context.Ratings.Any(r => r.UserId == userId && r.ProductId == productId);
        }

        public void Rate(string userId, int productId, int stars)
        {
            var existing = _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.ProductId == productId);

            if (existing != null)
            {
                existing.Stars = stars;
            }
            else
            {
                _context.Ratings.Add(new Rating
                {
                    UserId = userId,
                    ProductId = productId,
                    Stars = stars
                });
            }
            _context.SaveChanges();
        }

        public double GetAverageRating(int productId)
        {
            var ratings = _context.Ratings.Where(r => r.ProductId == productId).ToList();
            if (!ratings.Any()) return 0;
            return Math.Round(ratings.Average(r => r.Stars), 1);
        }

        public int GetRatingCount(int productId)
        {
            return _context.Ratings.Count(r => r.ProductId == productId);
        }

        public int? GetUserRating(string userId, int productId)
        {
            return _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.ProductId == productId)?.Stars;
        }
    }
}
