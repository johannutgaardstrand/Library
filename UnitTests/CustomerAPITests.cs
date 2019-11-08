using IDataInterface;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.CustomerAPI;

namespace UnitTests
{
    [TestClass]
    public class CustomerAPITests
    {
        [TestMethod]
        public void TestAddCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.AddCustomer("Anna", "Fiskvägen 10", "10122000");
            Assert.AreEqual(CustomerErrorCodes.ok, successfull);
            customerManagerMock.Verify(m =>
                m.AddCustomer(It.Is<string>(s => s == "Anna"), It.Is<string>(a => a == "Fiskvägen 10"), It.Is<DateTime>(d => d.Month == 12),It.Is<Customer>(c => c == null)), Times.Once);
        }

        [TestMethod]
        public void TestAddCustomerWrongDate()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.AddCustomer("Anna", "Fiskvägen 10", "11132019");
            Assert.AreEqual(CustomerErrorCodes.CustomerDateOfBirthIsIncorrect, successfull);
            customerManagerMock.Verify(m =>
                m.AddCustomer(It.Is<string>(s => s == "Anna"), It.Is<string>(a => a == "Fiskvägen 10"), It.IsAny<DateTime>(), It.Is<Customer>(c => c == null)), Times.Never);
        }
        [TestMethod]
        public void TestAddCustomerUnderAge()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.AddCustomer("Anna", "Fiskvägen 10", "01012019");
            Assert.AreEqual(CustomerErrorCodes.CustomerToYoungMustHaveParent, successfull);
            customerManagerMock.Verify(m =>
                m.AddCustomer(It.Is<string>(s => s == "Anna"), It.Is<string>(a => a == "Fiskvägen 10"), It.IsAny<DateTime>(), It.Is<Customer>(c => c == null)), Times.Never);
        }
        [TestMethod]
        public void TestAddUnderageCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer
                {
                    CustomerID = 1
                });

            customerManagerMock.Setup(m =>
                m.AddCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.AddCustomer("Anna", "Fiskvägen 10", "10122010", 1);
            Assert.AreEqual(CustomerErrorCodes.okUnderageCustomerWithParentAdded, successfull);
            customerManagerMock.Verify(m =>
                m.AddCustomer(It.Is<string>(s => s == "Anna"), It.Is<string>(a => a == "Fiskvägen 10"), It.IsAny<DateTime>(), It.IsAny<Customer>()), Times.Once);
        }
        [TestMethod]
        public void TestRemoveCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer
                {
                    CustomerID = 1
                });

            customerManagerMock.Setup(m =>
                m.RemoveCustomer(It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.RemoveCustomer(1);
            Assert.AreEqual(CustomerErrorCodes.ok, successfull);
            customerManagerMock.Verify(m =>
                m.RemoveCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public void TestRemoveCustomerNoSuchCustomer()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns<Customer>(null);
            customerManagerMock.Setup(m =>
                m.RemoveCustomer(It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.RemoveCustomer(1);
            Assert.AreEqual(CustomerErrorCodes.NoSuchCustomer, successfull);
            customerManagerMock.Verify(m =>
                m.RemoveCustomer(It.IsAny<Customer>()), Times.Never);
        }

        [TestMethod]
        public void TestRemoveCustomerBorrowedBooks()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer
                {
                    CustomerID = 1,
                    Borrows = new List<Borrow>
                    {
                        new Borrow{}
                    }
        }); 

            customerManagerMock.Setup(m =>
                m.RemoveCustomer(It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.RemoveCustomer(1);
            Assert.AreEqual(CustomerErrorCodes.CustomerStillHasBorrowedBooks, successfull);
            customerManagerMock.Verify(m =>
                m.RemoveCustomer(It.IsAny<Customer>()), Times.Never);
        }


        [TestMethod]
        public void TestRemoveCustomerOweMoney()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetCustomer(It.IsAny<int>()))
                .Returns(new Customer
                {
                    CustomerID = 1,
                    Bills = new List<Bill>
                    {
                        new Bill{}
                    }
                });

            customerManagerMock.Setup(m =>
                m.RemoveCustomer(It.IsAny<Customer>()));

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var successfull = CustomerAPI.RemoveCustomer(1);
            Assert.AreEqual(CustomerErrorCodes.CustomerStillOwesMoney, successfull);
            customerManagerMock.Verify(m =>
                m.RemoveCustomer(It.IsAny<Customer>()), Times.Never);
        }

        [TestMethod]
        public void TestGetLateCustomers()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(
                    new List<Customer>
                    {new Customer 
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } },
                    {new Customer 
                    {   Name = "Lars",
                        Address = "Kumla",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), Book = new Book { } }, new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } }} }
                );

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var reminderList = CustomerAPI.getListOfAllLateCustomers();
            Assert.AreEqual(reminderList.Count, 2);
        }

        [TestMethod]
        public void TestReminderList()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(
                    new List<Customer>
                    {new Customer 
                    {   Name = "Adam",
                        Address = "Örebro", 
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } },
                    {new Customer 
                    {   Name = "Lars", 
                        Address = "Kumla", 
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), Book = new Book { } }, new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } }} }
                );

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var reminderList = CustomerAPI.getReminderList();
            Assert.AreEqual(reminderList.Count, 2);
        }

        [TestMethod]
        public void TestReminderListAddUnderage()
        {
            var customerManagerMock = new Mock<ICustomerManager>();
            var parent = new Customer
            {
                CustomerID = 1,
                Name = "Adam",
                Address = "Örebro",
                Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } }
            };
            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(
                    new List<Customer>
                    {parent,
                    {new Customer
                    {   Name = "Albin",
                        Address = "Uzbekistan",
                        DateOfBirth = new DateTime(20150101),
                        Parent = parent,
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), Book = new Book { } }, new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } } },
                    {
                new Customer
                    {   Name = "Lars",
                        Address = "Kumla",
                        DateOfBirth = new DateTime(20150101),
                        Parent = parent,
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), Book = new Book { } }, new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } }} }

                );

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var reminderList = CustomerAPI.getReminderList();
            Assert.AreEqual(reminderList.Count, 1);
        }

        [TestMethod]
        public void TestBadCustomerList()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(
                    new List<Customer>
                    {new Customer
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } },
                        {new Customer
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } } },
                        {new Customer
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } } },
                        {new Customer
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } } },
                        {new Customer
                    {   Name = "Adam",
                        Address = "Örebro",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } } } ,
                    {new Customer
                    {   Name = "Lars",
                        Address = "Kumla",
                        Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2019, 10, 10), Book = new Book { } }, new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } }} }
                );

            var CustomerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var badCustomerList = CustomerAPI.GetBadCustomers();
            Assert.AreEqual(badCustomerList.Count, 5);
        }

        [TestMethod]
        public void TestGetListOfChildrenWithParent()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(
                    new List<Customer>
                    {new Customer{CustomerID = 1, Name = "Adam", Address = "Örebro", Borrows = new List<Borrow> { new Borrow { DateOfBorrow = new DateTime(2018, 10, 10), Book = new Book { } } } },
                     new Customer {CustomerID = 2, Parent = new Customer{ CustomerID = 9} },
                     new Customer {CustomerID = 3, Parent = new Customer{ CustomerID = 10} },
                     new Customer {CustomerID = 4, Parent = new Customer{ CustomerID = 11} },
                     new Customer {CustomerID = 5, Parent = new Customer{ CustomerID = 12} },
                     new Customer {CustomerID = 6, Parent = new Customer{ CustomerID = 13} },
                     new Customer {CustomerID = 7, Parent = new Customer { CustomerID = 14} },
                     new Customer {CustomerID = 8, Parent = new Customer { CustomerID = 15} } });

                    

            var customerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var childrenList = customerAPI.GetListOfChildrenWithParentAdressForChildrenEvent();
            Assert.AreEqual(3, childrenList.Count);
        }
        [TestMethod]
        public void TestGetListOfCustomersWithBirthdayToday()
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock.Setup(m =>
                m.GetAllCustomers())
                .Returns(new List<Customer>
                {
                    new Customer{DateOfBirth = new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day)},
                    new Customer{DateOfBirth = new DateTime(2018, 8, 9)}
                });

            var customerAPI = new CustomerAPI(customerManagerMock.Object, null, null);
            var result = customerAPI.GetListOfCustomersWithBirthdayToday();
            Assert.AreEqual(1, result.Count());
        }
    } 
}
