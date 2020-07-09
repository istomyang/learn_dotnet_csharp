using System;
using static System.Console;
using System.Runtime.ExceptionServices;

// 异常处理规范
// 1、捕捉能处理的异常，如果异常不能处理，交给最高级别（catch块）向用户抛出
// 2、不要隐藏危险的异常，不要视图捕捉所有异常，一旦捕捉必须妥善处理，否则如果跳过，会造成系统崩溃。（建议最高，保存所有资料，然后闪退程序，记得告诉用户）
// 3、有些异常不可恢复，这种情况应该直接关闭程序，CLR4之后，不可恢复异常会强制采取关闭程序操作，之前可以让你慢慢发挥，比如保存资料，然后。。。
// 4、较低的异常处理位置不应该记录或者向用户报告异常
// 5、使用 原始调用栈信息 重新抛出异常
// 6、避免在异常条件表达式发生异常，还有就是由于本地化导致的判断失效（ a === "本地语言")

namespace Exception1
{
    // 用异常实现基本错误处理（专业术语：异常处理）
    // 1、当用户输入错误的时候，编译器会报错，但是在生产版本时，需要告知用户，所以需要专门处理。
    // 2、避免使用异常来处理预料之中的错误。
    // 3、主题：
    // -- 1、简单的异常处理流程
    // -- 2、从代码中人为抛出异常
    // -- 3、使用异常的原则
    class Class1
    {
        public static void Run1()
        {

        }

        // 使用一个错误：如果输入的是 four，那是无法转换成 int 的
        private static void GetError()
        {
            WriteLine("输入一个数字：");
            string words = ReadLine();

            // 使用 try 语句包裹（专业术语：捕获异常）
            // 1、告诉编译器，这里面可能有异常
            // 2、一旦异常，编译器就找对应的 catch
            // 3、catch 是一个解决方案集合，所以有多个
            try
            {
                int number = int.Parse(words);
                WriteLine($"你的数字是：{number}");
            }

            // catch 语句指定异常的数据类型，异常类型和数据类型匹配就使用。
            // 如果没有找到匹配的 catch集，就跟没异常处理一样
            // 正确编写异常：
            // 1、描述异常为什么发生
            // 2、顺带提出解决方案（避免方案）
            catch (FormatException)
            {
                WriteLine("请输入数字！");
            }

            // 这是通用的异常 System.Exception（类似于基类，其他异常都是派生过来的）
            // 放在最后，所以catch排列原则：从具体到不具体。
            // 可以没有 catch 块
            // catch 里可以没有参数，那是默认从 Object.Exception 的异常（C#不允许，C++可以）
            // 如果只有 catch，在 CIL 中是 catch(object) 能捕捉所有异常，哪怕不是从 System.Exception 继承
            catch (Exception e)
            {
                WriteLine($"未知错误：{e.Message}");
            }

            // 无论是否异常，finally 都会执行
            // 1、提供一个最终位置，无论是否异常，都要执行。
            // 2、所以 finally 最适合做 资源清理。
            // 3、哪怕没有 catch 块也行，也就是 try/finally 语句
            finally
            {
                WriteLine("GoodBey!");
            }

            // 如果没有 catch 块：“运行时”会先报告异常，再处理 finally；因为
            // 1、运行时 在 执行 finally 之前已经知道没有 catch，它已经检查了调用栈上所有的 栈帧。
            // 2、一旦发现异常，第一步，检查机器是否安装调试器（是开发人员）然后将进程与调试器连接；
            // 3、如果是普通用户，并拒绝调试，则打印异常，并执行 finally 语句
            // 4、注意！运行时不一定要运行 finally，因为这是具体的实现细节，可能有差异。

            // a = System.Runtime.CompilerServices.RuntimeWrappedException
            // 1、C#2.0 开始，如果代码是非 C# 写的，并且不会抛出 System.Exception 异常
            // 2、那异常对象会被封装到 a 里面，这个 a 派生自 System.Exception。
            // 3、意味着：C# 程序集（C++编写）所有的异常都会表现的跟 System.Exception 一样。
        }

