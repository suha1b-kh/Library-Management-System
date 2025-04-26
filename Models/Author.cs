using System.Text.Json.Serialization;

namespace Library_Management_System.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]

        public ICollection<Book> Books { get; set; }
    }
}
