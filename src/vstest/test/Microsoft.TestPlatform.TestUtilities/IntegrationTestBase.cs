// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using FluentAssertions;

using Microsoft.TestPlatform.VsTestConsole.TranslationLayer;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Base class for integration tests.
/// </summary>
public class IntegrationTestBase
{
    public const string DesktopRunnerFramework = "net462";
    public const string CoreRunnerFramework = "netcoreapp3.1";

    private const string TotalTestsMessage = "Total tests: {0}";
    private const string PassedTestsMessage = " Passed: {0}";
    private const string FailedTestsMessage = " Failed: {0}";
    private const string SkippedTestsMessage = " Skipped: {0}";
    private const string TestSummaryStatusMessageFormat = "Total tests: {0} Passed: {1} Failed: {2} Skipped: {3}";
    private string _standardTestOutput = string.Empty;
    private string _standardTestError = string.Empty;
    private int _runnerExitCode = -1;

    private string? _arguments = string.Empty;

    protected readonly IntegrationTestEnvironment _testEnvironment;

    private readonly string _msTestPre3_0AdapterRelativePath = @"mstest.testadapter\{0}\build\_common".Replace('\\', Path.DirectorySeparatorChar);
    private readonly string _msTestAdapterRelativePath = @"mstest.testadapter\{0}\build\{1}".Replace('\\', Path.DirectorySeparatorChar);
    private readonly string _nUnitTestAdapterRelativePath = @"nunit3testadapter\{0}\build".Replace('\\', Path.DirectorySeparatorChar);
    private readonly string _xUnitTestAdapterRelativePath = @"xunit.runner.visualstudio\{0}\build\{1}".Replace('\\', Path.DirectorySeparatorChar);

    public enum UnitTestFramework
    {
        NUnit, XUnit, MSTest, CPP, NonDll
    }

    public IntegrationTestBase()
    {
        _testEnvironment = new IntegrationTestEnvironment();
        BuildConfiguration = IntegrationTestEnvironment.BuildConfiguration;
        TempDirectory = new TempDirectory();

        var drive = new DriveInfo(Directory.GetDirectoryRoot(TempDirectory.Path));
        Console.WriteLine($"Available space for TEMP: {drive.Name} {drive.AvailableFreeSpace / (1024 * 1024)} MB");

        IsCI = IntegrationTestEnvironment.IsCI;
    }

    public string StdOut => _standardTestOutput;
    public string StdOutWithWhiteSpace { get; private set; } = string.Empty;

    public string StdErr => _standardTestError;
    public string StdErrWithWhiteSpace { get; private set; } = string.Empty;

    public TempDirectory TempDirectory { get; }

    public TestContext? TestContext { get; set; }

    public string BuildConfiguration { get; }

    public bool IsCI { get; }

    [TestCleanup]
    public void TempDirectoryCleanup()
    {
        // In CI always delete the results, because we have limited disk space there.
        //
        // Locally delete the directory only when the test succeeded, so we can look
        // at results and logs of failed tests.
        if (IsCI || TestContext?.CurrentTestOutcome == UnitTestOutcome.Passed)
        {
            TempDirectory.Dispose();
        }
    }

    /// <summary>
    /// Prepare arguments for <c>vstest.console.exe</c>.
    /// </summary>
    /// <param name="testAssemblies">List of test assemblies.</param>
    /// <param name="testAdapterPath">Path to test adapter.</param>
    /// <param name="runSettings">Text of run settings.</param>
    /// <param name="framework">Framework to use.</param>
    /// <param name="inIsolation">If we should run in a separate process.</param>
    /// <param name="resultsDirectory">The directory where results are stored.</param>
    /// <returns>Command line arguments string.</returns>
    public static string PrepareArguments(string[] testAssemblies, string? testAdapterPath, string? runSettings,
        string framework, string? inIsolation = "", string? resultsDirectory = null)
    {
        var arguments = "";
        foreach (var path in testAssemblies)
        {
            // The incoming testAssembly path is either a single dll path in quotes or without quotes.
            // Or multiple assembly paths in a single string each double quoted and joined by space.
            // We trim, and add quotes here to get either:
            // C:\1.dll -> "C:\1.dll"
            // "C:\1.dll" -> "C:\1.dll"
            // "C:\1.dll" "C:\2.dll" -> "C:\1.dll" "C:\2.dll"
            //
            // For unquoted multi path string C:\1.dll C:\2.dll, we will get "C:\1.dll C:\2.dll"
            // which is wrong and will fail later, but it's the test's fault for doing it wrong
            // rather than providing an array of strings that this overload takes.
            arguments += path.Trim('\"').AddDoubleQuote() + " ";
        }

        arguments = arguments.Trim();

        if (!testAdapterPath.IsNullOrWhiteSpace())
        {
            // Append adapter path
            arguments = string.Concat(arguments, " /testadapterpath:", testAdapterPath.AddDoubleQuote());
        }

        if (!runSettings.IsNullOrWhiteSpace())
        {
            // Append run settings
            arguments = string.Concat(arguments, " /settings:", runSettings.AddDoubleQuote());
        }

        if (!framework.IsNullOrWhiteSpace())
        {
            // Append run settings
            arguments = string.Concat(arguments, " /framework:", framework.AddDoubleQuote());
        }

        arguments = string.Concat(arguments, " /logger:", "console;verbosity=normal".AddDoubleQuote());

        if (!inIsolation.IsNullOrWhiteSpace())
        {
            if (inIsolation != "/InIsolation")
            {
                // TODO: The whole inIsolation should be just a bool, but it is not, and it's changing in other PR.
                throw new InvalidOperationException("InIsolation value must be '/InIsolation'");
            }
            arguments = string.Concat(arguments, " ", inIsolation);
        }

        if (!resultsDirectory.IsNullOrWhiteSpace())
        {
            // Append results directory
            arguments = string.Concat(arguments, " /ResultsDirectory:", resultsDirectory.AddDoubleQuote());
        }

        return arguments;
    }

