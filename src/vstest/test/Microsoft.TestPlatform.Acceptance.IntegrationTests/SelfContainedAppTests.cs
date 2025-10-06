// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class SelfContainedAppTests : AcceptanceTestBase
{
    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetCoreTargetFrameworkDataSourceAttribute(useDesktopRunner: false)]
    public void RunningApplicationThatIsBuiltAsSelfContainedWillNotFailToFindHostpolicyDll(RunnerInfo runnerInfo)
    {
        // when the application is self-contained which is dictated by the RuntimeIdentifier and OutputType project
        // properties, the testhost.exe executable is given a runtimeconfig that instructs it to find a hostpolicy.dll and hostfxr.dll next to it
        // that will fail if we run the testhost.exe from the .nuget location, but will work when we run it from the output folder
        // see https://github.com/dotnet/runtime/issues/3569#issuecomment-595820524 and below for description of how it works
        SetTestEnvironment(_testEnvironment, runnerInfo);

        // the app is published to win-x64 because of the runtime identifier in the project
        var assemblyPath = GetAssetFullPath($@"win-x64{Path.DirectorySeparatorChar}SelfContainedAppTestProject.dll");
        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);

        ValidateSummaryStatus(passed: 1, 0, 0);
    }
}
