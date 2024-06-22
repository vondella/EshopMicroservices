using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.Messaging.Events.PostPublishEvents
{
    public  class MessageUpdatedPubEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }

    }
}
