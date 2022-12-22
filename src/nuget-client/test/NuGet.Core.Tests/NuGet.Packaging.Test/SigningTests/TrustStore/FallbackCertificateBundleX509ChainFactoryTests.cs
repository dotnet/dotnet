// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    [Collection(SigningTestsCollection.Name)]
    public sealed class FallbackCertificateBundleX509ChainFactoryTests : IDisposable
    {
        private readonly TestDirectory _directory;
        private readonly CertificatesFixture _fixture;

        public FallbackCertificateBundleX509ChainFactoryTests(CertificatesFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _directory = TestDirectory.Create();
        }

        public void Dispose()
        {
            _directory.Dispose();
        }

        [Fact]
        public void TryCreate_WhenFilePathDoesNotExist_ReturnsFalse()
        {
            string nonexistentFilePath = Path.Combine(_directory.Path, FallbackCertificateBundleX509ChainFactory.CodeSigningFileName);

            bool wasCreated = FallbackCertificateBundleX509ChainFactory.TryCreate(
                X509StorePurpose.CodeSigning,
                nonexistentFilePath,
                out FallbackCertificateBundleX509ChainFactory factory);

            Assert.False(wasCreated);
            Assert.Null(factory);
        }

        [Fact]
        public void TryCreate_WhenFilePathIsRelative_SetsFullFilePath()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                DirectoryInfo subdirectory = new(Path.Combine(currentDirectory, Path.GetRandomFileName()));

                try
                {
                    subdirectory.Create();

                    FileInfo bundleFile = CreateBundleFile(subdirectory.FullName, certificate);
                    string relativePath = ".." + bundleFile.FullName.Substring(currentDirectory.Length);

                    bool wasCreated = FallbackCertificateBundleX509ChainFactory.TryCreate(
                        X509StorePurpose.CodeSigning,
                        relativePath,
                        out FallbackCertificateBundleX509ChainFactory factory);

                    string absoluteActualPath = Path.GetFullPath(factory.FilePath);

                    Assert.True(wasCreated);
                    Assert.Equal(bundleFile.FullName, absoluteActualPath);
                }
                finally
                {
                    subdirectory.Delete(recursive: true);
                }
            }
        }

        [Fact]
        public void TryCreate_WhenFilePathIsRooted_SetsFullFilePath()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                FileInfo bundleFile = CreateBundleFile(_directory.Path, certificate);

                bool wasCreated = FallbackCertificateBundleX509ChainFactory.TryCreate(
                    X509StorePurpose.CodeSigning,
                    bundleFile.FullName,
                    out FallbackCertificateBundleX509ChainFactory factory);

                Assert.True(wasCreated);
                Assert.Equal(bundleFile.FullName, factory.FilePath);
            }
        }

        [Fact]
        public void Create_WhenFileIsEmpty_ReturnsInstance()
        {
            FileInfo emptyFile = new(Path.Combine(_directory.Path, FallbackCertificateBundleX509ChainFactory.CodeSigningFileName));

            File.WriteAllBytes(emptyFile.FullName, Array.Empty<byte>());

            bool wasCreated = FallbackCertificateBundleX509ChainFactory.TryCreate(
                X509StorePurpose.CodeSigning,
                emptyFile.FullName,
                out FallbackCertificateBundleX509ChainFactory factory);

            Assert.True(wasCreated);

            using (X509Chain chain = factory.Create())
            {
                Assert.Equal(X509ChainTrustMode.CustomRootTrust, chain.ChainPolicy.TrustMode);
                Assert.Empty(chain.ChainPolicy.CustomTrustStore);
            }
        }

        [Fact]
        public void Create_WhenFileIsValid_ReturnsInstance()
        {
            using (X509Certificate2 certificate = _fixture.GetDefaultCertificate())
            {
                FileInfo bundleFile = CreateBundleFile(_directory, certificate);
                bool wasCreated = FallbackCertificateBundleX509ChainFactory.TryCreate(
                    X509StorePurpose.CodeSigning,
                    bundleFile.FullName,
                    out FallbackCertificateBundleX509ChainFactory factory);

                Assert.True(wasCreated);

                using (X509Chain chain = factory.Create())
                {
                    Assert.Equal(X509ChainTrustMode.CustomRootTrust, chain.ChainPolicy.TrustMode);
                    Assert.Equal(1, chain.ChainPolicy.CustomTrustStore.Count);
                    Assert.Equal(certificate.Thumbprint, chain.ChainPolicy.CustomTrustStore[0].Thumbprint);
                }
            }
        }

        private static FileInfo CreateBundleFile(string directoryPath, X509Certificate2 certificate)
        {
            FileInfo file = new(Path.Combine(directoryPath, FallbackCertificateBundleX509ChainFactory.CodeSigningFileName));
            char[] pem = PemEncoding.Write("CERTIFICATE", certificate.RawData);

            File.WriteAllText(file.FullName, new string(pem));

            return file;
        }
    }
}
#endif