    /// <summary>
    /// Prepare arguments for <c>vstest.console.exe</c>.
    /// </summary>
    /// <param name="testAssembly">Name of the test assembly.</param>
    /// <param name="testAdapterPath">Path to test adapter.</param>
    /// <param name="runSettings">Text of run settings.</param>
    /// <param name="framework">The framework to use.</param>
    /// <param name="inIsolation">If we should run in separate process.</param>
    /// <param name="resultsDirectory">The directory in which results will be stored.</param>
    /// <returns>Command line arguments string.</returns>
    public static string PrepareArguments(string testAssembly, string? testAdapterPath, string? runSettings,
        string framework, string? inIsolation = "", string? resultsDirectory = null)
        => PrepareArguments([testAssembly], testAdapterPath, runSettings, framework, inIsolation, resultsDirectory);


    /// <summary>
    /// Invokes <c>vstest.console</c> with specified arguments.
    /// </summary>
    /// <param name="arguments">Arguments provided to <c>vstest.console</c>.exe</param>
    /// <param name="environmentVariables">Environment variables to set to the started process.</param>
    public void InvokeVsTest(string? arguments, Dictionary<string, string?>? environmentVariables = null)
    {
        var debugEnvironmentVariables = AddDebugEnvironmentVariables(environmentVariables);
        ExecuteVsTestConsole(arguments, out _standardTestOutput, out _standardTestError, out _runnerExitCode, debugEnvironmentVariables);
        FormatStandardOutCome();
    }

    /// <summary>
    /// Invokes our local copy of dotnet that is patched with artifacts from the build with specified arguments.
    /// </summary>
    /// <param name="arguments">Arguments provided to <c>vstest.console</c>.exe</param>
    /// <param name="environmentVariables">Environment variables to set to the started process.</param>
    public void InvokeDotnetTest(string arguments, Dictionary<string, string?>? environmentVariables = null)
    {
        var debugEnvironmentVariables = AddDebugEnvironmentVariables(environmentVariables);

        var vstestConsolePath = GetDotnetRunnerPath();

        if (arguments.Contains(".csproj"))
        {
            var consolePathParameter = $@" -p:VsTestConsolePath=""{vstestConsolePath}""";
            var position = arguments.IndexOf(" -- ");
            if (position == -1)
            {
                // Add at the end.
                arguments += consolePathParameter; ;
            }
            else
            {
                // Insert before inline runsettings.
                arguments = arguments.Insert(position, consolePathParameter);
            }
        }

        // This is used in dotnet/sdk to determine path to vstest.console:
        // https://github.com/dotnet/sdk/blob/main/src/Cli/dotnet/commands/dotnet-test/VSTestForwardingApp.cs#L30-L39
        debugEnvironmentVariables["VSTEST_CONSOLE_PATH"] = vstestConsolePath;

        IntegrationTestBase.ExecutePatchedDotnet("test", arguments, out _standardTestOutput, out _standardTestError, out _runnerExitCode, debugEnvironmentVariables);
        FormatStandardOutCome();
    }

    /// <summary>
    /// Invokes <c>vstest.console</c> to execute tests in a test assembly.
    /// </summary>
    /// <param name="testAssembly">A test assembly.</param>
    /// <param name="testAdapterPath">Path to test adapters.</param>
    /// <param name="framework">Dotnet Framework of test assembly.</param>
    /// <param name="runSettings">Run settings for execution.</param>
    /// <param name="environmentVariables">Environment variables to set to the started process.</param>
    public void InvokeVsTestForExecution(string testAssembly,
        string? testAdapterPath,
        string framework,
        string? runSettings = "",
        Dictionary<string, string?>? environmentVariables = null)
    {
        var arguments = PrepareArguments(testAssembly, testAdapterPath, runSettings, framework, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments, environmentVariables);
    }

