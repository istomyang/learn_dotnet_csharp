using System;

// 直接写 WriteLine() 就可以，省略命名空间和类名，但是只支持静态方法和对象。
using static System.Console;

namespace NamespaceStart
{
    class ClassStart
    {
        static public void DoTask()
        {




            // 基本类型变量重新赋值
        }


        // 关键字
        static private void KeyWords()
        {
            // default 获取任何数据类型的默认值（C#2.0+）
            string default1 = default(string);
            Console.WriteLine($"String 的默认值是 null 吗？：{ default1 }");

            // new 指示运行时给数据类型分配内存，即实例化数据类型。
            // --1 如果分配不给数据，运行时就分配默认值：String（null），int（0），bool（false），char（\0）
            // --2 非基元数据以递归的方法为字段初始化默认值。
            // 【分配内存，就意味需要指定内存大小，对于数组来说，要么 int[2]，要么 {1,2}。】
            int[] array = new int[2];
        }
    }


    // 基本数据类型：预定义类型（predefined type） 或 基元类型（primitive type）
    class ClassType
    {
        static public void DoTask()
        {
            Console.WriteLine("========== String ==========");
            String();
        }

        static private void String()
        {
            // 字符串 String 或者 System.String https://docs.microsoft.com/zh-cn/dotnet/api/system.string?view=netcore-3.1



            // 字符串是 “不可变”（immutable） 类型：出于性能问题，不可能在源内存位置直接修改，而是再创建一个新的，旧的因为没有引用被垃圾回收。
            // 如果想使用原来那个：
            string myWords = "Halo";
            myWords = myWords.ToUpper();


            // 字符串连接
            Console.WriteLine("String1" + "String2");


            // $ - 字符串内插（C# 参考） C#6.0+
            // $"" 是语法糖，使用的是 string.Format() 进行转换，但是完整的功能支持，建议还是使用 string.Format()。
            string words = "这是变量的值";
            Console.WriteLine($"字符串内插：{ words }");

            // @ -- 逐字字符串标识符（C# 参考）
            Console.WriteLine(@"
            @ -- 逐字字符串标识符:
             这也是双引号内容：""两个双引号内部"" 
                    /\
                   /  \
                  /    \
                 /      \
                /________\
            ");

            // 使用关键字作为变量
            string @string = "halo";
            Console.WriteLine($"{@string}");

            // 混合
            Console.WriteLine($@"这是混合的效果");


            // 转义符号的使用
            Console.Write("与WriteLine区别在于：它没有回车；\"\n");

            // Console.Write：为了跨平台兼容，不建议使用 \n，而是 System.Environment.NewLine
            Console.Write(System.Environment.NewLine);

            // string 方法：

            // -- string.Format() 静态，格式化字符串
            string name = "Tom";

            // --- 格式设置类型：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/formatting-types?view=netcore-3.1
            // --- hh:https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-date-and-time-format-strings?view=netcore-3.1#hhSpecifier
            Console.WriteLine(string.Format("Hello,{0}! This is {1:hh} Clock.", name, DateTime.Now));

            // --- 左对齐和右对齐
            // --- 1、正数：右对齐，负数就反
            // --- 2、C1 啊都是固定数据类型的：比如 C3 意思就是 decimal，后面3位小数。
            Console.WriteLine(string.Format("Hello,{0:20}!", name));

            // -- string.Concat() 静态，连接多个字符串  
            // -- string.Compare() 静态，比较字符串大小
            // -- 。。。。。


            // String 属性（字段）：
            // String.Length，不可更改。



            // 使用 System.Text.StringBuilder https://docs.microsoft.com/zh-cn/dotnet/api/system.text.stringbuilder?view=netcore-3.1
            // 用来构建长字符串，唯一的区别就是，后者不返回新数据，直接在源字符串操作。
        }


    }


    // 语句：if
    class ClassStatement
    {
        static public void DoTask()
        {

        }

        // foreach(Foreach Loop)
        static private void ForEach()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // number 时只读的临时变量
            foreach (var number in numbers)
            {
                Console.WriteLine($"{number}");

                // For Loop ？ ForEach Loop
                // 1、foreach不用关心数组溢出问题
                // 2、foreach：不能更改源数组内容
                // 3、foreach：不能以相反的顺序处理数组
                // 4、foreach：不能访问一些数组内容
                // 5、foreach：不能使用多个数组（双循环）
            }
        }
    }


}