using Helpers;

namespace LibraryDb.Models
{
    internal class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public ICollection<ISBN> ISBNs { get; set; } = new List<ISBN>();

        public void Seed(csSeedGenerator seed)
        {
            AuthorName = seed.FirstName + seed.LastName;
        }
    }
}
