using Consumer;
using Consumer.Interfaces;

// Set up consumer service for kafka messages
IConsumer consumerService = new ConsumerService();
IPublisher publisherService = new PublisherService();
IDatabase db = new PostgresDBService();
PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

LogicHandler logicHandler = new LogicHandler(consumerService, publisherService, db, timer);
await logicHandler.StartProgram();






