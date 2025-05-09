using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class UserRepository
    {
        private const string FilePath = "Data/users.json";

        // Loads user data from the JSON file
        public List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        // Saves user data to the JSON file
        public void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
