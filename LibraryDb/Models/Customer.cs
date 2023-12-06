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
        public ICollection<OrderHistory>? OrderHistory { get; set; } = new List<OrderHistory>();


        public Customer() { }
        public Customer(string firstName, string lastName)
        {
            csSeedGenerator seed = new csSeedGenerator();
            FirstName = firstName;
            LastName = lastName;
            Card card = new Card();
            card.Seed(seed);
            Card = card;
        }

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
