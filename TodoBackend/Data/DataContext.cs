using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;

namespace TodoBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Todo>? Todos { get; set; }
        public DbSet<List>? Lists { get; set; }
    }
}
