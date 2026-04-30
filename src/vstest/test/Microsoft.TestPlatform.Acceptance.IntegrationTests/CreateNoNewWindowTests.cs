// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class CreateNoNewWindowTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: false)]
    public void WhenCreateNoNewWindowIsFalse_DiagShowsCreateNoWindowFalse(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");

        var runsettingsXml = "<RunSettings><RunConfiguration>"
            + "<CreateNoNewWindow>false</CreateNoNewWindow>"
            + "</RunConfiguration></RunSettings>";
        var runsettingsPath = GetRunsettingsFilePath(runsettingsXml);
        var diagLogPath = Path.Combine(TempDirectory.Path, "logs", "log.txt");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments += $" /settings:{runsettingsPath} /Diag:{diagLogPath}";

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("CreateNoWindow=False", diagLogs,
            "Expected 'CreateNoWindow=False' in diag logs when CreateNoNewWindow is set to false.");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: false)]
    public void WhenCreateNoNewWindowIsTrue_DiagShowsCreateNoWindowTrue(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");

        var runsettingsXml = "<RunSettings><RunConfiguration>"
            + "<CreateNoNewWindow>true</CreateNoNewWindow>"
            + "</RunConfiguration></RunSettings>";
        var runsettingsPath = GetRunsettingsFilePath(runsettingsXml);
        var diagLogPath = Path.Combine(TempDirectory.Path, "logs", "log.txt");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments += $" /settings:{runsettingsPath} /Diag:{diagLogPath}";

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("CreateNoWindow=True", diagLogs,
            "Expected 'CreateNoWindow=True' in diag logs when CreateNoNewWindow is set to true.");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: false)]
    public void WhenCreateNoNewWindowIsNotSet_DefaultIsTrue(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var diagLogPath = Path.Combine(TempDirectory.Path, "logs", "log.txt");

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        arguments += $" /Diag:{diagLogPath}";

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("CreateNoWindow=True", diagLogs,
            "Expected 'CreateNoWindow=True' in diag logs when CreateNoNewWindow is not set (default).");
    }

    private string GetRunsettingsFilePath(string runsettingsXml)
    {
        var runsettingsPath = Path.Combine(TempDirectory.Path, "test.runsettings");
        File.WriteAllText(runsettingsPath, runsettingsXml);
        return runsettingsPath;
    }

    private string GetDiagLogContents()
    {
        var logsDir = Path.Combine(TempDirectory.Path, "logs");
        if (!Directory.Exists(logsDir))
        {
            return string.Empty;
        }

        var logFiles = Directory.GetFiles(logsDir, "*.txt");

        return string.Join("\n", logFiles.Select(File.ReadAllText));
    }
}
