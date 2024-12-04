// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;
using HashAlgorithmName = NuGet.Common.HashAlgorithmName;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetSignTests
    {
        private DotnetIntegrationTestFixture _dotnetFixture;
        private SignCommandTestFixture _signFixture;
        private readonly ITestOutputHelper _testOutputHelper;
        private const string _packageAlreadySignedError = "NU3001: The package already contains a signature. Please remove the existing signature before adding a new signature.";
        private readonly string _invalidPasswordError = "NU3001: Invalid password was provided for the certificate file";
        private readonly string _noCertFoundError = "NU3001: No certificates were found that meet all the given criteria.";
        private readonly string _chainBuildFailureErrorCode = NuGetLogCode.NU3018.ToString();
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3002.ToString();
        private readonly string _timestampUnsupportedDigestAlgorithmCode = NuGetLogCode.NU3024.ToString();
        private readonly string _insecureCertificateFingerprintCode = NuGetLogCode.NU3043.ToString();

        public DotnetSignTests(DotnetIntegrationTestFixture dotnetFixture, SignCommandTestFixture signFixture, ITestOutputHelper testOutputHelper)
        {
            _dotnetFixture = dotnetFixture;
            _signFixture = signFixture;
            _testOutputHelper = testOutputHelper;
            _signFixture.SetFallbackCertificateBundles(dotnetFixture.SdkDirectory);
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithTrustedCertificate_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithTrustedCertificateWithRelativePath_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                var packageFileName = "PackageA.1.0.0.nupkg";
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign .{Path.DirectorySeparatorChar}{packageFileName} " +
                    $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint} " +
                    $"--certificate-store-name {storeCertificate.StoreName} " +
                    $"--certificate-store-location {storeCertificate.StoreLocation}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithInvalidEku_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.CertificateWithInvalidEku;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithExpiredCertificate_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.ExpiredCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithNotYetValidCertificate_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.NotYetValidCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithTimestamping_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                TimestampService timestampService = await _signFixture.GetDefaultTrustedTimestampServiceAsync();
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) +
                    $" --timestamper {timestampService.Url.OriginalString}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
        public async Task DotnetSign_SignPackageWithRevokedLeafCertChain_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.RevokedCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithUnknownRevocationCertChain_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.RevocationUnknownCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate),
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_chainBuildFailureErrorCode);
                result.AllOutput.Should().Contain(X509ChainStatusFlags.RevocationStatusUnknown.ToString());
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithOutputDirectory_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                string outputDir = Path.Combine(pathContext.WorkingDirectory, "Output");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                Directory.CreateDirectory(outputDir);

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) +
                    $" --output {outputDir}",
                    testOutputHelper: _testOutputHelper);

                string signedPackagePath = Path.Combine(outputDir, "PackageA.1.0.0.nupkg");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                File.Exists(signedPackagePath).Should().BeTrue();
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_ResignPackageWithoutOverwrite_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;
                string args = GetDefaultArgs(packageFilePath, storeCertificate);

                // Act
                CommandRunnerResult firstResult = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args,
                    testOutputHelper: _testOutputHelper);

                CommandRunnerResult secondResult = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    args,
                    testOutputHelper: _testOutputHelper);

                // Assert
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.AllOutput.Should().Contain(_packageAlreadySignedError);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_ResignPackageWithOverwrite_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;
                string args = GetDefaultArgs(packageFilePath, storeCertificate);

                // Act
                CommandRunnerResult firstResult = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args,
                    testOutputHelper: _testOutputHelper);

                CommandRunnerResult secondResult = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args + " --overwrite",
                    testOutputHelper: _testOutputHelper);

                // Assert
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithOverwrite_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) + " --overwrite",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFile_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                string pfxPath = Path.Combine(pathContext.WorkingDirectory, Guid.NewGuid().ToString());
                string password = Guid.NewGuid().ToString();
                byte[] pfxBytes = storeCertificate.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(pfxPath, pfxBytes);

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-path {pfxPath} " +
                    $"--certificate-password {password}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFileOfRelativePath_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                string pfxName = Guid.NewGuid().ToString() + ".pfx";
                string pfxPath = Path.Combine(pathContext.PackageSource, pfxName);
                string password = Guid.NewGuid().ToString();
                byte[] pfxBytes = storeCertificate.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(pfxPath, pfxBytes);

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-path .{Path.DirectorySeparatorChar}{pfxName} " +
                    $"--certificate-password {password}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFileWithoutPasswordAndWithNonInteractive_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;
                string pfxPath = Path.Combine(pathContext.WorkingDirectory, Guid.NewGuid().ToString());
                string password = Guid.NewGuid().ToString();
                byte[] pfxBytes = storeCertificate.Certificate.Export(X509ContentType.Pfx, password);

                File.WriteAllBytes(pfxPath, pfxBytes);

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} --certificate-path {pfxPath}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(string.Format(_invalidPasswordError, pfxPath));
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithUntrustedSelfIssuedCertificateInCertificateStore_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint}",
                    testOutputHelper: _testOutputHelper);

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_chainBuildFailureErrorCode);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithUnsuportedTimestampHashAlgorithm_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                byte[] originalFile = File.ReadAllBytes(packageFilePath);

                ISigningTestServer testServer = await _signFixture.GetSigningTestServerAsync();
                CertificateAuthority certificateAuthority = await _signFixture.GetDefaultTrustedTimestampingRootCertificateAuthorityAsync();
                var options = new TimestampServiceOptions() { SignatureHashAlgorithm = new Oid(Oids.Sha1) };
                TimestampService timestampService = TimestampService.Create(certificateAuthority, options);
                IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

                using (testServer.RegisterResponder(timestampService))
                {
                    // Act
                    CommandRunnerResult result = _dotnetFixture.RunDotnetExpectFailure(
                        pathContext.PackageSource,
                        $"nuget sign {packageFilePath} " +
                        $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint} " +
                        $"--timestamper {timestampService.Url}",
                    testOutputHelper: _testOutputHelper);

                    // Assert
                    result.AllOutput.Should().Contain(_timestampUnsupportedDigestAlgorithmCode);
                    Assert.Contains("The timestamp signature has an unsupported digest algorithm (SHA1). The following algorithms are supported: SHA256, SHA384, SHA512.", result.AllOutput);
                    Assert.True(result.AllOutput.Contains(_insecureCertificateFingerprintCode), result.AllOutput);

                    byte[] resultingFile = File.ReadAllBytes(packageFilePath);
                    Assert.Equal(resultingFile, originalFile);
                }
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        public async Task DotnetSign_SignPackageWithInsecureCertificateFingerprint_RaisesWarningAsync()
        {
            var result = await ExecuteSignPackageTestWithCertificateFingerprintAsync(HashAlgorithmName.SHA1);

            if (typeof(int).Assembly.GetName().Version.Major >= 10)
            {
                Assert.False(result.Success, result.AllOutput);
                Assert.True(result.Errors.Contains(_insecureCertificateFingerprintCode), result.Errors);
            }
            else
            {
                Assert.True(result.Success, result.AllOutput);
                Assert.True(result.AllOutput.Contains(_insecureCertificateFingerprintCode), result.AllOutput);
            }
        }

        [PlatformTheory(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Client.Engineering/issues/2781
        [InlineData(HashAlgorithmName.SHA256)]
        [InlineData(HashAlgorithmName.SHA384)]
        [InlineData(HashAlgorithmName.SHA512)]
        public async Task DotnetSign_SignPackageWithSecureCertificateFingerprint_SucceedsAsync(HashAlgorithmName hashAlgorithmName)
        {
            var result = await ExecuteSignPackageTestWithCertificateFingerprintAsync(hashAlgorithmName);

            Assert.True(result.Success, result.AllOutput);
            Assert.False(result.AllOutput.Contains(_insecureCertificateFingerprintCode), result.AllOutput);
        }

        private async Task<CommandRunnerResult> ExecuteSignPackageTestWithCertificateFingerprintAsync(HashAlgorithmName hashAlgorithmName)
        {
            // Arrange
            using SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext();
            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PackageA", "1.0.0"));

            string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
            byte[] originalFile = File.ReadAllBytes(packageFilePath);

            ISigningTestServer testServer = await _signFixture.GetSigningTestServerAsync();
            CertificateAuthority certificateAuthority = await _signFixture.GetDefaultTrustedTimestampingRootCertificateAuthorityAsync();
            var options = new TimestampServiceOptions() { SignatureHashAlgorithm = new Oid(Oids.Sha256) };
            TimestampService timestampService = TimestampService.Create(certificateAuthority, options);
            IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;
            string certFingerprint = hashAlgorithmName == HashAlgorithmName.SHA1 ? storeCertificate.Certificate.Thumbprint :
                SignatureTestUtility.GetFingerprint(storeCertificate.Certificate, hashAlgorithmName);

            using (testServer.RegisterResponder(timestampService))
            {
                // Act
                CommandRunnerResult result = _dotnetFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-fingerprint {certFingerprint} " +
                    $"--timestamper {timestampService.Url}",
                    testOutputHelper: _testOutputHelper);

                return result;
            }
        }

        private static string GetDefaultArgs(string packageFilePath, IX509StoreCertificate storeCertificate)
        {
            return $"nuget sign {packageFilePath} " +
                $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint} " +
                $"--certificate-store-name {storeCertificate.StoreName} " +
                $"--certificate-store-location {storeCertificate.StoreLocation}";
        }
    }
}
