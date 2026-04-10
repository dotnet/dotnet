namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Shared.Extensibility.Implementation;

    /// <summary>
    /// Represents an object used to Build a TelemetryProcessorChain.
    /// </summary>
    public sealed class TelemetryProcessorChainBuilder
    {
        private readonly List<Func<ITelemetryProcessor, ITelemetryProcessor>> factories;
        private readonly TelemetryConfiguration configuration;
        private readonly TelemetrySink telemetrySink;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorChainBuilder" /> class.
        /// </summary>
        /// <param name="configuration"> The <see cref="TelemetryConfiguration"/> instance to which the constructed processing chain should be set to.</param>        
        public TelemetryProcessorChainBuilder(TelemetryConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.factories = new List<Func<ITelemetryProcessor, ITelemetryProcessor>>();
            this.telemetrySink = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorChainBuilder" /> class.
        /// </summary>
        /// <param name="configuration">Configuration instance to use for constructing the processor chain.</param>
        /// <param name="telemetrySink">Telemetry sink the processor chain will be assigned to.</param>
        public TelemetryProcessorChainBuilder(TelemetryConfiguration configuration, TelemetrySink telemetrySink) : this(configuration)
        {
            this.telemetrySink = telemetrySink ?? throw new ArgumentNullException(nameof(telemetrySink));
        }

        internal TelemetrySink TelemetrySink => this.telemetrySink;

        /// <summary>
        /// Uses given factory to add TelemetryProcessor to the chain of processors. The processors
        /// in the chain will be invoked in the same order in which they are added.
        /// </summary>
        /// <param name="telemetryProcessorFactory">A delegate that returns a <see cref="ITelemetryProcessor"/>
        /// , given the next <see cref="ITelemetryProcessor"/> in the call chain.</param>
        public TelemetryProcessorChainBuilder Use(Func<ITelemetryProcessor, ITelemetryProcessor> telemetryProcessorFactory)
        {
            this.factories.Add(telemetryProcessorFactory);
            return this;
        }

        /// <summary>
        /// Builds the chain of linked <see cref="ITelemetryProcessor" /> instances and sets the same in configuration object passed.
        /// A special telemetry processor for handling Transmission is always appended as the last
        /// processor in the chain.
        /// </summary>        
        public void Build()
        {
            var telemetryProcessorsList = new List<ITelemetryProcessor>();
            ITelemetryProcessor linkedTelemetryProcessor;

            if (this.telemetrySink == null)
            {
                // We are building the "common" telemetry processor chain.
                if (this.configuration.TelemetrySinks.Count == 1)
                {
                    // We just need to pass the telemetry directly into the (single) sink.
                    linkedTelemetryProcessor = new PassThroughProcessor(this.configuration.DefaultTelemetrySink);
                }
                else
                {
                    linkedTelemetryProcessor = new BroadcastProcessor(this.configuration.TelemetrySinks);
                }
            }
            else
            {
                linkedTelemetryProcessor = new TransmissionProcessor(this.telemetrySink);
            }

            telemetryProcessorsList.Add(linkedTelemetryProcessor);

            foreach (var generator in this.factories.AsEnumerable().Reverse())
            {
                ITelemetryProcessor prevTelemetryProcessor = linkedTelemetryProcessor;
                linkedTelemetryProcessor = generator.Invoke(linkedTelemetryProcessor);

                if (linkedTelemetryProcessor == null)
                {
                    // Loading of a telemetry processor failed, so skip it
                    // Error is logged when telemetry processor loading failed
                    linkedTelemetryProcessor = prevTelemetryProcessor;
                    continue;
                }

                telemetryProcessorsList.Add(linkedTelemetryProcessor);

                // If a Processor also implements ITelemtryModule, We should Initialize that Module
                if (linkedTelemetryProcessor is ITelemetryModule telemetryModule)
                {
                    try
                    {
                        telemetryModule.Initialize(this.configuration);
                    }
                    catch (Exception ex)
                    {
                        CoreEventSource.Log.ComponentInitializationConfigurationError(telemetryModule.ToString(), ex.ToInvariantString());
                    }
                }
            }
            
            // Save changes to the TelemetryProcessorChain
            var telemetryProcessorChain = new TelemetryProcessorChain(telemetryProcessorsList.AsEnumerable().Reverse());
            if (this.telemetrySink != null)
            {
                this.telemetrySink.TelemetryProcessorChain = telemetryProcessorChain;
            }
            else
            {
                this.configuration.TelemetryProcessorChain = telemetryProcessorChain;
            }
        }
    }
}
