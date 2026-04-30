// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// A data source that checks compatibility of changes in vstest.console with different versions of testhost. It also adds in-process mode and runner from VSIX.
/// We are testing with all versions of testhost, because we want to make sure that even project with very old Microsoft.NET.Test.Sdk is able to be opened and used
/// in Visual Studio.
/// We test with VSIX and in-process to avoid duplicating tests only because they need runner from a different place.
/// Use for testing changes specific to vstest.console. Or for interaction between runner and testhost.
/// 
/// When that adds up to no configuration exception is thrown.
/// </summary>
public class RunnerCompatibilityDataSource : CompatibilityDataSourceAttribute
{
    private readonly CompatibilityRowsBuilder _builder;

    public RunnerCompatibilityDataSource()
    {
        _builder = new CompatibilityRowsBuilder(
            // runner
            AcceptanceTestBase.LATEST,
            AcceptanceTestBase.DEFAULT_RUNNER_NETFX_AND_NET,
            // host
            AcceptanceTestBase.LATEST_TO_LEGACY,
            AcceptanceTestBase.DEFAULT_HOST_NETFX_AND_NET,
            // adapter
            AcceptanceTestBase.LATESTSTABLE,
            AcceptanceTestBase.MSTEST);

        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }
    public int JustRow { get; set; } = -1;

    public string? BeforeFeature { get; set; }
    public string? AfterFeature { get; set; }

    //public string? BeforeTestHostFeature { get; set; }
    //public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithVSIXRunner = true;
        _builder.WithInProcess = true;

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
