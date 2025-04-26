namespace Library_Management_System.Models
{
    public class Borrow
    {
        public int BorrowId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public bool IsReturned { get; set; }
    }
}
