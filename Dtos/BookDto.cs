namespace Library_Management_System.Dtos
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }
    }
}
