using System;
using System.Collections.Generic;
using static System.Console;

// 设计规范：
// 1、不要在一个文件放置多个类
// 2、用类名称定义文件名

// 关于垃圾回收
// 1、C# 没有明确的 delete操作符
// 2、CLR 会管理内存，但是不管理 数据库连接、网络端口等
// 3、可以通过 using 语句显式确定性资源的清理
// 4、通过终结器支持隐式资源清理

// 属性和字段设计规范
// 1、属性代表数据，方法代表行动，不要用方法可以修改属性
// 2、命名问题：建议使用 _PascalCase或者 m_PascalCase（C++命名规定，m:member variable），尽量避免 _camelCase因为局部变量和参数也是这样的写法

// new 实现细节
// 1、从内存管理器获取空白内存
// 2、调用构造函数，把内存引用作为 this 传给构造函数
// 3、一边拷贝一边修改this
// 4、修改完毕，返回 this（内存地址）

// 终结器在 合式类型
// 1、垃圾回收会识别有终结器的对象
// 2、添加到垃圾回收队列
// 3、使用独立线程删除对象

// ？
// nameof的使用

// 实例（instance）
namespace Class
{
    class Class1
    {
        public static void Run()
        {
            // 类 不是对象，而是对象的描述（模板）
            // 1、new 应该理解为实例化对象，而分配内存只是其中一部分
            Class2 my = new Class2("Tom");
            my.SayHello();
            Console.WriteLine($"访问字段：{ my.GetName() }");
            // 2、对象没有 回收内存 的操作符，但对象不再应用，就由 垃圾回收器 回收。
            // 3、我觉得可以这样。
            my = null;


            // 使用属性
            // 在用属性传递给函数时，不能用 ref 和 out（也就是引用传递）
            my.Name = "Harry";
            Console.WriteLine($"访问属性：{ my.Name }");

            // 对象传递到方法，验证传递对象引用
            UseClass(my, "Jerry");

            // 将对象传递给方法
            // 1、对象是引用类型，所以哪怕没有 ref ，还是会修改原来的对象，而不是对象的拷贝
            // 2、使用 @作为转义，哈哈
            static void UseClass(Class2 @class, string newName)
            {
                @class.SetName(newName);
                @class.SayHello();
            }


            // Class Overlaoded
            Overloaded obj = new Overloaded("Tom");

        }
    }


    // 类头（Class Header）
    class Class2
    {
        // 成员声明（Member Declaration）：是定义类字段、属性和方法的语句。

        // 私有字段（Field）
        // 1、使用 private 访问修饰符，防止外面访问
        // 2、将所有字段设为 private 并使用方法提供对字段的访问
        // 3、习惯：使用 下横线 表示字段，来消除 属性名 和 字段名 混淆，建议使用 PascalCase。
        private string _name;
        private string _age;
        private int _score;

        // 常量字段
        // 1、不可修改
        // 2、自动成为静态字段，如果显式会报错
        // 3、只能是有字面值的类型 string int double 等
        // 4、一定不要改变，一旦其他程序集使用，这个值是要被拷贝走的
        // 5、如果将来可能要改，就用 readonly
        public const float PI = 3.14562F;

        // readonly（C# 6.0后不要使用了）
        // 1、只能用于字段（不能用于变量）
        // 2、只能 构造函数 和 初始化器 指定值
        // 3、可以是实例或静态字段（只能更改一次）
        // 4、可在 执行时 为其赋值（只有一次就行）
        // 5、readonly 可从属性外访问，其余字段不行
        // 6、和 const 相比，不限字面值。
        // 7、由于规范要求，字段必须只能属性方法，所以 C# 6.0后不用了
        // 8、你应该使用 只读自动属性
        // 9、★ 不管 自动属性还是readonly，确保数组引用“不可变”至关重要
        // 9.1、可对数组施加只读限制，防止赋值的新数组 把 旧数组 丢弃
        // 9.2、但允许修改（不能添加）数组元素，因为保证引用不变（数量、实例变量）
        // 10、设计规范
        // 10.1、C# 6.0之后，优先使用 只读自动属性 而不是 readonly
        // 10.2、C# 6.0之前，为预定义对象实例使用 public static readonly XXX
        // 10.3、如果 C# 6.0 之后看到 readonly，则考虑兼容，不要改 只读自动属性。
        public readonly int _readOnly;
        public readonly static Class3 obj = new Class3(10); // 预定义，显然必须是静态的，直接用

