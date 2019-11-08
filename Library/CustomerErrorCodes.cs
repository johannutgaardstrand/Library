using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public enum CustomerErrorCodes
    {
        ok,
        CustomerToYoungMustHaveParent,
        CustomerDateOfBirthIsIncorrect,
        okUnderageCustomerWithParentAdded,
        NoSuchCustomer,
        CustomerStillHasBorrowedBooks,
        CustomerStillOwesMoney,
        
    }
}
