// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.CommandLine;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.config
{
    public class ConfigCommandTests
    {
        [Fact]
        public void ConfigCommand_HasHelpUrl()
        {
            // Arrange
            CliCommand rootCommand = new("nuget");

            // Act
            ConfigCommand.Register(rootCommand, NullLoggerWithColor.GetInstance);

            // Assert
            rootCommand.Subcommands[0].Should().BeAssignableTo<DocumentedCommand>();
            ((DocumentedCommand)rootCommand.Subcommands[0]).HelpUrl.Should().NotBeNullOrEmpty();

            foreach (var subcommand in rootCommand.Subcommands)
            {
                subcommand.Should().BeAssignableTo<DocumentedCommand>();
                ((DocumentedCommand)subcommand).HelpUrl.Should().NotBeNullOrEmpty();
            }
        }
    }
}
