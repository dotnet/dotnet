// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Text.RegularExpressions;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class ProcessesInteractionTests : AcceptanceTestBase
{
    /// <summary>
    /// Having an invalid framework is a way to reproduce an issue we had on Unix, where we were
    /// not handling correctly the process exit (causing us to not let time to the process to
    /// flush its output and error streams).See https://github.com/microsoft/vstest/issues/3375
    /// </summary>
    [TestMethod]
    [DoNotParallelize] // Modifies the test asset's runtimeconfig.json on disk.
    [TestMatrix(testHost: Net)]
    public void WhenTestHostProcessExitsBecauseTheTargetedRuntimeIsNoFoundThenTheMessageIsCapturedFromTheErrorOutput(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        const string testAssetProjectName = "SimpleTestProjectMessedUpTargetFramework";
        var assemblyPath = GetTestDllForFramework(testAssetProjectName + ".dll", Core11TargetFramework);

        // On the contrary to other tests, we need to modify the test asset we are using to replace
        // the target framework with an invalid framework. This is why we have a specific test asset
        // that's only meant to be used by this project.
        var runtimeConfigJson = Path.Combine(Path.GetDirectoryName(assemblyPath)!, testAssetProjectName + ".runtimeconfig.json");
        var originalContent = File.ReadAllText(runtimeConfigJson);
        try
        {
            // The version the SDK writes here is its own runtime version, not the one of the TFM, so don't match a fixed
            // version. Use a version that cannot be found, because DOTNET_ROLL_FORWARD=LatestMajor would roll
            // a low version like 0.0.0 forward to the installed runtime.
            var updatedContent = Regex.Replace(originalContent, @"""version""\s*:\s*""[^""]+""", @"""version"": ""9999.0.0""");
            Assert.AreNotEqual(originalContent, updatedContent, "The framework version in runtimeconfig.json was not rewritten, did the test asset layout change?");
            File.WriteAllText(runtimeConfigJson, updatedContent);

            // Act
            InvokeVsTest(assemblyPath);

            // Assert
            ExitCodeEquals(1);
            StdErrorRegexIsMatch("You must install or update \\.NET to run this application\\. App: .*testhost\\.(exe|dll) Architecture: x64 Framework: 'Microsoft\\.NETCore\\.App', version '9999\\.0\\.0' \\(x64\\)");
        }
        finally
        {
            File.WriteAllText(runtimeConfigJson, originalContent);
        }
    }
}
