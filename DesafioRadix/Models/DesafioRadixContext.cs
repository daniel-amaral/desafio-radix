using Microsoft.EntityFrameworkCore;

namespace DesafioRadix.Models
{
    public class DesafioRadixContext : DbContext
    {
        public DesafioRadixContext(DbContextOptions<DesafioRadixContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }

        /*
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Review>().ToTable("Review");
        }
        */
    }
}
