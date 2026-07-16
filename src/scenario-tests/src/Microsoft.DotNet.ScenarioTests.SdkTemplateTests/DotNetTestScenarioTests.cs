// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.ScenarioTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

/// <summary>
/// Exercises <c>dotnet test</c> end-to-end for MSTest projects while collecting code coverage, across both
/// supported test runners (VSTest and Microsoft.Testing.Platform) and both a current .NET target and .NET
/// Framework (net48). Each scenario generates a project with <c>dotnet new mstest</c>, runs the tests, and
/// asserts a coverage artifact was produced.
///
/// The .NET Framework legs only run on Windows because net48 test apps cannot execute on Linux/macOS; they
/// are gated with <c>SkipIfPlatform</c> traits (enforced by the harness in Program.cs).
/// </summary>
public class DotNetTestScenarioTests : IClassFixture<ScenarioTestFixture>
{
    private const string NetFrameworkTargetFramework = "net48";

    private readonly ScenarioTestFixture _scenarioTestInput;
    private readonly DotNetSdkHelper _sdkHelper;

    public DotNetTestScenarioTests(ScenarioTestFixture testInput, ITestOutputHelper outputHelper)
    {
        if (string.IsNullOrEmpty(testInput.DotNetRoot))
        {
            throw new ArgumentException("sdk root must be set for sdk tests");
        }

        _scenarioTestInput = testInput;
        _sdkHelper = new DotNetSdkHelper(outputHelper, _scenarioTestInput.DotNetRoot, _scenarioTestInput.SdkVersion, _scenarioTestInput.BinlogDir);
    }

    [Fact]
    public void VerifyMSTestVSTestWithCoverage() =>
        RunMSTestCoverageScenario("VSTest", targetFramework: null, useMicrosoftTestingPlatform: false);

    [Fact]
    [Trait("SkipIfPlatform", "LINUX")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyMSTestVSTestNetFrameworkWithCoverage() =>
        RunMSTestCoverageScenario("VSTestNetFx", targetFramework: NetFrameworkTargetFramework, useMicrosoftTestingPlatform: false);

    [Fact]
    public void VerifyMSTestMtpWithCoverage() =>
        RunMSTestCoverageScenario("Mtp", targetFramework: null, useMicrosoftTestingPlatform: true);

    [Fact]
    [Trait("SkipIfPlatform", "LINUX")]
    [Trait("SkipIfPlatform", "OSX")]
    public void VerifyMSTestMtpNetFrameworkWithCoverage() =>
        RunMSTestCoverageScenario("MtpNetFx", targetFramework: NetFrameworkTargetFramework, useMicrosoftTestingPlatform: true);

    private void RunMSTestCoverageScenario(string scenario, string? targetFramework, bool useMicrosoftTestingPlatform)
    {
        string projectName = $"{nameof(DotNetTestScenarioTests)}_{scenario}";
        string projectDirectory = Path.Combine(_scenarioTestInput.TestRoot, projectName);

        Directory.CreateDirectory(projectDirectory);

        if (useMicrosoftTestingPlatform)
        {
            // Anchor a local global.json before generating. The mstest template's
            // --test-runner Microsoft.Testing.Platform switch opts in by modifying the nearest global.json
            // it finds; without a local one it rewrites the shared repo-root global.json and breaks every
            // other generated test project. See DotNetSdkHelper.EnableTestingPlatformRunner.
            DotNetSdkHelper.EnableTestingPlatformRunner(projectDirectory);
        }

        // Generate a coherent MSTest project via the template's first-class switches (no csproj hand-editing):
        //   --test-runner selects VSTest vs Microsoft.Testing.Platform,
        //   --coverage-tool pins Microsoft.CodeCoverage so both runners collect coverage,
        //   --framework (when set) targets .NET Framework instead of the SDK's default TFM.
        string testRunner = useMicrosoftTestingPlatform ? "Microsoft.Testing.Platform" : "VSTest";
        string customArgs = $"--test-runner {testRunner} --coverage-tool Microsoft.CodeCoverage";
        if (!string.IsNullOrEmpty(targetFramework))
        {
            customArgs += $" --framework {targetFramework}";
        }

        _sdkHelper.ExecuteNew(DotNetSdkTemplate.MSTest.GetName(), projectName, projectDirectory, customArgs: customArgs);

        _sdkHelper.ExecuteTestWithCoverage(projectDirectory, useMicrosoftTestingPlatform);
    }
}
