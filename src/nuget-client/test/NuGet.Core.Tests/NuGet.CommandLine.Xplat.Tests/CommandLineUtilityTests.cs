// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class CommandLineUtilityTests
    {
        [Fact]
        public void SplitAndJoinAcrossMultipleValues_EmptyInput()
        {
            // Arrange
            var inputs = new List<string>();

            // Act
            var result = XPlat.CommandLineUtility.SplitAndJoinAcrossMultipleValues(inputs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void SplitAndJoinAcrossMultipleValues_NullInput()
        {
            // Arrange
            List<string> inputs = null;

            // Act
            var result = XPlat.CommandLineUtility.SplitAndJoinAcrossMultipleValues(inputs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void SplitAndJoinAcrossMultipleValues_SingleInput()
        {
            // Arrange
            var inputs = new List<string> { "first; second; third" };

            // Act
            var result = XPlat.CommandLineUtility.SplitAndJoinAcrossMultipleValues(inputs);

            // Assert
            result.Should().BeEquivalentTo(new string[] { "first", "second", "third" });
        }

        [Fact]
        public void SplitAndJoinAcrossMultipleValues_MultipleInputs()
        {
            // Arrange
            var inputs = new List<string> { "first; second; third", "fourth ; second ; third " };

            // Act
            var result = XPlat.CommandLineUtility.SplitAndJoinAcrossMultipleValues(inputs);

            // Assert
            result.Should().BeEquivalentTo(new string[] { "first", "second", "third", "fourth", "second", "third" });
        }
    }
}
