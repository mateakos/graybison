using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spare.Tsapi;

namespace Spare.Jsonp.Entity
{
    public class EntityItem
    {
        private Guid _id;
        private DateTime _lastHeartbeat;
        private Generic.BlockingQueue _bQueue;
        private const int QUEUE_TIMEOUT = 10000;
        private CTIService _ctiService;

        private string _extension;
        private string _agent;
        private string _password;
        private bool _monitorred;

        public EntityItem(CTIService telephonyProvider)
        {
            _id = Guid.NewGuid();
            _lastHeartbeat = DateTime.UtcNow;
            _bQueue = new Generic.BlockingQueue();
            _ctiService = telephonyProvider;
        }

        #region CTI Events

        internal void ConnectionClearedEvent(ConnectionClearedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_CONN_CLEARED
            });
        }

        internal void QueuedEvent(QueuedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_QUEUED
            });
        }

        internal void ConferencedEvent(ConferencedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_CONFERENCE
            });
        }

        internal void TransferredEvent(TransferredEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_TRANSFER
            });
        }

        internal void RetrievedEvent(RetrievedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_RETRIEVED
            });
        }

        internal void HeldEvent(HeldEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_HELD
            });
        }

        internal void FailedEvent(FailedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_CALL_FAILED
            });
        }

        internal void NetworkReachedEvent(NetworkReachedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_NETWORK_REACHED
            });
        }

        internal void OriginatedEvent(OriginatedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_ORIGINATED
            });
        }

        internal void DivertedEvent(DivertedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_DIVERTED
            });
        }

        internal void EstablishedEvent(EstablishedEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_ESTABLISHED
            });
        }

        internal void DeliveredEvent(DeliveredEventArgs e)
        {
            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_DELIVERED
            });
        }


        internal void ServiceInitiatedEvent(ServiceInitiatedEventArgs e)
        {

            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(e),
                Result = true,
                Event = Generic.MessageConstant.EVT_SERVICE_INIT
            });
        }

        internal void StartMonitorResponse(Tsapi.MonitorConfirmationEventArgs e)
        {
            this._monitorred = true;
            
            _bQueue.Enqueue(  new Generic.Response()
            {
                Identifier = Id,
                Data = "Station monitorred",
                Result = true,
                Event = Generic.MessageConstant.EVT_MONITOR_START
            });
        }

        internal void MonitorEndedEvent(MonitorEndedEventArgs e)
        {
            this._monitorred = false;

            _bQueue.Enqueue(new Generic.Response()
            {
                Identifier = Id,
                Data = "Station monitor ended",
                Result = true,
                Event = Generic.MessageConstant.EVT_MONITOR_STOP
            });
        }

        //internal void QueueEvent(Generic.MessageConstant eventType, object eventArgs)
        //{
 
        //}

        internal void LoggedOnEvent(Tsapi.LoggedOnEventArgs e)
        {
            Console.WriteLine("Agent logged in event ");
        }

        #endregion

        public Guid Id
        {
            get { return _id; }
        }

        public string Extension
        {
            get { return _extension; }
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

        internal Generic.Response PollEvent()
        {
            Generic.Response retVal = null;
            _bQueue.TryDequeue(out retVal, QUEUE_TIMEOUT);//check state...
            return retVal != null ? retVal : new Generic.Response(this.Id);
        }

        internal bool LoginAgent(string station, string agent, string password, int workmode)
        {
            return _ctiService.LoginAgent(station, agent, password, workmode);
        }

        internal bool LogoutAgent(string station, string agent, int reason)
        {
            return _ctiService.LogoutAgent(station, agent, reason);
        }

        internal bool SetAgentState(string station, string agent, int reason, int workmode)
        {
            return _ctiService.SetAgentState(station, agent, reason, workmode);
        }

        internal bool Monitor(string extension)
        {
            this._extension = extension;
            return _ctiService.Monitor(extension);
        }
    }
}
