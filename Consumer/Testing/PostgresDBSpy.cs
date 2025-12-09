using Consumer.Interfaces;

namespace Consumer.Testing
{
    public sealed class PostgresDBSpy : IDatabase
    {
        private bool openedDB = false;
        private bool rowAdded = false;

        public void addRowToDB(int count, DateTime timeStamp)
        {
            if (openedDB)
            {
                rowAdded = true;
            }
        }

        public void closeConnection()
        {
            openedDB = false;
        }

        public void connectToDB()
        {
            openedDB = true;
        }

        public bool WasDBOpened()
        {
            return openedDB;
        }

        public bool WasRowAdded()
        {
            return rowAdded;
        }
    }
}
