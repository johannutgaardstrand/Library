using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IShelfManager
    {
        public void AddShelf(int hallID, int ShelfNumber);

        public Shelf GetShelf(int ShelfID);

        public Shelf GetShelfByShelfNumber(int hallID, int shelfNumber);

        public void RemoveShelf(Shelf shelf);

        public void ChangeShelfHall(Shelf shelf, int newHallID);

        public void ChangeShelfNumber(Shelf shelf, int newShelfNumber);
    }
}
