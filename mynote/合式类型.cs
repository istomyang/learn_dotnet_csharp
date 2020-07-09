using System;
using System.Collections.Generic;

namespace Name
{
    // 重写 Object成员（虚方法）
    class Class1 : IEquatable<Class1>
    {
        // 重写 ToString：默认返回类完全限定名称
        // 1、如果string，返回字符串，联系人类返回联系人姓名，坐标类返回坐标……
        // 2、Console.WriteLine 和 System.Diagnostics.Trace.WriteLine 会默认调用对象 ToString 方法，所以非常有意义
        // 3、对日志文件相当有用，如果是诊断信息，则长度控制不要一行，简短。
        // 4、避免ToString引发异常或改变对象状态
        // 5、如果考虑格式化，就重载ToString，或实现IFormattable
        // 6、考虑返回独一无二的字符串标识对象
        public override string ToString() { return $"我可以返回任何数据！"; }


        // 重写 GetHashCode：
        // 1、重写 Equals 就要重写 GetHashCode；作为哈希表集合的键使用，也是要的重写的（System.Collections.Hashtable 和 System.Collections.Generic.Dictionary）。
        // 2、哈希码作用：生成和对象值对应的数字，从而高效平衡哈希表，所以有以下原则：（见附件：哈希码实现原则）
        // 我的理解：
        // 1、哈希码类似于身份证，是判断是一个对象身份用的，但是身份有 id 有 class，有下面几个要求。
        // 2、相等的对象有相等的哈希码： a.GetHashCode() == b.GetHashCode()
        // 3、哪怕里面的数据改变，哈希码不要变。想想人，换了个衣服身份变了吗？但换了脑子就不一样了，所以要选择好
        // 4、性能的问题：这个方法不能引发异常，不然 Equals 可难受了，而且频繁调用，甚至 Equals方法比较的时候，用哈希码省力。
        // 5、哈希姆尽可能唯一，这是针对id来说的，但是id最多只有 32位（int），如果是 long，那不要想直接用值作为哈希码了。
        // 6、大小就是32位，尽可能地分散开来。
        // 7、在核心部件上，1bit的差异，最好用16bit体现出来，防止哪怕哈希表对哈希值“装桶（bucketing），也能保持平衡
        // 8、这个哈希码要难以伪造，不然就GG了。
        // 9、要求过高，难度过大，所以平衡考虑。

        long value = 9999999999999;
        public override int GetHashCode()
        {
            // 如果是数值，一般方案是 XOR 操作符，并保证操作数不相近不相等。如果，则考虑移位
            return ((int)value ^ (int)(value >> 32));

            // 多次引用 AND 会出现全为0，多次 OR 会全为1
            // 如果基类不是 Object，则XOR赋值包括 base.GetHashCode()
            // 如果计算的值可能改变，或者缓存能提升性能，则缓存。
        }

        // 重写 Equals：
        // 对象同一性 和 相等的对象值
        // 1、同一个引用相等，通过  ReferenceEquals() 检查，但仅是一例。
        // 2、用相同数据构造的对象 虽然引用不等，但他们关键数据相同
        // 3、值类型使用 ReferenceEquals 需要装箱，所以必然不等。
        int key1 = 1;
        int key2 = 2;
        public bool Equals(Class1 obj) { return true; }
        public override bool Equals(object obj)
        {
            // 两个对象相等，其关键数据必须相等。
            // 重写 Equals步骤一般如下：

            // 1、检查是否为null
            if (obj == null) { return false; }

            // 2、如果引用类型，检查是否相等 ReferenceEquals()
            if (ReferenceEquals(this, obj)) { return true; }

            // 3、检查数据类型是否相同
            if (this.GetType() != obj.GetType()) { return false; }

            // 4、各自调用一个有返回的方法，看看是否相等
            // 5、检查成员的哈希码
            if (this.GetHashCode() != obj.GetHashCode()) { return false; }

            // 6、如果基类重写Equals，则检查 base.Equals()
            System.Diagnostics.Debug.Assert(base.GetType() != typeof(object));
            if (!base.Equals(obj)) { return false; }

            // 7、比较关键字段，看是否相等
            // return (key1.Equals(obj.key1) && key2.Equals(obj.key2));

            // 8、重写 GetHashCode
            // 9、重写 == 和 != 操作符

            return false;
        }
    }

