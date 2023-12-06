using Helpers;

namespace LibraryDb.Models
{
    internal class OrderHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
