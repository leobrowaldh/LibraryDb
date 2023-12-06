using Helpers;
using LibraryDb.Models;

namespace LibraryDb.Data
{
    internal class DataAccess
    {
        #region Database Seeding
        public void SeedLibrary(int numberOfIsbn, int minNumberOFCopies, int maxNumberOfCopies)
        {
            csSeedGenerator seeder = new csSeedGenerator();
            for (int i = 0; i < numberOfIsbn; i++)
            {
                CreateNewIsbn(seeder, out ISBN isbn);
                TheAuthor theAuthor = new TheAuthor();
                theAuthor.Seed(seeder);
                isbn.Authors.Add(theAuthor);

                int copies = seeder.Next(minNumberOFCopies, maxNumberOfCopies);
                for (int j = 0; j < copies; j++)
                {
                    CreateCopyOfExistingIsbn(copies, isbn);
                }
                using (Context context = new Context())
                {
                    context.ISBNs.Add(isbn);
                    context.SaveChanges();
                }
                        
            }
        }

        public void SeedCustomers(int numberOfCustomers)
        {
            using (Context context = new Context())
            {
                csSeedGenerator seeder = new csSeedGenerator();
                for (int i = 0; i < numberOfCustomers; i++)
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
       
        public void CreateAuthor(string authorName)
        {
            using (Context context = new Context())
            {
                TheAuthor theAuthor = new TheAuthor() { AuthorName = authorName};
                context.TheAuthors.Add(theAuthor);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates an instance of an isbn, this does not create copies of the book in the library it sets the book information (isbn)
        /// A random unique string is generated as isbn property.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <param name="rating"></param>
        /// <param name="authors"></param>
        public void CreateNewIsbn(string title, int year, int rating, List<TheAuthor> authors, out ISBN isbn)
        {
            using (Context context = new Context())
            {
                csSeedGenerator seeder = new csSeedGenerator();
                isbn = new ISBN()
                {
                    Title = title,
                    Year = year,
                    Rating = rating,
                    Authors = authors
                };
                //Generating a unique random Isbn:
                ISBN? existingIsbn = null;
                string randomIsbn;
                do
                {
                    randomIsbn = ISBN.RandomizeIsbn(seeder);
                    existingIsbn = context.ISBNs.FirstOrDefault(x => x.Isbn == randomIsbn);
                }
                while (existingIsbn != null);
                isbn.Isbn = randomIsbn;
                context.ISBNs.Add(isbn);
                context.SaveChanges();
            }
        }

        public void CreateNewIsbn(csSeedGenerator seeder, out ISBN isbn)
        {
            using (Context context = new Context())
            {
                isbn = new ISBN();
                isbn.Seed(seeder);
                //Generating a unique random Isbn:
                ISBN? existingIsbn = null;
                string randomIsbn;
                do
                {
                    randomIsbn = ISBN.RandomizeIsbn(seeder);
                    existingIsbn = context.ISBNs.FirstOrDefault(x => x.Isbn == randomIsbn);
                }
                while (existingIsbn != null);
                isbn.Isbn = randomIsbn;
                context.ISBNs.Add(isbn);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a number of actual instances of the book(isbn) in the library.
        /// </summary>
        public void CreateCopyOfExistingIsbn(int copiesToAdd, ISBN isbn)
        {
            using (Context context = new Context())
            {
                for (int i = 0; i < copiesToAdd; i++)
                {
                    Book book = new Book();
                    book.ISBN = isbn;
                    context.Books.Add(book);
                }
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

        #region Borrowing and returning
        public void BorrowBook(Book book, Customer customer, DateTime borrowDate, DateTime returnDate)
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

        public void ReturnBook(Book book)
        {
            book.Borrowed = false;
            book.BorrowedDate= null;
            book.ReturnDate= null;
            using (Context context = new Context())
            {
                context.SaveChanges();
            }
        }

        #endregion

        #region Removing things from database

        /// <summary>
        /// Deletes the Customer with the provided CustomerId and return true if deleted, 
        /// return false if the customerId is not found in the database.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>bool</returns>
        public bool DeleteCustomer(int customerId)
        {
            using (Context context = new Context())
            {
                Customer? customerToBeDeleted = context.Customers.Find(customerId);
                if(customerToBeDeleted != null)
                {
                    context.Customers.Remove(customerToBeDeleted);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes the Book with the provided bookId and return true if deleted, 
        /// return false if the bookId is not found in the database.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>bool</returns>
        public bool DeleteBook(int bookId)
        {
            using (Context context = new Context())
            {
                Book? bookToBeDeleted = context.Books.Find(bookId);
                if (bookToBeDeleted != null)
                {
                    context.Books.Remove(bookToBeDeleted);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }



        #endregion

    }
}