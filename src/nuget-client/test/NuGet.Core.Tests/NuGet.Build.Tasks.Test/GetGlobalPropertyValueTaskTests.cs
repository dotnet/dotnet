// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetGlobalPropertyValueTaskTests
    {
        [Fact]
        public void Execute_WithoutSpecialGlobalProperties_ReturnsNull()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var task = new GetGlobalPropertyValueTask()
            {
                BuildEngine = buildEngine,
                PropertyName = "TargetFramework",
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.GlobalPropertyValue.Should().Be(null);
            task.CheckCompleted.Should().Be(true);
        }

        [Theory]
        [InlineData("TargetFramework")]
        [InlineData("targetframework")]
        public void Execute_WithGlobalProperties_ReturnsValue(string requestedProperty)
        {
            // Arrange
            string expectedValue = "net472";
            Dictionary<string, string> globalProps = new()
            {
                { "TargetFramework", expectedValue }
            };
            var buildEngine = new TestBuildEngine(globalProps);

            var task = new GetGlobalPropertyValueTask()
            {
                BuildEngine = buildEngine,
                PropertyName = requestedProperty,
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.GlobalPropertyValue.Should().Be(expectedValue);
            task.CheckCompleted.Should().Be(true);
        }

    }
}
