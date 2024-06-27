using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Models;
using ReversiMvcApp.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReversiMvcApp.Data.Context
{
    public class ReversiDbIdentityContext : IdentityDbContext<Speler>
    {

        public DbSet<Speler> Spelers { get; set; }

        public DbSet<Spel> Spellen { get; set; }

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
                .Property<DateTime?>("Created");

            modelBuilder.Entity<Spel>()
                .Property<DateTime?>("Modified");

            modelBuilder.Entity<Spel>()
                .Property(spel => spel.Bord)
                .HasConversion(bordConverter);

            modelBuilder.Entity<Speler>()
                .Property<DateTime?>("Created");

            modelBuilder.Entity<Speler>()
                .Property<DateTime?>("Modified");

            modelBuilder.Entity<Speler>()
                .HasOne<Spel>()
                .WithMany(spel => spel.Spelers)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void SeedData(ModelBuilder modelBuilder)
        {
            this.SeedUsers(modelBuilder);
            SeedRoles(modelBuilder);
            this.SeedUserRoles(modelBuilder);
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            PasswordHasher<Speler> passwordHasher = new PasswordHasher<Speler>();

            Speler adminUser = new Speler()
            {
                Id = "60c35514-e6cf-47f4-84ad-8b010ce0b7b4",
                UserName = "admin@reversi.nl",
                NormalizedUserName = "ADMIN@REVERSI.NL",
                Naam = "Admin Bram",
                Email = "admin@reversi.nl",
                NormalizedEmail = "ADMIN@REVERSI.NL",
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
                NormalizedUserName = "MEDIATOR@REVERSI.NL",
                Naam = "Mediator Bram",
                Email = "mediator@reversi.nl",
                NormalizedEmail = "MEDIATOR@REVERSI.NL",
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
                NormalizedUserName = "SPELER@REVERSI.NL",
                Naam = "Bram",
                Email = "speler@reversi.nl",
                NormalizedEmail = "SPELER@REVERSI.NL",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "12345678",
                IsEnabled = true,
            };

            spelerUser.PasswordHash = passwordHasher.HashPassword(spelerUser, "Test1234!");

            modelBuilder.Entity<Speler>().HasData(adminUser, mediatorUser, spelerUser);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
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


        public override int SaveChanges()
        {
            HandleAuditableEntries();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            HandleAuditableEntries();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleAuditableEntries();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            HandleAuditableEntries();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void HandleAuditableEntries()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (EntityEntry entry in modifiedEntries)
            {
                var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());

                var modifiedProperty = entityType.FindProperty("Modified");
                var createdProperty = entityType.FindProperty("Created");

                if (entry.State == EntityState.Modified && modifiedProperty != null)
                {
                    entry.Property("Modified").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Added && createdProperty != null)
                {
                    entry.Property("Created").CurrentValue = DateTime.Now;
                }
            }
        }
    }
}

