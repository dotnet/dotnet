// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// A data source for checking compatibility of changes in testhost with different versions of vstest.console.
/// This provides the development version of testhost and pairs it with different versions of vstest.console, from Latest to RecentStable.
/// To give us confidence that we are not breaking customers with recent versions. We don't test with older versions of vstest.console,
/// because they are less likely to be used by customers that update to latest NET.Test.Sdk.
/// Because .NET Framework testhost is shipped together with VSTest console, we add only Latest - Latest for .NET Framework testhost.
/// </summary>
public class TestHostCompatibilityDataSource : CompatibilityDataSourceAttribute
{
    private readonly CompatibilityRowsBuilder _builder;

    public TestHostCompatibilityDataSource()
    {
        _builder = new CompatibilityRowsBuilder(
            // runner
            AcceptanceTestBase.LATEST_TO_RECENT_STABLE,
            AcceptanceTestBase.RUNNER_NETFX_AND_NET,
            // host
            AcceptanceTestBase.LATEST,
            AcceptanceTestBase.HOST_NETFX_AND_NET,
            // adapter
            AcceptanceTestBase.LATESTSTABLE,
            AcceptanceTestBase.MSTEST);

        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    public string? BeforeFeature { get; set; }
    public string? AfterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.BeforeTestHostFeature = BeforeFeature;
        _builder.AfterTestHostFeature = AfterFeature;

        _builder.DebugDataCollector = DebugDataCollector;
        _builder.DebugVSTestConsole = DebugVSTestConsole;
        _builder.DebugTestHost = DebugTestHost;
        _builder.DebugStopAtEntrypoint = DebugStopAtEntrypoint;

        var data = _builder.CreateData();
        data.ForEach(AddData);
    }
}
