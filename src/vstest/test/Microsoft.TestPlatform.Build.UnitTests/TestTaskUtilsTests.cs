// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

using Microsoft.Build.Framework;
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

        StringAssert.Contains(commandline, " -- ");
        StringAssert.Contains(commandline, $"\"{arg1}\"");
        StringAssert.Contains(commandline, $"{arg2}");
    }

    [TestMethod]
    public void CreateArgumentShouldAddCLIRunSettingsArgAtEnd()
    {
        const string codeCoverageOption = "Code Coverage";

        _vsTestTask.VSTestCollect = new string[] { codeCoverageOption };
        _vsTestTask.VSTestBlame = true;

        const string arg1 = "RunConfiguration.ResultsDirectory=Path having Space";
        const string arg2 = "MSTest.DeploymentEnabled";

        _vsTestTask.VSTestCLIRunSettings = new string[2];
        _vsTestTask.VSTestCLIRunSettings[0] = arg1;
        _vsTestTask.VSTestCLIRunSettings[1] = arg2;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, " -- ");
        StringAssert.Contains(commandline, $"\"{arg1}\"");
        StringAssert.Contains(commandline, $"{arg2}");
    }

    [TestMethod]
    public void CreateArgumentShouldPassResultsDirectoryCorrectly()
    {
        const string resultsDirectoryValue = @"C:\tmp\Results Directory";
        _vsTestTask.VSTestResultsDirectory = new TaskItem(resultsDirectoryValue);

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, $"--resultsDirectory:\"{_vsTestTask.VSTestResultsDirectory?.ItemSpec}\"");
    }

    [TestMethod]
    public void CreateArgumentShouldNotSetConsoleLoggerVerbosityIfConsoleLoggerIsGivenInArgs()
    {
        _vsTestTask.VSTestVerbosity = "diag";
        _vsTestTask.VSTestLogger = new string[] { "Console;Verbosity=quiet" };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.DoesNotMatch(commandline, new Regex("(--logger:\"Console;Verbosity=normal\")"));
        StringAssert.Contains(commandline, "--logger:\"Console;Verbosity=quiet\"");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsn()
    {
        _vsTestTask.VSTestVerbosity = "n";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsnormal()
    {
        _vsTestTask.VSTestVerbosity = "normal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsd()
    {
        _vsTestTask.VSTestVerbosity = "d";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdetailed()
    {
        _vsTestTask.VSTestVerbosity = "detailed";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdiag()
    {
        _vsTestTask.VSTestVerbosity = "diag";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsdiagnostic()
    {
        _vsTestTask.VSTestVerbosity = "diagnostic";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsq()
    {
        _vsTestTask.VSTestVerbosity = "q";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=quiet");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsquiet()
    {
        _vsTestTask.VSTestVerbosity = "quiet";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=quiet");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToMinimalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsm()
    {
        _vsTestTask.VSTestVerbosity = "m";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=minimal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToMinimalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsminimal()
    {
        _vsTestTask.VSTestVerbosity = "minimal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=minimal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToNormalIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsNormalWithCapitalN()
    {
        _vsTestTask.VSTestVerbosity = "Normal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=normal");
    }

    [TestMethod]
    public void CreateArgumentShouldSetConsoleLoggerVerbosityToQuietIfConsoleLoggerIsNotGivenInArgsAndVerbosityIsQuietWithCapitalQ()
    {
        _vsTestTask.VSTestVerbosity = "Quiet";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:Console;Verbosity=quiet");
    }

    [TestMethod]
    public void CreateArgumentShouldPreserveWhiteSpaceInLogger()
    {
        _vsTestTask.VSTestLogger = new string[] { "trx;LogFileName=foo bar.trx" };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:\"trx;LogFileName=foo bar.trx\"");
    }

    [TestMethod]
    public void CreateArgumentShouldAddOneCollectArgumentForEachCollect()
    {
        _vsTestTask.VSTestCollect = new string[2];

        _vsTestTask.VSTestCollect[0] = "name1";
        _vsTestTask.VSTestCollect[1] = "name 2";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--collect:name1");
        StringAssert.Contains(commandline, "--collect:\"name 2\"");
    }

    [TestMethod]
    public void CreateArgumentShouldAddMultipleTestAdapterPaths()
    {
        _vsTestTask.VSTestTestAdapterPath = new ITaskItem[] { new TaskItem("path1"), new TaskItem("path2") };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--testAdapterPath:path1");
        StringAssert.Contains(commandline, "--testAdapterPath:path2");
    }

    [TestMethod]
    public void CreateArgumentShouldAddMultipleLoggers()
    {
        _vsTestTask.VSTestLogger = new string[] { "trx;LogFileName=foo bar.trx", "console" };
        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--logger:\"trx;LogFileName=foo bar.trx\"");
        StringAssert.Contains(commandline, "--logger:console");
    }

    [TestMethod]
    public void CreateArgumentShouldAddTraceCollectorDirectoryPathAsTestAdapterForCodeCoverageCollect()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedata collector";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestCollect = new string[] { "code coverage" };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string expectedArg = $"--testAdapterPath:\"{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}\"";
        StringAssert.Contains(commandline, expectedArg);
    }

    [TestMethod]
    public void CreateArgumentShouldNotAddTraceCollectorDirectoryPathAsTestAdapterForNonCodeCoverageCollect()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedata collector";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestCollect = new string[] { "not code coverage" };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string notExpectedArg = $"--testAdapterPath:\"{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}\"";
        StringAssert.DoesNotMatch(commandline, new Regex(Regex.Escape(notExpectedArg)));
    }

    [TestMethod]
    public void CreateArgumentShouldAddTraceCollectorDirectoryPathAsTestAdapterIfSettingsGiven()
    {
        const string traceDataCollectorDirectoryPath = @"c:\path\to\tracedatacollector\";
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = new TaskItem(traceDataCollectorDirectoryPath);
        _vsTestTask.VSTestSetting = @"c:\path\to\sample.runsettings";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        string expectedArg = $"--testAdapterPath:{_vsTestTask.VSTestTraceDataCollectorDirectoryPath?.ItemSpec}";
        StringAssert.Contains(commandline, expectedArg);
    }

    [TestMethod]
    public void CreateArgumentShouldNotAddTestAdapterPathIfVSTestTraceDataCollectorDirectoryPathIsEmpty()
    {
        _vsTestTask.VSTestTraceDataCollectorDirectoryPath = null;
        _vsTestTask.VSTestSetting = @"c:\path\to\sample.runsettings";
        _vsTestTask.VSTestCollect = new string[] { "code coverage" };

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.DoesNotMatch(commandline, new Regex(@"(--testAdapterPath:)"));
    }

    [TestMethod]
    public void CreateArgumentShouldAddNoLogoOptionIfSpecifiedByUser()
    {
        _vsTestTask.VSTestNoLogo = true;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        StringAssert.Contains(commandline, "--nologo");
    }
}
