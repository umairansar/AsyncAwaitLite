using TaskLite;

namespace UsageExamples.BaseExamples;

public static class IterateExample
{
    public static void Execute()
    {
        
        ManualResetEventSlim finished = new ManualResetEventSlim();
        var iterateTask = MyTask.Iterate(PrintAsync());
        Console.WriteLine("ok1");
        iterateTask.ContinueWith(delegate
        {
            Console.WriteLine($"waiting via continue with: {iterateTask.IsCompleted}");
            finished.Set();
        });
        Console.WriteLine("ok2");
        finished.Wait();


        static IEnumerable<MyTask> PrintAsync()
        {
            for (var i = 0; i < 60; i++)
            {
                yield return MyTask.Delay(100);
                Console.WriteLine(i);
            }
        }
    }
}