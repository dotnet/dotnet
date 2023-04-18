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
/// The attribute defining runner framework and target framework for net462.
/// First Argument (Runner framework) = This decides who will run the tests. If runner framework is netcoreapp then "dotnet vstest.console.dll" will run the tests.
/// If runner framework is net46 then vstest.console.exe will run the tests.
/// Second argument (target framework) = The framework for which test will run
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NetFullTargetFrameworkDataSourceAttribute : Attribute, ITestDataSource
{
    private readonly bool _inIsolation;
    private readonly bool _inProcess;
    private readonly bool _useDesktopRunner;
    private readonly bool _useCoreRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetFullTargetFrameworkDataSourceAttribute"/> class.
    /// </summary>
    /// <param name="inIsolation">Run test in isolation</param>
    /// <param name="inProcess">Run tests in process</param>
    /// <param name="useDesktopRunner">To run tests with desktop runner(vstest.console.exe)</param>
    /// <param name="useCoreRunner">To run tests with core runner(dotnet vstest.console.dll)</param>
    public NetFullTargetFrameworkDataSourceAttribute(bool inIsolation = true, bool inProcess = false, bool useDesktopRunner = true, bool useCoreRunner = true)
    {
        _inIsolation = inIsolation;
        _inProcess = inProcess;
        _useDesktopRunner = useDesktopRunner;
        _useCoreRunner = useCoreRunner;
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var dataRows = new List<object[]>();
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (_useCoreRunner && isWindows)
        {
            var runnerInfo = new RunnerInfo
            {
                RunnerFramework = IntegrationTestBase.CoreRunnerFramework,
                TargetFramework = AcceptanceTestBase.DesktopTargetFramework,
                InIsolationValue = null
            };
            runnerInfo.DebugInfo = new DebugInfo
            {
                DebugVSTestConsole = DebugVSTestConsole,
                DebugTestHost = DebugTestHost,
                DebugDataCollector = DebugDataCollector,
                DebugStopAtEntrypoint = DebugStopAtEntrypoint,
            };
            dataRows.Add(new object[] { runnerInfo });
        }

        if (_useDesktopRunner && isWindows)
        {
            if (_inIsolation)
            {
                var runnerInfo = new RunnerInfo
                {
                    RunnerFramework = IntegrationTestBase.DesktopRunnerFramework,
                    TargetFramework = AcceptanceTestBase.DesktopTargetFramework,
                    InIsolationValue = AcceptanceTestBase.InIsolation
                };
                runnerInfo.DebugInfo = new DebugInfo
                {
                    DebugVSTestConsole = DebugVSTestConsole,
                    DebugTestHost = DebugTestHost,
                    DebugDataCollector = DebugDataCollector,
                    DebugStopAtEntrypoint = DebugStopAtEntrypoint,
                };
                dataRows.Add(new object[] { runnerInfo });
            }

            if (_inProcess)
            {
                var runnerInfo = new RunnerInfo
                {
                    RunnerFramework = IntegrationTestBase.DesktopRunnerFramework,
                    TargetFramework = AcceptanceTestBase.DesktopTargetFramework,
                    InIsolationValue = null
                };
                runnerInfo.DebugInfo = new DebugInfo
                {
                    DebugVSTestConsole = DebugVSTestConsole,
                    DebugTestHost = DebugTestHost,
                    DebugDataCollector = DebugDataCollector,
                    DebugStopAtEntrypoint = DebugStopAtEntrypoint,
                };
                dataRows.Add(new object[] { runnerInfo });
            }
        }

        return dataRows;
    }

    public string GetDisplayName(MethodInfo methodInfo, object[] data)
    {
        return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data));
    }
}
