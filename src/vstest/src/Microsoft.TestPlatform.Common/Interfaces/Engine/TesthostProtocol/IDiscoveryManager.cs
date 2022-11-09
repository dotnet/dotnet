// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;

/// <summary>
/// Orchestrates discovery operations for the engine communicating with the test host process.
/// </summary>
public interface IDiscoveryManager
{
    /// <summary>
    /// Initializes the discovery manager.
    /// </summary>
    /// <param name="pathToAdditionalExtensions"> The path to additional extensions. </param>
    void Initialize(IEnumerable<string> pathToAdditionalExtensions, ITestDiscoveryEventsHandler2? eventHandler);

    /// <summary>
    /// Discovers tests
    /// </summary>
    /// <param name="discoveryCriteria">Settings, parameters for the discovery request</param>
    /// <param name="eventHandler">EventHandler for handling discovery events from Engine</param>
    void DiscoverTests(DiscoveryCriteria discoveryCriteria, ITestDiscoveryEventsHandler2 eventHandler);

    /// <summary>
    /// Aborts the test discovery.
    /// </summary>
    void Abort();

    /// <summary>
    /// Aborts the test discovery with eventHandler.
    /// </summary>
    /// <param name="eventHandler">EventHandler for handling discovery events from Engine</param>
    void Abort(ITestDiscoveryEventsHandler2 eventHandler);
}
