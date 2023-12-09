using LibraryDb.Data;
using ConsoleCompanion;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using LibraryDb.Models;

namespace LibraryDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataAccess dataAccess = new DataAccess();
            RunMenu(dataAccess);
        }

        private static void RunMenu(DataAccess dataAccess)
        {
            
            ConsoleCompanionHelper cc = new();
            int selectedOption = -1;
            var menuSelections = new List<string> {"Seed the library with some initial Data", "Create an author", "Create a book", 
            "Add more copies to an allready existing book", "Create a new customer", "Borrow a book", "Return a book", "Delete a customer", 
                "Delete a book copy", "Delete an author", "Show customer orderhistory", "Show book orderhistory", "Show all books in library", 
                "Show all customers", "Show all copies of a book", "Delete all copies of a book"};
            string menuGuideText = "Menu options, select with enter, ESC to Exit\n";
            do
            {
                Console.Clear();
                selectedOption = cc.CreateMenu(menuSelections, menuGuideText, ConsoleColor.Green, true, optionsPerColumn:8, columnSpacing:55);

                switch (selectedOption)
                {
                    case 0:
                        {
                            //Seed the library with some initial Data
                            LibrarySeeding(dataAccess);
                            Feedback("--- Library seeded ---");
                            break;
                        }
                    case 1:
                        {
                            //Create an author
                            Console.Clear();
                            int authorId = AuthorCreate(cc, dataAccess);
                            Feedback($"--- Author Created with Id = {authorId} (remember this id) ---");
                            break;
                        }
                    case 2:
                        {
                            //Create a book
                            Console.Clear();
                            int IsbnId = CreateNewBook(dataAccess, cc);
                            Feedback($"--- New book created with IsbnId = {IsbnId} ---");
                            break;
                        }
                    case 3:
                        {
                            //Add more copies to an allready existing book
                            Console.Clear();
                            List<int> booksIds = BookCopiesCreate(dataAccess, cc);
                            string bookIdString = string.Join(", ", booksIds);
                            Feedback($"--- Book copies created with Id: {bookIdString} ---");
                            break;
                        }
                    case 4:
                        {
                            //Create a new customer
                            Console.Clear();
                            string firstName = cc.AskForString("Customer first name: ");
                            string lastName = cc.AskForString("Customer last name: ");
                            dataAccess.CreateNewCustomer(firstName, lastName);
                            Feedback($"--- Customer {firstName} {lastName} created, Personal Library Card created ---");
                            break;
                        }
                    case 5:
                        {
                            //borrow a book
                            Console.Clear();
                            int bookId = BookBorrowing(dataAccess, cc, out int customerId);
                            if(bookId == -1)
                            {
                                FeedbackRed("book or customer not found in database, try again.");
                                break;
                            }
                            else
                            {
                                Feedback($"--- Book {bookId} borrowed to customer {customerId} ---");
                                break;
                            }
                        }
                    case 6:
                        {
                            //Return a book
                            Console.Clear();
                            int bookId = cc.AskForInt("BookId: ", "Try again");
                            bool sucess = dataAccess.ReturnBook(bookId);
                            if (!sucess)
                            {
                                FeedbackRed("Book not found");
                                break;
                            }
                            else
                            {
                                Feedback($"Book {bookId} returned");
                                break;
                            }
                        }
                    case 7:
                        {
                            //Delete a customer
                            Console.Clear();
                            int customerId = cc.AskForInt("CustomerId: ", "Try again");
                            bool success = dataAccess.DeleteCustomer(customerId);
                            if (success)
                            {
                                Feedback("Customer Deleter");
                                break;
                            }
                            else
                            {
                                FeedbackRed("Customer not found");
                                break;
                            }
                            
                        }
                    case 8: //TODO: this is only deleting a book instance, change to be able to delete a number of copies, or the whole isbn
                        {
                            //Delete a book copy
                            int bookId = cc.AskForInt("BookId: ", "try again");
                            bool success = dataAccess.DeleteBook(bookId);
                            if (success)
                            {
                                Feedback($"Book {bookId} removed from database");
                                break;
                            }
                            else
                            {
                                FeedbackRed("Book not found");
                                break;
                            }
                            
                        }
                    case 9:
                        {
                            //Delete an author
                            Console.Clear();
                            int authorId = cc.AskForInt("AuthorId: ", "try again");
                            bool success = dataAccess.DeleteAuthor(authorId);
                            if (success)
                            {
                                Feedback($"Author {authorId} removed from database");
                                break;
                            }
                            else
                            {
                                FeedbackRed("Author not found");
                                break;
                            }
                        }
                    case 10:
                        {
                            //Show customer orderhistory
                            Console.Clear();
                            int customerId = cc.AskForInt("CustomerId: ", "Try again");
                            List<OrderHistory> customerHistory = dataAccess.ShowCustomerOrderHistory(customerId);
                            if (!customerHistory.Any())
                            {
                                FeedbackRed("No history for this customer");
                                break;
                            }
                            StringBuilder sb = new StringBuilder();
                            sb.Append($"{"Date", -25} {"BookId", -15} {"Title", -15}\n");
                            foreach (OrderHistory entry in customerHistory)
                            {
                                DateTime date = entry.Date;
                                int bookId = entry.BookId;
                                bool success = dataAccess.GetBookTitle(bookId, out string title);
                                if (!success)
                                {
                                    continue;
                                }
                                sb.Append($"{date, -25} {bookId,-15} {title,-15}\n");
                            }
                            Feedback(sb.ToString());
                            break;
                        }
                    case 11:
                        {
                            //Show book orderhistory
                            Console.Clear();
                            int bookId = cc.AskForInt("BookId: ", "Try again");
                            List<OrderHistory> bookHistory = dataAccess.ShowBookOrderHistory(bookId);
                            if (!bookHistory.Any())
                            {
                                FeedbackRed("No history for this book");
                                break;
                            }
                            StringBuilder sb = new StringBuilder();
                            sb.Append($"{"Date",-25} {"CustomerId",-15} {"Customer Name",-15}\n");
                            foreach (OrderHistory entry in bookHistory)
                            {
                                DateTime date = entry.Date;
                                int customerId = entry.CustomerId;
                                bool success = dataAccess.GetCustomerName(customerId, out string customerName);
                                if (!success)
                                {
                                    continue;
                                }
                                sb.Append($"{date, -25} {customerId,-15} {customerName,-15}\n");
                            }
                            Feedback(sb.ToString());
                            break;
                        }
                    case 12:
                        {
                            //Show all books in library
                            Console.Clear();
                            List<string> allBooks = dataAccess.GetAllBooksAsString();
                            StringBuilder sb = new StringBuilder();
                            foreach (string row in allBooks)
                            {
                                sb.Append(row + "\n");
                            }
                            Feedback(sb.ToString());
                            break;
                        }
                    case 13:
                        {
                            //Show all customers
                            Console.Clear();
                            List<string> allCustomers = dataAccess.GetAllCustomersAsString();
                            StringBuilder sb = new StringBuilder();
                            foreach (string row in allCustomers)
                            {
                                sb.Append(row + "\n");
                            }
                            Feedback(sb.ToString());
                            break;
                        }
                    case 14:
                        {
                            //Show all copies of a book
                            //bookid, leant or not, customer leant to...
                            Console.Clear();
                            int isbn = cc.AskForInt("IsbnId: ", "Try again");
                            List<string> books = dataAccess.GetAllCopiesOfIsbnAsString(isbn);
                            StringBuilder sb = new StringBuilder();
                            foreach (string row in books)
                            {
                                sb.Append(row + "\n");
                            }
                            Feedback(sb.ToString());
                            break;
                        }
                    case 15:
                        {
                            Console.Clear();
                            //Delete all copies of a book
                            int isbnId = cc.AskForInt("IsbnId: ", "Try again");
                            bool success = dataAccess.DeleteIsbn(isbnId);
                            if (success)
                            {
                                Feedback($"Isbn ID = {isbnId} completely removed from database with all its copies.");
                            }
                            else
                            {
                                FeedbackRed("Isbn not found");
                            }
                            break;
                        }
                }
            }
            while (selectedOption != -1);
        }

        private static int BookBorrowing(DataAccess dataAccess, ConsoleCompanionHelper cc, out int customerId)
        {
            int bookId = cc.AskForInt("Book Id: ");
            customerId = cc.AskForInt("Customer Id: ");
            //parsing correct DateTimes:
            bool borrowDateCorrect;
            bool returnDateCorrect;
            DateTime borrowDate;
            DateTime returnDate = DateTime.Now;
            do
            {
                string borrowDatestring = cc.AskForString("borrow date (yyyy-mm-dd): ");
                borrowDateCorrect = DateTime.TryParse(borrowDatestring, out borrowDate);
                if (borrowDateCorrect)
                {   //the book should be returned in 30 days.
                    returnDate = borrowDate.AddDays(30);
                }
            }
            while (!borrowDateCorrect);
            bool success = dataAccess.BorrowBook(bookId, customerId, borrowDate, returnDate);
            if (success)
            {
                return bookId;
            }
            else
            {
                return -1;
            }
            
        }

        private static List<int> BookCopiesCreate(DataAccess dataAccess, ConsoleCompanionHelper cc)
        {
            int isbnId = cc.AskForInt("IsbnId of book: ", "Please enter a valid IsdnID");
            int numberOfCopies = cc.AskForInt("Number of copies to add: ", "Invalid input");
            List<int> booksIds = CreateCopiesOfIsbn(dataAccess, numberOfCopies, isbnId);
            return booksIds;
        }

        private static int CreateNewBook(DataAccess dataAccess, ConsoleCompanionHelper cc)
        {
            string title = cc.AskForString("Book Title: ");
            int year = cc.AskForInt("Year: ");
            int rating;
            do
            {
                rating = cc.AskForInt("Rating (1 - 5): ");
                if (rating < 1 || rating > 5)
                {
                    Feedback("Rating has to be between 1 and 5");
                }
            }
            while (rating < 1 || rating > 5);
            List<int> authorIds = new List<int>();
            int numberOfAuthors = cc.AskForInt("How many authors do your book have? ");
            for (int i = 0; i < numberOfAuthors; i++)
            {
                int authorId = cc.AskForInt($"Author ID: ");
                authorIds.Add(authorId);
            }
            int numberOfCopies = cc.AskForInt("Number of copies of the book: ");
            int IsbnId = dataAccess.CreateNewIsbn(title, year, rating, authorIds, out _);
            CreateCopiesOfIsbn(dataAccess, numberOfCopies, IsbnId);
            return IsbnId;
        }

        private static List<int> CreateCopiesOfIsbn(DataAccess dataAccess, int numberOfCopies, int IsbnId)
        {
            List<int> bookIds = new List<int>();
            for (int i = 0; i < numberOfCopies; i++)
            {
                int bookId = dataAccess.CreateCopyOfExistingIsbn(IsbnId, out _);
                bookIds.Add(bookId);
            }
            return bookIds;
        }

        private static int AuthorCreate(ConsoleCompanionHelper cc, DataAccess dataAccess)
        {
            string authorName = cc.AskForString("Enter the name of the new author ");
            int authorId = dataAccess.CreateAuthor(authorName, out _);
            return authorId;
        }

        private static void Feedback(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("press any key to continue...");
            Console.ReadKey(true);
        }

        private static void FeedbackRed(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("press any key to continue...");
            Console.ReadKey(true);
        }

        private static void LibrarySeeding(DataAccess dataAccess)
        {
            dataAccess.SeedLibrary(20, 2, 10);
            dataAccess.SeedCustomers(10);
        }
    }
}