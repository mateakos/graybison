using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace Spare.Jsonp.Service
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

       public Spare.Jsonp.Generic.LoginResponse LoginAgent(string identifier, string agent, string password, string station, Generic.AgentWorkMode workmode)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(identifier, out id);
            return _service.LoginAgent(id, agent, password, station, (Generic.AgentWorkMode)workmode);
        }

       public Spare.Jsonp.Generic.Response LogoutAgent(string identifier, string agent, string station, int reason = 0)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(identifier, out id);
            return _service.LogoutAgent(id, agent, station, reason);
        }

       public Spare.Jsonp.Generic.Response SetAgentState(string identifier, string agent, string station, Generic.AgentWorkMode workmode, int reason)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(identifier, out id);
            return _service.SetAgentState(id, agent, station, workmode, reason);
        }
    }
}
