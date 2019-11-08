using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class BookAPITests
    {

        [TestMethod]
        public void TestISBNCorrect()
        {
            var bookAPI = new BookAPI(null, null);
            Assert.IsTrue(bookAPI.ValidateISBN(9780136083221));
        }

        [TestMethod]
        public void TestISBNWrong()
        {
            var bookAPI = new BookAPI(null, null);
            Assert.IsFalse(bookAPI.ValidateISBN(9780156083221));
        }

        [TestMethod]
        public void TestISBNWrongLenght()
        {
            var bookAPI = new BookAPI(null, null);
            Assert.IsFalse(bookAPI.ValidateISBN(978015608321));
        }
        [TestMethod]
        public void TestAddBook()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    ShelfNumber = 1,
                });

            bookManagerMock.Setup(m =>
                m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book>());

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.AddNewBook("Clean Code", "Robert", 450.30, 9780136083221, 1);
            Assert.AreEqual(BookErrorCodes.ok, successfull);
            bookManagerMock.Verify(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestAddBookNoSuchShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns<Shelf>(null);

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.AddNewBook("Clean Code", "Robert", 450, 9780136083221, 1);
            Assert.AreEqual(BookErrorCodes.NoSuchShelf, successfull);
            bookManagerMock.Verify(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestAddBookWrongISBN()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    ShelfNumber = 1,
                });

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.AddNewBook("Clean Code", "Robert", 450, 9180136083221, 1);
            Assert.AreEqual(BookErrorCodes.IncorrectISBN, successfull);
            bookManagerMock.Verify(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestAddBookExistingBook()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    ShelfNumber = 1,
                });

            bookManagerMock.Setup(m =>
                m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> {
                    new Book
                    {
                        Title = "Clean Code",
                        ShelfID = 2,
                    }});

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.AddNewBook("Clean Code", "Robert", 450, 9780136083221, 1);
            Assert.AreEqual(BookErrorCodes.OkButBookIsPutInAnotherShelfWithExistingBooks, successfull);
            bookManagerMock.Verify(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveBookNoSuchBook()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetBookByBookID(It.IsAny<int>()))
                .Returns<Book>(null);

            bookManagerMock.Setup(m =>
                m.RemoveBook(It.IsAny<Book>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, null);
            var successfull = bookAPI.RemoveBookFromLibrary(1);
            Assert.AreEqual(BookErrorCodes.NoSuchBook, successfull);
            bookManagerMock.Verify(m =>
                m.MoveBook(It.IsAny<Book>(), It.IsAny<int>()), Times.Never);
        }
        [TestMethod]
        public void TestRemoveBook()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetBookByBookID(It.IsAny<int>()))
                .Returns(new Book
                {
                    BookID = 1
                });

            bookManagerMock.Setup(m =>
                m.RemoveBook(It.IsAny<Book>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, null);
            var successfull = bookAPI.RemoveBookFromLibrary(1);
            Assert.AreEqual(BookErrorCodes.ok, successfull);
            bookManagerMock.Verify(m =>
                m.RemoveBook(It.IsAny<Book>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveBorrowedBook()
        {
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetBookByBookID(It.IsAny<int>()))
                .Returns(new Book
                {
                    BookID = 1,
                    Borrow = new Borrow
                    {
                        BorrowID = 1
                    }
                });

            bookManagerMock.Setup(m =>
                m.RemoveBook(It.IsAny<Book>()));

            var bookAPI = new BookAPI(bookManagerMock.Object, null);
            var successfull = bookAPI.RemoveBookFromLibrary(1);
            Assert.AreEqual(BookErrorCodes.BookBorrowed, successfull);
            bookManagerMock.Verify(m =>
                m.RemoveBook(It.IsAny<Book>()), Times.Never);
        }
        [TestMethod]
        public void TestMoveBook()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> {
                    new Book
                    {
                        Title = "Clean Code",
                        ShelfID = 2,
                    }});

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1
                });



            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.MoveBookToAnotherShelf("Clean code", 1);
            Assert.AreEqual(BookErrorCodes.BookMoved, successfull);
            bookManagerMock.Verify(m =>
                m.MoveBook(It.IsAny<Book>(), It.Is<int>(i => i == 1)), Times.Once);
        }

        [TestMethod]
        public void TestMoveBookNoSuchBook()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book>());

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1
                });

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.MoveBookToAnotherShelf("Clean code", 1);
            Assert.AreEqual(BookErrorCodes.NoSuchBook, successfull);
            bookManagerMock.Verify(m =>
                m.MoveBook(It.IsAny<Book>(), It.Is<int>(i => i == 1)), Times.Never);
        }

        [TestMethod]
        public void TestMoveBookNoSuchShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.GetListOfBooksByTitle(It.IsAny<string>()))
                .Returns(new List<Book> {
                    new Book
                    {
                        Title = "Clean Code",
                        ShelfID = 2,
                    }});

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns<Shelf>(null);

            var bookAPI = new BookAPI(bookManagerMock.Object, shelfManagerMock.Object);
            var successfull = bookAPI.MoveBookToAnotherShelf("Clean code", 1);
            Assert.AreEqual(BookErrorCodes.NoSuchShelf, successfull);
            bookManagerMock.Verify(m =>
                m.MoveBook(It.IsAny<Book>(), It.Is<int>(i => i == 1)), Times.Never);
        }
    }
}
