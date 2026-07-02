// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DataCollectorTestsCoverlets : AcceptanceTestBase
{
    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void RunCoverletCoverage(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // We use netcoreapp runner
        // "...\vstest\tools\dotnet\dotnet.exe "...\vstest\artifacts\Debug\net11.0\vstest.console.dll" --collect:"XPlat Code Coverage" ...
        var resultsDir = new TempDirectory();

        string coverletAdapterPath = Path.GetDirectoryName(Directory.GetFiles(_testEnvironment.GetNugetPackage("coverlet.collector"), "coverlet.collector.dll", SearchOption.AllDirectories).Single())!;
        string assemblyPath = GetAssetFullPath("CoverletCoverageTestProject.dll").Trim('\"');
        string argument = $"--collect:{"XPlat Code Coverage".AddDoubleQuote()} {PrepareArguments(assemblyPath, coverletAdapterPath, "", FrameworkArgValue, resultsDirectory: resultsDir.Path)}";
        InvokeVsTest(argument);

        // Verify vstest.console.dll CollectArgumentProcessor fix codeBase for coverlet package
        // This assert check that we're sure that we've updated collector setting code base with full path,
        // otherwise without "custom coverlet code" inside ProxyExecutionManager coverlet dll won't be resolved inside testhost.
        var log = Directory.GetFiles(DiagLogsDirectory, "log.txt").Single();
        Assert.Contains("CoverletDataCollector in-process codeBase path", File.ReadAllText(log));

        // Verify out-of-proc coverlet collector load
        // Diag log naming: Path.ChangeExtension("log.txt", "datacollector.{ts}_{tid}.txt") produces "log.datacollector.*.txt"
        var dataCollectorLog = Directory.GetFiles(DiagLogsDirectory, "log.datacollector*").Single();
        Assert.Contains("[coverlet]Initializing CoverletCoverageDataCollector", File.ReadAllText(dataCollectorLog));

        // Verify in-proc coverlet collector load
        var hostLog = Directory.GetFiles(DiagLogsDirectory, "log.host*").Single();
        Assert.Contains("[coverlet]Initialize CoverletInProcDataCollector", File.ReadAllText(hostLog));

        // Verify default coverage file is generated
        StdOutputContains("coverage.cobertura.xml");
    }
}
