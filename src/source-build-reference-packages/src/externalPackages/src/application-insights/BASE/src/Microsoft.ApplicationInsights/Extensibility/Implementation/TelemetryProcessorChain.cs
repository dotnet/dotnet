namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Represents the TelemetryProcessor chain. Clients should use TelemetryProcessorChainBuilder to construct this object.
    /// </summary>
    public sealed class TelemetryProcessorChain : IDisposable
    {        
        private readonly SnapshottingList<ITelemetryProcessor> telemetryProcessors = new SnapshottingList<ITelemetryProcessor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorChain" /> class.
        /// Marked internal, as clients should use TelemetryProcessorChainBuilder to build the processing chain.
        /// </summary>
        internal TelemetryProcessorChain()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorChain" /> class by using the given list elements.
        /// Marked internal, as clients should use TelemetryProcessorChainBuilder to build the processing chain.
        /// </summary>
        internal TelemetryProcessorChain(IEnumerable<ITelemetryProcessor> telemetryProcessors)
        {
            foreach (var item in telemetryProcessors)
            {
                this.telemetryProcessors.Add(item);
            }
        }

        /// <summary>
        /// Gets the first telemetry processor from the chain of processors.        
        /// </summary>
        internal ITelemetryProcessor FirstTelemetryProcessor
        {
            get { return this.telemetryProcessors.First(); }            
        }

        /// <summary>
        /// Gets the list of TelemetryProcessors making up this chain.        
        /// </summary>
        internal SnapshottingList<ITelemetryProcessor> TelemetryProcessors
        {
            get { return this.telemetryProcessors; }                       
        }

        /// <summary>
        /// Invokes the process method in the first telemetry processor.
        /// </summary>        
        public void Process(ITelemetry item)
        {
            this.telemetryProcessors.First().Process(item);
        }

        /// <summary>
        /// Releases resources used by the current instance of the <see cref="TelemetryProcessorChain"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                SnapshottingList<ITelemetryProcessor> processors = this.telemetryProcessors;

                if (processors != null)
                {
                    foreach (ITelemetryProcessor processor in processors)
                    {
                        if (processor is IDisposable disposableProcessor)
                        {
                            disposableProcessor.Dispose();
                        }
                    }
                }
            }
        }
    }
}