        // 属性
        // 1、使用属性访问器（Accessor）
        // 2、属性并非公共字段，而是 属性访问器 和 私有字段 联合起来一套方法：属性 = 属性访问器 + 私有字段
        // 3、私有字段也被称为属性的 后备字段（Backing Filed），因为 字段存储属性的值；(如果只要 get，就没必要后备字段)
        // 4、public：属性被外部代码访问；string：声明属性是个字符串；Name：该属性的名称（和私有字段不一样，通过属性名访问私有字段）
        public string Name
        {
            // 使用访问器的目的是数据赋值前进行检查
            // 或者数据同步，哈哈！
            // 如果只有一条语句，可以这么写
            get { return _name; }
            private set
            {
                // value 形参是“隐式”声明的
                // 数据类型 与 属性类型 相同
                // 如果设置属性只读，就去掉 set{}
                _name = value;
            }
        }

        // C# 3.0 属性：简写
        // 自动属性（Auto-Properties）
        // 1、自动属性不需要后备字段
        // 2、当属性没有深入的逻辑时（检查输入），简写时，使用自动属性
        // 3、如果不简写，发现必须使用 后背字段
        // C# 6.0 使用 “20” 对 Age 进行初始化
        // public string Age { get; } = "20"; 设置为只读。
        public string Age { get; private set; } = "20";

        // C# 7.0写法
        public int Score
        {
            get => _score;
            set => _score = value;
        }

        // C# 6.0只读自动实现属性
        // 1、不需要 后备字段
        // 2、只能在构造器或者属性声明时初始化
        // 3、除非字段和属性类型不一样时
        public int[] CanRead1 { get; } = new int[10];
        public int[] CanRead2 { get; }

        // 构造器（Constructor）
        // 1、构造函数名称必须与类相同
        // 2、没有 返回值类型
        // 3、是 public（大多数情况）
        // 4、一个 类 可以没有构造函数，这时候编译器会提供一个 默认构造函数（Default Constructor）：不带任何数据初始化，值类型用 0，引用用 null
        // 5、如果开发者写了构造器，则默认失效
        // 6、可以试试，在构造器执行一个 private函数，这个函数去默认初始化属性
        public Class2(string yourName)
        {
            _name = yourName;
            _age = "20";
            CanRead2 = new int[10];
        }

        // 方法（Method）
        public void SayHello()
        {
            Console.WriteLine($"{ _name }, 你好！");
        }
        // 1、可以调用内部方法
        public void UseThisFn()
        {
            // this
            // 1、可在类中显式访问方法
            this.SayHello();
        }

        // 静态方法
        public static void StaticF1()
        {

        }

        // 访问私有字段 (不使用这个，而使用访问器)
        public string GetName()
        {
            return this._name;
        }

        public void SetName(string newName)
        {
            this._name = newName;
        }
    }

    class Class3
    {
        private int _number1;
        private int _number2;
        private int _number3;

        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Number3 { get; set; }

        public Class3(int number1 = 10) // 习惯：设置默认值
        {
            _number1 = number1;
        }

        // 构造器链(值类型也有)
        // 1、减少重复代码
        // 2、别人的是否放最后？
        public Class3(int number2, int number3, int number1) : this(number1)
        {
            _number2 = number2;
            _number3 = number3;
        }

        // 解构函数
        // 1、构造器：把参数封装到对象实例中
        // 2、解构函数：把对象实例中参数拆开，
        public void MyDeConstruct(out int number1, out int number2, out int number3)
        {
            // 使用 C# 7.0元组语法
            (number1, number2, number3) = (_number1, _number2, _number3);
        }

