using Confluent.Kafka;

namespace Producer
{
    internal class ProducerService : IProducer
    {
        // hardcoded configs, can be modified with constructor

        int count = 0;

        const string topic = "messages";

        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "kafka:9092",

            Acks = Acks.All
        };

        

        public ProducerService() {
            
        }

        public async Task StartProducing()
        {
            var producer = new ProducerBuilder<string, string>(config).Build();

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

                /*
                Message message = new Message();
                for (int i = 0; i<count; i++) {
                    message.UpdateCounter();
                }
                */

                producer.Flush();
                /* Console.WriteLine(message.ToString());*/
            }
        }
    }
}
