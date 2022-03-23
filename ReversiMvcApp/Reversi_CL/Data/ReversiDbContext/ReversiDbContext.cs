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
    }
}
