// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Publisher;

/// <summary>
/// This class will be initialized if Telemetry is opted out.
/// </summary>
public class TextFileTelemetryPublisher : IMetricsPublisher
{
    /// <summary>
    /// Publishes telemetry to a file.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="metrics"></param>
    public void PublishMetrics(string eventName, IDictionary<string, object?> metrics)
    {
        // Log to Text File
        LogToFile(eventName, metrics, new FileHelper());
    }

    /// <summary>
    /// Will do NO-OP
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // No operation
    }

    /// <summary>
    /// Log the telemetry to file.
    /// For Testing purposes.
    /// </summary>
    /// <param name="eventName">
    /// The event Name.
    /// </param>
    /// <param name="metrics">
    /// Metrics
    /// </param>
    /// <param name="fileHelper">
    /// The file Helper.
    /// </param>
    internal static void LogToFile(string eventName, IDictionary<string, object?> metrics, IFileHelper fileHelper)
    {
        string resultDirectory = Environment.GetEnvironmentVariable("VSTEST_LOGTELEMETRY_PATH")
            ?? Path.GetTempPath() + "TelemetryLogs";
        string resultFileName = Guid.NewGuid().ToString();
        string path = Path.Combine(resultDirectory, resultFileName);

        if (!fileHelper.DirectoryExists(resultDirectory))
        {
            fileHelper.CreateDirectory(resultDirectory);
        }

        var telemetryData = string.Join(";", metrics.Select(x => x.Key + "=" + x.Value));
        var finalData = string.Concat(eventName, ";", telemetryData);

        EqtTrace.Info("TextFileTelemetryPublisher.LogToFile: Logging telemetry data points to file {0}", path);

        fileHelper.WriteAllTextToFile(path, finalData);
    }
}
