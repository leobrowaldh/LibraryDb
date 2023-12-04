using Helpers;
using LibraryDb.Models;

namespace LibraryDb.Data
{
    internal class DataAccess
    {
        public void SeedLibrary()
        {
            using (Context context = new Context())
            {
                csSeedGenerator seeder = new csSeedGenerator();
                //20 isbn will be created, there will be 2-10 copies of each in the library.
                for (int i = 0; i < 20; i++)
                {
                    ISBN isbn = new ISBN();
                    isbn.Seed(seeder);
                    int copies = seeder.Next(2, 10);
                    for (int j = 0; j < copies; j++)
                    {
                        Book book = new Book();
                        book.ISBN = isbn;
                    }
                    //only one isbn will have 2 authors, since this is not very common.
                    if (i == 0)
                    {
                        Author author1 = new Author();
                        Author author2 = new Author();
                        author1.Seed(seeder);
                        author2.Seed(seeder);
                        isbn.Authors.Add(author1);
                        isbn.Authors.Add(author2);
                        Book book = new Book();
                        isbn.Books.Add(book);
                        context.ISBNs.Add(isbn);
                    }
                    else
                    {
                        Author author = new Author();
                        author.Seed(seeder);
                        isbn.Authors.Add(author);
                        Book book = new Book();
                        isbn.Books.Add(book);
                        context.ISBNs.Add(isbn);
                    }
                }
                context.SaveChanges();
            }
        }

        public void SeedCustomers()
        {
            using (Context context = new Context())
            {
                //10 customers will be created
                csSeedGenerator seeder = new csSeedGenerator();
                for (int i = 0; i < 10; i++)
                {
                    Customer customer = new Customer();
                    customer.Seed(seeder);
                    context.Customers.Add(customer);
                }
                context.SaveChanges();
            }
        }
    }
}