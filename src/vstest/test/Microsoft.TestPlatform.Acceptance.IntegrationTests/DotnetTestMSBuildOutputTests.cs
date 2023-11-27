// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    public void RunDotnetTestWithCsproj(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectPath = GetIsolatedTestAsset("SimpleTestProject.csproj");
        InvokeDotnetTest($@"{projectPath} /p:VsTestUseMSBuildOutput=true /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}");

        // The output:
        // Determining projects to restore...
        //   Restored C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj (in 441 ms).
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\net462\SimpleTestProject.dll
        //   SimpleTestProject -> C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\artifacts\bin\TestAssets\SimpleTestProject\Debug\netcoreapp3.1\SimpleTestProject.dll
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=net462]
        // C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\UnitTest1.cs(41): error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.  [C:\Users\nohwnd\AppData\Local\Temp\vstest\xvoVt\SimpleTestProject.csproj::TargetFramework=netcoreapp3.1]

        StdOutputContains("error VSTEST1: (FailingTest) SampleUnitTestProject.UnitTest1.FailingTest() Assert.AreEqual failed. Expected:<2>. Actual:<3>.");
        ExitCodeEquals(1);
    }
}
