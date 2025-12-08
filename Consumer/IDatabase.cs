using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal interface IDatabase
    {

        void connectToDB();
        bool pingDB();

        void addRowToDB(int count, DateTime timeStamp);
    }
}
