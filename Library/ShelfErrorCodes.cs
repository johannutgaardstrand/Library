using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public enum ShelfErrorCodes
    {
        ok,
        NoSuchHall,
        ShelfNumberOccupied,
        NoSuchShelf,
        TheShelfContainsBooks,
        TheShelfNumberHasBeenChanged,
        TheShelfHasChangedHall,
    }
}
