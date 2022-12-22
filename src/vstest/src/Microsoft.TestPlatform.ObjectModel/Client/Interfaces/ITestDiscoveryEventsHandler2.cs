// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling discovery events during test discovery operation
/// </summary>
public interface ITestDiscoveryEventsHandler2 : ITestMessageEventHandler
{
    /// <summary>
    /// Dispatch DiscoveryComplete event to listeners.
    /// </summary>
    /// <param name="discoveryCompleteEventArgs">Discovery Complete Event Args</param>
    /// <param name="lastChunk">Last set of test cases discovered.</param>
    void HandleDiscoveryComplete(DiscoveryCompleteEventArgs discoveryCompleteEventArgs, IEnumerable<TestCase>? lastChunk);

    /// <summary>
    /// Dispatch DiscoveredTest event to listeners.
    /// </summary>
    /// <param name="discoveredTestCases">Discovered  test cases.</param>
    void HandleDiscoveredTests(IEnumerable<TestCase>? discoveredTestCases);
}
