using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly List<User> _users;

        public UserService()
        {
            _repository = new UserRepository();
            _users = _repository.LoadUsers();
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void AddUser(User user)
        {
            var newId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.Id = newId;
            _users.Add(user);
            SaveChanges();
        }

        public bool RemoveUser(int id)
        {
            var user = GetUserById(id);
            if (user == null)
                return false;

            _users.Remove(user);
            SaveChanges();
            return true;
        }

        public bool UpdateUser(int id, string newName)
        {
            var user = GetUserById(id);
            if (user == null)
                return false;

            user.Name = newName;
            SaveChanges();
            return true;
        }

        private void SaveChanges()
        {
            _repository.SaveUsers(_users);
        }
    }
}
