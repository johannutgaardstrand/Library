using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class BorrowTest
    {
        [TestMethod]
        public void TestBorrowOneBook()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer { CustomerID = 1});

            bookManagerMock.Setup(m =>
            m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> 
                { 
                    new Book 
                    { 
                        Title = "Clean Code" 
                    } 
                });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.BorrowBook(1, "CleanCode");
            Assert.AreEqual(BorrowErrorCodes.ok, successfull);
            borrowManagerMock.Verify(m =>
                m.AddBorrow(It.IsAny<Customer>(), It.IsAny<Book>()), Times.Once());
        }

        [TestMethod]
        public void TestBorrowCustomerHasBorrowedToManyBooks()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer 
                {   CustomerID = 1,
                    Borrows = new List<Borrow> 
                    {   
                        new Borrow { },
                        new Borrow { },
                        new Borrow { },
                        new Borrow { },
                        new Borrow { },
                    }
                });

            bookManagerMock.Setup(m =>
            m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> { new Book { Title = "Clean Code" } });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.BorrowBook(1, "Clean Code");
            Assert.AreEqual(BorrowErrorCodes.CustomerNotAllowedToBorrowMoreBooks, successfull);
            borrowManagerMock.Verify(m =>
                m.AddBorrow(It.IsAny<Customer>(), It.IsAny<Book>()), Times.Never());
        }

        [TestMethod]
        public void TestBorrowNoBooksAvailable()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer { });

            bookManagerMock.Setup(m =>
            m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> 
                { 
                    new Book 
                    { 
                        BookID = 1,
                        Title = "Clean Code", 
                        Borrow = new Borrow { } 
                    }
                });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.BorrowBook(1, "Clean Code");
            Assert.AreEqual(BorrowErrorCodes.BookAlreadyBorrowed, successfull);
            borrowManagerMock.Verify(m =>
                m.AddBorrow(It.IsAny<Customer>(), It.IsAny<Book>()), Times.Never());
        }

        [TestMethod]
        public void TestBorrowNoBooksAvailableUpdateDate()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer 
                {
                    Borrows = new List<Borrow>
                    {
                        new Borrow
                        {
                            BorrowID = 1,
                            Book = new Book
                            {
                                BookID = 1,
                                Title = "Clean Code"
                            }
                        }
                    }
                });

            bookManagerMock.Setup(m =>
            m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> { new Book { Title = "Clean Code", Borrow = new Borrow { } } });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.BorrowBook(1, "Clean Code");
            Assert.AreEqual(BorrowErrorCodes.CustomerBorrowHasBeenExteneded, successfull);
            borrowManagerMock.Verify(m =>
                m.UpdateBorrowDate(It.IsAny<Borrow>()), Times.Once());
        }

        [TestMethod]
        public void TestBorrowOldBill()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer 
                {
                    Bills = new List <Bill>
                    {
                        new Bill
                        {
                            BillDate = new DateTime(2018, 12, 10)
                        }
                    }
                });

            bookManagerMock.Setup(m =>
            m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> 
                { 
                    new Book 
                    {
                        BookID = 1,
                        Title = "Clean Code"
                    } 
                });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.BorrowBook(1, "Clean Code");
            Assert.AreEqual(BorrowErrorCodes.CustomerNotAllowedToBorrowMoreBooks, successfull);
            borrowManagerMock.Verify(m =>
                m.AddBorrow(It.IsAny<Customer>(), It.IsAny<Book>()), Times.Never());
        }
        [TestMethod]
        public void TestReturnOneBook()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer { });

            bookManagerMock.Setup(m =>
                m.GetBookByBookID(It.IsAny<int>()))
                .Returns(new Book { BookID = 1 });

            borrowManagerMock.Setup(m =>
                m.GetBorrowByBookID(It.IsAny<int>()))
                .Returns(new Borrow { });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.ReturnBook(1, 3);
            Assert.AreEqual(0, successfull);
            borrowManagerMock.Verify(m =>
                m.ReturnBorrowedBook(It.IsAny<Borrow>(), It.IsAny<Byte>()), Times.Once());
        }

        [TestMethod]
        public void TestReturnOneBookWithBill()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();
            var bookManagerMock = new Mock<IBookManager>();
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer { });

            bookManagerMock.Setup(m =>
                m.GetBookByBookID(It.IsAny<int>()))
                .Returns(new Book { BookID = 1 });

            borrowManagerMock.Setup(m =>
                m.GetBorrowByBookID(It.IsAny<int>()))
                .Returns(new Borrow 
                { 
                    Bill = new Bill 
                    { 
                        Amount = 100 
                    }, 
                    BookID = 1 
                });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, bookManagerMock.Object, customerManagerMock.Object);
            var successfull = borrowAPI.ReturnBook(1, 2);
            Assert.AreEqual(100, successfull);
            borrowManagerMock.Verify(m =>
                m.ReturnBorrowedBook(It.IsAny<Borrow>(), It.IsAny<Byte>()), Times.Once());
        }

        [TestMethod]
        public void TestMostPopularBooks()
        {
            var borrowManagerMock = new Mock<IBorrowManager>();

            borrowManagerMock.Setup(m =>
                m.GetAllBorrows())
                .Returns(new List<Borrow>
                {
                    new Borrow
                    {
                        Book = new Book
                        {
                            Author = "Albin", 
                            Title = "c#"
                        },
                        Customer = new Customer
                        {
                            DateOfBirth = new DateTime(2010, 10, 10), 
                            Borrows = new List<Borrow>
                            {
                                new Borrow{  } 
                            } 
                        } 
                    } 
                });

            var borrowAPI = new BorrowAPI(borrowManagerMock.Object, null, null);
            var popularBookList = borrowAPI.GetMostPopularBooksByAgeGroup();
            Assert.AreEqual(popularBookList.Count, 1);
        }
    }
}
