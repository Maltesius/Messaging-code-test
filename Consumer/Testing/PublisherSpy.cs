using Consumer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Testing
{
    public class PublisherSpy : IPublisher
    {
        public bool hasPublished = false;
        public int countToPublish;

        public void Publish(int count)
        {
            hasPublished = true;
            countToPublish = count;
        }
    }
}
