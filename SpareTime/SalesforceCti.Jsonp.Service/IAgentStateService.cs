using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ComponentModel;
using System.ServiceModel.Web;

using Spare.Jsonp.Generic;

namespace Spare.Jsonp.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAgentStateService" in both code and config file together.
    [ServiceContract]
    public interface IAgentStateService
    {
        [OperationContract]
        [Description("Maintains heartbeat between client and server")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Spare.Jsonp.Generic.LoginResponse LoginAgent(string identifier, string agent, string password, string station, Generic.AgentWorkMode workmode);
        

        [OperationContract]
        [Description("Maintains heartbeat between client and server")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Spare.Jsonp.Generic.Response LogoutAgent(string identifier, string agent, string station, int reason = 0);


        [OperationContract]
        [Description("Maintains heartbeat between client and server")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Spare.Jsonp.Generic.Response SetAgentState(string identifier, string agent, string station, Generic.AgentWorkMode workmode, int reason);
    }
}
