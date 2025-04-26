namespace Library_Management_System.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
