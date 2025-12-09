using Consumer;
using Consumer.Interfaces;
using Consumer.Services;

// Set up consumer service for kafka messages
IConsumer consumerService = new ConsumerService();
IProducer producerService = new ProducerService();
IDatabase db = new PostgresDBService();
PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

LogicHandler logicHandler = new LogicHandler(consumerService, producerService, db);

// Consume once every second
while (await timer.WaitForNextTickAsync())
{
    await logicHandler.Consume();
}






