using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spare.Jsonp.Generic
{
    class MessageConstant
    {
        public const string UNREGISTERED = "Client identifier not registered";
        public const string FAILED_CTI_REQ = "Failed to send request to server";
        public const string SUCCESS_CTI_REQ = "Request sent to server";

        public const string EVT_MONITOR_START = "MONITOR_START";
        public const string EVT_MONITOR_STOP = "MONITOR_STOP";
        public const string EVT_SERVICE_INIT = "SERVICE_INIT";
        public const string EVT_ESTABLISHED = "ESTABLISHED";
        public const string EVT_DELIVERED = "DELIVERED";
        public const string EVT_DIVERTED = "DIVERETED";
        public const string EVT_ORIGINATED = "ORIGINATED";
        public const string EVT_NETWORK_REACHED = "NETWORK_REACHED";
        public const string EVT_CALL_FAILED = "CALL_FAILED";
        public const string EVT_HELD = "HELD";
        public const string EVT_RETRIEVED = "RETRIEVED";
        public const string EVT_TRANSFER = "TRANSFER";
        public const string EVT_CONFERENCE = "CONFERENCE";
        public const string EVT_QUEUED = "QUEUED";
        public const string EVT_CONN_CLEARED = "CONN_CLEARED";
    }
}
