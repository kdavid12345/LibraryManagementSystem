using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class LendingService
    {
        private readonly LendingRepository _repository;
        private readonly List<Lending> _lendings;
        private readonly double _lendPeriodDays;
        private readonly int _maxLendingsPerUser;

        public LendingService()
        {
            _repository = new LendingRepository();
            _lendings = _repository.LoadLendings();
            _lendPeriodDays = 30;
            _maxLendingsPerUser = 3;
        }

        // Returns all lending records
        public List<Lending> GetAllLendings()
        {
            return _lendings;
        }

        // Returns only lendings that have not been returned yet
        public List<Lending> GetActiveLendings()
        {
            return _lendings.Where(l => l.ReturnDate == null).ToList();
        }

        // Returns a list of lendings that are overdue
        public List<Lending> GetOverdueLendings()
        {
            return _lendings
                .Where(l => l.ReturnDate == null && l.BorrowDate < DateTime.Now.AddDays(-_lendPeriodDays))
                .ToList();
        }

        // Returns all lendings made by a specific user
        public List<Lending> GetUserLendings(int userId)
        {
            return _lendings.Where(l => l.UserId == userId).ToList();
        }

        // Returns only active (unreturned) lendings made by a specific user
        public List<Lending> GetUserActiveLendings(int userId)
        {
            return _lendings.Where(l => l.UserId == userId && l.ReturnDate == null).ToList();
        }

        // Checks if a user is allowed to borrow more books
        public bool CanUserBorrow(int userId)
        {
            return GetUserActiveLendings(userId).Count < _maxLendingsPerUser;
        }

        // Checks if the user already has the specified book borrowed and not returned yet
        public bool IsBookAlreadyBorrowedByUser(int userId, int bookId)
        {
            return _lendings.Any(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
        }

        // Tries to borrow a book for a user. Returns true if successful.
        public bool BorrowBook(int userId, int bookId, DateTime borrowDate)
        {
            if (!CanUserBorrow(userId) || IsBookAlreadyBorrowedByUser(userId, bookId))
                return false;

            _lendings.Add(new Lending
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = borrowDate,
                ReturnDate = null
            });

            SaveChanges();
            return true;
        }

        // Marks a book as returned for a user. Returns true if successful.
        public bool ReturnBook(int userId, int bookId)
        {
            var lending = _lendings.FirstOrDefault(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
            if (lending == null)
                return false;

            lending.ReturnDate = DateTime.Now;
            SaveChanges();
            return true;
        }

        // Saves current lending list to storage
        public void SaveChanges()
        {
            _repository.SaveLendings(_lendings);
        }
    }
}
