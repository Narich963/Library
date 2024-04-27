using Microsoft.EntityFrameworkCore;

namespace ControlWork7.Models;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public LibraryContext(DbContextOptions opts) : base(opts)
    {
        
    }
}
