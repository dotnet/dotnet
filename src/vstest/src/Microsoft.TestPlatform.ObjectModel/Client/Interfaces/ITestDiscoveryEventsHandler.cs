// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling discovery events during test discovery operation
/// </summary>
public interface ITestDiscoveryEventsHandler : ITestMessageEventHandler
{
    /// <summary>
    /// Dispatch DiscoveryComplete event to listeners.
    /// </summary>
    /// <param name="totalTests">Total number of tests discovered.</param>
    /// <param name="lastChunk">Last set of test cases discovered.</param>
    /// <param name="isAborted">True if the discovery operation is aborted.</param>
    void HandleDiscoveryComplete(long totalTests, IEnumerable<TestCase>? lastChunk, bool isAborted);


    /// <summary>
    /// Dispatch DiscoveredTest event to listeners.
    /// </summary>
    /// <param name="discoveredTestCases">Discovered test cases.</param>
    void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases);
}
