using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Net;
using System.IO;
using System.Text;

namespace ERP.Ctrl
{
    public class ProxyTools
    {

        public static string EasyWeb(string _url = "http://www.baidu.com")
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( _url );    //创建一个请求示例
            HttpWebResponse response  = (HttpWebResponse)request.GetResponse();　　//获取响应，即发送请求
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string html = streamReader.ReadToEnd();
            return html;
            //Console.WriteLine(html); 
            //Console.ReadKey(); 
        }
        public static string DownLoadHtml(string url, int timeout = 10, bool enableProxy = false,
            string proxyUrl="localhost",int proxyPort=80 )
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
                    var webProxy = new WebProxy(proxyUrl,proxyPort);
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

        //test3
        static string Web3(string url = "http://localhost:2539/")
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            CookieContainer cc = new CookieContainer();
            request = (HttpWebRequest)WebRequest.Create( url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:19.0) Gecko/20100101 Firefox/19.0";

            string requestForm = "userName=1693372175&userPassword=123456";     //拼接Form表单里的信息
            byte[] postdatabyte = Encoding.UTF8.GetBytes(requestForm);
            request.ContentLength = postdatabyte.Length;
            request.AllowAutoRedirect = false;
            request.CookieContainer = cc;
            request.KeepAlive = true;

            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length); //设置请求主体的内容
            stream.Close();

            //接收响应
            response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine();

            Stream stream1 = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream1);
            string str = sr.ReadToEnd();
            return str;
            //Console.WriteLine(str); 
            //Console.ReadKey();
        }

        //test2
        public static string WebWithCookie(string[] args)
        {
            HttpHeader header = new HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;

            //在这里自己拼接一下Cookie，不用复制过来的那个GetCookie方法了，原来的那个写法还是比较严谨的
            CookieContainer cc = new CookieContainer();
            Cookie cUserName = new Cookie("cSpaceUserEmail", "742783833%40qq.com");
            cUserName.Domain = ".7soyo.com";
            Cookie cUserPassword = new Cookie("cSpaceUserPassWord", "4f270b36a4d3e5ee70b65b1778e8f793");
            cUserPassword.Domain = ".7soyo.com";
            cc.Add(cUserName);
            cc.Add(cUserPassword);

            string html = HTMLHelper.GetHtml("http://user.7soyo.com/CollectUser/List", cc, header);
            return html;
            //FileStream fs = new FileStream(@"D:\123.txt", FileMode.CreateNew, FileAccess.ReadWrite);
            //fs.Write(Encoding.UTF8.GetBytes(html), 0, html.Length);
            //fs.Flush();
            //fs.Dispose(); 
            //Console.WriteLine(html); 
            //Console.ReadKey();
        }
    }

    public class HTMLHelper
    {
        /// <summary>
        /// 获取CooKie
        /// </summary>
        /// <param name="loginUrl"></param>
        /// <param name="postdata"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static CookieContainer GetCooKie(string loginUrl, string postdata, HttpHeader header)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer cc = new CookieContainer();
                request = (HttpWebRequest)WebRequest.Create(loginUrl);
                request.Method = header.method;
                request.ContentType = header.contentType;
                byte[] postdatabyte = Encoding.UTF8.GetBytes(postdata);     //提交的请求主体的内容
                request.ContentLength = postdatabyte.Length;    //提交的请求主体的长度
                request.AllowAutoRedirect = false;
                request.CookieContainer = cc;
                request.KeepAlive = true;

                //提交请求
                Stream stream;
                stream = request.GetRequestStream();
                stream.Write(postdatabyte, 0, postdatabyte.Length);     //带上请求主体
                stream.Close();

                //接收响应
                response = (HttpWebResponse)request.GetResponse();      //正式发起请求
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

                CookieCollection cook = response.Cookies;
                //Cookie字符串格式
                string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);

                return cc;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取html
        /// </summary>
        /// <param name="getUrl"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string GetHtml(string getUrl, CookieContainer cookieContainer, HttpHeader header)
        {
            System.Threading.Thread.Sleep(1000); 
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(getUrl);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = header.contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = header.maxTry;
                httpWebRequest.Referer = getUrl;
                httpWebRequest.Accept = header.accept;
                httpWebRequest.UserAgent = header.userAgent;
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }
    }

    public class HttpHeader
    {
        public string contentType { get; set; }

        public string accept { get; set; }

        public string userAgent { get; set; }

        public string method { get; set; }

        public int maxTry { get; set; }
    }
}