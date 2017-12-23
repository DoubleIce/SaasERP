using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.InteropServices;//[DllImport]

public class _ProxyTool { }



public delegate bool CallBack(int hwnd, int lParam); //定义委托函数类型 
public class EnumReportApp
{
    [DllImport("user32")]
    public static extern int EnumWindows(CallBack x, int y);
    public static void Main()
    {
        CallBack myCallBack = new CallBack(EnumReportApp.Report);
        EnumWindows(myCallBack, 0);
    }
    public static bool Report(int hwnd, int lParam)
    {
        Console.Write("Window handle is ");
        Console.WriteLine(hwnd); return true;
    }
} 

public class IEProxy
{
    private const int INTERNET_OPTION_PROXY = 38;
    private const int INTERNET_OPEN_TYPE_PROXY = 3;
    private const int INTERNET_OPEN_TYPE_DIRECT = 1;

    private string ProxyStr;


    [DllImport("wininet.dll", SetLastError = true)]

    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

    public struct Struct_INTERNET_PROXY_INFO
    {
        public int dwAccessType;
        public IntPtr proxy;
        public IntPtr proxyBypass;
    }

    public bool InternetSetOption(string strProxy)
    {
        int bufferLength;
        IntPtr intptrStruct;
        Struct_INTERNET_PROXY_INFO struct_IPI;

        if (string.IsNullOrEmpty(strProxy) || strProxy.Trim().Length == 0)
        {
            strProxy = string.Empty;
            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
        }
        else
        {
            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
        }
        struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
        struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");
        bufferLength = Marshal.SizeOf(struct_IPI);
        intptrStruct = Marshal.AllocCoTaskMem(bufferLength);
        Marshal.StructureToPtr(struct_IPI, intptrStruct, true);
        return InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, bufferLength);
    }
    public IEProxy(string strProxy)
    {
        this.ProxyStr = strProxy;
    }
    //设置代理  
    public bool RefreshIESettings()
    {
        return InternetSetOption(this.ProxyStr);
    }
    //取消代理  
    public bool DisableIEProxy()
    {
        return InternetSetOption(string.Empty);
    }
}
 



#region ...

        //public Boolean setip(string ip)  
        //{  
        //        RefreshIESettings(ip);  
        //        IEProxy ie = new IEProxy(ip);  
        //        return ie.RefreshIESettings();  
        //}  
        //public struct Struct_INTERNET_PROXY_INFO  
        //{  
        //    public int dwAccessType;  
        //    public IntPtr proxy;  
        //    public IntPtr proxyBypass;  
        //}  
        //private void RefreshIESettings(string strProxy)  
        //{  
        //    const int INTERNET_OPTION_PROXY = 38;  
        //    const int INTERNET_OPEN_TYPE_PROXY = 3;  
        //    const int INTERNET_OPEN_TYPE_DIRECT = 1;  

        //    Struct_INTERNET_PROXY_INFO struct_IPI;  
        //    // Filling in structure  
        //    struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;  
        //    struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);  
        //    struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");  

        //    // Allocating memory  
        //    IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI));  
        //    if (string.IsNullOrEmpty(strProxy) || strProxy.Trim().Length == 0)  
        //    {  
        //        strProxy = string.Empty;  
        //        struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;  

        //    }  
        //    // Converting structure to IntPtr  
        //    Marshal.StructureToPtr(struct_IPI, intptrStruct, true);  

        //    bool iReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI));  
        //}  

        //[DllImport("wininet.dll", SetLastError = true)]  
        //private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);  
        
#endregion 