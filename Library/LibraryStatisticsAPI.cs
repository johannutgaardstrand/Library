using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IDataInterface;

namespace Library
{
    public class LibraryStatisticsAPI
    {
        private IBookManager bookManager;
        private ICustomerManager customerManager;
        private IBorrowManager borrowManager;
        private IBillManager billManager;

        public LibraryStatisticsAPI(IBookManager bookManager, ICustomerManager customerManager, IBorrowManager borrowManager, IBillManager billManager)
        {
            this.bookManager = bookManager;
            this.customerManager = customerManager;
            this.borrowManager = borrowManager;
            this.billManager = billManager;
        }
        public double GetAverageCustomerAge()
        {
            return DateTime.Today.Year - customerManager.GetAllCustomers().Average(c => c.DateOfBirth.Year);
        }

        public double GetAverageBookAge()
        {
            return DateTime.Today.Year - bookManager.GetListOfAllBooks().Average(b => b.DateOfPurchase.Year);
        }

        public int GetAverageNumberOfBooksBorrowedEachMonthLastSixMonths()
        {
            double averageNumberOfBooks;
            averageNumberOfBooks = GetListOfAllBorrowsLastSixMonths().Count() / 6;
            return (int)averageNumberOfBooks;
        }

        public int GetAverageBookConditionDecreasedEachMonthLastSixMonths()
        {
            double averageBookDecrease;
            averageBookDecrease = GetListOfAllBorrowsLastSixMonths().Sum(b => b.BookConditionDecreased)/6;
            return (int)averageBookDecrease;
        }

        private List<Borrow> GetListOfAllBorrowsLastSixMonths()
        {
            return borrowManager.GetAllBorrows().FindAll(b => b.DateOfBorrow >= DateTime.Today.AddMonths(-6));
        }

        public int GetAverageNumberOfBooksBoughtEachMonth()
        {
            return bookManager.GetListOfAllBooks().Count() / GetNumberOfMonthsSinceFirstBookPurchase();
        }
        public double GetAverageExpenditureOnBooksEachMonth()
        {
            return bookManager.GetListOfAllBooks().Sum(b => b.Cost) / GetNumberOfMonthsSinceFirstBookPurchase();
        }

        private int GetNumberOfMonthsSinceFirstBookPurchase()
        {
            return DateTime.Today.Month - bookManager.GetListOfAllBooks().OrderBy(b => b.DateOfPurchase).First().DateOfPurchase.Month;
        }

        public double GetTotalIncome()
        {
            return billManager.GetListOfAllBills().Sum(b => b.AmountPayed);
        }

        public double GetTotalOutstandingCustomerDebt()
        {
            return billManager.GetListOfAllBills().Sum(b => b.Amount - b.AmountPayed);
        }

        public double GetTotalValueOfBooks()
        {
            return bookManager.GetListOfAllBooks().Sum(b => b.Cost * b.Condition * 0.2);
        }

       
    }
}
