// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows")]
public class DisableAppdomainTests : AcceptanceTestBase
{
    [TestMethod]
    [TestCategory("Windows")]
    [NetFullTargetFrameworkDataSource]
    public void DisableAppdomainTest(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var diableAppdomainTest1 = _testEnvironment.GetTestAsset("DisableAppdomainTest1.dll", Net462TargetFramework);
        var diableAppdomainTest2 = _testEnvironment.GetTestAsset("DisableAppdomainTest2.dll", Net462TargetFramework);

        RunTests(runnerInfo, $"{diableAppdomainTest1}\" \"{diableAppdomainTest2}", 2);
    }

    [TestMethod]
    [TestCategory("Windows")]
    [NetFullTargetFrameworkDataSource]
    public void NewtonSoftDependencyWithDisableAppdomainTest(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var newtonSoftDependnecyTest = _testEnvironment.GetTestAsset("NewtonSoftDependency.dll", Net462TargetFramework);

        RunTests(runnerInfo, newtonSoftDependnecyTest, 1);
    }

    private void RunTests(RunnerInfo runnerInfo, string testAssembly, int passedTestCount)
    {
        if (runnerInfo.IsNetRunner)
        {
            Assert.Inconclusive("This test is not meant for .netcore.");
            return;
        }

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "DisableAppDomain", "true" }
        };

        var arguments = PrepareArguments(
            testAssembly,
            string.Empty,
            GetRunsettingsFilePath(TempDirectory, runConfigurationDictionary),
            FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(passedTestCount, 0, 0);
    }

    private static string GetRunsettingsFilePath(TempDirectory tempDirectory, Dictionary<string, string> runConfigurationDictionary)
    {
        var runsettingsPath = Path.Combine(tempDirectory.Path, $"test_{Guid.NewGuid()}.runsettings");
        CreateRunSettingsFile(runsettingsPath, runConfigurationDictionary);
        return runsettingsPath;
    }
}
