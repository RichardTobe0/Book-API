using Microsoft.EntityFrameworkCore;

namespace Books.model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { }

        public DbSet<Book> Books { get; set; }
    }
}
