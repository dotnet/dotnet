// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

namespace Microsoft.IdentityModel.Telemetry
{
    /// <summary>
    /// Pushes telemetry data to the configured <see cref="Counter{T}"/> or <see cref="Histogram{T}"/>.
    /// </summary>
    internal class TelemetryDataRecorder
    {
        /// <summary>
        /// Meter name for MicrosoftIdentityModel.
        /// </summary>
        private const string MeterName = "MicrosoftIdentityModel_Meter";

        /// <summary>
        /// The meter responsible for creating instruments.
        /// </summary>
        private static readonly Meter IdentityModelMeter = new(MeterName, "1.0.0");

        internal const string TotalDurationHistogramName = "IdentityModelConfigurationRequestTotalDurationInMS";

        /// <summary>
        /// Counter to capture configuration refresh requests to ConfigurationManager.
        /// </summary>
        internal const string IdentityModelConfigurationManagerCounterName = "IdentityModelConfigurationManager";
        internal const string IdentityModelConfigurationManagerCounterDescription = "Counter capturing configuration manager operations.";
        internal static readonly Counter<long> ConfigurationManagerCounter = IdentityModelMeter.CreateCounter<long>(IdentityModelConfigurationManagerCounterName, description: IdentityModelConfigurationManagerCounterDescription);

        /// <summary>
        /// Counter to capture background refresh failures in the ConfigurationManager.
        /// </summary>
        internal const string BackgroundConfigurationRefreshFailureCounterName = "IdentityModelConfigurationManagerBackgroundRefreshFailure";
        internal const string BackgroundConfigurationRefreshFailureCounterDescription = "Counter capturing configuration manager background refresh failures.";
        internal static readonly Counter<long> BackgroundConfigurationRefreshFailureCounter = IdentityModelMeter.CreateCounter<long>(BackgroundConfigurationRefreshFailureCounterName, description: BackgroundConfigurationRefreshFailureCounterDescription);

        /// <summary>
        /// Counter to capture signature validation results along with algorithm and key size.
        /// </summary>
        internal const string SignatureValidationCounterName = "IdentityModelSignatureValidation";
        internal const string SignatureValidationCounterDescription = "Counter capturing signature validation operations with algorithm and key size details.";
        internal static readonly Counter<long> SignatureValidationCounter = IdentityModelMeter.CreateCounter<long>(SignatureValidationCounterName, description: SignatureValidationCounterDescription);

        /// <summary>
        /// Histogram to capture total duration of configuration retrieval by ConfigurationManager in milliseconds.
        /// </summary>
        internal static readonly Histogram<long> TotalDurationHistogram = IdentityModelMeter.CreateHistogram<long>(
            TotalDurationHistogramName,
            unit: "ms",
            description: "Configuration retrieval latency during configuration manager operations.");

        internal static void RecordConfigurationRetrievalDurationHistogram(long requestDurationInMs, in TagList tagList)
        {
            TotalDurationHistogram.Record(requestDurationInMs, tagList);
        }

        internal static void IncrementConfigurationRefreshRequestCounter(in TagList tagList)
        {
            ConfigurationManagerCounter.Add(1, tagList);
        }

        internal static void IncrementBackgroundConfigurationRefreshFailureCounter(in TagList tagList)
        {
            BackgroundConfigurationRefreshFailureCounter.Add(1, tagList);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IncrementSignatureValidationCounter(in TagList tagList)
        {
            SignatureValidationCounter.Add(1, tagList);
        }
    }
}
