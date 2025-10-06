// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Microsoft.TestPlatform.TestUtilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Runs tests using the dotnet vstest.console.dll built against .NET Core 3.1.
/// Provide a list of target frameworks to run the tests from given as a ';' separated list, or using a constant containing that range such as
/// AcceptanceTestBase.NETFX462_NET9 = "net462;net472;net48;net8.0;net9.0" to determine which target framework of the project
/// to test. The target project must list those TFMs in the TargetFrameworks property in csproj.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NetCoreRunnerAttribute : Attribute, ITestDataSource
{
    private readonly string _targetFrameworks;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetCoreRunnerAttribute"/> class.
    /// </summary>
    /// <param name="targetFrameworks">To run tests with desktop runner(vstest.console.exe), use AcceptanceTestBase.Net462TargetFramework or alike values.</param>
    public NetCoreRunnerAttribute(string targetFrameworks = AcceptanceTestBase.NETFX462_NET9)
    {
        _targetFrameworks = targetFrameworks;
    }

    public bool DebugVSTestConsole { get; set; }
    public bool DebugTestHost { get; set; }
    public bool DebugDataCollector { get; set; }
    public bool DebugStopAtEntrypoint { get; set; }

    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var dataRows = new List<object[]>();
        var isWindows = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        // on non-windows we want to filter down only to netcoreapp runner, and net5.0 and newer.
        Func<string, bool> filter = tfm => isWindows || !tfm.StartsWith("net4");
        foreach (var fmw in _targetFrameworks.Split(';').Where(filter))
        {
            var runnerInfo = new RunnerInfo
            {
                RunnerFramework = IntegrationTestBase.CoreRunnerFramework,
                TargetFramework = fmw,
                InIsolationValue = null,
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
