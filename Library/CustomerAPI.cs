using System;
using System.Collections.Generic;
using System.Text;
using IDataInterface;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Library
{
    public class CustomerAPI
    {
        private ICustomerManager customerManager;
        private IBorrowManager borrowManager;
        private IBookManager bookManager;

        public CustomerAPI(ICustomerManager customerManager, IBorrowManager borrowManager, IBookManager bookManager)
        {
            this.customerManager = customerManager;
            this.borrowManager = borrowManager;
            this.bookManager = bookManager;
        }
        public CustomerErrorCodes AddCustomer(string name, string adress, string dateOfBirthDDMMYYYY, [Optional] int parentCustomerID)
        {
            DateTime dateOfBirth;
            bool correctDate = DateTime.TryParseExact(dateOfBirthDDMMYYYY, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
            if (correctDate == true)
            {
                TimeSpan yearsOld = DateTime.Today - dateOfBirth;
                if (yearsOld.TotalDays < 5475)
                {
                    var parent = customerManager.GetCustomer(parentCustomerID);
                    if (parent == null)
                    {
                        return CustomerErrorCodes.CustomerToYoungMustHaveParent;
                    }
                    else
                        customerManager.AddCustomer(name, adress, dateOfBirth, parent);
                    return CustomerErrorCodes.okUnderageCustomerWithParentAdded;
                }
                else
                    customerManager.AddCustomer(name, adress, dateOfBirth);
                return CustomerErrorCodes.ok;
            }
            else
                return CustomerErrorCodes.CustomerDateOfBirthIsIncorrect;
        }

        public CustomerErrorCodes RemoveCustomer(int customerID)
        {
            var customer = customerManager.GetCustomer(customerID);
            if (customer == null)
            {
                return CustomerErrorCodes.NoSuchCustomer;
            }
            else
            {
                if (customer.Borrows == null)
                {
                    if (customer.Bills == null)
                    {
                        customerManager.RemoveCustomer(customer);
                        return CustomerErrorCodes.ok;
                    }
                    else
                        return CustomerErrorCodes.CustomerStillOwesMoney;
                }
                else
                    return CustomerErrorCodes.CustomerStillHasBorrowedBooks;
            }
        }

        public List<Customer> getListOfAllLateCustomers()
        {
            var listOfCustomers = customerManager.GetAllCustomers();
            var listOfAllLateCustomers = new List<Customer>();
            foreach(Customer customer in listOfCustomers)
            {
                foreach(Borrow borrow in customer.Borrows)
                {
                    if(listOfAllLateCustomers.Contains(customer) == false && borrow.BookReturned == false)
                    {
                        TimeSpan timeSinceBorrow = DateTime.Today - borrow.DateOfBorrow;
                        if (timeSinceBorrow.Days > 30)
                        {
                            listOfAllLateCustomers.Add(customer);
                        }
                    }
                }
            }
            return listOfAllLateCustomers;
        }

        public struct ReminderBook
        {
            public Book book;
            public int ReminderFee;
        }
        public struct ReminderCustomer
        {
            public int customerID;
            public string customerName;
            public string customerAdress;
            public List<ReminderBook> reminderBooks;
            public int totalReminderFee;
        }


        public List<ReminderCustomer> getReminderList()
        {
            var listOfReminderCustomers = new List<ReminderCustomer>();
            var lateCustomers = getListOfAllLateCustomers();
            foreach(Customer customer in lateCustomers)
            {
                if(customer.Parent == null)
                {
                    ReminderCustomer reminderCustomer = GetNewReminderCustomer(customer);
                    foreach (Borrow borrow in customer.Borrows)
                    {
                        if (borrow.BookReturned == false)
                        {
                            TimeSpan timeSinceBorrow = DateTime.Today - borrow.DateOfBorrow;
                            if (timeSinceBorrow.Days > 30)
                            {
                                var reminderBook = new ReminderBook();
                                reminderBook.book = borrow.Book;
                                reminderBook.ReminderFee = (timeSinceBorrow.Days / 30 - 1) * 50;
                                reminderCustomer.reminderBooks.Add(reminderBook);
                                reminderCustomer.totalReminderFee += reminderBook.ReminderFee;
                            }
                        }
                    }
                    listOfReminderCustomers.Add(reminderCustomer);
                }
            }
            foreach(Customer customer in lateCustomers)
            {
                if(customer.Parent != null)
                {
                    if(lateCustomers.Any(c => c.CustomerID == customer.Parent.CustomerID) == false)
                    {
                        var reminderCustomer = GetNewReminderCustomer(customer.Parent);
                        listOfReminderCustomers.Add(reminderCustomer);
                    }
                    {
                        var reminderCustomerParent = (from c in listOfReminderCustomers
                                                      where c.customerID == customer.Parent.CustomerID
                                                      select c)
                                                      .First();
                        foreach(Borrow borrow in customer.Borrows)
                        {
                            if(borrow.BookReturned == false)
                            {
                                TimeSpan timeSinceBorrow = DateTime.Today - borrow.DateOfBorrow;
                                if (timeSinceBorrow.Days > 30)
                                {
                                    var reminderBook = new ReminderBook();
                                    reminderBook.book = borrow.Book;
                                    reminderBook.ReminderFee = (timeSinceBorrow.Days / 30 - 2) * 50;
                                    reminderCustomerParent.totalReminderFee += reminderBook.ReminderFee;
                                    reminderCustomerParent.reminderBooks.Add(reminderBook);
                                }
                            }
                        }
                    }
                }
            }
            return listOfReminderCustomers;
        }

        private static ReminderCustomer GetNewReminderCustomer(Customer customer)
        {
            var reminderCustomer = new ReminderCustomer();
            reminderCustomer.customerID = customer.CustomerID;
            reminderCustomer.customerAdress = customer.Address;
            reminderCustomer.customerName = customer.Name;
            reminderCustomer.reminderBooks = new List<ReminderBook>();
            return reminderCustomer;
        }

        public struct BadCustomer
        {
            public Customer customer;
            public int badCustomerScore;
        }
        public List<BadCustomer> GetBadCustomers()
        {
            var badCustomers = new List<BadCustomer>();
            var allCustomers = customerManager.GetAllCustomers();
            foreach (Customer customer in allCustomers)
            {
                var badCustomer = new BadCustomer();
                badCustomer.customer = customer;
                badCustomer.badCustomerScore += customer.DamageToBooks * 3;
                if(customer.Borrows != null)
                {
                    foreach (Borrow borrow in customer.Borrows)
                    {
                        TimeSpan timeSinceBorrow = DateTime.Today - borrow.DateOfBorrow;
                        if (timeSinceBorrow.Days > 30)
                        {
                            badCustomer.badCustomerScore += (timeSinceBorrow.Days / 30 - 1);
                        }
                    }
                }
                badCustomers.Add(badCustomer);
            }
            badCustomers.OrderBy(BadCustomer => BadCustomer.badCustomerScore);
            var top5BadCustomers = badCustomers.Take(5).ToList();
            return top5BadCustomers;
        }

        public List<Customer> GetListOfChildrenWithParentAdressForChildrenEvent()
        {
            var childrenList = new List<Customer>();
            var allCustomers = customerManager.GetAllCustomers();
            var badCustomers = GetBadCustomers();

            allCustomers.RemoveAll(c => badCustomers.Any(b => b.customer.CustomerID == c.CustomerID));

            foreach(Customer customer in allCustomers)
            {
                if(customer.Parent != null)
                {
                    childrenList.Add(customer);
                }
            }
            return childrenList;
        }

        public List<Customer> GetListOfCustomersWithBirthdayToday()
        {
            var customersWithBirthday = new List<Customer>();
            foreach(Customer customer in customerManager.GetAllCustomers())
            {
                if(customer.DateOfBirth.Month == DateTime.Today.Month && customer.DateOfBirth.Day == DateTime.Today.Day)
                {
                    customersWithBirthday.Add(customer);
                    if(DateTime.Today.Year - customer.DateOfBirth.Year == 15)
                    {
                        borrowManager.AddBorrow(customer, SendMostPopularBook(customer));
                    }
                }
            }
            return customersWithBirthday;
        }

        private Book SendMostPopularBook(Customer customer)
        {
            var borrowAPI = new BorrowAPI(borrowManager, null, customerManager);
            var popularBooks = borrowAPI.GetMostPopularBooksByAgeGroup();
            var book = popularBooks.Find(b => b.AgeGroupTenYearsFromThisAge == 10).Book;
            if(CheckIfCustomerIsbadCustomer(customer))
            {
                return FindBookInWorstCondition(book);
            }
            return FindBookInBestCondition(book);
        }

        private Book FindBookInBestCondition(Book book)
        {
            var books = bookManager.GetListOfBooksByTitle(book.Title);
            return books.OrderByDescending(b => b.Condition).FirstOrDefault();
        }

        private Book FindBookInWorstCondition(Book book)
        {
            var books = bookManager.GetListOfBooksByTitle(book.Title);
            return books.OrderBy(b => b.Condition).FirstOrDefault();
        }
        
        private bool CheckIfCustomerIsbadCustomer(Customer customer)
        {
            var badCustomers = GetBadCustomers();
            if(badCustomers.Any(c => c.customer.CustomerID == customer.CustomerID))
            {
                return true;
            }
            return false;
        }
    }
}
