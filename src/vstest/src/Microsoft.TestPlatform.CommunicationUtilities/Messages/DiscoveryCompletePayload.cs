// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

/// <summary>
/// The discovery complete payload.
/// </summary>
public class DiscoveryCompletePayload
{
    /// <summary>
    /// Gets or sets the total number of tests discovered.
    /// </summary>
    public long TotalTests { get; set; }

    /// <summary>
    /// Gets or sets the last chunk of discovered tests.
    /// </summary>
    public IEnumerable<TestCase>? LastDiscoveredTests { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether discovery was aborted.
    /// </summary>
    public bool IsAborted { get; set; }

    /// <summary>
    /// Gets or sets the Metrics
    /// </summary>
    public IDictionary<string, object>? Metrics { get; set; }

    /// <summary>
    /// Gets or sets list of sources which were fully discovered.
    /// </summary>
    public IList<string>? FullyDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets list of sources which were partially discovered (started discover tests, but then discovery aborted).
    /// </summary>
    public IList<string>? PartiallyDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets list of sources which were not discovered at all.
    /// </summary>
    public IList<string>? NotDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets list of sources which skipped in discovery on purpose, e.g. because they are known dlls that have no tests, or there is no runtime provider to run them.
    /// </summary>
    public IList<string>? SkippedDiscoverySources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of discovered extensions.
    /// </summary>
    public Dictionary<string, HashSet<string>>? DiscoveredExtensions { get; set; } = new();
}
