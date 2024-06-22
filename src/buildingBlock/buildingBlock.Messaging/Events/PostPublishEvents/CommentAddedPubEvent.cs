using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.Messaging.Events.PostPublishEvents
{
    public  class CommentAddedPubEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
