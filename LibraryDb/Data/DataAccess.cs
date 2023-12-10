using Helpers;
using LibraryDb.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
                {   CreateNewIsbn(seeder, out ISBN newIsbn, context);
                    TheAuthor theAuthor = new TheAuthor();
                    theAuthor.Seed(seeder);
                    newIsbn.Authors.Add(theAuthor);
                    context.TheAuthors.Add(theAuthor);
                    context.SaveChanges();
                    isbnId = newIsbn.Id;
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
        /// if a context is passed, that context will be used, and context.SaveChanges is not executed.
        /// </summary>
        /// <param name="seeder"></param>
        /// <param name="newlyCreatedIsbn"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void CreateNewIsbn(csSeedGenerator seeder, out ISBN newlyCreatedIsbn, Context context)
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
                int isbnId = newlyCreatedIsbn.Id;
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
        public bool BorrowBook(int bookId, int customerId, DateTime borrowDate, DateTime returnDate)
        {
            using (Context context = new Context())
            {
                Book? book = context.Books.FirstOrDefault(x => x.Id ==  bookId);
                Customer? customer = context.Customers.FirstOrDefault(x => x.Id == customerId);
                if (book != null && customer != null)
                {
                    book.Borrowed = true;
                    book.BorrowedDate = borrowDate;
                    book.ReturnDate = returnDate;
                    book.Customer = customer;
                    OrderHistory newOrder = new OrderHistory();
                    newOrder.Date = borrowDate;
                    newOrder.Customer = customer;
                    newOrder.Book = book;
                    context.OrderHistories.Add(newOrder);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ReturnBook(int bookId)
        {
            using (Context context = new Context())
            {
                Book? book = context.Books.Include(b => b.Customer).FirstOrDefault(x => x.Id == bookId);
                if (book != null)
                {
                    book.Borrowed = false;
                    book.BorrowedDate = null;
                    book.ReturnDate = null;
                    book.Customer = null;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
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

        public bool DeleteAuthor(int authorId)
        {
            using (Context context = new Context())
            {
                TheAuthor? authorToBeDeleted = context.TheAuthors.Find(authorId);
                if (authorToBeDeleted != null)
                {
                    context.TheAuthors.Remove(authorToBeDeleted);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteIsbn(int isbnId)
        {
            using (var context = new Context())
            {
                var isbnToRemove = context.ISBNs.Find(isbnId);
                if (isbnToRemove != null)
                {
                    context.Remove(isbnToRemove);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Looking at things in database

        public List<OrderHistory> ShowCustomerOrderHistory(int customerId)
        {
            using (Context context = new Context())
            {
                List<OrderHistory> customerHistory = context.OrderHistories.Where(x => x.CustomerId == customerId).ToList();
                return customerHistory;
            }
        }

        public List<OrderHistory> ShowBookOrderHistory(int bookId)
        {
            using (Context context = new Context())
            {
                List<OrderHistory> bookHistory = context.OrderHistories.Where(x => x.BookId == bookId).ToList();
                return bookHistory;
            }
        }

        public bool GetBookTitle(int bookId, out string bookTitle)
        {
            using (Context context = new Context())
            {
                Book? book = context.Books.FirstOrDefault(x => x.Id == bookId);
                ISBN? isbn = context.Books
                    .Include(b => b.ISBN)
                    .Where(b => b.Id == bookId)
                    .Select(b => b.ISBN)
                    .FirstOrDefault();
                if (book != null && isbn != null)
                {
                    bookTitle = isbn.Title;
                    return true;
                }
                else
                {
                    bookTitle = "unknown";
                    return false;
                }
            }
        }

        public bool GetCustomerName(int customerId, out string customerName)
        {
            using (Context context = new Context())
            {
                Customer? customer = context.Customers.FirstOrDefault(x => x.Id == customerId);
                if (customer != null)
                {
                    customerName = customer.FirstName + " " + customer.LastName;
                    return true;
                }
                else
                {
                    customerName = "unknown";
                    return false;
                }
            }
        }

        public List<string> GetAllBooksAsString()
        {
            using (Context context = new Context())
            {
                List<string> allBooks = new List<string>();
                allBooks.Add($"{"isbn",-15} {"Book name",-35} {"Number of copies",-15}");
                foreach (var isbn in context.ISBNs.Include(x => x.Books))
                {
                    int bookCount = isbn.Books?.Count() ?? 0;
                    allBooks.Add($"{isbn.Id, -15} {isbn.Title, -35} {bookCount, -15}");
                }
                return allBooks;
            }
        }

        public List<string> GetAllCustomersAsString()
        {
            using (Context context = new Context())
            {
                List<string> allCustomers = new List<string>();
                allCustomers.Add($"{"Id",-15} {"First name",-15} {"Last name", -15}");
                foreach (var customer in context.Customers)
                {

                    allCustomers.Add($"{customer.Id,-15} {customer.FirstName,-15} {customer.LastName, -15}");
                }
                return allCustomers;
            }
        }

        public List<string> GetAllCopiesOfIsbnAsString(int isbnId)
        {
            using (Context context = new Context())
            {
                List<string> books = new List<string>();
                books.Add($"{"BookId",-15} {"Borrowed to",-25} {"CustomerId", -15}");
                foreach (var book in context.Books
                    .Include(i => i.Customer)
                    .Include(s => s.ISBN)
                    .Where(b => b.ISBN.Id == isbnId))
                {
                    books.Add($"{book.Id,-15} {book.Customer?.FirstName + " " + book.Customer?.LastName,-25} {book.Customer?.Id,-15}");
                }
                return books;
            }
        }

        

        #endregion

    }
}