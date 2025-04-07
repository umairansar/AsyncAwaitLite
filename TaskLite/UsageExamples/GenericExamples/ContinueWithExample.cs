using UsageExamples;

namespace UsageExamples.GenericExamples;

public class ContinueWithExample
{
    public static void Execute()
    {
        Utils.PrintBanner(nameof(ContinueWithExample));

        var number = Utils.GetNumber();
        var myNumber = Utils.MyGetNumber();
        var myNumberWithRun = Utils.MyGetNumberWithRun();

        myNumber.Wait();
        myNumberWithRun.Wait();

        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] BCL Task Result {number.Result}");
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] My Task Result {myNumber.Result}");
        Console.WriteLine($"[Thread: {Thread.CurrentThread.ManagedThreadId}] My Task Result with Run {myNumberWithRun.Result}");
       
        number.ContinueWith(Utils.PrintOkViaActionDelegate).Wait();
        myNumber.ContinueWith(Utils.PrintOkViaActionDelegate).Wait();
        myNumberWithRun.ContinueWith(Utils.PrintOkViaFuncTask).Wait();
    }}