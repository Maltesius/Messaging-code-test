using Producer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    public class ProducerSpy : IProducer
    {
        public bool hasProduced = false;
        public Task Produce()
        {
            hasProduced = true;
            return Task.CompletedTask;
        }
    }
}
