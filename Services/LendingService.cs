using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class LendingService
    {
        private readonly LendingRepository _repository;
        private readonly List<Lending> _lendings;
        private readonly double _lendPeriodDays;

        public LendingService()
        {
            _repository = new LendingRepository();
            _lendings = _repository.LoadLendings();
            _lendPeriodDays = 30;
        }

        public List<Lending> GetAllLendings()
        {
            return _lendings;
        }

        public List<Lending> GetActiveLendings()
        {
            return _lendings.Where(l => l.ReturnDate == null).ToList();
        }

        public List<Lending> GetUserLendings(int userId)
        {
            return _lendings.Where(l => l.UserId == userId).ToList();
        }

        public List<Lending> GetUserActiveLendings(int userId)
        {
            return _lendings.Where(l => l.UserId == userId && l.ReturnDate == null).ToList();
        }

        public bool CanUserBorrow(int userId)
        {
            return GetUserActiveLendings(userId).Count < 3;
        }

        public bool IsBookAlreadyBorrowedByUser(int userId, int bookId)
        {
            return _lendings.Any(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
        }

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

        public bool ReturnBook(int userId, int bookId)
        {
            var lending = _lendings.FirstOrDefault(l => l.UserId == userId && l.BookId == bookId && l.ReturnDate == null);
            if (lending == null)
                return false;

            lending.ReturnDate = DateTime.Now;
            SaveChanges();
            return true;
        }

        public List<Lending> GetOverdueLendings()
        {
            return _lendings
                .Where(l => l.ReturnDate == null && l.BorrowDate < DateTime.Now.AddDays(-_lendPeriodDays))
                .ToList();
        }

        public void SaveChanges()
        {
            _repository.SaveLendings(_lendings);
        }
    }
}
