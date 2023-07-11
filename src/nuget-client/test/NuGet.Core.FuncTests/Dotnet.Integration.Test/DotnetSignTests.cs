// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetSignTests
    {
        private DotnetIntegrationTestFixture _msbuildFixture;
        private SignCommandTestFixture _signFixture;

        private const string _packageAlreadySignedError = "NU3001: The package already contains a signature. Please remove the existing signature before adding a new signature.";
        private readonly string _invalidPasswordError = "NU3001: Invalid password was provided for the certificate file";
        private readonly string _noCertFoundError = "NU3001: No certificates were found that meet all the given criteria.";
        private readonly string _chainBuildFailureErrorCode = NuGetLogCode.NU3018.ToString();
        private readonly string _noTimestamperWarningCode = NuGetLogCode.NU3002.ToString();
        private readonly string _timestampUnsupportedDigestAlgorithmCode = NuGetLogCode.NU3024.ToString();

        public DotnetSignTests(DotnetIntegrationTestFixture buildFixture, SignCommandTestFixture signFixture)
        {
            _msbuildFixture = buildFixture;
            _signFixture = signFixture;

            _signFixture.SetFallbackCertificateBundles(buildFixture.SdkDirectory);
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithTrustedCertificate_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithTrustedCertificateWithRelativePath_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                var packageFileName = "PackageA.1.0.0.nupkg";
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign .{Path.DirectorySeparatorChar}{packageFileName} " +
                    $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint} " +
                    $"--certificate-store-name {storeCertificate.StoreName} " +
                    $"--certificate-store-location {storeCertificate.StoreLocation}");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithInvalidEku_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.CertificateWithInvalidEku;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithExpiredCertificate_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.ExpiredCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithNotYetValidCertificate_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.NotYetValidCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithTimestamping_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                TimestampService timestampService = await _signFixture.GetDefaultTrustedTimestampServiceAsync();
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) +
                    $" --timestamper {timestampService.Url.OriginalString}");

                // Assert
                result.AllOutput.Should().NotContain(_noTimestamperWarningCode);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)] // https://github.com/NuGet/Home/issues/9501
        public async Task DotnetSign_SignPackageWithRevokedLeafCertChain_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.RevokedCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_noCertFoundError);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithUnknownRevocationCertChain_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.RevocationUnknownCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate));

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_chainBuildFailureErrorCode);
                result.AllOutput.Should().Contain(X509ChainStatusFlags.RevocationStatusUnknown.ToString());
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithOutputDirectory_SucceedsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                string outputDir = Path.Combine(pathContext.WorkingDirectory, "Output");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                Directory.CreateDirectory(outputDir);

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) +
                    $" --output {outputDir}");

                string signedPackagePath = Path.Combine(outputDir, "PackageA.1.0.0.nupkg");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                File.Exists(signedPackagePath).Should().BeTrue();
            }
        }

        [Fact]
        public async Task DotnetSign_ResignPackageWithoutOverwrite_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;
                string args = GetDefaultArgs(packageFilePath, storeCertificate);

                // Act
                CommandRunnerResult firstResult = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args);

                CommandRunnerResult secondResult = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    args);

                // Assert
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.AllOutput.Should().Contain(_packageAlreadySignedError);
            }
        }

        [Fact]
        public async Task DotnetSign_ResignPackageWithOverwrite_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;
                string args = GetDefaultArgs(packageFilePath, storeCertificate);

                // Act
                CommandRunnerResult firstResult = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args);

                CommandRunnerResult secondResult = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    args + " --overwrite");

                // Assert
                firstResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
                secondResult.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithOverwrite_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.DefaultCertificate;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    GetDefaultArgs(packageFilePath, storeCertificate) + " --overwrite");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFile_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
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
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-path {pfxPath} " +
                    $"--certificate-password {password}");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFileOfRelativePath_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
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
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-path .{Path.DirectorySeparatorChar}{pfxName} " +
                    $"--certificate-password {password}");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithPfxFileWithoutPasswordAndWithNonInteractive_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
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
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} --certificate-path {pfxPath}");

                // Assert
                result.AllOutput.Should().Contain(string.Format(_invalidPasswordError, pfxPath));
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithUntrustedSelfIssuedCertificateInCertificateStore_SuccessAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    new SimpleTestPackageContext("PackageA", "1.0.0"));

                string packageFilePath = Path.Combine(pathContext.PackageSource, "PackageA.1.0.0.nupkg");
                IX509StoreCertificate storeCertificate = _signFixture.UntrustedSelfIssuedCertificateInCertificateStore;

                // Act
                CommandRunnerResult result = _msbuildFixture.RunDotnetExpectSuccess(
                    pathContext.PackageSource,
                    $"nuget sign {packageFilePath} " +
                    $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint}");

                // Assert
                result.AllOutput.Should().Contain(_noTimestamperWarningCode);
                result.AllOutput.Should().Contain(_chainBuildFailureErrorCode);
            }
        }

        [Fact]
        public async Task DotnetSign_SignPackageWithUnsuportedTimestampHashAlgorithm_FailsAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = _msbuildFixture.CreateSimpleTestPathContext())
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
                    CommandRunnerResult result = _msbuildFixture.RunDotnetExpectFailure(
                        pathContext.PackageSource,
                        $"nuget sign {packageFilePath} " +
                        $"--certificate-fingerprint {storeCertificate.Certificate.Thumbprint} " +
                        $"--timestamper {timestampService.Url}");

                    // Assert
                    result.AllOutput.Should().Contain(_timestampUnsupportedDigestAlgorithmCode);
                    Assert.Contains("The timestamp signature has an unsupported digest algorithm (SHA1). The following algorithms are supported: SHA256, SHA384, SHA512.", result.AllOutput);

                    byte[] resultingFile = File.ReadAllBytes(packageFilePath);
                    Assert.Equal(resultingFile, originalFile);
                }
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
