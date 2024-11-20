// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public class SignatureUtilityTests
    {
        private readonly CertificatesFixture _fixture;

        public SignatureUtilityTests(CertificatesFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _fixture = fixture;
        }

        [Fact]
        public void GetCertificateChain_WhenPrimarySignatureNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.GetCertificateChain(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WithAuthorSignature_ReturnsCertificates()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            using (IX509CertificateChain certificates = SignatureUtility.GetCertificateChain(primarySignature))
            {
                Assert.Equal(3, certificates.Count);
                Assert.Equal("7d14ef1eaa95c41e3cb6c25bb177ce4f9bd7020c", certificates[0].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("0a08d814f1c1c4058bf709c4796a53a47df00e61", certificates[1].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("a1b6d9c348850849be54e3e8ac2ae9938e59e4b3", certificates[2].Thumbprint, StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public async Task GetCertificateChain_WithUnknownSignature_ReturnsCertificatesAsync()
        {
            using (TestDirectory directory = TestDirectory.Create())
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();
                FileInfo unsignedPackageFile = await packageContext.CreateAsFileAsync(directory, "Package.nupkg");
                FileInfo signedPackageFile = await SignedArchiveTestUtility.SignPackageFileWithBasicSignedCmsAsync(
                    directory,
                    unsignedPackageFile,
                    certificate);

                using (var packageReader = new PackageArchiveReader(signedPackageFile.FullName))
                {
                    PrimarySignature signature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                    using (IX509CertificateChain certificates = SignatureUtility.GetCertificateChain(signature))
                    {
                        Assert.Equal(1, certificates.Count);
                        Assert.Equal(certificate.RawData, certificates[0].RawData);
                    }
                }
            }
        }

        [Fact]
        public void GetCertificateChain_WithUnrelatedRepositoryCountersignature_Throws()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            RepositoryCountersignature repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            primarySignature = RemoveRepositoryCountersignature(primarySignature);

            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => SignatureUtility.GetCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal("repositoryCountersignature", exception.ParamName);
            Assert.StartsWith("The primary signature and repository countersignature are unrelated.", exception.Message);
        }

        [Fact]
        public void GetCertificateChain_WithRepositoryCountersignature_ReturnsCertificates()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            RepositoryCountersignature repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            using (IX509CertificateChain certificates = SignatureUtility.GetCertificateChain(primarySignature, repositoryCountersignature))
            {
                Assert.Equal(3, certificates.Count);
                Assert.Equal("8d8cc5bdf9e5f86b971d7fb961fe24b999486483", certificates[0].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("c8ae47bfd632870a15e3775784affd2bdc96cbf1", certificates[1].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("d4e8185475a062de3518d1aa693f13c4283f81ff", certificates[2].Thumbprint, StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void GetTimestampCertificateChain_WhenSignatureNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithoutTimestamp_Throws()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            primarySignature = RemoveTimestamp(primarySignature);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature));

            Assert.Equal(NuGetLogCode.NU3000, exception.Code);
            Assert.Equal("The primary signature does not have a timestamp.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithAuthorSignatureTimestamp_ReturnsCertificates()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            using (IX509CertificateChain certificates = SignatureUtility.GetTimestampCertificateChain(primarySignature))
            {
                Assert.Equal(3, certificates.Count);
                Assert.Equal("5f970d4b17786b091a77eabdd0cf92ff8d1fdb43", certificates[0].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("c5e93f93089bd49dc1d8e2b657093b9e29132dcf", certificates[1].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("a0e355c9f370a3069823afa3ce22b14a91475e77", certificates[2].Thumbprint, StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void GetTimestampCertificateChain_WithUnrelatedRepositoryCountersignature_Throws()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            RepositoryCountersignature repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            primarySignature = RemoveRepositoryCountersignature(primarySignature);

            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal("repositoryCountersignature", exception.ParamName);
            Assert.StartsWith("The primary signature and repository countersignature are unrelated.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithRepositoryCountersignatureWithoutTimestamp_Throws()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            primarySignature = RemoveRepositoryCountersignatureTimestamp(primarySignature);

            RepositoryCountersignature repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            SignatureException exception = Assert.Throws<SignatureException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal(NuGetLogCode.NU3000, exception.Code);
            Assert.Equal("The repository countersignature does not have a timestamp.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithRepositoryCountersignatureTimestamp_ReturnsCertificates()
        {
            PrimarySignature primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            RepositoryCountersignature repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            using (IX509CertificateChain certificates = SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature))
            {
                Assert.Equal(3, certificates.Count);
                Assert.Equal("96b479acf63394f3bcc9928c396264afd60909ed", certificates[0].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("fa4e4ca3d9a26b92a73bb875f964972983b55ccd", certificates[1].Thumbprint, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("88b288ff6d3d826469a9ef7816166a7def221885", certificates[2].Thumbprint, StringComparer.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void HasRepositoryCountersignature_WithNullPrimarySignature_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.HasRepositoryCountersignature(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public async Task HasRepositoryCountersignature_WithSignatureWithoutRepositoryCountersignature_ReturnsFalseAsync()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();

                using (MemoryStream unsignedPackageStream = await packageContext.CreateAsStreamAsync())
                {
                    AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(
                        certificate,
                        unsignedPackageStream);

                    bool hasRepoCountersignature = SignatureUtility.HasRepositoryCountersignature(signature);

                    Assert.False(hasRepoCountersignature);
                }
            }
        }

        [Fact]
        public async Task HasRepositoryCountersignature_WithSignatureWithRepositoryCountersignature_ReturnsTrueAsync()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            using (X509Certificate2 repositoryCertificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();

                using (MemoryStream unsignedPackageStream = await packageContext.CreateAsStreamAsync())
                {
                    AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(
                        certificate,
                        unsignedPackageStream);

                    Common.HashAlgorithmName hashAlgorithm = Common.HashAlgorithmName.SHA256;
                    var v3ServiceIndexUri = new Uri("https://v3serviceIndex.test/api/index.json");

                    using (RepositorySignPackageRequest request = new(
                        repositoryCertificate,
                        hashAlgorithm,
                        hashAlgorithm,
                        v3ServiceIndexUri,
                        packageOwners: null))
                    {
                        PrimarySignature repoCountersignedSignature = await SignedArchiveTestUtility.RepositoryCountersignPrimarySignatureAsync(signature, request);
                        bool hasRepoCountersignature = SignatureUtility.HasRepositoryCountersignature(repoCountersignedSignature);

                        Assert.True(hasRepoCountersignature);
                    }
                }
            }
        }

        [Fact]
        public void LogAdditionalContext_WhenChainIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.LogAdditionalContext(chain: null, new List<SignatureLog>()));

            Assert.Equal("chain", exception.ParamName);
        }

        [Fact]
        public void LogAdditionalContext_WhenIssuesIsNull_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.LogAdditionalContext(Mock.Of<IX509Chain>(), issues: null));

            Assert.Equal("issues", exception.ParamName);
        }

        [Fact]
        public void LogAdditionalContext_WhenChainAdditionalContextIsNull_DoesNotLog()
        {
            Mock<IX509Chain> chain = new(MockBehavior.Strict);

            chain.SetupGet(x => x.AdditionalContext)
                .Returns((ILogMessage)null!);

            List<SignatureLog> issues = new();

            SignatureUtility.LogAdditionalContext(chain.Object, issues);

            Assert.Empty(issues);
        }

        [Fact]
        public void LogAdditionalContext_WhenChainAdditionalContextIsNotNull_Logs()
        {
            Mock<IX509Chain> chain = new(MockBehavior.Strict);
            LogMessage logMessage = LogMessage.CreateWarning(NuGetLogCode.NU3042, "abc");

            chain.SetupGet(x => x.AdditionalContext)
                .Returns(logMessage);

            List<SignatureLog> issues = new();

            SignatureUtility.LogAdditionalContext(chain.Object, issues);

            SignatureLog signatureLog = Assert.Single(issues);

            Assert.Equal(LogLevel.Warning, signatureLog.Level);
            Assert.Equal(logMessage.Code, signatureLog.Code);
            Assert.Equal(logMessage.Message, signatureLog.Message);
        }

        private static PrimarySignature RemoveTimestamp(PrimarySignature signature)
        {
            return RemovePrimarySignerUnsignedAttribute(signature, new Oid(Oids.SignatureTimeStampTokenAttribute));
        }

        private static PrimarySignature RemoveRepositoryCountersignature(PrimarySignature signature)
        {
            return RemovePrimarySignerUnsignedAttribute(signature, new Oid(Oids.Countersignature));
        }

        private static PrimarySignature RemoveRepositoryCountersignatureTimestamp(PrimarySignature signature)
        {
            TestSignedCms testSignedCms = TestSignedCms.Decode(signature.SignedCms);
            TestSignerInfo testSignerInfo = testSignedCms.SignerInfos[0];

            if (testSignerInfo.TryGetUnsignedAttribute(
                TestOids.Countersignature,
                out CryptographicAttributeObject? countersignatureAttribute))
            {
                testSignerInfo.RemoveUnsignedAttribute(TestOids.Countersignature);

                TestSignerInfo testCountersigner = TestSignerInfo.Decode(
                    new ReadOnlyMemory<byte>(countersignatureAttribute!.Values[0].RawData));

                testCountersigner.RemoveUnsignedAttribute(TestOids.SignatureTimestampToken);

                AsnWriter writer = new(AsnEncodingRules.DER);

                testCountersigner.Encode(writer);

                countersignatureAttribute = new CryptographicAttributeObject(countersignatureAttribute.Oid);
                countersignatureAttribute.Values.Add(
                    new AsnEncodedData(
                        countersignatureAttribute.Oid,
                        writer.Encode()));

                testSignerInfo.AddUnsignedAttribute(countersignatureAttribute);
            }

            SignedCms updatedSignedCms = testSignedCms.Encode();

            return PrimarySignature.Load(updatedSignedCms);
        }

        private static PrimarySignature RemovePrimarySignerUnsignedAttribute(PrimarySignature signature, Oid oid)
        {
            TestSignedCms testSignedCms = TestSignedCms.Decode(signature.SignedCms);

            testSignedCms.SignerInfos[0].RemoveUnsignedAttribute(oid);

            SignedCms updatedSignedCms = testSignedCms.Encode();

            return PrimarySignature.Load(updatedSignedCms);
        }
    }
}
#endif
