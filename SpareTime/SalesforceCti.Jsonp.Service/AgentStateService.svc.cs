using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace SalesforceCti.Jsonp.Service
{
   [AspNetCompatibilityRequirements(RequirementsMode =
       AspNetCompatibilityRequirementsMode.Allowed)]
    public class AgentStateService : IAgentStateService
    {
       private JsonService _service;

       public AgentStateService()
       {
           _service = JsonService.GetInstance();
       }

        public Generic.LoginResponse LoginAgent(string agent, string password, string station, Generic.AgentWorkMode workmode)
        {
            return _service.LoginAgent(agent, password, station, (Generic.AgentWorkMode)workmode);
        }

        public Generic.Response LogoutAgent(string agent, string station, int reason = 0)
        {
            return _service.LogoutAgent(agent, station, reason);
        }

        public Generic.Response SetAgentState(string agent, string station, Generic.AgentWorkMode workmode, int reason)
        {
            return _service.SetAgentState(agent, station, workmode, reason);
        }
    }
}
