namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Hosting;

    internal class FakeTelemetryProcessorWithImportingConstructor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor nextProcessor;

        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Constructs an instance of the telemetry processor.
        /// This constructor is designed to be called from a DI framework.
        /// </summary>
        /// <param name="next">The next procesor in the chain.</param>
        /// <param name="hostingEnvironment">The hosting environment. This parameter will be injected by the DI framework.</param>
        public FakeTelemetryProcessorWithImportingConstructor(ITelemetryProcessor next, IHostingEnvironment hostingEnvironment)
        {
            this.nextProcessor = next;
            this.HostingEnvironment = hostingEnvironment;
        }

        public void Process(ITelemetry item)
        {
            nextProcessor.Process(item);
        }
    }
}
