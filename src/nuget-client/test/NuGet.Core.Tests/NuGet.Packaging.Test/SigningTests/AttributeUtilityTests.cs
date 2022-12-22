// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Packaging.Signing.DerEncoding;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Test.Utility.Signing;
using Xunit;
using BcAttribute = Org.BouncyCastle.Asn1.Cms.Attribute;
using BcCommitmentTypeIndication = Org.BouncyCastle.Asn1.Esf.CommitmentTypeIndication;
using DotNetUtilities = Org.BouncyCastle.Security.DotNetUtilities;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class AttributeUtilityTests
    {
        private const string CommitmentTypeIdentifierProofOfDelivery = "1.2.840.113549.1.9.16.6.3";
        private const string CommitmentTypeIdentifierProofOfSender = "1.2.840.113549.1.9.16.6.4";

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
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Unknown));

            Assert.Contains("signatureType", exception.Message);
        }

        [Fact]
        public void CreateCommitmentTypeIndication_WithAuthorSignature_ReturnsOriginType()
        {
            var attribute = AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Author);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Author);
        }

        [Fact]
        public void CreateCommitmentTypeIndication_WithRepositorySignature_ReturnsReceiptType()
        {
            var attribute = AttributeUtility.CreateCommitmentTypeIndication(SignatureType.Repository);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Repository);
        }

        [Fact]
        public void GetSignatureType_WhenSignedAttributesNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
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
            var attribute = GetCommitmentTypeTestAttribute(Oids.CommitmentTypeIdentifierProofOfOrigin);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Author);
        }

        [Fact]
        public void GetSignatureType_WithReceiptType_ReturnsRepository()
        {
            var attribute = GetCommitmentTypeTestAttribute(Oids.CommitmentTypeIdentifierProofOfReceipt);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Repository);
        }

        [Fact]
        public void GetSignatureType_WithUnknownType_ReturnsUnknown()
        {
            var attribute = GetCommitmentTypeTestAttribute(CommitmentTypeIdentifierProofOfDelivery);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Unknown);
        }

        [Fact]
        public void GetSignatureType_WithMultipleUnknownTypes_ReturnsUnknown()
        {
            var attribute = GetCommitmentTypeTestAttribute(
                CommitmentTypeIdentifierProofOfDelivery, CommitmentTypeIdentifierProofOfSender);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            AttributeUtility.GetSignatureType(attributes).Should().Be(SignatureType.Unknown);
        }

        [Fact]
        public void GetSignatureType_WithNoType_ReturnsUnknown()
        {
            var attribute = GetCommitmentTypeTestAttribute();
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Unknown, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithBothOriginAndReceiptTypes_Throws()
        {
            var attribute = GetCommitmentTypeTestAttribute(
                Oids.CommitmentTypeIdentifierProofOfOrigin, Oids.CommitmentTypeIdentifierProofOfReceipt);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            var exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetSignatureType(attributes));

            Assert.Equal("The commitment-type-indication attribute contains an invalid combination of values.", exception.Message);
        }

        [Fact]
        public void GetSignatureType_WithDuplicateOriginTypes_ReturnsAuthor()
        {
            var attribute = GetCommitmentTypeTestAttribute(
                Oids.CommitmentTypeIdentifierProofOfOrigin, Oids.CommitmentTypeIdentifierProofOfOrigin);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Author, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithOriginAndUnknownType_ReturnsAuthor()
        {
            var attribute = GetCommitmentTypeTestAttribute(
                Oids.CommitmentTypeIdentifierProofOfOrigin, CommitmentTypeIdentifierProofOfSender);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            Assert.Equal(SignatureType.Author, AttributeUtility.GetSignatureType(attributes));
        }

        [Fact]
        public void GetSignatureType_WithOriginTypeInNonFirstAttributeInstance_ReturnsAuthor()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfApproval))));

                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin))));
                    });

                Assert.Equal(
                    2,
                    attributes.Cast<CryptographicAttributeObject>()
                        .Count(attribute => attribute.Oid.Value == Oids.CommitmentTypeIndication));

                Assert.Equal(SignatureType.Author, AttributeUtility.GetSignatureType(attributes));
            }
        }

        [Fact]
        public void GetSignatureType_WithOriginTypeInOneAttributeInstanceAndReceiptTypeInAnotherAttributeInstance_ReturnsAuthor()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin))));

                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfReceipt))));
                    });

                Assert.Equal(
                    2,
                    attributes.Cast<CryptographicAttributeObject>()
                        .Count(attribute => attribute.Oid.Value == Oids.CommitmentTypeIndication));

                var exception = Assert.Throws<SignatureException>(
                    () => AttributeUtility.GetSignatureType(attributes));

                Assert.Equal("The commitment-type-indication attribute contains an invalid combination of values.", exception.Message);
            }
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
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.CreateSigningCertificateV2(
                    certificate: null,
                    hashAlgorithm: Common.HashAlgorithmName.SHA256));

            Assert.Equal("certificate", exception.ParamName);
        }

        [Fact]
        public void CreateSigningCertificateV2_WhenHashAlgorithmUnknown_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var exception = Assert.Throws<ArgumentException>(
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
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attribute = AttributeUtility.CreateSigningCertificateV2(certificate, hashAlgorithmName);

                Assert.Equal(Oids.SigningCertificateV2, attribute.Oid.Value);
                Assert.Equal(1, attribute.Values.Count);

                var signingCertificateV2 = SigningCertificateV2.Read(attribute.Values[0].RawData);

                Assert.Equal(1, signingCertificateV2.Certificates.Count);

                var essCertIdV2 = signingCertificateV2.Certificates[0];
                var expectedHash = SigningTestUtility.GetHash(certificate, hashAlgorithmName);

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
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetAttribute(attributes: null, oid: "1.2.3"));

            Assert.Equal("attributes", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAttribute_WhenOidNullOrEmpty_Throws(string oid)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.GetAttribute(new CryptographicAttributeObjectCollection(), oid));

            Assert.Equal("oid", exception.ParamName);
        }

        [Fact]
        public void GetAttribute_WithNoMatches_ReturnsNull()
        {
            var attribute = AttributeUtility.GetAttribute(new CryptographicAttributeObjectCollection(), Oids.SigningTime);

            Assert.Null(attribute);
        }

        [Fact]
        public void GetAttribute_WithMultipleMatches_ReturnsMatches()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin))));

                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfReceipt))));
                    });

                var exception = Assert.Throws<CryptographicException>(
                    () => AttributeUtility.GetAttribute(attributes, Oids.CommitmentTypeIndication));

                Assert.Equal(
                    $"Multiple {Oids.CommitmentTypeIndication} attributes are not allowed.",
                    exception.Message);
            }
        }

        [Fact]
        public void GetAttributes_WhenAttributesNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetAttributes(attributes: null, oid: "1.2.3"));

            Assert.Equal("attributes", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetAttributes_WhenOidNullOrEmpty_Throws(string oid)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.GetAttributes(new CryptographicAttributeObjectCollection(), oid));

            Assert.Equal("oid", exception.ParamName);
        }

        [Fact]
        public void GetAttributes_WithNoMatches_ReturnsEmptyEnumerable()
        {
            var attributes = AttributeUtility.GetAttributes(new CryptographicAttributeObjectCollection(), Oids.SigningTime);

            Assert.Empty(attributes);
        }

        [Fact]
        public void GetAttributes_WithMultipleMatches_ReturnsMatches()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        vector.Add(
                            new BcAttribute(
                                CmsAttributes.SigningTime,
                                new DerSet(new DerUtcTime(DateTime.UtcNow))));

                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin))));

                        vector.Add(
                            new BcAttribute(
                                PkcsObjectIdentifiers.IdAAEtsCommitmentType,
                                new DerSet(
                                    new BcCommitmentTypeIndication(PkcsObjectIdentifiers.IdCtiEtsProofOfReceipt))));
                    });

                var matches = AttributeUtility.GetAttributes(attributes, Oids.CommitmentTypeIndication).ToArray();

                Assert.Equal(2, matches.Length);
                Assert.Equal(
                    PkcsObjectIdentifiers.IdCtiEtsProofOfOrigin.ToString(),
                    CommitmentTypeIndication.Read(matches[0].Values[0].RawData).CommitmentTypeId.Value);
                Assert.Equal(
                    PkcsObjectIdentifiers.IdCtiEtsProofOfReceipt.ToString(),
                    CommitmentTypeIndication.Read(matches[1].Values[0].RawData).CommitmentTypeId.Value);
            }
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexUrlNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.CreateNuGetV3ServiceIndexUrl(v3ServiceIndexUrl: null));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexNotAbsolute_Throws()
        {
            var url = new Uri("/", UriKind.Relative);
            var exception = Assert.Throws<ArgumentException>(() => AttributeUtility.CreateNuGetV3ServiceIndexUrl(url));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
            Assert.StartsWith("The URL value is invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WhenV3ServiceIndexSchemeIsNotHttps_Throws()
        {
            var url = new Uri("http://test.test", UriKind.Absolute);
            var exception = Assert.Throws<ArgumentException>(() => AttributeUtility.CreateNuGetV3ServiceIndexUrl(url));

            Assert.Equal("v3ServiceIndexUrl", exception.ParamName);
            Assert.StartsWith("The URL value is invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetV3ServiceIndexUrl_WithValidInput_ReturnsAttribute()
        {
            var url = new Uri("https://test.test", UriKind.Absolute);
            var attribute = AttributeUtility.CreateNuGetV3ServiceIndexUrl(url);

            Assert.Equal(Oids.NuGetV3ServiceIndexUrl, attribute.Oid.Value);
            Assert.Equal(1, attribute.Values.Count);

            var nugetV3ServiceIndexUrl = NuGetV3ServiceIndexUrl.Read(attribute.Values[0].RawData);

            Assert.True(nugetV3ServiceIndexUrl.V3ServiceIndexUrl.IsAbsoluteUri);
            Assert.Equal(url.OriginalString, nugetV3ServiceIndexUrl.V3ServiceIndexUrl.OriginalString);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WhenSignedAttributesNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetNuGetV3ServiceIndexUrl(signedAttributes: null));

            Assert.Equal("signedAttributes", exception.ParamName);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WhenSignedAttributesEmpty_Throws()
        {
            var exception = Assert.Throws<SignatureException>(
                () => AttributeUtility.GetNuGetV3ServiceIndexUrl(new CryptographicAttributeObjectCollection()));

            Assert.Equal("Exactly one nuget-v3-service-index-url attribute is required.", exception.Message);
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WithMultipleAttributes_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        var attribute = new BcAttribute(
                            new DerObjectIdentifier(Oids.NuGetV3ServiceIndexUrl),
                            new DerSet(new DerIA5String("https://test.test")));

                        vector.Add(attribute);
                        vector.Add(attribute);
                    });

                var exception = Assert.Throws<CryptographicException>(
                    () => AttributeUtility.GetNuGetV3ServiceIndexUrl(attributes));

                Assert.Equal("Multiple 1.3.6.1.4.1.311.84.2.1.1.1 attributes are not allowed.", exception.Message);
            }
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WithMultipleAttributeValues_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        var value = new DerIA5String("https://test.test");
                        var attribute = new BcAttribute(
                            new DerObjectIdentifier(Oids.NuGetV3ServiceIndexUrl),
                            new DerSet(value, value));

                        vector.Add(attribute);
                    });

                var exception = Assert.Throws<SignatureException>(
                    () => AttributeUtility.GetNuGetV3ServiceIndexUrl(attributes));

                Assert.Equal(
                    "The nuget-v3-service-index-url attribute must have exactly one attribute value.",
                    exception.Message);
            }
        }

        [Fact]
        public void GetNuGetV3ServiceIndexUrl_WithValidInput_ReturnsUrl()
        {
            var url = new Uri("https://test.test");
            var attribute = AttributeUtility.CreateNuGetV3ServiceIndexUrl(url);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            var v3ServiceIndexUrl = AttributeUtility.GetNuGetV3ServiceIndexUrl(attributes);

            Assert.True(v3ServiceIndexUrl.IsAbsoluteUri);
            Assert.Equal(url.OriginalString, v3ServiceIndexUrl.OriginalString);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WhenPackageOwnersNull_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetPackageOwners(packageOwners: null));

            Assert.Equal("packageOwners", exception.ParamName);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WhenPackageOwnersEmpty_Throws()
        {
            var exception = Assert.Throws<ArgumentException>(
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
            var exception = Assert.Throws<ArgumentException>(
                () => AttributeUtility.CreateNuGetPackageOwners(new[] { packageOwner }));

            Assert.Equal("packageOwners", exception.ParamName);
            Assert.StartsWith("One or more package owner values are invalid.", exception.Message);
        }

        [Fact]
        public void CreateNuGetPackageOwners_WithValidInput_ReturnsInstance()
        {
            var packageOwners = new[] { "a", "b", "c" };
            var attribute = AttributeUtility.CreateNuGetPackageOwners(packageOwners);

            Assert.Equal(Oids.NuGetPackageOwners, attribute.Oid.Value);
            Assert.Equal(1, attribute.Values.Count);

            var nugetPackageOwners = NuGetPackageOwners.Read(attribute.Values[0].RawData);

            Assert.Equal(packageOwners, nugetPackageOwners.PackageOwners);
        }

        [Fact]
        public void GetNuGetPackageOwners_WhenSignedAttributesNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => AttributeUtility.GetNuGetPackageOwners(signedAttributes: null));

            Assert.Equal("signedAttributes", exception.ParamName);
        }

        [Fact]
        public void GetNuGetPackageOwners_WithEmptySignedAttributes_ReturnsNull()
        {
            var packageOwners = AttributeUtility.GetNuGetPackageOwners(new CryptographicAttributeObjectCollection());

            Assert.Null(packageOwners);
        }

        [Fact]
        public void GetNuGetPackageOwners_WithMultipleAttributes_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        var attribute = new BcAttribute(
                            new DerObjectIdentifier(Oids.NuGetPackageOwners),
                            new DerSet(
                                new DerSequence(
                                    new DerUtf8String("a"),
                                    new DerUtf8String("b"),
                                    new DerUtf8String("c"))));

                        vector.Add(attribute);
                        vector.Add(attribute);
                    });

                var exception = Assert.Throws<CryptographicException>(
                    () => AttributeUtility.GetNuGetPackageOwners(attributes));

                Assert.Equal("Multiple 1.3.6.1.4.1.311.84.2.1.1.2 attributes are not allowed.", exception.Message);
            }
        }

        [Fact]
        public void GetNuGetPackageOwners_WithMultipleAttributeValues_Throws()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var attributes = CreateAttributeCollection(certificate, _fixture.Key,
                    vector =>
                    {
                        var value = new DerSequence(
                            new DerUtf8String("a"),
                            new DerUtf8String("b"),
                            new DerUtf8String("c"));
                        var attribute = new BcAttribute(
                            new DerObjectIdentifier(Oids.NuGetPackageOwners),
                            new DerSet(value, value));

                        vector.Add(attribute);
                    });

                var exception = Assert.Throws<SignatureException>(
                    () => AttributeUtility.GetNuGetPackageOwners(attributes));

                Assert.Equal("The nuget-package-owners attribute must have exactly one attribute value.", exception.Message);
            }
        }

        [Fact]
        public void GetNuGetPackageOwners_WithValidInput_ReturnsUrl()
        {
            var packageOwners = new[] { "a", "b", "c" };
            var attribute = AttributeUtility.CreateNuGetPackageOwners(packageOwners);
            var attributes = new CryptographicAttributeObjectCollection(attribute);

            var nugetPackageOwners = AttributeUtility.GetNuGetPackageOwners(attributes);

            Assert.Equal(packageOwners, nugetPackageOwners);
        }

        /// <summary>
        /// Allows encoding data that the production helper does not.
        /// </summary>
        private static CryptographicAttributeObject GetCommitmentTypeTestAttribute(params string[] oids)
        {
            var commitmentTypeIndication = new Oid(Oids.CommitmentTypeIndication);
            var values = new AsnEncodedDataCollection();

            foreach (var oid in oids)
            {
                var value = DerEncoder.ConstructSequence(DerEncoder.SegmentedEncodeOid(oid));

                values.Add(new AsnEncodedData(commitmentTypeIndication, value));
            }

            return new CryptographicAttributeObject(commitmentTypeIndication, values);
        }

        private static CryptographicAttributeObjectCollection CreateAttributeCollection(
            X509Certificate2 certificate,
            RSA privateKey,
            Action<Asn1EncodableVector> addAttributes)
        {
            var content = new CmsProcessableByteArray(new byte[0]);
            var attributes = new Asn1EncodableVector();

            addAttributes(attributes);

            var signedAttributes = new AttributeTable(attributes);
            var unsignedAttributes = new AttributeTable(DerSet.Empty);

            var generator = new CmsSignedDataGenerator();
            var keyPair = DotNetUtilities.GetRsaKeyPair(privateKey);

            generator.AddSigner(
                keyPair.Private,
                DotNetUtilities.FromX509Certificate(certificate),
                Oids.Sha256,
                signedAttributes,
                unsignedAttributes);

            var bcSignedCms = generator.Generate(content, encapsulate: true);
            var signedCms = new SignedCms();

            signedCms.Decode(bcSignedCms.GetEncoded());

            return signedCms.SignerInfos[0].SignedAttributes;
        }
    }
}
#endif
