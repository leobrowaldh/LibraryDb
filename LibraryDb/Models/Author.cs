using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDb.Models
{
    internal class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public ICollection<ISBN> ISBNs { get; set; } = new List<ISBN>();
    }
}
