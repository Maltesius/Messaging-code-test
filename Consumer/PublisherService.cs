using Confluent.Kafka;

namespace Consumer
{
    internal class PublisherService() : IPublisher
    {
        // Topic on which to produce messages
        const string topic = "messages";
        

        // Configuration options for Kafka producer
        readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",

            Acks = Acks.All
        };

        IProducer<string, string>? producer;

        /// <summary>
        /// Publishes a message with a given count to the Kafka topic
        /// </summary>
        /// <param name="count">
        /// The integer value that is specified to be published on the message
        /// </param>
        public void Publish(int count)
        {
            // If producer is not initialized, create a new one
            producer ??= new ProducerBuilder<string, string>(config).Build();


            Timestamp ts = Timestamp.Default;
            producer.Produce(topic, new Message<string, string> { Key = "count", Value = $"{count}", Timestamp = ts },
                (deliveryReport) =>
                {
                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        Console.WriteLine($"Error in message delivery: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        Console.WriteLine($"Delivered message to topic: {topic}, key: count, value: {count}");
                    }
                });


            producer.Flush();

        }
    }
}
