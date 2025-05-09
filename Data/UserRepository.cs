using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class UserRepository
    {
        private const string FilePath = "Data/users.json";
        private readonly List<User> _users;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        public UserRepository()
        {
            _users = LoadUsers();
        }

        // Loads user data from the JSON file
        public static List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
                return [];

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? [];
        }

        // Saves user data to the JSON file
        public void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, jsonSerializerOptions);
            File.WriteAllText(FilePath, json);
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

        // Adds a new user with a unique ID
        public void AddUser(string newName)
        {
            List<User> users = GetAllUsers();
            var newId = _users.Count != 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(new User { Id = newId, Name = newName });
            SaveUsers(_users);
        }

        // Removes the user with the specified ID (if found)
        public void RemoveUser(int id)
        {
            var user = GetUserById(id);
            if (user == null) return;

            _users.Remove(user);
            SaveUsers(_users);
        }

        // Updates the name of the user with the given ID (if found)
        public void UpdateUser(int id, string newName)
        {
            var user = GetUserById(id);
            if (user == null) return;

            user.Name = newName;
            SaveUsers(_users);
        }
    }
}
