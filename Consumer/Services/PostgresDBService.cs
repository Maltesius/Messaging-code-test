using Consumer.Interfaces;
using Npgsql;

namespace Consumer.Services
{
    public class PostgresDBService : IDatabase
    {
        // Initial setup parameters for the PostgreSQL container db
        private static string Host = "db";
        private static string User = "postgres";
        private static string DBName = "postgres";
        private static string Password = "test";
        private static string Port = "5432";

        private static string connString = $"Server={Host};Username={User};Database={DBName};Port={Port};Password={Password};SSLMode=Prefer";


        private NpgsqlConnection? conn;

        /// <summary>
        /// Inserts a new row in the 'messages' table with the given 'count' and 'timeStamp'
        /// </summary>
        /// <remarks>
        /// Cancels the operation if no connection exists to the database
        /// </remarks>
        /// <param name="count">The count of the given message to be added to the db</param>
        /// <param name="timeStamp">The timestamp of the given message to be added to the db</param>
        public void addRowToDB(int count, DateTime timeStamp)
        {

            if (conn == null)
            {
                Console.Out.WriteLine("no connection to db - cannot add row");
                return;
            }

            // We need to ensure that the table 'messages' actually exists
            using (NpgsqlCommand cmd = new("CREATE TABLE IF NOT EXISTS messages(message_id SERIAL PRIMARY KEY, count INTEGER, timestamp TIMESTAMP)", conn))
            {
                cmd.ExecuteNonQuery();
                Console.Out.WriteLine("Finished creating table");
            }

            // Do the insert operation
            using (NpgsqlCommand cmd = new("INSERT INTO messages (count, timestamp) VALUES (@c1, @t1)", conn))
            {
                cmd.Parameters.AddWithValue("c1", count);
                cmd.Parameters.AddWithValue("t1", timeStamp);

                int nRows = cmd.ExecuteNonQuery();
                Console.Out.WriteLine($"Number of rows inserted={nRows}");
            }

        }

        /// <summary>
        /// Opens a connection to the PostgreSQL database using the parameters in the class
        /// </summary>
        public void connectToDB()
        {
            conn = new NpgsqlConnection(connString);

            Console.WriteLine("Opening connection to PostgreSQL db");
            conn.OpenAsync().Wait();
        }

        /// <summary>
        /// Closes the current database connection if one is established.
        /// </summary>
        /// <remarks>
        /// No action is done if there does not exist a connection to the database. Waits for the connection to close
        /// </remarks>
        public void closeConnection()
        {
            if (conn == null)
            {
                Console.WriteLine("Cannot close connection - No connection established");
            } else {
                conn.CloseAsync().Wait();
            }
                
        }
    }
}
