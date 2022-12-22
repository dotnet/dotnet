// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Discovery;

[TestClass]
public class DiscoveryResultCacheTests
{
    [TestMethod]
    public void DiscoveryResultCacheConstructorShouldInitializeDiscoveredTestsList()
    {
        var cache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });

        Assert.IsNotNull(cache.Tests);
        Assert.AreEqual(0, cache.Tests.Count);
    }

    [TestMethod]
    public void DiscoveryResultCacheConstructorShouldInitializeTotalDiscoveredTests()
    {
        var cache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });

        Assert.AreEqual(0, cache.TotalDiscoveredTests);
    }

    [TestMethod]
    public void AddTestShouldAddATestCaseToTheList()
    {
        var cache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        cache.AddTest(testCase);

        Assert.AreEqual(1, cache.Tests.Count);
        Assert.AreEqual(testCase, cache.Tests[0]);
    }

    [TestMethod]
    public void AddTestShouldIncreaseDiscoveredTestsCount()
    {
        var cache = new DiscoveryResultCache(1000, TimeSpan.FromHours(1), (tests) => { });

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        cache.AddTest(testCase);

        Assert.AreEqual(1, cache.TotalDiscoveredTests);
    }

    [TestMethod]
    public void AddTestShouldReportTestCasesIfMaxCacheSizeIsMet()
    {
        ICollection<TestCase>? reportedTestCases = null;
        var cache = new DiscoveryResultCache(2, TimeSpan.FromHours(1), (tests) => reportedTestCases = tests);

        var testCase1 = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        var testCase2 = new TestCase("A.C.M2", new Uri("executor://unittest"), "A");
        cache.AddTest(testCase1);
        cache.AddTest(testCase2);

        Assert.IsNotNull(reportedTestCases);
        Assert.AreEqual(2, reportedTestCases.Count);
        CollectionAssert.AreEqual(new List<TestCase> { testCase1, testCase2 }, reportedTestCases.ToList());
    }

    [TestMethod]
    public void AddTestShouldResetTestListOnceCacheSizeIsMet()
    {
        var cache = new DiscoveryResultCache(2, TimeSpan.FromHours(1), (tests) => { });

        var testCase1 = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        var testCase2 = new TestCase("A.C.M2", new Uri("executor://unittest"), "A");
        cache.AddTest(testCase1);
        cache.AddTest(testCase2);

        Assert.IsNotNull(cache.Tests);
        Assert.AreEqual(0, cache.Tests.Count);

        // Validate that total tests is no reset though.
        Assert.AreEqual(2, cache.TotalDiscoveredTests);
    }

    [TestMethod]
    public void AddTestShouldReportTestCasesIfCacheTimeoutIsMet()
    {
        ICollection<TestCase>? reportedTestCases = null;
        var cache = new DiscoveryResultCache(100, TimeSpan.FromMilliseconds(10), (tests) => reportedTestCases = tests);

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        Task.Delay(20).Wait();
        cache.AddTest(testCase);

        Assert.IsNotNull(reportedTestCases);
        Assert.AreEqual(1, reportedTestCases.Count);
        Assert.AreEqual(testCase, reportedTestCases.ToArray()[0]);
    }

    [TestMethod]
    public void AddTestShouldResetTestListIfCacheTimeoutIsMet()
    {
        ICollection<TestCase>? reportedTestCases = null;
        var cache = new DiscoveryResultCache(100, TimeSpan.FromMilliseconds(10), (tests) => reportedTestCases = tests);

        var testCase = new TestCase("A.C.M", new Uri("executor://unittest"), "A");
        Task.Delay(20).Wait();
        cache.AddTest(testCase);

        Assert.IsNotNull(cache.Tests);
        Assert.AreEqual(0, cache.Tests.Count);

        // Validate that total tests is no reset though.
        Assert.AreEqual(1, cache.TotalDiscoveredTests);
    }
}
