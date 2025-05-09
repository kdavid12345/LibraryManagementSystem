namespace LibraryApp.Models
{
    public class Book
    {
        public required int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Author { get; set; } = string.Empty;
        public required int OriginalQuantity { get; set; }
        public required int Quantity { get; set; }

        public override string ToString()
        {
            return $"Book [{Id}]: \"{Title}\" by {Author} (Quantity: {Quantity})";
        }
    }
}
