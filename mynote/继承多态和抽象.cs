using System;

namespace Inherit
{
    // 1、继承（Inheritance）用来扩展类之间 “Is-a”的关系，比如：蚊子 is a 昆虫。
    // 2、基类（Base Class）：也是通用类，派生类（Derived Class）：也是特殊化类。

    // 3、基类
    class Rectangle
    {
        private int _length;
        private int _width;

        public Rectangle()
        {
            _length = 10;
            _width = 10;
        }

        public Rectangle(int lenght, int width)
        {
            _length = lenght;
            _width = width;
        }

        public string Length { set;get; }
        public string Width { set;get; }
    }

    // 4、派生类
    // 5、使用 ：表示派生
    // 6、派生类继承基类所有的成员，除了构造函数（想想构造函数的用途就知道原因了）；
    // 7、虽然继承了基类的私有成员，但只有基类的方法才能去访问。
    class Box:Rectangle
    {
        private int _height;

        public Box()
        {
            _height = 10;
        }

        public Box(int height)
        {
            _height = height;
        }

        // 使用 base关键字 继承Base类的构造函数，这样才能真正继承到Base类的公共特性。
        // base 后面是 基类 的形参列表
        // 因为先执行基类，所以基类参数放前面；
        public Box(int length,int width,int height):base(length,width)
        {
            _height = height;
        }

        public string Height{ set; get; }
    }

    class Use
    {
        // 8、创建派生类实例，首先执行基类的构造函数，然后再执行派生类的构造函数
        // 9、如果基类和派生类不带参数，那都默认不带参数。
        Box one = new Box();

        // 10、如果不继承Base的构造函数，默认使用不带形参的构造函数，所以将无法设置Base类的参数。
        Box Two = new Box(10);

        // 11、带有继承 Base 的构造函数Use 
        Box three = new Box(10, 10, 10);

        // 12、如果基类没有不带形参的构造函数，则再派生类中必须使用 base 继承。
    }
}

namespace Override
{
    // 1、多态（polymorphism）：派生类能够覆写基类的方法。
    class People
    {
        private string _name;

        // 6、可以在 属性 上使用多态。
        public virtual string Name { get; set; }

        public People(string name)
        {
            _name = name;
        }

        // 2、virtual关键字：表示派生类可以覆盖此方法。
        public virtual void SayHello()
        {
            System.Console.WriteLine($"{_name}, Hello!, Im {_name}");
        }
    }

    class Student:People
    {
        private int _age;

        public int Age { get; set; }

        // 7、覆盖基类的属性
        public override string Name { get; set; } = "Student";

        // 3、可以使用 实参 为基类传值
        public Student(int age):base("XianMing")
        {
            _age = age;
        }

        // 4、Override关键字：表示会覆盖基类中的方法
        // 5、覆盖的方法只能跟基类一样的参数。
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
}

namespace Abstract
{
    // 1、抽象类（Abstract class）：不打算实例化，仅仅作为基类。
    // 2、使用 abstract关键词 声明抽象类
    // 3、抽象类与具体类区别在于：前者不能被实例化，哪怕new，也不会编译。
    abstract class Plane
    {
        // 3、抽象方法目的就是：希望在派生类中去写。
        // 4、抽象方法没有主体。
        public abstract void DoSomething();

        // 5、抽象属性也是如此。
        public abstract string Name { get; set; }

        // 6、使用 override 去覆写
    }

}