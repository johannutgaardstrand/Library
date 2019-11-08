using System;
using System.Collections.Generic;
using System.Text;
using IDataInterface;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Library
{
    public class BorrowAPI
    {
        private IBorrowManager borrowManager;
        private IBookManager bookManager;
        private ICustomerManager customerManager;

        public BorrowAPI(IBorrowManager borrowManager, IBookManager bookManager,  ICustomerManager customerManager)
        {
            this.borrowManager = borrowManager;
            this.bookManager = bookManager;
            this.customerManager = customerManager;
        }

        public BorrowErrorCodes BorrowBook(int customerID, string bookTitle)
        {
            var customer = customerManager.GetCustomer(customerID);
            if (customer == null)
            {
                return BorrowErrorCodes.NoSuchCustomer;
            }
            var book = bookManager.GetListOfBooksByTitle(bookTitle);
            if(book == null)
            {
                return BorrowErrorCodes.NoSuchBook;
            }
            if(customer.Borrows != null && CheckIfCustomerAlreadyBorrowedBook(customer, book.First()) == true)
            {
                return BorrowErrorCodes.CustomerBorrowHasBeenExteneded;
            }
            if (CheckIfCanBorrow(customer) == false)
            {
                return BorrowErrorCodes.CustomerNotAllowedToBorrowMoreBooks;
            }
            var availableBook = new List<Book>();
            foreach (Book book1 in book)
            {
                if (book1.Borrow == null)
                {
                    availableBook.Add(book1);
                }
            }
            if(availableBook.Count == 0)
            {
                return BorrowErrorCodes.BookAlreadyBorrowed;
            }
            borrowManager.AddBorrow(customer, availableBook.First());
            return BorrowErrorCodes.ok;
        }

        private bool CheckIfCustomerAlreadyBorrowedBook(Customer customer, Book book)
        {
            foreach (Borrow borrow in customer.Borrows)
            {
                if(borrow.Book != null && borrow.Book.Title == book.Title)
                {
                    borrowManager.UpdateBorrowDate(borrow);
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfCanBorrow(Customer customer)
        {
            if(customer.Borrows != null)
            {
                if(customer.Borrows.Count > 4)
                {
                    return false;
                }
            }
            if (customer.Bills != null)
            {
                TimeSpan days;
                foreach (Bill bill in customer.Bills)
                {
                    days = DateTime.Today - bill.BillDate;
                    if (days.Days > 30)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public double ReturnBook(int bookID, byte newBookCondition)
        {
            var book = bookManager.GetBookByBookID(bookID);
            var borrow = borrowManager.GetBorrowByBookID(bookID);
            customerManager.CustomerDamagedBook(borrow.CustumerID, book.Condition - newBookCondition);
            bookManager.UpdateCondition(book, newBookCondition);
            borrowManager.ReturnBorrowedBook(borrow, (Byte)(book.Condition - newBookCondition));
            if(borrow.Bill == null)
            {
                return 0;
            }
            return borrow.Bill.Amount;
        }

        public struct PopularBook
        {
            public Book Book;
            public int AgeGroupTenYearsFromThisAge;
        }


        public List<PopularBook> GetMostPopularBooksByAgeGroup()
        {
            var allPopularBooks = new List<PopularBook>();
            var allBooksBorrowedByAgeGroup = GetArrayWithListsOfAllBooksInEachAgeGroup();
            int yearGroupLowestAge = 0;
            int yearGroupMaxAge = 9;
            for (int i = 0; i < 10; i++)
            {
                var mostPopularBookInAgeGroup = (from b in allBooksBorrowedByAgeGroup[i]
                                                 group b by b into grp
                                                 orderby grp.Count() descending
                                                 select grp.Key).FirstOrDefault();
                if (mostPopularBookInAgeGroup != null)
                {
                    var popularBook = new PopularBook();
                    popularBook.Book = mostPopularBookInAgeGroup;
                    popularBook.AgeGroupTenYearsFromThisAge = yearGroupLowestAge;
                    allPopularBooks.Add(popularBook);
                }
                yearGroupLowestAge += 10;
                yearGroupMaxAge += 10;
            }
            return allPopularBooks;
        }

        private List<Book>[] GetArrayWithListsOfAllBooksInEachAgeGroup()
        {
            var allBorrows = borrowManager.GetAllBorrows();
            List<Book>[] books = new List<Book>[10];
            int a = 0;
            for (int i = 9; i < 100; i += 10)
            {
                books[a] = new List<Book>();
                foreach (Borrow borrow in allBorrows)
                {
                    int age = DateTime.Now.Year - borrow.Customer.DateOfBirth.Year;
                    if (age <= i && age > (i - 10))
                    {
                        books[a].Add(borrow.Book);
                    }
                }
                a++;
            }
            return books;
        }
    }
}
