// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test;

[Collection(DotnetIntegrationCollection.Name)]
public sealed class DotnetRemovePackageTests(DotnetIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
{
    private readonly DotnetIntegrationTestFixture _fixture = fixture;
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public void RemovePkg_FileBasedApp()
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

        // Remove the package.
        _fixture.RunDotnetExpectSuccess(fbaDir, "package remove packageX --file app.cs", testOutputHelper: _testOutputHelper);

        // Verify the full content of the modified .cs file.
        var modifiedContent = File.ReadAllText(appFile);
        _testOutputHelper.WriteLine("after:\n" + modifiedContent);
        Assert.Equal(
            """
            Console.WriteLine();
            """,
            modifiedContent);
    }
}
