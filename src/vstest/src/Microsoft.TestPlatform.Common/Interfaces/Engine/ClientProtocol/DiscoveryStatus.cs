// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Enums for indicating discovery status of source
/// </summary>
public enum DiscoveryStatus
{
    /// <summary>
    /// Indicates the sources which were not touched during discovery.
    /// </summary>
    NotDiscovered,

    /// <summary>
    /// Indicates that we started discovery of the source but something happened (cancel/abort) and we stopped processing it.
    /// </summary>
    PartiallyDiscovered,

    /// <summary>
    /// Indicates that source was fully discovered.
    /// </summary>
    FullyDiscovered,

    /// <summary>
    /// Indicates that source was skipped in discovery.
    /// </summary>
    SkippedDiscovery
}
