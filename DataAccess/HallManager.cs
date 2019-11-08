using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class HallManager : IHallManager
    {
        public void AddHall()
        {
            var hall = new Hall();
            using var context = new LibraryContext();
            context.Add(hall);
            context.SaveChanges();
        }
        public Hall GetHall(int hallID)
        {
            using var context = new LibraryContext();
            return (from h in context.Halls
                    where h.HallID == hallID
                    select h)
                    .Include(h => h.Shelves)
                    .FirstOrDefault();
        }

        public void RemoveHall(Hall hall)
        {
            using var context = new LibraryContext();
            context.Halls.Remove(hall);
            context.SaveChanges();
        }
    }
}
