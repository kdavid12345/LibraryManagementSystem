namespace LibraryApp.Models
{
    public class HistoryEntry
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string State { get; set; } = string.Empty; // "Borrowed" or "Returned"
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

}