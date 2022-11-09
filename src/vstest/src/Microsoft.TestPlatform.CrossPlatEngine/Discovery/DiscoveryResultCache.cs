// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;

/// <summary>
/// The discovery result cache.
/// </summary>
internal class DiscoveryResultCache
{
    /// <summary>
    /// Callback used when cache is full.
    /// </summary>
    private readonly OnReportTestCases _onReportTestCases;

    /// <summary>
    /// Max size of the test case buffer
    /// </summary>
    private readonly long _cacheSize;

    /// <summary>
    /// Timeout that triggers sending test cases regardless of cache size.
    /// </summary>
    private readonly TimeSpan _cacheTimeout;

    /// <summary>
    /// Last time test cases were sent.
    /// </summary>
    private DateTime _lastUpdate;

    /// <summary>
    /// Test case buffer
    /// </summary>
    private List<TestCase> _tests;

    /// <summary>
    /// Sync object
    /// </summary>
    private readonly object _syncObject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryResultCache"/> class.
    /// </summary>
    /// <param name="cacheSize"> The cache size. </param>
    /// <param name="discoveredTestEventTimeout"> The discovered test event timeout. </param>
    /// <param name="onReportTestCases"> The on report test cases. </param>
    public DiscoveryResultCache(long cacheSize, TimeSpan discoveredTestEventTimeout, OnReportTestCases onReportTestCases)
    {
        TPDebug.Assert(cacheSize > 0, "Buffer size cannot be less than zero");
        TPDebug.Assert(onReportTestCases != null, "Callback which listens for cache size limit cannot be null.");
        TPDebug.Assert(discoveredTestEventTimeout > TimeSpan.MinValue, "The cache timeout must be greater than min value.");

        _cacheSize = cacheSize;
        _onReportTestCases = onReportTestCases;
        _lastUpdate = DateTime.Now;
        _cacheTimeout = discoveredTestEventTimeout;

        _tests = new List<TestCase>();
        TotalDiscoveredTests = 0;
    }

    /// <summary>
    /// Called when the cache is ready to report some discovered test cases.
    /// </summary>
    public delegate void OnReportTestCases(ICollection<TestCase> tests);

    /// <summary>
    /// Gets the tests present in the cache currently
    /// </summary>
    public IList<TestCase> Tests
    {
        get
        {
            // This needs to new list to avoid concurrency issues.
            return new List<TestCase>(_tests);
        }
    }

    /// <summary>
    /// Gets the total discovered tests
    /// </summary>
    public long TotalDiscoveredTests { get; private set; }

    /// <summary>
    /// Adds a test to the cache.
    /// </summary>
    /// <param name="test"> The test. </param>
    public void AddTest(TestCase test)
    {
        TPDebug.Assert(test != null, "DiscoveryResultCache.AddTest called with no new test.");

        if (test == null)
        {
            EqtTrace.Warning("DiscoveryResultCache.AddTest: An attempt was made to add a 'null' test");
            return;
        }

        lock (_syncObject)
        {
            _tests.Add(test);
            TotalDiscoveredTests++;

            // Send test cases when the specified cache size has been reached or
            // after the specified cache timeout has been hit.
            var timeDelta = DateTime.Now - _lastUpdate;
            if (_tests.Count >= _cacheSize || (timeDelta > _cacheTimeout && _tests.Count > 0))
            {
                // Pass on the buffer to the listener and clear the old one
                _onReportTestCases(_tests);
                _tests = new List<TestCase>();
                _lastUpdate = DateTime.Now;

                EqtTrace.Verbose("DiscoveryResultCache.AddTest: Notified the onReportTestCases callback.");
            }
        }
    }
}
