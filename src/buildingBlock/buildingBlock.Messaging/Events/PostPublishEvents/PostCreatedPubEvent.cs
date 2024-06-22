using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.Messaging.Events.PostPublishEvents
{
    public  class PostCreatedPubEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
