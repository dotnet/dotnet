namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.ComponentModel;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

    /// <summary>
    /// Extension methods for <see cref="TelemetryProcessorChainBuilder"/>.
    /// Adds shorthand for adding well-known processors.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TelemetryProcessorChainBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="SamplingTelemetryProcessor"/> to the given<see cref="TelemetryProcessorChainBuilder" />.
        /// </summary>
        /// <param name="builder">Instance of <see cref="TelemetryProcessorChainBuilder"/>.</param>
        /// <param name="samplingPercentage">Sampling Percentage to configure.</param>     
        /// <param name="excludedTypes">Semicolon separated list of types that should not be sampled. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace.</param>   
        /// <param name="includedTypes">Semicolon separated list of types that should be sampled. All types are sampled when left empty. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace.</param> 
        /// <return>Same instance of <see cref="TelemetryProcessorChainBuilder"/>.</return>
        public static TelemetryProcessorChainBuilder UseSampling(this TelemetryProcessorChainBuilder builder, double samplingPercentage, string excludedTypes = null, string includedTypes = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(next => new SamplingTelemetryProcessor(next)
            {
                SamplingPercentage = samplingPercentage,
                ProactiveSamplingPercentage = null,
                ExcludedTypes = excludedTypes,
                IncludedTypes = includedTypes,
            });
        }

        /// <summary>
        /// Adds <see cref="AdaptiveSamplingTelemetryProcessor"/> to the <see cref="TelemetryProcessorChainBuilder" />.
        /// </summary>
        /// <param name="builder">Instance of <see cref="TelemetryProcessorChainBuilder"/>.</param>
        /// <param name="excludedTypes">Semicolon separated list of types that should not be sampled. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. </param>
        /// <param name="includedTypes">Semicolon separated list of types that should be sampled. All types are sampled when left empty. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. </param> 
        /// <return>Same instance of <see cref="TelemetryProcessorChainBuilder"/>.</return>
        public static TelemetryProcessorChainBuilder UseAdaptiveSampling(this TelemetryProcessorChainBuilder builder, string excludedTypes = null, string includedTypes = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(next => new AdaptiveSamplingTelemetryProcessor(next)
            {
                ExcludedTypes = excludedTypes,
                IncludedTypes = includedTypes,
            });
        }

        /// <summary>
        /// Adds <see cref="AdaptiveSamplingTelemetryProcessor"/> to the <see cref="TelemetryProcessorChainBuilder" />.
        /// </summary>
        /// <param name="builder">Instance of <see cref="TelemetryProcessorChainBuilder"/>.</param>
        /// <param name="maxTelemetryItemsPerSecond">Maximum number of telemetry items to be generated on this application instance.</param>
        /// <param name="excludedTypes">Semicolon separated list of types that should not be sampled. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. </param>
        /// <param name="includedTypes">Semicolon separated list of types that should be sampled. All types are sampled when left empty. Allowed type names: Dependency, Event, Exception, PageView, Request, Trace. </param> 
        /// <return>Same instance of <see cref="TelemetryProcessorChainBuilder"/>.</return>
        public static TelemetryProcessorChainBuilder UseAdaptiveSampling(this TelemetryProcessorChainBuilder builder, double maxTelemetryItemsPerSecond, string excludedTypes = null, string includedTypes = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(next => new AdaptiveSamplingTelemetryProcessor(next)
            {
                MaxTelemetryItemsPerSecond = maxTelemetryItemsPerSecond,
                ExcludedTypes = excludedTypes,
                IncludedTypes = includedTypes,
            });
        }

        /// <summary>
        /// Adds <see cref="AdaptiveSamplingTelemetryProcessor"/> to the <see cref="TelemetryProcessorChainBuilder" />.
        /// </summary>
        /// <param name="builder">Instance of <see cref="TelemetryProcessorChainBuilder"/>.</param>
        /// <param name="settings">Set of settings applicable to dynamic sampling percentage algorithm.</param>
        /// <param name="callback">Callback invoked every time sampling percentage evaluation occurs.</param>
        /// <param name="excludedTypes">Semicolon separated list of types that should not be sampled.</param>
        /// <param name="includedTypes">Semicolon separated list of types that should be sampled. All types are sampled when left empty.</param> 
        /// <return>Same instance of <see cref="TelemetryProcessorChainBuilder"/>.</return>
        public static TelemetryProcessorChainBuilder UseAdaptiveSampling(
            this TelemetryProcessorChainBuilder builder,
            WindowsServer.Channel.Implementation.SamplingPercentageEstimatorSettings settings,
            WindowsServer.Channel.Implementation.AdaptiveSamplingPercentageEvaluatedCallback callback, 
            string excludedTypes = null,
            string includedTypes = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return builder.Use(next => new AdaptiveSamplingTelemetryProcessor(settings, callback, next)
            {
                InitialSamplingPercentage = 100.0 / settings.EffectiveInitialSamplingRate,
                ExcludedTypes = excludedTypes,
                IncludedTypes = includedTypes,
            });
        }
    }
}
