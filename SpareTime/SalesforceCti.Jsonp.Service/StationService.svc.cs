using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace Spare.Jsonp.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode =
        AspNetCompatibilityRequirementsMode.Allowed)]
    public class StationService : IStationService
    {
        private Spare.Jsonp.JsonService _service;

        public StationService()
        {
            _service = JsonService.GetInstance();
        }

        public Spare.Jsonp.Generic.Response CallObserve(string identifier, string extension)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(identifier, out id);
            return _service.CallObserve(id, extension);
        }
    }
}
