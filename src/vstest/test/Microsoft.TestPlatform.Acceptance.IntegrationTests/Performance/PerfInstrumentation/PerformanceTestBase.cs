// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;
using System.Collections.Generic;

namespace Microsoft.TestPlatform.AcceptanceTests.Performance.PerfInstrumentation;

/// <summary>
/// The performance test base.
/// </summary>
public class PerformanceTestBase : AcceptanceTestBase
{
    private readonly PerfAnalyzer _perfAnalyzer;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceTestBase"/> class.
    /// </summary>
    public PerformanceTestBase()
        : base()
    {
#if NET
        throw new InvalidOperationException("Perf tests are not supported on .NET");
#else
        _perfAnalyzer = new PerfAnalyzer();
#endif
    }

    /// <summary>
    /// The run execution performance tests.
    /// </summary>
    /// <param name="testAsset">
    /// The test asset.
    /// </param>
    /// <param name="testAdapterPath">
    /// The test adapter path.
    /// </param>
    /// <param name="runSettings">
    /// The run settings.
    /// </param>
    public void RunExecutionPerformanceTests(string testAsset, string testAdapterPath, string runSettings)
    {
        using (_perfAnalyzer.Start())
        {
            InvokeVsTestForExecution(testAsset, testAdapterPath, framework: string.Empty, runSettings);
        }
    }

    /// <summary>
    /// The run discovery performance tests.
    /// </summary>
    /// <param name="testAsset">
    /// The test asset.
    /// </param>
    /// <param name="testAdapterPath">
    /// The test adapter path.
    /// </param>
    /// <param name="runSettings">
    /// The run settings.
    /// </param>
    public void RunDiscoveryPerformanceTests(string testAsset, string testAdapterPath, string runSettings)
    {
        using (_perfAnalyzer.Start())
        {
            InvokeVsTestForDiscovery(testAsset, testAdapterPath, runSettings);
        }
    }

    /// <summary>
    /// The get execution time.
    /// </summary>
    /// <returns>
    /// The <see cref="double"/>.
    /// </returns>
    public TimeSpan GetExecutionTime()
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetElapsedTimeByTaskName(Constants.ExecutionTask));
    }

    public TimeSpan GetDiscoveryTime()
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetElapsedTimeByTaskName(Constants.DiscoveryTask));
    }

    public TimeSpan GetVsTestTime()
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetElapsedTimeByTaskName(Constants.VsTestConsoleTask));
    }

    public TimeSpan GetTestHostTime()
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetElapsedTimeByTaskName(Constants.TestHostTask));
    }

    public TimeSpan GetAdapterSearchTime()
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetElapsedTimeByTaskName(Constants.AdapterSearchTask));
    }

    public IDictionary<string, string> GetDiscoveryData()
    {
        return _perfAnalyzer.GetEventDataByTaskName(Constants.AdapterDiscoveryTask);
    }

    public IDictionary<string, string> GetExecutionData()
    {
        return _perfAnalyzer.GetEventDataByTaskName(Constants.AdapterExecutionTask);
    }

    public TimeSpan GetAdapterExecutionTime(string executorUri)
    {
        return TimeSpan.FromMilliseconds(_perfAnalyzer.GetAdapterExecutionTime(executorUri));
    }
}
#endif
