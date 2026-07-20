// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Runs the test once for every cell of the vstest.console × testhost matrix that the current OS supports,
/// using the locally-built bits. Each generated row is handed to the test as a <see cref="RunnerInfo"/>.
///
/// With no arguments the test runs the full matrix: both consoles — .NET Framework (<c>vstest.console.exe</c>)
/// and .NET (<c>dotnet vstest.console.dll</c>) — against both testhost target frameworks (net481 and net11.0).
/// Pin an axis to narrow it, e.g. <c>[TestMatrix(testHost: Net)]</c> keeps both consoles but only the
/// .NET testhost, and <c>[TestMatrix(console: Net)]</c> keeps both testhosts but only the .NET console.
///
/// <c>/InIsolation</c> is used only for the .NET Framework console driving a .NET Framework testhost; every other
/// cell runs in its natural mode. On non-Windows the .NET Framework console and net4* testhosts are skipped.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class TestMatrixAttribute : Attribute, ITestDataSource
{
    private readonly Target _console;
    private readonly Target _testHost;
    private readonly bool _inIsolation;
    private readonly bool _inProcess;
    private readonly bool _vsix;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestMatrixAttribute"/> class.
    /// </summary>
    /// <param name="console">Which vstest.console to run: both (default), only .NET Framework, or only .NET.</param>
    /// <param name="testHost">Which testhost target framework to run against: both (default), only net481, or only net11.0.</param>
    /// <param name="inIsolation">Emit the <c>/InIsolation</c> row for the .NET Framework console × .NET Framework testhost cell (default <see langword="true"/>). Ignored for every other cell.</param>
    /// <param name="inProcess">Additionally run the .NET Framework console × .NET Framework testhost cell in-process (without <c>/InIsolation</c>). Ignored for every other cell.</param>
    /// <param name="vsix">Additively run the vstest.console shipped in the Visual Studio VSIX as its own row (a .NET Framework console and testhost), independent of the <paramref name="console"/> and <paramref name="testHost"/> axes. Windows-only.</param>
    public TestMatrixAttribute(
        Target console = Target.Both,
        Target testHost = Target.Both,
        bool inIsolation = true,
        bool inProcess = false,
        bool vsix = false)
    {
        _console = console;
        _testHost = testHost;
        _inIsolation = inIsolation;
        _inProcess = inProcess;
        _vsix = vsix;
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var dataRows = new List<object[]>();
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");

        var wantNetFxConsole = _console is Target.Both or Target.NetFx;
        var wantNetConsole = _console is Target.Both or Target.Net;
        var wantNetFxHost = _testHost is Target.Both or Target.NetFx;
        var wantNetHost = _testHost is Target.Both or Target.Net;

        // .NET Framework testhost (net481) is Windows-only in its entirety. Emitted first so that for the
        // both-testhost matrix the rows are ordered net481 then net11.0, matching the legacy attributes.
        if (wantNetFxHost && isWindows)
        {
            if (wantNetConsole)
            {
                AddRow(dataRows, IntegrationTestBase.CoreRunnerFramework, AcceptanceTestBase.DesktopTargetFramework, inIsolationValue: null);
            }

            if (wantNetFxConsole)
            {
                if (_inIsolation)
                {
                    AddRow(dataRows, IntegrationTestBase.DesktopRunnerFramework, AcceptanceTestBase.DesktopTargetFramework, AcceptanceTestBase.InIsolation);
                }

                if (_inProcess)
                {
                    AddRow(dataRows, IntegrationTestBase.DesktopRunnerFramework, AcceptanceTestBase.DesktopTargetFramework, inIsolationValue: null);
                }
            }
        }

        // The VSIX console is a .NET Framework console driving a .NET Framework testhost. `vsix: true` is
        // additive: it always adds a VSIX run regardless of the console/testHost axes (Windows-only).
        if (_vsix && isWindows)
        {
            AddRow(dataRows, IntegrationTestBase.DesktopRunnerFramework, AcceptanceTestBase.DesktopTargetFramework, inIsolationValue: null, vsixConsole: true);
        }

        // .NET testhost (net11.0).
        if (wantNetHost)
        {
            // .NET Framework console driving a .NET testhost is Windows-only; never isolated.
            if (wantNetFxConsole && isWindows)
            {
                AddRow(dataRows, IntegrationTestBase.DesktopRunnerFramework, AcceptanceTestBase.Core11TargetFramework, inIsolationValue: null);
            }

            // .NET console driving a .NET testhost runs on every OS.
            if (wantNetConsole)
            {
                AddRow(dataRows, IntegrationTestBase.CoreRunnerFramework, AcceptanceTestBase.Core11TargetFramework, inIsolationValue: null);
            }
        }

        return dataRows;
    }

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data ?? []));
    }

    private void AddRow(List<object[]> dataRows, string runnerFramework, string targetFramework, string? inIsolationValue, bool vsixConsole = false)
    {
        var runnerInfo = new RunnerInfo
        {
            RunnerFramework = runnerFramework,
            TargetFramework = targetFramework,
            InIsolationValue = inIsolationValue,
            DebugInfo = new DebugInfo
            {
                DebugVSTestConsole = DebugVSTestConsole,
                DebugTestHost = DebugTestHost,
                DebugDataCollector = DebugDataCollector,
                DebugStopAtEntrypoint = DebugStopAtEntrypoint,
            },
        };

        if (vsixConsole)
        {
            runnerInfo.VSTestConsoleInfo = new VSTestConsoleInfo
            {
                Version = IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion,
                Path = Path.Combine(IntegrationTestEnvironment.PublishDirectory, Path.GetFileName(IntegrationTestEnvironment.LocalVsixInsertion), "vstest.console.exe"),
            };
        }

        dataRows.Add([runnerInfo]);
    }
}

/// <summary>
/// Selects a runtime family — .NET Framework or .NET — for an axis of <see cref="TestMatrixAttribute"/>.
/// Shared by the <c>console</c> axis (<c>vstest.console.exe</c> vs <c>dotnet vstest.console.dll</c>) and the
/// <c>testHost</c> axis (net481 vs net11.0); the parameter name selects which axis it applies to.
/// </summary>
public enum Target
{
    /// <summary>Run both the .NET Framework and .NET variants of the axis (default).</summary>
    Both,

    /// <summary>Run only the .NET Framework variant (<c>vstest.console.exe</c> / net481 testhost).</summary>
    NetFx,

    /// <summary>Run only the .NET variant (<c>dotnet vstest.console.dll</c> / net11.0 testhost).</summary>
    Net,
}
