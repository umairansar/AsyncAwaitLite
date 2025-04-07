using TaskLite;

namespace UsageExamples.BaseExamples;

public class RunWaitExample
{
    public static void Execute()
    {
        Utils.PrintBanner(nameof(RunWaitExample));

        AsyncLocal<int> myValue = new(); 
        List<MyTask> tasks = new();
        for (int i = 0; i < 50; i++)
        {
            myValue .Value = i;
            tasks.Add(MyTask.Run(delegate
            {
                Console.WriteLine(myValue.Value);
                Thread.Sleep(1000); //this is bad, blocking.
            }));
        }
        
        foreach(var ts in tasks) ts.Wait();
    }
}