using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IDataInterface;

namespace Library
{
    public class BookAPI
    {
        private IBookManager bookManager;
        private IShelfManager shelfManager;

        public BookAPI(IBookManager bookManager, IShelfManager shelfManager)
        {
            this.bookManager = bookManager;
            this.shelfManager = shelfManager;
        }

        public BookErrorCodes AddNewBook(string title, string author, double cost, long ISBN, int shelfID)
        {
            var existingShelf = shelfManager.GetShelf(shelfID);
            if (existingShelf == null)
            {
                return BookErrorCodes.NoSuchShelf;
            }
            if(ValidateISBN(ISBN) == false)
            {
                return BookErrorCodes.IncorrectISBN;
            }
            var sameBooks = bookManager.GetListOfBooksByTitle(title);
            if (sameBooks.Count > 0)
            {
                bookManager.AddBook(title, author, cost, ISBN, sameBooks.First().ShelfID);
                return BookErrorCodes.OkButBookIsPutInAnotherShelfWithExistingBooks;
            }
            else
            bookManager.AddBook(title, author, cost, ISBN, shelfID);
            return BookErrorCodes.ok;
        }
        public BookErrorCodes RemoveBookFromLibrary(int bookID)
        {
            var existingBook = bookManager.GetBookByBookID(bookID);
            if (existingBook == null)
            {
                return BookErrorCodes.NoSuchBook;
            }
            if (existingBook.Borrow != null)
            {
                return BookErrorCodes.BookBorrowed;
            }
            bookManager.RemoveBook(existingBook);
            return BookErrorCodes.ok;
        }
        public BookErrorCodes MoveBookToAnotherShelf(string title, int shelfID)
        {
            var existingShelf = shelfManager.GetShelf(shelfID);
            if (existingShelf == null)
            {
                return BookErrorCodes.NoSuchShelf;
            }
            var bookList = bookManager.GetListOfBooksByTitle(title);
            if (bookList.Count != 0)
            {
                foreach (Book book in bookList)
                {
                    bookManager.MoveBook(book, shelfID);
                }
                return BookErrorCodes.BookMoved;
            }
            return BookErrorCodes.NoSuchBook;
        }

        public bool ValidateISBN(long ISBN)
        {
            bool correktNumber = false;
            int numberLength = ISBN.ToString().Length;
            if(numberLength == 13)
            {
                var tal = ISBN.ToString().Select(t => int.Parse(t.ToString())).ToArray();
                int a2 = tal[1] * 3;
                int a4 = tal[3] * 3;
                int a6 = tal[5] * 3;
                int a8 = tal[7] * 3;
                int a10 = tal[9] * 3;
                int a12 = tal[11] * 3;
                int sum = tal[0] + a2 + tal[2] + a4 + tal[4] + a6 + tal[6] + a8 + tal[8] + a10 + tal[10] + a12;
                sum %= 10;
                if (sum != 0)
                {
                    sum = 10 - sum;
                }
                if (sum == tal[12])
                {
                    correktNumber = true;
                }
            }
            return correktNumber;
        }
    }
}
