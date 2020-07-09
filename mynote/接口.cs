using System;
using static System.Console;

// 接口
// 1、可以用接口实现多态性。
// 2、接口和抽象类相似，但接口不包含实现。
// 3、调用者可以认为接口已经实现。
// 4、接口是 实现和使用 之间订立的契约 —— 必须按照接口的方法签明实现。

// 接口 仿佛是家里用的电的插座，实现包括 核能、火力、水力，而我们不关心具体实现方法，只要提供相应的接口就行：220v 60Hz
// 接口 体现出：实现 与 使用 分离，所以实现部分，体现了多态性。

namespace 简单的接口
{
    // 约定俗称：I + PascalCase
    // 接口是一个类，不包含任何 数据（字段）和实现。
    interface IFileCompression
    {
        // 函数以分号结尾。
        // C# 不允许接口成员使用 访问修饰符，都是自动公共。
        void Compress(string targetFileName, string[] fileList);

        // 节约代码量（因为你必须都实现）
        // void Uncompress(string compressFileName, string targetDirectory);

        // 可以包含 属性
        string Words { get; }
    }

    // 简单的实现接口
    class Class1 : IFileCompression
    {
        // 实现接口 Numbers
        public string Words { get { return "第一个，OK，我实现了！"; } }

        // 实现接口 Compress
        public void Compress(string targetFileName, string[] fileList)
        {
            WriteLine("第一个，好，我实现了！");
        }
    }

    // 接口类 能被任意类继承实现。
    class Class2 : IFileCompression
    {
        public string Words { get { return "第二个，OK，我实现了！"; } }
        public void Compress(string targetFileName, string[] fileList)
        {
            WriteLine("第二个，好，我实现了！");
        }
    }

    // 接口类 的使用
    class Class3
    {
        // 接口的使用：
        // 1、想想看，接口作为一个有一套方法的类，还是基类，是不是就可以放入任务实现接口的类？
        public static void F1(string words, IFileCompression item)
        {
            words = "把接口作为参数传递给函数!";
            words = "记住，接口本质是类，item 可以使用类的特性，唯一区别就是没有 value";
            WriteLine(item.Words);
        }
    }

    // 调用接口
    class RunClass
    {
        public static void Run()
        {
            // one 虽然是 Class1类，但也是 IFileCompression类
            Class1 one = new Class1();
            Class2 two = new Class2();

            // 使用不同的实现，但是：同一个方法！
            Class3.F1("类比：可能是...火力发电", one);
            Class3.F1("类比：可能是...水利发电", two);
        }
    }
}

namespace 深入接口
{
    interface I1 { string Word1 { get; } }
    interface I2 { string Word2 { get; } }
    interface I3 { string Word1 { get; } }
    interface I4 { void F1(); }
    class Base { public static void SayBase() { Console.WriteLine("你好！这是基类。"); } }

    // 继承的接口（关联的契约 ）
    interface I5 : I2 { string Word5 { get; } }
    //  多接口继承
    // 1、同时实现两个接口继承就可以不写成员
    // 2、代表一种契约关系
    // 3、用隐式实现可以不考虑这个复杂问题。
    interface I6 : I5, I1 { }

    // 抽象类继承接口
    abstract class Abstract : I1, I4
    {
        // 抽象类引用接口可以将接口映射为自己的抽象成员
        // 非抽象实现可以在方法主题中抛出 NotImplementedException：在 Class1 里
        // 注意：
        // 1、抽象类继承，接口内的成员命名不能重复
        public abstract string Word1 { get; }
        public abstract void F1();
    }


    // 接口实现
    // 基类和抽象类在前，只能有一个，接口在后，顺序随意。
    // 1、接口永远不能被实例化（new）
    // 2、而实现接口的是类，所以可以： I1 one = new Class1();
    // 3、接口不能包含静态成员，想想看，接口的价值取决于继承它的类的实现。
    // 4、接口和抽象类差不多，都是强迫实现，但是不需要在 接口添加 abstract
    // 5、实现接口两种方式：显示、隐式。
    class Class1 : Base, I1, I2, I3, I4
    {
        public static void SayClass1() { Console.WriteLine("你好！这是Class1类。"); }

        // 隐式成员实现
        // 要求：
        // 1、成员是公共的
        // 2、签明 与 接口成员签明 相同
        // 3、不需要 overide 或其他与接口关联的指示符，virtual是可选的，取决于类是否被其他派生重写
        // 所以可以直接调用，不需要类型转换
        public string Word1 { get { return "1号接口"; } }

        // 这是显示实现接口:
        // 1、使用接口前缀
        // 2、显示接口和接口直接关联，所以不能使用修饰符
        // 3、通过接口前缀，好像可以不同接口，同一成员签明，但感觉有问题。
        // 4、研究：没有问题，但最好避免，因为调用很烦，你需要 I3 one = new Class1();
        string I2.Word2 { get { return "2号接口"; } }
        string I3.Word1 { get { return "3号接口"; } }

