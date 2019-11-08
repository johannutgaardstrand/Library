using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class LibraryStatisticsAPITests
    {
        [TestMethod]
        public void TestCustomerAverageAge()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(new List<Customer>
                {new Customer{ DateOfBirth = new DateTime(2014,10,31) },
                 new Customer{ DateOfBirth = new DateTime(2003,10,31) }
                });

            var libraryAPI = new LibraryStatisticsAPI(null, customerManagerMock.Object, null, null);
            var result = libraryAPI.GetAverageCustomerAge();
            Assert.AreEqual(10.5, result);
        }

        [TestMethod]
        public void TestBookAverageAge()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>
                {new Book{ DateOfPurchase = new DateTime (2009, 11, 07) },
                 new Book{ DateOfPurchase = new DateTime (1999, 11, 07) },
                 new Book{ DateOfPurchase = new DateTime (1999, 11, 07) },
                 new Book{ DateOfPurchase = new DateTime (2009, 11, 07) }
                });

            var libraryAPI = new LibraryStatisticsAPI(bookManagerMock.Object, null, null, null);
            var result = libraryAPI.GetAverageBookAge();
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void TestGetAverageNumberOfBooksBorrowedEachMonthLastSixMonths()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();

            borrowManagerMock.Setup(m =>
            m.GetAllBorrows())
                .Returns(new List<Borrow>
                { new Borrow {DateOfBorrow = new DateTime (2019, 10, 10) },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10) } } }
                );

            var libraryAPI = new LibraryStatisticsAPI(null, null, borrowManagerMock.Object, null);
            var result = libraryAPI.GetAverageNumberOfBooksBorrowedEachMonthLastSixMonths();
            Assert.AreEqual(2, result);
        }
        [TestMethod]
        public void TestGetAverageBookConditionDecreasedEachMonthLastSixMonths()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();

            borrowManagerMock.Setup(m =>
            m.GetAllBorrows())
                .Returns(new List<Borrow>
                { new Borrow {DateOfBorrow = new DateTime (2019, 10, 10), BookConditionDecreased = 2 },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), BookConditionDecreased = 2 } },
                { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), BookConditionDecreased = 2 } } }
                );

            var libraryAPI = new LibraryStatisticsAPI(null, null, borrowManagerMock.Object, null);
            var result = libraryAPI.GetAverageBookConditionDecreasedEachMonthLastSixMonths();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestGetTotalIncome()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetListOfAllBills())
                .Returns(new List<Bill>
                {new Bill {AmountPayed = 100},
                {new Bill {AmountPayed = 50} }
                });

            var libraryAPI = new LibraryStatisticsAPI(null, null, null, billManagerMock.Object);
            var result = libraryAPI.GetTotalIncome();
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void TestGetAverageNumberOfBooksBoughtEachMonth()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>
                {new Book{ DateOfPurchase = new DateTime (2019, 10, 9) },
                 new Book{ DateOfPurchase = new DateTime (2019, 10, 9) },
                 new Book{ DateOfPurchase = new DateTime (2019, 9, 9) },
                 new Book{ DateOfPurchase = new DateTime (2019, 9, 9) }
                });

            var libraryAPI = new LibraryStatisticsAPI(bookManagerMock.Object, null, null, null);
            var result = libraryAPI.GetAverageNumberOfBooksBoughtEachMonth();
            Assert.AreEqual(2, result);
        }
        [TestMethod]
        public void TestGetAverageExpenditureOnBooksEachMonth()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>
                {new Book{ DateOfPurchase = new DateTime (2019, 10, 9), Cost = 100 },
                 new Book{ DateOfPurchase = new DateTime (2019, 9, 9), Cost = 300 },
                });

            var libraryAPI = new LibraryStatisticsAPI(bookManagerMock.Object, null, null, null);
            var result = libraryAPI.GetAverageExpenditureOnBooksEachMonth();
            Assert.AreEqual(200, result);
        }

        [TestMethod]
        public void TestGetTotalOutstandingCustomerDebt()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetListOfAllBills())
                .Returns(new List<Bill>
                {new Bill {Amount = 100},
                {new Bill {Amount = 50} }
                });

            var libraryAPI = new LibraryStatisticsAPI(null, null, null, billManagerMock.Object);
            var result = libraryAPI.GetTotalOutstandingCustomerDebt();
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void TestGetTotalValueOfBooks()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>
                {new Book{ Cost = 100, Condition = 5 },
                 new Book{ Cost = 500, Condition = 2 },
                });

            var libraryAPI = new LibraryStatisticsAPI(bookManagerMock.Object, null, null, null);
            var result = libraryAPI.GetTotalValueOfBooks();
            Assert.AreEqual(300, result);
        }
    }
}
