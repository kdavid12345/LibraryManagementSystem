# Library Management System

## Overview

This project is a **Console Application** built for managing a library's book collection and lending process. The application features a simple, text-based **User Interface (UI)** that allows administrators to interact with the system.

Developed as part of the .NET Internship 2025 Application for Siemens.

## How to Run the Application

### Prerequisites

- .NET SDK 6.0+ installed  
  Download: https://dotnet.microsoft.com/en-us/download

### Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/kdavid12345/LibraryManagementSystem.git
   ```
2. **Build and run the project**:
   ```bash
   dotnet build
   dotnet run
   ```

## Features

### Persistent Data Storage

- All data is stored persistently using JSON files.
- Three core entities are managed:
  - **Books** – Title, Author, Quantity
  - **Users** – ID, Name
  - **Lendings** – Borrow and Return dates, linked to Users and Books

### Book Management

- Add new books with automatic ID generation
- Update book details
- Delete outdated or damaged books
- Search for books by title and/or author
- View the entire catalog of books

### User Management

- Add users with automatic ID generation
- Update user details
- Remove users
- Search users by name
- View all registered users

### Lending Management

- Users can borrow books (up to 3 active lendings)
- Prevent duplicate lending of the same book by the same user
- Return books and update system state
- Detect and list overdue lendings (over 30 days)
- View all lending records
- View a user’s individual lending history

## Additional Functionality

To go beyond the basic task requirements, several enhancements were implemented to make the system more realistic, useful, and administratively powerful:

### 1. User Management

The system introduces a separate **user registry**, allowing the administrator to store and manage individual users. Each user has a unique ID and a name. This addition enables tracking who borrowed which book, supporting accountability and personal lending history.

### 2. Lending History Tracking

Instead of merely adjusting the quantity of a book upon borrow/return, the application now **stores each lending event** in a separate record. These lending records include:

- The ID of the user
- The ID of the book
- The date of borrowing
- (Optionally) the date of return

This structured approach enables features such as viewing a user's lending history, identifying active or overdue borrowings, and ensuring that lending rules are enforced precisely per user.

### 3. Overdue Lending Detection

The system includes logic to detect **overdue lendings**, identifying any borrowing that exceeds a 30-day period without return. This allows administrators to follow up with users and maintain better circulation of books.

---

These enhancements transform the application from a minimalistic inventory manager into a functional **library management system**. They allow for a more **user-centric workflow**, improve data traceability, and provide a solid foundation for future scalability — such as sending notifications, applying lending limits, or generating reports.

## Data Storage

The application stores data in the following files:

Data/books.json – all book records

Data/users.json - all user records

Data/lendings.json – all lending transactions

These are automatically created if they do not exist on first run.
