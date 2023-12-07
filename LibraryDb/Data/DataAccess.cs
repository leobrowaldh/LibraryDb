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
                int isbnId;
                using (Context context = new Context())
                {
                    isbnId = CreateNewIsbn(seeder, out ISBN newIsbn);
                    TheAuthor theAuthor = new TheAuthor();
                    theAuthor.Seed(seeder);
                    newIsbn.Authors.Add(theAuthor);
                    context.TheAuthors.Add(theAuthor);
                    context.SaveChanges();
                }
                int copies = seeder.Next(minNumberOFCopies, maxNumberOfCopies);
                for (int j = 0; j < copies; j++)
                {
                    int bookId = CreateCopyOfExistingIsbn(isbnId, out Book? newCopy);
                    if (bookId == -1)
                    {
                        throw new Exception("isbn not found in database");
                    }
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
       
        /// <summary>
        /// Creates a new author and return its id, outputs its instance
        /// </summary>
        /// <param name="authorName"></param>
        /// <returns></returns>
        public int CreateAuthor(string authorName , out TheAuthor newlyCreatedAuthor)
        {
            using (Context context = new Context())
            {
                newlyCreatedAuthor = new TheAuthor() { AuthorName = authorName};
                context.TheAuthors.Add(newlyCreatedAuthor);
                context.SaveChanges();
                int authorId = newlyCreatedAuthor.Id;
                return authorId;
            }
        }

        /// <summary>
        /// Creates an instance of an isbn, this does not create copies of the book in the library, it sets the book information (isbn)
        /// A random unique string is generated as isbn property. author IDs of existing authors has to be provided in a list.
        /// The new IsbnId is returned, and its instance as an out.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <param name="rating"></param>
        /// <param name="authorsIds"></param>
        /// <returns></returns>
        public int CreateNewIsbn(string title, int year, int rating, List<int> authorsIds, out ISBN newlyCreatedIsbn )
        {
            using (Context context = new Context())
            {

                List<TheAuthor> authors = context.TheAuthors
                .Where(x => authorsIds.Contains(x.Id))
                .ToList();

                csSeedGenerator seeder = new csSeedGenerator();
                newlyCreatedIsbn = new ISBN()
                {
                    Title = title,
                    Year = year,
                    Rating = rating,
                    Authors = authors
                };
                //Generating a unique random Isbn:
                ISBN? existingIsbn = null;
                string randomIsbn;
                // randomize until we get one that is not in the database:
                do
                {
                    randomIsbn = ISBN.RandomizeIsbn(seeder);
                    existingIsbn = context.ISBNs.FirstOrDefault(x => x.Isbn == randomIsbn);
                }
                while (existingIsbn != null);
                newlyCreatedIsbn.Isbn = randomIsbn;
                context.ISBNs.Add(newlyCreatedIsbn);
                context.SaveChanges();
                int isbnId = newlyCreatedIsbn.Id;
                return isbnId;
            }
        }

        /// <summary>
        /// Creates a random instance of an isbn, this does not create copies of the book in the library, it sets the book information (isbn)
        /// A random unique string is generated as isbn property. The new IsbnId is returned, and its instance as an out.
        /// </summary>
        /// <param name="seeder"></param>
        /// <returns></returns>
        public int CreateNewIsbn(csSeedGenerator seeder, out ISBN newlyCreatedIsbn)
        {
            using (Context context = new Context())
            {
                newlyCreatedIsbn = new ISBN();
                newlyCreatedIsbn.Seed(seeder);
                TheAuthor author = new TheAuthor();
                author.Seed(seeder);

                //Generating a unique random Isbn:
                ISBN? existingIsbn = null;
                string randomIsbn;
                do
                {
                    randomIsbn = ISBN.RandomizeIsbn(seeder);
                    existingIsbn = context.ISBNs.FirstOrDefault(x => x.Isbn == randomIsbn);
                }
                while (existingIsbn != null);
                newlyCreatedIsbn.Isbn = randomIsbn;
                context.ISBNs.Add(newlyCreatedIsbn);
                context.SaveChanges();
                int isbnId = newlyCreatedIsbn.Id;
                return isbnId;
            }
        }

        /// <summary>
        /// Creates a copy of the book(isbn) in the library. return the bookId (or -1 if isbn dont exist), outputs the book instance.
        /// </summary>
        public int CreateCopyOfExistingIsbn( int isbnId, out Book? newCopy)
        {
            using (Context context = new Context())
            {
                ISBN? isbn = context.ISBNs.FirstOrDefault(x => x.Id == isbnId);
                if (isbn != null)
                {
                    newCopy = new Book();
                    newCopy.ISBN = isbn;
                    isbn.Books.Add(newCopy);
                    context.SaveChanges();
                    int bookId = newCopy.Id;
                    return bookId;
                }
                else 
                { 
                    newCopy = null;
                    return -1; 
                }
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