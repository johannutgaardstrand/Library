using System;
using System.Collections.Generic;
using System.Text;

namespace IDataInterface
{
    public interface IHallManager
    {
        public void AddHall();
        public Hall GetHall(int hallID);

        public void RemoveHall(Hall hall);
    }
}
