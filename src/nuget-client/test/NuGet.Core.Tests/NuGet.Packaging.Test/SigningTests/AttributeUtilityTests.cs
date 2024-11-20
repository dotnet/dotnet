// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;
using EssCertIdV2 = NuGet.Packaging.Signing.EssCertIdV2;
using SigningCertificateV2 = NuGet.Packaging.Signing.SigningCertificateV2;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class AttributeUtilityTests
    {
        private readonly CertificatesFixture _fixture;

        public AttributeUtilityTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void CreateCommitmentTypeIndication_WithUnknownSignature_Throws()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Unknown));

            Assert.Contains("signatureType", exception.Message);
        }

        [Fact]
        public void CreateCommitmentTypeIndication_WithAuthorSignature_ReturnsOriginType()
        {
            CryptographicAttributeObject attribute = AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Author);
        }

        [Fact]
        public void CreateCommitmentTypeIndication_WithRepositorySignature_ReturnsReceiptType()
        {
            CryptographicAttributeObject attribute = AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Repository);
        }

        [Fact]
        public void GetSignatureType_WhenSignedAttributesNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetSignatureType(signedAttributes: null));

            Assert.Equal("signedAttributes", exception.ParamName);
        }

        [Fact]
        public void GetSignatureType_WhenCommitmentTypeIndicationAttributeNotFound_ReturnsUnknown()
        {
            var attribute = new CryptographicAttributeObject(new Oid(Oids.SigningCertificateV2));
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            var signatureType = AttributeUtility.GetSignatureType(attributes);

            Assert.Equal(SignatureType.Unknown, signatureType);
        }

        [Fact]
        public void GetSignatureType_WithOriginType_ReturnsAuthor()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(TestOids.CommitmentTypeIdentifierProofOfOrigin);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Author);
        }

        [Fact]
        public void GetSignatureType_WithReceiptType_ReturnsRepository()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(TestOids.CommitmentTypeIdentifierProofOfReceipt);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Repository);
        }

        [Fact]
        public void GetSignatureType_WithUnknownType_ReturnsUnknown()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(TestOids.CommitmentTypeIdentifierProofOfDelivery);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Unknown);
        }

        [Fact]
        public void GetSignatureType_WithMultipleUnknownTypes_ReturnsUnknown()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(
                TestOids.CommitmentTypeIdentifierProofOfDelivery, TestOids.CommitmentTypeIdentifierProofOfSender);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Unknown);
        }

        [Fact]
        public void GetSignatureType_WithNoType_ReturnsUnknown()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute();
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Unknown, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithBothOriginAndReceiptTypes_Throws()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(
                TestOids.CommitmentTypeIdentifierProofOfOrigin, TestOids.CommitmentTypeIdentifierProofOfReceipt);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetSignatureType(attributes));

            Assert.Equal("The commitment-type-indication attribute contains an invalid combination of values.", exception.Message);
        }

        [Fact]
        public void GetSignatureType_WithDuplicateOriginTypes_ReturnsAuthor()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(
                TestOids.CommitmentTypeIdentifierProofOfOrigin, TestOids.CommitmentTypeIdentifierProofOfOrigin);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Author, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithOriginAndUnknownType_ReturnsAuthor()
        {
            CryptographicAttributeObject attribute = GetCommitmentTypeTestAttribute(
                TestOids.CommitmentTypeIdentifierProofOfOrigin, TestOids.CommitmentTypeIdentifierProofOfSender);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Author, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithOriginTypeInOneAttributeInstanceAndReceiptTypeInAnotherAttributeInstance_ReturnsAuthor()
        {
            CryptographicAttributeObjectCollection attributes = new();

            attributes.Add(GetCommitmentTypeTestAttribute(TestOids.CommitmentTypeIdentifierProofOfOrigin));
            attributes.Add(GetCommitmentTypeTestAttribute(TestOids.CommitmentTypeIdentifierProofOfReceipt));

            Assert.Equal(
                1,
                attributes.Cast<CryptographicAttributeObject>()
                    .Count(attribute => attribute.Oid.Value == Oids.CommitmentTypeIndication));

            SignatureException exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetSignatureType(attributes));

            Assert.Equal("The commitment-type-indication attribute contains an invalid combination of values.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1.2.3")]
        public void GetSignatureType_WithUnknownOid_ReturnsUnknownType(string oid)
        {
            Assert.Equal(SignatureType.Unknown, AttributeUtility.GetSignatureType(oid));
        }

        [Theory]
        [InlineData(Oids.CommitmentTypeIdentifierProofOfOrigin, SignatureType.Author)]
        [InlineData(Oids.CommitmentTypeIdentifierProofOfReceipt, SignatureType.Repository)]
        public void GetSignatureType_WithKnownOid_ReturnsKnownType(string oid, SignatureType expectedResult)
        {
            Assert.Equal(expectedResult, AttributeUtility.GetSignatureType(oid));
        }

        [Fact]
        public void CreateSigningCertificateV2_WhenCertificateNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.CreateSigningCertificateV2(
                    certificate: null,
                    hashAlgorithm: Common.HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void CreateSigningCertificateV2_WhenHashAlgorithmUnknown_Throws()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                ArgumentException exception = Assert.Throws<ArgumentException>(
                    () => AttributeUtility.CreateSigningCertificateV2(
                        certificate,
                        Common.HashAlgorithmName.Unknown));

                Assert.Equal("hashAlgorithmName", exception.ParamName);
            }
        }

        [Theory]
        [InlineData(Common.HashAlgorithmName.SHA256)]
        [InlineData(Common.HashAlgorithmName.SHA384)]
        [InlineData(Common.HashAlgorithmName.SHA512)]
        public void CreateSigningCertificateV2_WithValidInput_ReturnsAttribute(Common.HashAlgorithmName hashAlgorithmName)
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                CryptographicAttributeObject attribute = AttributeUtility.CreateSigningCertificateV2(certificate, hashAlgorithmName);

                Assert.Equal(Oids.SigningCertificateV2, attribute.Oid.Value);
                Assert.Equal(1, attribute.Values.Count);

                SigningCertificateV2 signingCertificateV2 = SigningCertificateV2.Read(attribute.Values[0].RawData);

                Assert.Equal(1, signingCertificateV2.Certificates.Count);

                EssCertIdV2 essCertIdV2 = signingCertificateV2.Certificates[0];
                byte[] expectedHash = SigningTestUtility.GetHash(certificate, hashAlgorithmName);

                SigningTestUtility.VerifyByteArrays(expectedHash, essCertIdV2.CertificateHash);
                Assert.Equal(
                    hashAlgorithmName,
                    CryptoHashUtility.OidToHashAlgorithmName(essCertIdV2.HashAlgorithm.Algorithm.Value));
                Assert.Equal(certificate.IssuerName.Name, essCertIdV2.IssuerSerial.GeneralNames[0].DirectoryName.Name);

                var serialNumber = certificate.GetSerialNumber();

                // Convert from little endian to big endian.
                Array.Reverse(serialNumber);

                SigningTestUtility.VerifyByteArrays(
                    serialNumber,
                    essCertIdV2.IssuerSerial.SerialNumber);
            }
        }

        [Fact]
        public void GetAttribute_WhenAttributesNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetAttribute(attributes: null, oid: "1.2.3"));

            Assert.Equal("attributes", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAttribute_WhenOidNullOrEmpty_Throws(string oid)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.GetAttribute(new CryptographicAttributeObjectCollection(), oid));

            Assert.Equal("oid", exception.ParamName);
        }

        [Fact]
        public void GetAttribute_WithNoMatches_ReturnsNull()
        {
            CryptographicAttributeObject attribute = AttributeUtility.GetAttribute(new CryptographicAttributeObjectCollection(), Oids.SigningTime);

            Assert.Null(attribute);
        }

        [Fact]
        public void GetAttributes_WhenAttributesNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetAttributes(attributes: null, oid: "1.2.3"));

            Assert.Equal("attributes", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAttributes_WhenOidNullOrEmpty_Throws(string oid)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.GetAttributes(new CryptographicAttributeObjectCollection(), oid));

            Assert.Equal("oid", exception.ParamName);
        }

        [Fact]
        public void GetAttributes_WithNoMatches_ReturnsEmptyEnumerable()
        {
            IEnumerable<CryptographicAttributeObject> attributes = AttributeUtility.GetAttributes(
                new CryptographicAttributeObjectCollection(),
                Oids.SigningTime);

            Assert.Empty(attributes);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexUrlNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.CreateNuGetV3ServiceIndexUrl(v3ServiceIndexUrl: null));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexNotAbsolute_Throws()
        {
            var url = new Uri("/", UriKind.Relative);
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetV3ServiceIndexUrl(url));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
            Assert.StartsWith("The URL value is invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexSchemeIsNotHttps_Throws()
        {
            var url = new Uri("http://test.test", UriKind.Absolute);
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetV3ServiceIndexUrl(url));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
            Assert.StartsWith("The URL value is invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WithValidInput_ReturnsAttribute()
        {
            var url = new Uri("https://test.test", UriKind.Absolute);
            CryptographicAttributeObject attribute = AttributeUtility.CreateNuGetV3ServiceIndexUrl(url);

            Assert.Equal(Oids.NuGetV3ServiceIndexUrl, attribute.Oid.Value);
            Assert.Equal(1, attribute.Values.Count);

            NuGetV3ServiceIndexUrl nugetV3ServiceIndexUrl = NuGetV3ServiceIndexUrl.Read(attribute.Values[0].RawData);

            Assert.True(nugetV3ServiceIndexUrl.V3ServiceIndexUrl.IsAbsoluteUri);
            Assert.Equal(url.OriginalString, nugetV3ServiceIndexUrl.V3ServiceIndexUrl.OriginalString);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WhenSignedAttributesNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetNuGetV3ServiceIndexUrl(signedAttributes: null));

            Assert.Equal("signedAttributes", exception.ParamName);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WhenSignedAttributesEmpty_Throws()
        {
            SignatureException exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetNuGetV3ServiceIndexUrl(new CryptographicAttributeObjectCollection()));

            Assert.Equal("Exactly one nuget-v3-service-index-url attribute is required.", exception.Message);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WithMultipleAttributeValues_Throws()
        {
            CryptographicAttributeObjectCollection attributes = new();
            AsnWriter writer = new(AsnEncodingRules.DER);

            writer.WriteCharacterString(UniversalTagNumber.IA5String, "https://test.test");

            CryptographicAttributeObject attribute = new(new Oid(Oids.NuGetV3ServiceIndexUrl));

            AsnEncodedData value = new AsnEncodedData(writer.Encode());

            attribute.Values.Add(value);
            attribute.Values.Add(value);

            attributes.Add(attribute);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetNuGetV3ServiceIndexUrl(attributes));

            Assert.Equal(
                "The nuget-v3-service-index-url attribute must have exactly one attribute value.",
                exception.Message);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WithValidInput_ReturnsUrl()
        {
            var url = new Uri("https://test.test");
            CryptographicAttributeObject attribute = AttributeUtility.CreateNuGetV3ServiceIndexUrl(url);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Uri v3ServiceIndexUrl = AttributeUtility.GetNuGetV3ServiceIndexUrl(attributes);

            Assert.True(v3ServiceIndexUrl.IsAbsoluteUri);
            Assert.Equal(url.OriginalString, v3ServiceIndexUrl.OriginalString);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WhenPackageOwnersNull_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetPackageOwners(packageOwners: null));

            Assert.Equal("packageOwners", exception.ParamName);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WhenPackageOwnersEmpty_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetPackageOwners(Array.Empty<string>()));

            Assert.Equal("packageOwners", exception.ParamName);
            Assert.StartsWith("The argument cannot be null or empty.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateNuGetPackageOwners_WhenPackageOwnersContainsInvalidValue_Throws(string packageOwner)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetPackageOwners(new[] { packageOwner }));

            Assert.Equal("packageOwners", exception.ParamName);
            Assert.StartsWith("One or more package owner values are invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WithValidInput_ReturnsInstance()
        {
            var packageOwners = new[] { "a", "b", "c" };
            CryptographicAttributeObject attribute = AttributeUtility.CreateNuGetPackageOwners(packageOwners);

            Assert.Equal(Oids.NuGetPackageOwners, attribute.Oid.Value);
            Assert.Equal(1, attribute.Values.Count);

            NuGetPackageOwners nugetPackageOwners = NuGetPackageOwners.Read(attribute.Values[0].RawData);

            Assert.Equal(packageOwners, nugetPackageOwners.PackageOwners);
        }

        [Fact]
        public void GetNuGetPackageOwners_WhenSignedAttributesNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetNuGetPackageOwners(signedAttributes: null));

            Assert.Equal("signedAttributes", exception.ParamName);
        }

        [Fact]
        public void GetNuGetPackageOwners_WithEmptySignedAttributes_ReturnsNull()
        {
            IReadOnlyList<string> packageOwners = AttributeUtility.GetNuGetPackageOwners(
                new CryptographicAttributeObjectCollection());

            Assert.Null(packageOwners);
        }

        [Fact]
        public void GetNuGetPackageOwners_WithMultipleAttributeValues_Throws()
        {
            CryptographicAttributeObjectCollection attributes = new();
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteCharacterString(UniversalTagNumber.UTF8String, "a");
                writer.WriteCharacterString(UniversalTagNumber.UTF8String, "b");
                writer.WriteCharacterString(UniversalTagNumber.UTF8String, "c");
            }

            CryptographicAttributeObject attribute = new(new Oid(Oids.NuGetPackageOwners));
            AsnEncodedData value = new AsnEncodedData(writer.Encode());

            attribute.Values.Add(value);
            attribute.Values.Add(value);

            attributes.Add(attribute);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetNuGetPackageOwners(attributes));

            Assert.Equal("The nuget-package-owners attribute must have exactly one attribute value.", exception.Message);
        }

        [Fact]
        public void GetNuGetPackageOwners_WithValidInput_ReturnsUrl()
        {
            var packageOwners = new[] { "a", "b", "c" };
            CryptographicAttributeObject attribute = AttributeUtility.CreateNuGetPackageOwners(packageOwners);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            IReadOnlyList<string> nugetPackageOwners = AttributeUtility.GetNuGetPackageOwners(attributes);

            Assert.Equal(packageOwners, nugetPackageOwners);
        }

        /// <summary>
        /// Allows encoding data that the production helper does not.
        /// </summary>
        private static CryptographicAttributeObject GetCommitmentTypeTestAttribute(params Oid[] oids)
        {
            var commitmentTypeIndication = new Oid(Oids.CommitmentTypeIndication);
            var values = new AsnEncodedDataCollection();
            AsnWriter writer = new(AsnEncodingRules.DER);

            foreach (Oid oid in oids)
            {
                writer.Reset();

                using (writer.PushSequence())
                {
                    writer.WriteObjectIdentifier(oid.Value);
                }

                byte[] value = writer.Encode();

                values.Add(new AsnEncodedData(commitmentTypeIndication, value));
            }

            return new CryptographicAttributeObject(commitmentTypeIndication, values);
        }
    }
}
#endif
