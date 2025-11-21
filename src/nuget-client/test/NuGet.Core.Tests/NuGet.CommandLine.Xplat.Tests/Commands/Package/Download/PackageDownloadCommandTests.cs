// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Download
{
    public class PackageDownloadCommandArgsTest
    {
        [Fact]
        public async Task NoArguments_ThrowsAnExceptionForMissingArg()
        {
            // Arrange
            string args = "package download";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => RunAsync(args));
        }

        [Fact]
        public async Task WithSinglePackage_ShouldParsePackageId()
        {
            // Arrange
            string args = "package download Contoso.Utils";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Packages.Should().HaveCount(1);
            result.Packages[0].Id.Should().Be("Contoso.Utils");
            result.Packages[0].NuGetVersion.Should().BeNull();
        }

        [Fact]
        public async Task WithMultiplePackages_ShouldParseAllPackageIds()
        {
            // Arrange
            string args = "package download Contoso.Utils Contoso.Framework";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Packages.Should().HaveCount(2);
            result.Packages[0].Id.Should().Be("Contoso.Utils");
            result.Packages[0].NuGetVersion.Should().BeNull();
            result.Packages[1].Id.Should().Be("Contoso.Framework");
            result.Packages[1].NuGetVersion.Should().BeNull();
        }

        [Fact]
        public async Task WithPackageAndVersion_ShouldParsePackageWithVersion()
        {
            // Arrange
            string args = "package download Contoso.Utils@2.1.0";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Packages.Should().HaveCount(1);
            result.Packages[0].Id.Should().Be("Contoso.Utils");
            result.Packages[0].NuGetVersion.Should().NotBeNull();
            result.Packages[0].NuGetVersion!.ToString().Should().Be("2.1.0");
        }

        [Fact]
        public async Task WithPackageAndVersionRange_ShouldFail()
        {
            // Arrange
            string args = "package download Contoso.Utils@[2.0.0,3.0.0)";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => RunAsync(args));
        }

        [Fact]
        public async Task WithMixedPackages_ShouldParseMixedPackagesCorrectly()
        {
            // Arrange
            string args = "package download Contoso.Utils@2.1.0 Contoso.Framework";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Packages.Should().HaveCount(2);
            result.Packages[0].Id.Should().Be("Contoso.Utils");
            result.Packages[0].NuGetVersion.ToString().Should().Be("2.1.0");
            result.Packages[1].Id.Should().Be("Contoso.Framework");
            result.Packages[1].NuGetVersion.Should().BeNull();
        }

        [Fact]
        public async Task WithOutputAndConfig_ShouldBindPaths()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string outDir = Path.Combine(pathContext.WorkingDirectory, "out");
            string cfg = Path.Combine(pathContext.WorkingDirectory, "nuget.config");

            string args = $"package download Contoso --output \"{outDir}\" --configfile \"{cfg}\"";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.OutputDirectory.Should().Be(outDir);
            result.ConfigFile.Should().Be(cfg);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task WithInteractiveOption_ShouldSetCorrectInteractiveValue(bool value)
        {
            // Arrange
            string args = $"package download Contoso --interactive:{value}";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Interactive.Should().Be(value);
        }

        [Fact]
        public async Task WithBooleanFlags_ShouldSetAllFlagsTrue()
        {
            // Arrange
            string args = "package download contoso --prerelease --allow-insecure-connections --interactive";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.IncludePrerelease.Should().BeTrue();
            result.AllowInsecureConnections.Should().BeTrue();
            result.Interactive.Should().BeTrue();
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
            string args = $"package download contoso {verbosityArgs}";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.LogLevel.Should().Be(expectedLogLevel);
        }

        [Fact]
        public void WithInvalidVersion_ShouldHaveParseErrors()
        {
            // Arrange
            string args = "package download Contoso.Utils@invalid-version";

            // Act
            var result = Parse(args);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WithEmptyVersionAfterAt_ShouldHaveParseErrors()
        {
            // Arrange
            string args = "package download Contoso.Utils@";

            // Act
            var result = Parse(args);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task WithAllOptions_ShouldParseAllOptionsCorrectly()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            string outDir = Path.Combine(pathContext.WorkingDirectory, "out");
            string cfg = Path.Combine(pathContext.WorkingDirectory, "nuget.config");

            string args = $"package download Contoso.Utils@2.1.0 --output \"{outDir}\" --configfile \"{cfg}\" --prerelease --allow-insecure-connections --source s1 --source s2 --verbosity detailed --interactive";

            // Act
            var result = await RunAsync(args);

            // Assert
            result.Should().NotBeNull();
            result.Packages.Should().HaveCount(1);
            result.Packages[0].Id.Should().Be("Contoso.Utils");
            result.Packages[0].NuGetVersion.ToString().Should().Be("2.1.0");
            result.OutputDirectory.Should().Be(outDir);
            result.ConfigFile.Should().Be(cfg);
            result.IncludePrerelease.Should().BeTrue();
            result.AllowInsecureConnections.Should().BeTrue();
            result.Sources.Should().ContainInOrder("s1", "s2");
            result.LogLevel.Should().Be(LogLevel.Verbose);
            result.Interactive.Should().BeTrue();
        }

        private ParseResult Parse(string commandLine, Func<PackageDownloadArgs, CancellationToken, Task<int>> action = null)
        {
            RootCommand rootCommand = new RootCommand();

            var packageCommand = new Command("package");
            rootCommand.Subcommands.Add(packageCommand);

            // Simulate SDK-provided interactive option
            var interactiveOption = new Option<bool>("--interactive");

            if (action == null)
            {
                action = (_, _) => throw new NotImplementedException("No action provided for command execution.");
            }

            PackageDownloadCommand.Register(packageCommand, interactiveOption, action);

            var parser = rootCommand.Parse(commandLine);
            return parser;
        }

        private async Task<PackageDownloadArgs> RunAsync(string commandLine)
        {
            PackageDownloadArgs commandArgs = null;

            var parseResult = Parse(commandLine, (args, cancellationToken) =>
            {
                commandArgs = args;
                return Task.FromResult(0);
            });

            using var output = new StringWriter();
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
}
