using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class BookService
    {
        private readonly BookRepository _repository;
        private readonly List<Book> _books;

        public BookService()
        {
            _repository = new BookRepository();
            _books = _repository.LoadBooks();
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

        // Finds a book by title and author
        public Book? FindBookByTitleAndAuthor(string? title, string? author)
        {
            return _books.FirstOrDefault(b =>
                (string.IsNullOrWhiteSpace(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)));
        }

        // Adds a new book with a unique ID
        public void AddBook(Book book)
        {
            book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(book);
            SaveChanges();
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

            SaveChanges();
        }

        // Removes the book with the specified ID (if found)
        public void RemoveBook(int id)
        {
            var book = GetBookById(id);
            if (book == null) return;

            _books.Remove(book);
            SaveChanges();
        }

        // Searches for books by title and/or author
        public List<Book> SearchBooks(string? title, string? author)
        {
            return _books
                .Where(b =>
                    (title == null || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (author == null || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        // Saves current book list to storage
        private void SaveChanges()
        {
            _repository.SaveBooks(_books);
        }
    }
}
