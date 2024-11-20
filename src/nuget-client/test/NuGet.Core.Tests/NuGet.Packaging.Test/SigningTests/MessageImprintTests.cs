// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;
using MessageImprint = NuGet.Packaging.Signing.MessageImprint;
using TestAlgorithmIdentifier = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.AlgorithmIdentifier;
using TestMessageImprint = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.MessageImprint;

namespace NuGet.Packaging.Test
{
    public class MessageImprintTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => MessageImprint.Read(new byte[] { 0x30, 0x0b }));
        }

        [Fact]
        public void Read_WithInvalidHashedMessage_Throws()
        {
            TestMessageImprint testMessageImprint = new(new TestAlgorithmIdentifier(TestOids.Sha256), new byte[0]);
            byte[] bytes = Encode(testMessageImprint);

            CryptographicException exception = Assert.Throws<CryptographicException>(() => MessageImprint.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(Oids.Sha256)]
        [InlineData(Oids.Sha512)]
        public void Read_WithValidInput_ReturnsInstance(string oid)
        {
            byte[] data = Encoding.UTF8.GetBytes("peach");
            Common.HashAlgorithmName hashAlgorithmName = CryptoHashUtility.OidToHashAlgorithmName(oid);
            byte[] hash = hashAlgorithmName.ComputeHash(data);
            TestMessageImprint testMessageImprint = new(new TestAlgorithmIdentifier(new Oid(oid)), hash);
            byte[] bytes = Encode(testMessageImprint);

            MessageImprint messageImprint = MessageImprint.Read(bytes);

            Assert.Equal(oid, messageImprint.HashAlgorithm.Algorithm.Value);
            Assert.Equal(hash, messageImprint.HashedMessage);
        }

        private static byte[] Encode(TestMessageImprint testMessageImprint)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            testMessageImprint.Encode(writer);

            return writer.Encode();
        }
    }
}
