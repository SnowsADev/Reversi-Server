using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public DbSet<ReversiMvcApp.Models.Speler> Speler { get; set; }
    }
}
