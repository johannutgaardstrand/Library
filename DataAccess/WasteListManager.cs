using IDataInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class WasteListManager : IWasteListManager
    {
        public WasteList GetWasteList(int wasteListID)
        {
            using var context = new LibraryContext();
            return (from wl in context.WasteLists
                    where wasteListID == wl.WasteListID
                    select wl)
                    .Include(wl => wl.Books)
                    .ThenInclude(wl => wl.Shelf)
                    .ThenInclude(wl => wl.Hall)
                    .FirstOrDefault();
        }

        public void MakeWasteList(List<Book> books)
        {
            using var context = new LibraryContext();
            var wasteList = new WasteList();
            wasteList.Books = books;
            context.WasteLists.Add(wasteList);
            context.SaveChanges();
        }

        public void RemoveWasteList(WasteList wasteList)
        {
            using var context = new LibraryContext();
            context.WasteLists.Remove(wasteList);
            context.SaveChanges();
        }
    }
}
