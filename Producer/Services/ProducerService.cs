using Confluent.Kafka;
using Producer.Interfaces;

namespace Producer.Services
{
    public class ProducerService : IProducer
    {

        readonly int count;

        readonly string topic;

        readonly ProducerConfig config;

        readonly IProducer<string, string> producer;

        public ProducerService()
        {
            // Messages always start the count with 0
            this.count = 0;

            // hardcoded configs, modification can be added through constructor

            // Configuration for Kafka container producer
            this.topic = "messages";

            // Configuration for Kafka container producer
            this.config = new ProducerConfig
            {
                BootstrapServers = "kafka-service:9092",

                Acks = Acks.All
            };

            this.producer = new ProducerBuilder<string, string>(config).Build();
        }

        /// <summary>
        /// Starts the asynchronous process of producing messages to the configured kafka service at set intervals 
        /// </summary>
        /// <remarks>
        /// Interval is currently set to 5 seconds, could be made modular in the future. Method runs indefinitely until the application is terminated.
        /// </remarks>
        /// <returns>
        /// A Task - only returns when application is terminated. 
        /// </returns>
        public async Task Produce()
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
