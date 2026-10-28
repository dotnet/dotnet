// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DotnetTestTests : AcceptanceTestBase
{
    private static string GetFinalVersion(string version)
    {
        // Mirrors PrintSplashScreen in vstest.console: the banner drops the whole version suffix,
        // so the expected version here has to drop it the same way.
        var end = version.IndexOf("-");
        return (end >= 0) ? version.Substring(0, end) : version;
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunDotnetTestWithCsproj(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("SimpleTestProject.csproj");
        // Same inherited MSBUILDENSURESTDOUTFORTASKPROCESSES=1 as in DotnetTestMSBuildOutputTests. It does not change
        // which target runs here - VSTestUseMSBuildOutput=false selects _VSTestConsole either way - but it does change
        // how MSBuild handles the task process stdout, and on the modern agents the vstest.console banner line carrying
        // the version, which is the only thing this test asserts beyond the summary, is absent from the captured output
        // while RunDotnetTestWithDll, which reaches vstest.console without the MSBuild task, still shows it.
        // Neutralizing the variable tests that mechanism. If the banner is still missing after this, the cause is
        // separate from this variable and the row needs a written reason under bar (c) rather than a weaker assertion.
        //
        // MEASURED: build 3067659 answered that in the negative. With the variable pinned to "0" the banner was STILL
        // absent, so by this comment's own terms the cause is separate and is not this variable. RunDotnetTestWithDll
        // asserts the SAME version string and PASSES on the same build, differing only by reaching vstest.console
        // without the MSBuild task, so the banner is lost specifically on the VSTestTask path. 17.11 shows the identical
        // symptom, so this is not a 17.12 port artifact. -v:detailed makes MSBuild report the task invocation and the
        // Low-importance "VSTestNoLogo = ..." dump the targets already emit, which distinguishes "the banner was never
        // requested" from "the banner was written but not captured".
        InvokeDotnetTest($@"{projectPath} -p:VSTestUseMSBuildOutput=false -v:detailed --logger:""Console;Verbosity=normal"" /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", environmentVariables: new Dictionary<string, string?> { ["MSBUILDENSURESTDOUTFORTASKPROCESSES"] = "0" });

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));
        ValidateSummaryStatus(1, 1, 1);
        ExitCodeEquals(1);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunDotnetTestWithDll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("SimpleTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal""");

        // ensure our dev version is used
        StdOutputContains(GetFinalVersion(IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion));
        ValidateSummaryStatus(1, 1, 1);
        ExitCodeEquals(1);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunDotnetTestWithCsprojPassInlineSettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("ParametrizedTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} --logger:""Console;Verbosity=normal"" -p:VSTestUseMSBuildOutput=false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion} -- TestRunParameters.Parameter(name =\""weburl\"", value=\""http://localhost//def\"")");

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunDotnetTestWithDllPassInlineSettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("ParametrizedTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal"" -- TestRunParameters.Parameter(name=\""weburl\"", value=\""http://localhost//def\"")");

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    [Ignore("TODO: This scenario is broken in real environment as well (running with shipped `dotnet test`. Old tests (before arcade) use location of vstest.console that have more dlls in place than what we ship, and they make it work.")]
    public void RunDotnetTestWithNativeDll(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        string assemblyRelativePath = @"microsoft.testplatform.testasset.nativecpp\2.0.0\contentFiles\any\any\x64\Microsoft.TestPlatform.TestAsset.NativeCPP.dll";
        var assemblyAbsolutePath = Path.Combine(_testEnvironment.PackageDirectory, assemblyRelativePath);

        InvokeDotnetTest($@"{assemblyAbsolutePath} --logger:""Console;Verbosity=normal"" --diag:c:\temp\logscpp\");

        ValidateSummaryStatus(1, 1, 0);
        ExitCodeEquals(1);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void RunDotnetTestAndSeeOutputFromConsoleWriteLine(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("OutputtingTestProject.dll");
        InvokeDotnetTest($@"{assemblyPath} --logger:""Console;Verbosity=normal"" ");

        StdOutputContains("MY OUTPUT FROM TEST");

        ValidateSummaryStatus(1, 0, 0);
        ExitCodeEquals(0);
    }
}
