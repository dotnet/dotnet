// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace NonDll.TestAdapter;
[FileExtension(".js")]
[DefaultExecutorUri(Uri)]
[ExtensionUri(Uri)]
public class TestAdapter : ITestExecutor, ITestDiscoverer
{
    public const string Uri = "executor://nondll.testadapter";

    public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
        IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
    {
        var count = 1;
        foreach (var source in sources)
        {
            TestCase testCase = new()
            {
                Source = source,
                CodeFilePath = source,
                DisplayName = $"Test{count++}",
                ExecutorUri = new Uri(Uri),
            };
            discoverySink.SendTestCase(testCase);
        }
    }

    public void Cancel()
    {
    }

    public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        foreach (var test in tests)
        {
            TestResult testResult = new(test)
            {
                Outcome = TestOutcome.Passed,
            };
            frameworkHandle.RecordResult(testResult);
        }
    }

    public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
    {
        var count = 1;
        foreach (var source in sources)
        {
            TestCase testCase = new()
            {
                Source = source,
                CodeFilePath = source,
                DisplayName = $"Test{count++}",
                ExecutorUri = new Uri(Uri),
            };
            TestResult testResult = new(testCase)
            {
                Outcome = TestOutcome.Passed,
            };
            frameworkHandle.RecordResult(testResult);
        }
    }
}
