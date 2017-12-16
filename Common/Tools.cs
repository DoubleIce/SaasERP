using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
namespace ERP.Common
{
    public class Tools
    {
        public void test(string str)
        {
            //str.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
        }

        #region 公用对象，第一次调用时才实例化，之后不再多次实例化

        //是否开启快速模式
        private static bool _UseFastMode = true;
        
        //是否开启快速模式
        public static bool UseFastMode
        {
            get { return _UseFastMode; }
            set { _UseFastMode = value; }
        }

        private static Random _random  ;
        public static Random random
        {
            get
            {
                if (_random == null)
                    _random = new Random();
                return _random;
            }
        }

        private static System.Security.Cryptography.MD5 _md5;
        public static System.Security.Cryptography.MD5 md5
        {
            get
            {
                if (_md5 == null)
                    _md5 = new MD5CryptoServiceProvider();
                return _md5;
            } 
        }

        #endregion

        /// <summary>
        /// 计算字符串MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Compute(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            //System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", ""); 
        }

        /// <summary>
        /// 计算文件（全路径）的MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5ComputeFile(string filename)
        {
            if (!File.Exists(filename))
                return String.Empty;

            System.IO.Stream stream = new FileStream(filename, FileMode.Open);
            //System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(stream);
            return BitConverter.ToString(output).Replace("-", "");
        }


    }
}