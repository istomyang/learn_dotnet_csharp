#define CSHARP7

using System;
using static System.Console;
using System.Collections.Generic; // 泛型专用命名空间
using 辅助代码.Generic;
using 辅助代码.Class;


namespace MyNote.Generic.概述
{
    public static class Class1
    {
        public static void Fn1()
        {
            Stack<string> numbers = new Stack<string>();
            numbers.Push("one");
            numbers.Push("two");

            WriteLine(numbers.Pop().ToString());

            /**
            * 泛型的优点（见资料：7.0本质论 12.2.3）
            * 【泛型是一个模板，可以用来约束很多东西。】
            * 还可以使用 值类型。
            */
        }
    }

    // 泛型结构和结构
    interface IOne<T>
    {
        T First { get; set; }
    }
    public struct One<T> : IOne<T>
    {
        public T First { get; set; }
    }

    // 类实现接口的多个（避免这样）
    class Address { }
    public class Phone { }
    public interface IContainer<T>
    {
        ICollection<T> Items { get; set; }
    }
    public class Person : IContainer<Address>, IContainer<Phone>
    {
        ICollection<Address> IContainer<Address>.Items { get; set; }

        public ICollection<Phone> Items { get; set; } /*隐式*/
        // ICollection<Phone> IContainer<Phone>.Items { get; set; } /*显式*/
    }

    // 构造函数
    struct Struct1<T> : IOne<T>
    {
        public T First { get; set; }
        public Struct1(T first)
        {
            First = first;
        }
    }



    // 默认值
    struct Struct2<T> : IOne<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public Struct2(T second)
        {
#if CSHARP7
            /**
            * C# 7.0+ default 可以类型推断。
            * 包括函数也是，return default 返回默认值
            */
            First = default;
#else 
            First = default(T);
#endif
            Second = second;
        }
    }

    // 多个 类型参数
    interface IPair<TFirst, TSecond> /*定义接口*/
    {
        TFirst First { get; set; }
        TSecond Second { get; set; }
    }
    interface IPair<TFirst, TSecond, TThird> /* 不同类型参数 同 类/结构 名可共存 */
    {
        TFirst First { get; set; }
        TSecond Second { get; set; }
        TThird Third { get; set; }
    }
    class Class2<TFirst, TSecond, TThird> : IPair<TFirst, TSecond> // 类型参数必须相同
    {
        /**
        * 泛型接口，接口的模板。
        * 继承的话，继承的模板必须拥有被继承的模板的接口
        */
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
        public TThird Third { get; set; }
        public Class2(TFirst first, TSecond second, TThird third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }
    class Class3 /*使用*/
    {
        private void Fn1()
        {
            Class2<int, string, bool> pair1 = new Class2<int, string, bool>(10, "Hello", false); /* new后三个参数不能去掉 */
        }
    }

    /*元组是最大的泛型，查阅 ValueTuple*/

    // 嵌套泛型类型
    class Class4<T, U>
    {
        public T First { get; set; }

        private class Class5<X> /* Class5<U> 会覆盖 包容类 的 U */
        {
            public T Second { get; set; }
            public X Third { get; set; }
        }
    }
}


namespace MyNote.Generic.约束
{
    interface IPair<T> /*数对接口*/
    {
        T First { get; set; }
        T Second { get; set; }
    }

