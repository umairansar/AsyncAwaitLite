using System.Collections.Concurrent;

namespace MyThreadpool.Core;

public static class MyThreadPool
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