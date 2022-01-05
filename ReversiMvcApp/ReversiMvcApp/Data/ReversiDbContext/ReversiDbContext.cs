using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Models;

namespace ReversiMvcApp.Data.ReversiDbContext
{
    public class ReversiDbContext : IdentityDbContext<Speler>
    {

        public ReversiDbContext() : base()
        {
        }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options) 
        { 
        }

        public DbSet<Speler> Speler { get; set; }
    }
}
