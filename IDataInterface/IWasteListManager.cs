using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IWasteListManager
    {
        public void MakeWasteList(List<Book> books);

        public WasteList GetWasteList(int wasteListID);

        public void RemoveWasteList(WasteList wasteList);
    }
}
