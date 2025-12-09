using Confluent.Kafka;
using Consumer.Interfaces;

namespace Consumer.Services
{
    public class ProducerService : IProducer
    {
        // Topic on which to produce messages
        string topic;

        // Configuration options for Kafka producer
        readonly ProducerConfig config;

        IProducer<string, string> producer;

        public ProducerService() {
            this.topic = "messages";
            this.config = new ProducerConfig
            {
                BootstrapServers = "kafka:9092",

                Acks = Acks.All
            };
            this.producer = new ProducerBuilder<string, string>(config).Build();
        }

        /// <summary>
        /// Publishes a message with a given count to the Kafka topic
        /// </summary>
        /// <param name="count">
        /// The integer value that is specified to be published on the message
        /// </param>
        public void Produce(int count)
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
                });


            producer.Flush();

        }
    }
}
