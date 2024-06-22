using postcmd.posts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.Messaging.Producers
{
    public  interface IEventProducer
    {
        Task ProduceAsync<T>(string Topic, T @event) where T : BaseEvent;
    }
}
