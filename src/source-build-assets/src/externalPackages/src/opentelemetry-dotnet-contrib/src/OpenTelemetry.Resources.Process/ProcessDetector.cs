// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Globalization;
using CurrentProcess = System.Diagnostics.Process;

namespace OpenTelemetry.Resources.Process;

/// <summary>
///     Process detector.
/// </summary>
internal sealed class ProcessDetector : IResourceDetector
{
    /// <summary>
    ///     Detects the resource attributes for process.
    /// </summary>
    /// <returns>Resource with key-value pairs of resource attributes.</returns>
    public Resource Detect()
    {
        GetProcessAttributes(
            out var processId,
            out var creationTime);

        var attributes = new List<KeyValuePair<string, object>>(3)
        {
            new(ProcessSemanticConventions.AttributeProcessOwner, Environment.UserName),
            new(ProcessSemanticConventions.AttributeProcessPid, processId),
        };

        if (creationTime is { } startTime)
        {
            attributes.Add(new(ProcessSemanticConventions.AttributeProcessCreationTime, startTime.ToString("O", CultureInfo.InvariantCulture)));
        }

        return new Resource(attributes);

        static void GetProcessAttributes(
            out int processId,
            out DateTime? creationTime)
        {
            using var process = CurrentProcess.GetCurrentProcess();
            processId = process.Id;

            creationTime = SafeGet(process, (p) => p.StartTime);

            static T? SafeGet<T>(CurrentProcess process, Func<CurrentProcess, T> getter)
            {
                try
                {
                    return getter(process);
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }
    }
}
