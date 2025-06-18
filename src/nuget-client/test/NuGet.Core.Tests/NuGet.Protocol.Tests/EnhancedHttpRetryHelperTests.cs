// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using FluentAssertions;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class EnhancedHttpRetryHelperTests
    {
        [Fact]
        public void NoEnvironmentVariablesSet_UsesDefaultValues()
        {
            // Arrange
            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());

            // Act
            EnhancedHttpRetryHelper helper = new(testEnvironmentVariableReader);

            // Assert
            helper.RetryCountOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultRetryCount);
            helper.DelayInMillisecondsOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultDelayMilliseconds);
            helper.Retry429OrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultRetry429);
            helper.ObserveRetryAfterOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultObserveRetryAfter);
            helper.MaxRetryAfterDelayOrDefault.Should().Be(TimeSpan.FromSeconds(EnhancedHttpRetryHelper.DefaultMaximumRetryAfterDelayInSeconds));
            helper.DelayInMilliseconds.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("true")]
        [InlineData("something")]
        [InlineData("-5")]
        public void RetryCount_InvalidIntValue_UsesDefault(string value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = value
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.RetryCountOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultRetryCount);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void RetryCount_ValidIntValue_UsesValue(int value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.RetryCountOrDefault.Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("true")]
        [InlineData("something")]
        [InlineData("-5")]
        public void DelayInMilliseconds_InvalidIntValue_UsesDefault(string value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = value
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.DelayInMillisecondsOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultDelayMilliseconds);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void DelayInMilliseconds_ValidIntValue_UsesValue(int value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.DelayInMillisecondsEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.DelayInMillisecondsOrDefault.Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TRUEEEE")]
        [InlineData("something")]
        [InlineData("-5")]
        public void Retry429_InvalidBoolValue_UsesDefault(string value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.Retry429EnvironmentVariableName] = value
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.Retry429OrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultRetry429);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Retry429_ValidBoolValue_UsesValue(bool value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.Retry429EnvironmentVariableName] = value.ToString().ToLowerInvariant()
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.Retry429OrDefault.Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TRUEEEE")]
        [InlineData("something")]
        [InlineData("-5")]
        public void ObserveRetryAfter_InvalidBoolValue_UsesDefault(string value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.ObserveRetryAfterEnvironmentVariableName] = value
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.ObserveRetryAfterOrDefault.Should().Be(EnhancedHttpRetryHelper.DefaultRetry429);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ObserveRetryAfter_ValidBoolValue_UsesValue(bool value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.ObserveRetryAfterEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.ObserveRetryAfterOrDefault.Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("true")]
        [InlineData("something")]
        [InlineData("-5")]
        public void MaxRetryAfterDelay_InvalidIntValue_UsesDefault(string value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.MaximumRetryAfterDurationEnvironmentVariableName] = value
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.MaxRetryAfterDelayOrDefault.Should().Be(TimeSpan.FromSeconds(EnhancedHttpRetryHelper.DefaultMaximumRetryAfterDelayInSeconds));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void MaxRetryAfterDelay_ValidIntValue_UsesValue(int value)
        {
            // Arrange
            var environmentReader = new TestEnvironmentVariableReader(new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.MaximumRetryAfterDurationEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            });

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            helper.MaxRetryAfterDelayOrDefault.Should().Be(TimeSpan.FromSeconds(value));
        }
    }
}
