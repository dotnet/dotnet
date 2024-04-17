namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class AtomicSampledItemsCounter
    {
        // Interlocked operations support an array indexer but not a dictionary property accessor
        private readonly long[] proactivelySampledOutItems = new long[] { 0, 0, 0, 0, 0, 0, 0 };
        private readonly Dictionary<SamplingTelemetryItemTypes, int> typeToSamplingIndexMap = new Dictionary<SamplingTelemetryItemTypes, int>
        {
            { SamplingTelemetryItemTypes.Request, 1 },
            { SamplingTelemetryItemTypes.RemoteDependency, 2 },
            { SamplingTelemetryItemTypes.Exception, 3 },
            { SamplingTelemetryItemTypes.Event, 4 },
            { SamplingTelemetryItemTypes.PageView, 5 },
            { SamplingTelemetryItemTypes.Message, 6 },
        };

        internal void AddItems(SamplingTelemetryItemTypes telemetryItemTypeFlag, long value)
        {
            // Returns known index in the counter array or default value 0 for unknown types
            this.typeToSamplingIndexMap.TryGetValue(telemetryItemTypeFlag, out int typeIndex);
            Interlocked.Add(ref this.proactivelySampledOutItems[typeIndex], value);
        }

        internal void ClearItems(SamplingTelemetryItemTypes telemetryItemTypeFlag)
        {
            // Returns known index in the counter array or default value 0 for unknown types
            this.typeToSamplingIndexMap.TryGetValue(telemetryItemTypeFlag, out int typeIndex);
            Interlocked.Exchange(ref this.proactivelySampledOutItems[typeIndex], 0);
        }

        internal long GetItems(SamplingTelemetryItemTypes telemetryItemTypeFlag)
        {
            // Returns known index in the counter array or default value 0 for unknown types
            this.typeToSamplingIndexMap.TryGetValue(telemetryItemTypeFlag, out int typeIndex);
            return Interlocked.Read(ref this.proactivelySampledOutItems[typeIndex]);
        }
    }
}
