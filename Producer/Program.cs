using Producer;
using Producer.Interfaces;

// Get timer length from environment variable or default to 5 seconds
int timerLength = Environment.GetEnvironmentVariable("TIMER") != null ? Int32.Parse(Environment.GetEnvironmentVariable("TIMER")) : 5;
Console.WriteLine($"Timer set to {timerLength} seconds.");

// Start the producer service
PeriodicTimer timer = new(TimeSpan.FromSeconds(timerLength));
IProducer producerService = new ProducerService();

LogicHandler logicHandler = new LogicHandler(producerService, timer);

await logicHandler.StartProgram();











