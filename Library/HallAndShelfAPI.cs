using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IDataInterface;

namespace Library
{
    public class HallAndShelfAPI
    {
        private IHallManager hallManager;
        private IShelfManager shelfManager;

        public HallAndShelfAPI(IHallManager hallManager, IShelfManager shelfManager)
        {
            this.hallManager = hallManager;
            this.shelfManager = shelfManager;
        }

        public HallErrorCodes RemoveHall(int hallID)
        {
            var hall = hallManager.GetHall(hallID);
            if (hall == null)
            {
                return HallErrorCodes.NoSuchHall;
            }
            else
            {
                if (hall.Shelves.Count > 0)
                {
                    return HallErrorCodes.ThereAreShelvesInThisHall;
                }
                else
                {
                    hallManager.RemoveHall(hall);
                    return HallErrorCodes.ok;
                }
            }
        }
        public ShelfErrorCodes AddShelf(int hallID, int shelfNumber)
        {
            var existingHall = hallManager.GetHall(hallID);
            if (existingHall == null)
                return ShelfErrorCodes.NoSuchHall;
            else
            {
                var existingShelf = shelfManager.GetShelfByShelfNumber(hallID, shelfNumber);
                if (existingShelf != null)
                    return ShelfErrorCodes.ShelfNumberOccupied;
                shelfManager.AddShelf(hallID, shelfNumber);
                return ShelfErrorCodes.ok;
            }
        }
        public ShelfErrorCodes RemoveShelf(int shelfID)
        {
            var shelf = shelfManager.GetShelf(shelfID);
            if (shelf == null)
            {
                return ShelfErrorCodes.NoSuchShelf;
            }
            else
            {
                if (shelf.Books != null)
                {
                    return ShelfErrorCodes.TheShelfContainsBooks;
                }
                else
                    shelfManager.RemoveShelf(shelf);
                return ShelfErrorCodes.ok;
            }
        }
        public ShelfErrorCodes ChangeShelf(int shelfID, int hallID = 0, int shelfNumber = 0)
        {
            var shelf = shelfManager.GetShelf(shelfID);
            if (shelf == null)
            {
                return ShelfErrorCodes.NoSuchShelf;
            }
            else
            {
                if (hallID != 0)
                {
                    var existingHall = hallManager.GetHall(hallID);
                    if (existingHall == null)
                    {
                        return ShelfErrorCodes.NoSuchHall;
                    }
                    shelfManager.ChangeShelfHall(shelf, hallID);
                    return ShelfErrorCodes.TheShelfHasChangedHall;

                }
                else
                {
                    var existingShelfNumber = shelfManager.GetShelfByShelfNumber(shelf.HallID, shelfNumber);
                    if (existingShelfNumber != null)
                    {
                        return ShelfErrorCodes.ShelfNumberOccupied;
                    }
                    else
                        shelfManager.ChangeShelfNumber(shelf, shelfNumber);
                    return ShelfErrorCodes.TheShelfNumberHasBeenChanged;
                }
            }
        }
    }
}
