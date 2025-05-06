using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class HistoryRepository
    {
        private const string FilePath = "Data/history.json";

        public List<HistoryEntry> LoadHistory()
        {
            if (!File.Exists(FilePath))
                return new List<HistoryEntry>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<HistoryEntry>>(json) ?? new List<HistoryEntry>();
        }

        public void SaveHistory(List<HistoryEntry> history)
        {
            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}