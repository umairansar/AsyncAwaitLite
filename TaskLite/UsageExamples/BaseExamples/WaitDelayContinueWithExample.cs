using TaskLite;

namespace UsageExamples.BaseExamples;

public class WaitDelayContinueWithExample
{
    public static void Execute()
    {
        Utils.PrintBanner(nameof(WaitDelayContinueWithExample));

        Console.WriteLine("Hello, ");
        var t = MyTask.Delay(8000).ContinueWith(delegate
        {
            Console.Write("World!");
            return MyTask.Delay(2000).ContinueWith(delegate
            {
                Console.WriteLine(" We made it!");
            });
        });
        Console.WriteLine("Before Wait, ");
        t.Wait();
        Console.WriteLine("After wait ");
    }
}