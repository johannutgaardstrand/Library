using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ShelfManager : IShelfManager
    {
        public void AddShelf(int hallID, int ShelfNumber)
        {
            using var context = new LibraryContext();
            var shelf = new Shelf();
            shelf.HallID = hallID;
            shelf.ShelfNumber = ShelfNumber;
            context.Shelves.Add(shelf);
            context.SaveChanges();
        }

        public void ChangeShelfHall(Shelf shelf, int newHallID)
        {
            using var context = new LibraryContext();
            shelf.HallID = newHallID;
            context.SaveChanges();
        }

        public void ChangeShelfNumber(Shelf shelf, int newShelfNumber)
        {
            using var context = new LibraryContext();
            shelf.ShelfNumber = newShelfNumber;
            context.SaveChanges();
        }

        public Shelf GetShelf(int shelfID)
        {
            using var context = new LibraryContext();
            return (from s in context.Shelves
                    where s.ShelfID == shelfID
                    select s)
                    .Include(s => s.Books)
                    .FirstOrDefault();
        }

        public Shelf GetShelfByShelfNumber(int hallID, int shelfNumber)
        {
            using var context = new LibraryContext();
            return (from s in context.Shelves
                    where s.HallID == hallID && s.ShelfNumber == shelfNumber
                    select s)
                    .FirstOrDefault();
        }

        public void RemoveShelf(Shelf shelf)
        {
            using var context = new LibraryContext();
            context.Shelves.Remove(shelf);
            context.SaveChanges();
        }
    }
}
