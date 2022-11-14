// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class EnhancedHttpRetryHelperTests
    {
        [Fact]
        public void NoEnvionrmentVaraiblesSet_UsesDefaultValues()
        {
            // Arrange
            TestEnvironmentVariableReader testEnvironmentVariableReader = new TestEnvironmentVariableReader(new Dictionary<string, string>());

            // Act
            EnhancedHttpRetryHelper helper = new(testEnvironmentVariableReader);

            // Assert
            Assert.Equal(helper.IsEnabled, EnhancedHttpRetryHelper.DefaultEnabled);
            Assert.Equal(helper.RetryCount, EnhancedHttpRetryHelper.DefaultRetryCount);
            Assert.Equal(helper.DelayInMilliseconds, EnhancedHttpRetryHelper.DefaultDelayMilliseconds);
            Assert.Equal(helper.Retry429, EnhancedHttpRetryHelper.DefaultRetry429);
        }

        [Theory]
        [InlineData("")]
        [InlineData("5")]
        [InlineData("something")]
        public void InvalidBoolValue_UsesDefault(string value)
        {
            // Arrange
            var dict = new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.IsEnabledEnvironmentVariableName] = value
            };
            var environmentReader = new TestEnvironmentVariableReader(dict);

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            Assert.Equal(helper.IsEnabled, EnhancedHttpRetryHelper.DefaultEnabled);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidBoolValue_UsesValue(bool value)
        {
            // Arrange
            var dict = new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.IsEnabledEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            };
            var environmentReader = new TestEnvironmentVariableReader(dict);

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            Assert.Equal(helper.IsEnabled, value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("true")]
        [InlineData("something")]
        [InlineData("-5")]
        public void InvalidIntValue_UsesDefault(string value)
        {
            // Arrange
            var dict = new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = value
            };
            var environmentReader = new TestEnvironmentVariableReader(dict);

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            Assert.Equal(helper.RetryCount, EnhancedHttpRetryHelper.DefaultRetryCount);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void ValidIntValue_UsesValue(int value)
        {
            // Arrange
            var dict = new Dictionary<string, string>()
            {
                [EnhancedHttpRetryHelper.RetryCountEnvironmentVariableName] = value.ToString().ToLowerInvariant()
            };
            var environmentReader = new TestEnvironmentVariableReader(dict);

            // Act
            EnhancedHttpRetryHelper helper = new(environmentReader);

            // Assert
            Assert.Equal(helper.RetryCount, value);
        }

    }
}
