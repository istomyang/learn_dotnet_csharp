using System;
using static System.Console;

// 继承是为了特殊化类（从人到学生）
// 继承（Inheritance）用来扩展类之间 “Is-a”的关系，比如：蚊子 is a 昆虫。
// 基类（Base Class）：也是通用类，派生类（Derived Class）：也是特殊化类。

// 扩展方法
// 1、如果为基类扩展方法，派生类可以使用
// 2、实例方法有更高优先级，如果冲突，优先在类中声明的方法
// 3、很少为基类写扩展方法，哪怕没有基类源代码，也可以在基类和派生类实现的接口上添加扩展方法

namespace Inherit
{
    // 基类
    class BaseClass1
    {
        // private 修饰符：派生类能够继承，但无法访问
        private string _CanNotUse = "Can't Use in Derived Class!";

        // protected 修饰符：只有派生类才能访问的成员
        // 1、如果是实例对象，则无法访问
        protected static string _CanUseOnly = "只有派生类能访问";

        public string Field1 { set; get; }

        // 构造函数
        // 1、实例化派生类时，运行时先调用默认构造函数
        // 2、如果派生类没有指定，则调用默认构造函数（无参数），如果没有默认，就报错
        // 3、所以还是指定构造函数，并且转递参数
        public BaseClass1(string field)
        {
            Field1 = field;
        }
    }

    // 派生类
    // 1、使用 ：表示派生
    // 2、派生类继承基类所有的成员，除了构造函数（想想构造函数的用途就知道原因了）；
    // 3、虽然继承了基类的私有成员，但只有基类的方法才能去访问。
    class DerivedClass1 : BaseClass1
    {
        //  访问基类受保护的成员
        public void F1()
        {
            WriteLine(_CanUseOnly);
        }

        // 指定基类构造函数（转递参数）
        // 1、使用 base关键字 继承Base类的构造函数，这样才能真正继承到Base类的公共特性。
        // 2、base 后面是 基类 的形参列表
        // 3、因为先执行基类，所以基类参数放前面；
        public DerivedClass1(int length, int width, int height) : base(length, width)
        {
            _height = height;
            
        }
    }

    class Use
    {
        // 派生类可以隐式转换为基类，不会异常，但基类转换为派生类必须显式，且可能异常
        // 1、C#编译器允显式转换，而运行时会再次检查，如果无效转型则异常
        BaseClass1 rect = new DerivedClass1();
        DerivedClass1 box1 = (DerivedClass1)rect;


        // 创建派生类实例，首先执行基类的构造函数，然后再执行派生类的构造函数
        // 如果基类和派生类不带参数，那都默认不带参数。
        DerivedClass1 one = new DerivedClass1();

        // 如果不继承Base的构造函数，默认使用不带形参的构造函数，所以将无法设置Base类的参数。
        DerivedClass1 Two = new DerivedClass1(10);

        // 带有继承 Base 的构造函数Use 
        Box three = new Box(10, 10, 10);

        // 如果基类没有不带形参的构造函数，则再派生类中必须使用 base 继承。
    }

    // 单继承
    // 1、一个类只能一个继承
    // 2、可以使用聚合模拟多继承
    class BaseClass1 { public static int _Number1 = 1; }
    class BaseClass2 { public int _Number2 = 2; }
    class Derived1 : BaseClass1
    {
        // 另一个基类是以实例的形式传入
        // 1、声明字段，然后派生类就可修改和访问基类所有公共成员
        // 2、相当于是一个委托。
        // 3、虽然代码有重复的地方，但是不多。
        // 4、基类新增的方法，派生类必须手动加进去。
        private BaseClass2 OtherClass { get; set; }
        public int Value1
        {
            get { return OtherClass._Number2; }
        }
    }

    // 密封类 sealed：防止其他类对他进行派生
    sealed class SealedClass { }
}


namespace Override
{
    // 多态（polymorphism）：派生类能够覆写基类的方法。
    class People
    {
        private string _name;

        // 可以在 属性 上使用多态。
        public virtual string Name { get; set; }

        public People(string name)
        {
            _name = name;
        }

        // virtual关键字：表示派生类可以覆盖此方法。
        // 1、不允许重写属性和实例方法，不允许字段和静态成员
        // 2、适用于 public protected
        // 3、注意：
        // 3.1、运行时遇到 virtual方法会调用派生最远的实现（什么类型调用自己或者上一个的override）
        // 3.2、意味着原来的代码永远不会得到执行，所以谨慎。
        // 3.3、不要轻易把 虚方法 改成 正常方法
        // 3.4、虚方法不要包含关键代码
        // 4、虚方法不适用于 静态成员
        // 5、C++是派生类与基类关联，虚方法调用时基类的实现。而C#相反。
        // 6、不要在构造函数中调用虚方法，尤其是在其派生类实现，会造成无法预知的错误。
        public virtual void SayHello()
        {
            System.Console.WriteLine($"{_name}, Hello!, Im {_name}");
        }

