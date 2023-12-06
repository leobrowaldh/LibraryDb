using Helpers;
using System.Text;

namespace LibraryDb.Models
{
    internal class Card
    {
        public int Id { get; set; }
        public string PIN { get; set; }
        public Customer Customer { get; set; }

        public void Seed(csSeedGenerator seed)
        {
            StringBuilder pin = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                pin.Append(seed.Next(0, 10));
            }
            PIN = pin.ToString();
        }
    }
}
