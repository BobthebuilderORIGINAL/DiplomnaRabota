using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSimulatorApp.Core.Contracts;
using CarSimulatorApp.Data;
using CarSimulatorApp.Infrastructure.Data;
using CarSimulatorApp.Infrastructure.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarSimulatorApp.Core.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly ApplicationDbContext _context;

        public FavouriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<int> GetFavouriteIds(string userId)
        {
            return _context.Favourites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToList();
        }

        public bool Toggle(string userId, int productId)
        {
            var existing = _context.Favourites
                .FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);

            if (existing != null)
            {
                _context.Favourites.Remove(existing);
                _context.SaveChanges();
                return false; // removed = not favourited
            }
            else
            {
                _context.Favourites.Add(new Favourites
                {
                    UserId = userId,
                    ProductId = productId
                });
                _context.SaveChanges();
                return true; // added = is favourited
            }
        }

        public List<Favourites> GetFavourites(string userId)
        {
            return _context.Favourites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
        }

        public bool Remove(string userId, int favouriteId)
        {
            var fav = _context.Favourites
                .FirstOrDefault(f => f.Id == favouriteId && f.UserId == userId);

            if (fav == null) return false;

            _context.Favourites.Remove(fav);
            return _context.SaveChanges() > 0;
        }
    }
}