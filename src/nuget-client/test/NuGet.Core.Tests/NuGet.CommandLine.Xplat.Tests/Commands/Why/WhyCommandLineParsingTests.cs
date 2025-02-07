// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat.Commands;
using NuGet.CommandLine.XPlat.Commands.Why;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Why
{
    public class WhyCommandLineParsingTests
    {
        [Fact]
        public void WhyCommand_HasHelpUrl()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            // Act
            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance);

            // Assert
            rootCommand.Subcommands[0].Should().BeAssignableTo<DocumentedCommand>();
            ((DocumentedCommand)rootCommand.Subcommands[0]).HelpUrl.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void WithTwoArguments_PathAndPackageAreSet()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().Be(@"path\to\my.proj");
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().BeNullOrEmpty();
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse(@"nuget why path\to\my.proj packageid");
            result.Errors.Should().BeEmpty();
            result.Invoke();
        }

        [Fact]
        public void WithOneArguments_PackageIsSet()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().NotBeNull();
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().BeNullOrEmpty();
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse(@"nuget why packageid");
            result.Errors.Should().BeEmpty();
            result.Invoke();
        }

        [Fact]
        public void WithZeroArguments_HasParseError()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                throw new Exception("Should not get here");
            });

            // Act
            var result = rootCommand.Parse(@"nuget why");
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void WithThreeArguments_HasParseError()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                throw new Exception("Should not get here");
            });

            // Act
            var result = rootCommand.Parse(@"nuget why 1 2 3");
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("-f net8.0 my.proj packageid")]
        [InlineData("my.proj -f net8.0 packageid")]
        [InlineData("my.proj packageid -f net8.0")]
        public void FrameworkOption_CanBeAtAnyPosition(string args)
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().Be("my.proj");
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().Equal(["net8.0"]);
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse("nuget why " + args);
            result.Errors.Should().BeEmpty();
            result.Invoke();
        }

        [Theory]
        [InlineData("-f")]
        [InlineData("--framework")]
        public void FrameworkOption_CanBeLongOrShortForm(string arg)
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().Be("my.proj");
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().Equal(["net8.0"]);
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse($"nuget why my.proj packageid {arg} net8.0");
            result.Errors.Should().BeEmpty();
            result.Invoke();
        }

        [Fact]
        public void FrameworkOption_AcceptsMultipleValues()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().Be("my.proj");
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().Equal(["net8.0", "net481"]);
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse($"nuget why my.proj packageid -f net8.0 -f net481");
            result.Errors.Should().BeEmpty();
            result.Invoke();
        }

        [Fact]
        public void HelpOption_ShowsHelp()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            WhyCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, whyCommandArgs =>
            {
                // Assert
                whyCommandArgs.Path.Should().Be("my.proj");
                whyCommandArgs.Package.Should().Be("packageid");
                whyCommandArgs.Frameworks.Should().Equal(["net8.0", "net481"]);
                return Task.FromResult(0);
            });

            // Act
            var result = rootCommand.Parse($"nuget why -h");
            result.Errors.Should().BeEmpty();
            result.Action.Should().BeOfType<System.CommandLine.Help.HelpAction>();
        }
    }
}
