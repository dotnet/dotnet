// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.CommandLine.XPlat;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test;

[Collection(DotnetIntegrationCollection.Name)]
public sealed class DotnetRemovePackageTests(DotnetIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
{
    private readonly DotnetIntegrationTestFixture _fixture = fixture;
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    // https://github.com/NuGet/Home/issues/14823: This should use `dotnet package remove` when it supports file-based apps.
    [Fact]
    public async Task RemovePkg_FileBasedApp()
    {
        using var pathContext = _fixture.CreateSimpleTestPathContext();

        // Create the file-based app.
        var fbaDir = Path.Join(pathContext.SolutionRoot, "fba");
        Directory.CreateDirectory(fbaDir);

        var appFile = Path.Join(fbaDir, "app.cs");
        File.WriteAllText(appFile, """
            #:package packageX@1.0.0
            Console.WriteLine();
            """);

        // Get project content.
        var virtualProject = _fixture.GetFileBasedAppVirtualProject(appFile, _testOutputHelper);
        _testOutputHelper.WriteLine("before:\n" + virtualProject.Content);
        Assert.Contains("""<PackageReference Include="packageX" Version="1.0.0" />""", virtualProject.Content);
        using var builder = new TestVirtualProjectBuilder(virtualProject);

        // Remove the package.
        using var outWriter = new StringWriter();
        using var errorWriter = new StringWriter();
        var testApp = new CommandLineApplication
        {
            Out = outWriter,
            Error = errorWriter,
        };
        RemovePackageReferenceCommand.Register(
            testApp,
            () => new TestLogger(_testOutputHelper),
            () => new RemovePackageReferenceCommandRunner(),
            () => builder);
        int result = testApp.Execute([
            "remove",
            "--project", appFile,
            "--package", "packageX",
        ]);

        var output = outWriter.ToString();
        var error = errorWriter.ToString();

        _testOutputHelper.WriteLine(output);
        _testOutputHelper.WriteLine(error);

        Assert.Equal(0, result);

        Assert.Empty(error);

        var modifiedProjectContent = builder.ModifiedContent;
        _testOutputHelper.WriteLine("after:\n" + modifiedProjectContent);
        Assert.DoesNotContain("PackageReference", modifiedProjectContent);
    }
}
