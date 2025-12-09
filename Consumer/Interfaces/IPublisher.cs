using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Interfaces
{
    public interface IPublisher
    {
        void Publish(int count);
    }
}
