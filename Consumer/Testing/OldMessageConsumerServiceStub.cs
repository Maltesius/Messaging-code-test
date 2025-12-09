using Consumer.Interfaces;
using Consumer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Testing
{
    public class OldMessageConsumerServiceStub : IConsumer
    {
        public (int, DateTime)? ConsumeMessage()
        {
            return (1, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(2)));
        }
    }
}
