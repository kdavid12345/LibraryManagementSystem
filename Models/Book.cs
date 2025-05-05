namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int OriginalQuantity { get; set; }
        public int Quantity { get; set; }
        public override string ToString()
        {
            return $"{Id}: \"{Title}\" by {Author} (Quantity: {Quantity})";
        }
    }
}
