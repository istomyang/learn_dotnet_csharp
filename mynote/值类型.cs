using System;
using System.Collections;
using static System.Console;

// 值类型：
// 1、int bool decimal 等等 (string是引用类型)
// 2、结构、枚举

// 值类型和引用类型的区别：拷贝策略

// 值类型：
// 1、变量名称 直接和 数据位置 关联。
// 2、所以除非使用 ref out 参数，否则将一个变量赋值给另一个变量，会发生数据的拷贝。
// 3、因为拷贝策略是：内存数据拷贝，所以不要创建过大的值类型（一般小于 16 字节）。
// 4、【想想数据结构的 顺序表】
// 5、值变量存储在 栈 中，速度更快，垃圾回收代价更低，但由于频繁的存储，还是会对性能造成影响。
// 6、值类型一般是短时间存在，通常作为表达式的一部分，或用于激活方法。
// 7、new 本质
// -- 7.1、运行时 会在临时存储池创建对象的新实例
// -- 7.2、将所有字段为默认值，调用构造函数
// -- 7.3、将临时存储位置作为 ref 变量以this形式传递 【ref可更改的，因为在临时地方创建】
// -- 7.4、值被拷贝到与变量关联的位置。【ref自动改变】

// 引用类型：
// 1、引用变量 是对 对象实例 的引用，变量名称 和 内存地址关联。
// 2、引用变量要么是 null，要么就是对 需要垃圾回收的堆 上的数据引用。
// 3、拷贝变量拷贝的是内存指针：这个容量非常小（32位是4字节，64位是8字节），所以速度很快。
// 4、如果传递给方法，则方法会对原始对象做出改变。
// 5、new 本质
// -- 5.1、运行时 会再托管堆上创建对象新实例
// -- 5.2、将所有字段初始化为默认值，再调用构造函数
// -- 5.3、将对新实例的引用以 this 传递
// -- 5.4、new 操作符返回对实例的引用
// -- 5.5、该引用拷贝到与变量关联的内存位置

// 看到 VSCode 的引用数吗？：那些函数都在堆里面存放。

// 不要定义结构，除非它在逻辑上代表一个值，消耗16字节甚至更少的空间，不可变，很少装箱。
namespace Value1
{
    // 值类型也可实现接口。
    interface IAngle { void SayHalo(); }

    // 结构：高精度角
    // 1、结构一旦实例化，便不可修改。【想想看这是在栈里面，使用的是 顺序表结构。】
    // 2、值类型里可以有：属性、字段、方法、构造函数（不包括自定义的无参默认构造函数，编译器自己自动。）
    // 3、构造器：必须初始化所有字段和属性（C#6.0 只需要初始化属性就行）【之后全部只读】
    // 4、C# 不允许 字段 设置默认值：int _number = 1；
    // new 问题：
    // 1、结构不支持终结器
    // 2、使用 new 拷贝的时候，运行时 并不没有机制跟踪值类型有多少拷贝。
    struct Angle : IAngle
    {
        // C# 6.0：只读自动实现属性
        public int Degrees { get; }
        public int Minutes { get; }
        public int Seconds { get; }

        // C# 6.0 之前用法
        // readonly private int _number;
        // public int Number { get { return _number; } }
        // 如果用这个，那构造器需要改成 _Number = number

        // 构造器1
        public Angle(int degrees, int minutes, int seconds)
        {
            Degrees = degrees;
            Minutes = minutes;
            Seconds = seconds;
        }

        // 构造器2：default操作符
        // 1、default(int)和 new int()生成一样的值
        // 2、如果 default(T) 的 T 是值类型就是有效的，如果是 引用类型，则抛出 NullReferenceException 错误
        // 3、对一个引用类型来说不能产生有效状态，实例化后要初始化该类型
        public Angle(int degrees, int minutes) : this(degrees, minutes, default(int))
        {
            // C# 7.1 起可不加 (int) 类型
            // public Angle(int degrees, int minutes) : this(degrees, minutes, default)
            Degrees = degrees;
            Minutes = minutes;
        }

        void IAngle.SayHalo() { }

        public Angle Move(int degrees, int minutes, int seconds)
        {
            return new Angle(
                Degrees + degrees,
                Minutes + minutes,
                Seconds + seconds
            );
        }
    }

    // C# 7.2：可在编译时验证结构只读
    // 如果发现非只读字段或者某个属性含有 setter 就报错。
    readonly struct Struct1 { }

    class Class2
    {
        public Angle a { get; set; }

        // 值类型结构必须使用 new 调用构造函数，显式初始化数据。
        // 如果不使用 new，则所有数据隐式初始化为各数据类型的默认值。
        // 什么情况下不使用 new：
        // 1、实例化含有未赋值的值类型字段的一个引用类型
        // 2、实例化值类型的一个数组，不使用数组初始化构造器时。
    }

    class Class3
    {
        // 装箱（boxing）：把值类型数据拷贝到堆上，然后变成引用。
        // 1、堆上分配内存：用来存放值类型的数据 和 少许额外开销（SyncBlockIndex和方法表指针）
        // 2、内存拷贝操作：值类型数据拷贝到堆上分配好的位置
        // 3、转换的结果是对 堆上新存储位置的引用
        // 拆箱（unboxing）：与装箱相反。

