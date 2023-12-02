using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDb.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public bool Borrowed { get; set; }
        public DateTime? BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public ISBN ISBN { get; set; }
        public Customer? Customer { get; set; }
    }
}
