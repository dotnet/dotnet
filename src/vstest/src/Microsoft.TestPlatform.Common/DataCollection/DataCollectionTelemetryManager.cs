// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// Stores and provides telemetry information for data collection.
/// </summary>
internal class DataCollectionTelemetryManager : IDataCollectionTelemetryManager
{
    private const string CorProfilerVariable = "COR_PROFILER";
    private const string CoreClrProfilerVariable = "CORECLR_PROFILER";
    private const string ClrIeInstrumentationMethodConfigurationPrefix32Variable = "MicrosoftInstrumentationEngine_ConfigPath32_";
    private const string ClrIeInstrumentationMethodConfigurationPrefix64Variable = "MicrosoftInstrumentationEngine_ConfigPath64_";

    private static readonly Guid ClrIeProfilerGuid = new("{324f817a-7420-4e6d-b3c1-143fbed6d855}");
    private const string OverwrittenProfilerName = "overwritten";

    private readonly IRequestData _requestData;

    internal DataCollectionTelemetryManager(IRequestData requestData)
    {
        _requestData = requestData;
    }

    /// <inheritdoc/>
    public void RecordEnvironmentVariableAddition(DataCollectorInformation dataCollectorInformation, string name, string value)
    {
        RecordProfilerMetricForNewVariable(CorProfilerVariable, TelemetryDataConstants.DataCollectorsCorProfiler, dataCollectorInformation, name, value);
        RecordProfilerMetricForNewVariable(CoreClrProfilerVariable, TelemetryDataConstants.DataCollectorsCoreClrProfiler, dataCollectorInformation, name, value);
    }

    /// <inheritdoc/>
    public void RecordEnvironmentVariableConflict(DataCollectorInformation dataCollectorInformation, string name, string value, string existingValue)
    {
        RecordProfilerMetricForConflictedVariable(CorProfilerVariable, TelemetryDataConstants.DataCollectorsCorProfiler, dataCollectorInformation, name, value, existingValue);
        RecordProfilerMetricForConflictedVariable(CoreClrProfilerVariable, TelemetryDataConstants.DataCollectorsCoreClrProfiler, dataCollectorInformation, name, value, existingValue);
    }

    private void RecordProfilerMetricForNewVariable(string profilerVariable, string telemetryPrefix, DataCollectorInformation dataCollectorInformation, string name, string value)
    {
        if (!string.Equals(profilerVariable, name, StringComparison.Ordinal))
        {
            return;
        }

        _requestData.MetricsCollection.Add(GetTelemetryKey(telemetryPrefix, dataCollectorInformation), GetProfilerGuid(value).ToString());
    }

    private void RecordProfilerMetricForConflictedVariable(string profilerVariable, string telemetryPrefix, DataCollectorInformation dataCollectorInformation, string name, string value, string existingValue)
    {
        // If data collector is requesting same profiler record it same as new
        if (string.Equals(value, existingValue, StringComparison.OrdinalIgnoreCase))
        {
            RecordProfilerMetricForNewVariable(profilerVariable, telemetryPrefix, dataCollectorInformation, name, value);
            return;
        }

        if (!string.Equals(profilerVariable, name, StringComparison.Ordinal))
        {
            return;
        }

        var existingProfilerGuid = GetProfilerGuid(existingValue);

        if (ClrIeProfilerGuid == existingProfilerGuid)
        {
            if (dataCollectorInformation.TestExecutionEnvironmentVariables != null &&
                dataCollectorInformation.TestExecutionEnvironmentVariables.Any(pair => pair.Key.StartsWith(ClrIeInstrumentationMethodConfigurationPrefix32Variable)) &&
                dataCollectorInformation.TestExecutionEnvironmentVariables.Any(pair => pair.Key.StartsWith(ClrIeInstrumentationMethodConfigurationPrefix64Variable)))
            {
                _requestData.MetricsCollection.Add(GetTelemetryKey(telemetryPrefix, dataCollectorInformation), ClrIeProfilerGuid.ToString());
                return;
            }
        }

        _requestData.MetricsCollection.Add(GetTelemetryKey(telemetryPrefix, dataCollectorInformation), $"{existingProfilerGuid}({OverwrittenProfilerName}:{GetProfilerGuid(value)})");
    }

    private static Guid GetProfilerGuid(string profilerGuid)
    {
        return Guid.TryParse(profilerGuid, out var guid) ? guid : Guid.Empty;
    }

    private static string GetTelemetryKey(string telemetryPrefix, DataCollectorInformation dataCollectorInformation)
    {
        return $"{telemetryPrefix}.{dataCollectorInformation.DataCollectorConfig?.TypeUri}";
    }
}
