// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.SourceBuild.Tests;

/// <summary>
/// Web project create, build, run, publish scenario tests not covered by the scenario-tests repo.
/// </summary>
public class WebScenarioTests : SdkTests
{
    public WebScenarioTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

    // [Theory] - Disabled until SDK templates are updated to .NET 11.0 - https://github.com/dotnet/source-build/issues/5422
    // [MemberData(nameof(GetScenarioObjects))]
    public void VerifyScenario(TestScenario scenario) => scenario.Execute(DotNetHelper);

    public static IEnumerable<object[]> GetScenarioObjects() => GetScenarios().Select(scenario => new object[] { scenario });

    private static IEnumerable<TestScenario> GetScenarios()
    {
        yield return new(nameof(WebScenarioTests), DotNetTemplate.WebApp, DotNetActions.PublishSelfContained, VerifyRuntimePacksForSelfContained);

        // Blazor test is only run on builds where Microsoft-built packages are available since it requires WASM packages.
        if (!Config.SkipTestsRequiringMicrosoftArtifacts)
        {
            yield return new(nameof(WebScenarioTests), DotNetTemplate.BlazorWasm, DotNetActions.Build | DotNetActions.Run | DotNetActions.Publish);
        }
    }

    private static void VerifyRuntimePacksForSelfContained(string projectPath)
    {
        // 'expectedPackageFiles' key in project.nuget.cache' will contain paths to restored packages
        // Since we are publishing an emtpy template, the only packages that could end up there are the ref packs we are after

        string projNugetCachePath = Path.Combine(projectPath, "obj", "project.nuget.cache");

        JsonNode? projNugetCache = JsonNode.Parse(File.ReadAllText(projNugetCachePath));
        JsonArray? restoredPackageFiles = (JsonArray?)projNugetCache?["expectedPackageFiles"];

        Assert.True(restoredPackageFiles is not null, "Failed to parse project.nuget.cache");

        string packagesDirectory = Path.Combine(Environment.CurrentDirectory, "packages");

        IEnumerable<string> packages = restoredPackageFiles.GetValues<string>()
            .Where(file => file is not null)
            .Select(file =>
            {
                string path = file.Substring(packagesDirectory.Length + 1); // trim the leading path up to the package name directory
                return path.Substring(0, path.IndexOf('/')); // trim the rest of the path
            });

        if (packages.Any())
        {
            Assert.Fail($"The following runtime packs were retrieved from NuGet instead of the SDK: {string.Join(",", packages.ToArray())}");
        }
    }
}
