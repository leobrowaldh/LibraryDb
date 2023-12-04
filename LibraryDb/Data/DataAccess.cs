using Helpers;
using LibraryDb.Models;

namespace LibraryDb.Data
{
    internal class DataAccess
    {
        #region Database Seeding
        public void SeedLibrary()
        {
            csSeedGenerator seeder = new csSeedGenerator();
            //20 isbn will be created, there will be 2-10 copies of each in the library.
            for (int i = 0; i < 20; i++)
            {
                ISBN isbn = new ISBN();
                isbn.Seed(seeder); 
                Author author = new Author();
                author.Seed(seeder);
                //if the author already exists, we add the isbn to the existing author.
                if (CheckIfAuthorExists(author, out  Author existingAuthor))
                {
                    author = existingAuthor;
                }
                isbn.Authors.Add(author);

                int copies = seeder.Next(2, 10);
                for (int j = 0; j < copies; j++)
                {
                    Book book = new Book();
                    isbn.Books.Add(book);
                }
                using (Context context = new Context())
                {
                    context.ISBNs.Add(isbn);
                    context.SaveChanges();
                }
                        
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
        #endregion


        /// <summary>
        /// Creates a desired Author and reurns true if successfull, false if it allready exists.
        /// </summary>
        /// <param name="authorName"></param>
        /// <returns></returns>
        public bool CreateAuthor(string authorName)
        {
            using (Context context = new Context())
            {
                Author author = new Author(){ AuthorName = authorName };
                if (!CheckIfAuthorExists(author, out _))
                {
                    context.Authors.Add(author);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// returns true if the author allready exists in the database, and outputs this existing Author.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="existingAuthor"></param>
        /// <returns></returns>
        public bool CheckIfAuthorExists(Author author, out Author existingAuthor)
        {
            using (Context context = new Context())
            {
                var existingAuthors = context.Authors.ToList();
                for (int i = 0;i < existingAuthors.Count; i++)
                {
                    if (existingAuthors[i].AuthorName.ToLower() == author.AuthorName.ToLower())
                    {
                        existingAuthor = existingAuthors[i];
                        return true;
                    }
                }
            }
            existingAuthor = null;
            return false;
        }

        public void CreateBook(string title, int year, int rating, int copies, List<Author> authors)
        {
            using (Context context = new Context())
            {
                csSeedGenerator seeder = new csSeedGenerator();
                ISBN isbn = new ISBN()
                {
                    Isbn = ISBN.RandomizeIsbn(seeder),
                    Title = title,
                    Year = year,
                    Rating = rating
                };
            }
        }

        public void CreateNewCustomer(string firstName, string lastName)
        {

        }

        
    }
}