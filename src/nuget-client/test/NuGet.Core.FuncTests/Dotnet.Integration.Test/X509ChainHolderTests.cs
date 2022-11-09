// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;

    [Collection(DotnetIntegrationCollection.Name)]
    public class X509ChainHolderTests
    {
        private readonly MsbuildIntegrationTestFixture _msbuildFixture;
        private readonly TestLogger _logger;

        public X509ChainHolderTests(MsbuildIntegrationTestFixture msbuildFixture, ITestOutputHelper helper)
        {
            _msbuildFixture = msbuildFixture;
            _logger = new TestLogger(helper);
        }

        [Fact]
        public void CreateForCodeSigning_Always_ReturnsRootCertificatesValidForCodeSigning()
        {
            FileInfo codeSigningCertificateBundle = new(
                Path.Combine(
                    _msbuildFixture.SdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.CodeSigningFileName));

            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            {
                X509ChainPolicy policy = chainHolder.Chain.ChainPolicy;

                Verify(codeSigningCertificateBundle, policy);
            }
        }

        [Fact]
        public void CreateForTimestamping_Always_ReturnsRootCertificatesValidForTimestamping()
        {
            FileInfo codeSigningCertificateBundle = new(
                Path.Combine(
                    _msbuildFixture.SdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.TimestampingFileName));

            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForTimestamping())
            {
                X509ChainPolicy policy = chainHolder.Chain.ChainPolicy;

                Verify(codeSigningCertificateBundle, policy);
            }
        }

        private static void AssertSameCertificates(
            X509Certificate2Collection expectedCertificates,
            X509Certificate2Collection actualCertificates)
        {
            Assert.Equal(expectedCertificates.Count, actualCertificates.Count);

            Dictionary<string, X509Certificate2> expectedCertificatesDictionary =
                expectedCertificates.ToDictionary(
                    certificate => certificate.GetCertHashString(HashAlgorithmName.SHA256),
                    certificate => certificate,
                    StringComparer.Ordinal);
            Dictionary<string, X509Certificate2> actualCertificatesDictionary =
                actualCertificates.ToDictionary(
                    certificate => certificate.GetCertHashString(HashAlgorithmName.SHA256),
                    certificate => certificate,
                    StringComparer.Ordinal);

            Assert.Equal(0, expectedCertificatesDictionary.Keys.Except(actualCertificatesDictionary.Keys).Count());
            Assert.Equal(0, actualCertificatesDictionary.Keys.Except(expectedCertificatesDictionary.Keys).Count());
        }

        private void Verify(FileInfo certificateBundleFile, X509ChainPolicy policy)
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                Assert.Equal(X509ChainTrustMode.System, policy.TrustMode);
            }
            else if (RuntimeEnvironmentHelper.IsLinux || RuntimeEnvironmentHelper.IsMacOSX)
            {
                X509Certificate2Collection expectedCertificates = LoadCertificateBundle(certificateBundleFile);

                Assert.Equal(X509ChainTrustMode.CustomRootTrust, policy.TrustMode);

                AssertSameCertificates(expectedCertificates, policy.CustomTrustStore);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private X509Certificate2Collection LoadCertificateBundle(FileInfo certificateBundle)
        {
            _logger.LogVerbose($"Expected fallback certificate bundle file path:  {certificateBundle.FullName}");
            _logger.LogVerbose($"Fallback certificate bundle file exists:  {certificateBundle.Exists}");

            X509Certificate2Collection certificates = new();

            certificates.ImportFromPemFile(certificateBundle.FullName);

            return certificates;
        }
    }
}
