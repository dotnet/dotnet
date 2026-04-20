namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Threading.Tasks;
    
    internal static class ExceptionHandler
    {
        /// <summary>
        /// Starts the <paramref name="asyncMethod"/>, catches and logs any exceptions it may throw.
        /// </summary>
        public static void Start(Func<Task> asyncMethod)
        {
            try
            {
                // Do not use await here because ASP.NET does not allow that and throws
                var asyncTask = asyncMethod();
                if (asyncTask != null)
                {
                    asyncTask.ContinueWith(
                        task => TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(task.Exception.ToString()),
                        TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(exp.ToString());
            }
        }

        public static void Start<T>(Func<T, Task> asyncMethod, T param1)
        {
            try
            {
                // Do not use await here because ASP.NET does not allow that and throws
                asyncMethod(param1).ContinueWith(
                    task => TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(task.Exception.ToString()),
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ExceptionHandlerStartExceptionWarning(exp.ToString());
            }
        }
    }
}