using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class HallAndShelfAPITests
    {
        [TestMethod]
        public void TestRemoveHall()
        {
            var hallManagerMock = new Mock<IHallManager>();

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns(new Hall
                {
                    HallID = 1,
                    Shelves = new List<Shelf>(),
                });

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, null);
            var successfull = hallAndShelfAPI.RemoveHall(1);
            Assert.AreEqual(HallErrorCodes.ok, successfull);
            hallManagerMock.Verify(m =>
                m.RemoveHall(It.IsAny<Hall>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveHallWithShelf()
        {
            var hallManagerMock = new Mock<IHallManager>();

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns(new Hall
                {
                    HallID = 1,
                    Shelves = new List<Shelf>{
                        new Shelf{
                            ShelfNumber = 1
                        } },
                });

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, null);
            var successfull = hallAndShelfAPI.RemoveHall(1);
            Assert.AreEqual(HallErrorCodes.ThereAreShelvesInThisHall, successfull);
            hallManagerMock.Verify(m =>
                m.RemoveHall(It.IsAny<Hall>()), Times.Never);
        }

        [TestMethod]
        public void TestAddShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var hallManagerMock = new Mock<IHallManager>();

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns(new Hall
                {
                    HallID = 1,
                });

            shelfManagerMock.Setup(m =>
                m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<Shelf>(null);

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.AddShelf(1, 1);
            Assert.AreEqual(ShelfErrorCodes.ok, successfull);
            shelfManagerMock.Verify(
                m => m.AddShelf(It.Is<int>(i => i == 1), It.Is<int>(n => n == 1)), Times.Once());
        }

        [TestMethod]
        public void TestAddShelfNoHall()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var hallManagerMock = new Mock<IHallManager>();

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns<Hall>(null);

            shelfManagerMock.Setup(m =>
                m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<Shelf>(null);

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.AddShelf(1, 1);
            Assert.AreEqual(ShelfErrorCodes.NoSuchHall, successfull);
            shelfManagerMock.Verify(
                m => m.AddShelf(It.Is<int>(i => i == 1), It.Is<int>(n => n == 1)), Times.Never());
        }

        [TestMethod]
        public void TestAddShelfSameShelfNumber()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var hallManagerMock = new Mock<IHallManager>();

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns(new Hall
                {
                    HallID = 1,
                });

            shelfManagerMock.Setup(m =>
                m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Shelf
                {
                    HallID = 1,
                    ShelfNumber = 1,
                });

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.AddShelf(1, 1);
            Assert.AreEqual(ShelfErrorCodes.ShelfNumberOccupied, successfull);
            shelfManagerMock.Verify(
                m => m.AddShelf(It.Is<int>(i => i == 1), It.Is<int>(n => n == 1)), Times.Never());
        }

        [TestMethod]
        public void TestRemoveShelfNoShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns<Shelf>(null);

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.RemoveShelf(1);
            Assert.AreEqual(ShelfErrorCodes.NoSuchShelf, successfull);
            shelfManagerMock.Verify(m =>
                m.RemoveShelf(It.IsAny<Shelf>()), Times.Never);
        }
        [TestMethod]
        public void TestRemoveShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                }
                    );

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.RemoveShelf(1);
            Assert.AreEqual(ShelfErrorCodes.ok, successfull);
            shelfManagerMock.Verify(m =>
                m.RemoveShelf(It.IsAny<Shelf>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveShelfWithBooks()
        {
            var shelfManagerMock = new Mock<IShelfManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    Books = new List<Book>
                    {
                        new Book{}
                    }
                });

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.RemoveShelf(1);
            Assert.AreEqual(ShelfErrorCodes.TheShelfContainsBooks, successfull);
            shelfManagerMock.Verify(m =>
                m.RemoveShelf(It.IsAny<Shelf>()), Times.Never);
        }

        [TestMethod]
        public void TestChangeShelfNoSuchShelf()
        {
            var shelfManagerMock = new Mock<IShelfManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns<Shelf>(null);

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.ChangeShelf(1);
            Assert.AreEqual(ShelfErrorCodes.NoSuchShelf, successfull);
            shelfManagerMock.Verify(m =>
                m.ChangeShelfNumber(It.IsAny<Shelf>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestChangeShelChangeShelfNumber()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            
            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    ShelfNumber = 2
                });

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.ChangeShelf(1, shelfNumber: 1);
            Assert.AreEqual(ShelfErrorCodes.TheShelfNumberHasBeenChanged, successfull);
            shelfManagerMock.Verify(m =>
                m.ChangeShelfNumber(It.Is<Shelf>(s => s.ShelfID == 1), It.Is<int>(i => i == 1)), Times.Once);
        }
        [TestMethod]
        public void TestChangeShelfChangeExistingShelfNumber()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            
            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    ShelfNumber = 2
                });

            shelfManagerMock.Setup(m =>
                m.GetShelfByShelfNumber(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 2,
                    ShelfNumber = 1
                });

            var hallAndShelfAPI = new HallAndShelfAPI(null, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.ChangeShelf(1, shelfNumber: 1);
            Assert.AreEqual(ShelfErrorCodes.ShelfNumberOccupied, successfull);
            shelfManagerMock.Verify(m =>
                m.ChangeShelfNumber(It.Is<Shelf>(s => s.ShelfID == 1), It.Is<int>(i => i == 1)), Times.Never);
        }

        [TestMethod]
        public void TestChangeShelfChangeHall()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var hallManagerMock = new Mock<IHallManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    HallID = 2,
                });

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns(new Hall
                {
                    HallID = 1
                });

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.ChangeShelf(1, hallID: 1);
            Assert.AreEqual(ShelfErrorCodes.TheShelfHasChangedHall, successfull);
            shelfManagerMock.Verify(m =>
                m.ChangeShelfHall(It.Is<Shelf>(s => s.ShelfID == 1), It.Is<int>(i => i == 1)), Times.Once);
        }

        [TestMethod]
        public void TestChangeShelfChangeNoSuchHall()
        {
            var shelfManagerMock = new Mock<IShelfManager>();
            var hallManagerMock = new Mock<IHallManager>();

            shelfManagerMock.Setup(m =>
                m.GetShelf(It.IsAny<int>()))
                .Returns(new Shelf
                {
                    ShelfID = 1,
                    HallID = 2,
                });

            hallManagerMock.Setup(m =>
                m.GetHall(It.IsAny<int>()))
                .Returns<Hall>(null);

            var hallAndShelfAPI = new HallAndShelfAPI(hallManagerMock.Object, shelfManagerMock.Object);
            var successfull = hallAndShelfAPI.ChangeShelf(1, hallID: 1);
            Assert.AreEqual(ShelfErrorCodes.NoSuchHall, successfull);
            shelfManagerMock.Verify(m =>
                m.ChangeShelfHall(It.Is<Shelf>(s => s.ShelfID == 1), It.Is<int>(i => i == 1)), Times.Never);
        }
    }
}
