using System;
using IDataInterface;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace DataAccess
{
    public class BookManager : IBookManager
    {
        public void AddBook(string title, string author, double cost, long ISBN, int shelfID)
        {
            using var context = new LibraryContext();
            var book = new Book();
            book.ShelfID = shelfID;
            book.Title = title;
            book.ISBN = ISBN;
            book.DateOfPurchase = DateTime.Today;
            book.Condition = 5;
            book.Cost = cost;
            book.Author = author;
            context.Books.Add(book);
            context.SaveChanges();
        }

        public List<Book> GetListOfAllBooks()
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    select b)
                    .ToList();
        }

        public Book GetBookByBookID(int bookID)
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    where b.BookID == bookID
                    select b)
                    .Include(b => b.Borrow)
                    .FirstOrDefault();
        }

        public List<Book> GetListOfBooksByTitle(string title)
        {
            using var context = new LibraryContext();
            return (from b in context.Books
                    where b.Title == title
                    select b)
                    .Include(b => b.Borrow)
                    .ToList();
        }

        public void MoveBook(Book book, int shelfID)
        {
            using var context = new LibraryContext();
            book.ShelfID = shelfID;
            context.SaveChanges();

        }

        public void RemoveBook(Book book)
        {
            using var context = new LibraryContext();
            context.Books.Remove(book);
            context.SaveChanges();
        }

        public void UpdateCondition(Book book, byte newCondition)
        {
            using var context = new LibraryContext();
            book.Condition = newCondition;
            context.SaveChanges();
        }
    }
}
