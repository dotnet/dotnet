// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Interface defining the parallel discovery manager
/// </summary>
public interface IParallelProxyDiscoveryManager : IProxyDiscoveryManager
{
    /// <summary>
    /// Indicates if user requested an abortion
    /// </summary>
    bool IsAbortRequested { get; }

    /// <summary>
    /// Handles Partial Discovery Complete event coming from a specific concurrent proxy discovery manager
    /// Each concurrent proxy discovery manager will signal the parallel discovery manager when its complete
    /// </summary>
    /// <param name="proxyDiscoveryManager">discovery manager instance</param>
    /// <param name="totalTests">Total Tests discovered for the concurrent discovery</param>
    /// <param name="lastChunk">LastChunk testcases for the concurrent discovery</param>
    /// <param name="isAborted">True if discovery is aborted</param>
    /// <returns>True if parallel discovery is complete</returns>
    bool HandlePartialDiscoveryComplete(
        IProxyDiscoveryManager proxyDiscoveryManager,
        long totalTests,
        IEnumerable<TestCase>? lastChunk,
        bool isAborted);
}
