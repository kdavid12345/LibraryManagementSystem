using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class LendingRepository
    {
        private const string FilePath = "Data/lendings.json";
        private readonly List<Lending> _lendings;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public LendingRepository()
        {
            _lendings = LoadLendings();
            jsonSerializerOptions = new() { WriteIndented = true };
        }

        // Loads lending data from the JSON file
        public static List<Lending> LoadLendings()
        {
            if (!File.Exists(FilePath))
                return [];

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Lending>>(json) ?? [];
        }

        // Saves lending data to the JSON file
        public void SaveLendings()
        {
            var json = JsonSerializer.Serialize(_lendings, jsonSerializerOptions);
            File.WriteAllText(FilePath, json);
        }

        // Returns all lending records
        public List<Lending> GetAllLendings()
        {
            return _lendings;
        }
    }
}