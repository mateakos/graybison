using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace SalesforceCti.Jsonp.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode =
        AspNetCompatibilityRequirementsMode.Allowed)]
    public class HeartbeatService : IHeartbeatService
    {
        private SalesforceCti.Jsonp.JsonService _service;

        public HeartbeatService()
        {
            _service = JsonService.GetInstance();
        }

        public SalesforceCti.Jsonp.Generic.Response Heartbeat(string identifier)
        {
            return _service.Heartbeat(Guid.Empty);
        }
    }

}