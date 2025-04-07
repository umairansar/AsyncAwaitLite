using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace TaskLite;

class MyTask<T> : IMyTask<MyTask<T>, T>
{
    private bool _completed;
    private T? _value;
    private Exception? _exception;
    private Action? _continuation;
    private ExecutionContext? _context;
    
    public bool IsCompleted { 
        get
        {
            lock (this)
            {
                return _completed;
            }
        } 
    }

    public T? GetValue {  
        get
        {
            lock (this)
            {
                return _value;
            }
        } 
    }

    public void SetResult(T value) => Complete(value);

    public void SetException(Exception exception) => Complete(_value, exception); //TODO: pass null instead of _value
    
    private void Complete(T? value, Exception? exception = null)
    {
        lock (this)
        {
            if (_completed) throw new Exception("Can't complete a task again.");

            _completed = true;
            _value = value;
            _exception = exception;

            if (_continuation is not null)
            {
                MyThreadPool.QueueUserWorkItem(delegate
                {
                    if (_context is null)
                    {
                        _continuation();
                    }
                    else
                    {
                        //We do this because MyThreadPool captures context of current thread’s,
                        //not the context that explicitly exists as private field.
                        ExecutionContext.Run(_context, state => ((Action) state!).Invoke(), _continuation);
                    }
                });
            }
        }
       
    }

    public void Wait()
    {
        ManualResetEventSlim mres = null;

        lock (this)
        {
            if (!_completed)
            {
                mres = new ManualResetEventSlim();
                ContinueWith(mres.Set); 
            }
        }

        mres?.Wait(); //waits until set

        if (_exception is not null)
        {
            ExceptionDispatchInfo.Throw(_exception); 
        }
    }

    public static MyTask<T> Delay(int timeout)
    {
        MyTask<T> t = new();
        new Timer(_ => t.SetResult(default)).Change(timeout, -1);
        return t;
    }

    public MyTask<T> ContinueWith(Action action)
    {
        MyTask<T> t = new();

        Action callback = () =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
            t.SetResult(default);
        };
        
        lock (this)
        {
            if (_completed)
            {
                //Why are we ignoring context here?
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                _continuation = callback;
                _context = ExecutionContext.Capture();
            }
        }

        return t;
    }

    public MyTask<T> ContinueWith(Func<MyTask<T>> action)
    {
        MyTask<T> t = new();

        Action callback = () =>
        {
            try
            {
                MyTask<T> next = action();
                next.ContinueWith(delegate
                {
                    if (next._exception is not null)
                        t.SetException(next._exception);
                    else
                        t.SetResult(next.GetValue);
                });
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
        };
        
        lock (this)
        {
            if (_completed)
            {
                //Why are we ignoring context here?
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                _continuation = callback;
                _context = ExecutionContext.Capture();
            }
        }

        return t;
    }

    public static MyTask<T> Run(Func<MyTask<T>> action)
    {
        MyTask<T> t = new();

        MyThreadPool.QueueUserWorkItem(() =>
        {
            try
            {
                var result = action();
                t.SetResult(result.GetValue);
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
        });
        
        return t;
    }
}

class MyTask : IMyTask<MyTask>
{
    private bool _completed;
    private Exception? _exception;
    private Action? _continuation;
    private ExecutionContext? _context;

    public bool IsCompleted
    {
        get
        {
            lock (this)
            {
                return _completed;
            }
        }
    }

    public void SetResult() => Complete(null);

    public void SetException(Exception exception) => Complete(exception);

    private void Complete(Exception? exception)
    {
        lock (this)
        {
            if (_completed) throw new Exception("Can't complete a task again.");

            _completed = true;
            _exception = exception;

            if (_continuation is not null)
            {
                MyThreadPool.QueueUserWorkItem(delegate
                {
                    if (_context is null)
                    {
                        _continuation();
                    }
                    else
                    {
                        //We do this because MyThreadPool captures context of current thread’s,
                        //not the context that explicitly exists as private field.
                        ExecutionContext.Run(_context, state => ((Action) state!).Invoke(), _continuation);
                    }
                });
            }
        }
       
    }

    public void Wait()
    {
        ManualResetEventSlim mres = null;

        lock (this)
        {
            if (!_completed)
            {
                mres = new ManualResetEventSlim();
                ContinueWith(mres.Set); 
            }
        }

        mres?.Wait(); //waits until set

        if (_exception is not null)
        {
            ExceptionDispatchInfo.Throw(_exception); 
        }
    }

    public static MyTask Delay(int timeout)
    {
        MyTask t = new();
        new Timer(_ => t.SetResult()).Change(timeout, -1);
        return t;
    }

    public MyTask ContinueWith(Action action)
    {
        MyTask t = new();

        Action callback = () =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
            t.SetResult();
        };
        
        lock (this)
        {
            if (_completed)
            {
                //Why are we ignoring context here?
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                _continuation = callback;
                _context = ExecutionContext.Capture();
            }
        }

        return t;
    }
    
    public MyTask ContinueWith(Func<MyTask> action)
    {
        MyTask t = new();

        Action callback = () =>
        {
            try
            {
                MyTask next = action();
                next.ContinueWith(delegate
                {
                    if (next._exception is not null)
                        t.SetException(next._exception);
                    else
                        t.SetResult();
                });
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
        };
        
        lock (this)
        {
            if (_completed)
            {
                //Why are we ignoring context here?
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                _continuation = callback;
                _context = ExecutionContext.Capture();
            }
        }

        return t;
    }

    public static MyTask Run(Action action)
    {
        MyTask t = new();

        MyThreadPool.QueueUserWorkItem(() =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
            
            t.SetResult();
        });
        
        return t;
    }
}