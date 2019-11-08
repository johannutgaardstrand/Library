using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
namespace UnitTests
{
    [TestClass]
    public class BillAPITests
    {
        [TestMethod]
        public void TestGetListOfAllBills()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(new List<Customer> { new Customer { Bills = new List<Bill> { new Bill { Amount = 100 } } } });

            customerManagerMock.Setup(m =>
                m.GetCustomersChildren(It.IsAny<int>()))
                .Returns(new List<Customer> { });

            var billAPI = new BillAPI(null, customerManagerMock.Object);
            var result = billAPI.GetListOfAllCustomersWithBills();
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestGetTotalBillAmount()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            var customer = new Customer { Bills = new List<Bill> { new Bill { Amount = 100 }, new Bill { Amount = 50 } } };

            customerManagerMock.Setup(m =>
                m.GetCustomersChildren(It.IsAny<int>()))
                .Returns(new List<Customer> { });


            var billAPI = new BillAPI(null, customerManagerMock.Object);
            var result = billAPI.GetTotalBillAmount(customer);
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void TestGetTotalBillAmountCustomerWithChild()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            var customer = new Customer { Bills = new List<Bill> { new Bill { Amount = 100 }, new Bill { Amount = 50 } } };
            

            customerManagerMock.Setup(m =>
                m.GetCustomersChildren(It.IsAny<int>()))
                .Returns(new List<Customer> { new Customer { Parent = customer, Bills = new List<Bill> { new Bill { Amount = 100 } } } });


            var billAPI = new BillAPI(null, customerManagerMock.Object);
            var result = billAPI.GetTotalBillAmount(customer);
            Assert.AreEqual(250, result);
        }

        [TestMethod]
        public void TestPayBill()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetBill(It.IsAny<int>()))
                .Returns(new Bill { BillID = 1, Amount = 200 });

            var billAPI = new BillAPI(billManagerMock.Object, null);
            var result = billAPI.PayBill(1, 100);
            Assert.AreEqual(BillErrorCodes.ok, result);
            billManagerMock.Verify(m =>
                m.PayBill(It.IsAny<Bill>(), It.IsAny<double>()), Times.Once);
        }

        [TestMethod]
        public void TestPayBillNoSuchBill()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetBill(It.IsAny<int>()))
                .Returns<Bill>(null);

            var billAPI = new BillAPI(billManagerMock.Object, null);
            var result = billAPI.PayBill(1, 100);
            Assert.AreEqual(BillErrorCodes.NoSuchBill, result);
            billManagerMock.Verify(m =>
                m.PayBill(It.IsAny<Bill>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestPayBillTooLargeAmount()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetBill(It.IsAny<int>()))
                .Returns(new Bill { BillID = 1, Amount = 100 });

            var billAPI = new BillAPI(billManagerMock.Object, null);
            var result = billAPI.PayBill(1, 101);
            Assert.AreEqual(BillErrorCodes.ToLargeAmount, result);
            billManagerMock.Verify(m =>
                m.PayBill(It.IsAny<Bill>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestPayBillExactAmount()
        {
            var billManagerMock = new Mock<IBillManager>();

            billManagerMock.Setup(m =>
                m.GetBill(It.IsAny<int>()))
                .Returns(new Bill { BillID = 1, Amount = 100 });

            var billAPI = new BillAPI(billManagerMock.Object, null);
            var result = billAPI.PayBill(1, 100);
            Assert.AreEqual(BillErrorCodes.okTheWholeBillIsPayed, result);
            billManagerMock.Verify(m =>
                m.PayBill(It.IsAny<Bill>(), It.IsAny<double>()), Times.Once);
        }
    }
}
