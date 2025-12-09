using Consumer;
using Consumer.Interfaces;
using Consumer.Services;
using Consumer.Testing;

namespace ConsumerTests
{
    [TestClass]
    public class ConsumerTests
    {

        [TestMethod]
        public async Task NoKafkaConnectionStopsExecution()
        {
            // Arrange
            IConsumer consumerService = new DisconnectedConsumerServiceSpy();
            IProducer publisherService = new ProducerService();
            IDatabase dbService = new PostgresDBSpy();

            ILogicHandler logicHandler = new LogicHandler(consumerService, publisherService, dbService);

            Assert.Throws<InvalidOperationException>(() => logicHandler.Consume().GetAwaiter().GetResult());
        }

        [TestMethod]
        public async Task ConnectedKafkaReturns()
        {
            // Arrange
            IConsumer consumerService = new ConnectedConsumerServiceSpy();
            IProducer publisherService = new ProducerSpy();
            IDatabase dbService = new PostgresDBSpy();

            ILogicHandler logicHandler = new LogicHandler(consumerService, publisherService, dbService);

            // Act & Assert
            logicHandler.Consume().GetAwaiter().GetResult();
            
        }

        [TestMethod]
        public void AddRowToDBWithoutConnectionFails()
        {
            // Arrange
            IConsumer consumerService = new ConsumerService();
            IProducer publisherService = new ProducerService();
            PostgresDBSpy dbSpy = new();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);
            
            // Act
            logicHandler.EvenSeconds(0, DateTime.UtcNow);
            var actual = dbSpy.WasRowAdded();

            // Assert

            Assert.IsFalse(actual);

        }

        [TestMethod]
        public void AddRowToDBWithConnectionSucceeds() 
        {
            // Arrange
            IConsumer consumerService = new ConsumerService();
            IProducer publisherService = new ProducerService();
            PostgresDBSpy dbSpy = new PostgresDBSpy();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);

            // Act
            dbSpy.connectToDB();
            logicHandler.EvenSeconds(0, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(dbSpy.WasRowAdded());
        }

        [TestMethod]
        public void OldMessageIsDiscarded()
        {
            // Arrange
            IConsumer consumerService = new OldMessageConsumerServiceStub();
            ProducerSpy publisherService = new ProducerSpy();
            PostgresDBSpy dbSpy = new PostgresDBSpy();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);

            // Act
            logicHandler.Consume().GetAwaiter().GetResult();

            // Assert
            Assert.IsFalse(publisherService.hasPublished);
            Assert.IsFalse(dbSpy.WasRowAdded());
        }



        [TestMethod]
        public void OddSecondsPublishesMessage()
        {
            // Arrange
            IConsumer consumerService = new ConsumerService();
            ProducerSpy publisherService = new ProducerSpy();
            PostgresDBSpy dbSpy = new PostgresDBSpy();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);

            // Act
            logicHandler.OddSeconds(1);

            // Assert
            Assert.IsTrue(publisherService.hasPublished);
        }

        [TestMethod]
        public void OddSecondsIncrementsMessageCount()
        {
            // Arrange
            IConsumer consumerService = new ConsumerService();
            ProducerSpy publisherService = new ProducerSpy();
            PostgresDBSpy dbSpy = new PostgresDBSpy();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);

            // Act
            logicHandler.OddSeconds(1);

            // Assert
            Assert.AreEqual(2, publisherService.countToPublish);

        }

        [TestMethod]
        public void EvenSecondsDoesNotPublishMessage()
        {
            // Arrange
            IConsumer consumerService = new ConsumerService();
            ProducerSpy publisherService = new ProducerSpy();
            PostgresDBSpy dbSpy = new PostgresDBSpy();

            var logicHandler = new LogicHandler(consumerService, publisherService, dbSpy);

            // Act
            logicHandler.EvenSeconds(0, DateTime.UtcNow);

            // Assert
            Assert.IsFalse(publisherService.hasPublished);
        }
    }
}
