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
    }

}
