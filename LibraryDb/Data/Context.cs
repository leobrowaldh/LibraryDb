using Microsoft.EntityFrameworkCore;
using LibraryDb.Models;

namespace LibraryDb.Data
{
    internal class Context : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ISBN> ISBNs { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"
                    Server=localhost; 
                    Database=NewtonLibraryLeo; 
                    Trusted_Connection=True; 
                    Trust Server Certificate=Yes; 
                    User Id=NewtonLibrary; 
                    password=NewtonLibrary");
        }
    }
}
