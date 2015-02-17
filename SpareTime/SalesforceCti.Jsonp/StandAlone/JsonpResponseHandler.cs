using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;

namespace Spare.Jsonp
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class JsonpResponseHandler
    {

        HttpServer.IHttpResponse PlaybackUrlResponse;
        string PlaybackUrlCallback;
        JsonpRequestHandler jsonphandler;
        //readonly static private log4net.ILog log = log4net.LogManager.GetLogger("UAClient.Form1.cs");

        public JsonpResponseHandler(JsonpRequestHandler j)
        {
            this.jsonphandler = j;
            j.responsehandler = this;
        }

        public void sendPlaybackUrlResponse(bool result, string data)
        {
            JObject j = new JObject();
            if (result)
            {
                j["result"] = true;
                j["url"] = data;
            }
            else
            {
                j["error"] = data;
                //log.Warn("Login error: " + result);
            }
            if (isWaitingPlaybackUrl(false))
            {
                jsonphandler.sendResponse(PlaybackUrlResponse, PlaybackUrlCallback, j);
                PlaybackUrlResponse = null;
                PlaybackUrlCallback = null;
            }
            else
            {
                //log.Warn("Unexpected login response: " + msg);
            }
        }

        public bool isWaitingPlaybackUrl(bool del)
        {
            if (del)
            {
                bool ret = PlaybackUrlResponse != null;
                PlaybackUrlResponse = null;
                PlaybackUrlCallback = null;
                return ret;
            }
            else
            {
                return PlaybackUrlResponse != null;
            }
        }

        public void setPlaybackUrl(HttpServer.IHttpResponse resp, string callback)
        {
            PlaybackUrlResponse = resp;
            PlaybackUrlCallback = callback;
        }
    }
}
