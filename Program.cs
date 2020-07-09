using System;
// using my = MyNote.Event.My;
// using my =  MyNote.Attribute;
using my = MyNote.ThreadHandle;

namespace CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // my.R2.F();

            // my.C3.R5();
            my.C4.R1();


            // 防止CLI窗口关闭
            Console.WriteLine("Press Enter to quit!");
            Console.ReadLine();
            return;
        }
    }
}
