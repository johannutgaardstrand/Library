using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public enum BorrowErrorCodes
    {
        ok,
        BookAlreadyBorrowed,
        CustomerNotAllowedToBorrowMoreBooks,
        CustomerBorrowHasBeenExteneded,
        NoSuchCustomer,
        NoSuchBook,
    }
}