        // 默认解构函数（用这个）
        // C# 7.0起，可使用实例赋值元组直接解构
        // 1、必须是 Deconstruct签明
        // 2、必须 void
        // 3、接收大于等于两个out参数
        public void Deconstruct(out int number1, out int number2, out int number3)
        {
            (number1, number2, number3) = (_number1, _number2, _number3);
        }
    }

    class RunClass3
    {
        public void Run()
        {
            // 对象初始化器（object Initializer）
            // 1、赋值所有可访问成员属性
            // 2、左边时属性，右边是值，等价于赋值。
            Class3 obj = new Class3(20) { Number1 = 2, Number3 = 3 };

            // 集合初始化器
            List<Class3> objs = new List<Class3>()
            {
                new Class3(1),
                new Class3(2),
                new Class3(3)
            };

            // 解构函数
            obj.MyDeConstruct(out int number1, out int number2, out int number3);
            WriteLine(number1);
            WriteLine(number2);
            WriteLine(number3);

            // C# 7.0隐式调用 Deconstruct方法
            int num1, num2, num3;
            (num1, num2, num3) = obj;
            // 不可以这样：
            // (int, int, int) numbers = obj;
            // (int nmber1, int number2, int number3) numbers = obj;
        }
    }


    // 静态成员
    // C#上静态成员和全局成员功能上无异，除了提高访问修饰符，更好的封装
    class Class4
    {
        // 静态字段：多个实例共享的数据
        // 1、可声明时初始化 0 null false 取决于 default(T)
        // 2、未初始化可访问
        // 3、使用类名，非实例名，访问静态字段
        // 4、字段的作用域是类，不需要 this，也不需要用字段取代属性
        // 5、不能定义同名字段（静态、实例）
        // 6、实例字段可以 this，而静态不能 
        public static int NextId;

        // 静态属性：
        // 1、多使用公共静态属性而非公共静态字段
        // 2、C# 6.0使用自动实现属性（带初始化）
        public static int Id { get; private set; } = 42;
        public string Name { get; set; }

        // 静态方法
        // 1、与实例无关，都纳入静态
        // 2、不能有 this
        // 3、若引用实例成员，参数加实例对象
        public static void SayHello(string name) { }

        // 静态构造函数
        // 1、不显式调用，运行时自动处理（实例化、执行普通构造函数、访问字段时）
        // 2、既然不显式，即不允许有参数
        // 3、作用：初始化静态数据(不能在声明时获取，比如到服务器获取某个东西)
        // 4、如果构造器都对变量有初始化，静态构造器赋值 优先于 实例声明的赋值
        // 5、没有 “静态终结器” 的说法
        // 6、不要在静态构造器抛出异常，否则造成应用程序剩余生命期无法使用
        // 7、运行机制
        // 7.1、静态构造 在访问类成员之前执行
        // 7.2、如果眉头静态构造，则初始化静态成员默认值，不再检查静态构造函数
        // 7.3、静态成员会在访问前得到初始化
        // 7.4、但不会在所有静态成员初始化完才允许调用（有的时候需要等待网络）
        // 7.5、因为类用到才会初始化，为造成不必要代价，尽量避免高代价的静态构造函数（以内联方式初始化静态字段）
        static Class4()
        {
            Random rand = new Random();
            NextId = rand.Next(101, 999);
        }

        public Class4(string name)
        {
            Name = name;
            Id = NextId;
            NextId++;
        }
    }

    // 静态类(不含任何实例字段，纯方法类)
    // 1、C# 编译器会自动标记 abstract 和 sealed 修饰符 —— 该类不可扩展
    // 2、可使用 using static Class5
    public static class Class5
    {
        // 目前看到的代码，内部依然加上 static
    }

