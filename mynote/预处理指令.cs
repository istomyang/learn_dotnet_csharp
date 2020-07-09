// 预处理指令：告诉编译器有关代码的组织信息。
// C# 的预处理 与 C++ 不一样。
// https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/preprocessor-directives/

// 总结：
// #define 定义预处理器符号，定义了，就是 true
// #undef 取消定义，就是 false
// #if 可以与 #elif 和 #else 连用，但必须与 #endif 结尾，经常与预处理符号连用，选择编译某些语句（比如内置符号：平台差异、框架差异、发布与测试）
// #warning 显示警告，用于发现但没有解决的问题
// #error 显示错误，用于发现但没有解决的错误
// #pragma 用来忽略某些警告、错误
// #line 这个地方发生错误/警告，指定其他地方的锅，要与 #line default 连用，用来恢复。
// #region 必须和 #endregion 连用，表示一个可折叠的代码区域，可嵌套，好处就是关闭与当下无关的代码


// 定义预处理器符号1 —— #define
// 必须放在顶部
// 定义了，就是 为真。
#define CSHARP2PLUS

// 定义预处理器符号2 —— 编译器选项
// 定义了，就为真，所以：可以通过选项用同一份文件生成两套代码
// > dotnet.exe -define:CSHARP2PLUS index.cs

// 取消定义处理器符号 —— 改为false
// CSHARP2PLUS 包含的代码不编译
// 所以第二种选项法更不错。（如果时单文件的话）
#undef CSHARP2PLUS

using System;

namespace 预处理指令
{



    class Start
    {
        public static void Start1()
        {
            // 预处理指令必须：1、以 # 开头；2、一行写完；3、以换行符而不是分号结束
            // 1、用来处理框架兼容
#if CSHARP2PLUS
            System.Console.WriteLine("只有C#2.0+才能执行这段代码。");
            System.Console.WriteLine("这是专门为 C#2.0+ 写的代码。。。很多代码");
#endif


            // 2、用来处理平台差异
#if LINUX
            System.Console.WriteLine("给 Liunx 编译的");
            System.Console.WriteLine("这一块是可以有很多很多行代码。");

#elif WINDOWS
            System.Console.WriteLine("给 Windows 编译的");
#endif


            // 3、用来调试（发布时自动移除）
#if DEBUG
            System.Console.WriteLine("用来调试的代码");
#endif

#if RELEASE
            System.Console.WriteLine("调试不编译，而发布的时候编译。");
#endif

#warning 用来指示代码潜在的BUG和改进的地方。【开发者必备好帮手！是不是有黄色浪线？】

#error 用来指示代码潜在的BUG和改进的地方。【开发者必备好帮手！是不是有红色浪线？】


            //禁用警告
#pragma warning disable 1030
            System.Console.WriteLine("这里面的警告可以被安全的忽略。");
            System.Console.WriteLine("警告类似于：CS1030：XXXXXX");
            System.Console.WriteLine("禁用警告也可以使用 命令行选项：nowarn。【会影响整个一套文件s】");
            // 启用警告
#pragma warning restore 1030


            // 指定行号
#line 60 "some.cs"
#warning "如果这边出错，那就去另一个地方的错误。"
            // 恢复显示行号
#line default


            // 可视编辑器提示：划分一个代码区域
            // 唯一的用处就是可以 折叠 起来
            // 它必须成对出现，区域可以嵌套区域
            // 指令后面跟的是一个描述语句
            #region "描述：某个与当前思考无关的代码块"
            System.Console.WriteLine("这是代码区域");


            #endregion
        }
    }
}