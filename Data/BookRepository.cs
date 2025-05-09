using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class BookRepository
    {
        private readonly string _filePath = "Data/books.json";
        private readonly List<Book> _books;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BookRepository()
        {
            _books = LoadBooks();
            _jsonSerializerOptions = new() { WriteIndented = true };
        }

        // Loads book data from the JSON file
        public List<Book> LoadBooks()
        {
            if (!File.Exists(_filePath))
            {
                return [];
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? [];
        }

        // Saves book data to the JSON file
        public void SaveBooks()
        {
            string json = JsonSerializer.Serialize(_books, _jsonSerializerOptions);
            File.WriteAllText(_filePath, json);
        }

        // Returns all books
        public List<Book> GetAllBooks()
        {
            return _books;
        }

        // Returns the book with the specified ID (or null if not found)
        public Book? GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        // Adds a new book with a unique ID
        public void AddBook(string newTitle, string newAuthor, int newQuantity, int newOriginalQuantity)
        {
            var newId = _books.Count != 0 ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(new Book { Id = newId, Title = newTitle, Author = newAuthor, Quantity = newQuantity, OriginalQuantity = newOriginalQuantity });
            SaveBooks();
        }

        // Removes the book with the specified ID (if found)
        public void RemoveBook(int id)
        {
            var book = GetBookById(id);
            if (book == null) return;

            _books.Remove(book);
            SaveBooks();
        }

        // Updates book details
        public void UpdateBook(int id, string? newTitle, string? newAuthor, int? newQuantity, int? newOriginalQuantity)
        {
            var existing = GetBookById(id);
            if (existing == null) return;

            existing.Title = newTitle ?? existing.Title;
            existing.Author = newAuthor ?? existing.Author;
            existing.Quantity = newQuantity ?? existing.Quantity;
            existing.OriginalQuantity = newOriginalQuantity ?? existing.OriginalQuantity;

            SaveBooks();
        }
    }
}
