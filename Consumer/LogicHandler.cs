using Confluent.Kafka;
using Consumer.Interfaces;

namespace Consumer
{
    public class LogicHandler(IConsumer consumerService, IProducer producerService, IDatabase db) : ILogicHandler
    {
        IConsumer consumerService = consumerService;
        IProducer producerService = producerService;
        IDatabase db = db;

        /// <summary>
        /// Starts the main program loop that consumes messages from the Kafka topic and processes them based on the timestamp values
        /// </summary>
        /// <returns>Runs indefinitely</returns>
        public async Task Consume()
        {
            // consumerService will handle incoming messages from kafka topic and return a tuple of (int count, DateTime timestamp) or null
            var res = consumerService.ConsumeMessage();

            // If no message was received, continue to next iteration
            if (res == null)
            {
                return;
            }

            // Unpack the tuple
            var resValues = res.GetValueOrDefault();
            int count = resValues.Item1;
            DateTime timeStamp = resValues.Item2;

            // Get the age of the message in seconds (UTC)
            double messageAge = (DateTime.UtcNow - timeStamp).TotalSeconds;

            // Count is -1 if invalid count value was received
            if (count == -1)
            {
                Console.WriteLine($"Invalid count value from message - Discarding message from {timeStamp}");
                return;
            }

            // Discard messages older than 1 minute
            if (messageAge > 60.0)
            {
                Console.WriteLine($"Message older than 1 minute - Discarding message from {timeStamp}");
                return;
            }


            // Check if the timestamp seconds value is even or odd
            int timestampinSeconds = timeStamp.Second;
            bool isEven = timestampinSeconds % 2 == 0;

            if (isEven)
            {
                db.connectToDB();
                EvenSeconds(count, timeStamp);
                db.closeConnection();
            }
            else
            {
                OddSeconds(count);
            }

        }



        /// <summary>
        /// This method handles the logic for even seconds timestamps
        /// <param name="count">The count value from the message</param>"
        /// <param name="timeStamp">The timestamp value from the message</param>"
        /// </summary>
        public void EvenSeconds(int count, DateTime timeStamp)
        {
            Console.WriteLine("Seconds even - Adding message to DB");
            db.addRowToDB(count, timeStamp);
        }

        /// <summary>
        /// This method handles the logic for odd seconds timestamps
        /// <param name="count">The count value from the message</param>"
        /// </summary>
        public void OddSeconds(int count)
        {
            Console.WriteLine("Odd seconds on message - put back into queue");
            count++;

            // Start up a publisher service to republish the message with incremented count and new timestamp
            producerService.Produce(count);

        }



    }
}
