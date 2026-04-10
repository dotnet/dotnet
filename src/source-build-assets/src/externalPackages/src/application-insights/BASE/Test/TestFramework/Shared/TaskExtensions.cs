namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class TaskExtensions
    {
        /// <summary>
        /// A helper method for mocking APM (classic) asynchronous methods with on TAP (modern) asynchronous methods.
        /// </summary>
        /// <remarks>
        /// This is based on <a href="http://blogs.msdn.com/b/pfxteam/archive/2011/06/27/10179452.aspx"/> 
        /// and <a href="http://msdn.microsoft.com/en-us/library/hh873178.aspx"/>.
        /// </remarks>
        public static IAsyncResult AsAsyncResult(this Task task, AsyncCallback callback, object state)
        {
            Debug.Assert(task != null, "task");

            var taskCompletionSource = new TaskCompletionSource<object>(state);
            task.ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        taskCompletionSource.TrySetException(t.Exception.InnerExceptions);
                    }
                    else if (t.IsCanceled)
                    {
                        taskCompletionSource.TrySetCanceled();
                    }
                    else
                    {
                        taskCompletionSource.SetResult(null);
                    }

                    if (callback != null)
                    {
                         callback(taskCompletionSource.Task);
                    }
                },
                TaskScheduler.Default);

            return taskCompletionSource.Task;
        }
    }
}
