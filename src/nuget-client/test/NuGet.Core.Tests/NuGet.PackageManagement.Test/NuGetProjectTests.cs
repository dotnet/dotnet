// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Test;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class NuGetProjectTests
    {
        [Theory]
        [InlineData("Name", "test")]
        [InlineData("DoesNotExist", null)]
        public void GetMetadataOrNull_TestProject_HasExpectedValue(string key, string expected)
        {
            // Arrange
            var project = new TestNuGetProject("test", null);

            // Act
            var result = project.GetMetadataOrNull(key);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
