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

        // Returns all users
        public List<User> GetAllUsers()
        {
            return _users;
        }

        // Returns the user with the specified ID (or null if not found)
        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        // Finds the first user whose name contains the given string
        public User? FindUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        // Adds a new user with a unique ID
        public void AddUser(User user)
        {
            var newId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.Id = newId;
            _users.Add(user);
            SaveChanges();
        }

        // Removes the user with the specified ID (if found)
        public void RemoveUser(int id)
        {
            var user = GetUserById(id);
            if (user == null) return;

            _users.Remove(user);
            SaveChanges();
        }

        // Updates the name of the user with the given ID (if found)
        public void UpdateUser(int id, string newName)
        {
            var user = GetUserById(id);
            if (user == null) return;

            user.Name = newName;
            SaveChanges();
        }

        // Saves current user list to storage
        private void SaveChanges()
        {
            _repository.SaveUsers(_users);
        }
    }
}
