using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _30_Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace _20_Business.Services
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(int userId, Favorite favorite);
        Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(int userId);
        Task<bool> DeleteFavoriteAsync(int userId, int mealId);
    }
}
