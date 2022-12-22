// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Discovery;

[TestClass]
public class TestCaseDiscoverySinkTests
{
    [TestMethod]
    public void SendTestCaseShouldNotThrowIfCacheIsNull()
    {
        var sink = new TestCaseDiscoverySink(null);

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");

        // This should not throw.
        sink.SendTestCase(testCase);
    }

    [TestMethod]
    public void SendTestCaseShouldSendTestCasesToCache()
    {
        var cache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });
        var sink = new TestCaseDiscoverySink(cache);

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");

        sink.SendTestCase(testCase);

        // Assert that the cache has the test case.
        Assert.IsNotNull(cache.Tests);
        Assert.AreEqual(1, cache.Tests.Count);
        Assert.AreEqual(testCase, cache.Tests[0]);
    }
}
