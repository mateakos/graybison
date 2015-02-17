using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace SalesforceCti.Jsonp
{
    public class WebServiceManager
    {
        HttpServer.HttpServer l;
        JsonpRequestHandler jsonpReq;
        FileHandler fileHndl;
        JsonpResponseHandler jsonpResp;
        private readonly ILog log = LogManager.GetLogger(typeof(WebServiceManager));

        private string _listenIp;
        private int _listenPort;

        public WebServiceManager(string ip, int port)
        {
            this.ListeningIp = ip;
            this.ListeningPort = port;

            l = new HttpServer.HttpServer();
            jsonpReq = new JsonpRequestHandler();
            fileHndl = new FileHandler();

            l.ExceptionThrown += new HttpServer.ExceptionHandler(WebServerExceptionThrown);

            l.ServerName = "GeoCRMConnectWS";
            l.Add(jsonpReq);

            fileHndl.Mapping["htm"] = "text/html";
            fileHndl.Rules["PORT"] = port.ToString();

            l.Add(fileHndl);

            jsonpResp = new JsonpResponseHandler(jsonpReq);
        }

        void WebServerExceptionThrown(object source, Exception exception)
        {
            log.Error("Web service caught an unhandled exception", exception);
        }

        public void Start()
        {
            log.Info("Starting JSONP web service listener");
            l.Start(System.Net.IPAddress.Parse(this.ListeningIp), this.ListeningPort);
        }

        public void Stop()
        {
            log.Warn("Stopping JSONP web service listener");
            l.Stop();
        }

        public string ListeningIp
        {
            get { return _listenIp; }
            set
            {
                if (String.IsNullOrEmpty(value) ||
                    value.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
                {
                    _listenIp = "127.0.0.1";
                }
                else
                    _listenIp = value;
            }
        }

        public int ListeningPort
        {
            get
            {
                return _listenPort;
            }
            set
            {
                _listenPort = value;
            }
        }
    }
}
