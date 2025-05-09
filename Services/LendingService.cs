using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class LendingService
    {
        private readonly LendingRepository _repository;
        private readonly double _lendPeriodDays;
        private readonly int _maxLendingsPerUser;

        public LendingService()
        {
            _repository = new LendingRepository();
            _lendPeriodDays = 30;
            _maxLendingsPerUser = 3;
        }

        // Returns all lending records
        public List<Lending> GetAllLendings()
        {
            return _repository.GetAllLendings();
        }

        // Returns only lendings that have not been returned yet
        public List<Lending> GetActiveLendings()
        {
            List<Lending> lendings = _repository.GetAllLendings();
            return [.. lendings.Where(l => l.ReturnDate == null)];
        }

        // Returns a list of lendings that are overdue
        public List<Lending> GetOverdueLendings()
        {
            List<Lending> lendings = _repository.GetAllLendings();
            return [.. lendings.Where(l => l.ReturnDate == null && l.BorrowDate < DateTime.Now.AddDays(-_lendPeriodDays))];
        }

        // Returns all lendings made by a specific user
        public List<Lending> GetUserLendings(int userId)
        {
            List<Lending> lendings = _repository.GetAllLendings();
            return [.. lendings.Where(l => l.UserId == userId)];
        }

        // Returns only active (unreturned) lendings made by a specific user
        public List<Lending> GetUserActiveLendings(int userId)
        {
            List<Lending> lendings = _repository.GetAllLendings();
            return [.. lendings.Where(l => l.UserId == userId && l.ReturnDate == null)];
        }

        // Checks if a user is allowed to borrow more books
        public bool CanUserBorrow(int userId)
        {
            return GetUserActiveLendings(userId).Count < _maxLendingsPerUser;
        }

        // Checks if the user already has the specified book borrowed and not returned yet
        public bool IsBookAlreadyBorrowedByUser(int userId, int bookId)
        {
            List<Lending> lendings = _repository.GetAllLendings();
            return lendings.Any(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
        }

        // Tries to borrow a book for a user. Returns true if successful.
        public bool BorrowBook(int userId, int bookId, DateTime borrowDate)
        {
            if (!CanUserBorrow(userId) || IsBookAlreadyBorrowedByUser(userId, bookId))
                return false;

            List<Lending> lendings = _repository.GetAllLendings();
            lendings.Add(new Lending
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = borrowDate,
                ReturnDate = null
            });

            _repository.SaveLendings();
            return true;
        }

        // Marks a book as returned for a user. Returns true if successful.
        public bool ReturnBook(int userId, int bookId)
        {
            List<Lending> lendings = _repository.GetAllLendings();
            var lending = lendings.FirstOrDefault(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
            if (lending == null)
                return false;

            lending.ReturnDate = DateTime.Now;
            _repository.SaveLendings();
            return true;
        }
    }
}
