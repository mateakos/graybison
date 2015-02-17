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

        private JsonService()
        {
            _ctiService = new Tsapi.TelephonyServiceManager("graybison");
            Initialize();
        }

        private void Initialize()
        {
            if (!_ctiService.Open("AVAYA#CM#CSTA#HUBIVSR83", "tman", "Tm@npassw0rd"))
            {
                _ctiService.Shutdown = true;
            }
            else { _ctiService.StartService();
            _ctiService.OnLoggedOnEvent += _ctiService_OnLoggedOnEvent;
            }
        }

        void _ctiService_OnLoggedOnEvent(string subjectDevice, Tsapi.LoggedOnEventArgs e)
        {
            Console.WriteLine("Agent logged in event ");
        }

        #region Heartbeat

        public Response Heartbeat(Guid id)
        {
            return new Response()
            {
                Result = true,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(true),
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

        public static JsonService GetInstance()
        {
            lock (typeof(JsonService)) {
                if (_service == null)
                    _service = new JsonService();
                return _service;
            }
        }
    }
}
