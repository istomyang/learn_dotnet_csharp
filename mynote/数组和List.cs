using System;
using System.IO;
using System.Collections.Generic;

// C#编程指南-数组：https://docs.microsoft.com/zh-cn/dotnet/api/system.array?view=netcore-3.1
// dotNET.System.Array：https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/arrays/

// * C# 和 dotNet 所有数据类型两类：值类型（Value Type） 和 引用类型（Reference Type）
// * 数组是引用类型
namespace NamespaceArrayAndList
{
    class ClassArray
    {
        static private void Start()
        {
            // 创建和使用引用类型的变量有两个步骤：
            // 1、声明引用变量
            // 2、创建对象并与引用变量关联

            // 所以，创建引用类型的Array一般方法
            // 在初始化不赋值的时候，int类型默认值是0，而对象数组是 null，string 也是 null；
            // 合并为： int[] numArray = new int[10];
            int[] numArray;
            numArray = new int[10];

            // 可以为数组重新制定
            // 旧的数组在内存中为空对象，会被 垃圾回收（Garbage Collection）
            numArray = new int[2];

            // 但，建议这种方法，便于统一操作
            const int SIZE = 6;
            int[] numArray1 = new int[SIZE];

            // 为数组各元素赋值
            // 读作： numArray sub Zero
            numArray[1] = 1;

            // 读取和使用数组元素
            // 必须转换成字符串
            Console.WriteLine($"{ numArray[1].ToString() }");

            // 多维(rank)数组（n-1 个逗号）
            int[,,] multiArray = new int[2, 2, 2];
        }

        // 数组的初始化：初始化列表（Initialization List）
        static private void Init()
        {
            // 方法 1
            int[] numArray1 = new int[2] { 1, 2 };

            // 方法 2：不带大小声明符，系统自动判断
            int[] numArray2 = new int[] { 1, 2 };

            // ★ 方法 3 ：省略 new
            int[] numArray3 = { 1, 2 };

            // 方法 4：后期提供初始值，但必须之前指定大小，但大小不一定为常量，可以是变量
            int size = 2;
            int[] numArray4 = new int[size];
            numArray4 = new int[] { 1, 2 };

            // 多维数组
            int[,] multiArray = {
                {1,2},
                {1,2}
            };

            // 交错数组(jagged array)
            // 1、注意 [][]
            // 2、必须要 new 【因为 new 返回的是引用地址，大小是一致的，至于 [][], 应该是处理细节问题标识符。】
            int[][] jagged = {
                new int[] {1,2,3},
                new int[]{1,2},
                new int[]{1}
            };
        }

        // 使用数组
        static private void Value()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6 };
            int[,] multi = {
                {1,2},
                {1,2}
            };
            int[][] jagged = {
                new int[] {1,2,3},
                new int[]{1,2},
                new int[]{1}
            };

            // 一维数组：使用：从0下标开始
            int number1 = numbers[0];

            // 一维数组：交叉修改值
            numbers[1] = numbers[3];

            // 一维数组：修改值
            numbers[3] = number1;

            // 多元数组
            multi[1, 1] = 3;

            // 交叉数组
            jagged[1][1] = 1;
        }

        // 数组属性
        static private void Prop()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6 };

            // 长度
            // -- 托管语言 CLR 会检查数组是否越界，而 C++ 作为非托管语言，会导致越界，产生缓冲区溢出。
            Console.WriteLine($"numbers 数组的长度：{ numbers.Length }");
        }

        // 数组方法
        static private void Function()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6 };

            // Array.Sort 方法：https://docs.microsoft.com/zh-cn/dotnet/api/system.array.sort?view=netcore-3.1
        }
    }

    class ClassArrayUsage
    {
        static public void Start()
        {
            Console.WriteLine("list用法类开始：");
            File();

            Console.WriteLine("list用法类结束。");
        }


        static private void File()
        {
            // 将数组写入文件
            // 1、使用 using，因为牵涉读写
            // 2、将数组写入文件
            // 3、读取就是 System.IO.File.OpenText
            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            using (StreamWriter fileWriter = System.IO.File.CreateText(@".\data\MyTest.txt"))
            {
                foreach (var item in numbers)
                {
                    fileWriter.WriteLine(item);
                }
            }
        }

        static private void Copy()
        {
            // 数组的深度拷贝
            int[] numbers1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] numbers2 = new int[numbers1.Length];
            for (int i = 0; i < numbers1.Length; i++)
            {
                numbers2[i] = numbers1[i];
            }
        }

        static private void Equal()
        {
            // 比较数组
            // 不同使用 == 运算符，因为数组是引用，== 只会比较引用里的地址，而非内容
            // 所以使用一套原始的算法（考虑性能的）：
            // 1、先比较 length 是否一样
            // 2、如果严格模式只能一个一个比较内容了
            int[] numbers1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] numbers2 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            if (IsEqualArray(numbers1, numbers2))
            {
                Console.WriteLine("相等！");
            }
            else
            {
                Console.WriteLine("不相等！");
            }

            static bool IsEqualArray(int[] array1, int[] array2)
            {
                bool isEqual = true;
                int index = 0;

                if (array1.Length != array2.Length)
                {
                    isEqual = false;
                    return isEqual;
                }

                while (isEqual && index < array1.Length)
                {
                    if (array1.Length != array2.Length)
                    {
                        isEqual = false;
                        return isEqual;
                    }

                    index++;
                }

                return false;
            }

        }

    }

    // List 与数组不同在于：自动调整大小。（链表结构）
    // 其性能同链表结构
    // 需要：using System.Collections.Generic;
    class ClassList
    {
        static private void Start()
        {
            // 声明
            List<int> intNumber = new List<int>() { 1, 2, 3, 4, 5 };

            // 添加
            intNumber.Add(10);

            // 索引从0开始
            System.Console.WriteLine(intNumber[1]);

            // 长度
            System.Console.WriteLine(intNumber.Count);

            // 删除 (按位置)
            intNumber.RemoveAt(1);

            // 删除（按值）
            // remove() 返回：bool，代表删除成功与否
            intNumber.Remove(4);

            // 清空
            intNumber.Clear();

            // 插入
            // (值，位置)
            intNumber.Insert(111, 1);

            // 寻找值并返回索引
            int postion = intNumber.IndexOf(111);
        }
    }

}