// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.CMake.Sdk.Tests;

public class ResolveVSGeneratorTests
{
    private static readonly XDocument s_targets = XDocument.Load(Path.Combine(
        AppContext.BaseDirectory,
        "testassets",
        "CMakeSdkBuild",
        "Microsoft.DotNet.CMake.Sdk.targets"));

    private static XElement ResolveVSGenerator => s_targets
        .Descendants("Target")
        .Single(element => element.Attribute("Name")?.Value == "ResolveVSGenerator");

    [Theory]
    [InlineData("16.", "Visual Studio 16 2019")]
    [InlineData("17.", "Visual Studio 17 2022")]
    [InlineData("18.", "Visual Studio 18 2026")]
    public void MapsVisualStudioVersionToGenerator(string versionPrefix, string generator)
    {
        Assert.Contains(
            ResolveVSGenerator.Descendants("VSGenerator"),
            element =>
                element.Value == generator &&
                element.Attribute("Condition")?.Value.Contains(
                    $"_HighestCompatibleVSVersion.StartsWith({versionPrefix})",
                    StringComparison.Ordinal) == true);
    }

    [Fact]
    public void DefaultVersionRangeIncludesEveryKnownVisualStudioVersion()
    {
        XElement versionRange = Assert.Single(ResolveVSGenerator.Descendants("VSGeneratorVersionRange"));

        Assert.Equal("[15,19.0)", versionRange.Value);
    }

    [Fact]
    public void MapsX86PlatformToWin32()
    {
        Assert.Contains(
            ResolveVSGenerator.Descendants("Platform"),
            element =>
                element.Value == "Win32" &&
                element.Attribute("Condition")?.Value.Contains(
                    "'$(Platform)' == 'x86'",
                    StringComparison.Ordinal) == true);
    }

    [Fact]
    public void UsesMultiConfigurationGeneratorAndPassesArchitecture()
    {
        Assert.Contains(
            ResolveVSGenerator.Descendants("_CMakeMultiConfigurationGenerator"),
            element => element.Value == "true");
        Assert.Contains(
            ResolveVSGenerator.Descendants("_CMakePassArchitectureToGenerator"),
            element => element.Value == "true");
    }

    [Fact]
    public void ReportsWhenCMakeDoesNotSupportTheResolvedGenerator()
    {
        Assert.Contains(
            ResolveVSGenerator.Descendants("Error"),
            element => element.Attribute("Text")?.Value.Contains(
                "does not support the '$(VSGenerator)' generator",
                StringComparison.Ordinal) == true);
    }

    [Fact]
    public void GeneratorSupportCheckUsesTheConfiguredCMakeCommandWrapper()
    {
        XElement help = GetCMakeHelpExec();

        Assert.Equal("$(CMakeConfigureCommandWrapper) cmake --help", help.Attribute("Command")?.Value);
    }

    [Fact]
    public void GeneratorSupportCheckIsSkippedInDesignTimeBuilds()
    {
        XElement help = GetCMakeHelpExec();

        Assert.Contains(
            "'$(DesignTimeBuild)' != 'true'",
            help.Attribute("Condition")?.Value,
            StringComparison.Ordinal);
    }

    private static XElement GetCMakeHelpExec() => Assert.Single(
        ResolveVSGenerator.Descendants("Exec"),
        element => element.Attribute("Command")?.Value.Contains(
            "cmake --help",
            StringComparison.Ordinal) == true);
}
