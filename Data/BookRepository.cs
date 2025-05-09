using LibraryApp.Models;
using System.Text.Json;

namespace LibraryApp.Data
{
    public class BookRepository
    {
        private readonly string _filePath = "Data/books.json";

        // Loads book data from the JSON file
        public List<Book> LoadBooks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Book>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }

        // Saves book data to the JSON file
        public void SaveBooks(List<Book> books)
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
