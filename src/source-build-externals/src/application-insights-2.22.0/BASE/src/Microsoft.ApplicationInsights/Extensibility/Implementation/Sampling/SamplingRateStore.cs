namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Sampling
{
    using System;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class SamplingRateStore
    {
        private double requestSampleRate = 100;
        private double dependencySampleRate = 100;
        private double eventSampleRate = 100;
        private double exceptionSampleRate = 100;
        private double pageViewSampleRate = 100;
        private double messageSampleRate = 100;

        /// <summary>
        /// Gets last known request sampling percentage for telemetry type.
        /// </summary>
        internal double GetLastObservedSamplingPercentage(SamplingTelemetryItemTypes samplingItemType)
        {
            switch (samplingItemType)
            {
                case SamplingTelemetryItemTypes.Request:
                    return this.requestSampleRate;
                case SamplingTelemetryItemTypes.Message:
                    return this.messageSampleRate;
                case SamplingTelemetryItemTypes.RemoteDependency:
                    return this.dependencySampleRate;
                case SamplingTelemetryItemTypes.Event:
                    return this.eventSampleRate;
                case SamplingTelemetryItemTypes.Exception:
                    return this.exceptionSampleRate;
                case SamplingTelemetryItemTypes.PageView:
                    return this.pageViewSampleRate;
                default:
                    throw new ArgumentException("Unsupported Item Type", nameof(samplingItemType));
            }
        }

        /// <summary>
        /// Sets last known request sampling percentage for telemetry type.
        /// </summary>
        internal void SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes samplingItemType, double value)
        {
            switch (samplingItemType)
            {
                case SamplingTelemetryItemTypes.Request:
                    Interlocked.Exchange(ref this.requestSampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.Message:
                    Interlocked.Exchange(ref this.messageSampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.RemoteDependency:
                    Interlocked.Exchange(ref this.dependencySampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.Event:
                    Interlocked.Exchange(ref this.eventSampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.Exception:
                    Interlocked.Exchange(ref this.exceptionSampleRate, value);
                    break;
                case SamplingTelemetryItemTypes.PageView:
                    Interlocked.Exchange(ref this.pageViewSampleRate, value);
                    break;
                default:
                    throw new ArgumentException("Unsupported Item Type", nameof(samplingItemType));
            }
        }
    }
}
