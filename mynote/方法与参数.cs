using System;
using System.IO;

// 原则：
// 1、写出容易理解的代码，最好不要包含大量语句，最好不超过10行
// 2、学会使用分解任务的方式（分而治之）写方法。

// * Function 采用的是 分治策略（Divide and Conquer），而一个又一个程序叫模块化程序（Modularized Program），使用这个策略的好处是代码重用（Code Reuse）。
// * 方法的设计时自上而下设计（Top-Down Design），就是步步分解，这样的过程称为 逐步细化（Stepwise Refinement）。
// * 方法有两种：void方法（Void Method）和 返回值方法（Value-Returning Method）。
// * 程序在执行的时候，调用该方法之前，会记录调用之前的点的内存地址，称为返回点（return Point）
// * 可以说：程序的控制权转移到该方法上。

// 方法的连续调用称为；调用栈。
// 只有 async 和 迭代器方法 是在堆上。
namespace NamespaceFunction
{
    // 不能与关键词冲突
    class Function1
    {
        // 在同一类型（同一类）的函数调用时不需要加类名称。
        static public void Run1()
        {
            // * 命名参数（Named Argument）：指定参数的值。
            // * 如果有很多形参，那调用的时候，如果忽略其中一个，那后面的必须全部忽略。
            // * 实参与形参时单独的变量，实参通过值传递（Passed by value）
            // * 如果实参时引用类型的变量，则传递的是地址，修改的是地址处的数据。
            DisplayMessage(value1: "Hello", value2: "dotNet!");

            // ref & Out 的调用：
            int number1 = 1;
            int number2;
            // 调用方法时，参数最好与形参一致，出于可读性。
            UseRef(ref number1);
            Console.WriteLine($"ref：number has changed in 0 => {number1}");
            UseOut(out number2);
            Console.WriteLine($"Out: number has give value of 456 => {number2}");
            // C# 7.0 可以在内部声明 Out 实参
            UseOut(out int number3);
            // C# 7.0 允许完全放弃 Out 参数（弃元），就比如寄快递不需要返回的 订单 一样。
            UseOut(out _);

            // 调用返回元组的方法：
            (string f, string l) name = GetName();
        }


        // C# 7.2 只读传引用：
        // 1、值类型传递引用地址
        // 2、值类型为只读，不可修改
        // 目的就是提高性能
        static private void Method(in int number) { }


        // C# 7.0  多个值通过 元组 打包返回
        static private (string firstName, string lastName) GetName()
        {
            string f = "Tom";
            string l = "Yang";
            return (f, l);
        }


        // ===================================== 关于引用开始 =====================================

        // C# 7.0 返回引用：具体使用，应该专注解决方案。
        // 与对象生存期有关的重要限制：
        // 1、对象在引用时不被垃圾回收
        // 2、不被引用的对象不消耗内存
        // 所有返回的引用只能是：
        // 1、对字段和数组元素的引用（基于堆，可能）
        // 2、其他返回引用的属性和方法
        // 3、作为参数传给 方法 的引用（ref out）
        static private ref string GetName(string[] students)
        {
            // 返回引用注意：
            // 1、返回引用，必须每次都要返回。建议用传引用参数的方式，返回 bool 作为执行状态码。
            // 2、声明引用局部变量，必须进行初始化：你可以用方法返回的 ref 也可以是一个变量引用。
            return ref students[1];
        }

        // Number 是对字段的引用。
        // ref 局部变量一旦被初始化引用一个变量，就不能再进行赋值引用。
        // C# 7.0 允许声明 ref 变量，但不允许声明 ref 字段，比如 ref int[] _Number;
        int[] _Numbers;
        // 自动实现的属性不能声明为 ref，比如：public ref int[] Number { get;set; } }
        // 但允许返回引用的属性
        public ref int[] Number { get { return ref _Numbers; } }

        // 不对的：
        // ref int number = 15;
        // ref int number =null;

        // ===================================== 关于引用结束 =====================================


        // C# 6.0 箭头函数（expression-bodies method）
        static private string GetString(string words) => $"hello, { words }";


        // *这是方法头（Method Header）
        // 1、private：访问修饰符（Access Modifier），当为 private 时，只能类中使用，而如果是 public，则类外部可以使用。
        // 2、void：返回类型（Return Type）
        // 3、DisplayMessage：方法名（Method Name），拼写符合 PascalCase 风格。
        // 4、（string value）：圆括号（parenttheses）,里面参数必须声明类型。形参（Parameter），实参（Argument），变量（Variable）

        // * 实参的兼容性问题：int 可以 赋给 double，但是反过来不行。int 不能 传给 string。
        // * 形参不能这样 string value1,value2
        // * 默认形参：必须为常量，不能时变量，且必须在最后位置。
        static private void DisplayMessage(string value1, string value2, string value3 = "default")
        {
            // 这是方法体（Method Body）
            Console.WriteLine("Hello,This is void Fucntion!");

            // Console 属于 System
            // value 的作用于就是大括号
            Console.WriteLine($"Hello, display {value1}!" + $"And this is default value => {value3}");
        }


