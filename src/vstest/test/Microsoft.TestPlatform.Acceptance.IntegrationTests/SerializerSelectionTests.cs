// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class SerializerSelectionTests : AcceptanceTestBase
{
    [TestMethod]
    [NetCoreRunner(Core11TargetFramework)]
    public void OnNetCoreRunner_ShouldUseSystemTextJson(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        var diagLogPath = Path.Combine(TempDirectory.Path, "logs", "log.txt");
        arguments = string.Concat(arguments, $" /Diag:{diagLogPath}");

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("Using System.Text.Json serializer", diagLogs,
            "Expected 'Using System.Text.Json serializer' in diag logs but not found.");
    }

    [TestMethod]
    [NetFrameworkRunner(Net481TargetFramework)]
    public void OnNetFrameworkRunner_ShouldUseJsonite(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);
        var diagLogPath = Path.Combine(TempDirectory.Path, "logs", "log.txt");
        arguments = string.Concat(arguments, $" /Diag:{diagLogPath}");

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("Using Jsonite serializer", diagLogs,
            "Expected 'Using Jsonite serializer' in diag logs but not found.");
    }

    /// <summary>
    /// Reads all diag log files from the test's temp directory and returns their combined content.
    /// </summary>
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
