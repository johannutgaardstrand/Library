using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IDataInterface
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public byte Condition { get; set; }

        public double Cost { get; set; }

        public long ISBN { get; set; }

        public int ShelfID { get; set; }

        public Shelf Shelf { get; set; }

        public int WasteListID { get; set; }

        public WasteList WasteList { get; set; }

        public Borrow Borrow { get; set; }
    }
}

