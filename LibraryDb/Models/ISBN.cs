using Microsoft.EntityFrameworkCore;

namespace LibraryDb.Models
{
    internal class ISBN
    {
        public int Id { get; set; }
        public string Isbn {  get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}
