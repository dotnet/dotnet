// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

using Microsoft.Build.Utilities;
using Microsoft.TestPlatform.Build.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.UnitTests;

[TestClass]
public class TestTaskUtilsTests
{
    private readonly ITestTask _vsTestTask;

    public TestTaskUtilsTests()
    {
        _vsTestTask = new VSTestTask
        {
            BuildEngine = new FakeBuildEngine(),
            TestFileFullPath = new TaskItem(@"C:\path\to\test-assembly.dll"),
            VSTestFramework = ".NETCoreapp,Version2.0"
        };
    }

    [TestMethod]
    public void CreateArgumentShouldAddOneEntryForCLIRunSettings()
    {
        const string arg1 = "RunConfiguration.ResultsDirectory=Path having Space";
        const string arg2 = "MSTest.DeploymentEnabled";

        _vsTestTask.VSTestCLIRunSettings = new string[2];
        _vsTestTask.VSTestCLIRunSettings[0] = arg1;
        _vsTestTask.VSTestCLIRunSettings[1] = arg2;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains($"\"{arg1}\"", commandline);
        Assert.Contains($"{arg2}", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddCLIRunSettingsArgAtEnd()
    {
        const string codeCoverageOption = "Code Coverage";

        _vsTestTask.VSTestCollect = [codeCoverageOption];
        _vsTestTask.VSTestBlame = true;

        const string arg1 = "RunConfiguration.ResultsDirectory=Path having Space";
        const string arg2 = "MSTest.DeploymentEnabled";

        _vsTestTask.VSTestCLIRunSettings = new string[2];
        _vsTestTask.VSTestCLIRunSettings[0] = arg1;
        _vsTestTask.VSTestCLIRunSettings[1] = arg2;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains($"\"{arg1}\"", commandline);
        Assert.Contains($"{arg2}", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldPassResultsDirectoryCorrectly()
    {
        const string resultsDirectoryValue = @"C:\tmp\Results Directory";
        _vsTestTask.VSTestResultsDirectory = new TaskItem(resultsDirectoryValue);

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains($"--resultsDirectory:\"{_vsTestTask.VSTestResultsDirectory?.ItemSpec}\"", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldNotSetConsoleLoggerVerbosityIfConsoleLoggerIsGivenInArgs()
    {
        _vsTestTask.VSTestVerbosity = "diag";
        _vsTestTask.VSTestLogger = ["Console;Verbosity=quiet"];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.DoesNotMatchRegex(new Regex("(--logger:\"Console;Verbosity=normal\")"), commandline);
        Assert.Contains("--logger:\"Console;Verbosity=quiet\"", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsn()
    {
        _vsTestTask.VSTestVerbosity = "n";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsnormal()
    {
        _vsTestTask.VSTestVerbosity = "normal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsd()
    {
        _vsTestTask.VSTestVerbosity = "d";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdetailed()
    {
        _vsTestTask.VSTestVerbosity = "detailed";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdiag()
    {
        _vsTestTask.VSTestVerbosity = "diag";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdiagnostic()
    {
        _vsTestTask.VSTestVerbosity = "diagnostic";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsq()
    {
        _vsTestTask.VSTestVerbosity = "q";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=quiet", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsquiet()
    {
        _vsTestTask.VSTestVerbosity = "quiet";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=quiet", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToMinimalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsm()
    {
        _vsTestTask.VSTestVerbosity = "m";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=minimal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToMinimalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsminimal()
    {
        _vsTestTask.VSTestVerbosity = "minimal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=minimal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsNormalWithCapitalN()
    {
        _vsTestTask.VSTestVerbosity = "Normal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsQuietWithCapitalQ()
    {
        _vsTestTask.VSTestVerbosity = "Quiet";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:Console;Verbosity=quiet", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldPreserveWhiteSpaceInLogger()
    {
        _vsTestTask.VSTestLogger = ["trx;LogFileName=foo bar.trx"];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:\"trx;LogFileName=foo bar.trx\"", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddOneCollectArgumentForEachCollect()
    {
        _vsTestTask.VSTestCollect = new string[2];

        _vsTestTask.VSTestCollect[0] = "name1";
        _vsTestTask.VSTestCollect[1] = "name 2";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--collect:name1", commandline);
        Assert.Contains("--collect:\"name 2\"", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddMultipleTestAdapterPaths()
    {
        _vsTestTask.VSTestTestAdapterPath = [new TaskItem("path1"), new TaskItem("path2")];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--testAdapterPath:path1", commandline);
        Assert.Contains("--testAdapterPath:path2", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddMultipleLoggers()
    {
        _vsTestTask.VSTestLogger = ["trx;LogFileName=foo bar.trx", "console"];
        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--logger:\"trx;LogFileName=foo bar.trx\"", commandline);
        Assert.Contains("--logger:console", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddTraceCollectorDirectoryPathAsTestAdapterForCodeCoverageCollect()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedata collector";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestCollect = ["code coverage"];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string expectedArg = $"--testAdapterPath:\"{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}\"";
        Assert.Contains(expectedArg, commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldNotAddTraceCollectorDirectoryPathAsTestAdapterForNonCodeCoverageCollect()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedata collector";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestCollect = ["not code coverage"];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string notExpectedArg = $"--testAdapterPath:\"{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}\"";
        Assert.DoesNotMatchRegex(new Regex(Regex.Escape(notExpectedArg)), commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddTraceCollectorDirectoryPathAsTestAdapterIfSettingsGiven()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedatacollector\";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestSetting = @"c:\path\to\sample.runsettings";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string expectedArg = $"--testAdapterPath:{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}";
        Assert.Contains(expectedArg, commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldNotAddTestAdapterPathIfVSTestTraceDataCollectorDirectoryPathIsEmpty()
    {
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = null;
        _vsTestTask.VSTestSetting = @"c:\path\to\sample.runsettings";
        _vsTestTask.VSTestCollect = ["code coverage"];

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.DoesNotMatchRegex(new Regex(@"(--testAdapterPath:)"), commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldAddNoLogoOptionIfSpecifiedByUser()
    {
        _vsTestTask.VSTestNoLogo = true;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains("--nologo", commandline);
    }
}
