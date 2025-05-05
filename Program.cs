using LibraryApp.Models;
using LibraryApp.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new LibraryService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== Library Management System =====");
            Console.WriteLine("1. View all books");
            Console.WriteLine("2. Add new book");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Remove book");
            Console.WriteLine("5. Search books");
            Console.WriteLine("6. Borrow book");
            Console.WriteLine("7. Return book");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    var books = service.GetAllBooks();
                    Console.WriteLine("All the books in the library: ");
                    foreach (var b in books)
                        Console.WriteLine(b);
                    break;

                case "2":
                    Console.Write("Title: ");
                    var title = Console.ReadLine() ?? "";
                    Console.Write("Author: ");
                    var author = Console.ReadLine() ?? "";
                    Console.Write("Quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        var newId = service.GetAllBooks().Any() ? service.GetAllBooks().Max(b => b.Id) + 1 : 1;
                        service.AddBook(new Book
                        {
                            Id = newId,
                            Title = title,
                            Author = author,
                            Quantity = quantity,
                            OriginalQuantity = quantity
                        });
                        Console.WriteLine("Book added.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                    break;

                case "3":
                    Console.Write("Book ID to update: ");
                    if (int.TryParse(Console.ReadLine(), out int updId))
                    {
                        Console.Write("New Title (leave empty to skip): ");
                        var newTitle = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newTitle)) newTitle = null;

                        Console.Write("New Author (leave empty to skip): ");
                        var newAuthor = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newAuthor)) newAuthor = null;

                        Console.Write("New Quantity (leave empty to skip): ");
                        string? newQtyInput = Console.ReadLine();
                        int? newQty = null;
                        if (!string.IsNullOrWhiteSpace(newQtyInput))
                        {
                            if (!int.TryParse(newQtyInput, out int parsedQty))
                            {
                                Console.WriteLine("Invalid quantity.");
                                break;
                            }
                            newQty = parsedQty;
                        }

                        Console.Write("New Original Quantity (leave empty to skip): ");
                        string? newOQtyInput = Console.ReadLine();
                        int? newOQty = null;
                        if (!string.IsNullOrWhiteSpace(newOQtyInput))
                        {
                            if (!int.TryParse(newOQtyInput, out int parsedOQty))
                            {
                                Console.WriteLine("Invalid original quantity.");
                                break;
                            }
                            newOQty = parsedOQty;
                        }

                        if (service.UpdateBook(updId, newTitle, newAuthor, newQty, newOQty))
                            Console.WriteLine("Book updated.");
                        else
                            Console.WriteLine("Book not found.");
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;

                case "4":
                    Console.Write("Book ID to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int remId))
                    {
                        if (service.RemoveBook(remId))
                            Console.WriteLine("Book removed.");
                        else Console.WriteLine("Book not found.");
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;

                case "5":
                    Console.Write("Search by title (leave empty to skip): ");
                    var searchTitle = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(searchTitle)) searchTitle = null;

                    Console.Write("Search by author (leave empty to skip): ");
                    var searchAuthor = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(searchAuthor)) searchAuthor = null;

                    var results = service.SearchBooks(searchTitle, searchAuthor);
                    if (results.Count == 0)
                        Console.WriteLine("No books found.");
                    else
                    {
                        Console.WriteLine("Results:");
                        foreach (var b in results)
                            Console.WriteLine(b);
                    }
                    break;

                case "6":
                    Console.Write("Book ID to borrow: ");
                    if (int.TryParse(Console.ReadLine(), out int borrowId))
                    {
                        if (service.BorrowBook(borrowId))
                            Console.WriteLine("Book borrowed.");
                        else Console.WriteLine("Unavailable or not found.");
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;

                case "7":
                    Console.Write("Book ID to return: ");
                    if (int.TryParse(Console.ReadLine(), out int returnId))
                    {
                        if (service.ReturnBook(returnId))
                            Console.WriteLine("Book returned.");
                        else Console.WriteLine("Cannot return book.");
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;

                case "0":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
