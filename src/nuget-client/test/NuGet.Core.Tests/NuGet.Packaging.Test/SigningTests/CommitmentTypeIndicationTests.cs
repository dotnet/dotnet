// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Xunit;
using BcCommitmentTypeIndication = Org.BouncyCastle.Asn1.Esf.CommitmentTypeIndication;
using BcCommitmentTypeQualifier = Org.BouncyCastle.Asn1.Esf.CommitmentTypeQualifier;

namespace NuGet.Packaging.Test
{
    public class CommitmentTypeIndicationTests
    {
        [Fact]
        public void Create_WhenCommitmentTypeIdNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => CommitmentTypeIndication.Create(commitmentTypeId: null));

            Assert.Equal("commitmentTypeId", exception.ParamName);
        }

        [Fact]
        public void Create_WithCommitmentTypeId_ReturnsInstance()
        {
            var commitmentTypeIndication = CommitmentTypeIndication.Create(new Oid(Oids.CommitmentTypeIdentifierProofOfOrigin));

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
            var bcCommitmentTypeIndication = new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfSender);
            var bytes = bcCommitmentTypeIndication.GetDerEncoded();

            var commitmentTypeIndication = CommitmentTypeIndication.Read(bytes);

            Assert.Equal(
                bcCommitmentTypeIndication.CommitmentTypeID.ToString(),
                commitmentTypeIndication.CommitmentTypeId.Value);
            Assert.Null(commitmentTypeIndication.Qualifiers);
        }

        [Fact]
        public void Read_WithEmptyQualifiers_Throws()
        {
            var bcCommitmentTypeIndication = new BcCommitmentTypeIndication(
                PkcsObjectIdentifiers.IdCtiEtsProofOfSender,
                DerSequence.Empty);
            var bytes = bcCommitmentTypeIndication.GetDerEncoded();

            var exception = Assert.Throws<SignatureException>(
                () => CommitmentTypeIndication.Read(bytes));

            Assert.Equal("The commitment-type-indication attribute is invalid.", exception.Message);
        }

        [Fact]
        public void Read_WithQualifiers_ReturnsInstance()
        {
            var commitmentTypeIdentifier = new DerObjectIdentifier("1.2.3");
            var bcCommitmentTypeQualifier = new BcCommitmentTypeQualifier(commitmentTypeIdentifier);
            var bcCommitmentTypeIndication = new BcCommitmentTypeIndication(
                PkcsObjectIdentifiers.IdCtiEtsProofOfSender,
                new DerSequence(bcCommitmentTypeQualifier));
            var bytes = bcCommitmentTypeIndication.GetDerEncoded();

            var commitmentTypeIndication = CommitmentTypeIndication.Read(bytes);

            Assert.Equal(1, commitmentTypeIndication.Qualifiers.Count);
            Assert.Equal(commitmentTypeIdentifier.ToString(), commitmentTypeIndication.Qualifiers[0].CommitmentTypeIdentifier.Value);
        }
    }
}
