using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class LendingRepository
    {
        private const string FilePath = "Data/lendings.json";

        public List<Lending> LoadLendings()
        {
            if (!File.Exists(FilePath))
                return new List<Lending>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Lending>>(json) ?? new List<Lending>();
        }

        public void SaveLendings(List<Lending> lendings)
        {
            var json = JsonSerializer.Serialize(lendings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}