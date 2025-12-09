using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Testing
{
    public class ConnectedConsumerServiceSpy : IConsumer
    {
        public (int, DateTime)? ConsumeMessage()
        {
            return (1, DateTime.UtcNow);
        }
    }
}
