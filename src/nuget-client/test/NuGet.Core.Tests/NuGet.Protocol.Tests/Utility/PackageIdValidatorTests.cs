// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
