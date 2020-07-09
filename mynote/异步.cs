using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

// 异步方法是 C#异步编程 实现之一，要求：C#5.0+ NET4.5+
// 核心技术主要就是 async、await 关键字声明，还有 Task
// 异步方法，异步调用其他方法而不阻塞主线程
// 异步方法的返回类型必须为 void、Task、Task<TResult> 中的其中一种。
namespace 异步方法
{
    // 启动
    class Start
    {
        static public void Launch()
        {
            // ReturnVoid.Do();
            // ReturnTask.Do();
            // ReturnTasks.Do();
            // UseTasks.Do();
            UseCancel.Do();
        }



        // 用来显示各个参数
        public static void ShowInfo(string name)
        {
            Thread thread = Thread.CurrentThread;
            System.Console.WriteLine("+++++++++++++++++++++++++");
            System.Console.WriteLine($"信息：{name} - 线程ID：{thread.ManagedThreadId }");
            System.Console.WriteLine($"信息：{name} - 后台线程：{thread.IsBackground }");
            System.Console.WriteLine($"信息：{name} - 托管线程池：{thread.IsThreadPoolThread }");
            System.Console.WriteLine("+++++++++++++++++++++++++");
        }

        // 显示Task完成情况
        public static void ShowResult(string name, Task task)
        {
            // https://docs.microsoft.com/zh-cn/dotnet/api/system.threading.tasks.taskstatus?view=netcore-3.1
            System.Console.WriteLine($"信息：{name} - 任务状态数：{ task.Status }");
        }
    }

    // void，表示无返回值，不关心异步方法执行后的结果，一般用于仅仅执行某一项任务，但是不关心结果的场景。
    class ReturnVoid
    {
        static public void Do()
        {
            Start.ShowInfo("主线程，调用函数");
            System.Console.WriteLine("主线程：启动异步方法之前");
            AsyncFunction();
            System.Console.WriteLine("主线程：启动异步方法之后");
        }

        private static async void AsyncFunction()
        {
            Start.ShowInfo("被调用函数，外部");

            // 会把该线程加入线程池作为后台线程运行
            // 仅仅是 await 语句被异步执行，其余正常顺序执行
            await Task.Run(() =>
            {
                Start.ShowInfo("异步函数");
                Console.WriteLine("异步方法开始：");
                // 模拟异步：暂停5秒
                Thread.Sleep(5000);
                Console.WriteLine("异步方法结束！");
            });
        }
    }

    // Task，表示异步方法将返回一个 Task 对象，该对象通常用于判断异步任务是否已经完成，可以使用 taskObj.Wait() 方法等待，或者 taskObj.IsCompleted 判断。
    class ReturnTask
    {
        public static void Do()
        {
            Start.ShowInfo("调用函数");
            // 虽是赋值，但已经开始执行
            Task task = AsyncFunction();

            // Wait() 会阻塞主线程
            // task.Wait();

            // task 没有基于牛逼的回调机制
            while (!task.IsCompleted)
            {
                // 间隔去检查
                Thread.Sleep(1000);
                System.Console.WriteLine(task.Status);
            }

            System.Console.WriteLine("后续语句");
        }

        private static async Task AsyncFunction()
        {
            await Task.Run(() =>
            {
                Start.ShowInfo("异步方法");
                System.Console.WriteLine(Task.CurrentId);
                Console.WriteLine("异步方法开始：");
                Thread.Sleep(5000);
                Console.WriteLine("异步方法结束！");
            });

            // 不需要 return，比较特殊的机制，自动完成
        }
    }

    // Task<TResult>，表示异步方法将返回一个 Task<TResult> 对象，该对象的 Result 属性则是异步方法的执行结果，调用该属性时将阻塞当前线程（异步方法未执行完成时）。
    class ReturnTasks
    {
        public static void Do()
        {
            Start.ShowInfo("调用函数");
            Task<string> task = AsyncFunction();
            System.Console.WriteLine("检测阻塞1");

            // task.Result 会阻塞当前进程
            System.Console.WriteLine(task.Result);
            System.Console.WriteLine("检测阻塞2");
        }


        private static async Task<string> AsyncFunction()
        {
            await Task.Run(() =>
            {
                Start.ShowInfo("异步方法");
                System.Console.WriteLine(Task.CurrentId);
                Console.WriteLine("异步方法开始：");
                Thread.Sleep(5000);
                Console.WriteLine("异步方法结束！");
            });

            Start.ShowInfo("调用函数，底部");

            // 返回的是字符串
            return "异步函数调用完毕！";
        }
    }

    // 使用多个 Task
    class UseTasks
    {
        public static void Do()
        {
            Task tasks = AsyncFn();
        }

        private static async Task AsyncFn()
        {
            Start.ShowInfo("await外部");

            await Task.Run(() =>
            {
                Start.ShowInfo("await1");
                Thread.Sleep(1000);
            });

            await Task.Run(() =>
            {
                Start.ShowInfo("await2");
                Thread.Sleep(2000);
            });

            await Task.Run(() =>
            {
                Start.ShowInfo("await3");
                Thread.Sleep(1000);
            });


        }
    }

    //  取消标记
    // 功能需求：满足一定要求就要取消
    class UseCancel
    {
        public static void Do()
        {

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                Task task = AsyncFn(i);
                tasks.Add(task);
                Thread.Sleep(100);
                Start.ShowResult($"{i}位置", task);
            }
        }

        private static async Task AsyncFn(int i)
        {
            // 核心要点：
            // 在任务开始之前，都要检查 cancellationToken.IsCancellationRequested 是否为 True
            // 若为 True 则取消执行Task
            // source.Cancel() 的目的 就是：设置为 True。
            CancellationTokenSource source = new CancellationTokenSource();
            // Tips：这是一个小指向，Token就是一个标记，记号
            CancellationToken token = source.Token;

            // 先判断，然后决定是否需要取消task【注意先后】
            if (i == 4 || i == 5)
            {
                source.Cancel();
                System.Console.WriteLine($"取消：{i}位置：{token.IsCancellationRequested}");
            }

            await Task.Run(() =>
            {
                System.Console.WriteLine($"任务：{i}位置：执行任务！");
            }, token);
        }
    }

    class MoreHard
    {
        public static void Do()
        {
            CancellationTokenSource source1 = new CancellationTokenSource();
            CancellationTokenSource source2 = new CancellationTokenSource();
            CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, source2.Token);
        }

        private static async Task<int> AsyncFn(CancellationToken token)
        {
            int count = 0;
            int now = await Task.Run(() =>
            {
                return count + 1;
            }, token).ContinueWith((task) =>
            {
                return count + 1;
            }, token);
            return now;
        }
    }
}