using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _30_Data;
using _30_Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace _20_Business.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;


        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteAsync(int userId, Favorite favorite)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            favorite.UserId = userId;
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFavoriteAsync(int userId, int mealId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MealId == mealId);

            if (favorite == null)
            {
                return false;
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFavoriteAsync(int userId, int mealId, string newComment)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MealId == mealId);

            if (favorite == null)
            {
                return false;
            }

            favorite.Comment = newComment;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesByUserAsync(int userId)
        {
            return await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();
        }

    }
}
