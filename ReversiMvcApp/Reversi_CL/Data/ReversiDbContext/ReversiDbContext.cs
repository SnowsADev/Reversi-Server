using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reversi_CL.Helpers;
using Reversi_CL.Models;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi_CL.Data.ReversiDbContext
{
    public class ReversiDbContext : DbContext
    {
        
        public DbSet<Spel> Spellen { get; set; }

        public ReversiDbContext() : base()
        {
        }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var bordConverter = new ValueConverter<Kleur[,], string>(
                value => value.ConvertToString(),
                value => value.ConvertBordFromString()
            );

            modelBuilder
                .Entity<Spel>()
                .Property(spel => spel.Bord)
                .HasConversion(bordConverter);

            modelBuilder.Entity<Spel>()
                .HasMany(spel => spel.Spelers)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        if (entry.Entity.GetType() == typeof(Spel))
                        {
                            GameFinished((Spel)entry.Entity);
                        }
                        break;
                }
            }
        }

        private void GameFinished(Spel spel)
        {
            if (spel.SpelIsAfgelopen)
            {

            }
        }


    }
}
