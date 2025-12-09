using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Testing
{
    public class ProducerSpy : IProducer
    {
        public bool hasPublished = false;
        public int countToPublish;

        public void Produce(int count)
        {
            hasPublished = true;
            countToPublish = count;
        }
    }
}
