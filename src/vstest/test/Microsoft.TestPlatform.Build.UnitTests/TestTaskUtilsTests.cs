// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
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

        _vsTestTask.VSTestCLIRunSettings = $"{arg1}\n{arg2}";

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

        _vsTestTask.VSTestCLIRunSettings = $"{arg1}\n{arg2}";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains($"\"{arg1}\"", commandline);
        Assert.Contains($"{arg2}", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldPreserveBackslashesInCLIRunSettings()
    {
        // Backslashes in CLI run settings (e.g. regex patterns on Unix) must not be converted to forward slashes.
        const string arg = @"NUnit.Where=namespace =~ /Abc\.Space1($|\.)/";

        _vsTestTask.VSTestCLIRunSettings = arg;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains(@"Abc\.Space1", commandline);
    }

    [TestMethod]
    [DataRow(typeof(VSTestTask))]
    [DataRow(typeof(VSTestTask2))]
    public void VSTestCLIRunSettingsMustBindAsStringToSurviveUnixPathNormalization(Type taskType)
    {
        // MSBuild expands an array task parameter into ITaskItem instances, and ITaskItem.ItemSpec
        // rewrites \ to / on Unix. That silently corrupted run settings containing backslashes, for
        // example regex patterns (https://github.com/microsoft/vstest/issues/15043). A scalar string
        // parameter is expanded as text and keeps the value intact, so the type must stay string.
        var property = taskType.GetProperty(nameof(ITestTask.VSTestCLIRunSettings));

        Assert.IsNotNull(property);
        Assert.AreEqual(typeof(string), property.PropertyType);
    }

    [TestMethod]
    public void CreateArgumentShouldSplitCLIRunSettingsOnSemicolon()
    {
        // dotnet test joins the arguments that follow "--" with a semicolon before it sets the
        // VSTestCLIRunSettings property, so semicolon separated input has to keep working.
        const string arg1 = "RunConfiguration.ResultsDirectory=Path having Space";
        const string arg2 = "MSTest.DeploymentEnabled";

        _vsTestTask.VSTestCLIRunSettings = $"{arg1};{arg2}";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains($"\"{arg1}\"", commandline);
        Assert.Contains($"{arg2}", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldNotKeepCarriageReturnWhenCLIRunSettingsAreSeparatedByCrLf()
    {
        const string arg1 = "MSTest.DeploymentEnabled";
        const string arg2 = "MSTest.MapInconclusiveToFailed";

        _vsTestTask.VSTestCLIRunSettings = $"{arg1}\r\n{arg2}";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.Contains(" -- ", commandline);
        Assert.Contains($" {arg1} ", commandline);
        Assert.Contains($" {arg2}", commandline);
        Assert.DoesNotContain("\r", commandline);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(";")]
    [DataRow("\n")]
    public void CreateArgumentShouldNotAppendSeparatorWhenCLIRunSettingsAreEmpty(string cliRunSettings)
    {
        _vsTestTask.VSTestCLIRunSettings = cliRunSettings;

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        Assert.DoesNotEndWith("--", commandline.TrimEnd(), $"Command line should not end with a lone '--'. Got: {commandline}");
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

    [TestMethod]
    public void CreateArgumentShouldNotInjectVerbosityWhenSettingsConfigureConsoleVerbosity()
    {
        var settingsFile = CreateRunSettings("""
            <RunSettings>
              <LoggerRunSettings>
                <Loggers>
                  <Logger friendlyName="console">
                    <Configuration>
                      <Verbosity>normal</Verbosity>
                    </Configuration>
                  </Logger>
                </Loggers>
              </LoggerRunSettings>
            </RunSettings>
            """);

        try
        {
            _vsTestTask.VSTestVerbosity = "minimal";
            _vsTestTask.VSTestSetting = settingsFile;

            var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

            Assert.DoesNotMatchRegex(new Regex("(--logger:Console;Verbosity=)"), commandline);
            Assert.Contains("--logger:Console", commandline);
        }
        finally
        {
            File.Delete(settingsFile);
        }
    }

    [TestMethod]
    public void CreateArgumentShouldNotInjectVerbosityWhenConfigurationElementCasingDiffers()
    {
        var settingsFile = CreateRunSettings("""
            <RunSettings>
              <LoggerRunSettings>
                <Loggers>
                  <Logger friendlyName="console">
                    <configuration>
                      <verbosity>normal</verbosity>
                    </configuration>
                  </Logger>
                </Loggers>
              </LoggerRunSettings>
            </RunSettings>
            """);

        try
        {
            _vsTestTask.VSTestVerbosity = "minimal";
            _vsTestTask.VSTestSetting = settingsFile;

            var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

            Assert.DoesNotMatchRegex(new Regex("(--logger:Console;Verbosity=)"), commandline);
            Assert.Contains("--logger:Console", commandline);
        }
        finally
        {
            File.Delete(settingsFile);
        }
    }

    [TestMethod]
    public void CreateArgumentShouldInjectVerbosityWhenVerbosityIsNotDirectlyUnderConfiguration()
    {
        // The console logger only reads Configuration/Verbosity. A Verbosity element that belongs
        // to some other block under Logger must not suppress the MSBuild-derived verbosity.
        var settingsFile = CreateRunSettings("""
            <RunSettings>
              <LoggerRunSettings>
                <Loggers>
                  <Logger friendlyName="console">
                    <PluginOptions>
                      <Verbosity>normal</Verbosity>
                    </PluginOptions>
                  </Logger>
                </Loggers>
              </LoggerRunSettings>
            </RunSettings>
            """);

        try
        {
            _vsTestTask.VSTestVerbosity = "quiet";
            _vsTestTask.VSTestSetting = settingsFile;

            var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

            Assert.Contains("--logger:Console;Verbosity=quiet", commandline);
        }
        finally
        {
            File.Delete(settingsFile);
        }
    }

    [TestMethod]
    public void CreateArgumentShouldInjectVerbosityWhenSettingsDoNotConfigureConsoleVerbosity()
    {
        var settingsFile = CreateRunSettings("""
            <RunSettings>
              <RunConfiguration>
                <MaxCpuCount>1</MaxCpuCount>
              </RunConfiguration>
              <MSTest>
                <Logger friendlyName="console">
                  <Verbosity>quiet</Verbosity>
                </Logger>
              </MSTest>
            </RunSettings>
            """);

        try
        {
            _vsTestTask.VSTestVerbosity = "normal";
            _vsTestTask.VSTestSetting = settingsFile;

            var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

            Assert.Contains("--logger:Console;Verbosity=normal", commandline);
        }
        finally
        {
            File.Delete(settingsFile);
        }
    }

    [TestMethod]
    public void CreateArgumentShouldLeaveInvalidSettingsValidationToVSTest()
    {
        var settingsFile = CreateRunSettings("<RunSettings>");

        try
        {
            _vsTestTask.VSTestVerbosity = "normal";
            _vsTestTask.VSTestSetting = settingsFile;

            var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

            Assert.Contains("--logger:Console;Verbosity=normal", commandline);
            Assert.Contains("--settings:", commandline);
            Assert.Contains(settingsFile, commandline);
        }
        finally
        {
            File.Delete(settingsFile);
        }
    }

    [TestMethod]
    public void CreateArgumentShouldInjectVerbosityWhenNoSettingsFileIsProvided()
    {
        _vsTestTask.VSTestVerbosity = "normal";

        var commandline = TestTaskUtils.CreateCommandLineArguments(_vsTestTask);

        // Without a settings file, verbosity is injected from MSBuild verbosity.
        Assert.Contains("--logger:Console;Verbosity=normal", commandline);
    }

    [TestMethod]
    public void CreateArgumentShouldInjectVerbosityForVSTestTask2EvenWhenSettingsFileIsProvided()
    {
        // VSTestTask2 uses MSBuildLogger whose verbosity is always driven by MSBuild, not by
        // the user's settings file. Even when a settings file is in use, MSBuildLogger must
        // receive the MSBuild-derived verbosity so it doesn't silently fall back to a default.
        ITestTask vsTestTask2 = new VSTestTask2
        {
            BuildEngine = new FakeBuildEngine(),
            TestFileFullPath = new TaskItem(@"C:\path\to\test-assembly.dll"),
            VSTestFramework = ".NETCoreapp,Version2.0",
            VSTestVerbosity = "normal",
            VSTestSetting = @"c:\path\to\sample.runsettings",
        };

        var commandline = TestTaskUtils.CreateCommandLineArguments(vsTestTask2);

        Assert.Contains("--logger:Microsoft.TestPlatform.MSBuildLogger;Verbosity=normal", commandline);
    }

    private static string CreateRunSettings(string contents)
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.runsettings");
        File.WriteAllText(path, contents);

        return path;
    }
}
