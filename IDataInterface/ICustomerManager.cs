using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IDataInterface
{
    public interface ICustomerManager
    {
        public void AddCustomer(string name, string adress, DateTime dateOfBirthDDMMYYYY, [Optional] Customer parent);

        public Customer GetCustomer(int customerID);

        public List<Customer> GetCustomersChildren(int customerID);

        public void RemoveCustomer(Customer customer);

        public List<Customer> GetAllCustomers();

        public void CustomerDamagedBook(int customerID, int amountOfDamage);
    }
}