        // 想想看装箱和拆箱的作用：
        // 1、值类型最大的结构，很大
        // 2、那基于内存管理，你应该不用的先打包（装箱）到堆上
        // 3、如果要用，就拆包（unboxing）
        // 4、但是装箱会严重影响性能，所以应该学习在 CIL 中统计 box/unbox 指令数目，比如：
        private void F1()
        {
            // 5、装箱拆箱的性能问题主要体现在：强制类型转换。—— 把值类型转换成引用类型

            int number = 3;
            object myObject;

            // Boxing 装箱
            // CIL代码： IL_0005 box [mscrolib]System.int32
            myObject = number;

            // Unboxing 拆箱
            // CIL代码： IL_000c unbox.any [mscrolib]System.int32
            number = (int)myObject;
        }

        // 6、下面是性能的典型问题
        private void F2()
        {
            // ArrayList 维护的是对象的引用列表
            ArrayList list = new ArrayList();

            // double 继承自 Object，是引用类型
            // list 只能添加 引用类型 变量，比如 double
            list.Add((double)1);
            list.Add((double)2);
            list.Add((double)3);
            list.Add((double)4);
            list.Add((double)5);
        }

        private void F3()
        {
            // Lock 语句中的值类型:
            // 1、Lock 语句编译出来，相当于 System.Threading.Monitor 的 Enter 和 Exit 方法
            // 2、两个函数必须成对使用，Enter 传递 lock，而 Exit释放 lock
            // 3、值类型的问题在于装箱，所以每次 Enter和 Exit 都必须在堆上创建新值
            // 4、而撞见完新值后，对象就变了，所以 Lock语句 不允许使用值类型。


            // 如果结构有个方法可以修改内部字段:
            // 1、(Angle)objectAngle.MoveTo(2,2,2)
            // 2、会 拆箱、会创建值的拷贝，拷贝的过程中修改完成，但是会被丢弃，原堆无改变
            // 3、(IAngle)angle.MoveTo(5, 5, 5)
            // 4、值类型被装箱，然后返回箱子的引用，然后修改箱子的值，然后原值未修改。
            // 5、(IAngle)objectAngle.MoveTo(6,6,6)
            // 6、这不是装箱转换，而是引用转换，修改的是原值。


            // 可变值类型:
            // 1、假如结构继承某个接口（引用类型），然后初始化，返回实例。
            // 2、实例再强制转型为 接口，变成引用类型（值的拷贝），就可以修改该值类型（其实是装箱）
            // 3、这时候，修改的是堆上的箱子，而不是原来的值。
            // 4、然后再拆箱，变成值类型，也就是可变的值类型。


            // 避免方法调用期间避免装箱:
            // 1、调用方法，不可能是值类型，而必须是变量（引用类型）
            Object objAngle = new Angle(2, 2, 2);
            // 2、因为，方法可能修改值本身，而不是修改值的拷贝，然后扔掉。 
            // 3、而像这样，(Angle)objectAngle.MoveTo(2,2,2)
            // 4、方法的调用者，也就是 this，是值拷贝过程中的那个新值，而不是箱子
            // 5、如果是 Angle angle，那要么是 在临时存储位置新建，要么报错，一般是前者，调用完就丢弃。
            // 6、对于 (Angle)objectAngle.MoveTo(2,2,2)，先拆箱后调用，无论方法是否修改本身都遵循这样的流程：
            // -- 6.1、对 装箱的值 进行类型检查；
            // -- 6.2、拆箱，生成值的存储位置，并分配临时变量
            // -- 6.3、将箱子里的 值 拷贝到临时变量的存储位置
            // -- 6.4、再调用方法，传递刚刚那个临时位置
            // -- 6.5、如果不修改本身，那很多工作都避免，但是 C# 不知道会不会修改本身，所以“杀一错百”。
            // 7、有个方法可以免除开销：调用值类型的 接口
            // -- 6.1、想想看，值类型是可以继承接口的，而接口是引用类型
            // -- 6.2、(IAngle)objectAngle.MoveTo(6,6,6)
            // -- 6.3、这是就不需要 拆箱，运行时 之间把箱子作为调用者。


            Angle angle = new Angle(2, 2, 2);
            Object angleObj = angle;
            // 如果 值类型的实例 作为调用者 调用 Object声明的虚方法，会视情况而定：
            // 1、接收者已拆箱，结构重写虚方法。那就直接调用结构的方法。
            angle.ToString();
            // 2、接收者已拆箱，结构没有重写虚方法。那就必须调用基类实现，调用者被装箱。
            ((Object)angle).ToString();
            // 3、接收者已装箱，结构重写虚方法。直接调用箱子的方法，不拆箱。
            angleObj.ToString();
            // 4、接收者已装箱，结构没有重写虚方法。把箱子的引用传给基类调用。
            ((Object)angleObj).ToString();
        }
    }
}

