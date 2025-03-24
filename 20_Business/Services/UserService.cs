using _30_Data;
using _30_Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace _20_Business.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Hash the plain text password before storing it
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("Email address already in use.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateUserAsync(int userId, User user)
        {
            
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            
            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                var emailInUse = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (emailInUse)
                {
                    throw new Exception("Email address is already in use.");
                }
            }

            
            existingUser.FirstName = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : existingUser.FirstName;
            existingUser.LastName = !string.IsNullOrEmpty(user.LastName) ? user.LastName : existingUser.LastName;
            existingUser.Email = !string.IsNullOrEmpty(user.Email) ? user.Email : existingUser.Email;

            
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, user.Password);
            }

            
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }







        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            // Verify the provided password against the stored hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }
    }

}