        // 对于实在要虚方法、并且希望执行一些代码
        public void WantRun() { }
        public void F1() // 想让别人实现的功能
        {
            WantRun(); // 我希望执行的功能，不能被覆盖
            YouCanOverride(); // 把该重写的部分给别人
        }
        public virtual void YouCanOverride() { }
    }

    class Student : People
    {
        private int _age;

        public int Age { get; set; }

        // 覆盖基类的属性
        public override string Name { get; set; } = "Student";

        // 可以使用 实参 为基类传值
        public Student(int age) : base("XianMing")
        {
            _age = age;
        }

        // Override关键字：表示会覆盖基类中的方法
        // 1、覆盖的方法只能跟基类一样的参数。
        // 2、如果继承的类是 override，那本方法还必须 override，才能永远传递下去
        public override void SayHello()
        {
            System.Console.WriteLine($"Hello, Im {_age} years old!");
        }
    }

    class Use
    {
        // 8、可以使用 基类引用 new一个派生类，但是有限制：基类引用只能访问基类的成员，派生类的成员无法访问。
        People xm = new Student(20);

        // 9、不要干这种傻事，当然从解决方案的角度不会这么想的
        // Student xm = new People("XM");
    }


    // new 修饰符：继承的一个问题
    class Class1 { public void F1() { } }
    class Class2 : Class1
    {
        // 如果继承的类没有virtual，而我想覆写，那怎么办？这时，基类成为脆弱的基类（brittle base class 或 fragile base class）
        // 这是可以使用 new 关键词，重新创建一个。
        // 这时候，如果继承链中有 virtual，new 是不参与的。
        public new void F1() { }
    }


    // sealed 修饰符：禁止从方法覆写，密封虚成员
    class Class3 { public override sealed void F2() { } }


    // base 成员
    class Class4 { public virtual void F1() { } }
    class Class5 : Class4
    {
        public override void F1()
        {
            base.F1(); // base 表示基类 === this
        }
    }
}

namespace Abstract
{
    // 抽象类（Abstract class）：不打算实例化，也没有成员具体实现，仅仅作为基类。
    // 1、使用 abstract关键词 声明抽象类
    // 2、抽象类与具体类区别在于：前者不能被实例化，哪怕new，也不会编译。
    // 3、抽象也是实现多态的方式
    abstract class Plane
    {
        // 抽象方法目的就是：希望在派生类中去写。
        // 1、抽象方法没有主体。
        // 2、抽象方法自动为虚，不需要 virtual
        // 3、必须是 public
        public abstract void DoSomething();

        // 抽象属性也是如此。
        public abstract string Name { get; set; }
    }

    class Class1
    {
        // 使用 override
        public override void DoSomething() { }
        public override string Name { get; set; } = "halo";
    }

    // 所有类都来自于 System.Object 派生
    // 1、如果一个类没有派生却又 override 方法，则是隐式继承 Object
}

namespace OtherClass
{
    class Class1
    {
        public static void F1(object data)
        {
            // is 操作符：验证基础类型
            // 1、is用于判断数据的类型，便于操作（没有异常处理开销）
            // 2、有些动态性的方法需要验证类型，可以通过参数传递进去
            // 3、只有在无法选择多态性方案才使用
            if (data is string)
            {
                null;
            }
        }

        // C# 7.0 模式匹配（pattern matching）
        public static void F2(object data)
        {
            // 1、判断data是否为string
            // 2、判断结果作为 bool 赋值给行变量 text
            if (data is string text && text.Length > 0) { }
        }

        public static void Eject(Storage storage)
        {
            // 支持模式匹配的switch，新功能
            // 1、case 可以是任何类型
            // 2、可以声明一个变量 UsbKey usbKey，作用域为这一小节
            // 3、case 标签支持条件表达式
            // 4、顺序变得重要：如果一开始就是基类，则后面就不执行了，如果没有条件表达式，会报错，null可以随意位置
            // 5、可以为相同类型写多个 case
            // 6、常量case可以和 模式匹配case混用。 case 11 === case int i when i == 11
            // 7、不允许可空类型 int?
            // 8、和 is 一样，只有在无法选择多态性方案才使用
            switch (storage)
            {
                // nameof 运算符获取变量、类型或成员的名称作为字符串常量
                case null: throw new ArgumentException(nameof(storage));
                case UsbKey usbKey when usbKey.IsPluggedIn: usbKey.unload(); break;
                default: throw new ArgumentException(nameof(storage));
            }
        }

        public static void F3()
        {
            // as 操作符：类型转换
            // 1、尝试将对象转换为特定数据类型，失败返回null，避免异常
            // 2、但不能判断数据类型
            // 3、as 操作符要求采取额外步骤对赋值变量执行空检查，而is操作符自动包含，所以 C# 7.0以上基本上用不着 as操作符
            object data;
            WriteLine($"{ data as string }");
        }
    }
}