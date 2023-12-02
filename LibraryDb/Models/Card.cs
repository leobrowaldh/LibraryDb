using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDb.Models
{
    internal class Card
    {
        public int Id { get; set; }
        public int PIN { get; set; }
        public Customer Customer { get; set; }
    }
}
