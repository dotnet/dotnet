// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using FluentAssertions;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public class NuGetFeatureFlagConstantsTests
    {
        [Fact]
        public void Constructor_WithNullFlightFlag_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new NuGetFeatureFlagConstants(null, "value", defaultState: true));
        }

        [Fact]
        public void Constructor_WithNullFlightExperimentalVariable_DoesNotThrow()
        {
            var constant = new NuGetFeatureFlagConstants("value", null, defaultState: true);

            constant.EnvironmentVariable.Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GeDefaultFeatureFlag_ReturnsValueThatMatchesConstructor(bool defaultFeatureFlag)
        {
            var constant = new NuGetFeatureFlagConstants("value", null, defaultFeatureFlag);

            constant.DefaultState.Should().Be(defaultFeatureFlag);
        }
    }
}
