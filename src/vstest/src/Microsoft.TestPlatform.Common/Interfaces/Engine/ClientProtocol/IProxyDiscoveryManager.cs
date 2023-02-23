// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Orchestrates discovery operations for the engine communicating with the client.
/// </summary>
public interface IProxyDiscoveryManager
{
    /// <summary>
    /// Initializes test discovery. Create the test host, setup channel and initialize extensions.
    /// <param name="skipDefaultAdapters">Skip default adapters flag.</param>
    /// </summary>
    void Initialize(bool skipDefaultAdapters);

    /// <summary>
    /// Initializes test discovery. Create the test host, setup channel and initialize extensions.
    /// </summary>
    /// 
    /// <param name="discoveryCriteria">Settings, parameters for the discovery request</param>
    /// <param name="eventHandler">EventHandler for handling discovery events from Engine</param>
    /// <param name="skipDefaultAdapters">Skip default adapters flag</param>
    void InitializeDiscovery(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler, bool skipDefaultAdapters);

    /// <summary>
    /// Discovers tests
    /// </summary>
    /// <param name="discoveryCriteria">Settings, parameters for the discovery request</param>
    /// <param name="eventHandler">EventHandler for handling discovery events from Engine</param>
    void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler);

    /// <summary>
    /// Aborts the test operation.
    /// </summary>
    void Abort();

    /// <summary>
    /// Aborts discovery operation with EventHandler.
    /// </summary>
    /// <param name="eventHandler">EventHandler for handling discovery events from Engine</param>
    void Abort(ITestDiscoveryEventsHandler2 eventHandler);

    /// <summary>
    /// Closes the current test operation.
    /// Send a EndSession message to close the test host and channel gracefully.
    /// </summary>
    void Close();
}