    private Dictionary<string, string?> AddDebugEnvironmentVariables(Dictionary<string, string?>? environmentVariables)
    {
        environmentVariables ??= new();

        if (_testEnvironment.DebugInfo != null)
        {
            if (_testEnvironment.DebugInfo.DebugVSTestConsole)
            {
                environmentVariables["VSTEST_RUNNER_DEBUG_ATTACHVS"] = "1";
            }

            if (_testEnvironment.DebugInfo.DebugTestHost)
            {
                environmentVariables["VSTEST_HOST_DEBUG_ATTACHVS"] = "1";
            }

            if (_testEnvironment.DebugInfo.DebugDataCollector)
            {
                environmentVariables["VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS"] = "1";
            }

            if (!_testEnvironment.DebugInfo.DebugStopAtEntrypoint)
            {
                environmentVariables["VSTEST_DEBUG_NOBP"] = "1";
            }
        }

        return environmentVariables;
    }

    /// <summary>
    /// Invokes <c>vstest.console</c> to discover tests in a test assembly. "/listTests" is appended to the arguments.
    /// </summary>
    /// <param name="testAssembly">A test assembly.</param>
    /// <param name="testAdapterPath">Path to test adapters.</param>
    /// <param name="runSettings">Run settings for execution.</param>
    /// <param name="targetFramework">The target framework to use.</param>
    /// <param name="environmentVariables">Environment variables to set to the started process.</param>
    public void InvokeVsTestForDiscovery(string testAssembly, string testAdapterPath, string runSettings = "", string targetFramework = "",
        Dictionary<string, string?>? environmentVariables = null)
    {
        var arguments = PrepareArguments(testAssembly, testAdapterPath, runSettings, targetFramework, _testEnvironment.InIsolationValue!, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /listtests");
        InvokeVsTest(arguments, environmentVariables);
    }

    /// <summary>
    /// Execute Tests that are not supported with given Runner framework.
    /// </summary>
    /// <param name="runnerFramework">Runner Framework</param>
    /// <param name="framework">Framework for which Tests are not supported</param>
    /// <param name="message">Message to be shown</param>
    public static void ExecuteNotSupportedRunnerFrameworkTests(string runnerFramework, string framework, string message)
    {
        if (runnerFramework.StartsWith(framework))
        {
            Assert.Inconclusive(message);
        }
    }

    /// <summary>
    /// Validate if the overall test count and results are matching.
    /// </summary>
    /// <param name="passed">Passed test count</param>
    /// <param name="failed">Failed test count</param>
    /// <param name="skipped">Skipped test count</param>
    public void ValidateSummaryStatus(int passed, int failed, int skipped)
    {
        // TODO: Switch on the actual version of vstest console when we have that set on test environment.
        if (_testEnvironment.VSTestConsoleInfo?.Path?.Contains($"{Path.DirectorySeparatorChar}15.") == true)
        {
            ValidateSummaryStatusv15(passed, failed, skipped);
            return;
        }

        var totalTestCount = passed + failed + skipped;
        if (totalTestCount == 0)
        {
            // No test should be found/run
            var summaryStatus = string.Format(
                CultureInfo.CurrentCulture,
                TestSummaryStatusMessageFormat,
                @"\d+",
                @"\d+",
                @"\d+",
                @"\d+");
            StringAssert.DoesNotMatch(
                _standardTestOutput,
                new Regex(summaryStatus),
                "Excepted: There should not be test summary{2}Actual: {0}{2}Standard Error: {1}{2}Arguments: {3}{2}",
                _standardTestOutput,
                _standardTestError,
                Environment.NewLine,
                _arguments);
        }
        else
        {
            var summaryStatus = string.Format(CultureInfo.CurrentCulture, TotalTestsMessage, totalTestCount);
            if (passed != 0)
            {
                summaryStatus += string.Format(CultureInfo.CurrentCulture, PassedTestsMessage, passed);
            }

            if (failed != 0)
            {
                summaryStatus += string.Format(CultureInfo.CurrentCulture, FailedTestsMessage, failed);
            }

            if (skipped != 0)
            {
                summaryStatus += string.Format(CultureInfo.CurrentCulture, SkippedTestsMessage, skipped);
            }

            Assert.IsTrue(
                _standardTestOutput.Contains(summaryStatus),
                "The Test summary does not match.{3}Expected summary: {1}{3}Test Output: {0}{3}Standard Error: {2}{3}Arguments: {4}{3}",
                _standardTestOutput,
                summaryStatus,
                _standardTestError,
                Environment.NewLine,
                _arguments);
        }
    }

    /// <summary>
    /// Validate if the overall test count and results are matching.
    /// </summary>
    /// <param name="passed">Passed test count</param>
    /// <param name="failed">Failed test count</param>
    /// <param name="skipped">Skipped test count</param>
    public void ValidateSummaryStatusv15(int passed, int failed, int skipped)
    {
        // example: Total tests: 6. Passed: 2. Failed: 2. Skipped: 2.
        var totalTestCount = passed + failed + skipped;
        if (totalTestCount == 0)
        {
            // No test should be found/run
            StringAssert.DoesNotMatch(
                _standardTestOutput,
                new Regex("Total tests\\:"),
                "Excepted: There should not be test summary{2}Actual: {0}{2}Standard Error: {1}{2}Arguments: {3}{2}",
                _standardTestOutput,
                _standardTestError,
                Environment.NewLine,
                _arguments);
        }
        else
        {
            var summaryStatus = $"Total tests: {totalTestCount}.";
            if (passed != 0)
            {
                summaryStatus += $" Passed: {passed}.";
            }

            if (failed != 0)
            {
                summaryStatus += $" Failed: {failed}.";
            }

            if (skipped != 0)
            {
                summaryStatus += $" Skipped: {skipped}.";
            }

            Assert.IsTrue(
                _standardTestOutput.Contains(summaryStatus),
                "The Test summary does not match.{3}Expected summary: {1}{3}Test Output: {0}{3}Standard Error: {2}{3}Arguments: {4}{3}",
                _standardTestOutput,
                summaryStatus,
                _standardTestError,
                Environment.NewLine,
                _arguments);
        }
    }

    public void StdErrorContains(string substring)
    {
        Assert.IsTrue(_standardTestError.Contains(substring), "StdErrorOutput - [{0}] did not contain expected string '{1}'", _standardTestError, substring);
    }

    public void StdErrorRegexIsMatch(string pattern)
    {
        Assert.IsTrue(Regex.IsMatch(_standardTestError, pattern), "StdErrorOutput - [{0}] did not contain expected pattern '{1}'", _standardTestError, pattern);
    }

    public void StdErrorDoesNotContains(string substring)
    {
        Assert.IsFalse(_standardTestError.Contains(substring), "StdErrorOutput - [{0}] did not contain expected string '{1}'", _standardTestError, substring);
    }

    public void StdOutputContains(string substring)
    {
        Assert.IsTrue(_standardTestOutput.Contains(substring), $"{Environment.NewLine}StdOutput:{Environment.NewLine}{Environment.NewLine}Expected substring: {substring}{Environment.NewLine}{Environment.NewLine}Actual string: {_standardTestOutput}");
    }

    public void StdOutputDoesNotContains(string substring)
    {
        Assert.IsFalse(_standardTestOutput.Contains(substring), $"{Environment.NewLine}StdOutput:{Environment.NewLine}{Environment.NewLine}Not expected substring: {substring}{Environment.NewLine}{Environment.NewLine}Actual string: {_standardTestOutput}");
    }

    public void ExitCodeEquals(int exitCode)
    {
        Assert.AreEqual(exitCode, _runnerExitCode, $"ExitCode - [{_runnerExitCode}] doesn't match expected '{exitCode}'.");
    }

    /// <summary>
    /// Validates if the test results have the specified set of passed tests.
    /// </summary>
    /// <param name="passedTests">Set of passed tests.</param>
    /// <remarks>Provide the full test name similar to this format SampleTest.TestCode.TestMethodPass.</remarks>
    public void ValidatePassedTests(params string[] passedTests)
    {
        // Convert the unicode character to its unicode value for assertion
        _standardTestOutput = Regex.Replace(_standardTestOutput, @"[^\x00-\x7F]", c => $@"\u{(int)c.Value[0]:x4}");
        foreach (var test in passedTests)
        {
            // Check for tick or ? both, in some cases as unicode character for tick is not available
            // in std out and gets replaced by ?
            var flag = _standardTestOutput.Contains("Passed " + test)
                       || _standardTestOutput.Contains("Passed " + GetTestMethodName(test))
                       || _standardTestOutput.Contains("\\ufffd " + test)
                       || _standardTestOutput.Contains("\\ufffd " + GetTestMethodName(test));
            Assert.IsTrue(flag, "Test {0} does not appear in passed tests list.", test);
        }
    }

    /// <summary>
    /// Validates if the test results have the specified set of failed tests.
    /// </summary>
    /// <param name="failedTests">Set of failed tests.</param>
    /// <remarks>
    /// Provide the full test name similar to this format SampleTest.TestCode.TestMethodFailed.
    /// Also validates whether these tests have stack trace info.
    /// </remarks>
    public void ValidateFailedTests(params string[] failedTests)
    {
        foreach (var test in failedTests)
        {
            var flag = _standardTestOutput.Contains("Failed " + test)
                       || _standardTestOutput.Contains("Failed " + GetTestMethodName(test));
            Assert.IsTrue(flag, "Test {0} does not appear in failed tests list.", test);

            // Verify stack information as well.
            Assert.IsTrue(_standardTestOutput.Contains(GetTestMethodName(test)), "No stack trace for failed test: {0}", test);
        }
    }

    /// <summary>
    /// Validates if the test results have the specified set of skipped tests.
    /// </summary>
    /// <param name="skippedTests">The set of skipped tests.</param>
    /// <remarks>Provide the full test name similar to this format SampleTest.TestCode.TestMethodSkipped.</remarks>
    public void ValidateSkippedTests(params string[] skippedTests)
    {
        foreach (var test in skippedTests)
        {
            var flag = _standardTestOutput.Contains("Skipped " + test)
                       || _standardTestOutput.Contains("Skipped " + GetTestMethodName(test));
            Assert.IsTrue(flag, "Test {0} does not appear in skipped tests list.", test);
        }
    }

    /// <summary>
    /// Validate if the discovered tests list contains provided tests.
    /// </summary>
    /// <param name="discoveredTestsList">List of tests expected to be discovered.</param>
    public void ValidateDiscoveredTests(params string[] discoveredTestsList)
    {
        foreach (var test in discoveredTestsList)
        {
            var flag = _standardTestOutput.Contains(test)
                       || _standardTestOutput.Contains(GetTestMethodName(test));
            Assert.IsTrue(flag, $"Test {test} does not appear in discovered tests list." +
                                $"{Environment.NewLine}Std Output: {_standardTestOutput}" +
                                $"{Environment.NewLine}Std Error: {_standardTestError}");
        }
    }

    /// <summary>
    /// Validate that the discovered tests list doesn't contain specified tests.
    /// </summary>
    /// <param name="testsList">List of tests expected not to be discovered.</param>
    public void ValidateTestsNotDiscovered(params string[] testsList)
    {
        foreach (var test in testsList)
        {
            var flag = _standardTestOutput.Contains(test)
                       || _standardTestOutput.Contains(GetTestMethodName(test));
            Assert.IsFalse(flag, $"Test {test} should not appear in discovered tests list." +
                                $"{Environment.NewLine}Std Output: {_standardTestOutput}" +
                                $"{Environment.NewLine}Std Error: {_standardTestError}");
        }
    }

    public void ValidateFullyQualifiedDiscoveredTests(string filePath, params string[] discoveredTestsList)
    {
        var fileOutput = File.ReadAllLines(filePath);
        Assert.IsTrue(fileOutput.Length == 3);

        foreach (var test in discoveredTestsList)
        {
            var flag = fileOutput.Contains(test)
                       || fileOutput.Contains(GetTestMethodName(test));
            Assert.IsTrue(flag, $"Test {test} does not appear in discovered tests list." +
                                $"{Environment.NewLine}Std Output: {_standardTestOutput}" +
                                $"{Environment.NewLine}Std Error: {_standardTestError}");
        }
    }

    protected string GetSampleTestAssembly()
    {
        return GetAssetFullPath("SimpleTestProject.dll");
    }

    protected string GetAssetFullPath(string assetName)
    {
        return _testEnvironment.GetTestAsset(assetName);
    }

    protected string GetTestDllForFramework(string assetName, string targetFramework)
    {
        return _testEnvironment.GetTestAsset(assetName, targetFramework);
    }

    protected List<string> GetTestDlls(params string[] assetNames)
    {
        var assets = new List<string>();
        foreach (var assetName in assetNames)
        {
            assets.Add(GetAssetFullPath(assetName));
        }

        return assets;
    }

    protected string GetProjectFullPath(string projectName)
    {
        return _testEnvironment.GetTestProject(projectName);
    }

    protected string GetProjectAssetFullPath(string projectName, string assetName)
    {
        var projectPath = _testEnvironment.GetTestProject(projectName);
        return Path.Combine(Path.GetDirectoryName(projectPath)!, assetName);
    }

    protected string GetTestAdapterPath(UnitTestFramework testFramework = UnitTestFramework.MSTest)
    {
        if (testFramework == UnitTestFramework.NonDll)
        {
            var dllPath = _testEnvironment.GetTestAsset("NonDll.TestAdapter.dll", "netstandard2.0");
            return Path.GetDirectoryName(dllPath)!;
        }

        string adapterRelativePath = string.Empty;

        if (testFramework == UnitTestFramework.MSTest)
        {
            var version = IntegrationTestEnvironment.DependencyVersions["MSTestTestAdapterVersion"];
            if (version.StartsWith("3"))
            {
                var tfm = _testEnvironment.TargetFramework.StartsWith("net4") ? "net462" : _testEnvironment.TargetFramework;
                adapterRelativePath = string.Format(CultureInfo.InvariantCulture, _msTestAdapterRelativePath, version, tfm);
            }
            else
            {
                adapterRelativePath = string.Format(CultureInfo.InvariantCulture, _msTestPre3_0AdapterRelativePath, version);
            }
        }
        else if (testFramework == UnitTestFramework.NUnit)
        {
            adapterRelativePath = string.Format(CultureInfo.InvariantCulture, _nUnitTestAdapterRelativePath, IntegrationTestEnvironment.DependencyVersions["NUnit3AdapterVersion"]);
        }
        else if (testFramework == UnitTestFramework.XUnit)
        {
            var tfm = _testEnvironment.TargetFramework.StartsWith("net4") ? "net462" : "netcoreapp3.1";
            adapterRelativePath = string.Format(CultureInfo.InvariantCulture, _xUnitTestAdapterRelativePath, IntegrationTestEnvironment.DependencyVersions["XUnitAdapterVersion"], tfm);
        }

        return _testEnvironment.GetNugetPackage(adapterRelativePath);
    }

    protected bool IsDesktopRunner()
    {
        return _testEnvironment.RunnerFramework == DesktopRunnerFramework;
    }

    protected bool IsNetCoreRunner()
    {
        return _testEnvironment.RunnerFramework == CoreRunnerFramework;
    }

    /// <summary>
    /// Gets the path to <c>vstest.console.exe</c> or <c>dotnet.exe</c>.
    /// </summary>
    /// <returns>
    /// Full path to test runner
    /// </returns>
    public virtual string GetConsoleRunnerPath()
    {
        string consoleRunnerPath = string.Empty;

        if (IsDesktopRunner())
        {
            consoleRunnerPath = StringUtils.IsNullOrWhiteSpace(_testEnvironment.VSTestConsoleInfo?.Path)
                ? Path.Combine(IntegrationTestEnvironment.PublishDirectory, $"Microsoft.TestPlatform.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg", "tools", "net462", "Common7", "IDE", "Extensions", "TestPlatform", "vstest.console.exe")
                : _testEnvironment.VSTestConsoleInfo.Path;
        }
        else if (IsNetCoreRunner())
        {
            var executablePath = OSUtils.IsWindows ? @".dotnet\dotnet.exe" : @".dotnet/dotnet";
            consoleRunnerPath = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, executablePath);
        }
        else
        {
            Assert.Fail("Unknown Runner framework - [{0}]", _testEnvironment.RunnerFramework);
        }

        Assert.IsTrue(File.Exists(consoleRunnerPath), "GetConsoleRunnerPath: Path not found: \"{0}\"", consoleRunnerPath);
        return consoleRunnerPath;
    }

