using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IBorrowManager
    {
        public Borrow GetBorrow(int borrowID);

        public void AddBorrow(Customer customer, Book book);

        public void UpdateBorrowDate(Borrow borrow);

        public void ReturnBorrowedBook(Borrow borrow, Byte bookConditionDecreased);

        public Borrow GetBorrowByBookID(int BookID);

        public List<Borrow> GetAllBorrows();
    }
}
