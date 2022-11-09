// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using BcAlgorithmIdentifier = Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier;
using BcMessageImprint = Org.BouncyCastle.Asn1.Tsp.MessageImprint;

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
            var bcAlgorithmIdentifier = new BcAlgorithmIdentifier(new DerObjectIdentifier(Oids.Sha256));
            var bcMessageImprint = new DerSequence(bcAlgorithmIdentifier, new DerOctetString(new byte[0]));
            var bytes = bcMessageImprint.GetDerEncoded();

            var exception = Assert.Throws<CryptographicException>(() => MessageImprint.Read(bytes));

            Assert.Equal("The ASN.1 data is invalid.", exception.Message);
        }

        [Theory]
        [InlineData(Oids.Sha256)]
        [InlineData(Oids.Sha512)]
        public void Read_WithValidInput_ReturnsInstance(string oid)
        {
            var data = Encoding.UTF8.GetBytes("peach");
            var hashAlgorithmName = CryptoHashUtility.OidToHashAlgorithmName(oid);
            var hash = hashAlgorithmName.ComputeHash(data);
            var bcAlgorithmIdentifier = new BcAlgorithmIdentifier(new DerObjectIdentifier(oid));
            var bcMessageImprint = new BcMessageImprint(bcAlgorithmIdentifier, hash);
            var bytes = bcMessageImprint.GetDerEncoded();

            var messageImprint = MessageImprint.Read(bytes);

            Assert.Equal(oid, messageImprint.HashAlgorithm.Algorithm.Value);
            Assert.Equal(hash, messageImprint.HashedMessage);
        }
    }
}
