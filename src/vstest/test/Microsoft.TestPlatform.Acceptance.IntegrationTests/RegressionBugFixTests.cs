// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Integration tests that verify specific bug fixes remain working end-to-end.
/// Each test references the GitHub issue it guards against.
/// </summary>
[TestClass]
public class RegressionBugFixTests : AcceptanceTestBase
{
    #region GH-4461: Testhost crash must not expose socket stack traces to the user

    [TestMethod]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void TesthostCrash_MustNotShowSocketExceptionToUser(RunnerInfo runnerInfo)
    {
        // GH-4461: When testhost crashes (e.g. stack overflow), the parent process
        // was dumping IOException / SocketException stack traces to stderr, confusing
        // developers. The fix ensures socket errors are logged internally but not
        // propagated to stderr.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("crash.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");

        InvokeVsTest(arguments);

        // The test run should fail because the test crashes.
        ExitCodeEquals(1);

        // The key regression check: stderr must NOT contain raw socket exception details.
        StdErrorDoesNotContains("System.IO.IOException");
        StdErrorDoesNotContains("SocketException");
    }

    #endregion

    #region GH-5184: Stderr from testhost must not fail the test run

    [TestMethod]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void StderrFromTesthost_MustNotFailTestRun(RunnerInfo runnerInfo)
    {
        // GH-5184: When a test writes debug output to stderr, the test run was
        // incorrectly marked as failed because stderr was treated as error output.
        // The fix forwards stderr as Informational, not Error.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("StderrOutputProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        arguments = string.Concat(arguments, $" /ResultsDirectory:{TempDirectory.Path}");

        InvokeVsTest(arguments);

        // The test writes to stderr but passes — the run must succeed.
        ExitCodeEquals(0);
        ValidateSummaryStatus(passed: 1, failed: 0, skipped: 0);
    }

    #endregion

    #region GH-3136: HTML logger must handle special characters in test output

    [TestMethod]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void HtmlLogger_SpecialCharactersInOutput_MustNotThrow(RunnerInfo runnerInfo)
    {
        // GH-3136: Test output containing invalid XML characters (e.g. U+FFFF) caused
        // XmlException in the HTML logger. The fix sets CheckCharacters = false on the
        // XmlReader so these characters are tolerated.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SpecialCharOutputProject.dll");
        var htmlFileName = "TestResults.html";
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"html;LogFileName={htmlFileName}\"");

        InvokeVsTest(arguments);

        ExitCodeEquals(0);
        ValidateSummaryStatus(passed: 1, failed: 0, skipped: 0);

        // The HTML log file must exist and not be empty — before the fix, the logger
        // would throw and the file would be missing or truncated.
        var htmlFiles = Directory.GetFiles(TempDirectory.Path, "*.html", SearchOption.AllDirectories);
        Assert.IsNotEmpty(htmlFiles, $"Expected an HTML log file in {TempDirectory.Path}");
        Assert.IsGreaterThan(0L, new FileInfo(htmlFiles.First()).Length, "HTML log file is empty");
    }

    #endregion
}
