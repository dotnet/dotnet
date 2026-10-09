// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Globalization;
using FluentAssertions;
using NuGet.CommandLine.XPlat.Commands.NuGet.Add;
using NuGet.CommandLine.XPlat.Commands.NuGet.Update;
using NuGet.Commands;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Source;

public class DotnetNuGetSourceCommandTests
{
    [Theory]
    [InlineData("add source https://source.test", true)]
    [InlineData("update source test_source", false)]
    public void MinPublishAgeHours_WhenNotSpecified_IsNull(string commandLine, bool isAddCommand)
    {
        // Arrange
        AddSourceArgs? addArgs = null;
        UpdateSourceArgs? updateArgs = null;
        Command rootCommand = RegisterCommands(args => addArgs = args, args => updateArgs = args);

        // Act
        var result = rootCommand.Parse(commandLine);
        result.Invoke();

        // Assert
        result.Errors.Should().BeEmpty();
        (isAddCommand ? addArgs?.MinPublishAgeHours : updateArgs?.MinPublishAgeHours).Should().BeNull();
    }

    [Theory]
    [InlineData("add source https://source.test --min-publish-age-hours 0", 0u, true)]
    [InlineData("add source https://source.test --min-publish-age-hours 72", 72u, true)]
    [InlineData("update source test_source --min-publish-age-hours 0", 0u, false)]
    [InlineData("update source test_source --min-publish-age-hours 72", 72u, false)]
    public void MinPublishAgeHours_WhenValid_ParsesValue(string commandLine, uint expected, bool isAddCommand)
    {
        // Arrange
        AddSourceArgs? addArgs = null;
        UpdateSourceArgs? updateArgs = null;
        Command rootCommand = RegisterCommands(args => addArgs = args, args => updateArgs = args);

        // Act
        var result = rootCommand.Parse(commandLine);
        result.Invoke();

        // Assert
        result.Errors.Should().BeEmpty();
        (isAddCommand ? addArgs?.MinPublishAgeHours : updateArgs?.MinPublishAgeHours).Should().Be(expected);
    }

    [Theory]
    [InlineData("add source https://source.test")]
    [InlineData("update source test_source")]
    public void MinPublishAgeHours_WhenNegative_HasParseError(string commandLine)
    {
        // Arrange
        bool invoked = false;
        Command rootCommand = RegisterCommands(_ => invoked = true, _ => invoked = true);

        // Act
        var result = rootCommand.Parse($"{commandLine} --min-publish-age-hours -1");
        result.Invoke();

        // Assert
        result.Errors.Should().ContainSingle();
        invoked.Should().BeFalse();
    }

    [Theory]
    [InlineData("add source https://source.test")]
    [InlineData("update source test_source")]
    public void MinPublishAgeHours_WhenLargerThanTimeSpan_HasValidationError(string commandLine)
    {
        // Arrange
        bool invoked = false;
        Command rootCommand = RegisterCommands(_ => invoked = true, _ => invoked = true);
        uint value = uint.MaxValue;
        string expectedError = string.Format(
            CultureInfo.CurrentCulture,
            global::NuGet.CommandLine.XPlat.Strings.SourcesCommandMinPublishAgeHoursOutOfRange,
            value,
            (uint)TimeSpan.MaxValue.TotalHours);

        // Act
        var result = rootCommand.Parse($"{commandLine} --min-publish-age-hours {value}");
        result.Invoke();

        // Assert
        result.Errors.Should().ContainSingle().Which.Message.Should().Be(expectedError);
        invoked.Should().BeFalse();
    }

    private static Command RegisterCommands(Action<AddSourceArgs> runAddSource, Action<UpdateSourceArgs> runUpdateSource)
    {
        Command rootCommand = new("nuget");
        DotnetNuGetAddCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, runAddSource);
        DotnetNuGetUpdateCommand.Register(rootCommand, NullLoggerWithColor.GetInstance, runUpdateSource);
        return rootCommand;
    }
}
