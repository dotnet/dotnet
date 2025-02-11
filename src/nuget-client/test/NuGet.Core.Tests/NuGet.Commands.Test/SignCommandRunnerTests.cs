// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
#if IS_SIGNING_SUPPORTED
using System.IO.Compression;
#endif
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class SignCommandRunnerTests : IClassFixture<CertificatesFixture>, IClassFixture<X509TrustTestFixture>
    {
        private readonly CertificatesFixture _fixture;

        public SignCommandRunnerTests(CertificatesFixture fixture)
        {
            _fixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public async Task ExecuteCommandAsync_WithCertificateFileNotFound_RaisesErrorsOnceAsync()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                var certificateFilePath = Path.Combine(testContext.Directory.Path, "certificate.pfx");

                testContext.Args.CertificatePath = certificateFilePath;

                await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                var expectedMessage = $"Certificate file '{certificateFilePath}' not found. For a list of accepted ways to provide a certificate, visit https://docs.nuget.org/docs/reference/command-line-reference";

                Assert.Equal(1, testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Error && message.Code == NuGetLogCode.NU3001 && message.Message.Equals(expectedMessage)));
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithEmptyPkcs7File_RaisesErrorsOnceAsync()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                const string fileName = "EmptyCertificateStore.p7b";
                var certificateFilePath = Path.Combine(testContext.Directory.Path, fileName);

                // This resource was created by calling X509Certificate2Collection.Export(X509ContentType.SerializedStore)
                // with no certificates in the collection.  Programmatic creation works fine on Windows but not under Mono,
                // hence the static resource.
                var bytes = GetResource(fileName);

                File.WriteAllBytes(certificateFilePath, bytes);

                testContext.Args.CertificatePath = certificateFilePath;

                int result = await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                var expectedMessage = $"Certificate file '{certificateFilePath}' is invalid. For a list of accepted ways to provide a certificate, visit https://docs.nuget.org/docs/reference/command-line-reference";
                Assert.Equal(1, result);
                Assert.True(1 == testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Error && message.Code == NuGetLogCode.NU3001 && message.Message.Equals(expectedMessage)),
                    string.Join(Environment.NewLine, testContext.Logger.LogMessages));
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithNoCertificateFound_RaisesErrorsOnceAsync()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                testContext.Args.CertificateFingerprint = "invalid fingerprint";
                testContext.Args.CertificateStoreLocation = StoreLocation.CurrentUser;
                testContext.Args.CertificateStoreName = StoreName.My;

                await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                var expectedMessage = $"No certificates were found that meet all the given criteria. For a list of accepted ways to provide a certificate, visit https://docs.nuget.org/docs/reference/command-line-reference";

                Assert.Equal(1, testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Error && message.Code == NuGetLogCode.NU3001 && message.Message.Equals(expectedMessage)));
            }
        }

        // Skip the tests when signing is not supported.
#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task ExecuteCommandAsync_WithExistingCertificateFromPathAndNoPassword_Succeed()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                const string fileName = "ExistingCertFile.pfx";
                var certificateFilePath = Path.Combine(testContext.Directory.Path, fileName);

                var bytes = testContext.Certificate.Export(X509ContentType.Pfx);

                File.WriteAllBytes(certificateFilePath, bytes);

                testContext.Args.CertificatePath = certificateFilePath;

                testContext.Args.SignatureHashAlgorithm = HashAlgorithmName.SHA256;
                testContext.Args.TimestampHashAlgorithm = HashAlgorithmName.SHA256;

                var returncode = await testContext.Runner.ExecuteCommandAsync(testContext.Args);
                Assert.Equal(returncode, 0);

                var packagePaths = testContext.Args.PackagePaths;
                Assert.Equal(packagePaths.Count, 1);

                var packagePath = packagePaths[0];
                using (var zip = new ZipArchive(File.OpenRead(packagePath), ZipArchiveMode.Read))
                {
                    var signatureEntry = zip.GetEntry(".signature.p7s");

                    Assert.NotNull(signatureEntry);
                }
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithExistingCertificateFromPathAndCorrectPassword_Succeed()
        {
            const string password = "password";

            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                const string fileName = "ExistingCertFile.pfx";
                var certificateFilePath = Path.Combine(testContext.Directory.Path, fileName);

                var bytes = testContext.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(certificateFilePath, bytes);

                testContext.Args.CertificatePath = certificateFilePath;
                testContext.Args.CertificatePassword = "password";

                testContext.Args.SignatureHashAlgorithm = HashAlgorithmName.SHA256;
                testContext.Args.TimestampHashAlgorithm = HashAlgorithmName.SHA256;

                var returncode = await testContext.Runner.ExecuteCommandAsync(testContext.Args);
                Assert.Equal(returncode, 0);

                var packagePaths = testContext.Args.PackagePaths;
                Assert.Equal(packagePaths.Count, 1);

                var packagePath = packagePaths[0];
                using (var zip = new ZipArchive(File.OpenRead(packagePath), ZipArchiveMode.Read))
                {
                    var signatureEntry = zip.GetEntry(".signature.p7s");

                    Assert.NotNull(signatureEntry);
                }
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithExistingCertificateFromPathAndWrongPassword_RaisesErrorsOnceAsync()
        {
            const string password = "password";

            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                const string fileName = "ExistingCertFile.pfx";
                var certificateFilePath = Path.Combine(testContext.Directory.Path, fileName);

                var bytes = testContext.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(certificateFilePath, bytes);

                testContext.Args.CertificatePath = certificateFilePath;
                testContext.Args.CertificatePassword = "PlaceholderPassword";

                testContext.Args.SignatureHashAlgorithm = HashAlgorithmName.SHA256;
                testContext.Args.TimestampHashAlgorithm = HashAlgorithmName.SHA256;

                await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                var expectedMessage = $"Invalid password was provided for the certificate file '{certificateFilePath}'. Provide a valid password using the '-CertificatePassword' option";

                Assert.Equal(1, testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Error && message.Code == NuGetLogCode.NU3001 && message.Message.Equals(expectedMessage)));
            }
        }

        [Fact]
        public async Task ExecuteCommandAsync_WithExistingCertificateFromStoreAndNoPassword_Succeed()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetTrustedCertificate()))
            {
                testContext.Args.CertificateStoreName = StoreName.My;
                testContext.Args.CertificateStoreLocation = StoreLocation.CurrentUser;
                testContext.Args.CertificateFingerprint = testContext.Certificate.Thumbprint;

                testContext.Args.SignatureHashAlgorithm = HashAlgorithmName.SHA256;
                testContext.Args.TimestampHashAlgorithm = HashAlgorithmName.SHA256;

                var returncode = await testContext.Runner.ExecuteCommandAsync(testContext.Args);
                Assert.Equal(returncode, 0);

                var packagePaths = testContext.Args.PackagePaths;
                Assert.Equal(packagePaths.Count, 1);

                var packagePath = packagePaths[0];
                using (var zip = new ZipArchive(File.OpenRead(packagePath), ZipArchiveMode.Read))
                {
                    var signatureEntry = zip.GetEntry(".signature.p7s");

                    Assert.NotNull(signatureEntry);
                }
            }
        }
