using Helpers;

namespace LibraryDb.Models
{
    internal class OrderHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }
        public Book Book { get; set; }

    }
}
