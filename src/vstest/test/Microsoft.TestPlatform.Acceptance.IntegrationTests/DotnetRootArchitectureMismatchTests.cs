// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

/// <summary>
/// End-to-end coverage for the scenario behind https://github.com/microsoft/vstest/issues/16151.
///
/// When <c>vstest.console.exe</c> (the .NET Framework console) is invoked directly - e.g. by
/// Visual Studio or by hand - rather than through <c>dotnet test</c>, the SDK does not provide the
/// <c>VSTEST_DOTNET_ROOT_PATH</c> hint that normally disambiguates the runtime location. If the
/// surrounding environment also has an architecture-less <c>DOTNET_ROOT</c> pointing at an x64
/// installation, launching an x86 testhost used to make the x86 apphost load the x64
/// <c>hostfxr.dll</c> and fail with <c>0x800700C1</c> (<c>ERROR_BAD_EXE_FORMAT</c>).
///
/// This test runs an x86 .NET test through <c>vstest.console.exe</c> invoked directly while an
/// ambiguous architecture-less <c>DOTNET_ROOT</c> (pointing at the x64 install) is set, and asserts
/// the run completes normally. Because the CI machines do not expose an x86 .NET runtime at a default
/// discoverable location (it lives next to the build under <c>.dotnet/dotnet-sdk-x86</c>), the x86
/// runtime location is provided explicitly via <c>DOTNET_ROOT_X86</c> / <c>DOTNET_ROOT(x86)</c> - the
/// same approach the existing architecture-switch acceptance tests use. The strict
/// "fails without the fix" behavior is covered by the <c>DotnetTestHostManagerTests</c> unit tests; this
/// test guards the direct-invocation x86 flow end-to-end.
/// </summary>
[TestClass]
// testhost.x86.exe and the apphost/muxer runtime resolution this test exercises only exist on Windows.
[TestCategory("Windows-Review")]
public class DotnetRootArchitectureMismatchTests : AcceptanceTestBase
{
    [TestMethod]
    public void RunningX86NetTestThroughVsTestConsoleExeWithX64DotnetRootRunsWithoutErrors()
    {
        // Desktop runner => vstest.console.exe (.NET Framework) is invoked directly, which is the
        // scenario where the SDK does not pass VSTEST_DOTNET_ROOT_PATH. Target net11.0 + /Platform:x86
        // makes vstest launch an x86 testhost that resolves its runtime from the DOTNET_ROOT* variables.
        SetTestEnvironment(_testEnvironment, new RunnerInfo
        {
            RunnerFramework = DesktopRunnerFramework,
            TargetFramework = Core11TargetFramework,
            InIsolationValue = InIsolation,
        });

        // ".dotnet/dotnet.exe" is the x64 muxer installed by the build, so its parent is a real x64
        // DOTNET_ROOT. "dotnet-sdk-x86/dotnet.exe" is the matching x86 installation.
        var x64DotnetRoot = Path.GetDirectoryName(GetDownloadedDotnetMuxerFromTools("X64"))!;
        var x86DotnetRoot = Path.GetDirectoryName(GetDownloadedDotnetMuxerFromTools("X86"))!;

        var arguments = PrepareArguments(
            GetTestDllForFramework("SimpleTestProject.dll", Core11TargetFramework, automaticallyResolveCompatibilityTestAsset: false),
            GetTestAdapterPath(),
            string.Empty,
            FrameworkArgValue,
            _testEnvironment.InIsolationValue,
            resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /Platform:x86");

        var environmentVariables = new Dictionary<string, string?>
        {
            // The ambiguous architecture-less DOTNET_ROOT from issue #16151: it points at an x64 install
            // even though we are about to launch an x86 testhost.
            ["DOTNET_ROOT"] = x64DotnetRoot,

            // Tell the x86 testhost where its (private) x86 runtime is. CI machines have no x86 runtime at
            // a default discoverable location, so without this the run cannot find an x86 host at all. With
            // the fix, the ambiguous architecture-less DOTNET_ROOT no longer interferes with this resolution.
            ["DOTNET_ROOT_X86"] = x86DotnetRoot,
            ["DOTNET_ROOT(x86)"] = x86DotnetRoot,

            // Ensure we exercise the direct vstest.console.exe invocation path and not the SDK (dotnet test)
            // path that already disambiguates the runtime for us. The production code treats an empty value as
            // "not set"; use string.Empty rather than null because the test harness assigns these into
            // ProcessStartInfo.EnvironmentVariables, where null can behave inconsistently.
            ["VSTEST_DOTNET_ROOT_PATH"] = string.Empty,
            ["VSTEST_DOTNET_ROOT_ARCHITECTURE"] = string.Empty,
        };

        InvokeVsTest(arguments, environmentVariables);

        // SimpleTestProject has 1 passing, 1 failing and 1 skipped test. Getting a complete summary proves
        // the x86 testhost resolved its runtime and actually executed the tests despite the mismatched
        // architecture-less DOTNET_ROOT; before the fix the apphost crashed with 0x800700C1 in this setup.
        ValidateSummaryStatus(1, 1, 1);
    }
}
