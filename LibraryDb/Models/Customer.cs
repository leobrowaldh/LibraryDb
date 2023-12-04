using Helpers;

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

        public void Seed(csSeedGenerator seed)
        {
            FirstName = seed.FirstName;
            LastName = seed.LastName;
            Card card = new Card();
            card.Seed(seed);
            Card = card;
        }
    }
}
