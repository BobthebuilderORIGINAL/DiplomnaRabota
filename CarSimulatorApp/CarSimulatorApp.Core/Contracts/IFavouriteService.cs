using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarSimulatorApp.Infrastructure.Data.Domain;

namespace CarSimulatorApp.Core.Contracts
{
    public interface IFavouriteService
    {
        List<int> GetFavouriteIds(string userId);
        bool Toggle(string userId, int productId);
        List<Favourites> GetFavourites(string userId);
        bool Remove(string userId, int favouriteId);
    }
}