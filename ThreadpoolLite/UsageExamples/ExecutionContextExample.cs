using MyThreadpool.Core;

namespace MyThreadpool.UsageExamples;

public class ExecutionContextExample
{
    public static void Execute()
    {
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
    }
}