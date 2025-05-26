using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDC_F24.Application.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void Publish(string queueName, object message);
    }
}
