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
/// Runs tests using the dotnet vstest.console.dll built against .NET 6.0.
/// Provide a list of target frameworks to run the tests from given as a ';' separated list, or using a constant containing that range such as
/// AcceptanceTestBase.NETFX462_NET9 = "net462;net472;net48;net8.0;net9.0" to determine which target framework of the project
/// to test. The target project must list those TFMs in the TargetFrameworks property in csproj.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NetFrameworkRunnerAttribute : Attribute, ITestDataSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NetCoreTargetFrameworkDataSourceAttribute"/> class.
    /// </summary>
    /// <param name="targetFrameworks">To run tests with desktop runner(vstest.console.exe), use AcceptanceTestBase.Net462TargetFramework or alike values.</param>
    public NetFrameworkRunnerAttribute(string targetFrameworks = AcceptanceTestBase.NETFX462_NET9)
    {
        _targetFrameworks = targetFrameworks;
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    private readonly string _targetFrameworks;

    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var dataRows = new List<object[]>();
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        if (!isWindows)
        {
            return dataRows;
        }

        foreach (var fmw in _targetFrameworks.Split(';'))
        {
            var runnerInfo = new RunnerInfo
            {
                RunnerFramework = IntegrationTestBase.DesktopRunnerFramework,
                TargetFramework = fmw,
                InIsolationValue = AcceptanceTestBase.InIsolation
            };
            runnerInfo.DebugInfo = new DebugInfo
            {
                DebugVSTestConsole = DebugVSTestConsole,
                DebugTestHost = DebugTestHost,
                DebugDataCollector = DebugDataCollector,
                DebugStopAtEntrypoint = DebugStopAtEntrypoint,
            };

            dataRows.Add([runnerInfo]);
        }

        return dataRows;
    }

    public string GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data ?? []));
    }
}
