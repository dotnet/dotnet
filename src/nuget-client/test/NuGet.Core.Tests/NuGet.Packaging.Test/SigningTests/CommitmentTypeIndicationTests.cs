// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class CommitmentTypeIndicationTests
    {
        [Fact]
        public void Create_WhenCommitmentTypeIdNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => CommitmentTypeIndication.Create(commitmentTypeId: null));

            Assert.Equal("commitmentTypeId", exception.ParamName);
        }

        [Fact]
        public void Create_WithCommitmentTypeId_ReturnsInstance()
        {
            CommitmentTypeIndication commitmentTypeIndication = CommitmentTypeIndication.Create(
                new Oid(Oids.CommitmentTypeIdentifierProofOfOrigin));

            Assert.Equal(Oids.CommitmentTypeIdentifierProofOfOrigin, commitmentTypeIndication.CommitmentTypeId.Value);
            Assert.Null(commitmentTypeIndication.Qualifiers);
        }

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => CommitmentTypeIndication.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithOnlyCommitmentTypeId_ReturnsInstance()
        {
            byte[] bytes = GetCommitmentTypeIndicationBytes(TestOids.CommitmentTypeIdentifierProofOfSender);

            CommitmentTypeIndication commitmentTypeIndication = CommitmentTypeIndication.Read(bytes);

            Assert.Equal(
                TestOids.CommitmentTypeIdentifierProofOfSender.Value,
                commitmentTypeIndication.CommitmentTypeId.Value);
            Assert.Null(commitmentTypeIndication.Qualifiers);
        }

        [Fact]
        public void Read_WithEmptyQualifiers_Throws()
        {
            byte[] bytes = GetCommitmentTypeIndicationBytes(
                TestOids.CommitmentTypeIdentifierProofOfSender, Array.Empty<Oid>());

            SignatureException exception = Assert.Throws<SignatureException>(
                () => CommitmentTypeIndication.Read(bytes));

            Assert.Equal("The commitment-type-indication attribute is invalid.", exception.Message);
        }

        [Fact]
        public void Read_WithQualifiers_ReturnsInstance()
        {
            Oid qualifier = new("1.2.3");
            byte[] bytes = GetCommitmentTypeIndicationBytes(
                TestOids.CommitmentTypeIdentifierProofOfSender, new[] { qualifier });

            CommitmentTypeIndication commitmentTypeIndication = CommitmentTypeIndication.Read(bytes);

            Assert.Equal(1, commitmentTypeIndication.Qualifiers.Count);
            Assert.Equal(qualifier.Value, commitmentTypeIndication.Qualifiers[0].CommitmentTypeIdentifier.Value);
        }

        private static byte[] GetCommitmentTypeIndicationBytes(Oid commitmentTypeIdentifier, Oid[] qualifiers = null)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteObjectIdentifier(commitmentTypeIdentifier.Value!);

                if (qualifiers is not null)
                {
                    using (writer.PushSequence())
                    {
                        foreach (Oid qualifier in qualifiers)
                        {
                            using (writer.PushSequence())
                            {
                                writer.WriteObjectIdentifier(qualifier.Value!);
                            }
                        }
                    }
                }
            }

            return writer.Encode();
        }
    }
}
