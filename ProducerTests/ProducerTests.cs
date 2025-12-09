using Producer;
using Producer.Interfaces;
using Producer.Testing;

namespace ProducerTests
{
    [TestClass]
    public sealed class ProducerTests
    {
        [TestMethod]
        public void ProducerPublishesMessage()
        {
            // Arrange
            ProducerSpy producer = new ProducerSpy();
            LogicHandler logicHandler = new LogicHandler(producer);

            // Act
            logicHandler.StartProgram().GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(producer.hasProduced);
        }
    }
}
