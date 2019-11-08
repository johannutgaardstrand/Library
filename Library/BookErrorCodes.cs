using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public enum BookErrorCodes
    {
        ok,
        OkButBookIsPutInAnotherShelfWithExistingBooks,
        NoSuchShelf,
        IncorrectISBN,
        BookBorrowed,
        NoSuchBook,
        BookMoved
    }
}
