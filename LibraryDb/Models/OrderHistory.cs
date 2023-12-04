using Helpers;

namespace LibraryDb.Models
{
    internal class OrderHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<ISBN> ISBNs { get; set; } = new List<ISBN>();

    }
}