    // 使用元组重写 Equals 和 GetHashCode（需要IEquatable实现）
    class Class2
    {
        public int Number1 { get; set; } = 1;
        public int Number2 { get; set; } = 2;

        public Class2(int number1, int number2)
        {
            Number1 = number1;
            Number2 = number2;
        }

        // public bool Equals(Class2 obj) => (Number1, Number2).Equals(obj.Number1, obj.Number2);
        // 1、元组 System.ValueTuple<>内部使用 EqualityComparee<T>，依赖 IEquatable<T> 实现
        // 2、重写 Equals 遵循以下规范：
        // 2.1、实现 IEquatable

        public override int GetHashCode() => (Number1, Number2).GetHashCode();
    }

    // 操作符重载：除非故意让类型表现的跟基元类型一样，否则不要重载
    class Class3
    {
        public override bool Equals(object obj) { return true; }
        public override int GetHashCode() { return 1; }

        // 比较操作符
        // == 默认引用相等性检查
        // == 和 != 成对实现
        public static bool operator ==(Class3 one, Class3 two)
        {
            if (ReferenceEquals(one, null)) // 执行 == 会导致无限递归！！！
            {
                // 有点绕人，但逻辑是对的，想想看，到这儿说明是 one 是null，而two是null就是 true，否则就是 false
                return ReferenceEquals(two, null);
            }
            return (one.Equals(two));
        }
        public static bool operator !=(Class3 one, Class3 two)
        {
            return !(one == two);
        }

        // 二元操作符
        // 跳过
    }
}


// XML注释：提供API文档，调用提示
namespace AddisonWesley.Michaelis.EssentialCSharp.Chapter10.Listing10_17
{
    /// <summary>
    /// DataStorage is used to persist and retrieve
    /// employee data from the files.
    /// </summary>
    class DataStorage
    {
        /// <summary>
        /// Save an employee object to a file
        /// named with the Employee name.
        /// </summary>
        /// <remarks>
        /// This method uses <seealso cref="System.IO.FileStream"/>
        /// in addition to
        /// <seealso cref="System.IO.StreamWriter"/>
        /// </remarks>
        /// <param name="employee">
        /// The employee to persist to a file</param>
        /// <date>January 1, 2000</date>
        public static void Store(Employee employee)
        {
            // ...
        }

        /** <summary>
         * Loads up an employee object.
         * </summary>
         * <remarks>
         * This method uses <seealso cref="System.IO.FileStream"/>
         * in addition to
         * <seealso cref="System.IO.StreamReader"/>
         * </remarks>
         * <param name="firstName">
         * The first name of the employee</param>
         * <param name="lastName">
         * The last name of the employee</param>
         * <returns>
         * The employee object corresponding to the names
         * </returns>
         * <date>January 1, 2000</date> **/
        public static Employee Load(string firstName, string lastName)
        {
            // ...
            return new Employee();
        }
    }

    class Program
    {
    }

    class Employee { }
}

namespace 垃圾回收
{
    // 垃圾回收的对象：引用（只回收引用对象，堆上的内存）、内存
    // 垃圾不回收：文件、数据库连接、句柄、网络端口

    class UseWeakReference
    {
        // 大部分引用为强引用，阻止垃圾回收机制
        // 弱引用，对象还是会被清理，但清理之前，可以复用，比如一些开销很大的资源
        // 声明弱引用：
        private WeakReference Data;

        // 可以把一个 弱引用 赋值给 强引用，目的是阻止在“检查Null”和“访问数据”间回收垃圾
    }
}

namespace 资源清理
{
    // 垃圾回收器负责内存管理
    // 终结器Finalizer 负责处理 文件、数据库这类资源清理，而这个，需要显式。

    class Finalizer
    {
        ~Finalizer()
        {
            Close();
        }

        private void Close() { }
    }
}

