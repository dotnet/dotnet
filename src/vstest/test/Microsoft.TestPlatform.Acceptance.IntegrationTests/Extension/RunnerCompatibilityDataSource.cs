// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// A data source that provides every version of runner.
/// 
/// When that adds up to no configuration exception is thrown.
/// </summary>
public class RunnerCompatibilityDataSource : TestDataSourceAttribute<RunnerInfo>
{
    private readonly CompatibilityRowsBuilder _builder;

    public RunnerCompatibilityDataSource(
        string runnerFrameworks = AcceptanceTestBase.DEFAULT_RUNNER_NETFX_AND_NET,
        string runnerVersions = AcceptanceTestBase.LATEST_TO_LEGACY,
        string hostFrameworks = AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET)
    {
        // TODO: We actually don't generate values to use different translation layers, because we don't have a good way to do
        // that right now. Translation layer is loaded directly into the acceptance test, and so we don't have easy way to substitute it.

        _builder = new CompatibilityRowsBuilder(
            runnerFrameworks,
            runnerVersions,
            hostFrameworks,
            // host versions
            AcceptanceTestBase.LATEST,
            // adapter versions
            AcceptanceTestBase.LATESTSTABLE,
            // adapters
            AcceptanceTestBase.MSTEST);

        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }
    public int JustRow { get; set; } = -1;

    /// <summary>
    /// Add run for in-process using the selected .NET Framework runners, and and all selected adapters.
    /// </summary>
    public bool InProcess { get; set; }

    public string? BeforeFeature { get; set; }
    public string? AfterFeature { get; set; }

    //public string? BeforeTestHostFeature { get; set; }
    //public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithEveryVersionOfRunner = true;
        _builder.WithEveryVersionOfHost = false;
        _builder.WithEveryVersionOfAdapter = false;
        _builder.WithOlderConfigurations = false;
        _builder.WithInProcess = InProcess;

        _builder.BeforeRunnerFeature = BeforeFeature;
        _builder.AfterRunnerFeature = AfterFeature;

        //_builder.BeforeTestHostFeature = BeforeTestHostFeature;
        //_builder.AfterTestHostFeature = AfterTestHostFeature;

        _builder.BeforeAdapterFeature = BeforeAdapterFeature;
        _builder.AfterAdapterFeature = AfterAdapterFeature;

        _builder.DebugDataCollector = DebugDataCollector;
        _builder.DebugVSTestConsole = DebugVSTestConsole;
        _builder.DebugTestHost = DebugTestHost;
        _builder.DebugStopAtEntrypoint = DebugStopAtEntrypoint;

        _builder.JustRow = JustRow < 0 ? null : JustRow;

        var data = _builder.CreateData();
        data.ForEach(AddData);
    }
}

