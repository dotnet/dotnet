// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Stage;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Stage
{
    public class StagePushCommandLineParsingTests
    {
        public sealed record ExpectedStagePushArguments(
            string PackagePath,
            string? Source = null,
            string? ApiKey = null,
            string? GroupId = null,
            bool NoSymbols = false,
            string? ConfigFile = null,
            bool Interactive = false);

        public static TheoryData<string, ExpectedStagePushArguments> ValidArgumentsTestData => new()
        {
            {
                "package stage push package.nupkg",
                new ExpectedStagePushArguments(PackagePath: "package.nupkg")
            },
            {
                "package stage push package.nupkg --source source",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    Source: "source")
            },
            {
                "package stage push -s source package.nupkg",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    Source: "source")
            },
            {
                "package stage push package.nupkg --api-key key",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    ApiKey: "key")
            },
            {
                "package stage push package.nupkg -k key",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    ApiKey: "key")
            },
            {
                "package stage push package.nupkg --group release-group",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    GroupId: "release-group")
            },
            {
                "package stage push package.nupkg --no-symbols",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    NoSymbols: true)
            },
            {
                "package stage push package.nupkg --configfile NuGet.Config",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    ConfigFile: "NuGet.Config")
            },
            {
                "package stage push package.nupkg --interactive",
                new ExpectedStagePushArguments(
                    PackagePath: "package.nupkg",
                    Interactive: true)
            },
            {
                "package stage push \"path with spaces\\package.nupkg\" --group \"release group\"",
                new ExpectedStagePushArguments(
                    PackagePath: "path with spaces\\package.nupkg",
                    GroupId: "release group")
            },
            {
                "package stage push artifacts\\Contoso.1.0.0.nupkg -s staging-source -k secret --group release-group --no-symbols --configfile config\\NuGet.Config --interactive",
                new ExpectedStagePushArguments(
                    PackagePath: "artifacts\\Contoso.1.0.0.nupkg",
                    Source: "staging-source",
                    ApiKey: "secret",
                    GroupId: "release-group",
                    NoSymbols: true,
                    ConfigFile: "config\\NuGet.Config",
                    Interactive: true)
            },
        };

        public static TheoryData<string> InvalidArgumentsTestData => new()
        {
            "package stage push",
            "package stage push one.nupkg two.nupkg",
            "package stage push package.nupkg --source",
            "package stage push package.nupkg --api-key",
            "package stage push package.nupkg --group",
            "package stage push package.nupkg --configfile",
            "package stage push package.nupkg --no-symbols true",
            "package stage push package.nupkg --allow-insecure-connections",
            "package stage push package.nupkg --unknown",
        };

        [Theory]
        [MemberData(nameof(ValidArgumentsTestData))]
        public void PushCommand_WithValidArguments_SetsExpectedValues(
            string commandLine,
            ExpectedStagePushArguments expected)
        {
            // Arrange
            Command rootCommand = new("nuget");
            bool actionInvoked = false;
            RegisterPushCommand(rootCommand, args =>
            {
                actionInvoked = true;
                args.PackagePath.Should().Be(expected.PackagePath);
                args.Source.Should().Be(expected.Source);
                args.ApiKey.Should().Be(expected.ApiKey);
                args.GroupId.Should().Be(expected.GroupId);
                args.NoSymbols.Should().Be(expected.NoSymbols);
                args.ConfigFile.Should().Be(expected.ConfigFile);
                args.Interactive.Should().Be(expected.Interactive);
                return Task.FromResult(0);
            });

            // Act
            ParseResult result = rootCommand.Parse(commandLine);
            result.Invoke();

            // Assert
            result.Errors.Should().BeEmpty();
            actionInvoked.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsTestData))]
        public void PushCommand_WithInvalidArguments_HasParseError(string commandLine)
        {
            // Arrange
            Command rootCommand = new("nuget");
            bool actionInvoked = false;
            RegisterPushCommand(rootCommand, _ =>
            {
                actionInvoked = true;
                return Task.FromResult(0);
            });

            // Act
            ParseResult result = rootCommand.Parse(commandLine);

            // Assert
            result.Errors.Should().NotBeEmpty();
            actionInvoked.Should().BeFalse();
        }

        [Fact]
        public void NuGetCommandsAdd_RegistersStageUnderPackageWithProvidedInteractiveOption()
        {
            // Arrange
            RootCommand rootCommand = new();
            var packageCommand = new Command("package");
            rootCommand.Subcommands.Add(packageCommand);
            var interactiveOption = new Option<bool>("--interactive")
            {
                DefaultValueFactory = _ => true,
            };

            // Act
            NuGetCommands.Add(
                rootCommand,
                interactiveOption,
                virtualProjectBuilder: null);
            ParseResult result = rootCommand.Parse("package stage push package.nupkg");

            // Assert
            result.Errors.Should().BeEmpty();
            packageCommand.Subcommands.Should().Contain(command => command.Name == "stage");
            packageCommand.Subcommands
                .Single(command => command.Name == "stage")
                .Subcommands
                .Should().ContainSingle(command => command.Name == "push");
            result.GetValue(interactiveOption).Should().BeTrue();
        }

        private static void RegisterPushCommand(
            Command rootCommand,
            Func<StagePushCommandArgs, Task<int>> action)
        {
            var packageCommand = new Command("package");
            var stageCommand = new Command("stage");
            StagePushCommand.Register(
                stageCommand,
                new Option<bool>("--interactive"),
                NullLoggerWithColor.GetInstance,
                action);
            packageCommand.Subcommands.Add(stageCommand);
            rootCommand.Subcommands.Add(packageCommand);
        }
    }
}
