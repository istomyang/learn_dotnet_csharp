using System;

// using 的好处时可以省略命名空间
// using 不支持任何嵌套的命名空间，必须显式声明
// using 可以在内部引入
using System.Collections;

// using static 的好处不仅省命名空间，还可以省略类型名称，直接写静态成员。
// using 还有个用处？：可以省略，通过预处理器，同时使用两个命名空间里的同一个方法与成员。
using static System.Console;

// 如果静态成员有冲突，会造成编译错误，所以权衡好：有个好办法，使用别名。
using t = System.Timers.Timer;


// using NamespaceStart;
// using NamespaceFunction;
// using NamespaceClass;
// using 异步方法;
// using 简单的接口;
using 深入接口;

namespace 笔记
{
    class Program
    {
        // Main()  方法作为程序的入口，如果有多个入口，使用 csc.exe /m 指定包含入口的类。
        // string[] args 是在程序运行时，命名行参数传递给它的值，比如：halo.exe <YourName> <SayWords>，两个参数将会传递进去
        // void，Main() 方法可以有返回值，比如 int，用来传递程序状态码，按约定，非 0 返回值代表出错。
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Console.WriteLine("================ 001 Start.cs 命名空间 ==================");
            // NamespaceStart.ClassStart.DoTask();

            // Console.WriteLine("================ 005 Fucntion.cs ==================");
            // NamespaceFunction.ClassFunctionVoid.RunFunction();

            // Console.WriteLine("================ 007 数组和List.cs ==================");
            // NamespaceArrayAndList.ClassArrayUsage.Start();

            // Console.WriteLine("================ 类 ==================");
            // NamespaceClass.ClassStart.Start();

            // Console.WriteLine("================ 异步 ==================");
            // 异步方法.Start.Launch();

            WriteLine("================ 接口 ==================");
            // 简单的接口.RunClass.Run();
            // 深入接口.RunClass.Run();


            // 防止CLI窗口关闭
            Console.WriteLine("Press Enter to quit!");
            Console.ReadLine();
            return;
        }

        private static void Do()
        {
            // 在非 Main 中获得命令行参数
            string[] args = System.Environment.GetCommandLineArgs();
        }
    }
}
