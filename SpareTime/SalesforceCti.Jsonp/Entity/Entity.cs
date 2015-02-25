using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spare.Jsonp.Entity
{
    public class Entity
    {
        private Guid _id;
        private DateTime _lastHeartbeat;
        private Generic.BlockingQueue _bQueue;
        private const int QUEUE_TIMEOUT = 10000;

        public Entity()
        {
            _id = Guid.NewGuid();
            _lastHeartbeat = DateTime.UtcNow;
            _bQueue = new Generic.BlockingQueue();
        }

        public Guid Id
        {
            get { return _id; }
        }

        public DateTime LastHeartbeat
        {
            get { return _lastHeartbeat; }
            set { _lastHeartbeat = value; }
        }

        internal void Heartbeat()
        {
            _lastHeartbeat = DateTime.UtcNow;
        }

        internal Newtonsoft.Json.Linq.JObject PollEvent()
        {
            Newtonsoft.Json.Linq.JObject retVal = default(Newtonsoft.Json.Linq.JObject);
            _bQueue.TryDequeue(out retVal, QUEUE_TIMEOUT);//check state...
            return retVal;
        }
    }
}
