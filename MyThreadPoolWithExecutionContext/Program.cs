using System;
using System.Collections.Concurrent;
using System.Threading;

/*
 * ExecutionContext: State flows from one thread to another thread
 * or whatever continuation you have. It is dictionary of key value pairs,
 * that is stored in thread local storage.
 * <My understanding is we init it outside thread pool context and capture from within>
 *
 * AsyncLocal<int>: Encapsulate a variable and flow its state via
 * execution context.
 */

/*
 * But queues are very low level: we can fork but we can refer to it or join multiple threads, like continuations.
 * Solution: NET Tasks.
 */

AsyncLocal<int> myValue = new();
for (int i = 0; i < 100; i++)
{
    myValue.Value = i;
    MyThreadPool.QueueUserWorkItem(delegate
    {
        Console.WriteLine(myValue.Value);
        Thread.Sleep(1000); //this is bad, blocking.
    });
}

Console.ReadLine();

static class MyThreadPool
{
    private static readonly BlockingCollection<(Action, ExecutionContext?)> s_workItems = new();

    public static void QueueUserWorkItem(Action action) => s_workItems.Add((action, ExecutionContext.Capture()));

    static MyThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    (Action workItem, ExecutionContext? context) = s_workItems.Take();
                    if (context is null)
                        workItem();
                    else
                        ExecutionContext.Run(context, (object? state) => ((Action)state!).Invoke(), workItem);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}