using Microsoft.EntityFrameworkCore;

namespace ControlWork7.Models;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserAndBook> UsersAndBooks { get; set; }
    public LibraryContext(DbContextOptions opts) : base(opts)
    {
        
    }
}
