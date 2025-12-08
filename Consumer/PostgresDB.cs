using Npgsql;

namespace Consumer
{
    public class PostgresDB : IDatabase
    {
        private static string Host = "localhost";
        private static string User = "postgres";
        private static string DBName = "postgres";
        private static string Password = "test";
        private static string Port = "5432";

        private static string connString = $"Server={Host};Username={User};Database={DBName};Port={Port};Password={Password};SSLMode=Prefer";


        public void addRowToDB(int count, DateTime timeStamp)
        {
            using (var conn = new NpgsqlConnection(connString)) 
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                

                using (NpgsqlCommand cmd = new("CREATE TABLE IF NOT EXISTS messages(message_id SERIAL PRIMARY KEY, count INTEGER, timestamp TIMESTAMP)", conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating table");
                }

                using (NpgsqlCommand cmd = new("INSERT INTO messages (count, timestamp) VALUES (@c1, @t1)",conn))
                {
                    cmd.Parameters.AddWithValue("c1", count);
                    cmd.Parameters.AddWithValue("t1", timeStamp);

                    int nRows = cmd.ExecuteNonQuery();
                    Console.Out.WriteLine($"Number of rows inserted={nRows}");
                }

                conn.CloseAsync().Wait();

            }



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
