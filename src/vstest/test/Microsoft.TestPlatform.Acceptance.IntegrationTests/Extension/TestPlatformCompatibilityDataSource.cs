// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// A data source that provides a huge mix of runners, hosts and mstest adapter, to add up >100 tests.
/// You can control which runner versions, host versions, and adapter versions will be used. This should be
/// used only to test the most common scenarios, or special configurations that are candidates for their own
/// specialized source.
///
/// By default net462 and net8.0 are used for both runner and host. (4 combinations)
/// Then run with every version of runner is added.
/// Then run with every version of test.sdk is added.
/// Then run with every combination of testhost and adapter is added.
/// And then run in process is added.
///
/// All of those are filtered down to have no duplicates, and to pass the
/// Before and After platform version filters, and adapter filters.
///
/// When that adds up to no configuration exception is thrown.
/// </summary>
public class TestPlatformCompatibilityDataSource : TestDataSourceAttribute<RunnerInfo>
{
    private readonly CompatibilityRowsBuilder _builder;

    public TestPlatformCompatibilityDataSource(
        string runnerFrameworks = AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET,
        string runnerVersions = AcceptanceTestBase.LATEST_TO_LEGACY,
        string hostFrameworks = AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET,
        string hostVersions = AcceptanceTestBase.LATEST_TO_LEGACY,
        string adapterVersions = AcceptanceTestBase.LATESTPREVIEW_TO_LEGACY,
        string adapters = AcceptanceTestBase.MSTEST)
    {
        // TODO: We actually don't generate values to use different translation layers, because we don't have a good way to do
        // that right now. Translation layer is loaded directly into the acceptance test, and so we don't have easy way to substitute it.

        _builder = new CompatibilityRowsBuilder(runnerFrameworks, runnerVersions, hostFrameworks, hostVersions, adapterVersions, adapters);
        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    /// <summary>
    /// Add run for in-process using the selected .NET Framework runners, and and all selected adapters.
    /// </summary>
    public bool WithInProcess { get; set; } = true;

    public bool WithEveryVersionOfRunner { get; set; } = true;

    public bool WithEveryVersionOfHost { get; set; } = true;

    public bool WithEveryVersionOfAdapter { get; set; } = true;

    public bool WithOlderConfigurations { get; set; } = true;

    public string? BeforeRunnerFeature { get; set; }
    public string? AfterRunnerFeature { get; set; }

    public string? BeforeTestHostFeature { get; set; }
    public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithEveryVersionOfRunner = WithEveryVersionOfRunner;
        _builder.WithEveryVersionOfHost = WithEveryVersionOfHost;
        _builder.WithEveryVersionOfAdapter = WithEveryVersionOfAdapter;
        _builder.WithOlderConfigurations = WithOlderConfigurations;
        _builder.WithInProcess = WithInProcess;

        _builder.BeforeRunnerFeature = BeforeRunnerFeature;
        _builder.AfterRunnerFeature = AfterRunnerFeature;

        _builder.BeforeTestHostFeature = BeforeTestHostFeature;
        _builder.AfterTestHostFeature = AfterTestHostFeature;

        _builder.BeforeAdapterFeature = BeforeAdapterFeature;
        _builder.AfterAdapterFeature = AfterAdapterFeature;

        _builder.DebugDataCollector = DebugDataCollector;
        _builder.DebugVSTestConsole = DebugVSTestConsole;
        _builder.DebugTestHost = DebugTestHost;
        _builder.DebugStopAtEntrypoint = DebugStopAtEntrypoint;

        var data = _builder.CreateData();
        data.ForEach(AddData);
    }
}
