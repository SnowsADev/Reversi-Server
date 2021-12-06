using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Data
{
    public class ReversiDbContext : DbContext
    {
        public DbSet<Speler> Spelers;

        public ReversiDbContext() : base()
        {
        }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options) 
        { 
        }

        public DbSet<ReversiMvcApp.Models.Speler> Speler { get; set; }
    }
}
