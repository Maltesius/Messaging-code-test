// See https://aka.ms/new-console-template for more information

using Consumer;
using System.Runtime.CompilerServices;

ConsumerService consumerService = new();

PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
while (await timer.WaitForNextTickAsync())
{
    var res = consumerService.StartConsuming();
    if (res == null)
    {
        continue;
    }

    var resValues = res.GetValueOrDefault();

    int count = resValues.Item1;
    DateTime timeStamp = resValues.Item2;

    double messageAge = (DateTime.UtcNow - timeStamp).TotalSeconds;
    Console.WriteLine(messageAge);

    if (count == -1)
    {
        Console.WriteLine($"Invalid count value from message - Discarding message from {timeStamp}");
        continue;
    }

    if (messageAge > 60.0)
    {
        Console.WriteLine($"Message older than 1 minute - Discarding message from {timeStamp}");
        continue;
    }

    int timestampinSeconds = timeStamp.Second;

    bool isEven = timestampinSeconds % 2 == 0;

    if (isEven) {
        EvenSeconds(count, timeStamp);
    } else {
        OddSeconds(count);
    }

    
    
 


}

void EvenSeconds(int count, DateTime timeStamp)
{
    Console.WriteLine("Second even - Adding message to DB");

}

void OddSeconds(int count)
{
    Console.WriteLine("Odd seconds on message - put back into queue");
    count++;

    PublisherService publisherService = new(count);
    publisherService.Publish();

}





