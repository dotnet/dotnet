// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// IDiscoverTestsRequest returned after calling GetDiscoveredTestsAsync
/// </summary>
public interface IDiscoveryRequest : IRequest
{
    /// <summary>
    /// Handler for notifying discovery process is started
    /// </summary>
    event EventHandler<DiscoveryStartEventArgs>? OnDiscoveryStart;

    /// <summary>
    /// Handler for notifying discovery process is complete
    /// </summary>
    event EventHandler<DiscoveryCompleteEventArgs>? OnDiscoveryComplete;

    /// <summary>
    /// Handler for notifying when newly found tests are available for UI to fetch.
    /// </summary>
    event EventHandler<DiscoveredTestsEventArgs>? OnDiscoveredTests;

    /// <summary>
    /// Handler for receiving error during fetching/execution. This is used for when abnormal error
    /// occurs; equivalent of IRunMessageLogger in the current RockSteady core
    /// </summary>
    event EventHandler<TestRunMessageEventArgs>? OnDiscoveryMessage;

    /// <summary>
    /// Gets the discovery criterion.
    /// </summary>
    DiscoveryCriteria DiscoveryCriteria
    {
        get;
    }

    /// <summary>
    /// Starts tests discovery async.
    /// </summary>
    void DiscoverAsync();

    /// <summary>
    /// Aborts the discovery request
    /// </summary>
    void Abort();
}
