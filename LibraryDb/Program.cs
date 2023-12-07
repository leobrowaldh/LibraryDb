using LibraryDb.Data;
using ConsoleCompanion;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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
                "Delete books", "Delete an author", "Show customer orderhistory", "Show book orderhistory", "Show all books in library", 
                "Show all customers"};
            string menuGuideText = "Menu options, select with enter, ESC to Exit\n";
            do
            {
                Console.Clear();
                selectedOption = cc.CreateMenu(menuSelections, menuGuideText, ConsoleColor.Green, true, optionsPerColumn:7, columnSpacing:55);

                switch (selectedOption)
                {
                    case 0:
                        {
                            LibrarySeeding(dataAccess);
                            Feedback("--- Library seeded ---");
                            break;
                        }
                    case 1:
                        {
                            int authorId = AuthorCreate(cc, dataAccess);
                            Feedback($"--- Author Created with Id = {authorId} (remember this id) ---");
                            break;
                        }
                    case 2:
                        {
                            int IsbnId = CreateNewBook(dataAccess, cc);
                            Feedback($"--- New book created with IsbnId = {IsbnId} ---");
                            break;
                        }
                    case 3:
                        {
                            List<int> booksIds = BookCopiesCreate(dataAccess, cc);
                            string bookIdString = string.Join(", ", booksIds);
                            Feedback($"--- Book copies created with Id: {bookIdString} ---");
                            break;
                        }
                    case 4:
                        {
                            string firstName = cc.AskForString("Customer first name: ");
                            string lastName = cc.AskForString("Customer last name: ");
                            dataAccess.CreateNewCustomer(firstName, lastName);
                            Feedback($"--- Customer {firstName} {lastName} created, Personal Library Card created ---");
                            break;
                        }
                    case 5:
                        {
                            //borrow a book

                            break;
                        }
                    case 6:
                        {
                            //Return a book

                            break;
                        }
                    case 7:
                        {
                            //Delete a customer

                            break;
                        }
                    case 8:
                        {
                            //Delete books

                            break;
                        }
                    case 9:
                        {
                            //Delete an author

                            break;
                        }
                    case 10:
                        {
                            //Show customer orderhistory

                            break;
                        }
                    case 11:
                        {
                            //Show book orderhistory

                            break;
                        }
                    case 12:
                        {
                            //Show all books in librar

                            break;
                        }
                    case 13:
                        {
                            //Show all customers

                            break;
                        }
                }
            }
            while (selectedOption != -1);
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
            int rating = cc.AskForInt("Rating (1 - 5): ");
            if (rating < 1 || rating > 5)
            {
                Feedback("Rating has to be between 1 and 5");
            }
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
            Console.WriteLine(message);
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