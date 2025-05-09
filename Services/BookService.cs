using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class BookService
    {
        private readonly BookRepository _repository;

        public BookService()
        {
            _repository = new BookRepository();
        }

        // Returns all books
        public List<Book> GetAllBooks()
        {
            return _repository.GetAllBooks();
        }

        // Returns the book with the specified ID (or null if not found)
        public Book? GetBookById(int id)
        {
            return _repository.GetBookById(id);
        }

        // Adds a new book with a unique ID
        public void AddBook(string newTitle, string newAuthor, int newQuantity, int newOriginalQuantity)
        {
            _repository.AddBook(newTitle, newAuthor, newQuantity, newOriginalQuantity);
        }

        // Removes the book with the specified ID (if found)
        public void RemoveBook(int id)
        {
            _repository.RemoveBook(id);
        }

        // Updates book details
        public void UpdateBook(int id, string? newTitle, string? newAuthor, int? newQuantity, int? newOriginalQuantity)
        {
            _repository.UpdateBook(id, newTitle, newAuthor, newQuantity, newOriginalQuantity);
        }

        // Finds a book by title and author
        public Book? FindBookByTitleAndAuthor(string? title, string? author)
        {
            List<Book> books = _repository.GetAllBooks();
            return books.FirstOrDefault(b =>
                    (string.IsNullOrWhiteSpace(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)));
        }

        // Searches for books by title and/or author
        public List<Book> SearchBooks(string? title, string? author)
        {
            List<Book> books = _repository.GetAllBooks();
            return [.. books
                .Where(b =>
                    (title == null || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (author == null || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)))];
        }
    }
}
