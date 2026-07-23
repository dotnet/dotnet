// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class SerializerSelectionTests : AcceptanceTestBase
{
    [TestMethod]
    [TestMatrix(console: Net, testHost: Net)]
    public void OnNetCoreRunner_ShouldUseSystemTextJson(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("Using System.Text.Json serializer", diagLogs,
            "Expected 'Using System.Text.Json serializer' in diag logs but not found.");
    }

    [TestMethod]
    // The .NET Framework runner (and its Jsonite serializer) only runs on Windows; the core counterpart
    // is covered by OnNetCoreRunner_ShouldUseSystemTextJson.
    [TestCategory("Windows-Review")]
    [TestMatrix(console: NetFx, testHost: NetFx)]
    public void OnNetFrameworkRunner_ShouldUseJsonite(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var assemblyPaths = GetAssetFullPath("SimpleTestProject.dll");
        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), string.Empty, string.Empty, runnerInfo.InIsolationValue);

        InvokeVsTest(arguments);

        var diagLogs = GetDiagLogContents();
        Assert.Contains("Using Jsonite serializer", diagLogs,
            "Expected 'Using Jsonite serializer' in diag logs but not found.");
    }
}
