using Microsoft.EntityFrameworkCore;
using DesafioRadix.Models.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace DesafioRadix.Models
{
    public class DesafioRadixContext : DbContext
    {
        public IConfiguration Configuration { get; private set; }

        public DesafioRadixContext(DbContextOptions<DesafioRadixContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Review>().ToTable("Review");
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string withinDockerConectionStr = Configuration.GetConnectionString("WithinDocker");
            Console.WriteLine("\n\n->Context connection string: " + withinDockerConectionStr + "\n\n");
            optionsBuilder.UseMySql(withinDockerConectionStr, options => options.EnableRetryOnFailure());
        }
        */
    }
}
