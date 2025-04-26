using System.Text.Json.Serialization;

namespace Library_Management_System.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        [JsonIgnore]
        public Author Author { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]

        public Category Category { get; set; }
        public bool IsAvailable { get; set; }

    }
}
