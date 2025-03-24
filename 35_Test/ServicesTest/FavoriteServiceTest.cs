using _20_Business.Services;
using _30_Data;
using _30_Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace _35_Test.ServicesTest
{
    public class FavoriteServiceTest
    {
        private readonly ITestOutputHelper _output;
        private readonly AppDbContext _dbContext;
        private readonly FavoriteService _favoriteService;

        public FavoriteServiceTest(ITestOutputHelper output)
        {
            _output = output;
        }

        // Creates a new in-memory database context for each test to ensure isolation
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        [Fact]
        public async Task AddFavoriteAsync_Should_Add_Favorite_When_User_Exists()
        {
            var dbContext = await GetDatabaseContext();
            var FavoriteService = new FavoriteService(dbContext);
            var user = new User { FirstName = "Favorite", LastName = "Tester", Email = "favorite@example.com", Password = "Password123" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var favorite = new Favorite { MealId = 1, Comment = "Lekkere maaltijd" };
            await FavoriteService.AddFavoriteAsync(user.Id, favorite);

            var savedFavorite = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == user.Id);
            Assert.NotNull(savedFavorite);
            Assert.Equal("Lekkere maaltijd", savedFavorite.Comment);
        }

        [Fact]
        public async Task AddFavoriteAsync_Should_Throw_Exception_When_User_Not_Found()
        {
            var dbContext = await GetDatabaseContext();
            var FavoriteService = new FavoriteService(dbContext);
            var favorite = new Favorite { MealId = 1, Comment = "Niet-bestaande gebruiker" };

            await Assert.ThrowsAsync<Exception>(async () => await FavoriteService.AddFavoriteAsync(200000, favorite));
        }

        [Fact]
        public async Task GetFavoritesByUserAsync_Should_Return_Favorites_When_They_Exist()
        {
            var dbContext = await GetDatabaseContext();
            var FavoriteService = new FavoriteService(dbContext);
            var user = new User { FirstName = "Favoriet", LastName = "Gebruiker", Email = "favuser@example.com", Password = "Password123" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var favorites = new List<Favorite>
        {
            new Favorite { UserId = user.Id, MealId = 1, Comment = "Mijn eerste favoriet" },
            new Favorite { UserId = user.Id, MealId = 2, Comment = "Mijn tweede favoriet" }
        };
            await dbContext.Favorites.AddRangeAsync(favorites);
            await dbContext.SaveChangesAsync();

            var userFavorites = await FavoriteService.GetFavoritesByUserAsync(user.Id);

            Assert.NotEmpty(userFavorites);
            Assert.Equal(2, userFavorites.Count());
        }

        [Fact]
        public async Task GetFavoritesByUserAsync_Should_Return_EmptyList_When_No_Favorites()
        {
            var dbContext = await GetDatabaseContext();
            var FavoriteService = new FavoriteService(dbContext);
            var user = new User { FirstName = "Lege", LastName = "Gebruiker", Email = "nogeen@example.com", Password = "Password123" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var userFavorites = await FavoriteService.GetFavoritesByUserAsync(user.Id);

            Assert.Empty(userFavorites);
        }

        [Fact]
        public async Task DeleteFavoriteAsync_Should_Remove_Favorite_When_Exists()
        {
            var dbContext = await GetDatabaseContext();
            var favoriteService = new FavoriteService(dbContext);
            var user = new User { FirstName = "Test", LastName = "User", Email = "testuser@example.com", Password = "Password123" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var favorite = new Favorite { UserId = user.Id, MealId = 1, Comment = "Favoriet om te verwijderen" };
            await dbContext.Favorites.AddAsync(favorite);
            await dbContext.SaveChangesAsync();

            // Controleren of de favoriet eerst bestaat
            var existingFavorite = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == user.Id && f.MealId == 1);
            Assert.NotNull(existingFavorite);

            // Verwijder de favoriet
            await favoriteService.DeleteFavoriteAsync(user.Id, 1);

            // Controleren of de favoriet is verwijderd
            var deletedFavorite = await dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == user.Id && f.MealId == 1);
            Assert.Null(deletedFavorite);
        }

        [Fact]
        public async Task DeleteFavoriteAsync_Should_Return_False_When_User_Not_Found()
        {
            var dbContext = await GetDatabaseContext();
            var favoriteService = new FavoriteService(dbContext);

            // Probeer een favoriet te verwijderen voor een niet-bestaande gebruiker
            var result = await favoriteService.DeleteFavoriteAsync(99999, 1);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFavoriteAsync_Should_Return_False_When_Favorite_Not_Found()
        {
            var dbContext = await GetDatabaseContext();
            var favoriteService = new FavoriteService(dbContext);
            var user = new User { FirstName = "Test", LastName = "User", Email = "testuser@example.com", Password = "Password123" };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Probeer een favoriet te verwijderen die niet bestaat
            var result = await favoriteService.DeleteFavoriteAsync(user.Id, 99999);

            Assert.False(result);
        }

    }
}
