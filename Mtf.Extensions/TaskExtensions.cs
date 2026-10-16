using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mtf.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Logs any exception of <paramref name="task"/> and returns a task that completes with the same
        /// state (RanToCompletion, Canceled or Faulted) and the same exception. The returned task's exception
        /// is marked as observed, so callers that discard the result (fire-and-forget) do not trigger
        /// <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </summary>
        public static Task LogExceptions(this Task task)
        {
            if (task == null)
            {
                return null;
            }

            var result = LogExceptionsCore(task);
            ObserveException(result);
            return result;
        }

        public static Task<T> LogExceptions<T>(this Task<T> task)
        {
            if (task == null)
            {
                return null;
            }

            var result = LogExceptionsCore(task);
            ObserveException(result);
            return result;
        }

        private static async Task LogExceptionsCore(Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Log(task);
                throw;
            }
        }

        private static async Task<T> LogExceptionsCore<T>(Task<T> task)
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                Log(task);
                throw;
            }
        }

        private static void Log(Task task)
        {
            if (task.Exception == null)
            {
                return;
            }

            foreach (var exception in task.Exception.Flatten().InnerExceptions)
            {
                Debug.WriteLine(exception);
                Console.Error.WriteLine(exception);
            }
        }

        private static void ObserveException(Task task)
        {
            _ = task.ContinueWith(t => _ = t.Exception,
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }
    }
}