        // C# 允许从代码中 抛出异常
        private static int ThrowError()
        {
            int result = 0;

            try
            {
                // 后面的字符串，就是 exception.Message
                throw new FormatException("哈哈，抛出 System.FormatException 异常！");
                throw new Exception("哈哈，抛出 System.Exception 异常！");
            }
            catch (System.FormatException)
            {
                WriteLine("OK,我收到异常了！");

                result = -1;

                // 有时候本 catch 解决不了问题，那就重新抛出异常
                // 注意：
                // 1、必须是单独的 throw
                // 2、原因1：保留了原始调用栈的信息
                // 3、原因2：如果throw new Exception，那原始信息会被代替
                // 4、放在最后
                throw;
            }
            catch (AggregateException exception)
            {
                // 保留 原始调用栈信息
                // 需要 using System.Runtime.ExceptionServices;
                // 与 throw; 区别：如果函数返回值，而没有值从 Throw() 路径返回，则编译器报错
                // 开发人员会在 Throw() 后面跟随一个 return语句，即时不运行。
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            }
            catch (System.Exception)
            {
                WriteLine("OK,我收到异常了！");
                result = -1;
            }

            //  注意：
            // 1、不要返回 异常信息
            // 2、尽量使用抛出异常而非返回 状态码 报告错误
            return result;

        }

        // 避免使用异常来处理预料之中的错误。
        // 1、异常时专门跟踪例外，难以预料的错误
        // 2、抛出异常会造成性能损失：从 纳秒级 到 毫秒级
        // 3、为意料之中错误使用异常，则会增加维护成本
        private static void AvoidException()
        {

            WriteLine("输入一个数字：");
            string words = ReadLine();

            // 使用 int.TryParse() 进行数值转换
            // 1、Parse 的问题在于，类型转换失败时抛出 代价极高的异常。
            // 2、最好的办法是：尝试执行转换，而不异常处理
            // 3、目前所有的 类型 都支持 TryParse
            // 4、TryParse：返回 bool，使用 out 关键字
            if (int.TryParse(words, out int number))
            {
                WriteLine($"你的数字是：{number}");
            }

            // 有了 TryParse，就不需要使用 异常 兴师动众地处理错误了。
        }

    }
}


namespace AddisonWesley.Michaelis.EssentialCSharp.Chapter11.Listing11_01
{
    using System;

    public sealed class TextNumberParser
    {
        public static int Parse(string textDigit)
        {
            string[] digitTexts =
                { "zero", "one", "two", "three", "four",
                  "five", "six", "seven", "eight", "nine" };

#if !PRECSHARP7
            int result = Array.IndexOf(
                digitTexts,
                // Leveraging C# 2.0’s null coelesce operator
                (textDigit ??
                  // Leveraging C# 7.0’s throw expression
                  throw new ArgumentNullException(nameof(textDigit))
                ).ToLower());

#else
            // ArgumentNullException：错误地传递空值抛出，空值是无效参数特例
            // ArugmentException ArugmentOutOfRangeException：非空的无效参数
            // NullReferenceException：底层运行时使用对象发现是 Null 是抛出
            if(textDigit == null) throw new ArgumentNullException(nameof(textDigit))
            int result = Array.IndexOf(
                digitTexts, textDigit?.ToLower());
#endif

            if (result < 0)
            {
#if !PRECSHARP6
                // 异常支持用字符串参数标识参数名，C# 6.0后使用 nameof，当更改参数时，IDE会自动重命名，而如果不重命名，则编译时会报错
                throw new ArgumentException(
                    "The argument did not represent a digit", nameof(textDigit));
#else
                throw new ArgumentException(
                    "The argument did not represent a digit",
                    "textDigit"); // C# 6.0前，需要自己手动修改
#endif
            }

            return result;
        }
    }
}

namespace AddisonWesley.Michaelis.EssentialCSharp.Chapter11.Listing11_02
{
#pragma warning disable 0168 // Disabled warning for unused variables for elucidation
    using System;
    using System.ComponentModel;

    public sealed class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                throw new Win32Exception(42);
                //TextNumberParser.Parse("negative forty-two");
                // ...
                throw new InvalidOperationException(
                    "Arbitrary exception");
                // ...
            }
            catch (Win32Exception exception)
                // 不能使用 if 代替，若使用 if，则该 catch 块先成为异常处理程序，再做判断，而难度在于很难跳出下一个 catch 块。
                // 使用 when 就可以先判断，然后再处理。
                when (args.Length == exception.NativeErrorCode)
            {

            }
            catch (NullReferenceException exception)
            {
                // Handle NullReferenceException
            }
            catch (ArgumentException exception)
            {
                // Handle ArgumentException
            }
            catch (InvalidOperationException exception)
            {
                // Handle ApplicationException
            }
            catch (Exception exception)
            {
                // Handle Exception
            }
            finally
            {
                // Handle any cleanup code here as it runs
                // regardless of whether there is an exception
            }
        }
    }
}