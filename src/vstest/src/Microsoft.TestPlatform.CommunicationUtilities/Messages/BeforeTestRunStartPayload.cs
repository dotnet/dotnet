// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The before test run start payload
/// </summary>
public class BeforeTestRunStartPayload
{
    /// <summary>
    /// Gets or sets run settings xml.
    /// </summary>
    public string? SettingsXml { get; set; }

    /// <summary>
    /// Gets or sets list of test sources.
    /// </summary>
    public IEnumerable<string>? Sources { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether telemetry is enabled.
    /// </summary>
    public bool IsTelemetryOptedIn { get; set; }
}
