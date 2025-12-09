using Producer.Interfaces;

namespace Producer
{
    public class LogicHandler(IProducer producerService, PeriodicTimer timer) : ILogicHandler
    {
        IProducer producerService = producerService;

        PeriodicTimer timer = timer;
        public async Task StartProgram()
        {
            while (await timer.WaitForNextTickAsync())
            {
                await producerService.Produce();
            }
        }
    }
}
