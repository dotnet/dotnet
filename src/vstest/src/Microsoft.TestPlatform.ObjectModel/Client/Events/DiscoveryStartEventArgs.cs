// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Event arguments used when test discovery starts
/// </summary>
public class DiscoveryStartEventArgs : EventArgs
{
    /// <summary>
    /// Constructor for creating event args object
    /// </summary>
    /// <param name="discoveryCriteria"> Discovery criteria to be used for test discovery. </param>
    public DiscoveryStartEventArgs(DiscoveryCriteria discoveryCriteria)
    {
        DiscoveryCriteria = discoveryCriteria;
    }

    /// <summary>
    /// Discovery criteria to be used for test discovery
    /// </summary>
    public DiscoveryCriteria DiscoveryCriteria { get; private set; }
}
