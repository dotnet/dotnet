// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;

/// <summary>
/// The test case discovery sink.
/// </summary>
internal class TestCaseDiscoverySink : ITestCaseDiscoverySink
{
    private readonly DiscoveryResultCache? _discoveryRequestCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestCaseDiscoverySink"/> class.
    /// </summary>
    /// <param name="discoveryRequestCache"> The discovery request cache. </param>
    internal TestCaseDiscoverySink(DiscoveryResultCache? discoveryRequestCache)
    {
        _discoveryRequestCache = discoveryRequestCache;
    }

    /// <summary>
    /// Sends the test case to the discovery cache.
    /// </summary>
    /// <param name="discoveredTest"> The discovered test. </param>
    public void SendTestCase(TestCase discoveredTest)
    {
        if (_discoveryRequestCache != null)
        {
            _discoveryRequestCache.AddTest(discoveredTest);
        }
    }
}
