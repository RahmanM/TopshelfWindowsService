using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServiceTopShelf
{

    public class DatabaseHelper : IDatabaseHelper
    {
        public void GetData()
        {
            Console.WriteLine("Getting data");
        }
    }
}
