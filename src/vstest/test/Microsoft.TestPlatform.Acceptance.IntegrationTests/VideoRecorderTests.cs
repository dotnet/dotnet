// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
[TestCategory("Windows-Review")]
public class VideoRecorderTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource(useCoreRunner: false, useVsixRunner: true)]
    public void VideoRecorderDataCollectorShouldRecordVideoWithRunSettings(RunnerInfo runnerInfo)
    {
        // Workaround for #15586 — video recording fails with access denied on the official CI pipeline.
        if (Environment.GetEnvironmentVariable("_RunAsInternal") == "True")
        {
            Assert.Inconclusive("Video recorder test is skipped on the official pipeline due to access denied errors. See #15586.");
        }

        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /Collect:\"Screen and Voice Recorder\"");

        InvokeVsTest(arguments);

        // Verify video attachments were created
        var resultFiles = Directory.GetFiles(TempDirectory.Path, "*.wmv", SearchOption.AllDirectories);
        Assert.IsNotEmpty(resultFiles,
            $"Expected video attachments (.wmv) in results directory '{TempDirectory.Path}', but found none. "
            + $"All files: [{string.Join(", ", Directory.GetFiles(TempDirectory.Path, "*", SearchOption.AllDirectories).Select(Path.GetFileName))}]");
    }
}
