using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDb.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CardId { get; set; }
        public Card? Card { get; set; }
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
