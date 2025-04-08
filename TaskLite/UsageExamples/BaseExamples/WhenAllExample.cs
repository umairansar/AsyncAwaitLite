using TaskLite;

namespace UsageExamples.BaseExamples;

public class WhenAllExample
{
    public static void Execute()
    {
        AsyncLocal<int> myValue = new();
        List<MyTask> tasks = new();
        for (int i = 0; i < 100; i++)
        {
            myValue.Value = i;
            tasks.Add(MyTask.Run(delegate
            {
                Console.WriteLine(myValue.Value);
                Thread.Sleep(1000); //this is bad, blocking.
            }));
        }

        var finalTask = MyTask.WhenAll(tasks);
        Console.WriteLine(finalTask.IsCompleted);
        finalTask.Wait();
        Console.WriteLine(finalTask.IsCompleted);
    }
}