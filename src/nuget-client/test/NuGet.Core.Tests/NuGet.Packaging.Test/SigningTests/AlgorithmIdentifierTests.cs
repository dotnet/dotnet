// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using AlgorithmIdentifier = NuGet.Packaging.Signing.AlgorithmIdentifier;
using BcAlgorithmIdentifier = Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier;

namespace NuGet.Packaging.Test
{
    public class AlgorithmIdentifierTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => AlgorithmIdentifier.Read(new byte[] { 0x30, 0x0b }));
        }

        [Theory]
        [InlineData(Oids.Sha1)]
        [InlineData(Oids.Sha256)]
        [InlineData(Oids.Sha384)]
        [InlineData(Oids.Sha512)]
        public void Read_WithValidInput_ReturnsAlgorithmIdentifier(string oid)
        {
            var bytes = new BcAlgorithmIdentifier(new DerObjectIdentifier(oid)).GetDerEncoded();

            var algorithmId = AlgorithmIdentifier.Read(bytes);

            Assert.Equal(oid, algorithmId.Algorithm.Value);
        }

        [Fact]
        public void Read_WithExplicitNullParameters_ReturnsAlgorithmIdentifier()
        {
            var bytes = new BcAlgorithmIdentifier(new DerObjectIdentifier(Oids.Sha256), DerNull.Instance).GetDerEncoded();

            var algorithmId = AlgorithmIdentifier.Read(bytes);

            Assert.Equal(Oids.Sha256, algorithmId.Algorithm.Value);
        }
    }
}
