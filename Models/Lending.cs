namespace LibraryApp.Models
{
    public class Lending
    {
        public required int UserId { get; set; }
        public required int BookId { get; set; }
        public required DateTime BorrowDate { get; set; }
        public required DateTime? ReturnDate { get; set; }
    }
}
