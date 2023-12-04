using Helpers;
using System.Text;

namespace LibraryDb.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public bool Borrowed { get; set; } = false;
        public DateTime? BorrowedDate { get; set; } = null;
        public DateTime? ReturnDate { get; set; } = null;
        public ISBN ISBN { get; set; }
        public Customer? Customer { get; set; } = null;

        
    }
}
