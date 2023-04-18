// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509.Store;
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
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.GetCertificateChain(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public void GetCertificateChain_WithAuthorSignature_ReturnsCertificates()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            using (var certificates = SignatureUtility.GetCertificateChain(primarySignature))
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
            using (var directory = TestDirectory.Create())
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();
                var unsignedPackageFile = await packageContext.CreateAsFileAsync(directory, "Package.nupkg");
                var signedPackageFile = await SignedArchiveTestUtility.SignPackageFileWithBasicSignedCmsAsync(
                    directory,
                    unsignedPackageFile,
                    certificate);

                using (var packageReader = new PackageArchiveReader(signedPackageFile.FullName))
                {
                    var signature = await packageReader.GetPrimarySignatureAsync(CancellationToken.None);

                    using (var certificates = SignatureUtility.GetCertificateChain(signature))
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
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            primarySignature = RemoveRepositoryCountersignature(primarySignature);

            var exception = Assert.Throws<ArgumentException>(
                () => SignatureUtility.GetCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal("repositoryCountersignature", exception.ParamName);
            Assert.StartsWith("The primary signature and repository countersignature are unrelated.", exception.Message);
        }

        [Fact]
        public void GetCertificateChain_WithRepositoryCountersignature_ReturnsCertificates()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            using (var certificates = SignatureUtility.GetCertificateChain(primarySignature, repositoryCountersignature))
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
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithoutTimestamp_Throws()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            primarySignature = RemoveTimestamp(primarySignature);

            var exception = Assert.Throws<SignatureException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature));

            Assert.Equal(NuGetLogCode.NU3000, exception.Code);
            Assert.Equal("The primary signature does not have a timestamp.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithAuthorSignatureTimestamp_ReturnsCertificates()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            using (var certificates = SignatureUtility.GetTimestampCertificateChain(primarySignature))
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
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            primarySignature = RemoveRepositoryCountersignature(primarySignature);

            var exception = Assert.Throws<ArgumentException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal("repositoryCountersignature", exception.ParamName);
            Assert.StartsWith("The primary signature and repository countersignature are unrelated.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithRepositoryCountersignatureWithoutTimestamp_Throws()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));

            primarySignature = RemoveRepositoryCountersignatureTimestamp(primarySignature);

            var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            var exception = Assert.Throws<SignatureException>(
                () => SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature));

            Assert.Equal(NuGetLogCode.NU3000, exception.Code);
            Assert.Equal("The repository countersignature does not have a timestamp.", exception.Message);
        }

        [Fact]
        public void GetTimestampCertificateChain_WithRepositoryCountersignatureTimestamp_ReturnsCertificates()
        {
            var primarySignature = PrimarySignature.Load(SigningTestUtility.GetResourceBytes(".signature.p7s"));
            var repositoryCountersignature = RepositoryCountersignature.GetRepositoryCountersignature(primarySignature);

            using (var certificates = SignatureUtility.GetTimestampCertificateChain(primarySignature, repositoryCountersignature))
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
            var exception = Assert.Throws<ArgumentNullException>(
                () => SignatureUtility.HasRepositoryCountersignature(primarySignature: null));

            Assert.Equal("primarySignature", exception.ParamName);
        }

        [Fact]
        public async Task HasRepositoryCountersignature_WithSignatureWithoutRepositoryCountersignature_ReturnsFalseAsync()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();
                var unsignedPackageStream = await packageContext.CreateAsStreamAsync();

                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(
                    certificate,
                    unsignedPackageStream);

                var hasRepoCountersignature = SignatureUtility.HasRepositoryCountersignature(signature);

                Assert.False(hasRepoCountersignature);
            }
        }

        [Fact]
        public async Task HasRepositoryCountersignature_WithSignatureWithRepositoryCountersignature_ReturnsTrueAsync()
        {
            using (var certificate = _fixture.GetDefaultCertificate())
            using (var repositoryCertificate = _fixture.GetDefaultCertificate())
            {
                var packageContext = new SimpleTestPackageContext();
                var unsignedPackageStream = await packageContext.CreateAsStreamAsync();

                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(
                    certificate,
                    unsignedPackageStream);

                var hashAlgorithm = Common.HashAlgorithmName.SHA256;
                var v3ServiceIndexUri = new Uri("https://v3serviceIndex.test/api/index.json");
                using (var request = new RepositorySignPackageRequest(repositoryCertificate, hashAlgorithm, hashAlgorithm, v3ServiceIndexUri, null))
                {
                    var repoCountersignedSignature = await SignedArchiveTestUtility.RepositoryCountersignPrimarySignatureAsync(signature, request);
                    var hasRepoCountersignature = SignatureUtility.HasRepositoryCountersignature(repoCountersignedSignature);

                    Assert.True(hasRepoCountersignature);
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
                .Returns((ILogMessage)null);

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

        private static PrimarySignature GeneratePrimarySignatureWithNoCertificates(PrimarySignature signature)
        {
            var certificateStore = X509StoreFactory.Create(
                "Certificate/Collection",
                new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Certificate>()));
            var crlStore = X509StoreFactory.Create(
                "CRL/Collection",
                new X509CollectionStoreParameters(Array.Empty<Org.BouncyCastle.X509.X509Crl>()));
            var bytes = signature.SignedCms.Encode();

            using (var readStream = new MemoryStream(bytes))
            using (var writeStream = new MemoryStream())
            {
                CmsSignedDataParser.ReplaceCertificatesAndCrls(
                    readStream,
                    certificateStore,
                    crlStore,
                    certificateStore,
                    writeStream);

                return PrimarySignature.Load(writeStream.ToArray());
            }
        }

        private static PrimarySignature RemoveTimestamp(PrimarySignature signature)
        {
            return RemoveUnsignedAttribute(
                signature,
                attributes => attributes.Remove(PkcsObjectIdentifiers.IdAASignatureTimeStampToken));
        }

        private static PrimarySignature RemoveRepositoryCountersignature(PrimarySignature signature)
        {
            return RemoveUnsignedAttribute(
                signature,
                attributes => attributes.Remove(new DerObjectIdentifier(Oids.Countersignature)));
        }

        private static PrimarySignature RemoveRepositoryCountersignatureTimestamp(PrimarySignature signature)
        {
            var bytes = signature.GetBytes();
            var signedData = new CmsSignedData(bytes);
            var signerInfos = signedData.GetSignerInfos();
            var signerInfo = GetFirstSignerInfo(signerInfos);

            var countersignerInfos = signerInfo.GetCounterSignatures();
            var countersignerInfo = GetFirstSignerInfo(countersignerInfos);
            var updatedCountersignerAttributes = countersignerInfo.UnsignedAttributes.Remove(new DerObjectIdentifier(Oids.SignatureTimeStampTokenAttribute));
            var updatedCountersignerInfo = SignerInformation.ReplaceUnsignedAttributes(countersignerInfo, updatedCountersignerAttributes);
            var updatedSignerAttributes = signerInfo.UnsignedAttributes.Remove(new DerObjectIdentifier(Oids.Countersignature));

            updatedSignerAttributes = updatedSignerAttributes.Add(CmsAttributes.CounterSignature, updatedCountersignerInfo.ToSignerInfo());

            var updatedSignerInfo = SignerInformation.ReplaceUnsignedAttributes(signerInfo, updatedSignerAttributes);

            var updatedSignerInfos = new SignerInformationStore(updatedSignerInfo);
            var updatedSignedData = CmsSignedData.ReplaceSigners(signedData, updatedSignerInfos);

            return PrimarySignature.Load(updatedSignedData.GetEncoded());
        }

        private static PrimarySignature RemoveUnsignedAttribute(PrimarySignature signature, Func<AttributeTable, AttributeTable> remover)
        {
            var bytes = signature.GetBytes();
            var signedData = new CmsSignedData(bytes);
            var signerInfos = signedData.GetSignerInfos();
            var signerInfo = GetFirstSignerInfo(signerInfos);

            var updatedAttributes = remover(signerInfo.UnsignedAttributes);
            var updatedSignerInfo = SignerInformation.ReplaceUnsignedAttributes(signerInfo, updatedAttributes);
            var updatedSignerInfos = new SignerInformationStore(updatedSignerInfo);

            var updatedSignedData = CmsSignedData.ReplaceSigners(signedData, updatedSignerInfos);

            return PrimarySignature.Load(updatedSignedData.GetEncoded());
        }

        private static SignerInformation GetFirstSignerInfo(SignerInformationStore store)
        {
            var signers = store.GetSigners();
            var enumerator = signers.GetEnumerator();

            enumerator.MoveNext();

            return (SignerInformation)enumerator.Current;
        }
    }
}
#endif
