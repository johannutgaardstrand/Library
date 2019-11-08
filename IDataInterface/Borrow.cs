using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IDataInterface
{
    public class Borrow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowID { get; set; }

        public DateTime DateOfBorrow { get; set; }

        public bool BookReturned { get; set; }

        public byte BookConditionDecreased { get; set; }

        public int CustumerID { get; set; }

        public Customer Customer { get; set; }

        public int BookID { get; set; }

        public Book Book { get; set; }

        public int BillID { get; set; }

        public Bill Bill { get; set; }

    }
}
