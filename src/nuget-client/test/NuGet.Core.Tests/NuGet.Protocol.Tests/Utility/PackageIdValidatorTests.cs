// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Moq;
using NuGet.Common;
using NuGet.Packaging;
using Xunit;

namespace NuGet.Protocol.Tests.Utility
{
    public class PackageIdValidatorTests
    {
        [Theory]
        [InlineData("../contoso")]
        [InlineData("contoso/../package")]
        [InlineData("contoso/.?///?")]
        public void Validate_InvalidId_Throws(string id)
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidPackageIdException>(() => PackageIdValidator.Validate(id));
            exception.Message.Contains(id);
        }

        [Fact]
        public void Validate_EnvironmentVariableSet_DoesNotThrow()
        {
            // Arrange
            var environment = new Mock<IEnvironmentVariableReader>();
            environment.Setup(e => e.GetEnvironmentVariable("NUGET_DISABLE_PACKAGEID_VALIDATION"))
                       .Returns("true");

            // Act & Assert
            // This should not throw for an invalid package ID
            PackageIdValidator.Validate("contoso/../package", environment.Object);
            Assert.True(true);
        }

        [Theory]
        [InlineData("contoso")]
        [InlineData("contoso.package.package")]
        [InlineData("contoso.package")]
        public void Validate_ValidId_DoesNotThrow(string id)
        {
            // Act & Assert
            PackageIdValidator.Validate(id);
            Assert.True(true);
        }

        [Fact]
        public void Validate_MoreThan100Chars_Succeeds()
        {
            // Act & Assert
            PackageIdValidator.Validate(new string('a', 200));
            Assert.True(true);
        }
    }
}
