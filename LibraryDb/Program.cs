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
            Console.WriteLine("\n------Welcome to your Local Amazing Library!------\n");
            ConsoleCompanionHelper cc = new();
            int selectedOption = -1;
            var menuSelections = new List<string> {"Seed the library with some initial Data", "Create an author", "Create a book", 
            "Create a new customer", "Borrow a book", "Return a book", "Delete a customer", "Delete books", "Delete an author"};
            string menuGuideText = "Menu options, select with enter, ESC to Exit";
            do
            {
                selectedOption = cc.CreateMenu(menuSelections, menuGuideText, ConsoleColor.Green, true);

                switch (selectedOption)
                {
                    case 0:
                        {
                            SeedLibrary(dataAccess);
                            break;
                        }
                    case 1:
                        {
                            
                            break;
                        }
                    case 2:
                        {
                            
                            break;
                        }
                    case 3:
                        {
                            
                            break;
                        }
                }
            }
            while (selectedOption != -1);
        }

        private static void SeedLibrary(DataAccess dataAccess)
        {
            dataAccess.SeedLibrary(20, 2, 10);
            dataAccess.SeedCustomers(10);
            PrintInColor(ConsoleColor.Green, "Library seeded. Can be seeded again if you want more data.");
        }

        private static void PrintInColor(ConsoleColor color, string toPrint)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(toPrint);
            Console.ResetColor();
        }
    }
}