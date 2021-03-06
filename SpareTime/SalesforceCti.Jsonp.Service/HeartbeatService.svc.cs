﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace Spare.Jsonp.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode =
        AspNetCompatibilityRequirementsMode.Allowed)]
    public class HeartbeatService : IHeartbeatService
    {
        private Spare.Jsonp.JsonService _service;

        public HeartbeatService()
        {
            _service = JsonService.GetInstance();
        }

        public Spare.Jsonp.Generic.Response Heartbeat(string identifier)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(identifier, out id);
            return _service.Heartbeat(id);
        }
    }

}