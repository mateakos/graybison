using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spare.Jsonp.Generic
{
    public class Response
    {
        public Response() { }

        public bool Result { get; set; }

        public Guid Identifier { get; set; }

        public string Event { get; set; }

        public string Data { get; set; }

        public string Error { get; set; }
    }
}
