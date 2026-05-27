// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Shared;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class NuGetFeatureFlagsTests
    {
        [Fact]
        public void UseSystemTextJsonDeserializationFeatureSwitch_Default_ReturnsFalse()
        {
            Assert.False(NuGetFeatureFlags.UseSystemTextJsonDeserializationFeatureSwitch);
        }

        [Fact]
        public void IsSystemTextJsonDeserializationEnabledByEnvironment_WhenEnvVarNotSet_ReturnsFalse()
        {
            Assert.False(NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(TestEnvironmentVariableReader.EmptyInstance));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("TRUE")]
        public void IsSystemTextJsonDeserializationEnabledByEnvironment_WhenEnvVarSetToTrue_ReturnsTrue(string value)
        {
            var env = new TestEnvironmentVariableReader(
                new Dictionary<string, string> { [NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar] = value });

            Assert.True(NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(env));
        }

        [Theory]
        [InlineData("false")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("anything")]
        public void IsSystemTextJsonDeserializationEnabledByEnvironment_WhenEnvVarSetToFalseOrUnrecognized_ReturnsFalse(string value)
        {
            var env = new TestEnvironmentVariableReader(
                new Dictionary<string, string> { [NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar] = value });

            Assert.False(NuGetFeatureFlags.IsSystemTextJsonDeserializationEnabledByEnvironment(env));
        }
    }
}
