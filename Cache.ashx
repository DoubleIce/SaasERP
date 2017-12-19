<%@ WebHandler Language="C#" Class="CacheHandler" %>


using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
//using Microsoft.Ajax.Utilities; 
using System.Collections;


public class CacheHandler : IHttpHandler {
    
    public static System.Security.Cryptography.MD5 md5;
    public static string MD5Compute(string str)
    {
        byte[] result = Encoding.Default.GetBytes(str);
        if (md5 == null)
            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        return BitConverter.ToString(output).Replace("-", "");
    }

    public static Hashtable ht = new Hashtable();
    public static bool storeFile = false;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/javascript";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        if (String.IsNullOrEmpty(request.QueryString["href"]))
        {
            response.Write("No Content");
        }
        else
        {
            string href = context.Request.QueryString["href"].Trim();

            string[] files = href.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);

            
            if (storeFile)
            {
                string _folder = MD5Compute(href);
                string folder = request.MapPath("/Log/" + _folder + "/");  
                if (!ht.Contains(_folder))
                {
                    ht[_folder] = 1;
                    //File.AppendAllText(folder + "_list.ini", "\n\n\n\r\t" + _folder, Encoding.UTF8);
                    //File.AppendAllText(folder + "_list.ini", href, Encoding.UTF8);
                }
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    int i = 0;
                    foreach (string fileName in files)
                    {
                        string filePath = context.Server.MapPath(fileName);
                        FileInfo finfo = new FileInfo(filePath);
                        if (!finfo.Exists)
                            File.WriteAllText(folder + i + "_" + finfo.Name, File.ReadAllText(filePath, Encoding.UTF8)); 
                        i++;
                    }
                }
            }
            
            CacheItem item = null;
            object obj = HttpRuntime.Cache.Get(href);//服务端缓存
            if (null == obj)
            {
                StringBuilder allText = new StringBuilder();
                foreach (string fileName in files)
                {
                    string filePath = context.Server.MapPath(fileName);
                    if (File.Exists(filePath))
                    {
                        allText.AppendLine(File.ReadAllText(filePath, Encoding.UTF8) + ";");
                    }
                    else
                    {
                        //response.Write("\r\n未找到源文件"+filePath+"\r\n");
                        //allText.Append("\r\n未找到源文件" + filePath + "\r\n");
                    }
                }//end foreach
                item = new CacheItem();
                //Minifier mf = new Minifier();
                item.Content = allText.ToString();// mf.MinifyJavaScript(allText.ToString());  
                item.Expires = DateTime.Now.AddHours(1); 
                HttpRuntime.Cache.Insert(href, item, null, item.Expires, TimeSpan.Zero);  
            }
            else
            {
                item = obj as CacheItem;
            }
            if (!String.IsNullOrEmpty(request.Headers["If-Modified-Since"]) && 
                TimeSpan.FromTicks(item.Expires.Ticks - DateTime.Parse(request.Headers["If-Modified-Since"].Split(';')[0]).Ticks).Seconds < 100)
            {
                response.StatusCode = 304;
                //response.Headers.Add("Content-Encoding", "gzip");
                response.StatusDescription = "Not Modified";
            }
            else
            {
                response.Write(item.Content);
                //SetClientCaching(response, DateTime.Now);
            }
        }//end else href
    }
    private void SetClientCaching(HttpResponse response, DateTime lastModified)
    {
        response.Cache.SetETag(lastModified.Ticks.ToString());
        response.Cache.SetLastModified(lastModified);
        //public 以指定响应能由客户端和共享（代理）缓存进行缓存。  
        response.Cache.SetCacheability(HttpCacheability.Public);
        //是允许文档在被视为陈旧之前存在的最长绝对时间。  
        response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
        //将缓存过期从绝对时间设置为可调时间  
        response.Cache.SetSlidingExpiration(true);
    }

    class CacheItem
    {
        private String _content = String.Empty;
        private DateTime _expires = DateTime.Now;
        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }
        public DateTime Expires
        {
            get
            {
                return this._expires;
            }
            set
            {
                this._expires = value;
            }
        }
    }

    #region IHttpHandler 成员

    public bool IsReusable
    {
        get { throw new NotImplementedException(); }
    }

    #endregion
}