using Helpers;
using System.Text;

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

        public void Seed(csSeedGenerator seed)
        {
            StringBuilder isbn = new StringBuilder();
            isbn.Append("ISBN");
            for (int i = 0; i < 10; i++)
            {
                isbn.Append(seed.Next(1, 10));
            }

            Isbn = isbn.ToString();
            Title = seed.MusicAlbum;
            Year = seed.Next(1900, 2023);
            Rating = seed.Next(1, 5);
        }
    }
}
