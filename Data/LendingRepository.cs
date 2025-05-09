using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class LendingRepository
    {
        private const string FilePath = "Data/lendings.json";

        // Loads lending data from the JSON file
        public List<Lending> LoadLendings()
        {
            if (!File.Exists(FilePath))
                return new List<Lending>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Lending>>(json) ?? new List<Lending>();
        }

        // Saves lending data to the JSON file
        public void SaveLendings(List<Lending> lendings)
        {
            var json = JsonSerializer.Serialize(lendings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}