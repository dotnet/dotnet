namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    internal static class Telemetry
    {
        public static void WriteEnvelopeProperties(this ITelemetry telemetry, ISerializationWriter json)
        {
            json.WriteProperty("time", telemetry.Timestamp.UtcDateTime.ToString("o", CultureInfo.InvariantCulture));

            var samplingSupportingTelemetry = telemetry as ISupportSampling;

            if (samplingSupportingTelemetry != null
                && samplingSupportingTelemetry.SamplingPercentage.HasValue
                && (samplingSupportingTelemetry.SamplingPercentage.Value > 0.0 + 1.0E-12)
                && (samplingSupportingTelemetry.SamplingPercentage.Value < 100.0 - 1.0E-12))
            {
                json.WriteProperty("sampleRate", samplingSupportingTelemetry.SamplingPercentage.Value);
            }

            json.WriteProperty("seq", telemetry.Sequence);
            WriteTelemetryContext(json, telemetry.Context);
        }

        public static void WriteTelemetryContext(ISerializationWriter json, TelemetryContext context)
        {
            if (context != null)
            {
                json.WriteProperty("iKey", context.InstrumentationKey);
                if (context.Flags != 0)
                {
                    json.WriteProperty("flags", context.Flags);
                }

                json.WriteProperty("tags", context.SanitizedTags);
            }
        }

        /// <summary>
        /// Copies GlobalProperties to the target's Properties. 
        /// This avoids accessing the public accessor GlobalProperties to avoid the penalty of ConcurrentDictionary instantiation.
        /// </summary> 
        internal static void CopyGlobalPropertiesIfExist(this ITelemetry telemetry)
        {
            if (telemetry.Context.GlobalPropertiesValue != null)
            {
                if (telemetry is ISupportProperties telemetryWithProperties)
                {
                    Utils.CopyDictionary(source: telemetry.Context.GlobalProperties, target: telemetryWithProperties.Properties);
                }
            }
        }

        /// <summary>
        /// Copies GlobalProperties to the target dictionary.
        /// This avoids accessing the public accessor GlobalProperties to avoid the penalty of ConcurrentDictionary instantiation.
        /// </summary>
        internal static void CopyGlobalPropertiesIfExist(this ITelemetry telemetry, IDictionary<string, string> target)
        {
            if (telemetry.Context.GlobalPropertiesValue != null)
            {
                Utils.CopyDictionary(telemetry.Context.GlobalProperties, target);
            }
        }

        /// <summary>
        /// Flattens Extension object on ITelemetry if exists into the properties and measurements.
        /// </summary>        
        internal static void FlattenIExtensionIfExists(this ITelemetry telemetry)
        {
            if (telemetry.Extension != null)
            {
                ISupportProperties itemWithProperties = telemetry as ISupportProperties;
                ISupportMetrics itemWithMetrics = telemetry as ISupportMetrics;

                // Do not serialize if data cannot be stored on the item
                if (itemWithProperties != null || itemWithMetrics != null)
                {
                    DictionarySerializationWriter extensionSerializationWriter = new DictionarySerializationWriter();
                    telemetry.Extension.Serialize(extensionSerializationWriter);

                    if (itemWithProperties != null)
                    {
                        Utils.CopyDictionary(extensionSerializationWriter.AccumulatedDictionary, itemWithProperties.Properties);
                    }

                    if (itemWithMetrics != null)
                    {
                        Utils.CopyDictionary(extensionSerializationWriter.AccumulatedMeasurements, itemWithMetrics.Metrics);
                    }
                }
            }
        }

        /// <summary>
        /// Flattens ITelemetry object into the properties and measurements.
        /// </summary>        
        /// <returns>EventData containing flattened ITelemetry object.</returns>
        internal static EventData FlattenTelemetryIntoEventData(this ITelemetry telemetry)
        {
            EventData flatTelemetry = new EventData();
            DictionarySerializationWriter dictionarySerializationWriter = new DictionarySerializationWriter();
            telemetry.SerializeData(dictionarySerializationWriter); // Properties and Measurements are covered as part of Data if present
            Utils.CopyDictionary(dictionarySerializationWriter.AccumulatedDictionary, flatTelemetry.properties);
            Utils.CopyDictionary(dictionarySerializationWriter.AccumulatedMeasurements, flatTelemetry.measurements);
            if (telemetry.Context.GlobalPropertiesValue != null)
            {
                Utils.CopyDictionary(telemetry.Context.GlobalProperties, flatTelemetry.properties);
            }

            if (telemetry.Extension != null)
            {
                DictionarySerializationWriter extensionSerializationWriter = new DictionarySerializationWriter();
                telemetry.Extension.Serialize(extensionSerializationWriter); // Extension is supposed to be flattened as well
                Utils.CopyDictionary(extensionSerializationWriter.AccumulatedDictionary, flatTelemetry.properties);
                Utils.CopyDictionary(extensionSerializationWriter.AccumulatedMeasurements, flatTelemetry.measurements);
            }

            return flatTelemetry;
        }

        /// <summary>
        /// Inspect if <see cref="ITelemetry"/> Properties contains 'DeveloperMode' and return it's boolean value.
        /// </summary>
        private static bool IsDeveloperMode(this ITelemetry telemetry)
        {
            if (telemetry is ISupportProperties telemetryWithProperties
                && telemetryWithProperties != null
                && telemetryWithProperties.Properties.TryGetValue("DeveloperMode", out string devModeProperty)
                && bool.TryParse(devModeProperty, out bool isDevMode))
            {
                return isDevMode;
            }

            return false;
        }

        /// <summary>
        /// Normalize instrumentation key by removing dashes ('-') and making string in the lowercase.
        /// In case no InstrumentationKey is available just return empty string.
        /// In case when InstrumentationKey is available return normalized key + dot ('.')
        /// as a separator between instrumentation key part and telemetry name part.
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Implementation expects lower case")]
        private static string NormalizeInstrumentationKey(string instrumentationKey)
        {
            if (instrumentationKey.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            return instrumentationKey.Replace("-", string.Empty).ToLowerInvariant() + ".";
        }
    }
}
