// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;
using Xunit;
using AlgorithmIdentifier = NuGet.Packaging.Signing.AlgorithmIdentifier;
using TestAlgorithmIdentifier = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.AlgorithmIdentifier;

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
            TestAlgorithmIdentifier algorithmIdentifier = new(new Oid(oid));
            AsnWriter writer = new(AsnEncodingRules.DER);

            algorithmIdentifier.Encode(writer);

            byte[] bytes = writer.Encode();
            AlgorithmIdentifier algorithmId = AlgorithmIdentifier.Read(bytes);

            Assert.Equal(oid, algorithmId.Algorithm.Value);
        }

        [Fact]
        public void Read_WithExplicitNullParameters_ReturnsAlgorithmIdentifier()
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteObjectIdentifier(TestOids.Sha256.Value!);
                writer.WriteNull();
            }

            byte[] bytes = writer.Encode();
            AlgorithmIdentifier algorithmId = AlgorithmIdentifier.Read(bytes);

            Assert.Equal(Oids.Sha256, algorithmId.Algorithm.Value);
        }
    }
}
