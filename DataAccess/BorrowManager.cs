using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BorrowManager : IBorrowManager
    {
        public void AddBorrow(Customer customer, Book book)
        {
            using var context = new LibraryContext();
            var borrow = new Borrow();
            borrow.BookID = book.BookID;
            borrow.CustumerID = customer.CustomerID;
            borrow.DateOfBorrow = DateTime.Now;
            context.Borrows.Add(borrow);
            context.SaveChanges();
        }

        public List<Borrow> GetAllBorrows()
        {
            using var context = new LibraryContext();
            return (from b in context.Borrows
                    select b)
                    .Include(b => b.Customer)
                    .Include(b => b.Book)
                    .ToList();
        }

        public Borrow GetBorrow(int borrowID)
        {
            using var context = new LibraryContext();
            return (from b in context.Borrows
                    where b.BorrowID == borrowID
                    select b)
                    .Include(b => b.Customer)
                    .FirstOrDefault();
        }

        public Borrow GetBorrowByBookID(int bookID)
        {
            using var context = new LibraryContext();
            return (from b in context.Borrows
                    where b.BookID == bookID
                    select b)
                    .Include(b => b.Bill)
                    .First();
        }

        public void ReturnBorrowedBook(Borrow borrow, Byte bookConditionDecreased)
        {
            using var context = new LibraryContext();
            borrow.BookReturned = true;
            borrow.BookConditionDecreased = bookConditionDecreased;
            context.SaveChanges();
        }

        public void UpdateBorrowDate(Borrow borrow)
        {
            using var context = new LibraryContext();
            borrow.DateOfBorrow = DateTime.Now;
            context.SaveChanges();
        }
    }
}
