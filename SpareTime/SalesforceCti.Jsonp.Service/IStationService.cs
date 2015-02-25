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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStationService" in both code and config file together.
    [ServiceContract]
    public interface IStationService
    {
        [OperationContract]
        [Description("Maintains heartbeat between client and server")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Spare.Jsonp.Generic.Response CallObserve(string identifier, string extension);
    }
}
