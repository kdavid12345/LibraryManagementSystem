using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class LibraryService
    {
        private readonly BookRepository _repository;
        private readonly List<Book> _books;

        public LibraryService()
        {
            _repository = new BookRepository();
            _books = _repository.LoadBooks();
        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public Book? GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            _books.Add(book);
            SaveChanges();
        }

        public bool UpdateBook(int id, string? newTitle, string? newAuthor, int? newQuantity, int? newOriginalQuantity)
        {
            var existing = GetBookById(id);
            if (existing == null)
                return false;

            existing.Title = (newTitle != null) ? newTitle : existing.Title;
            existing.Author = (newAuthor != null) ? newAuthor : existing.Author;
            existing.Quantity = (int)((newQuantity != null) ? newQuantity : existing.Quantity);
            existing.OriginalQuantity = (int)((newOriginalQuantity != null) ? newOriginalQuantity : existing.OriginalQuantity);

            SaveChanges();
            return true;
        }

        public bool RemoveBook(int id)
        {
            var book = GetBookById(id);
            if (book == null)
                return false;

            _books.Remove(book);
            SaveChanges();
            return true;
        }

        public List<Book> SearchBooks(string? title, string? author)
        {
            return _books
                .Where(b =>
                    (title == null || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (author == null || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                ).ToList();
        }

        private void SaveChanges()
        {
            _repository.SaveBooks(_books);
        }

        public bool BorrowBook(int id)
        {
            var book = GetBookById(id);
            if (book == null || book.Quantity <= 0)
                return false;

            book.Quantity--;
            SaveChanges();
            return true;
        }

        public bool ReturnBook(int id)
        {
            var book = GetBookById(id);
            if (book == null || book.Quantity >= book.OriginalQuantity)
                return false;

            book.Quantity++;
            SaveChanges();
            return true;
        }
    }
}