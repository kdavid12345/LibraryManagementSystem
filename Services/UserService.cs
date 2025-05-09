using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService()
        {
            _repository = new UserRepository();
        }

        // Returns all users
        public List<User> GetAllUsers()
        {
            return _repository.GetAllUsers();
        }

        // Returns the user with the specified ID (or null if not found)
        public User? GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        // Adds a new user with a unique ID
        public void AddUser(string newName)
        {
            _repository.AddUser(newName);
        }

        // Removes the user with the specified ID (if found)
        public void RemoveUser(int id)
        {
            _repository.RemoveUser(id);
        }

        // Updates the name of the user with the given ID (if found)
        public void UpdateUser(int id, string newName)
        {
            _repository.UpdateUser(id, newName);
        }

        // Finds the first user whose name contains the given string
        public User? FindUserByName(string name)
        {
            List<User> users = _repository.GetAllUsers();
            return users.FirstOrDefault(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
