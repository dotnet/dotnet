// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update;

public class CliParserTests
{
    [Fact]
    public async Task NoArguments_HasDefaultOptions()
    {
        // Arrange
        string args = "package update";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().BeEmpty();
        result.Interactive.Should().BeFalse();
        result.LogLevel.Should().Be(LogLevel.Information);
        result.Vulnerable.Should().BeFalse();
    }

    [Fact]
    public async Task WithSinglePackage_ShouldParsePackageId()
    {
        // Arrange
        string args = "package update Contoso.Utils";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(1);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().BeNull();
    }

    [Fact]
    public async Task WithMultiplePackages_ShouldParseAllPackageIds()
    {
        // Arrange
        string args = "package update Contoso.Utils Contoso.Framework";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(2);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().BeNull();
        result.Packages[1].Id.Should().Be("Contoso.Framework");
        result.Packages[1].VersionRange.Should().BeNull();
    }

    [Fact]
    public async Task WithPackageAndVersion_ShouldParsePackageWithVersionRange()
    {
        // Arrange
        string args = "package update Contoso.Utils@2.1.0";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(1);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().NotBeNull();
        result.Packages[0].VersionRange!.ToString().Should().Be("[2.1.0, )");
    }

    [Fact]
    public async Task WithPackageAndVersionRange_ShouldParsePackageWithVersionRange()
    {
        // Arrange
        string args = "package update Contoso.Utils@[2.0.0,3.0.0)";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(1);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().NotBeNull();
        result.Packages[0].VersionRange!.ToString().Should().Be("[2.0.0, 3.0.0)");
    }

    [Fact]
    public async Task WithMixedPackages_ShouldParseMixedPackagesCorrectly()
    {
        // Arrange
        string args = "package update Contoso.Utils@2.1.0 Contoso.Framework";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(2);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().NotBeNull();
        result.Packages[1].Id.Should().Be("Contoso.Framework");
        result.Packages[1].VersionRange.Should().BeNull();
    }

    [Fact]
    public async Task WithProjectOption_ShouldSetProjectPath()
    {
        // Arrange
        using var pathContext = new SimpleTestPathContext();
        string projectPath = Path.Combine(pathContext.WorkingDirectory, "test.csproj");
        File.WriteAllText(projectPath, "<Project />");

        string args = $"package update --project \"{projectPath}\"";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Project.Should().Be(projectPath);
    }

    [Fact]
    public void WithProjectThatDoesNotExist_ShouldHaveError()
    {
        // Arrange
        using var pathContext = new SimpleTestPathContext();
        string projectPath = Path.Combine(pathContext.WorkingDirectory, "test.csproj");

        string args = $"package update --project \"{projectPath}\"";

        // Act
        var result = Parse(args);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task WithInteractiveOption_ShouldSetCorrectInteractiveValue(bool value)
    {
        // Arrange
        string args = $"package update --interactive:{value}";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Interactive.Should().Be(value);
    }

    [Theory]
    [InlineData("--verbosity quiet", LogLevel.Warning)]
    [InlineData("--verbosity q", LogLevel.Warning)]
    [InlineData("--verbosity minimal", LogLevel.Minimal)]
    [InlineData("--verbosity m", LogLevel.Minimal)]
    [InlineData("--verbosity normal", LogLevel.Information)]
    [InlineData("--verbosity n", LogLevel.Information)]
    [InlineData("--verbosity detailed", LogLevel.Verbose)]
    [InlineData("--verbosity d", LogLevel.Verbose)]
    [InlineData("--verbosity diagnostic", LogLevel.Debug)]
    [InlineData("--verbosity diag", LogLevel.Debug)]
    [InlineData("-v quiet", LogLevel.Warning)]
    [InlineData("-v minimal", LogLevel.Minimal)]
    [InlineData("-v normal", LogLevel.Information)]
    [InlineData("-v detailed", LogLevel.Verbose)]
    [InlineData("-v diagnostic", LogLevel.Debug)]
    public async Task WithVerbosityOption_ShouldSetCorrectLogLevel(string verbosityArgs, LogLevel expectedLogLevel)
    {
        // Arrange
        string args = $"package update {verbosityArgs}";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.LogLevel.Should().Be(expectedLogLevel);
    }

    [Fact]
    public async Task WithAllOptions_ShouldParseAllOptionsCorrectly()
    {
        // Arrange
        using var pathContext = new SimpleTestPathContext();
        string projectPath = Path.Combine(pathContext.WorkingDirectory, "test.csproj");
        File.WriteAllText(projectPath, "<Project />");

        string args = $"package update Contoso.Utils@2.1.0 --project \"{projectPath}\" --interactive --verbosity detailed";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Should().NotBeNull();
        result.Packages.Should().HaveCount(1);
        result.Packages[0].Id.Should().Be("Contoso.Utils");
        result.Packages[0].VersionRange.Should().NotBeNull();
        result.Project.Should().Be(projectPath);
        result.Interactive.Should().BeTrue();
        result.LogLevel.Should().Be(LogLevel.Verbose);
    }

    [Fact]
    public void WithInvalidVersionRange_ShouldHaveParseErrors()
    {
        // Arrange
        string args = "package update Contoso.Utils@invalid-version";

        // Act
        var result = Parse(args);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WithEmptyVersionAfterAt_ShouldHaveParseErrors()
    {
        // Arrange
        string args = "package update Contoso.Utils@";

        // Act
        var result = Parse(args);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WithNonExistentProject_ShouldHaveParseErrors()
    {
        // Arrange
        string args = "package update --project non-existent-file.csproj";

        // Act
        var result = Parse(args);

        // Assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task WithVulnerableOption_ShouldSetVulnerableFlag()
    {
        // Arrange
        string args = $"package update --vulnerable";

        // Act
        var result = await RunAsync(args);

        // Assert
        result.Vulnerable.Should().BeTrue();
    }

    private ParseResult Parse(string commandLine, Func<PackageUpdateArgs, CancellationToken, Task<int>>? action = null)
    {
        RootCommand rootCommand = new RootCommand();

        var packageCommand = new Command("package");
        rootCommand.Subcommands.Add(packageCommand);

        // The product code gets the interactive option from the .NET SDK, so we simulate it here.
        var interactiveOption = new Option<bool>("--interactive");

        if (action == null)
        {
            action = (_, _) => throw new NotImplementedException("No action provided for command execution.");
        }

        PackageUpdateCommand.Register(packageCommand, interactiveOption, action);

        var parser = rootCommand.Parse(commandLine);
        return parser;
    }

    private async Task<PackageUpdateArgs> RunAsync(string commandLine)
    {
        PackageUpdateArgs? commandArgs = null;

        var parseResult = Parse(commandLine, (args, cancellationToken) =>
        {
            commandArgs = args;
            return Task.FromResult(0);
        });

        TextWriter output = new StringWriter();
        var commandLineConfiguration = new InvocationConfiguration
        {
            Output = output,
            Error = output,
        };

        await parseResult.InvokeAsync(commandLineConfiguration);

        if (commandArgs is null)
        {
            throw new InvalidOperationException("Command arguments were not set during command execution. Output:" + output.ToString());
        }

        return commandArgs;
    }
}
