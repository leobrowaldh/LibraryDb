using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDb.Models
{
    internal class OrderHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<ISBN> ISBNs { get; set; }
    }
}
