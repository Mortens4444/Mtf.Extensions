using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mtf.Extensions
{
    public static class TaskExtensions
    {
        public static Task LogExceptions(this Task task)
        {
            return task?.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception != null)
                {
                    var aggregateException = t.Exception.Flatten();
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        Debug.WriteLine(exception);
                        Console.Error.WriteLine(exception);
                    }
                }
                t.GetAwaiter().GetResult();
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public static Task<T> LogExceptions<T>(this Task<T> task)
        {
            return task?.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception != null)
                {
                    var aggregateException = t.Exception.Flatten();
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        Debug.WriteLine(exception);
                        Console.Error.WriteLine(exception);
                    }
                }
                return t.GetAwaiter().GetResult();
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }
    }
}
