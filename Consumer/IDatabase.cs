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
        void closeConnection();

        void addRowToDB(int count, DateTime timeStamp);
    }
}
