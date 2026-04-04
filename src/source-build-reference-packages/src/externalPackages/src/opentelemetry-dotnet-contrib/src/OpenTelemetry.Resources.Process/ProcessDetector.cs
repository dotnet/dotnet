// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

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
        return new Resource(new List<KeyValuePair<string, object>>(2)
        {
            new(ProcessSemanticConventions.AttributeProcessOwner, Environment.UserName),
#if NET
            new(ProcessSemanticConventions.AttributeProcessPid, Environment.ProcessId),
        });
#else
            new(ProcessSemanticConventions.AttributeProcessPid, GetProcessPid()),
        });
        static int GetProcessPid()
        {
            using var process = System.Diagnostics.Process.GetCurrentProcess();
            return process.Id;
        }
#endif
    }
}
