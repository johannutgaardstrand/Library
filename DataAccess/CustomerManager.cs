using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using IDataInterface;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CustomerManager : ICustomerManager
    {
        public void AddCustomer(string name, string adress, DateTime DateOfBirthDDMMYYYY, [Optional] Customer parent)
        {
            using var context = new LibraryContext();
            var customer = new Customer();
            customer.Address = adress;
            customer.DateOfBirth = DateOfBirthDDMMYYYY;
            customer.Parent = parent;
            customer.Name = name;
            context.Custumers.Add(customer);
            context.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            using var context = new LibraryContext();
            return (from c in context.Custumers
                    select c)
                    .Include(c => c.Parent)
                    .Include(c => c.Bills)
                    .Include(c => c.Borrows)
                    .ThenInclude(c => c.Book)
                    .ToList();
        }

        public Customer GetCustomer(int customerID)
        {
            using var context = new LibraryContext();
            return (from c in context.Custumers
                    where c.CustomerID == customerID
                    select c)
                    .Include(c => c.Borrows)
                    .ThenInclude(c => c.Book)
                    .Include(c => c.Bills)
                    .FirstOrDefault();
        }

        public void RemoveCustomer(Customer customer)
        {
            using var context = new LibraryContext();
            context.Custumers.Remove(customer);
            context.SaveChanges();
        }

        public void CustomerDamagedBook(int customerID, int amountOfDamage)
        {
            using var context = new LibraryContext();
            var customer = GetCustomer(customerID);
            customer.DamageToBooks += amountOfDamage;
            context.SaveChanges();

        }

        public List<Customer> GetCustomersChildren(int customerID)
        {
            using var context = new LibraryContext();
            return (from c in context.Custumers
                    where c.Parent.CustomerID == customerID
                    select c)
                    .Include(c => c.Bills)
                    .ToList();
        }
    }
}
