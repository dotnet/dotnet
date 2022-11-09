// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using BcAttribute = Org.BouncyCastle.Asn1.Cms.Attribute;

namespace NuGet.Packaging.Test
{
    public class NuGetPackageOwnersTests
    {
        [Fact]
        public void Constructor_WhenPackageOwnersNull_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new NuGetPackageOwners(packageOwners: null));

            Assert.Equal("packageOwners", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPackageOwnersEmpty_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new NuGetPackageOwners(Array.Empty<string>()));

            Assert.Equal("packageOwners", exception.ParamName);
            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WhenPackageOwnersContainsInvalidValue_Throws(string packageOwner)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new NuGetPackageOwners(new[] { packageOwner }));

            Assert.Equal("packageOwners", exception.ParamName);
            Assert.StartsWith("One or more package owner values are invalid.", exception.Message);
        }

        [Fact]
        public void Constructor_WithValidInput_InitializesProperty()
        {
            var packageOwners = new[] { "a", "b", "c" };
            var nugetPackageOwners = new NuGetPackageOwners(packageOwners);

            Assert.Equal(packageOwners, nugetPackageOwners.PackageOwners);
        }

        [Fact]
        public void Encode_ReturnsValidDer()
        {
            var packageOwners = new[] { "a", "b", "c" };

            var sequence = new DerSequence(
                new DerUtf8String("a"),
                new DerUtf8String("b"),
                new DerUtf8String("c"));
            var expectedBytes = sequence.GetDerEncoded();
            var nugetPackageOwners = new NuGetPackageOwners(packageOwners);

            var actualBytes = nugetPackageOwners.Encode();

            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(() => NuGetPackageOwners.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithEmptySequence_Throws()
        {
            var bytes = new DerSequence().GetDerEncoded();

            var exception = Assert.Throws<SignatureException>(() => NuGetPackageOwners.Read(bytes));

            Assert.Equal("The nuget-package-owners attribute is invalid.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Read_WithInvalidPackageOwner_Throws(string packageOwner)
        {
            var bytes = new DerSequence(new DerUtf8String(packageOwner)).GetDerEncoded();

            var exception = Assert.Throws<SignatureException>(() => NuGetPackageOwners.Read(bytes));

            Assert.Equal("The nuget-package-owners attribute is invalid.", exception.Message);
        }

        [Fact]
        public void Read_WithValidInput_ReturnsInstance()
        {
            var packageOwners = new[] { "a", "b", "c" };
            var sequence = new DerSequence(
                new DerUtf8String("a"),
                new DerUtf8String("b"),
                new DerUtf8String("c"));
            var bytes = sequence.GetDerEncoded();

            var nugetPackageOwners = NuGetPackageOwners.Read(bytes);

            Assert.Equal(packageOwners, nugetPackageOwners.PackageOwners);
        }
    }
}
