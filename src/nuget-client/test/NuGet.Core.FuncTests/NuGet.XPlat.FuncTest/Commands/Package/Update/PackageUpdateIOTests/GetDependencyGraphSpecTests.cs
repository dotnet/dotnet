// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

[Collection(XPlatCollection.Name)]
public class GetDependencyGraphSpecTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public void GetDependencyGraphSpec_WithValidProjectFile_ReturnsDgSpec()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var projectName = "TestProject";
        var projectPath = Path.Combine(testContext.SolutionRoot, $"{projectName}.csproj");

        var projectContent = @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
</Project>";

        File.WriteAllText(projectPath, projectContent);

        var envVars = new Dictionary<string, string>
        {
            ["DOTNET_HOST_PATH"] = TestFileSystemUtility.GetDotnetCli()
        };

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = packageUpdateIO.GetDependencyGraphSpec(projectPath);

        // Assert
        result.Should().NotBeNull();
        result.Projects.Should().ContainSingle();
    }

    [Fact]
    public void GetDependencyGraphSpec_WithInvalidProjectFile_ReturnsNull()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var nonExistentProject = Path.Combine(testContext.SolutionRoot, "NonExistent.csproj");

        var envVars = new Dictionary<string, string>
        {
            ["DOTNET_HOST_PATH"] = TestFileSystemUtility.GetDotnetCli()
        };

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = packageUpdateIO.GetDependencyGraphSpec(nonExistentProject);

        // Assert
        result.Should().BeNull();
    }
}
