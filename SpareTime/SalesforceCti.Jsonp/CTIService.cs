using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spare.Tsapi;
using Spare.Jsonp.Entity;

namespace Spare.Jsonp
{
    public class CTIService
    {
        private static EntityCollection _clients;
        private Spare.Tsapi.TelephonyServiceManager _ctiService;
        private bool _initated = false;

        public CTIService(EntityCollection collection)
        {
            _ctiService = new Tsapi.TelephonyServiceManager("graybison");
            _clients = collection;
            Initialize();
        }

        public bool IsInitialized
        {
            get { return _initated; }
        }

        private void Initialize()
        {
            if (!_ctiService.Open("AVAYA#CM#CSTA#HUBIVSR83", "tman", "Tm@npassw0rd"))
            {
                _ctiService.Shutdown = true;
            }
            else
            {
                _ctiService.StartService();

                _ctiService.OnLoggedOnEvent += _ctiService_OnLoggedOnEvent;
                _ctiService.OnStartMonitorResponse += _ctiService_OnStartMonitorResponse;
                _ctiService.OnMonitorEndedEvent += _ctiService_OnMonitorEndedEvent;
                //Call Events
                _ctiService.OnServiceInitiatedEvent += _ctiService_OnServiceInitiatedEvent;
                _ctiService.OnOriginatedEvent += _ctiService_OnOriginatedEvent;
                _ctiService.OnNetworkReachedEvent += _ctiService_OnNetworkReachedEvent;

                _ctiService.OnFailedEvent += _ctiService_OnFailedEvent;
                _ctiService.OnQueuedEvent += _ctiService_OnQueuedEvent;

                _ctiService.OnDeliveredEvent += _ctiService_OnDeliveredEvent;
                _ctiService.OnDivertedEvent += _ctiService_OnDivertedEvent;
                _ctiService.OnEstablishedEvent += _ctiService_OnEstablishedEvent;

                _ctiService.OnHeldEvent += _ctiService_OnHeldEvent;
                _ctiService.OnRetrievedEvent += _ctiService_OnRetrievedEvent;

                _ctiService.OnTransferredEvent += _ctiService_OnTransferredEvent;
                _ctiService.OnConferencedEvent += _ctiService_OnConferencedEvent;

                _ctiService.OnConnectionClearedEvent += _ctiService_OnConnectionClearedEvent;
            }
            _initated = true;
        }

        internal void Stop()
        {
            if (_ctiService.StreamUp)
            {
                _ctiService.Shutdown = true;
                _ctiService.Close();
                _ctiService.StopService();
            }
        }

        #region CTI Events

        void _ctiService_OnConnectionClearedEvent(string subjectDevice, ConnectionClearedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.ConnectionClearedEvent(e);
        }

        void _ctiService_OnQueuedEvent(string subjectDevice, QueuedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.QueuedEvent(e);
        }

        void _ctiService_OnConferencedEvent(string subjectDevice, ConferencedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.ConferencedEvent(e);
        }

        void _ctiService_OnTransferredEvent(string subjectDevice, TransferredEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.TransferredEvent(e);
        }

        void _ctiService_OnRetrievedEvent(string subjectDevice, RetrievedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.RetrievedEvent(e);
        }

        void _ctiService_OnHeldEvent(string subjectDevice, HeldEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.HeldEvent(e);
        }

        void _ctiService_OnFailedEvent(string subjectDevice, FailedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.FailedEvent(e);
        }

        void _ctiService_OnNetworkReachedEvent(string subjectDevice, NetworkReachedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.NetworkReachedEvent(e);
        }

        void _ctiService_OnOriginatedEvent(string subjectDevice, OriginatedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.OriginatedEvent(e);
        }

        void _ctiService_OnDivertedEvent(string subjectDevice, DivertedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.DivertedEvent(e);
        }

        void _ctiService_OnEstablishedEvent(string subjectDevice, EstablishedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.EstablishedEvent(e);
        }

        void _ctiService_OnDeliveredEvent(string subjectDevice, DeliveredEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.DeliveredEvent(e);
        }


        void _ctiService_OnServiceInitiatedEvent(string subjectDevice, ServiceInitiatedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.ServiceInitiatedEvent(e);
        }

        void _ctiService_OnStartMonitorResponse(string subjectDevice, Tsapi.MonitorConfirmationEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.StartMonitorResponse(e);
        }

        void _ctiService_OnMonitorEndedEvent(string subjectDevice, MonitorEndedEventArgs e)
        {
            EntityItem client = _clients[subjectDevice];

            if (client != null)
                client.MonitorEndedEvent(e);
        }


        void _ctiService_OnLoggedOnEvent(string subjectDevice, Tsapi.LoggedOnEventArgs e)
        {
            Console.WriteLine("Agent logged in event ");
        }

        #endregion

        #region CTI Commands

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

        #endregion
    }
}
