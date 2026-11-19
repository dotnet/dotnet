// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Core.Test
{
    public class PackageIdentityTests
    {
        [Theory]
        [InlineData("Example", "Example", null)]
        [InlineData("Example.Package@1.2.3", "Example.Package", "1.2.3")]
        [InlineData("Example-Package_1@1.2", "Example-Package_1", "1.2")]
        [InlineData("Example@1.2.3-beta.1+metadata", "Example", "1.2.3-beta.1+metadata")]
        public void Parse_ValidInput_ReturnsIdentity(string value, string id, string version)
        {
            var identity = PackageIdentity.Parse(value);

            Assert.Equal(id, identity.Id);
            Assert.Equal(version != null, identity.HasVersion);
            if (version != null)
            {
                Assert.Equal(version, identity.Version.OriginalVersion);
            }
        }

        [Fact]
        public void Parse_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => PackageIdentity.Parse(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData("@1.2.3")]
        [InlineData("Invalid/Id@1.2.3")]
        [InlineData("Invalid Id")]
        public void Parse_InvalidId_PreservesValidationError(string value)
        {
            var id = value.Split('@')[0];
            var expected = Assert.Throws<ArgumentException>(() => PackageIdValidator.ValidatePackageId(id, useRestrictedSet: false));

            var actual = Assert.Throws<ArgumentException>(() => PackageIdentity.Parse(value));

            Assert.Equal(expected.Message, actual.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("one")]
        [InlineData("1.0@2.0")]
        [InlineData("[1.0,2.0)")]
        public void Parse_InvalidVersion_PreservesInnerException(string version)
        {
            var expected = Assert.Throws<ArgumentException>(() => NuGetVersion.Parse(version));

            var actual = Assert.Throws<ArgumentException>(() => PackageIdentity.Parse("Example@" + version));

            Assert.Equal("value", actual.ParamName);
            Assert.Contains("Example", actual.Message);
            Assert.Contains(version, actual.Message);
            Assert.Equal(expected.Message, Assert.IsType<ArgumentException>(actual.InnerException).Message);
        }

        [Theory]
        [InlineData("Example", "Example", null)]
        [InlineData("Example.Package@1.2.3", "Example.Package", "1.2.3")]
        [InlineData("Example-Package_1@1.2", "Example-Package_1", "1.2")]
        [InlineData("Example@1.2.3-beta.1+metadata", "Example", "1.2.3-beta.1+metadata")]
        public void TryParse_ValidInput_ReturnsIdentity(string value, string id, string version)
        {
            Assert.True(PackageIdentity.TryParse(value, out var identity));

            Assert.NotNull(identity);
            Assert.Equal(id, identity.Id);
            Assert.Equal(version != null, identity.HasVersion);
            if (version != null)
            {
                Assert.Equal(version, identity.Version.OriginalVersion);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("@1.2.3")]
        [InlineData("Invalid/Id@1.2.3")]
        [InlineData("Invalid Id")]
        [InlineData("Example@")]
        [InlineData("Example@one")]
        [InlineData("Example@1.0@2.0")]
        [InlineData("Example@[1.0,2.0)")]
        public void TryParse_InvalidInput_ReturnsFalseAndNull(string value)
        {
            Assert.False(PackageIdentity.TryParse(value, out var identity));
            Assert.Null(identity);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ParseAndTryParse_IdExceedsPackLengthLimit_ReturnIdentity(bool hasVersion)
        {
            var id = new string('a', PackageIdValidator.MaxPackageIdLength + 1);
            var value = id + (hasVersion ? "@1.2.3" : "");

            var parsedIdentity = PackageIdentity.Parse(value);
            Assert.True(PackageIdentity.TryParse(value, out var triedIdentity));

            Assert.NotNull(triedIdentity);
            Assert.Equal(id, parsedIdentity.Id);
            Assert.Equal(id, triedIdentity.Id);
            Assert.Equal(hasVersion, parsedIdentity.HasVersion);
            Assert.Equal(hasVersion, triedIdentity.HasVersion);
            if (hasVersion)
            {
                Assert.Equal("1.2.3", parsedIdentity.Version.OriginalVersion);
                Assert.Equal("1.2.3", triedIdentity.Version.OriginalVersion);
            }
        }

        [Fact]
        public void TestToString()
        {
            var packageIdentity = new PackageIdentity("packageA", new NuGetVersion("1.0.0"));
            Assert.Equal("packageA.1.0.0", packageIdentity.ToString());

            var packageIdentityNoVersion = new PackageIdentity("packageB", null);
            Assert.Equal("packageB", packageIdentityNoVersion.ToString());

            var formattedString = string.Format("This is package '{0}'", packageIdentity);
            Assert.Equal("This is package 'packageA.1.0.0'", formattedString);
        }
    }
}
