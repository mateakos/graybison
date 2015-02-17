using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Spare.Jsonp.Service
{
    [ServiceContract]
    public interface IHeartbeatService
    {
        [OperationContract]
        [Description("Maintains heartbeat between client and server")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Spare.Jsonp.Generic.Response Heartbeat(string identifier);
    }
}
