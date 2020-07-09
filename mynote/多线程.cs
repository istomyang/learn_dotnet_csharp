using System;
using System.Threading;
using static System.Console;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyNote.ThreadHandle
{
    /// <summary>
    /// This code you would get the sense of how uncertain about muti-threading.
    /// </summary>
    public class C1
    {
        public static void R1()
        {
            ThreadStart fn = DoWork;
            Thread thread2 = new Thread(fn);
            thread2.Start();

            // This is Main Threading
            for (int i = 0; i < 1000; i++)
            {
                Write("-");
            }

            thread2.Join(); // main threading is waitting until thread2 is finished. 
        }

        public static void DoWork()
        {
            // This is Working Threading
            for (int i = 0; i < 1000; i++)
            {
                Write("+");
            }
        }
    }

    /// <summary>
    /// Use a thread pool.
    /// </summary>
    public class C2
    {
        public static void R1()
        {
            ThreadPool.QueueUserWorkItem(DoWork, "+");

            for (int i = 0; i < 1000; i++)
            {
                Write("-");
            }

            Thread.Sleep(1000);
        }

        public static void DoWork(object state)
        {
            for (int i = 0; i < 1000; i++)
            {
                Write(state);
            }
        }
    }

    /// <summary>
    /// Task
    /// </summary>
    public class C3
    {
        public static void R1()
        {
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < 500; i++)
                {
                    Write("-");
                }
            });

            for (int i = 0; i < 500; i++)
            {
                Write("+");
            }

            task.Wait(); // Like Thread.Join()
        }

        public static void R2()
        {
            WriteLine("Before");
            Task t1 = Task.Run(() => WriteLine("Starting")).ContinueWith((task) => WriteLine("t1 is running"));
            Task t2 = t1.ContinueWith((task) => WriteLine("Start t2"));
            Task t3 = t1.ContinueWith((task) => WriteLine("Start t3"));
            Task.WaitAll(t2, t3);

        }

        public static void R3()
        {
            Task t = Task.Run(() => WriteLine("task is running!"));

            Task faultedTask = t.ContinueWith((task) =>
            {
                Trace.Assert(task.IsFaulted); // Listen task's status
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task cancelTask = t.ContinueWith((task) =>
            {
                Trace.Assert(task.IsFaulted); // Listen task's status
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task completeTask = t.ContinueWith((task) =>
            {
                Trace.Assert(task.IsFaulted); // Listen task's status
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completeTask.Wait();
        }

        public static void R4()
        {
            Task t = Task.Run(() =>
                        {
                            throw new InvalidOperationException();
                        });

            try
            {
                t.Wait();
            }
            catch (AggregateException e)
            {
                e.Handle(eachE =>
                {
                    WriteLine($"Error: { eachE.Message }");

                    return true;
                });
            }
        }

        public static Stopwatch clock = new Stopwatch();
        public static void R5()
        {
            try
            {
                clock.Start();
                AppDomain.CurrentDomain.UnhandledException +=
                (s, e) =>
                {
                    Message("Event handler starting");
                    Delay(4000);
                };

                Thread thread = new Thread(() =>
                {
                    Message("Throwing Exception");
                    throw new Exception();
                });

                thread.Start();
                Delay(2000);
            }
            finally
            {

                Message("Finnaly Block is running");
            }
        }

        static void Delay(int i)
        {
            WriteLine($"Sleeping for {i} ms");
            Thread.Sleep(i);
            WriteLine("Awake");
        }
        static void Message(string text)
        {
            WriteLine("{0}:{1:0000}:{2}", Thread.CurrentThread.ManagedThreadId, clock.ElapsedMilliseconds, text);
        }
    }


    public class C4
    {
        public static void R1()
        {
            WriteLine("Push Enter to Exit.");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task task = Task.Run(() =>
            {
                WriteInfinity(cancellationTokenSource.Token);
            },cancellationTokenSource.Token);

            ReadLine();
            cancellationTokenSource.Cancel();
            WriteLine("*".PadRight(WindowWidth - 1), "*");
            task.Wait();
        }

        static void WriteInfinity(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                for (Int64 i = 0; i < Int64.MaxValue; i++)
                {
                    Write("_");
                    Write(i);
                    Write("_");
                }
            }
        }
    }



}