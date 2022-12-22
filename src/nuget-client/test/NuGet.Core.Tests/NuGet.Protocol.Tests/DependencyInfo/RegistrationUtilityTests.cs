// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class RegistrationUtilityTests
    {
        [Theory]
        [InlineData("", "(, )")]
        [InlineData(null, "(, )")]
        [InlineData("1.0.0", "[1.0.0, )")]
        [InlineData("1.0.0-*", "[1.0.0-0, )")]
        [InlineData("1.0.*", "[1.0.0, )")]
        [InlineData("[1.0.0,2.0.0]", "[1.0.0, 2.0.0]")]
        public void CreateVersionRange_AcceptsValidAndEmpty(string input, string expectedString)
        {
            // Arrange
            var expected = VersionRange.Parse(expectedString);

            // Act
            var actual = RegistrationUtility.CreateVersionRange(input);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(" \t \n ")]
        [InlineData("[15.106.0.preview]")]
        public void CreateVersionRange_RejectsInvalid(string input)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => RegistrationUtility.CreateVersionRange(input));
            Assert.Equal($"'{input}' is not a valid version string.", exception.Message);
        }
    }
}
