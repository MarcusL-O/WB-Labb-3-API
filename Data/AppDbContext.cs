using Labb3_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Technology> Technologies { get; set; }
    }
}
