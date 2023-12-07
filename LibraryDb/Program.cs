using LibraryDb.Data;
using ConsoleCompanion;
using System.Runtime.InteropServices;

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
                            Console.Clear();
                            string title = cc.AskForString("Book Title: ");
                            int year = cc.AskForInt("Year: ");
                            int rating = cc.AskForInt("Rating (1 - 5): ");
                            if (rating < 1 ||  rating > 5)
                            {
                                Feedback("Rating has to be between 1 and 5");
                            }
                            List<int>authorIds = new List<int>();
                            int numberOfAuthors = cc.AskForInt("How many authors do your book have? ");
                            for (int i = 0; i < numberOfAuthors; i++)
                            {
                                int authorId = cc.AskForInt($"Author ID: ");
                                authorIds.Add(authorId);
                            }
                            int numberOfCopies = cc.AskForInt("Number of copies of the book: ");

                            int IsbnId = dataAccess.CreateNewIsbn(title, year, rating, authorIds, out _);
                            int bookId = dataAccess.CreateCopyOfExistingIsbn(IsbnId, out _);


                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            break;
                        }
                }
            }
            while (selectedOption != -1);
        }

        private static int AuthorCreate(ConsoleCompanionHelper cc, DataAccess dataAccess)
        {
            Console.Clear();
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