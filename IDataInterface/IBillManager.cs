using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IBillManager
    {
        public Bill GetBill(int billID);

        public List<Bill> GetListOfAllBills();

        public void AddBill(int customerID, int amount, Borrow borrow);

        public void PayBill(Bill bill, double amount);
    }
}
