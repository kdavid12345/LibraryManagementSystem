namespace LibraryApp.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"User [{Id}]: {Name}";
        }
    }
}
