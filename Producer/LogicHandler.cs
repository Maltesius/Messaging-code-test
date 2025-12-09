using Producer.Interfaces;

namespace Producer
{
    public class LogicHandler(IProducer producerService) : ILogicHandler
    {
        IProducer producerService = producerService;

        public async Task StartProgram()
        {
            await producerService.Produce();
        }
    }
}
