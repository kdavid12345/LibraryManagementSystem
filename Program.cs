using LibraryApp.Models;
using LibraryApp.Services;

class Program
{
    private static BookService _bookService = new BookService();
    private static UserService _userService = new UserService();
    private static LendingService _lendingService = new LendingService();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Library Management System ===\n");
            Console.WriteLine("1. Manage Books");
            Console.WriteLine("2. Manage Users");
            Console.WriteLine("3. Manage Lendings");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1": ManageBooks(); break;
                case "2": ManageUsers(); break;
                case "3": ManageLendings(); break;
                case "0": Console.WriteLine("Exiting..."); return;
                default: ShowError("Invalid menu option."); Wait(); break;
            }
        }
    }

    // Displays the book management menu and handles actions
    static void ManageBooks()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Books ===\n");
            Console.WriteLine("1. View All Books");
            Console.WriteLine("2. Add Book");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Remove Book");
            Console.WriteLine("5. Search Book");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            Console.Clear();
            switch (input)
            {
                case "1":
                    // List all books
                    Console.WriteLine("--- List of all books ---\n");
                    foreach (var bk in _bookService.GetAllBooks())
                        Console.WriteLine(bk);
                    break;
                case "2":
                    // Add a new book
                    Console.WriteLine("--- Adding a book ---\n");
                    Console.Write("Title: ");
                    string? addTitle = Console.ReadLine();
                    Console.Write("Author: ");
                    string? addAuthor = Console.ReadLine();
                    Console.Write("Quantity: ");
                    string? addQtyInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(addTitle) && !string.IsNullOrWhiteSpace(addAuthor) && int.TryParse(addQtyInput, out int addQty))
                    {
                        if (ShowConfirmationMessage("Are you sure you want to add this book?"))
                        {
                            _bookService.AddBook(new Book { Title = addTitle, Author = addAuthor, Quantity = addQty, OriginalQuantity = addQty });
                            Console.WriteLine("Book added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    else
                    {
                        ShowError("Invalid input.");
                        break;
                    }
                    break;
                case "3":
                    // Update book details
                    Console.WriteLine("--- Updating a book ---\n");
                    Console.WriteLine("Current books in the library: ");
                    foreach (var bk in _bookService.GetAllBooks())
                        Console.WriteLine(bk);
                    Console.Write("\nEnter book title to update: ");
                    string? oldTitle = Console.ReadLine();
                    Console.Write("Enter the author (leave empty to skip): ");
                    string? oldAuthor = Console.ReadLine();
                    var bookToUpdate = _bookService.FindBookByTitleAndAuthor(oldTitle, oldAuthor);
                    if (bookToUpdate == null)
                    {
                        ShowError("Book not found.");
                        break;
                    }
                    Console.WriteLine("Book found: " + bookToUpdate);
                    Console.Write("New Title (leave empty to skip): ");
                    string? newTitle = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newTitle)) newTitle = null;
                    Console.Write("New Author (leave empty to skip): ");
                    string? newAuthor = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newAuthor)) newAuthor = null;
                    Console.Write("New Quantity (leave empty to skip): ");
                    var newQtyInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newQtyInput))
                    {
                        if (int.TryParse(newQtyInput, out int newQty) && newQty >= 0)
                        {
                            if (ShowConfirmationMessage("Are you sure you want to update this book?"))
                            {
                                // Adjust original quantity to preserve lending consistency
                                int activeLendingCount = _lendingService.GetActiveLendings().Count(l => l.BookId == bookToUpdate.Id);
                                _bookService.UpdateBook(bookToUpdate.Id, newTitle, newAuthor, newQty, newQty + activeLendingCount);
                                Console.WriteLine("Book updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Operation cancelled.");
                            }
                        }
                        else
                        {
                            ShowError("Invalid input.");
                            break;
                        }
                    }
                    else
                    {
                        if (ShowConfirmationMessage("Are you sure you want to update this book?"))
                        {
                            newQtyInput = null;
                            _bookService.UpdateBook(bookToUpdate.Id, newTitle, newAuthor, null, null);
                            Console.WriteLine("Book updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    break;
                case "4":
                    // Remove a book 
                    Console.WriteLine("--- Removing a book ---\n");
                    Console.WriteLine("Current books in the library: ");
                    foreach (var bk in _bookService.GetAllBooks())
                        Console.WriteLine(bk);
                    Console.Write("Enter book title to remove: ");
                    string? removeTitle = Console.ReadLine();
                    Console.Write("Enter the author (leave empty to skip): ");
                    string? removeAuthor = Console.ReadLine();
                    var bookToRemove = _bookService.FindBookByTitleAndAuthor(removeTitle, removeAuthor);
                    if (bookToRemove != null)
                    {
                        Console.WriteLine("Book found: " + bookToRemove);
                        if (ShowConfirmationMessage("Are you sure you want to remove this book?"))
                        {
                            _bookService.RemoveBook(bookToRemove.Id);
                            Console.WriteLine("Book removed successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    else ShowError("Book not found.");
                    break;
                case "5":
                    // Search for books based on filters
                    Console.WriteLine("--- Searching books ---\n");
                    Console.Write("Search by title (leave empty to skip): ");
                    string? titleFilter = Console.ReadLine();
                    Console.Write("Search by author (leave empty to skip): ");
                    string? authorFilter = Console.ReadLine();
                    var searchMatchBooks = _bookService.SearchBooks(titleFilter, authorFilter);
                    foreach (var b in searchMatchBooks)
                        Console.WriteLine(b);
                    break;
                case "0": return;
                default: ShowError("Invalid menu option."); break;
            }
            Wait();
        }
    }

    // Displays the user management menu and handles actions
    static void ManageUsers()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Users ===\n");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. Add User");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Remove User");
            Console.WriteLine("5. Search User");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            Console.Clear();
            switch (input)
            {
                case "1":
                    // List all registered users
                    Console.WriteLine("--- List of all users ---\n");
                    foreach (var usr in _userService.GetAllUsers())
                        Console.WriteLine(usr);
                    break;
                case "2":
                    // Add a new user
                    Console.WriteLine("--- Adding a user ---\n");
                    Console.Write("User Name: ");
                    string? addName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(addName))
                    {
                        if (ShowConfirmationMessage("Are you sure you want to add this user?"))
                        {
                            _userService.AddUser(new User { Name = addName });
                            Console.WriteLine("User added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    else ShowError("Invalid input!");
                    break;
                case "3":
                    // Update an existing user's name
                    Console.WriteLine("--- Updating a user ---\n");
                    Console.Write("Enter user name to update: ");
                    string? oldName = Console.ReadLine();
                    Console.Write("New Name: ");
                    string? newName = Console.ReadLine();
                    var user = _userService.FindUserByName(oldName ?? "");
                    if (user != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            if (ShowConfirmationMessage("Are you sure you want to update this user?"))
                            {
                                _userService.UpdateUser(user.Id, newName);
                                Console.WriteLine("User updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Operation cancelled.");
                            }
                        }
                        else ShowError("Invalid input.");
                    }
                    else ShowError("User not found.");
                    break;
                case "4":
                    // Remove a user
                    Console.WriteLine("--- Removing a user ---\n");
                    Console.Write("Enter user name to remove: ");
                    string? removeName = Console.ReadLine();
                    var u = _userService.FindUserByName(removeName ?? "");
                    if (u != null)
                    {
                        if (ShowConfirmationMessage("Are you sure you want to remove this user?"))
                        {
                            _userService.RemoveUser(u.Id);
                            Console.WriteLine("User removed successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    else ShowError("User not found.");
                    break;
                case "5":
                    // Search for users by name filter
                    Console.WriteLine("--- Finding a user ---\n");
                    Console.Write("Search by name: ");
                    string? nameFilter = Console.ReadLine();
                    var searchMatchUser = _userService.FindUserByName(nameFilter ?? "");
                    if (searchMatchUser != null)
                        Console.WriteLine(searchMatchUser);
                    else ShowError("User not found.");
                    break;
                case "0": return;
                default: ShowError("Invalid menu option."); break;
            }
            Wait();
        }
    }

    // Displays the lending management menu and handles actions
    static void ManageLendings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Lendings ===\n");
            Console.WriteLine("1. View All Lendings");
            Console.WriteLine("2. View Active Lendings");
            Console.WriteLine("3. View Overdue Lendings");
            Console.WriteLine("4. View Lendings Of a User");
            Console.WriteLine("5. Borrow Book");
            Console.WriteLine("6. Return Book");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            Console.Clear();
            switch (input)
            {
                case "1":
                    // Display all lendings
                    Console.WriteLine("--- List of all lendings ---\n");
                    foreach (var lending in _lendingService.GetAllLendings())
                        PrintLending(lending);
                    break;
                case "2":
                    // Display all currently active lendings
                    Console.WriteLine("--- List of active lendings ---\n");
                    foreach (var lending in _lendingService.GetActiveLendings())
                        PrintLending(lending);
                    break;
                case "3":
                    // Display all overdue not returned lendings
                    Console.WriteLine("--- List of overdue lendings ---\n");
                    foreach (var lending in _lendingService.GetOverdueLendings())
                        PrintLending(lending);
                    break;
                case "4":
                    // Display all lendings of a user
                    Console.WriteLine("--- Lendings of a user ---\n");
                    Console.WriteLine("User name: ");
                    string? userName = Console.ReadLine();
                    if (userName != null)
                    {
                        var chosenUser = _userService.FindUserByName(userName);
                        if (chosenUser != null)
                        {
                            foreach (var lending in _lendingService.GetUserLendings(chosenUser.Id))
                                PrintLending(lending);
                        }
                        else
                        {
                            ShowError("User not found.");
                        }
                    }
                    else
                    {
                        ShowError("Invalid input.");
                    }
                    break;
                case "5":
                    // Borrowing a book
                    Console.WriteLine("--- Borrowing a book ---\n");
                    Console.Write("User Name: ");
                    string? borrowerName = Console.ReadLine();
                    Console.Write("Book Title: ");
                    string? bookTitle = Console.ReadLine();
                    Console.Write("Book Author (leave empty to skip): ");
                    string? bookAuthor = Console.ReadLine();
                    var borrower = _userService.FindUserByName(borrowerName ?? "");
                    var bookToBorrow = _bookService.FindBookByTitleAndAuthor(bookTitle, bookAuthor);
                    if (borrower != null && bookToBorrow != null)
                    {
                        Console.WriteLine($"Preview: {borrower} borrows {bookToBorrow}");
                        if (ShowConfirmationMessage("Are you sure you want to lend this book?"))
                        {
                            if (_lendingService.BorrowBook(borrower.Id, bookToBorrow.Id, DateTime.Now))
                                Console.WriteLine("Book borrowed successfully.");
                            else
                                ShowError("Borrow failed. User may have too many active lendings or already borrowed this book.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }
                    }
                    else ShowError("User or Book not found.");
                    break;
                case "6":
                    // Returning a book
                    Console.WriteLine("--- Returning a book ---\n");
                    Console.Write("User Name: ");
                    string? returnerName = Console.ReadLine();
                    Console.Write("Book Title: ");
                    string? returnBookTitle = Console.ReadLine();
                    Console.Write("Book Author (leave empty to skip): ");
                    string? returnBookAuthor = Console.ReadLine();
                    var returner = _userService.FindUserByName(returnerName ?? "");
                    var bookToReturn = _bookService.FindBookByTitleAndAuthor(returnBookTitle, returnBookAuthor);
                    if (returner != null && bookToReturn != null)
                    {
                        Console.WriteLine($"Preview: {returner} returns {bookToReturn}");
                        if (ShowConfirmationMessage("Are you sure you want to return this book?"))
                        {
                            if (_lendingService.ReturnBook(returner.Id, bookToReturn.Id))
                                Console.WriteLine("Book returned successfully.");
                            else
                                ShowError("Return failed. No active lending found.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }

                    }
                    else ShowError("User or Book not found.");
                    break;
                case "0": return;
                default: ShowError("Invalid menu option."); break;
            }
            Wait();
        }
    }

    // Prints a lending
    static void PrintLending(Lending lending)
    {
        var user = _userService.GetUserById(lending.UserId);
        var book = _bookService.GetBookById(lending.BookId);
        string status = lending.ReturnDate == null ? "Not Returned" : $"Returned on {lending.ReturnDate.Value:yyyy-MM-dd}";
        Console.WriteLine($"User: {user?.Name} | Book: {book?.Title} | Borrowed: {lending.BorrowDate:yyyy-MM-dd} | {status}");
    }

    // Displays an error message
    static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Displays a confirmation prompt
    public static bool ShowConfirmationMessage(string message)
    {
        while (true)
        {
            Console.WriteLine($"{message} (y/n): ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y" || input == "yes")
                return true;
            else if (input == "n" || input == "no")
                return false;
            else
                Console.WriteLine("Please enter 'y' or 'n'.");
        }
    }

    // Pauses the execution until the user presses a key
    static void Wait()
    {
        Console.WriteLine("\nPress ENTER to continue...");
        Console.ReadLine();
    }
}
