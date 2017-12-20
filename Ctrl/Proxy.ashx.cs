using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
namespace ERP.Ctrl
{
    /// <summary>
    /// Proxy 的摘要说明
    /// </summary>
    public class Proxy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public void ddd()
        {
        }


        private static string DownLoadHtml(string url, int timeout = 10, bool enableProxy = false )
        {
            try
            {
                string html = "";
                var myRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
                myRequest.Method = "GET";
                myRequest.Timeout = 1000 * timeout;
                myRequest.AllowAutoRedirect = true;
                if (enableProxy)
                {
                    //如果启用WEBPROXY代理
                    var webProxy = new WebProxy("37.239.46.18", 80);
                    myRequest.Proxy = webProxy;
                }
                var myResponse = (HttpWebResponse)myRequest.GetResponse();
                using (var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding((myResponse.CharacterSet))))
                {
                    html = sr.ReadToEnd();
                    myResponse.Close();
                }
                return html;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}