using Helpers;

namespace LibraryDb.Models
{
    internal class OrderHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public Customer Customer { get; set; }
        public Book Book { get; set; }
    }
}
