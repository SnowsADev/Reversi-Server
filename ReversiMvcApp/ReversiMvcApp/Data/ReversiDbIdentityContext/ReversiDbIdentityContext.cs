using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Models;

namespace ReversiMvcApp.Data.ReversiDbIdentityContext
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

    }

}
