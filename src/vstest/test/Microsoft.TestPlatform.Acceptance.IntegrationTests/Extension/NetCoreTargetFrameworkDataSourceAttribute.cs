// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// The attribute defining runner framework, target framework and target runtime for netcoreapp1.*
/// First Argument (Runner framework) = This decides who will run the tests. If runner framework is netcoreapp then "dotnet vstest.console.dll" will run the tests.
/// If runner framework is net46 then vstest.console.exe will run the tests.
/// Second argument (target framework) = The framework for which test will run
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NetCoreTargetFrameworkDataSourceAttribute : Attribute, ITestDataSource
{
    private readonly bool _useDesktopRunner;
    private readonly bool _useCoreRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetCoreTargetFrameworkDataSourceAttribute"/> class.
    /// </summary>
    /// <param name="useDesktopRunner">To run tests with desktop runner(vstest.console.exe)</param>
    /// <param name="useCoreRunner">To run tests with core runner(dotnet vstest.console.dll)</param>
    public NetCoreTargetFrameworkDataSourceAttribute(
        bool useDesktopRunner = true,
        bool useCoreRunner = true)
    {
        _useDesktopRunner = useDesktopRunner;
        _useCoreRunner = useCoreRunner;
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    private void AddRunnerDataRow(List<object[]> dataRows, string runnerFramework, string targetFramework)
    {
        var runnerInfo = new RunnerInfo
        {
            RunnerFramework = runnerFramework,
            TargetFramework = targetFramework,
            InIsolationValue = null
        };
        runnerInfo.DebugInfo = new DebugInfo
        {
            DebugDataCollector = DebugDataCollector,
            DebugTestHost = DebugTestHost,
            DebugVSTestConsole = DebugVSTestConsole,
            DebugStopAtEntrypoint = DebugStopAtEntrypoint,
        };
        dataRows.Add([runnerInfo]);
    }

    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var dataRows = new List<object[]>();
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (_useDesktopRunner && isWindows)
        {
            var runnerFramework = IntegrationTestBase.DesktopRunnerFramework;

            AddRunnerDataRow(dataRows, runnerFramework, AcceptanceTestBase.Core80TargetFramework);
        }

        if (_useCoreRunner)
        {
            var runnerFramework = IntegrationTestBase.CoreRunnerFramework;

            AddRunnerDataRow(dataRows, runnerFramework, AcceptanceTestBase.Core80TargetFramework);
        }

        return dataRows;
    }

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data ?? []));
    }
}
