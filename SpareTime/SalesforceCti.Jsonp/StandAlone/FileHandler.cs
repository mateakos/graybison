using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Spare.Jsonp
{
    public class FileHandler : HttpServer.HttpModules.HttpModule
    {
        Assembly asm = Assembly.GetExecutingAssembly();

        Dictionary<string, string> _mapping = new Dictionary<string, string>();
        Dictionary<string, string> _rules = new Dictionary<string, string>();
        //readonly static private log4net.ILog log = log4net.LogManager.GetLogger("UAClient.Form1.cs");

        public Dictionary<string, string> Mapping
        {
            get { return _mapping; }
        }

        public Dictionary<string, string> Rules
        {
            get { return _rules; }
        }


        public FileHandler()
        {
            _mapping["html"] = "text/html";
            _mapping["htm"] = "text/html";
            _mapping["js"] = "application/javascript";
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public override bool Process(HttpServer.IHttpRequest request, HttpServer.IHttpResponse response, HttpServer.Sessions.IHttpSession session)
        {
            string[] resourceNames = asm.GetManifestResourceNames();
            string name = string.Join(".", request.UriParts);
            string ext = request.UriParts[request.UriParts.Length - 1];
            ext = ext.Substring(ext.LastIndexOf(".") + 1);
            foreach (string resname in resourceNames)
            {
                if (resname.EndsWith(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    using (Stream s = asm.GetManifestResourceStream(resname))
                    {
                        byte[] data = ReadFully(s);
                        if (_rules.Count > 0)
                        {
                            try
                            {
                                string d = Encoding.UTF8.GetString(data);
                                foreach (string key in _rules.Keys)
                                {
                                    d = d.Replace("$$" + key + "$$", _rules[key]);
                                }
                                data = Encoding.UTF8.GetBytes(d);
                            }
                            catch (Exception ex)
                            {
                                //log.Error("Error applying rule",ex);
                            }
                        }
                        response.AddHeader("Content-Type", _mapping[ext]);
                        response.AddHeader("Cache-Control", "no-cache");
                        response.AddHeader("Pragma", "no-cache");
                        response.AddHeader("Last-Modified", DateTime.UtcNow.ToString("R"));
                        response.AddHeader("Expires", DateTime.UtcNow.ToString("R"));
                        response.Body.Write(data, 0, data.Length);
                        response.Send();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
