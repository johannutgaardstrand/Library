using System;
using System.Globalization;

namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date1 = new DateTime(2018, 7, 15, 08, 15, 20);
            DateTime date2 = new DateTime(2008, 8, 17, 11, 14, 25);
            TimeSpan ts = date2 - date1;
            Console.WriteLine("No. of Minutes (Difference) = {0}", ts.Days);
            Console.WriteLine("nooooo");
        }
    }
}
