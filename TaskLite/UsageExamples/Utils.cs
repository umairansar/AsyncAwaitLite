using TaskLite;

namespace UsageExamples;

public static class Utils
{
    
    public static Task<int> GetNumber()
    {
        return GetNumberAsync();

        async Task<int> GetNumberAsync()
        {
            await Task.Delay(500);
            return 4;
        }
    }

    public static MyTask<int> MyGetNumber()
    {
        MyTask<int>.Delay(8000).Wait();
        var task = new MyTask<int>();
        task.SetResult(5);
        return task;
    }

    public static MyTask<int> MyGetNumberWithRun()
    {
        return MyTask<int>.Run(delegate
        {
            Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] Executing Inside Run..");
            var result = new MyTask<int>();
            result.SetResult(6);
            return result;
        });
    }
    
    public static void PrintBanner(string testName)
    {
        Console.WriteLine();
        Console.WriteLine("===================================");
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] Running {testName}...");
        Console.WriteLine("===================================");
        Console.WriteLine();
    }

    public static void PrintOkViaActionDelegate(Task<int> action)
    {
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] Continuation Ran via Action: Ok");
    }
    
    public static void PrintOkViaActionDelegate()
    {
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] Continuation Ran via Action: Ok");
    }
    
    public static MyTask<int> PrintOkViaFuncTask()
    {
        var printStringTaskWithDelay = MyTask<int>.Delay(3000).ContinueWith(delegate
        {
            var printOkTask = new MyTask<int>();
            printOkTask.SetResult(99);
            return printOkTask;
        });
        printStringTaskWithDelay.Wait();
        
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] Continuation Ran via Func: Ok");
        return printStringTaskWithDelay;
    }
}