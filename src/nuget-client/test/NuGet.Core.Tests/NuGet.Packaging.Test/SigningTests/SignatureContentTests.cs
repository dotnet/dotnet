// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignatureContentTests
    {
        [Fact]
        public void Constructor_WhenSigningSpecificationsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SignatureContent(
                    signingSpecifications: null,
                    hashAlgorithm: HashAlgorithmName.SHA384,
                    hashValue: "a"));

            Assert.Equal("signingSpecifications", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_WhenHashValueNullOrEmpty_Throws(string hashValue)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new SignatureContent(
                    SigningSpecifications.V1,
                    HashAlgorithmName.SHA384,
                    hashValue));

            Assert.Equal("hashValue", exception.ParamName);
        }

        [Fact]
        public void Constructor_InitializesProperties()
        {
            var content = new SignatureContent(SigningSpecifications.V1, HashAlgorithmName.SHA384, "a");

            Assert.Equal(HashAlgorithmName.SHA384, content.HashAlgorithm);
            Assert.Equal("a", content.HashValue);
        }

        [Fact]
        public void GetBytes_ReturnsValidContent()
        {
            var content = new SignatureContent(SigningSpecifications.V1, HashAlgorithmName.SHA256, "a");

            var bytes = content.GetBytes();

            Assert.Equal("Version:1\n\n2.16.840.1.101.3.4.2.1-Hash:a\n\n", Encoding.UTF8.GetString(bytes));
        }

        [Fact]
        public void Load_IfBytesIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignatureContent.Load(bytes: null, signingSpecifications: SigningSpecifications.V1));

            Assert.Equal("bytes", exception.ParamName);
        }

        [Fact]
        public void Load_IfSigningSpecificationsIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignatureContent.Load(new byte[] { }, signingSpecifications: null));

            Assert.Equal("signingSpecifications", exception.ParamName);
        }

        [Fact]
        public void Load_IfSignatureFormatVersionIsUnsupported_Throws()
        {
            var bytes = Encoding.UTF8.GetBytes("Version:2\n\n");

            var exception = Assert.Throws<SignatureException>(
                () => SignatureContent.Load(bytes, SigningSpecifications.V1));

            Assert.Equal(NuGetLogCode.NU3007, exception.Code);
            Assert.Equal(
                "The package signature format version is not supported. Updating your client may solve this problem.",
                exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Version:1")]
        [InlineData("Version:1\n")]
        [InlineData("Version:1\n\n")]
        public void Load_IfContentIsInvalid_Throws(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            var exception = Assert.Throws<SignatureException>(
                () => SignatureContent.Load(bytes, SigningSpecifications.V1));

            Assert.Equal("The package signature content is invalid.", exception.Message);
        }

        [Theory]
        [InlineData("Version:1\n\na:b\n\n")]
        [InlineData("Version:1\n\n1.3.14.3.2.26-Hash:b\n\n")]
        public void Load_IfHashAlgorithmIsUnsupported_Throws(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            var exception = Assert.Throws<SignatureException>(
                () => SignatureContent.Load(bytes, SigningSpecifications.V1));

            Assert.Equal("Package hash information could not be read from the package signature.", exception.Message);
        }

        [Fact]
        public void Load_ReadsValidContent()
        {
            var content = SignatureContent.Load(
                Encoding.UTF8.GetBytes("Version:1\r\n\r\n2.16.840.1.101.3.4.2.1-Hash:a\r\n\r\n"),
                SigningSpecifications.V1);

            Assert.Equal(HashAlgorithmName.SHA256, content.HashAlgorithm);
            Assert.Equal("a", content.HashValue);
        }
    }
}
