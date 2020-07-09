using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;


namespace MyNote.Delegate.概述
{
    class Class1
    {
        private static void Fn1(int number, Func<int, string> f) /*调用函数时，传递一个函数。是泛型。*/
        {
            WriteLine(f(number));
        }

        private delegate string F2(int number); /*自己声明委托，代替 Func<int, string>*/
        private static void Fn2(int number, F2 f)
        {
            WriteLine(f(number));
        }

        // 使用
        private static string f1(int number) /*实现委托，名称可不一样，其余签明必须一样*/
        {
            return (number * 10).ToString();
        }
        private static string f2(int number) /*实现委托*/
        {
            return (number * 20).ToString();
        }
        public static void Main1()
        {
            Fn1(10, f1); /*委托是一个对象实例，就是个方法。*/
            Fn2(20, f2);
        }
    }


}

namespace MyNote.Delegate.Lambda
{
    // Statement Lambda
    class Class1
    {
        private static void Fn1(int number, Func<int, string> f1)
        {
            WriteLine(f1(number));
        }

        public static void Run()
        {
            Fn1(10, (int number) => { return number.ToString(); }); /*语句*/
            Fn1(20, (number) => { return number.ToString(); }); /*类型推断*/
            Fn1(30, number => { return number.ToString(); }); /*单一参数*/
        }
    }

    // Expression Lambda
    class Class2
    {
        private static void Fn1(int number, Func<int, string> f1)
        {
            WriteLine(f1(number));
        }

        public static void Run()
        {
            Fn1(10, (number) => number.ToString()); /*表达式*/
        }
    }

    /* Lambda函数 不属于任何 类型，无法使用 typeOf()*/

    // C#2.0 匿名方法
    class Class3
    {
        private static void Fn1(int number, Func<int, string> f1)
        {
            WriteLine(f1(number));
        }

        public static void Run()
        {
            Fn1(10, delegate (int number) { return number.ToString(); });
            /**
            * 必须有 delegate
            * 必须显式 参数类型
            * 必须有 {} 代码块
            * 必须转换成委托类型
            */

            /**
            * 可以 无参数
            */
        }
    }

    // 委托没有结构相等性
    class Class4
    {
        private delegate string F2(int number);
        private static string f1(int number)
        {
            return (number * 10).ToString();
        }
        public static void Run()
        {
            Func<int, string> F1 = f1;

            F2 f2 = new F2(f1);
            // Func<int, string> f3 = f2; /*不能转换类型*/
            // Func<int, string> f3 = f2.Invoke(10); /*????*/
        }
    }

    // 委托可变性
    class Class5
    {
        public static void Run()
        {
            Action<object> a1 = (object o1) => { };
            Action<string> s1 = a1;

            Action<string> s2 = (string s1) => { };
            Action<object> a2 = a1;

            Func<object, string> so = (object o) => { return o.ToString(); };
        }
    }

    /**
    * Lambda会延长外部变量的生存期。
    * 在CIL中，对象是创建一个 闭包对象，以对象字段的形式存储 外部变量
    * C#会保持这些外部变量的最新版本，然而C#5.0后，只有foreach会保留创建时的变量版本
    */
    class Class6
    {
        public static void Run()
        {
            string[] names = { "Tom", "Jerry", "Lucy" };
            List<Action> actions = new List<Action>();

            foreach (string name in names)
            {
                actions.Add(() => WriteLine(name));

                // C#5.0前解决方案
                string _name = name;
                actions.Add(() => WriteLine(_name));

            }

            foreach (Action action in actions)
            {
                action();
            }

            /**
            * C#5.0
            *   Tom
            *   Jerry
            *   Lucy
            * C#4.0
            *   Lucy
            *   Lucy
            *   Lucy
            */
        }
    }

    /** 
    * 表达式树：不参与编译，执行时分析，转换成数据语言，查找数据库。
    * 编译器判断是表达式树（数据库对象）还是委托，分别进行隐式转换，谁行就是谁。
    */
    class Class7
    {
        void Run()
        {
            List<string> fruits = new List<string> { "apple", "passionfruit", "banana" };
            IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);
        }
    }
}

namespace MyNote.Delegate.My
{

#region delegate type have more than one functions
    /// <summary>
    /// one type use A delegate function.
    /// </summary>
    class C1
    {
        // public Action<int> Number { get; set; }
        private int _number = 10;

        public void F(Action<int> number)
        {
            WriteLine("发布者");
            number(_number);
        }
    }

    class R1
    {
        public static void F1()
        {
            C1 c1 = new C1();

            /// <summary>
            /// Action can merge into one Action.
            /// </summary>
            Action<int> f_1 = f1;
            Action<int> f_2 = f2;
            Action<int> f_3 = f_1 + f_2; /* this can merge into publisher type. */

            // use the intergated Action Function!
            c1.F(f_3);
        }

        private static void f1(int number) { WriteLine("1订阅者"); }
        private static void f2(int number) { WriteLine("2订阅者"); }
    }

    #endregion 


}
















