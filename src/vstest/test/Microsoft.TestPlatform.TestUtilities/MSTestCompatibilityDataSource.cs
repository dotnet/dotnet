// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Test compatibility of vstest.console and testhost with different MSTest versions.
/// </summary>
public class MSTestCompatibilityDataSource : CompatibilityDataSourceAttribute
{
    private readonly CompatibilityRowsBuilder _builder;

    public MSTestCompatibilityDataSource()
    {
        // 1 runner version and 1 testhost version
        // This tests different mstest versions against our latest runner and testhost.

        _builder = new CompatibilityRowsBuilder(
            // runner, use just .NET, because the adapter runs in the testhost, and we will add InProcess and VSIX, that will test running with the Runner.
            AcceptanceTestBase.LATEST,
            AcceptanceTestBase.DEFAULT_RUNNER_NET,
            // host
            AcceptanceTestBase.LATEST,
            AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET,
            // adapter
            AcceptanceTestBase.LATESTPREVIEW_TO_LEGACY,
            AcceptanceTestBase.MSTEST);

        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    public string? BeforeRunnerFeature { get; set; }
    public string? AfterRunnerFeature { get; set; }

    public string? BeforeTestHostFeature { get; set; }
    public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithInProcess = true;
        _builder.WithVSIXRunner = true;

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