        // * 让方法改变变量，可以通过 引用传递（Passed by Reference），两种方法：
        // 1、在方法中使用引用参数 Ref
        // 2、在方法中使用输出参数 Out

        // * 引用形参（Reference parameter）直接对变量进行操作，所以功能相当于：方法与方法之间 “双向通信”
        // 1、调用的方法通过 引用实参 与被调用函数通信
        // 2、被调用函数通过 更改实参 与调用的函数通信
        static private void UseRef(ref int number)
        {
            number = 0;
        }

        // * 输出形参（Output Parameter），与 引用形参 区别：
        // 1、调用传递的时候，实参不需要赋值。
        // 2、方法内必须把 输出形参 赋值。
        static private void UseOut(out int number)
        {
            number = 456;
        }



        // =====================================  参数数组 Params =====================================
        static public void Run2()
        {
            string fullName;

            // 展开形式（Expanded）形式
            fullName = Combine(Directory.GetCurrentDirectory(), "bin", "config");
            fullName = Combine(Environment.SystemDirectory, "bin", "config", "index.html");

            // 正常（normal）形式
            fullName = Combine(new string[] {
                "c:/","Data","index.html"
            });
        }

        // params 关键字 + 参数位置必须最后
        // 注意事项：
        // 1、位置必须最后
        // 2、可以是 零 个实参
        // 3、参数数组类型安全 —— 必须兼容参数数组的类型
        // 4、可以传递实际的数组，而非在里面写入数据
        // 5、如果非要指定必须有一个或者两个元素，那就在调用时显式的写出来，比如 string one, string two, params string[] paths
        static private void Combine1(string one, string two, params string[] paths) { }
        static private string Combine(params string[] paths)
        {
            string result = string.Empty;
            foreach (var path in paths)
            {
                result = Path.Combine(result, path);
            }
            return result;
        }


    }


    class Function2
    {
        // 调用函数
        static public void RunFunction()
        {
            // 第一个
            FunctionReturn();

            // 第二个
            if (IsRight())
            {
                Console.WriteLine("this is ✔ !");
            }

        }


        // string 是 返回值数据类型（Return Type）
        static private string FunctionReturn()
        {
            return "你好，这是返回值！";
        }

        // 返回 Bool，用于判断。
        static private bool IsRight()
        {
            return false;
        }
    }


    class Function3
    {
        public static void Run1()
        {

        }
        // ===================================== 递归 =====================================

        // 递归的性能不高，但是易读性强，开发者应该注意平衡。
        private static int Minus(int number)
        {
            if (number < 1)
            {
                Minus(number);
            }
            else
            {
                return 0;
            }

            return -1;
        }

        // ===================================== 方法重载 =====================================

        // 类中所有方法必须有唯一签明，包含：方法名、参数数据类型、参数数量差异。而不同的 return 不计入签明。
        // 设计原则：
        // 1、尽可能为默认参数提供好的默认值
        // 2、考虑从最简单到最复杂组织重载。
        private static int Sum(int first, int second)
        {
            return first + second;
        }

        // C# 4.0 新增：默认形参在重载的应用：
        // 可以代替 private static int Sum(int first, int second)，因为 third 有默认值。
        // 可以使用 Sum(1, 2) 调用 Sum(int first, int second, int third = 19)
        // 可选参数 必须是 常量，Environment.CurrentDirectory 是变量，所以不行。
        // 注意：如果有一个方法，其中一个有可选参数外，其他参数都一样，而调用的时候忽略了可选参数，那编译器默认选择没有可选参数的方法。
        private static int Sum(int first, int second, int third = 19)
        {
            // C# 4.0 新增：具名参数
            // 根据参数名称显式赋值，而不是按照顺序，这在很多库非常常见
            // 缺点就是：调用接口的参数名称不能被修改了。
            // 所以要养成非常好的习惯：不有随便更改参数名，慎重！
            int mid = Sum(first: first, second: second);
            return third + mid;
        }
        private static int Sum(int[] numbers)
        {
            int result = 0;
            foreach (int number in numbers)
            {
                // 常见的开发模式：
                // 在一个方法上实现核心逻辑，秉承不重复代码的原则，在重载的方法上调用核心的版本。
                // 这也为以后维护方便做铺垫。
                result = Sum(number, result);
            }
            return result;
        }

        // 方法解析问题：
        // 如果一个方法，有很多相似的形参，比如：
        // static void Method(object thing){}
        // static void Method(double thing){}
        // static void Method(long thing){}
        // static void Method(int thing){}
        // 那么，当调用 Method(42)的时候，将选择 int thing，所以原则是：
        // 1、选择类型最具体的，派生程度最大的，int > long > double > object

    }

}