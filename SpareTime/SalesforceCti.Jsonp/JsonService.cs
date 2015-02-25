using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spare.Jsonp.Generic;
using log4net;

namespace Spare.Jsonp
{
    public class JsonService
    {
        private static JsonService _service;
        private Spare.Tsapi.TelephonyServiceManager _ctiService;
        private bool _initated = false;
        private readonly ILog _log = LogManager.GetLogger(typeof(JsonService));
        private Entity.EntityCollection _clients;

        private JsonService()
        {
            _ctiService = new Tsapi.TelephonyServiceManager("graybison");
            _clients = new Entity.EntityCollection();
            Initialize();
        }

        public static JsonService GetInstance()
        {
            lock (typeof(JsonService))
            {
                if (_service == null)
                    _service = new JsonService();
                return _service;
            }
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
            else { _ctiService.StartService();
            _ctiService.OnLoggedOnEvent += _ctiService_OnLoggedOnEvent;
            _ctiService.OnStartMonitorResponse += _ctiService_OnStartMonitorResponse;
            }
            _initated = true;
        }

        void _ctiService_OnStartMonitorResponse(string subjectDevice, Tsapi.MonitorConfirmationEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _ctiService_OnLoggedOnEvent(string subjectDevice, Tsapi.LoggedOnEventArgs e)
        {
            Console.WriteLine("Agent logged in event ");
        }

        #region Heartbeat

        public Response Heartbeat(Guid id)
        {
            Entity.Entity client = _clients[id];
            if (client != null)
            {
                client.Heartbeat();
                client.PollEvent();
            }
            else
            {
                client = new Entity.Entity();
                _clients.Add(client);
            }

            Dictionary<string, object> array = new Dictionary<string, object>();
            array.Add("Id", client.Id);

            return new Response()
            {
                Result = true,
                Identifier = client.Id,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(array),
                Error = null
            };
        }

        #endregion

        #region AgentState

        public LoginResponse LoginAgent(string agent, string password, string station, AgentWorkMode workmode = AgentWorkMode.Auxiliary) {
            if (_ctiService.LoginAgent(station, agent, password, (int)workmode))
            {
                return new LoginResponse() { Result = true, Data = "Logged in" };
            }
            return new LoginResponse() { Error = "Failed" };
        }

        public Response LogoutAgent(string agent, string station, int reason = 0)
        {
            if (_ctiService.LogoutAgent(station, agent,  reason))
            {
                return new Response() { Result = true, Data = "Logged out" };
            }
            return new Response() { Error = "Failed" };
        }

        public Response SetAgentState(string agent, string station, AgentWorkMode workmode, int reason)
        {
            if (_ctiService.SetAgentState(station, agent, reason, (int)workmode))
            {
                return new Response() { Result = true, Data = "State changed" };
            }
            return new Response() { Error = "Failed" };
        }

        #endregion

        #region StationService

        Spare.Jsonp.Generic.Response CallObserve(Guid identifier, string extension)
        {
            Entity.Entity client = _clients[identifier];
            if (client == null)
            {
                return new Response()
                {
                    Result = true,
                    Data = "Client identifier not registered",
                    Error = null
                };
            }

            string data = "Failed to send request to server";
            if (_ctiService.Monitor(extension))
                data = "Monitor request sent";


            return new Response()
            {
                Result = true,
                Identifier = client.Id,
                Data = data,
                Error = null
            };
        }

        #endregion
    }
}
