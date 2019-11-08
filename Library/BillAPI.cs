using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class BillAPI
    {
        private IBillManager billManager;
        private ICustomerManager customerManager;

        public BillAPI(IBillManager billManager, ICustomerManager customerManager)
        {
            this.billManager = billManager;
            this.customerManager = customerManager;
        }

        public struct CustomerWithTotalBillAmount
        {
            public string name;
            public string addres;
            public double totalBillAmount;
        }
        public List<CustomerWithTotalBillAmount> GetListOfAllCustomersWithBills()
        {
            var customerWithTotalBillAmountList = new List<CustomerWithTotalBillAmount>();
            var allCustomers = customerManager.GetAllCustomers();
            foreach(Customer customer in allCustomers)
            {
                if (customer.Parent == null)
                {
                    customerWithTotalBillAmountList.Add(GetNewCustomerWithTotalBillAmount(customer));
                }
            }
            return customerWithTotalBillAmountList;
        }
        private CustomerWithTotalBillAmount GetNewCustomerWithTotalBillAmount(Customer customer)
        {
            var customerWithTotalBillAmount = new CustomerWithTotalBillAmount();
            customerWithTotalBillAmount.addres = customer.Address;
            customerWithTotalBillAmount.name = customer.Name;
            customerWithTotalBillAmount.totalBillAmount += GetTotalBillAmount(customer);
            return customerWithTotalBillAmount;
        }

        public double GetTotalBillAmount(Customer customer)
        {
            double totalAmount = 0;
            var customersChildren = customerManager.GetCustomersChildren(customer.CustomerID);
            foreach (Customer customerChild in customersChildren)
            {
                foreach(Bill bill in customerChild.Bills)
                {
                    totalAmount += bill.Amount;
                }
            }
            foreach(Bill bill in customer.Bills)
            {
                {
                    totalAmount += bill.Amount;
                }
            }
            return totalAmount;
        }

        public BillErrorCodes PayBill(int billID, double amount)
        {
            var bill = billManager.GetBill(billID);
            if (bill == null)
            {
                return BillErrorCodes.NoSuchBill;
            }
            else if (amount == bill.Amount)
            {
                billManager.PayBill(bill, amount);
                return BillErrorCodes.okTheWholeBillIsPayed;
            }
            else if (amount < bill.Amount)
            {
                billManager.PayBill(bill, amount);
                return BillErrorCodes.ok;
            }
            else
                return BillErrorCodes.ToLargeAmount;
        }
    }
}