    protected virtual string SetVSTestConsoleDLLPathInArgs(string? args)
    {
        var vstestConsoleDll = GetDotnetRunnerPath();
        vstestConsoleDll = vstestConsoleDll.AddDoubleQuote();
        args = string.Concat(
            vstestConsoleDll,
            " ",
            args);
        return args;
    }


    /// <summary>
    /// Returns the VsTestConsole Wrapper.
    /// </summary>
    /// <returns></returns>
    public IVsTestConsoleWrapper GetVsTestConsoleWrapper(Dictionary<string, string?>? environmentVariables = null, TraceLevel traceLevel = TraceLevel.Verbose)
    {
        ConsoleParameters consoleParameters = new();
        if (traceLevel != TraceLevel.Off)
        {
            if (!Directory.Exists(TempDirectory.Path))
            {
                Directory.CreateDirectory(TempDirectory.Path);
            }

            // Directory is already unique so there is no need to have a unique file name.
            var logFilePath = Path.Combine(TempDirectory.Path, "log.txt");
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }

            Console.WriteLine($"Logging diagnostics in {logFilePath}");
            consoleParameters.LogFilePath = logFilePath;
        }

        var consoleRunnerPath = IsNetCoreRunner()
                ? GetDotnetRunnerPath()
                : GetConsoleRunnerPath();
        var executablePath = OSUtils.IsWindows ? @".dotnet\dotnet.exe" : @".dotnet/dotnet";
        var dotnetPath = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, executablePath);

        if (!File.Exists(dotnetPath))
        {
            throw new FileNotFoundException($"File '{dotnetPath}' was not found.");
        }

        if (!File.Exists(consoleRunnerPath))
        {
            throw new FileNotFoundException($"File '{consoleRunnerPath}' was not found.");
        }

        Console.WriteLine($"Console runner path: {consoleRunnerPath}");

        // Providing any environment variable to vstest.console will clear all existing environment variables,
        // this works around it by copying all existing variables, and adding debug. But we only want to do that
        // when we are setting any debug variables.
        // TODO: This is scheduled to be fixed in 17.3, where it will start working normally. We will just add those
        // variables, unless we explicitly say to clean them. https://github.com/microsoft/vstest/pull/3433
        // Remove this code later, and just pass the variables you want to add.
        var debugEnvironmentVariables = AddDebugEnvironmentVariables(new());
        environmentVariables ??= new();
        if (debugEnvironmentVariables.Count > 0)
        {
            Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().ToList().ForEach(e => environmentVariables.Add(e.Key.ToString()!, e.Value?.ToString()));
            foreach (var pair in debugEnvironmentVariables)
            {
                environmentVariables[pair.Key] = pair.Value;
            }
        }

        if (environmentVariables.Count > 0)
        {
            // This clears all variables, so we copy all environment variables, and add the debug ones to them.
            consoleParameters.EnvironmentVariables = environmentVariables;
        }

        var vstestConsoleWrapper = new VsTestConsoleWrapper(consoleRunnerPath, dotnetPath, consoleParameters);
        vstestConsoleWrapper.StartSession();

        return vstestConsoleWrapper;
    }

    /// <summary>
    /// Gets the test method name from full name.
    /// </summary>
    /// <param name="testFullName">Fully qualified name of the test.</param>
    /// <returns>Simple name of the test.</returns>
    private static string GetTestMethodName(string testFullName)
    {
        string testMethodName = string.Empty;

        var splits = testFullName.Split('.');
        if (splits.Length >= 3)
        {
            testMethodName = testFullName.Split('.')[2];
        }

        return testMethodName;
    }

    protected void ExecuteVsTestConsole(string? args, out string stdOut, out string stdError, out int exitCode, Dictionary<string, string?>? environmentVariables = null)
    {
        if (IsNetCoreRunner())
        {
            args = SetVSTestConsoleDLLPathInArgs(args);
        }

        _arguments = args;

        ExecuteApplication(GetConsoleRunnerPath(), args, out stdOut, out stdError, out exitCode, environmentVariables);
    }

    /// <summary>
    /// Executes a local copy of dotnet that has VSTest task installed and possibly other modifications. Do not use this to
    /// do your builds or to run general tests, unless you want your changes to be reflected.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="args"></param>
    /// <param name="stdOut"></param>
    /// <param name="stdError"></param>
    /// <param name="exitCode"></param>
    /// <param name="environmentVariables">Environment variables to set to the started process.</param>
    private static void ExecutePatchedDotnet(string command, string args, out string stdOut, out string stdError, out int exitCode,
        Dictionary<string, string?>? environmentVariables = null)
    {
        environmentVariables ??= new();

        environmentVariables["DOTNET_MULTILEVEL_LOOKUP"] = "0";

        var executablePath = OSUtils.IsWindows ? @"dotnet.exe" : @"dotnet";
        var patchedDotnetPath = Path.GetFullPath(Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, "artifacts", "tmp", ".dotnet", executablePath));
        ExecuteApplication(patchedDotnetPath, string.Join(" ", command, args), out stdOut, out stdError, out exitCode, environmentVariables);
    }

    protected static void ExecuteApplication(string path, string? args, out string stdOut, out string stdError, out int exitCode,
        Dictionary<string, string?>? environmentVariables = null, string? workingDirectory = null)
    {
        if (path.IsNullOrWhiteSpace())
        {
            throw new ArgumentException("Executable path must not be null or whitespace.", nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new ArgumentException($"Executable path '{path}' could not be found.", nameof(path));
        }

        var executableName = Path.GetFileName(path);

        using var process = new Process();
        Console.WriteLine($"IntegrationTestBase.Execute: Starting {executableName}");
        process.StartInfo.FileName = path;
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
        process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

        if (workingDirectory != null)
        {
            process.StartInfo.WorkingDirectory = workingDirectory;
        }

        if (environmentVariables != null)
        {
            foreach (var variable in environmentVariables)
            {
                if (process.StartInfo.EnvironmentVariables.ContainsKey(variable.Key))
                {
                    process.StartInfo.EnvironmentVariables[variable.Key] = variable.Value;
                }
                else
                {
                    process.StartInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
                }
            }
        }

        var stdoutBuffer = new StringBuilder();
        var stderrBuffer = new StringBuilder();
        process.OutputDataReceived += (sender, eventArgs) => stdoutBuffer.AppendLine(eventArgs.Data);
        process.ErrorDataReceived += (sender, eventArgs) => stderrBuffer.AppendLine(eventArgs.Data);

        Console.WriteLine("IntegrationTestBase.Execute: Path = {0}", process.StartInfo.FileName);
        Console.WriteLine("IntegrationTestBase.Execute: Arguments = {0}", process.StartInfo.Arguments);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        if (!process.WaitForExit(30 * 60 * 1000)) // 30 minutes
        {
            Console.WriteLine($"IntegrationTestBase.Execute: Timed out waiting for {executableName}. Terminating the process.");
            process.Kill();
        }
        else
        {
            // Ensure async buffers are flushed
            process.WaitForExit();
            process.WaitForExit(1000);
        }

        stopwatch.Stop();

        Console.WriteLine($"IntegrationTestBase.Execute: Total execution time: {stopwatch.Elapsed.Duration()}");

        stdError = stderrBuffer.ToString();
        stdOut = stdoutBuffer.ToString();
        exitCode = process.ExitCode;

        Console.WriteLine("IntegrationTestBase.Execute: stdError = {0}", stdError);
        Console.WriteLine("IntegrationTestBase.Execute: stdOut = {0}", stdOut);
        Console.WriteLine($"IntegrationTestBase.Execute: Stopped {executableName}. Exit code = {0}", exitCode);
    }

    private void FormatStandardOutCome()
    {
        StdErrWithWhiteSpace = _standardTestError;
        _standardTestError = Regex.Replace(_standardTestError, @"\s+", " ");

        StdOutWithWhiteSpace = _standardTestOutput;
        _standardTestOutput = Regex.Replace(_standardTestOutput, @"\s+", " ");
    }

    /// <summary>
    /// Create runsettings file from runConfigurationDictionary at destinationRunsettingsPath
    /// </summary>
    /// <param name="destinationRunsettingsPath">
    /// Destination runsettings path where resulted file saves
    /// </param>
    /// <param name="runConfigurationDictionary">
    /// Contains run configuration settings
    /// </param>
    public static void CreateRunSettingsFile(string destinationRunsettingsPath, IDictionary<string, string> runConfigurationDictionary)
    {
        var doc = new XmlDocument();
        var xmlDeclaration = doc.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty);

        doc.AppendChild(xmlDeclaration);
        var runSettingsNode = doc.CreateElement(Constants.RunSettingsName);
        doc.AppendChild(runSettingsNode);
        var runConfigNode = doc.CreateElement(Constants.RunConfigurationSettingsName);
        runSettingsNode.AppendChild(runConfigNode);

        foreach (var settingsEntry in runConfigurationDictionary)
        {
            var childNode = doc.CreateElement(settingsEntry.Key);
            childNode.InnerText = settingsEntry.Value;
            runConfigNode.AppendChild(childNode);
        }

        Stream stream = new FileHelper().GetStream(destinationRunsettingsPath, FileMode.Create);
        doc.Save(stream);
        stream.Dispose();
    }

    /// <summary>
    /// Create runsettings file at destinationRunsettingsPath with the content from xmlString
    /// </summary>
    /// <param name="destinationRunsettingsPath">
    /// Destination runsettings path where resulted file is saved
    /// </param>
    /// <param name="runSettingsXml">
    /// Run settings xml string
    /// </param>
    public static void CreateRunSettingsFile(string destinationRunsettingsPath, string runSettingsXml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(runSettingsXml);
        var stream = new FileHelper().GetStream(destinationRunsettingsPath, FileMode.Create);
        doc.Save(stream);
        stream.Dispose();
    }

    protected string BuildMultipleAssemblyPath(params string[] assetNames)
    {
        // Double quoted sources separated by space.
        return string.Join(" ", GetTestDlls(assetNames).Select(a => a.AddDoubleQuote()));
    }

    protected static string GetDiagArg(string rootDir)
        => " --diag:" + Path.Combine(rootDir, "log.txt");

    /// <summary>
    /// Counts the number of logs following the '*.host.*' pattern in the given folder.
    /// </summary>
    protected static int CountTestHostLogs(string diagLogsDir)
        => Directory.GetFiles(diagLogsDir, "*.host.*").Length;

    protected static void AssertExpectedNumberOfHostProcesses(int expectedNumOfProcessCreated, string diagLogsDir, IEnumerable<string> testHostProcessNames,
        string? arguments = null, string? runnerPath = null)
    {
        var processCreatedCount = CountTestHostLogs(diagLogsDir);
        Assert.AreEqual(
            expectedNumOfProcessCreated,
            processCreatedCount,
            $"Number of {string.Join(", ", testHostProcessNames)} process created, expected: {expectedNumOfProcessCreated} actual: {processCreatedCount} {(arguments == null ? "" : "args: " + arguments)} {(runnerPath == null ? "" : "runner path: " + runnerPath)}");
    }

    protected static string GetDownloadedDotnetMuxerFromTools(string architecture)
    {
        if (architecture is not "X86" and not "X64")
        {
            throw new NotSupportedException(nameof(architecture));
        }

        string path = Path.Combine(IntegrationTestEnvironment.RepoRootDirectory, ".dotnet",
            architecture == "X86" ?
            "dotnet-sdk-x86" :
            "", // x64 is directly in .dotnet folder
            $"dotnet{(OSUtils.IsWindows ? ".exe" : "")}");

        Assert.IsTrue(File.Exists(path), $"Path '{path}' should exist.");

        return path;
    }

    protected string GetDotnetRunnerPath() =>
        _testEnvironment.VSTestConsoleInfo?.Path ?? Path.Combine(IntegrationTestEnvironment.PublishDirectory, $"Microsoft.TestPlatform.CLI.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg", "contentFiles", "any", "netcoreapp3.1", "vstest.console.dll");

    protected void StdOutHasNoWarnings()
    {
        StdOut.Should().NotContainEquivalentOf("warning");
    }

    protected void StdErrHasTestRunFailedMessageButNoOtherError()
    {
        StdErr?.Trim().Should().Be("Test Run Failed.");
    }
}