// 枚举使得程序更加通俗易懂
namespace Enum
{
    // 枚举
    // 1、在 Switch语句中，我们可以用枚举值代替 Case后面的值，这在性能上完全相同，并且提升可读性
    // 2、枚举的关键特征是生成一组具名常量值，使代码更易读
    // 3、规定，除了 位标志，枚举名称为 单数形式
    // 4、枚举的值作为整数常量实现，第一个默认0，后续递增1，但可以显式，只能是 int
    // 5、枚举的性能体现在基础类型的性能上，默认为 int，可以继承除 char 之外的整型(其实不是继承，只是语法一致性，其实是声明 short 类型)
    // 6、int 是 32位的，如果考虑性能，应该使用 小的整形，而如果标志数超过 32个，就要考虑大的。

    // 注意：
    // 1、添加新成员需要注意兼容问题（向后添加）
    // 2、避免创建不完整的枚举（问题考虑不到）
    // 3、避免创建供未来使用的值（未来的值，放在最后面）
    // 4、避免只包含一个值（有必要吗？）
    // 5、给简单的枚举提供 0 代表无。（如果你不初始化，默认就是0）
    enum Name : short
    {
        Tom = 10,
        Jerry = Tom,
        Bob,
        Daniel
    }

    enum ConnectState1
    {
        Disconnected,
        Connecting,
        Connected,
        Disconnecting,

        //新添加的值：
        // 1、如果已被其他人使用
        // 2、不能插入，因为后面的值会顺移
        // 3、只能插入末尾，保证兼容性
        NewValue
    }

    enum ConnectState2
    {
        Disconnected,
        Connecting,
        Connected,
        Disconnecting
    }

    class Class1
    {
        private void F1()
        {
            // 枚举类型的值能转换成 short，也能转换成枚举类型
            // 1、从基础类型值 转换 枚举值，必须显式。除非 数字0，它能隐式转换。
            int name = 46;
            short shortName = (short)name;
            Name name1 = (Name)name;


            // 枚举之间类型兼容
            // 1、C# 不允许不同枚举数组的直接转型，但CLR允许，所以要绕过限制。
            // 2、做法就是：先转型为 System.Array
            ConnectState1[] states = (ConnectState1[])(Array)new ConnectState2[20];
            // 3、还可以非法 int[]转 uint[]
            uint[] number1 = (uint[])(Array)new int[10];


            // 枚举和字符串之间转换
            // 1、将枚举值写入 跟踪缓冲区(trace buffer)，自动调用 ToString()方法
            System.Diagnostics.Trace.WriteLine($"{ Name.Tom }");
            // 2、字符串转枚举，不利用泛型（dotNet Core没有了）
            Name name2 = (Name)System.Enum.Parse(typeof(Name), "xxxxxxxxxx");
            // 3、字符串转枚举，利用泛型
            System.Diagnostics.ThreadPriorityLevel priority;
            if (System.Enum.TryParse("xxxxx", out priority))
            {
                Console.WriteLine(priority);
            }
            // 4、所有字符串到枚举，都是不能本地化的（中文，转换成日语），所以只要不给用户看的，都可以转换。
        }
    }

    // 枚举作为标志使用（例：文件属性）
    // 1、位标志 名称为复数，因为代表一个 flag的集合
    // 2、标志作用：可以进行复合，表示复合值，比如 只读和隐藏，就可以用位操作符合起来。

    // FlagsAttribute 类
    // 1、这个 [Flags] 指出枚举值可以组合
    // 2、也会改变 ToString和 Parse的行为，
    // 3、比如：FileAttributes1.ReadOnly.ToString()直接输出 ReadOnly，而不是 1
    // 4、如果两个枚举值相同，直接输出第一个。（因为没有本地化，所以使用谨慎）

    [Flags]
    public enum FileAttr // 实现 System.IO.FileAttributes
    {
        // 1、养成没有枚举值都有 None=0 的好习惯，因为默认值就是 0

        None = 0,                       // 000000000000000
        ReadOnly = 1 << 0,              // 000000000000001
        Hidden = 1 << 1,                // 000000000000010
        System = 1 << 2,                // 000000000000100
        Directory = 1 << 4,             // 000000000010000
        Archive = 1 << 5,               // 000000000100000
        Device = 1 << 6,                // 000000001000000
        Normal = 1 << 7,                // 000000010000000
        Temporary = 1 << 8,             // 000000100000000
        SparseFile = 1 << 9,            // 000001000000000
        ReparsePoint = 1 << 10,         // 000010000000000
        Compressed = 1 << 11,           // 000100000000000
        Offline = 1 << 12,              // 001000000000000
        NotContentIndexed = 1 << 13,    // 010000000000000
        Encrypted = 1 << 14,            // 100000000000000

        // 2、避免最后是 Maximum 这样的值。
        Maximum = 100


    }

    class Class2
    {
        // 关于使用
        // 1、书上的例子在 .NetCore 版本中已经没有了
        // 2、核心优势在于：可以复合多个值，通过 位操作符 表示出来，然后和获取的值比较。
        private void F1()
        {
            FileAttr attr = FileAttr.Hidden | FileAttr.ReadOnly & FileAttr.Normal;
            Console.WriteLine(attr);
        }
    }
}