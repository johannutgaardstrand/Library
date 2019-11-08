using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IDataInterface
{
    public class Shelf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ShelfID { get; set; }

        public int ShelfNumber { get; set; }

        public int HallID { get; set; }

        public Hall Hall { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}

