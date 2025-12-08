using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class PostgresDB : IDatabase
    {
        public void addRowToDB(int count, DateTime timeStamp)
        {
            throw new NotImplementedException();
        }

        public void connectToDB()
        {
            throw new NotImplementedException();
        }

        public bool pingDB()
        {
            throw new NotImplementedException();
        }
    }
}
