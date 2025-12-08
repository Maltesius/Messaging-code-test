using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class PublisherService(int count) : IPublisher
    {
        readonly int count = count;
        const string topic = "messages";

        readonly ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",

            Acks = Acks.All
        };

        public void Publish()
        {

            var producer = new ProducerBuilder<string, string>(config).Build();

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
