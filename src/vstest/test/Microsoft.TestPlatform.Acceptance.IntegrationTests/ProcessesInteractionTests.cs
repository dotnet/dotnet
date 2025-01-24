// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

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
    [NetCoreTargetFrameworkDataSource]
    public void WhenTestHostProcessExitsBecauseTheTargetedRuntimeIsNoFoundThenTheMessageIsCapturedFromTheErrorOutput(RunnerInfo runnerInfo)
    {
        // Arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        const string testAssetProjectName = "SimpleTestProjectMessedUpTargetFramework";
        var assemblyPath = GetTestDllForFramework(testAssetProjectName + ".dll", Core80TargetFramework);
        UpdateRuntimeConfigJsonWithInvalidFramework(assemblyPath, testAssetProjectName);

        // Act
        InvokeVsTest(assemblyPath);

        // Assert
        ExitCodeEquals(1);
        StdErrorRegexIsMatch("You must install or update \\.NET to run this application\\. App: .*testhost\\.(exe|dll) Architecture: x64 Framework: 'Microsoft\\.NETCore\\.App', version '0\\.0\\.0' \\(x64\\)");

        static void UpdateRuntimeConfigJsonWithInvalidFramework(string assemblyPath, string testAssetProjectName)
        {
            // On the contrary to other tests, we need to modify the test asset we are using to replace
            // the target framework with an invalid framework. This is why we have a specific test asset
            // that's only meant to be used by this project.
            var runtimeConfigJson = Path.Combine(Path.GetDirectoryName(assemblyPath)!, testAssetProjectName + ".runtimeconfig.json");
            var fileContent = File.ReadAllText(runtimeConfigJson);
            var updatedContent = fileContent.Replace("\"version\": \"8.0.0\"", "\"version\": \"0.0.0\"");
            File.WriteAllText(runtimeConfigJson, updatedContent);
        }
    }
}
