// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// Running dotnet test + csproj and using MSBuild for the output.
/// </summary>
[TestClass]
public class DotnetTestMSBuildOutputTests : AcceptanceTestBase
{
    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeEnabledByBuildPropertyAndDoesNotEatSpecialChars(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        // Microsoft.TestPlatform.targets calls _VSTestMSBuild - the target this test exists to cover - only when
        // "$(_MSBUILDTLENABLED) == '1' AND $(VsTestUseMSBuildOutput)", and falls back to _VSTestConsole when
        // "$(_MSBUILDTLENABLED) == '0' OR !$(VsTestUseMSBuildOutput) OR $(MSBUILDENSURESTDOUTFORTASKPROCESSES) == '1'".
        // Two of those three fallback clauses are live on the modern agents, so the fallback ran, failed the build on
        // the deliberately failing test, and _VSTestMSBuild never produced output at all:
        //   - VsTestUseMSBuildOutput defaults to False in the targets and this test never set it, so it depended on
        //     dotnet test defaulting it to true, which the 11.0.100-preview SDK on these agents does not do; and
        //   - the agent exports MSBUILDENSURESTDOUTFORTASKPROCESSES=1, which forces the fallback on its own.
        // Setting both makes target selection deterministic instead of inherited from the SDK and the agent, and it
        // makes the test finally match its own name by actually passing the build property. "0" is used rather than
        // null so the value fails the '== 1' comparison outright instead of relying on the null-removal semantics of
        // ProcessStartInfo.EnvironmentVariables. MSBuildLoggerCanBeDisabledByEnvironmentVariableProperty below sets
        // that same variable to 1 on purpose to DISABLE this logger, so a test asking for it to be ENABLED must clear
        // it. The assertions are untouched - this makes the MSBuild logger path run, it does not weaken what is checked.
        //
        // MEASURED AFTERWARDS, AND IT FALSIFIED THE PARAGRAPH ABOVE: build 3067659 set BOTH inputs - _MSBUILDTLENABLED=1
        // and VsTestUseMSBuildOutput=true, with MSBUILDENSURESTDOUTFORTASKPROCESSES pinned to 0 - and the captured output
        // was STILL the console-logger summary ("Failed! - Failed: 1, Passed: 1, Skipped: 1, Total: 3, Duration: ..."),
        // i.e. _VSTestConsole ran anyway. With every documented fallback clause neutralized that should be unreachable,
        // so one of the inputs is not arriving as this call believes. It also matches 17.11's failure symptom-for-symptom
        // even though 17.11 already passed the build property, which is evidence the two lines share a cause that is not
        // the targets condition on either branch. -v:detailed makes MSBuild report target selection and the
        // Low-importance property dump from ShowCallOfVSTestTaskWithParameter, which _VSTestConsole calls and
        // _VSTestMSBuild does not, so the failure output states which target ran instead of inviting a third guess.
        //
        // THEN 17.11 RAN THAT SAME DIAGNOSTIC (build 3067671) AND MSBUILD ANSWERED IT OUTRIGHT, from the very line that
        // owns the condition:
        //
        //   Microsoft.TestPlatform.targets(32,42): message : Property 'MSBUILDENSURESTDOUTFORTASKPROCESSES'
        //   with value '1' expanded from the environment.
        //
        // So the child process reads '1' no matter what this test writes into ProcessStartInfo.EnvironmentVariables -
        // the value is re-applied downstream. In 17.11's full console attachment _VSTestMSBuild appeared 0 times and
        // _VSTestConsole 8 times, ending in FAILED; _VSTestConsole is evaluated first, so the target under test never
        // ran at all. 17.12 carries the identical 'OR $(MSBUILDENSURESTDOUTFORTASKPROCESSES) == 1' clause and the
        // identical symptom, so this is the same defect, not a second one.
        //
        // The fix is precedence, not a different value. Environment variables are the LOWEST precedence source of an
        // MSBuild property, and a global property set with /p: overrides them and cannot be overwritten by the
        // environment. Measured directly with a standalone probe: with the variable exported as 1, $(...) evaluates to
        // 1 and the '== 1' comparison fires; adding /p:MSBUILDENSURESTDOUTFORTASKPROCESSES=0 makes it evaluate to 0 and
        // the comparison stops firing. Only the value the CONDITION sees changes - MSBuild's own stdout handling still
        // reads the real environment variable, so engine behaviour is untouched. The environment entry is kept as well
        // so the intent stays readable and so nothing regresses if the property is ever renamed.
        //
        // THAT WAS NECESSARY BUT NOT SUFFICIENT, AND BUILD 3067697 MEASURED THE REMAINING CLAUSE. The condition that
        // actually selects the target is evaluated in the SDK's copy of the targets, not this repo's:
        //
        //   .dotnet\sdk\11.0.100-preview.6.26359.118\Microsoft.TestPlatform.targets(33,42)
        //     _VSTestConsole : $(_MSBUILDTLENABLED) == '0' OR !$(VsTestUseMSBuildOutput) OR $(MSBUILDENSURESTDOUTFORTASKPROCESSES) == '1'
        //     _VSTestMSBuild : $(_MSBUILDTLENABLED) == '1' AND $(VsTestUseMSBuildOutput)
        //
        // In 3067697 the global property landed exactly as intended - the command line carried
        // "--property:MSBUILDENSURESTDOUTFORTASKPROCESSES=0" and MSBuild logged "Property reassignment:
        // $(MSBUILDENSURESTDOUTFORTASKPROCESSES)="0" (previous value: "1")" - and _VSTestConsole still ran 9 times
        // while _VSTestMSBuild ran 0 times. The same targets line also reported "Property '_MSBUILDTLENABLED' with
        // value '0' expanded from the environment", which is the answer: _MSBUILDTLENABLED is '0' because the terminal
        // logger is not active when the agent redirects stdout, so the FIRST fallback clause is true on its own and
        // neutralizing the third one could never change the outcome. The "1" written into the environment dictionary
        // below does not survive, because MSBuild re-applies its own value downstream - the identical precedence
        // defect already established for MSBUILDENSURESTDOUTFORTASKPROCESSES, and it takes the identical cure.
        // Measured with a standalone probe replaying the condition verbatim: with _MSBUILDTLENABLED exported as 0 the
        // probe selects _VSTestConsole, and adding /p:_MSBUILDTLENABLED=1 makes it select _VSTestMSBuild. The
        // assertions are untouched - this makes the target under test run, it does not weaken what is checked.
        InvokeDotnetTest($@"{projectPath} -nodereuse:false -v:detailed /p:VsTestUseMSBuildOutput=true /p:_MSBUILDTLENABLED=1 /p:MSBUILDENSURESTDOUTFORTASKPROCESSES=0 /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", new Dictionary<string, string?>
        {
            // Setting this temporarily, until we upgrade to final net9, which has this option set automatically in the MSBUILD sdk.
            // Without this option we don't produce any output to the terminal logger.
            ["_MSBUILDTLENABLED"] = "1",
            ["MSBUILDENSURESTDOUTFORTASKPROCESSES"] = "0"
        });

        // The output:
        // Determining projects to restore...
        //   Restored C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj (in 441 ms).
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\net462\SimpleTestProject.dll
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\netcoreapp3.1\SimpleTestProject.dll
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=net462]
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=netcoreapp3.1]

        StdOutputContains("error TESTERROR: FailingTest (");
        StdOutputContains("): Error Message: Assert.AreEqual failed. Expected:<ğğğ𦮙我們剛才從𓋴𓅓𓏏𓇏𓇌𓀀>. Actual:<not the same>.");
        StdOutputContains("at TerminalLoggerUnitTests.UnitTest1.FailingTest() in");
        // We are sending those as low prio messages, they won't show up on screen but will be in binlog.
        //StdOutputContains("passed PassingTest");
        //StdOutputContains("skipped SkippingTest");
        ExitCodeEquals(1);
    }

    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeDisabledByBuildProperty(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} -nodereuse:false /p:VsTestUseMSBuildOutput=false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}");

        // Check that we see the summary that is printed from the console logger, meaning the new output is disabled.
        StdOutputContains("Failed! - Failed: 1, Passed: 1, Skipped: 1, Total: 3, Duration:");
        // We are sending those as low prio messages, they won't show up on screen but will be in binlog.
        //StdOutputContains("passed PassingTest");
        //StdOutputContains("skipped SkippingTest");
        ExitCodeEquals(1);
    }


    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSource(useDesktopRunner: false)]
    public void MSBuildLoggerCanBeDisabledByEnvironmentVariableProperty(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("TerminalLoggerTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} -nodereuse:false /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", environmentVariables: new Dictionary<string, string?> { ["MSBUILDENSURESTDOUTFORTASKPROCESSES"] = "1" });

        // Check that we see the summary that is printed from the console logger, meaning the new output is disabled.
        StdOutputContains("Failed! - Failed: 1, Passed: 1, Skipped: 1, Total: 3, Duration:");

        ExitCodeEquals(1);
    }
}
