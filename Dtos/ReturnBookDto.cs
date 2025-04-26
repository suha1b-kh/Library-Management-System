namespace Library_Management_System.Dtos
{
    public class ReturnBookDto
    {
            public int BookId { get; set; }
            public string Title { get; set; }
            public bool IsAvailable { get; set; }
            public string AuthorName { get; set; }
            public string CategoryName { get; set; }
    }
}