        // 非抽象实现可以在方法中抛出 NotImplementedException
        // 目的就是：这一块我还没实现。
        public void F1()
        {
            throw new NotImplementedException();
        }

        // 大道理：
        // 1、代码分为 模型代码 和 机制代码；模型就是：长颈鹿是动物；而机制就是 接口，类似于规则。
        // 2、显式实现接口 就是把机制和模型区分开：因为你调用接口必须 用接口类型去声明对象。
        // 3、最好的做法就是：全模型，减少无关的机制。
        // 4、但是想想，I3 one = new Class1() 有时候也不错，我想用这个接口，然后表明我想使用的具体实现。
        // 5、所以，dotNet还是有机制代码的。

        // 显式还是隐式？
        // 1、是不是类的核心成员？如果不是，就显式。
        // 2、如果成员的名称在类中不是很明确（容易引起混淆），那就显式。
        // 3、接口成员是否与其他成员命名冲突，是就显式。
        // 4、避免显式实现，但如果不确定，那就显式。
        // 5、如果你的库被别人引用，那由隐式变成显式实现会造成调用代码全部重构，所以稳妥的使用显式。
        // 6、一个接口成员，可以部分显式，部分隐式。

        // 对于 ((I3)one).Word1
        // 实例可转型为接口
        // 而如果一个接口多个实现，那反过来转型就有点难。
    }

    // 接口的继承
    class Class2 : I5
    {
        string I5.Word5 { get { return "I5的显式时间"; } }

        // 接口如果显式实现，
        // 则：必须使用源接口。
        // 既然这样，最好这样：class Class2 : I5 , I2，突出了可读性。
        // 那就意味着：
        // 1、接口设计，应该使用单独而非复合的方式（通过契约连接）。
        // 2、I5 : I2 应该理解成，你实现 I5，必须也要实现 I2 的契约关系，比如 读取和写入，你可以 读取：写入
        // 3、这么省力，因为有的只要实现 读取 功能。
        // 所以建议隐式声明
        string I2.Word2 { get { return "I2的显式时间"; } }
    }

    // 接口上的扩展方法
    static class Class3
    {
        public static void Do(this I2[] iWords, string words)
        {
            words = "C# 不仅可以为 接口类型的实例 添加扩展方法";
            words = "还可以为 该接口实例 的对象集合添加方法";

            foreach (var word in iWords)
            {
                WriteLine(word.Word2);
            }

            // 对扩展方法的支持是 LINQ 的基础
            // IEnumerable 是所有集合都要实现的接口
            // 通过 IEnumerable 定义扩展方法，所有集合享受 LINQ 支持
        }
    }

    // 通过接口实现多继承
    // 一个类只能继承一个基类和多个接口，所以比较尴尬。
    class Class4
    {
        // 8.8 P266
    }

    // 版本控制
    // 1、如果由其他人使用类库，那修改接口签明会造成代码失效
    // 2、与类不同，也不能为接口添加其他成员，除非让所有开发者和继承的类库添加新成员实现。
    // 3、如果非得增加新特性，想想 USB3.0 和 USB2.0，其实可以通过继承扩展新特性
    interface I7 : I3 { string Word7 { get; } }
    // 旧版本接口 I3，可以选择升级新版本接口，也可以选择不升级
    class Class5 : I3 { string I3.Word1 { get { return "实现I3接口成员"; } } }
    // 新版本接口升级，不影响就有版本。
    class Class6 : I3, I7
    {
        string I3.Word1 { get { return "实现I3接口成员"; } }
        string I7.Word7 { get { return "实现I7接口成员"; } }
    }

    class RunClass
    {
        public static void Run()
        {
            // Class1.SayClass1();
            // Class1.SayBase();

            Class1 one = new Class1();
            // WriteLine(one.Word1);

            // 调用显式接口成员实现
            // 1、强制转换
            // 2、类型声明是接口
            WriteLine($"{one.Word1}"); // 1号接口

            // 实例可转型为接口
            // 而如果一个接口多个实现，那反过来转型就有点难。
            WriteLine($"{((I3)one).Word1}"); // 3号接

            I3 two = new Class1();
            WriteLine($"{two.Word1}"); // 3号接口
        }
    }

    // 最后：
    // 1、优先使用类而不是接口，用抽象类分离 契约（做什么）和实现细节（怎么做）。
    // 2、如果某个类已经继承其他类，这个时候添加新功能，考虑定义接口。
    // 3、接口可以作为 类型声明变量，但不要用无成员接口转换变量，这是滥用，应该回归接口的本意。
}