    class Pair<T> : IPair<T> /*数对类：实现接口*/
    {
        public T First { get; set; }
        public T Second { get; set; }
        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }
    }

    // 接口约束
    class Class1<T>
        where T : System.IComparable<T> /* 参数类型：接口/类 */
    {
        public T Item { get; set; }
        public Pair<Class1<T>> Item2 /* Pair是一个数对结构，Class1<T>是数对元素的类型，这个类型里面有个 T，这个 T 满足 IComparable<T> 接口 */
        {
            get { return _Item2; }
            set
            {
                IComparable<T> first = value.First.Item; /* 可以使用 */
            }
        }

        private Pair<Class1<T>> _Item2;
    }

    // 类类型约束
    class EntityBase
    {
        public int Number { get; set; }
        public EntityBase(int numebr)
        {
            Number = numebr;
        }
    }

    class EntityDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
        where TValue : EntityBase /*类类约束第一个，满足其他类的限制*/
    {
        public void Fn1()
        {
            EntityDictionary<int, EntityBase> my = new EntityDictionary<int, EntityBase>();
            // EntityDictionary<int, string> my2 = new EntityDictionary<int, EntityBase>(); /*报错*/
        }
    }

    // 限制引用类型和值类型
    class SomeClass1<T> where T : class { } /*类、接口、委托、数组类型可以*/
    class SomeClass2<T> where T : struct { } /*可空值类型不符合条件*/

    // 多个约束
    class EntityDictionary2<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : IComparable, IFormattable /* 相当于 AND 关系；末尾无逗号 */
        where TValue : EntityBase
    { }

    // 构造函数约束
    public class EntityBase2<TKey>
    {
        public TKey Key { get; set; }
    }

    public class EntityDictionary3<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : IComparable<TKey>, IFormattable
        where TValue : EntityBase2<TKey>, new() /*要有默认构造函数，不能用带参数的。若带参数，传递工厂函数进去*/
    {
        public TValue MakeValue(TKey key)
        {
            TValue newEntity = new TValue();
            newEntity.Key = key;
            Add(newEntity.Key, newEntity);
            return newEntity;
        }
    }

    // 约束的继承
    class EntityBase3<T> where T : IComparable<T> { } /*约束参数和约束不会被继承，类继承的本质在于：派生拥有基类的成员。*/
    class Entity<U> : EntityBase3<U> where U : IComparable<U>, IEnumerable<U> { } /*显式，具有相同或更大的约束*/

    // 泛型方法继承不需要声明
    class SomeClass3 { public virtual void Method<T>(T t) where T : IComparable<T> { } }
    class SomeClass4 : SomeClass3 { public override void Method<T>(T t) { } } /*不允许额外的约束*/

}

namespace MyNote.Generic.泛型方法
{
    public static class MathEx
    {
        public static T Max<T>(T first, params T[] values)
            where T : IComparable<T> /*约束*/
        {
            T maxium = first;
            foreach (T item in values)
            {
                if (item.CompareTo(maxium) > 0)
                {
                    maxium = item;
                }
            }
            return maxium;
        }

        public static void Use()
        {
            WriteLine(MathEx.Max<int>(1, 23, 45, 6));
            WriteLine(MathEx.Max(1, 23, 45, 6)); /*类型推断*/
        }

        // 注意点
        class Class1<T> where T : IComparable<T> { }
        private static void Fn1<T>(Class1<T> one) where T : IComparable<T> { } /*使用其他泛型，需要同步约束*/

    }

}

namespace MyNote.Generic.协变性和逆变性
{

    /**
    * 协变是 X --> Y
    * 逆变是 I<X> --> I<Y>
    */

    #region  协变
    public static class Class1
    {

        public static void Fn1()
        {
            // OK
            Object obj = (Object)new String("nihc");
            String str = new String("asaaa");
            WriteLine(obj); /*nihc*/
            WriteLine(str); /*asaaa*/

            // Error
            // Pair<Object> obj = (Pair<Object>)new String("toObj");
            // Pair<String> str = new Pair<Object>(new Object(), new Object());

            // Error
            Contact contact1 = new Contact("c1", 1);
            Contact contact2 = new Contact("c1", 2);
            Pair<Contact> contact = new Pair<Contact>(contact1, contact2);
            // Pair<PdaItem> pdaPair = (Pair<PdaItem>)contact; 
        }

    }

    #region 理论的协变性辅助代码
    interface IReadOnly<T>
    {
        T First { get; }
        T Second { get; }
    }