    // 扩展方法1
    // 提供被扩展的实例
    public class Class6
    {
        public void F1() { }
    }
    // 扩展方法2
    // C# 3.0扩展方法：模拟为其他类创建实例方法
    // 1、更改签明，第一个参数为：this + 被扩展类型 + 实例指针
    // 2、扩展方法必须是静态的（CLI作为普通静态方法调用），被扩展必须是非静态
    // 3、如果扩展方法和被扩展类型分离，需要 using 被扩展类型命名空间
    // 4、如果扩展函数签明冲突，则扩展不被调用，除非一个是实例方法，一个是静态方法
    // 5、注意：通过继承特化类型由于扩展方法
    // 5.1、旧扩展方法被覆盖，没有警告的
    // 5.2、如果你没有 被扩展类 没有控制权，更严重
    // 5.3、VS IDE 不易分辨是不是扩展方法
    // 6、慎用，尤其不要为 Object类型定义，另外接口也有讨论
    public static class Class6Ext
    {
        // 1、必须是静态
        // 2、方法名不能和源冲突
        // 3、第一参数：this 类型 实例变量
        public static void SayHello(this Class6 example, int name) { }
    }


    // 嵌套类：如果一个类在外部么有意义，就做成嵌套
    class Class7
    {
        // 1、命名不需要有什么前缀
        // 2、可以 私有
        // 3、嵌套类 this 与包含类无关
        // 4、嵌套类能访问包容类私有成员，反之不行
        // 5、小心 public 嵌套类
        private class Class71 { }

        // 只能通过次访问
        Class71 obj = new Class71();
    }

    // 分部类：C# 2.0
    // 1、编码规范：一个类一个文件
    // 2、应用1：把类分别放入多个文件中
    // 3、应用2：把嵌套类放到自己的文件中
    // 4、不允许用分部类扩展编译好的类（C# 3.0分部方法），只能用于拆分多个文件
    partial class Class81
    {
        ClassKid obj = new ClassKid();
    }
    partial class Class82
    {
        private class ClassKid { }
    }

    // 分部方法：只能在分部类中，只能为代码生成提供方便
    // 1、分部方法允许声明的方法不需要实现，如果有实现，则建议放到 姐妹 类中
    // 2、【进一步分离业务，专注于方法的编写，更重要是，有点像抽象方法或者接口】
    public partial class Class91
    {
        // 专门声明一系列分部方法接口
        // 1、partial 关键词
        // 2、分号
        // 3、不允许访问修饰符
        // 4、必须返回 void
        // 5、不允许 out，但可以 ref
        partial void DoSomething(ref bool isOk, string value);

        private string _data;
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                bool isOk;
                DoSomething(ref isOk, value);
                _data = value;
            }
        }
    }
    // 姐妹类
    partial class Class92
    {
        // 分部方法的实现
        partial void DoSomething(ref bool isOk, string value) { isOk = true; }
    }

    // 关于分部思想思考：
    // 1、有分部方法，是因为内部用所调用
    // 2、那如果部分之间无耦合，是否可以分部？字段？普通的方法？


    // 关于重载
    class Overloaded
    {
        // Field
        private string _name;
        private int _number;

        // Accessor
        public string Name { get; set; }

        // 重载的构造函数
        public Overloaded()
        {
            _name = "Jerry";
        }
        public Overloaded(string name)
        {
            _name = name;
        }
        // C #7.0
        public Overloaded(int number) => _number = number;

        // 重载的方法
        // 1、方法在被调用时：与正确的方法进行匹配叫做：绑定（binding）
        // 2、编译器会根据 函数名和参数列表 来选择，这个叫：签名（signature）
        // 3、那就意味着：如果只是返回值不同就不行。
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }

        public void SayHello(string name1, string name2)
        {
            Console.WriteLine($"Hello, {name1} and {name2}!");
        }

        static public void TheRest()
        {
            // class对象的数组
            // 1、如果 class 时成员，那这就是 成员数组
            // 2、细细品味在 C# 中什么时数组，是不是万物皆可 XXX[]
            const int SIZE = 4;
            Class2[] hellos = new Class2[SIZE];
            hellos[0] = new Class2("Tom1");
            hellos[1] = new Class2("Tom2");
            hellos[2] = new Class2("Tom3");
            hellos[3] = new Class2("Tom4");

            foreach (var hello in hellos)
            {
                hello.SayHello();
            }

            // class对象的List（未学）
        }
    }
}