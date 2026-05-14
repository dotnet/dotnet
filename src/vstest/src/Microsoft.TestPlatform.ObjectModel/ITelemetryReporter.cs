// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Interface for extensions that choose to send telemetry events
/// </summary>
public interface ITelemetryReporter
{
    /// <summary>
    /// Pushes telemetry event into TP
    /// </summary>
    void Report(TelemetryEvent telemetryEvent);
}
