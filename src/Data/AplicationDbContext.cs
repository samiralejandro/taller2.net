using Microsoft.EntityFrameworkCore;
using Taller2Net.Models;

namespace Taller2Net.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Productos { get; set; }
    }
}
