namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }

        public Book()
        {
            Title = string.Empty;
            Author = string.Empty;
        }

        public override string ToString()
        {
            return $"{Id}: \"{Title}\" by {Author} (Quantity: {Quantity})";
        }
    }
}
