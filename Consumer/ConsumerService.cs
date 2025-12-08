using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class ConsumerService : IConsumer
    {
        ConsumerConfig config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",

            GroupId = "test",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        const string topic = "messages";

        CancellationTokenSource token = new();

        IConsumer<string, string> consumer;

        public ConsumerService ()
        {
            consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);
        }

        public (int, DateTime)? StartConsuming()
        {


            var res = consumer.Consume(1000);

            
            if (res == null)
            {
                
                return null;
            }

            int value;
            try
            {
                value = Int32.Parse(res.Message.Value);
            }
            catch (Exception ex) 
            {
                value = -1;
            }
            
            DateTime ts = res.Message.Timestamp.UtcDateTime;
            Console.WriteLine($"Consumed event from topic: {topic}, key = {res.Message.Key}, value = {value}, timestamp = {ts.ToLongTimeString()}");

            consumer.Commit(res);

            return (value, ts);
            
        }
    }
}
