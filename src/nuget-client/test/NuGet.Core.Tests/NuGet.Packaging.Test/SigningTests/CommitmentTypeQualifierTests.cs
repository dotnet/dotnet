// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class CommitmentTypeQualifierTests
    {
        private readonly Oid _commitmentTypeQualifierId = new("1.2.3");

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => CommitmentTypeQualifier.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithOnlyCommitmentTypeId_ReturnsCommitmentTypeQualifier()
        {
            byte[] bytes = GetCommitmentTypeQualifierBytes(_commitmentTypeQualifierId);

            CommitmentTypeQualifier commitmentTypeQualifier = CommitmentTypeQualifier.Read(bytes);

            Assert.Equal(
                _commitmentTypeQualifierId.Value!,
                commitmentTypeQualifier.CommitmentTypeIdentifier.Value!);
            Assert.Null(commitmentTypeQualifier.Qualifier);
        }

        [Fact]
        public void Read_WithQualifier_ReturnsCommitmentTypeQualifier()
        {
            byte[] bytes = GetCommitmentTypeQualifierBytes(_commitmentTypeQualifierId, addNullQualifier: true);

            CommitmentTypeQualifier commitmentTypeQualifier = CommitmentTypeQualifier.Read(bytes);

            Assert.Equal(
                _commitmentTypeQualifierId.Value!,
                commitmentTypeQualifier.CommitmentTypeIdentifier.Value!);
            Assert.Equal(new byte[] { 0x05, 0x00 }, commitmentTypeQualifier.Qualifier);
        }

        private static byte[] GetCommitmentTypeQualifierBytes(Oid oid, bool addNullQualifier = false)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteObjectIdentifier(oid.Value!);

                if (addNullQualifier)
                {
                    writer.WriteNull();
                }
            }

            return writer.Encode();
        }
    }
}
