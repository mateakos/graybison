using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spare.Tsapi;

namespace Spare.Jsonp.Entity
{
    public class Entity
    {
        private Guid _id;
        private DateTime _lastHeartbeat;
        private Generic.BlockingQueue _bQueue;
        private const int QUEUE_TIMEOUT = 10000;
        private TelephonyServiceManager _ctiService;

        private string _extension;
        private string _agent;
        private string _password;

        public Entity(TelephonyServiceManager telephonyProvider)
        {
            _id = Guid.NewGuid();
            _lastHeartbeat = DateTime.UtcNow;
            _bQueue = new Generic.BlockingQueue();
            _ctiService = telephonyProvider;

            _ctiService.OnLoggedOnEvent += _ctiService_OnLoggedOnEvent;
            _ctiService.OnStartMonitorResponse += _ctiService_OnStartMonitorResponse;
        }

        #region CTI Events

        void _ctiService_OnStartMonitorResponse(string subjectDevice, Tsapi.MonitorConfirmationEventArgs e)
        {
            
            _extension = subjectDevice;
            
            _bQueue.Enqueue(  new Generic.Response()
            {
                Identifier = Id,
                Data = "Station monitorred",
                Result = true,
                Event = Generic.MessageConstant.EVT_MONITOR_START
            });
        }

        void _ctiService_OnLoggedOnEvent(string subjectDevice, Tsapi.LoggedOnEventArgs e)
        {
            Console.WriteLine("Agent logged in event ");
        }

        #endregion

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

        internal Generic.Response PollEvent()
        {
            Generic.Response retVal = default(Generic.Response);
            _bQueue.TryDequeue(out retVal, QUEUE_TIMEOUT);//check state...
            return retVal;
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
            return _ctiService.Monitor(extension);
        }
    }
}
