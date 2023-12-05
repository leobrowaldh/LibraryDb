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
                TheAuthor author = new TheAuthor();
                author.Seed(seeder);
                //if the author already exists, we add the isbn to the existing author.
                if (CheckIfAuthorExists(author, out  TheAuthor existingAuthor))
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

        #region Creating things in database
        /// <summary>
        /// Creates a desired Author and reurns true if successfull, false if it allready exists.
        /// </summary>
        /// <param name="authorName"></param>
        /// <returns></returns>
        public bool CreateAuthor(string authorName)
        {
            using (Context context = new Context())
            {
                TheAuthor author = new TheAuthor(){ AuthorName = authorName };
                if (!CheckIfAuthorExists(author, out _))
                {
                    context.TheAuthors.Add(author);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of an isbn, this does not create copies of the book in the library it sets the book information (isbn)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <param name="rating"></param>
        /// <param name="copies"></param>
        /// <param name="authors"></param>
        public void CreateNewIsbn(string title, int year, int rating, List<TheAuthor> authors)
        {
            csSeedGenerator seeder = new csSeedGenerator();
            ISBN isbn = new ISBN()
            {
                Isbn = ISBN.RandomizeIsbn(seeder),
                Title = title,
                Year = year,
                Rating = rating,
                Authors = authors
            };

            using (Context context = new Context())
            {
                context.ISBNs.Add(isbn);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a number of actual instances of the book(isbn) in the library.
        /// </summary>
        public void CreateCopyOfExistingIsbn(int copiesToAdd, ISBN isbn)
        {
            for (int i = 0; i < copiesToAdd; i++)
            {
                Book book = new Book();
                book.ISBN = isbn;
            }

            using (Context context = new Context())
            {
                context.SaveChanges();
            }
        }

        public void CreateNewCustomer(string firstName, string lastName)
        {
            Customer customer = new Customer(firstName, lastName);

            using (Context context = new Context())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
        }

        #endregion

        #region Checking things in database
        /// <summary>
        /// returns true if the author allready exists in the database, and outputs this existing Author.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="existingAuthor"></param>
        /// <returns></returns>
        public bool CheckIfAuthorExists(TheAuthor author, out TheAuthor existingAuthor)
        {
            using (Context context = new Context())
            {
                var existingAuthors = context.TheAuthors.ToList();
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


        /// <summary>
        /// returns true if the isbn allready exists in the database, and outputs this existing isbn.
        /// </summary>
        /// <param name="iSBN"></param>
        /// <param name="existingIsbn"></param>
        /// <returns></returns>
        public bool CheckIfIsbnInDatabase(ISBN iSBN, out ISBN existingIsbn)
        {
            using (Context context = new Context())
            {
                var existingIsbns = context.ISBNs.ToList();

                for (int i = 0; i < existingIsbns.Count; i++)
                {
                    if (existingIsbns[i].Isbn == iSBN.Isbn)
                    {
                        existingIsbn = existingIsbns[i];
                        return true;
                    }
                }
            }
            existingIsbn = null;
            return false;
        }

        #endregion

        #region renting and returning
        public void RentBook(Book book, Customer customer, DateTime borrowDate, DateTime returnDate)
        {
            book.Borrowed = true;
            book.BorrowedDate = borrowDate;
            book.ReturnDate = returnDate;
            book.Customer = customer;
            using(Context context = new Context())
            {
                context.SaveChanges();
            }
        }

        #endregion

    }
}