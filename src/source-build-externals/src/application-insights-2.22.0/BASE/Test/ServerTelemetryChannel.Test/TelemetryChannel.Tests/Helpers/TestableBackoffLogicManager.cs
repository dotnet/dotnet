namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.Channel.Implementation;

    internal class TestableBackoffLogicManager : BackoffLogicManager
    {
        private readonly TimeSpan backoffInterval;

        public TestableBackoffLogicManager(TimeSpan backoffInterval, int defaultBackoffEnabledIntervalInMin = 30) : base(TimeSpan.FromMinutes(defaultBackoffEnabledIntervalInMin))
        {
            this.backoffInterval = backoffInterval;
        }

        protected override TimeSpan GetBackOffTime(string headerValue)
        {
            return this.backoffInterval;
        }
    }
}
