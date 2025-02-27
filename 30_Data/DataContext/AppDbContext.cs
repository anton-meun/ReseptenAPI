using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using _30_Data.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace _30_Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            // ---- SEED DATA ----

            var passwordHasher = new PasswordHasher<User>();

            var userFaker = new Faker<User>()
                 .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                 .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                 .RuleFor(u => u.LastName, f => f.Name.LastName())
                 .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName).ToLower())
                 .RuleFor(u => u.Password, f =>
                 {
                     // Create a temporary user object
                     var user = new User();
                     // Hash a default password
                     return passwordHasher.HashPassword(user, "PasswordForFekeData"); 
                 });

            modelBuilder.Entity<User>().HasData(userFaker.Generate(1000));

            // Fake favorites 
            var favoriteFaker = new Faker<Favorite>()
                .RuleFor(f => f.Id, f => f.IndexFaker + 1)
                .RuleFor(f => f.MealId, f => f.Random.Int(1, 100))
                .RuleFor(f => f.UserId, f => f.Random.Int(1,1000))
                .RuleFor(f => f.Comment, f => f.Random.ArrayElement(new[]
                {
                    "Heerlijk gerecht, echt een aanrader!",
                    "Niet slecht, maar ik had iets meer kruiden verwacht.",
                    "Superlekker! Dit ga ik vaker maken.",
                    "Ik vond het een beetje te pittig, maar verder prima.",
                    "Perfect recept voor een gezellige avond!",
                    "Mijn kinderen vonden dit geweldig, zeker een favoriet.",
                    "Niet helemaal mijn smaak, maar wel goed bereid.",
                    "Ik zou het nog een keer maken, maar met een twist.",
                    "Echt geweldig! Dit wordt mijn go-to recept.",
                    "Makkelijk en snel te maken, perfect na een lange dag."
                }));

            modelBuilder.Entity<Favorite>().HasData(favoriteFaker.Generate(1000));
        }
    }
}

