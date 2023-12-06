using Microsoft.EntityFrameworkCore;
using LibraryDb.Models;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using EntityFrameworkCore.EncryptColumn.Extension;

namespace LibraryDb.Data
{
    internal class Context : DbContext
    {
        public DbSet<TheAuthor> TheAuthors { get; set; }
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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"
        //            Server=tcp:final-project-db-leo.database.windows.net,1433;Initial Catalog=NewtonLibraryLeo;
        //            Persist Security Info=False;User ID=NewtonLibrary;Password=TheBestLibrary23;MultipleActiveResultSets=False;
        //            Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}

        //To encrypt any column that use the attribute [encryptColumn]
        private readonly IEncryptionProvider _provider;
        public Context()
        {
            this._provider = new GenerateEncryptionProvider("eksa9o2f8sdv3qx374rlsiv1d0gd73du");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(this._provider);
        }
    }
}
