// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace TestPlatform.CrossPlatEngine.UnitTests.TestableImplementations;

public sealed class TestableTestRunCache : ITestRunCache
{
    public TestableTestRunCache()
    {
        TestStartedList = new List<TestCase>();
        TestCompletedList = new List<TestCase>();
        TestResultList = new List<TestResult>();
        TestResults = null!;
        InProgressTests = null!;
        TestRunStatistics = null!;
    }

    // use the below three to fill in data to the testable cache.
    public List<TestCase> TestStartedList { get; private set; }

    public List<TestCase> TestCompletedList { get; private set; }

    public List<TestResult> TestResultList { get; private set; }

    public ICollection<TestCase> InProgressTests { get; set; }


    // Use the TestResultList instead to fill in data. This is just to avoid confusion.
    public ICollection<TestResult> TestResults { get; set; }

    public TestRunStatistics TestRunStatistics { get; set; }

    public long TotalExecutedTests { get; set; }

    public IDictionary<string, int> AdapterTelemetry => new Dictionary<string, int>();

    public ICollection<TestResult> GetLastChunk()
    {
        return TestResultList;
    }

    public void OnNewTestResult(TestResult testResult)
    {
        TestResultList.Add(testResult);
    }

    public bool OnTestCompletion(TestCase testCase)
    {
        TestCompletedList.Add(testCase);

        return false;
    }

    public void OnTestStarted(TestCase testCase)
    {
        TestStartedList.Add(testCase);
    }

    public void Dispose()
    {
    }
}
