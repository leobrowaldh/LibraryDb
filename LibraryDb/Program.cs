using LibraryDb.Data;

namespace LibraryDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Inicial seeding
            DataAccess dataAccess = new DataAccess();
            dataAccess.SeedLibrary(20, 2, 10);
            dataAccess.SeedCustomers(10);
            #endregion

        }
    }
}