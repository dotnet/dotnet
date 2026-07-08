// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Runs the test across a matrix of <em>shipped</em> vstest.console / testhost / MSTest-adapter versions to guard
/// backward compatibility. The component named by the <c>scenario</c> is pinned to the locally-built bits;
/// the other dimensions sweep a range of released versions. Each row is handed to the test as a <see cref="RunnerInfo"/>.
/// Use the optional <c>Before*/After*Feature</c> properties to skip rows whose version predates / postdates a feature.
/// </summary>
public sealed class CompatibilityMatrixAttribute : CompatibilityDataSourceAttribute
{
    private readonly CompatibilityRowsBuilder _builder;
    private readonly bool _withInProcess;
    private readonly bool _withVsixRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompatibilityMatrixAttribute"/> class.
    /// </summary>
    /// <param name="scenario">Which component's local changes are under test — the others sweep their compatible range.</param>
    public CompatibilityMatrixAttribute(CompatScenario scenario)
    {
        (string runnerVersions, string runnerFrameworks, string hostVersions, string hostFrameworks, string adapterVersions, _withInProcess, _withVsixRunner) = scenario switch
        {
            // Locally-built vstest.console against the range of shipped testhost versions (adds in-process and VSIX console rows).
            CompatScenario.VSTestConsole => (
                AcceptanceTestBase.LATEST, AcceptanceTestBase.RUNNER_NETFX_AND_NET,
                AcceptanceTestBase.LATEST_TO_LEGACY, AcceptanceTestBase.HOST_NETFX_AND_NET,
                AcceptanceTestBase.LATESTSTABLE, true, true),

            // Locally-built testhost against recent shipped vstest.console versions.
            CompatScenario.TestHost => (
                AcceptanceTestBase.LATEST_TO_RECENT_STABLE, AcceptanceTestBase.RUNNER_NETFX_AND_NET,
                AcceptanceTestBase.LATEST, AcceptanceTestBase.HOST_NETFX_AND_NET,
                AcceptanceTestBase.LATESTSTABLE, false, false),

            // Locally-built VSTestConsoleWrapper against recent shipped vstest.console versions (adds VSIX, .NET testhost only).
            CompatScenario.Wrapper => (
                AcceptanceTestBase.LATEST_TO_RECENT_STABLE, AcceptanceTestBase.RUNNER_NETFX_AND_NET,
                AcceptanceTestBase.LATEST, AcceptanceTestBase.HOST_NET,
                AcceptanceTestBase.LATESTSTABLE, false, true),

            // Locally-built vstest.console + testhost against the range of shipped MSTest adapter versions.
            CompatScenario.Adapter => (
                AcceptanceTestBase.LATEST, AcceptanceTestBase.RUNNER_NET,
                AcceptanceTestBase.LATEST, AcceptanceTestBase.HOST_NETFX_AND_NET,
                AcceptanceTestBase.LATESTPREVIEW_TO_LEGACY, true, true),

            _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, null),
        };

        _builder = new CompatibilityRowsBuilder(
            runnerVersions, runnerFrameworks,
            hostVersions, hostFrameworks,
            adapterVersions, AcceptanceTestBase.MSTEST);

        // Do not generate the data rows here, properties (e.g. DebugVSTestConsole) are not populated until after constructor is done.
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }
    public int JustRow { get; set; } = -1;

    public string? BeforeVSTestConsoleFeature { get; set; }
    public string? AfterVSTestConsoleFeature { get; set; }

    public string? BeforeTestHostFeature { get; set; }
    public string? AfterTestHostFeature { get; set; }

    public string? BeforeAdapterFeature { get; set; }
    public string? AfterAdapterFeature { get; set; }

    public override void CreateData(MethodInfo methodInfo)
    {
        _builder.WithInProcess = _withInProcess;
        _builder.WithVSIXRunner = _withVsixRunner;

        _builder.BeforeRunnerFeature = BeforeVSTestConsoleFeature;
        _builder.AfterRunnerFeature = AfterVSTestConsoleFeature;

        _builder.BeforeTestHostFeature = BeforeTestHostFeature;
        _builder.AfterTestHostFeature = AfterTestHostFeature;

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

/// <summary>Picks which component's local changes <see cref="CompatibilityMatrixAttribute"/> tests for compatibility.</summary>
public enum CompatScenario
{
    /// <summary>Locally-built vstest.console against the range of shipped testhost versions (adds in-process and VSIX console rows).</summary>
    VSTestConsole,

    /// <summary>Locally-built testhost against recent shipped vstest.console versions.</summary>
    TestHost,

    /// <summary>Locally-built VSTestConsoleWrapper (translation layer) against recent shipped vstest.console versions (adds VSIX, .NET testhost only).</summary>
    Wrapper,

    /// <summary>Locally-built vstest.console + testhost against the range of shipped MSTest adapter versions.</summary>
    Adapter,
}
