// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using BcCommitmentTypeQualifier = Org.BouncyCastle.Asn1.Esf.CommitmentTypeQualifier;

namespace NuGet.Packaging.Test
{
    public class CommitmentTypeQualifierTests
    {
        private readonly DerObjectIdentifier _commitmentTypeQualifierId = new DerObjectIdentifier("1.2.3");

        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => CommitmentTypeQualifier.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithOnlyCommitmentTypeId_ReturnsCommitmentTypeQualifier()
        {
            var bcCommitmentTypeQualifier = new BcCommitmentTypeQualifier(_commitmentTypeQualifierId);
            var bytes = bcCommitmentTypeQualifier.GetDerEncoded();

            var commitmentTypeQualifier = CommitmentTypeQualifier.Read(bytes);

            Assert.Equal(
                bcCommitmentTypeQualifier.CommitmentTypeIdentifier.ToString(),
                commitmentTypeQualifier.CommitmentTypeIdentifier.Value);
            Assert.Null(commitmentTypeQualifier.Qualifier);
        }

        [Fact]
        public void Read_WithQualifier_ReturnsCommitmentTypeQualifier()
        {
            var bcCommitmentTypeQualifier = new BcCommitmentTypeQualifier(
                _commitmentTypeQualifierId, DerNull.Instance);
            var bytes = bcCommitmentTypeQualifier.GetDerEncoded();

            var commitmentTypeQualifier = CommitmentTypeQualifier.Read(bytes);

            Assert.Equal(
                bcCommitmentTypeQualifier.CommitmentTypeIdentifier.ToString(),
                commitmentTypeQualifier.CommitmentTypeIdentifier.Value);
            Assert.Equal(DerNull.Instance.GetDerEncoded(), commitmentTypeQualifier.Qualifier);
        }
    }
}
