// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class NuGetPackageOwnersTests
    {
        [Fact]
        public void Constructor_WhenPackageOwnersNull_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new NuGetPackageOwners(packageOwners: null));

            Assert.Equal("packageOwners", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPackageOwnersEmpty_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
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
            ArgumentException exception = Assert.Throws<ArgumentException>(
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
            byte[] expectedBytes = CreateNuGetPackageOwners(packageOwners);
            var nugetPackageOwners = new NuGetPackageOwners(packageOwners);

            byte[] actualBytes = nugetPackageOwners.Encode();

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
            byte[] bytes = CreateNuGetPackageOwners();

            SignatureException exception = Assert.Throws<SignatureException>(() => NuGetPackageOwners.Read(bytes));

            Assert.Equal("The nuget-package-owners attribute is invalid.", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Read_WithInvalidPackageOwner_Throws(string packageOwner)
        {
            byte[] bytes = CreateNuGetPackageOwners(packageOwner);

            SignatureException exception = Assert.Throws<SignatureException>(() => NuGetPackageOwners.Read(bytes));

            Assert.Equal("The nuget-package-owners attribute is invalid.", exception.Message);
        }

        [Fact]
        public void Read_WithValidInput_ReturnsInstance()
        {
            var packageOwners = new[] { "a", "b", "c" };
            byte[] bytes = CreateNuGetPackageOwners(packageOwners);

            NuGetPackageOwners nugetPackageOwners = NuGetPackageOwners.Read(bytes);

            Assert.Equal(packageOwners, nugetPackageOwners.PackageOwners);
        }

        private static byte[] CreateNuGetPackageOwners(params string[] owners)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                foreach (string owner in owners)
                {
                    writer.WriteCharacterString(UniversalTagNumber.UTF8String, owner);
                }
            }

            return writer.Encode();
        }
    }
}
