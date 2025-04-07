namespace UsageExamples.GenericExamples;

public class WaitDelayRunExample
{
    public static void Execute()
    {
        Utils.PrintBanner(nameof(WaitDelayRunExample));

        var number = Utils.GetNumber();
        var myNumber = Utils.MyGetNumber();
        var myNumberWithRun = Utils.MyGetNumberWithRun();

        myNumber.Wait();
        myNumberWithRun.Wait();

        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] BCL Task Result {number.Result}");
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] My Task Result {myNumber.Result}");
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] My Task Result with Run {myNumberWithRun.Result}");
    }
}