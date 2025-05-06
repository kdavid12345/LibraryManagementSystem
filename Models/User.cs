namespace LibraryApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Book> BorrowedBooks { get; set; } = new List<Book>();

        public override string ToString()
        {
            return $"User [{Id}]: {Name}";
        }
    }
}
