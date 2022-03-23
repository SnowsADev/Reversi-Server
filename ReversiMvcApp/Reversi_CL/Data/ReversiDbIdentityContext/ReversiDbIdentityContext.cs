using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reversi_CL.Helpers;
using Reversi_CL.Models;

namespace Reversi_CL.Data.ReversiDbIdentityContext
{
    public class ReversiDbIdentityContext : IdentityDbContext<Speler>
    {

        public DbSet<Speler> Spelers { get; set; }

        public ReversiDbIdentityContext() : base()
        {
        }

        public ReversiDbIdentityContext(DbContextOptions<ReversiDbIdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.SeedData(modelBuilder);

            var bordConverter = new ValueConverter<Kleur[,], string>(
                value => value.ConvertToString(),
                value => value.ConvertBordFromString()
            );

            modelBuilder.Entity<Spel>()
                .Property(spel => spel.Bord)
                .HasConversion(bordConverter);

            modelBuilder.Entity<Speler>()
                .HasOne<Spel>()
                .WithMany(spel => spel.Spelers)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            this.SeedUsers(modelBuilder);
            this.SeedRoles(modelBuilder);
            this.SeedUserRoles(modelBuilder);
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            PasswordHasher<Speler> passwordHasher = new PasswordHasher<Speler>();

            Speler adminUser = new Speler()
            {
                Id = "60c35514-e6cf-47f4-84ad-8b010ce0b7b4",
                UserName = "admin@reversi.nl",
                NormalizedEmail = "admin@reversi.nl".ToUpper(),
                Naam = "Admin Bram",
                Email = "admin@reversi.nl",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "12345678",
                IsEnabled = true
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Test1234!");

            Speler mediatorUser = new Speler()
            {
                Id = "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7",
                UserName = "mediator@reversi.nl",
                NormalizedEmail = "mediator@reversi.nl".ToUpper(),
                Naam = "Mediator Bram",
                Email = "mediator@reversi.nl",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "12345678",
                IsEnabled = true
            };

            mediatorUser.PasswordHash = passwordHasher.HashPassword(mediatorUser, "Test1234!");

            Speler spelerUser = new Speler()
            {
                Id = "c7d9dd9f-99bc-422f-8f7f-0828c45acf68",
                UserName = "speler@reversi.nl",
                NormalizedEmail = "speler@reversi.nl".ToUpper(),
                Naam = "Bram",
                Email = "speler@reversi.nl",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "12345678",
                IsEnabled = true,  
            };

            spelerUser.PasswordHash = passwordHasher.HashPassword(spelerUser, "Test1234!");



            modelBuilder.Entity<Speler>().HasData(adminUser, mediatorUser, spelerUser);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "44f80e14-9ca1-48db-b6cc-2a3153d01657",
                    Name = "Mediator",
                    NormalizedName = "MEDIATOR",
                },
                new IdentityRole()
                {
                    Id = "512a1ac9-a17e-4602-a200-bf9e0ac10a6c",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole()
                {
                    Id = "d4297ca9-77fe-496f-a148-7c7232811f85",
                    Name = "Speler",
                    NormalizedName = "Speler".ToUpper(),
                }
            );
        }

        private void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                //Admin
                new IdentityUserRole<string>()
                {
                    RoleId = "512a1ac9-a17e-4602-a200-bf9e0ac10a6c",
                    UserId = "60c35514-e6cf-47f4-84ad-8b010ce0b7b4"
                },
                //Mediator
                new IdentityUserRole<string>()
                {
                    RoleId = "44f80e14-9ca1-48db-b6cc-2a3153d01657",
                    UserId = "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7"
                },
                //Speler
                new IdentityUserRole<string>()
                {
                    RoleId = "d4297ca9-77fe-496f-a148-7c7232811f85",
                    UserId = "c7d9dd9f-99bc-422f-8f7f-0828c45acf68"
                }
            );

        }
    }
}
