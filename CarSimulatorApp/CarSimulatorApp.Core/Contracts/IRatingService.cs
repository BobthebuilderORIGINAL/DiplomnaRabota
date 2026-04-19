using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulatorApp.Core.Contracts
{
    public interface IRatingService
    {
        bool HasOrdered(string userId, int productId);
        bool HasRated(string userId, int productId);
        void Rate(string userId, int productId, int stars);
        double GetAverageRating(int productId);
        int GetRatingCount(int productId);
        int? GetUserRating(string userId, int productId);
    }
}