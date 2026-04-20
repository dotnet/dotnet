#if NET452
namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Threading;

    internal static class RetryPolicy
    {
        public static TResult Retry<TException, T, TResult>(
            Func<T, TResult> action,
            T param1,
            TimeSpan retryInterval,
            int retryCount = 3) where TException : Exception where TResult : class
        {
            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            for (int retry = 0; retry < retryCount; ++retry)
            {
                try
                {
                    return action(param1);
                }
                catch (Exception ex)
                {
                    if (((retry + 1) < retryCount) && (ex is TException))
                    {
                        Thread.Sleep(retryInterval);
                        continue;
                    }

                    throw;
                }
            }

            return null;
        }
    }
}
#endif