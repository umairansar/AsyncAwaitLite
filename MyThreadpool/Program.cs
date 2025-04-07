using System;
using System.Collections.Concurrent;
using System.Threading;

for (int i = 0; i < 100; i++)
{
    int currentContext_i = i;
    MyThreadPool.QueueUserWorkItem(delegate
    {
        Console.WriteLine(currentContext_i);
        Thread.Sleep(1000);
    });
}

Console.ReadLine();

static class MyThreadPool
{
    private static readonly BlockingCollection<Action> s_workItems = new();

    public static void QueueUserWorkItem(Action action) => s_workItems.Add(action);

    static MyThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            new Thread(() =>
            {
                while (true)
                {
                    Action workItem = s_workItems.Take();
                    workItem();
                }
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}