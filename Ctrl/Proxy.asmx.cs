using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ERP.Ctrl
{
    /// <summary>
    /// Proxy1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Proxy1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public string GetUrl(string url)
        {
            return ProxyTools.DownLoadHtml(url, 10, false);
        }

        [WebMethod]
        public string GetProxyUrl(string url)
        {
            return ProxyTools.DownLoadHtml(url, 10, true);
        } 
    }
}
