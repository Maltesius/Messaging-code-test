using Confluent.Kafka;

namespace Producer
{
    public class ProducerService : IProducer
    {
        // hardcoded configs, modification can be added through constructor

        // Messages always start the count with 0
        int count = 0;

        // Topic to produce kafka messages to
        const string topic = "messages";

        // Configuration for Kafka container producer
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",

            Acks = Acks.All
        };

        IProducer<string, string>? producer;

        /// <summary>
        /// Starts the asynchronous process of producing messages to the configured kafka service at set intervals 
        /// </summary>
        /// <remarks>
        /// Interval is currently set to 5 seconds, could be made modular in the future. Method runs indefinitely until the application is terminated.
        /// </remarks>
        /// <returns>
        /// A Task - only returns when application is terminated. 
        /// </returns>
        public async Task StartProducing()
        {
            // If producer is not initialized, create a new one
            producer ??= new ProducerBuilder<string, string>(config).Build();

            // Set up a periodic timer to produce messages every 5 seconds
            // Could possibly be modular in the future 
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await timer.WaitForNextTickAsync())
            {
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

                    }

                    );

                producer.Flush();
                
            }
        }
    }
}
