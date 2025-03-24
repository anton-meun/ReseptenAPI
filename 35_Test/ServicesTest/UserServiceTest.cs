using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using _20_Business.Services;
using _30_Data;
using _30_Data.Entities;
using System.Diagnostics;
using Xunit.Abstractions;

public class UserServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly AppDbContext _dbContext;
    private readonly UserService _userService;

    public UserServiceTests(ITestOutputHelper output)
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
    public async Task CreateUserAsync_Should_Create_User_When_Email_Is_Unique()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);
        var user = new User { FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", Password = "SecurePassword123" };

        var createdUser = await userService.CreateUserAsync(user);

        Assert.NotNull(createdUser);
        Assert.Equal("johndoe@example.com", createdUser.Email);
    }

    [Fact]
    public async Task CreateUserAsync_Should_Throw_Exception_When_Email_Exists()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);
        var existingUser = new User { FirstName = "Jane", LastName = "Doe", Email = "janedoe@example.com", Password = "SecurePassword123" };
        await dbContext.Users.AddAsync(existingUser);
        await dbContext.SaveChangesAsync();

        var newUser = new User { FirstName = "New", LastName = "User", Email = "janedoe@example.com", Password = "AnotherPassword" };

        await Assert.ThrowsAsync<Exception>(async () => await userService.CreateUserAsync(newUser));
    }

    [Fact]
    public async Task GetUserByIdAsync_Should_Return_User_When_Exists()
    {
        var stopwatch = Stopwatch.StartNew();

        stopwatch.Restart();
        var dbContext = await GetDatabaseContext();
        stopwatch.Stop();
        _output.WriteLine($"⏳ GetDatabaseContext: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Restart();
        var userService = new UserService(dbContext);
        stopwatch.Stop();
        _output.WriteLine($"⏳ Create UserService: {stopwatch.ElapsedMilliseconds} ms");

        var user = new User { FirstName = "Test", LastName = "User", Email = "testuser@example.com", Password = "TestPassword" };

        stopwatch.Restart();
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        stopwatch.Stop();
        _output.WriteLine($"⏳ Save User: {stopwatch.ElapsedMilliseconds} ms");

        stopwatch.Restart();
        var foundUser = await userService.GetUserByIdAsync(user.Id);
        stopwatch.Stop();
        _output.WriteLine($"⏳ GetUserByIdAsync: {stopwatch.ElapsedMilliseconds} ms");

        Assert.NotNull(foundUser);
        Assert.Equal("testuser@example.com", foundUser.Email);
    }


    [Fact]
    public async Task GetUserByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);

        var foundUser = await userService.GetUserByIdAsync(100000);

        Assert.Null(foundUser);
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Return_User_When_Email_Exists()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);
        var user = new User { FirstName = "Existing", LastName = "User", Email = "existing@example.com", Password = "Password123" };
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var foundUser = await userService.GetByEmailAsync("existing@example.com");

        Assert.NotNull(foundUser);
        Assert.Equal("existing@example.com", foundUser.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Return_Null_When_Email_Not_Found()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);

        var foundUser = await userService.GetByEmailAsync("notfound@example.com");

        Assert.Null(foundUser);
    }

    [Fact]
    public async Task ValidateUserAsync_Should_Return_User_When_Credentials_Are_Correct()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            FirstName = "Valid",
            LastName = "User",
            Email = "validuser@example.com",
            Password = passwordHasher.HashPassword(null, "ValidPassword123")
        };
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var validatedUser = await userService.ValidateUserAsync("validuser@example.com", "ValidPassword123");

        Assert.NotNull(validatedUser);
        Assert.Equal("validuser@example.com", validatedUser.Email);
    }

    [Fact]
    public async Task ValidateUserAsync_Should_Return_Null_When_Credentials_Are_Incorrect()
    {
        var dbContext = await GetDatabaseContext();
        var userService = new UserService(dbContext);
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            FirstName = "Invalid",
            LastName = "User",
            Email = "invaliduser@example.com",
            Password = passwordHasher.HashPassword(null, "CorrectPassword")
        };
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var validatedUser = await userService.ValidateUserAsync("invaliduser@example.com", "WrongPassword");

        Assert.Null(validatedUser);
    }

   

   
}
