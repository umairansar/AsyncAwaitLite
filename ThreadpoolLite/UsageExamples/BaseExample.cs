using MyThreadpool.Core;
namespace MyThreadpool.UsageExamples;

public class BaseExample
{
    public static void Execute()
    {
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
    }
}