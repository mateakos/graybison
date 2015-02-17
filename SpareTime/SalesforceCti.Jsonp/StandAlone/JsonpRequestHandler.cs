using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Newtonsoft.Json.Linq;

namespace SalesforceCti.Jsonp
{
    public class JsonpRequestHandler : HttpServer.HttpModules.HttpModule
    {

        public JsonpResponseHandler responsehandler;
        public delegate bool WaitHandler(bool b);
        readonly static private log4net.ILog log = log4net.LogManager.GetLogger(typeof(JsonpRequestHandler));

        public bool ProcessJSONP(HttpServer.IHttpRequest request, HttpServer.IHttpResponse response, HttpServer.Sessions.IHttpSession session)
        {
            string sessionid = session.Id;
            string jsoncallback = request.QueryString["jsoncallback"].Value;
            string rt = request.QueryString["rt"].Value;
            string guid = request.QueryString["guid"].Value;

            WaitHandler ret = ProcessRequest(rt, jsoncallback, guid, response, request);
            response.Connection = HttpServer.ConnectionType.KeepAlive;
            response.KeepAlive = 10;
            int i = 0;
            while (ret(false) && i < 50)
            {
                Thread.Sleep(200);
                i++;
            }

            if (ret(true))
            {
                JObject j = new JObject();
                j["error"] = "no_response";
                log.Error("No response from Call recorder server for request: " + rt);
                sendResponse(response, jsoncallback, j);
            }
            //
            return true;
        }

        public override bool Process(HttpServer.IHttpRequest request, HttpServer.IHttpResponse response, HttpServer.Sessions.IHttpSession session)
        {
            try
            {
                if (request.UriParts.Length > 0)
                {
                    string proc = request.UriParts[request.UriParts.Length - 1];
                    if (proc.Equals("proc.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ProcessJSONP(request, response, session);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to proccess request!", ex);
                return false;
            }
        }

        private WaitHandler ProcessRequest(string rt, string callback, string guid, HttpServer.IHttpResponse response, HttpServer.IHttpRequest req)
        {
            try
            {
                switch (rt)
                {
                    case "ping":
                        break;
                    case "monitor":
                        break;
                    case "stopmonitor":
                        break;
                    case "setagentstate":
                        break;
                }


                if (rt == "PlaybackUrl")
                {
                    if (responsehandler.isWaitingPlaybackUrl(false))
                    {
                        JObject j = new JObject();
                        j["error"] = "pending";
                        sendResponse(response, callback, j);
                        return isFalse;
                    }
                    else
                    {
                        responsehandler.setPlaybackUrl(response, callback);
                        string url = null;
                        try
                        {
                            //url = Geomant.Cti.Phone.GeoCTIServerProxy.GetInstance().GetPlaybackUrl();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed to retrieve playback url while processing request", ex);
                        }
                        if (!String.IsNullOrEmpty(url))
                        {
                            responsehandler.sendPlaybackUrlResponse(true, url);
                        }
                        else
                        {
                            responsehandler.sendPlaybackUrlResponse(false, "No recording URL was retrieved from server");
                        }
                        //ClientHandler.getInstance().LoginAgent(agentid, pass, station, mode);
                        return responsehandler.isWaitingPlaybackUrl;
                    }
                }
                else
                {
                    log.Error("Throwing exception, invalid request type");
                    throw new Exception("Invalid Request Type");
                }
            }
            catch (Exception ex)
            {

                JObject ret = new JObject();
                ret["error"] = ex.Message;
                sendResponse(response, callback, ret);
                log.Error("Error in ProcessRequest!", ex);
                return isFalse;
            }
        }

        public void sendResponse(HttpServer.IHttpResponse response, string jsoncallback, JObject j)
        {
            try
            {

                string msg = string.Format("{0}({1});", jsoncallback, j.ToString());
                byte[] buf = Encoding.UTF8.GetBytes(msg);
                response.ContentType = "application/json";
                response.AddHeader("Cache-Control", "no-cache");
                response.AddHeader("Pragma", "no-cache");
                response.AddHeader("Last-Modified", DateTime.UtcNow.ToString("R"));
                response.AddHeader("Expires", DateTime.UtcNow.ToString("R"));
                response.Body.Write(buf, 0, buf.Length);
                response.Send();
            }
            catch (Exception ex)
            {
                log.Error("Exception occured while sending response", ex);
            }
        }

        public bool isFalse(bool b)
        {
            return false;
        }
    }
}
