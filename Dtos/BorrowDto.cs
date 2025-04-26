namespace Library_Management_System.Dtos
{
    public class BorrowDto
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public bool IsReturned { get; set; }
    }
}
