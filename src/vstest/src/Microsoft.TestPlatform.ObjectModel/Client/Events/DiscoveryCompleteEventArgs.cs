// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Event arguments used on completion of discovery
/// </summary>
[DataContract]
public class DiscoveryCompleteEventArgs : EventArgs
{
    public DiscoveryCompleteEventArgs() { }

    /// <summary>
    /// Constructor for creating event args object
    /// </summary>
    /// <param name="totalTests">Total tests which got discovered</param>
    /// <param name="isAborted">Specifies if discovery has been aborted.</param>
    public DiscoveryCompleteEventArgs(long totalTests, bool isAborted)
    {
        TotalCount = totalTests;
        IsAborted = isAborted;
    }

    /// <summary>
    ///   Indicates the total tests which got discovered in this request.
    /// </summary>
    [DataMember]
    public long TotalCount { get; set; }

    /// <summary>
    /// Specifies if discovery has been aborted. If true TotalCount is also set to -1.
    /// </summary>
    [DataMember]
    public bool IsAborted { get; set; }

    /// <summary>
    /// Metrics
    /// </summary>
    [DataMember]
    public IDictionary<string, object>? Metrics { get; set; }

    /// <summary>
    /// Gets or sets the list of sources which were fully discovered.
    /// </summary>
    [DataMember]
    public IList<string>? FullyDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of sources which were partially discovered (started discover tests, but then discovery aborted).
    /// </summary>
    [DataMember]
    // Added in protocol version 6.
    public IList<string>? PartiallyDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    ///  Gets or sets the list of sources that were skipped during discovery.
    /// </summary>
    [DataMember]
    // Added in protocol version 7, for previous versions this is put into NotDiscoveredSources.
    public IList<string>? SkippedDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the list of sources which were not discovered at all.
    /// </summary>
    [DataMember]
    public IList<string>? NotDiscoveredSources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the collection of discovered extensions.
    /// </summary>
    [DataMember]
    public Dictionary<string, HashSet<string>>? DiscoveredExtensions { get; set; } = new();
}
