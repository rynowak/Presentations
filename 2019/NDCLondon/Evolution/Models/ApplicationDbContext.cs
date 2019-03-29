using Microsoft.EntityFrameworkCore;

namespace Evolution.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
