using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IDataInterface
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int BillID { get; set; }

        public DateTime BillDate { get; set; }

        public double Amount { get; set; }

        public double AmountPayed { get; set; }

        public int CustumerID { get; set; }

        public Customer Customer { get; set; }

        public Borrow borrow { get; set; }

    }
}