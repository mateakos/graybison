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
            }
            _initated = true;
        }

        #region Heartbeat

        public Response Heartbeat(Guid id)
        {
            Entity.Entity client = _clients[id];

            if (client != null)
            {
                client.Heartbeat();
                return client.PollEvent();
            }
            else
            {
                client = new Entity.Entity(_ctiService);
                _clients.Add(client);
            }

            return new Response()
            {
                Result = true,
                Identifier = client.Id,
                Error = null
            };
        }

        #endregion

        #region AgentState

        public LoginResponse LoginAgent(Guid id, string agent, string password, string station, AgentWorkMode workmode = AgentWorkMode.Auxiliary) {
            Entity.Entity client = _clients[id];
            if ((id == Guid.Empty) || (client == null)) { return new LoginResponse() { Error = MessageConstant.UNREGISTERED }; };

            if (client.LoginAgent(station, agent, password, (int)workmode))
            {
                return new LoginResponse() { Result = true, Data = MessageConstant.SUCCESS_CTI_REQ };
            }
            return new LoginResponse() { Error = MessageConstant.FAILED_CTI_REQ };
        }

        public Response LogoutAgent(Guid id, string agent, string station, int reason = 0)
        { 
            Entity.Entity client = _clients[id];
            if ((id == Guid.Empty) || (client == null)) { return new Response() { Error = MessageConstant.UNREGISTERED }; };

            if (client.LogoutAgent(station, agent,  reason))
            {
                return new Response() { Result = true, Data = MessageConstant.SUCCESS_CTI_REQ };
            }
            return new Response() { Error = MessageConstant.FAILED_CTI_REQ };
        }

        public Response SetAgentState(Guid id, string agent, string station, AgentWorkMode workmode, int reason)
        {
            Entity.Entity client = _clients[id];
            if ((id == Guid.Empty) || (client == null)) { return new Response() { Error = MessageConstant.UNREGISTERED }; };

            if (client.SetAgentState(station, agent, reason, (int)workmode))
            {
                return new Response() { Result = true, Data = MessageConstant.SUCCESS_CTI_REQ };
            }
            return new Response() { Error = MessageConstant.FAILED_CTI_REQ };
        }

        #endregion

        #region StationService

        public Spare.Jsonp.Generic.Response CallObserve(Guid identifier, string extension)
        {
            Entity.Entity client = _clients[identifier];
            if (client == null)
            {
                return new Response()
                {
                    Result = true,
                    Data = MessageConstant.UNREGISTERED,
                    Error = null
                };
            }

            string data = MessageConstant.FAILED_CTI_REQ;
            if (client.Monitor(extension))
                data = MessageConstant.SUCCESS_CTI_REQ;


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