#endif

        [Fact]
        public async Task ExecuteCommandAsync_WithAmbiguousMatch_RaisesErrorsOnceAsync()
        {
            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                testContext.Args.CertificateSubjectName = "Root";
                //X509 store is opened in ReadOnly mode in this code path. Hence StoreLocation is set to LocalMachine.
                testContext.Args.CertificateStoreLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation(readOnly: true);
                testContext.Args.CertificateStoreName = StoreName.Root;

                await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                var expectedMessage = "Multiple certificates were found that meet all the given criteria. Use the '-CertificateFingerprint' option with the hash of the desired certificate.";

                Assert.Equal(1, testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Error && message.Code == NuGetLogCode.NU3001 && message.Message.Equals(expectedMessage)));
            }
        }

        //skip this test when signing is not supported.
#if IS_SIGNING_SUPPORTED
        [Fact]
        public async Task ExecuteCommandAsync_WithMultiplePackagesAndInvalidCertificate_RaisesErrorsOnceAsync()
        {
            const string password = "password";

            using (TestContext testContext = await TestContext.CreateAsync(_fixture.GetDefaultCertificate()))
            {
                var certificateFilePath = Path.Combine(testContext.Directory.Path, "certificate.pfx");

                var bytes = testContext.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(certificateFilePath, bytes);

                testContext.Args.CertificatePath = certificateFilePath;
                testContext.Args.CertificatePassword = password;

                await TestContext.CreatePackageAsync(testContext.Directory, "package2.nupkg");

                var packagesFilePath = Path.Combine(testContext.Directory, "*.nupkg");

                testContext.Args.PackagePaths = new string[] { packagesFilePath };
                testContext.Args.SignatureHashAlgorithm = HashAlgorithmName.SHA256;
                testContext.Args.TimestampHashAlgorithm = HashAlgorithmName.SHA256;

                await testContext.Runner.ExecuteCommandAsync(testContext.Args);

                Assert.Equal(1, testContext.Logger.LogMessages.Count(
                    message => message.Level == LogLevel.Warning && message.Code == NuGetLogCode.NU3018));
            }
        }
#endif

        private static byte[] GetResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"NuGet.Commands.Test.compiler.resources.{name}",
                typeof(SignCommandRunnerTests));
        }

        private sealed class TestContext : IDisposable
        {
            private bool _isDisposed;

            internal SignArgs Args { get; }
            internal X509Certificate2 Certificate { get; }
            internal TestDirectory Directory { get; }
            internal TestLogger Logger { get; }
            internal SignCommandRunner Runner { get; }

            private TestContext(SignArgs args, TestDirectory directory, X509Certificate2 certificate, TestLogger logger)
            {
                Args = args;
                Directory = directory;
                Certificate = certificate;
                Runner = new SignCommandRunner();
                Logger = logger;
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    Certificate.Dispose();
                    Directory.Dispose();

                    GC.SuppressFinalize(this);

                    _isDisposed = true;
                }
            }

            internal static async Task<TestContext> CreateAsync(X509Certificate2 certificate)
            {
                var directory = TestDirectory.Create();
                var packageFilePath = await CreatePackageAsync(directory, "package.nupkg");
                var logger = new TestLogger();

                var args = new SignArgs()
                {
                    Logger = logger,
                    NonInteractive = true,
                    PackagePaths = new string[] { packageFilePath }
                };

                return new TestContext(args, directory, certificate, logger);
            }

            internal static async Task<string> CreatePackageAsync(TestDirectory directory, string packageFileName)
            {
                var packageFilePath = Path.Combine(directory.Path, packageFileName);
                var package = new SimpleTestPackageContext();

                using (var readStream = await package.CreateAsStreamAsync())
                using (var writeStream = File.OpenWrite(packageFilePath))
                {
                    readStream.CopyTo(writeStream);
                }

                return packageFilePath;
            }
        }
    }
}
