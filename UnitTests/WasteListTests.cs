using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class WasteListTests
    {
        [TestMethod]
        public void TestMakeWasteList()
        {
            var wasteListManagerMock = new Mock<IWasteListManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>()
                {
                    new Book {Condition = 1},
                    new Book {Condition = 1},
                });

            wasteListManagerMock.Setup(m =>
               m.MakeWasteList(It.IsAny<List<Book>>()));

            var wasteListAPI = new WasteListAPI(wasteListManagerMock.Object, bookManagerMock.Object);
            var successfull = wasteListAPI.MakeWasteList();
            Assert.AreEqual(WasteListErrorCodes.ok, successfull);
            wasteListManagerMock.Verify(
                m => m.MakeWasteList(It.IsAny<List<Book>>()), Times.Once());
        }

        [TestMethod]
        public void TestWasteListNoBrokenBooks()
        {
            var wasteListManagerMock = new Mock<IWasteListManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>()
                {
                    new Book {BookID = 1, Condition = 2},
                    new Book {BookID = 2, Condition = 3},
                });

            wasteListManagerMock.Setup(m =>
               m.MakeWasteList(It.IsAny<List<Book>>()));

            var wasteListAPI = new WasteListAPI(wasteListManagerMock.Object, bookManagerMock.Object);
            var successfull = wasteListAPI.MakeWasteList();
            Assert.AreEqual(WasteListErrorCodes.NoBooks, successfull);
            wasteListManagerMock.Verify(
                m => m.MakeWasteList(It.IsAny<List<Book>>()), Times.Never());
        }

        [TestMethod]
        public void TestWasteListBorrowedBooks()
        {
            var wasteListManagerMock = new Mock<IWasteListManager>();
            var bookManagerMock = new Mock<IBookManager>();

            bookManagerMock.Setup(m =>
                m.AddBook(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<int>()));

            bookManagerMock.Setup(m =>
                m.GetListOfAllBooks())
                .Returns(new List<Book>()
                {
                    new Book {BookID = 1, Condition = 1, Borrow = new Borrow{ } },
                });

            wasteListManagerMock.Setup(m =>
               m.MakeWasteList(It.IsAny<List<Book>>()));

            var wasteListAPI = new WasteListAPI(wasteListManagerMock.Object, bookManagerMock.Object);
            var successfull = wasteListAPI.MakeWasteList();
            Assert.AreEqual(WasteListErrorCodes.NoBooks, successfull);
            wasteListManagerMock.Verify(
                m => m.MakeWasteList(It.IsAny<List<Book>>()), Times.Never());
        }

        [TestMethod]
        public void TestWasteListRemoveBooks()
        {
            var wasteListManagerMock = new Mock<IWasteListManager>();
            var bookManagerMock = new Mock<IBookManager>();

            wasteListManagerMock.Setup(m =>
                m.GetWasteList(It.IsAny<int>()))
                .Returns(new WasteList
                {
                    WasteListID = 1,
                    Books = new List<Book> { new Book { }, new Book { } }
                });

            var wasteListAPI = new WasteListAPI(wasteListManagerMock.Object, bookManagerMock.Object);
            var successfull = wasteListAPI.RemoveAllBooksInWasteList(1);
            Assert.AreEqual(WasteListErrorCodes.ok, successfull);
            wasteListManagerMock.Verify(
                m => m.RemoveWasteList(It.IsAny<WasteList>()), Times.Once());
            bookManagerMock.Verify(m => 
                m.RemoveBook(It.IsAny<Book>()), Times.Exactly(2));
        }
    }
}
