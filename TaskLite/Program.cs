using System;
using System.Threading.Tasks;
using TaskLite;

var number = GetNumber();
var myNumber = MyGetNumber();
var myNumberWithRun = MyGetNumberWithRun();

Console.WriteLine($"BCL Task Result {number.Result}");
Console.WriteLine($"My Task Result {myNumber.GetValue}");
Console.WriteLine($"My Task Result with Run {myNumberWithRun.GetValue}");

async Task<int> GetNumber()
{
    await Task.Delay(500);
    return 4;
}

MyTask<int> MyGetNumber()
{
    MyTask<int>.Delay(500);
    var task = new MyTask<int>();
    task.SetResult(5);
    return task;
}

MyTask<int> MyGetNumberWithRun()
{
    MyTask<int>.Delay(500);
    return MyTask<int>.Run(delegate
    {
        var result = new MyTask<int>();
        result.SetResult(6);
        return result;
    });
}

MyTask<int> task = new MyTask<int>();
task = task.ContinueWith(() => MyTask<int>.Run(delegate
{
    Console.Write("World!");
    // var zeroTask = TaskLite<int>.Delay(2000).ContinueWith(delegate
    // {
    //     Console.WriteLine(" We made it!");
    // });
    // return zeroTask;
    var result = new MyTask<int>();
    result.SetResult(5);
    return result;
}));
task.SetResult(4);
Console.WriteLine($"Task result: {task.GetValue}");
task.Wait();

// Console.WriteLine("Hello, ");
// TaskLite.Delay(2000).ContinueWith(delegate
// {
//     Console.Write("World!");
//     return TaskLite.Delay(2000).ContinueWith(delegate
//     {
//         Console.WriteLine(" We made it!");
//     });
// }).Wait();
//
// AsyncLocal<int> myValue = new(); 
// List<TaskLite> tasks = new();
// for (int i = 0; i < 100; i++)
// {
//     myValue .Value = i;
//     tasks.Add(TaskLite.Run(delegate
//     {
//         Console.WriteLine(myValue.Value);
//         Thread.Sleep(1000); //this is bad, blocking.
//     }));
// }
//
// foreach(var t in tasks) t.Wait();