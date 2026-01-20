using Confluent.Kafka;
using Consumer.Interfaces;

namespace Consumer.Services
{
    /// <summary>
    /// The class for handling a Kafka consumer service
    /// </summary>
    public class ConsumerService : IConsumer
    {

        // Kafka consumer configuration options
        // BootstrapServers uses the Kafka broker Docker container address and NOT localhost
        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = "kafka-service:9092",

            GroupId = "test",
            AllowAutoCreateTopics = true,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        const string topic = "messages";

        IConsumer<string, string> consumer;

        /// <summary>
        /// Initializes the consumer service with the given configuration and subscribes to the topic
        /// </summary>
        public ConsumerService ()
        {
            consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);
        }

        /// <summary>
        /// Consumes a message from the Kafka topic
        /// </summary>
        /// <returns>A tuple of (int count, DateTime) if message is consumed on topic - null if no message was found on topic</returns>
        public (int, DateTime)? ConsumeMessage()
        {

            ConsumeResult<string,string>? res;
            try
            {
                // Try to consume message with 1 second max timeout
                res = consumer.Consume(1000);
            } catch (ConsumeException e)
            {
                // No message was found on topic within timeout period so the result is returned as null
                Console.WriteLine($"Error occured: {e.Error.Reason}");
                res = null;
            }

            // Useful for making sure that res is not null before accessing its properties
            if (res == null)
            {
                return null;
            }


            // Value from kafka message is a string so it needs to be parsed to an int
            int value;
            try
            {
                value = Int32.Parse(res.Message.Value);
            }
            catch (Exception ex) 
            {
                // Messages with invalid or non-parseable count values are discarded by returning -1 as the count value
                value = -1;
            }

            // Get the timestamp from the message as UTC DateTime object
            DateTime ts = res.Message.Timestamp.UtcDateTime;
            Console.WriteLine($"Consumed event from topic: {topic}, key = {res.Message.Key}, value = {value}, timestamp = {ts.ToLongTimeString()}");

            consumer.Commit(res);

            return (value, ts);
            
        }
    }
}