    interface ICanSet<T>
    {
        T First { get; set; }
        T Second { get; set; }

    }

    interface IReadOnlyOut<out T>
    {
        T First { get; }
        T Second { get; }
    }

    #endregion

    public struct Pair<T> : IReadOnly<T>, ICanSet<T>, IReadOnlyOut<T>
    {
        public T First { get; set; }
        public T Second { get; set; }
        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

    }

    public static class Class2
    {
        public static void Fn2()
        {
            Contact c1 = new Contact("n1", 1);
            Contact c2 = new Contact("n2", 2);

            Pair<Contact> pc1 = new Pair<Contact>(c1, c2);

            // IReadOnly<PdaItem> read = (IReadOnly<PdaItem>)pc1; /*理论对，需要*/
            IReadOnlyOut<PdaItem> readOut1 = (IReadOnlyOut<PdaItem>)pc1; /*显式*/
            IReadOnlyOut<PdaItem> readOut2 = pc1; /*隐式*/
        }
    }


    #region 自己研究的

    public static class Class3
    {
        public static void Fn3()
        {
            Wrapper<Base1> b1 = new Wrapper<Base1>(new Base1());
            WriteLine(b1.Member1.Common); /*Member1是接口成员，成员是Base1类型，啥也没有。*/

            Wrapper<Derived1> d1 = new Wrapper<Derived1>(new Derived1());
            WriteLine(d1.Member1.Common);
            WriteLine(d1.Member1.OnlyDerivedHas); /*成员是Derived类型，有一个OnlyDerivedHas*/

            // Wrapper<Base1> b2 = d1; /*如果转型，一阶成员安全转型（派生类转基类），二阶成员无法转型，尤其是写入的时候，类型不安全，这就是问题的核心。*/

            IReadOnlyOut2<Base1> b3 = d1;
            // b3.Member1 = new Base1(); /*出错*/

            /**
            * 本质：
            * 类型的成员可以转变过来，T 从 A 变成 B，没问题。
            * 但是如果代码有使用 A.C，而 B 里无 C，则就问题，尤其是其他地方使用 A.C 的时候。
            * 解决办法就是，使用接口，这个接口表明，封锁那个成员，你不能往一个没有这个属性的对象上赋值（而派生类有）
            */
        }
    }

    interface I1<T>
    {
        T Member1 { get; set; } 
    }

    interface IReadOnlyOut2<out T>
    {
        T Member1 { get; }
    }

    public class Wrapper<T> : I1<T>, IReadOnlyOut2<T>
    {
        public T Member1 { get; set; }
        public Wrapper(T member1)
        {
            Member1 = member1;
        }
    }

    public class Base1
    {
        public int Common { get; set; } = 2;
    }
    public class Derived1 : Base1
    {
        public int OnlyDerivedHas { get; set; } = 2;
    }

    #endregion
    #endregion

    #region 逆变

    class Fruit { }
    class Apple : Fruit { }
    class Orange : Fruit { }
    interface IComparableThings<in T>
    {
        bool FirstIsBetter(T first, T second);
    }
    class FruitComparer : IComparableThings<Fruit>
    {
        public bool FirstIsBetter(Fruit f1, Fruit f2) { return true; }
    }
    public class Class4
    {
        public void Main()
        {
            IComparableThings<Fruit> fc = new FruitComparer();
            Apple apple1 = new Apple();
            Apple apple2 = new Apple();
            Orange orange = new Orange();

            bool b1 = fc.FirstIsBetter(apple1, orange);
            bool b2 = fc.FirstIsBetter(apple1, apple2);

            IComparableThings<Apple> ac = fc;
            bool b3 = ac.FirstIsBetter(apple1, apple2);
            // bool b4 = ac.FirstIsBetter(apple1, orange);

            IComparableThings<Fruit> fc1 = (IComparableThings<Fruit>)ac;
        }
    }
    #endregion

}


















