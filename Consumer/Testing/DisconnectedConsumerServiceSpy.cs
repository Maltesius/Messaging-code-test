using Confluent.Kafka;
using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Testing
{
    public class DisconnectedConsumerServiceSpy : IConsumer
    {
        public (int, DateTime)? ConsumeMessage()
        {
            throw new InvalidOperationException("No connection to kafka message service");
        }
    }
}
