using System.Collections.Concurrent;

namespace MyThreadpool.Core;

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

public static class MyThreadPoolWithExecutionContext
{
    private static readonly BlockingCollection<(Action, ExecutionContext?)> s_workItems = new();

    public static void QueueUserWorkItem(Action action) => s_workItems.Add((action, ExecutionContext.Capture()));

    static MyThreadPoolWithExecutionContext()
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