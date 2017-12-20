using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
namespace ERP.Ctrl
{
    public class Person
    {
        public string str1 = "111", str2;
        private string pstr1 = "priv";
        public static string staticstr = "staticString";
        public string PStr { get; set; }
        public static int StaticInt { get; set; }
        public static int count = 0;
        public void voidFun() { count += 1; }
        public int GetInt(int x) { return x + count; }
    }
    public class Person2:Person
    {
        public int Person2(int x) { return 10000 + x + count; }
    }
    public class ReflectTools
    {
        public static void GetType_typeof(string dllexeFilePath)
        {
            Type t1 = Type.GetType("ERP.Ctrl.Person2");   //从字符串中获得Type对象
            Console.WriteLine(t1.ToString()); 
            Type t2 = typeof(ERP.Ctrl.Person);           //从具体类中获得Type对象
            Console.WriteLine(t2.ToString()); 
            Person p = new Person();
            Type t3 = p.GetType();              //实例，从实例中获得Type对象 
            Assembly ass = Assembly.LoadFrom(dllexeFilePath);
            Console.WriteLine(ass.GetType("ERP.Ctrl.Person").ToString());    //从字符串中获得Type对象
            Module mod = ass.GetModules()[0];
            Console.WriteLine(mod.GetType("ERP.Ctrl.Person").ToString());    //从字符串中获得Type对象 
            Console.ReadKey();
        }

        public static void AssemblyLoad()
        {
            Assembly assm = Assembly.Load("fanshe");
            Console.WriteLine(assm.FullName);   //输出 fanshe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

            //注释上面两行，移除程序集的引用 
            Assembly assm1 = Assembly.LoadFrom(@"D:\fanshe.dll");
            Console.WriteLine(assm1.FullName);      ////输出 fanshe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

            //与Assembly.LoadFrom基本一样，只是如果被加载的dll，还依赖其他的dll的话，被依赖的对象不会加载
            Assembly assm2 = Assembly.LoadFile(@"D:\fanshe.dll");
            Console.WriteLine(assm2.FullName);

            Console.ReadKey();
        }
    }
}
/*
类型	    作用

Assembly	通过此类可以加载操纵一个程序集，并获取程序集内部信息
EventInfo	该类保存给定的事件信息
FieldInfo	该类保存给定的字段信息
MethodInfo	该类保存给定的方法信息
MemberInfo	该类是一个基类，它定义了EventInfo、FieldInfo、MethodInfo、PropertyInfo的多个公用行为

Module	该类可以使你能访问多个程序集中的给定模块
ParameterInfo	该类保存给定的参数信息
PropertyInfo	该类保存给定的属性信息
*/