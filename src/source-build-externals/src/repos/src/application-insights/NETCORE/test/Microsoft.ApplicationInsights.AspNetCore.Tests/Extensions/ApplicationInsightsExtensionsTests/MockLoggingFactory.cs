namespace Microsoft.Extensions.DependencyInjection.Test
{
    using Logging;

    internal class MockLoggingFactory : ILoggerFactory
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return null;
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }
    }
}