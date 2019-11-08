using System;
using IDataInterface;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BillManager : IBillManager
    {
        public void AddBill(int customerID, int amount, Borrow borrow)
        {
            using var context = new LibraryContext();
            var bill = new Bill();
            bill.CustumerID = customerID;
            bill.BillDate = DateTime.Today;
            bill.Amount = amount;
            bill.borrow = borrow;
            context.Bills.Add(bill);
            context.SaveChanges();
        }

        public List<Bill> GetListOfAllBills()
        {
            using var context = new LibraryContext();
            return (from b in context.Bills
                    select b)
                    .Include(b => b.Customer)
                    .ToList();
        }

        public Bill GetBill(int billID)
        {
            using var context = new LibraryContext();
            return (from b in context.Bills
                    where b.BillID == billID
                    select b)
                    .FirstOrDefault();
        }

        public void PayBill(Bill bill, double amount)
        {
            using var context = new LibraryContext();
            bill.AmountPayed = amount;
            context.SaveChanges();
        }
    }
}
