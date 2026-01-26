// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class GetPackageSourceMappingTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public void GetPackageSourceMapping_WithNoMapping_ReturnsDisabledMapping()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = packageUpdateIO.GetPackageSourceMapping();

        // Assert
        result.Should().NotBeNull();
        result.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public void GetPackageSourceMapping_WithMapping_ReturnsConfiguredMapping()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var nugetConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key=""nuget.org"">
      <package pattern=""TestPackage.*"" />
    </packageSource>
  </packageSourceMapping>
</configuration>";
        File.WriteAllText(Path.Combine(testContext.SolutionRoot, "nuget.config"), nugetConfig);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = packageUpdateIO.GetPackageSourceMapping();

        // Assert
        result.Should().NotBeNull();
        result.IsEnabled.Should().BeTrue();
    }
}
