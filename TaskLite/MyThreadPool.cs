using System.Collections.Concurrent;

namespace TaskLite;

/*
 *  MyThreadPool
 *  Enqueues new actions to a 'BlockingCollection'.
 *  Start threads equal to number of processors on a computer.
 *  Each thread loops forever and tries to dequeue/take from the BlockingCollection.
 *  Take blocks the thread until an item is removed/returned from the BlockingCollection.
 *
 *  BlockingCollection blocks the thread. Improve this with non-blocking, asynchronous Channel<T>
 *  introduced in .Net5.
 */

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