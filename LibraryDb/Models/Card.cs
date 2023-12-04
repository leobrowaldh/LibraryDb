using Helpers;

namespace LibraryDb.Models
{
    internal class Card
    {
        public int Id { get; set; }
        public int PIN { get; set; }
        public Customer Customer { get; set; }

        public void Seed(csSeedGenerator seed)
        {
            PIN = seed.Next(0001, 9999);
        }
    }
}
