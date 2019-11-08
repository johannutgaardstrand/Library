using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using IDataInterface;

namespace DataAccess
{
    public class LibraryContext : DbContext
    {
        private const string connectionString = "Server=LAPTOP-ISMLQO8T;Database=EFCore;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Hall> Halls { get; set; }

        public DbSet<Shelf> Shelves { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Customer> Custumers { get; set; }

        public DbSet<WasteList> WasteLists { get; set; }

        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Bill> Bills { get; set; }
    }

}
