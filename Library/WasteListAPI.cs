using DataAccess;
using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class WasteListAPI
    {
        private IWasteListManager wasteListManager;
        private IBookManager bookManager;

        public WasteListAPI(IWasteListManager wasteListManager, IBookManager bookManager)
        {
            this.wasteListManager = wasteListManager;
            this.bookManager = bookManager;
        }

        public WasteListErrorCodes MakeWasteList()
        {
            var booksInBadCondition = new List<Book>();
            var books = bookManager.GetListOfAllBooks();
            foreach(Book book in books)
            {
                if (book.Condition == 1 && book.Borrow == null)
                {
                    booksInBadCondition.Add(book);
                }
            }
            if(booksInBadCondition.Count == 0)
            {
                return WasteListErrorCodes.NoBooks;
            }
            else
            {
                wasteListManager.MakeWasteList(booksInBadCondition);
                return WasteListErrorCodes.ok;
            }
        }

        public List<Book> GetWasteList(int wasteListID)
        {
            var wasteList = wasteListManager.GetWasteList(wasteListID);
            var books = new List<Book>();
            foreach(Book book in wasteList.Books)
            {
                books.Add(book);
            }
            return books;
            
        }

        public WasteListErrorCodes RemoveAllBooksInWasteList(int wasteListID)
        {
            var oldBooks = wasteListManager.GetWasteList(wasteListID);
            if(oldBooks == null)
            {
                return WasteListErrorCodes.NoSuchWasteList;
            }
            else
            {
                foreach (Book book in oldBooks.Books)
                {
                    bookManager.RemoveBook(book);
                }
                wasteListManager.RemoveWasteList(oldBooks);
                return WasteListErrorCodes.ok;
            }
        }
    }
}
