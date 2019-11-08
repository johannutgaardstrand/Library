using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IBookManager
    {
        public Book GetBookByBookID(int bookID);

        public List<Book> GetListOfAllBooks();

        public List<Book> GetListOfBooksByTitle(string title);

        public void AddBook(string title, string author, double cost, long ISBN, int shelfID);

        public void RemoveBook(Book book);

        public void MoveBook(Book book, int shelfID);

        public void UpdateCondition(Book book, byte newCondition);
    }
}
