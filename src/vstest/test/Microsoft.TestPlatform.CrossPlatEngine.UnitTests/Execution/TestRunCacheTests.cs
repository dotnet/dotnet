// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlatform.CrossPlatEngine.UnitTests.Execution;

[TestClass]
public class TestRunCacheBehaviors
{
    #region OnTestStarted tests

    [TestMethod]
    public void OnTestStartedShouldAddToInProgressTests()
    {
        var tester = new TestCacheTester { ExpectedCacheSize = int.MaxValue };

        var cache = new TestRunCache(int.MaxValue, TimeSpan.MaxValue, tester.CacheHitOnSize);

        var tr = GetTestResult(0);
        cache.OnTestStarted(tr.TestCase);

        CollectionAssert.Contains(cache.InProgressTests.ToList(), tr.TestCase);
    }

    [TestMethod]
    public void OnTestStartedShouldAddMultipleInProgressTestsTillCacheHit()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < (cacheSize - 1); i++)
        {
            var tr = GetTestResult(i);
            cache.OnTestStarted(tr.TestCase);

            Assert.AreEqual(i, cache.InProgressTests.Count - 1);
        }
    }

    //[TestMethod]
    //public void OnTestStartedShouldReportInProgressTestsForLongRunningUnitTest()
    //{
    //    var cacheTimeout = new TimeSpan(0, 0, 0, 3, 0);
    //    var tester = new TestCacheTester { ExpectedCacheSize = int.MaxValue };

    //    var cache = new TestRunCache(int.MaxValue, cacheTimeout, tester.CacheHitOnTimerLimit);

    //    var tr = this.GetTestResult(0);
    //    cache.OnTestStarted(tr.TestCase);

    //    Assert.AreEqual(0, tester.TotalInProgressTestsReceived);

    //    Assert.AreEqual(1, tester.TotalInProgressTestsReceived);
    //}

    [TestMethod]
    public void OnTestStartedShouldReportResultsOnCacheHit()
    {
        long cacheSize = 2;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < cacheSize; i++)
        {
            var tr = GetTestResult(i);
            cache.OnTestStarted(tr.TestCase);
        }

        Assert.AreEqual(1, tester.CacheHitCount);
        Assert.AreEqual(0, cache.TotalExecutedTests);
        Assert.AreEqual(0, cache.TestResults.Count);
        Assert.AreEqual(0, cache.InProgressTests.Count);
        Assert.AreEqual(2, tester.TotalInProgressTestsReceived);
    }

    #endregion

    #region OnNewTestResult tests

    [TestMethod]
    public void OnNewTestResultShouldAddToTotalExecutedTests()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(2, cache.TotalExecutedTests);
    }

    [TestMethod]
    public void OnNewTestResultShouldAddToTestResultCache()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
            CollectionAssert.Contains(cache.TestResults.ToList(), tr);
        }
    }

    [TestMethod]
    public void OnNewTestResultShouldUpdateRunStats()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            tr.Outcome = TestOutcome.Passed;
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(2, cache.TestRunStatistics.ExecutedTests);
        Assert.AreEqual(2, cache.TestRunStatistics.Stats![TestOutcome.Passed]);
    }

    [TestMethod]
    public void OnNewTestResultShouldRemoveTestCaseFromInProgressList()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            cache.OnTestStarted(tr.TestCase);
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(0, cache.InProgressTests.Count);
    }

    [TestMethod]
    public void OnNewTestResultShouldReportTestResultsWhenMaxCacheSizeIsHit()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < cacheSize; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(1, tester.CacheHitCount);
        Assert.AreEqual(cacheSize, cache.TotalExecutedTests);
        Assert.AreEqual(0, cache.TestResults.Count);
    }

    [TestMethod]
    public void OnNewTestResultShouldNotFireIfMaxCacheSizeIsNotHit()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        var executedTests = cacheSize - 1;
        for (var i = 0; i < executedTests; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(0, tester.CacheHitCount);
        Assert.AreEqual(executedTests, cache.TotalExecutedTests);
        Assert.AreEqual(executedTests, cache.TestResults.Count);
    }

    [TestMethod]
    public void OnNewTestResultShouldReportResultsMultipleTimes()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        long executedTests = 45;

        for (var i = 0; i < executedTests; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
        }

        Assert.AreEqual(4, tester.CacheHitCount);
        Assert.AreEqual(executedTests, cache.TotalExecutedTests);
        Assert.AreEqual(5, cache.TestResults.Count);
    }

    #endregion

    #region OnTestCompletion tests

    [TestMethod]
    public void OnTestCompletionShouldNotThrowIfCompletedTestIsNull()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);

        Assert.IsFalse(cache.OnTestCompletion(null));
    }

    [TestMethod]
    public void OnTestCompletionShouldReturnFalseIfInProgressTestsIsEmpty()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);

        Assert.IsFalse(cache.OnTestCompletion(GetTestResult(0).TestCase));
    }

    [TestMethod]
    public void OnTestCompletionShouldUpdateInProgressList()
    {
        long cacheSize = 2;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (int i = 0; i < cacheSize; i++)
        {
            var tr = GetTestResult(i);
            cache.OnTestStarted(tr.TestCase);
            Assert.IsTrue(cache.OnTestCompletion(tr.TestCase));

            Assert.AreEqual(0, cache.InProgressTests.Count);
        }
    }

    [TestMethod]
    public void OnTestCompletionShouldUpdateInProgressListWhenTestHasSameId()
    {
        long cacheSize = 2;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        for (var i = 0; i < cacheSize; i++)
        {
            var tr = GetTestResult(i);
            cache.OnTestStarted(tr.TestCase);

            var clone = new TestCase(
                tr.TestCase.FullyQualifiedName,
                tr.TestCase.ExecutorUri,
                tr.TestCase.Source);
            clone.Id = tr.TestCase.Id;

            Assert.IsTrue(cache.OnTestCompletion(clone));

            Assert.AreEqual(0, cache.InProgressTests.Count);
        }
    }

    [TestMethod]
    public void OnTestCompleteShouldNotRemoveTestCaseFromInProgressListForUnrelatedTestResult()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);

        var tr1 = GetTestResult(0);
        cache.OnTestStarted(tr1.TestCase);

        var tr2 = GetTestResult(1);
        Assert.IsFalse(cache.OnTestCompletion(tr2.TestCase));

        Assert.AreEqual(1, cache.InProgressTests.Count);
    }

    #endregion

    #region GetLastChunk tests

    [TestMethod]
    public void GetLastChunkShouldReturnTestResultsInCache()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);
        List<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult> pushedTestResults = new();

        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
            pushedTestResults.Add(tr);
        }

        var testResultsInCache = cache.GetLastChunk();
        CollectionAssert.AreEqual(pushedTestResults, testResultsInCache.ToList());
    }

    [TestMethod]
    public void GetLastChunkShouldResetTestResultsInCache()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);

        for (var i = 0; i < 2; i++)
        {
            var tr = GetTestResult(i);
            cache.OnNewTestResult(tr);
        }

        cache.GetLastChunk();
        Assert.AreEqual(0, cache.TestResults.Count);
    }

    #endregion

    #region TestRunStasts tests

    [TestMethod]
    public void TestRunStatsShouldReturnCurrentStats()
    {
        long cacheSize = 10;
        var tester = new TestCacheTester { ExpectedCacheSize = cacheSize };

        var cache = new TestRunCache(cacheSize, TimeSpan.MaxValue, tester.CacheHitOnSize);

        for (var i = 0; i < cacheSize; i++)
        {
            var tr = GetTestResult(i);
            tr.Outcome = i < 5 ? TestOutcome.Passed : TestOutcome.Failed;

            cache.OnNewTestResult(tr);
        }

        var stats = cache.TestRunStatistics;

        Assert.AreEqual(cacheSize, stats.ExecutedTests);
        Assert.AreEqual(5, stats.Stats![TestOutcome.Passed]);
        Assert.AreEqual(5, stats.Stats[TestOutcome.Failed]);
    }

    #endregion

    #region Helpers

    private static Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult GetTestResult(int index)
    {
        var tc = new TestCase("Test" + index, new Uri("executor://dummy"), "DummySourceFileName");
        var testResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(tc);
        testResult.TestCase.Id = Guid.NewGuid();

        return testResult;
    }

    private class TestCacheTester
    {
        public long ExpectedCacheSize { get; set; }

        public int CacheHitCount { get; set; }

        public int TotalInProgressTestsReceived { get; set; }

        public void CacheHitOnSize(TestRunStatistics _, ICollection<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult> results, ICollection<TestCase> tests)
        {
            Assert.AreEqual(ExpectedCacheSize, results.Count + tests.Count);
            CacheHitCount++;
            TotalInProgressTestsReceived += tests.Count;
        }

        public void CacheHitOnTimerLimit(ICollection<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult> _, ICollection<TestCase> tests)
        {
            CacheHitCount++;
            TotalInProgressTestsReceived += tests.Count;
        }
    }

    #endregion
}
