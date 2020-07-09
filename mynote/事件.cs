using System;
using System.Collections.Generic;
using static System.Console;

namespace MyNote.Event.My
{

    #region use delegate

    class P1
    {
        private int _number;

        public int Numebr
        {
            get { return _number; }
            set
            {
                _number = value;
                giveEvent(value);
            }
        }

        public Action<int> giveEvent { get; set; }
    }

    class S1
    {
        public void f1(int numebr) { WriteLine("1subscriber"); }
        public void f2(int numebr) { WriteLine("2subscriber"); }
        public void f3(int numebr) { WriteLine("3subscriber"); }
    }

    class R1
    {
        public static void F1()
        {
            P1 p1 = new P1();
            S1 s1 = new S1();

            Action<int> a1 = s1.f1;
            Action<int> a2 = s1.f2;
            Action<int> a3 = s1.f3;

            p1.giveEvent += a1;
            p1.giveEvent += a2;
            p1.giveEvent += a3;
            p1.giveEvent += (int number) => { };

            // Q1: can invoke out of object.
            p1.giveEvent(20);

            // Q2: override delegate easily.
            // p1.giveEvent = a1;

            WriteLine("My Event, Input a number!");
            p1.Numebr = int.Parse(ReadLine());
        }
    }


    #endregion

    #region use Event to slove problems

    class P2
    {
        public class NumberArgs : System.EventArgs
        {
            public int NewNumber { get; set; }
            public NumberArgs(int newNumber)
            {
                NewNumber = newNumber;
            }
        }

        public event EventHandler<NumberArgs> GiveEvent = delegate { };

        # region My custom delegate
        public delegate void MyCustomEvent<T>(object sender, T e);
        public event MyCustomEvent<int> MyGiveEvent = delegate { };

        #endregion

        public int Numebr
        {
            get { return _number; }
            set
            {
                _number = value;
                GiveEvent(this, new NumberArgs(value));

                // my custom delegate
                MyGiveEvent(this, value);
            }
        }

        private int _number;
    }

    class S2
    {
        public void f1<TEventArgs>(object s, TEventArgs e) { WriteLine("1subscriber -- event"); }
        public void f2<TEventArgs>(object s, TEventArgs e) { WriteLine("2subscriber -- event"); }
        public void f3<TEventArgs>(object s, TEventArgs e) { WriteLine("3subscriber -- event"); }
    }

    class R2
    {
        public static void F()
        {
            P2 p = new P2();
            S2 s = new S2();

            p.GiveEvent += s.f1;
            p.GiveEvent += s.f2;
            p.GiveEvent += s.f3;

            p.Numebr = 10;
        }
    }

    #endregion

    #region use event keywords
    class Publisher
    {
        public class NumberChangedEvent : System.EventArgs
        {
            public int NewNumber { get; set; }

            public NumberChangedEvent(int newNumber)
            {
                NewNumber = newNumber;
            }
        }

        /// <summary>
        /// 不让从外部进行调用
        /// </summary>
        public event EventHandler<NumberChangedEvent> OnNumberChange = delegate { };

        private int _nowNumber = 10;
        public int NowNumber
        {
            get { return _nowNumber; }
            set
            {
                OnNumberChange?.Invoke(this, new NumberChangedEvent(value));
            }
        }
    }

    class Subscribler1
    {
        public void subsribler<TEventArgs>(object sender, TEventArgs e)
        {
            WriteLine($"1发布者的类型：{sender.GetType()} ---- 接收者的{e.GetType()}");
        }
    }

    class Subscribler2
    {
        public void subsribler<TEventArgs>(object sender, TEventArgs e)
        {
            WriteLine($"2发布者的类型：{sender.GetType()} ---- 接收者的{e.ToString()}");
        }
    }


    class Run
    {
        public static void F()
        {
            Publisher p = new Publisher();
            Subscribler1 s1 = new Subscribler1();
            Subscribler2 s2 = new Subscribler2();

            p.OnNumberChange += s1.subsribler;
            p.OnNumberChange += s2.subsribler;

            WriteLine("Input a numebr:");
            p.NowNumber = int.Parse(ReadLine());
        }
    }

    #endregion
}








