using System;

namespace TaskLite;

public interface IMyTask<TMyTask, TValue> 
    where TMyTask : IMyTask<TMyTask, TValue>
{
    bool IsCompleted { get; }
    TValue? Result { get; }
    void SetResult(TValue value);
    void SetException(Exception exception);
    void Wait();
    static abstract TMyTask Delay(int timeout);
    TMyTask ContinueWith(Action action);
    TMyTask ContinueWith(Func<TMyTask> action);
    static abstract TMyTask Run(Func<TMyTask> action);
}

public interface IMyTask<T>
{
    bool IsCompleted { get; }
    void SetResult();
    void SetException(Exception exception);
    void Wait();
    static abstract T Delay(int timeout);
    T ContinueWith(Action action);
    T ContinueWith(Func<T> action);
    static abstract T Run(Action action);
    static abstract T WhenAll(List<T> tasks);
    static abstract T Iterate(IEnumerable<T> tasks);
}