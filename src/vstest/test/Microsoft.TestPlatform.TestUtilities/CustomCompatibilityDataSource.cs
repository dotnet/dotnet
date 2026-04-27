// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// A data source that provides a custom mix of runners, hosts and mstest adapter. You can control everything,
/// so be careful how many tests you generate. This is meant to be used for experimentation, and only when <see cref="WrapperCompatibilityDataSource"/>, <see cref="RunnerCompatibilityDataSource"/>, <see cref="TestHostCompatibilityDataSource"/> or <see cref="MSTestCompatibilityDataSource"/> do not fit the need.
/// </summary>
public class CustomCompatibilityDataSource : CompatibilityDataSourceAttribute
{
    private readonly CompatibilityRowsBuilder _builder;

    public CustomCompatibilityDataSource(
        string runnerFrameworks = AcceptanceTestBase.DEFAULT_RUNNER_NETFX_AND_NET,
        string runnerVersions = AcceptanceTestBase.LATEST_TO_LEGACY,
        string hostFrameworks = AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET,
        string hostVersions = AcceptanceTestBase.LATEST_TO_LEGACY,
        string adapterVersions = AcceptanceTestBase.LATESTPREVIEW_TO_LEGACY,
        string adapters = AcceptanceTestBase.MSTEST)
    {
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

    public bool WithVSIXRunner { get; set; } = true;

    public string? BeforeRunnerFeature { get; set; }
    public string? AfterRunnerFeature { get; set; }

    public string? BeforeTestHostFeature { get; set; }
    public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithVSIXRunner = WithVSIXRunner;
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
