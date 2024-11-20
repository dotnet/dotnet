// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    using X509StorePurpose = Microsoft.Internal.NuGet.Testing.SignedPackages.X509StorePurpose;

    [Collection(DotnetIntegrationCollection.Name)]
    public class X509TrustStoreTests
    {
        private readonly FileInfo _codeSigningCertificateBundle;
        private readonly FileInfo _timestampingCertificateBundle;
        private readonly TestLogger _logger;
        private readonly ITestOutputHelper _testOutputHelper;

        public X509TrustStoreTests(DotnetIntegrationTestFixture dotnetFixture, ITestOutputHelper testOutputHelper)
        {
            _logger = new TestLogger(testOutputHelper);

            _codeSigningCertificateBundle = new FileInfo(
                Path.Combine(
                    dotnetFixture.SdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.CodeSigningFileName));
            _timestampingCertificateBundle = new FileInfo(
                Path.Combine(
                    dotnetFixture.SdkDirectory.FullName,
                    FallbackCertificateBundleX509ChainFactory.SubdirectoryName,
                    FallbackCertificateBundleX509ChainFactory.TimestampingFileName));

            _logger.LogVerbose($"Expected code signing fallback certificate bundle file path:  {_codeSigningCertificateBundle.FullName}");
            _logger.LogVerbose($"Code signing fallback certificate bundle file exists:  {_codeSigningCertificateBundle.Exists}");
            _logger.LogVerbose($"Expected timestamping fallback certificate bundle file path:  {_timestampingCertificateBundle.FullName}");
            _logger.LogVerbose($"Timestamping fallback certificate bundle file exists:  {_timestampingCertificateBundle.Exists}");
            _testOutputHelper = testOutputHelper;
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(X509StorePurpose.CodeSigning)]
        [InlineData(X509StorePurpose.Timestamping)]
        public void CreateX509ChainFactoryForDotNetSdk_OnWindowsAlways_ReturnsInstance(X509StorePurpose storePurpose)
        {
            FileInfo certificateBundle = GetCertificateBundle(storePurpose);
            IX509ChainFactory factory = X509TrustStore.CreateX509ChainFactoryForDotNetSdk(
                (NuGet.Packaging.Signing.X509StorePurpose)storePurpose,
                _logger,
                certificateBundle);

            Assert.IsType<DotNetDefaultTrustStoreX509ChainFactory>(factory);

            // 1 message from the API under test and 4 messages from this test class's constructor
            Assert.Equal(5, _logger.Messages.Count);
            Assert.Equal(1, _logger.InformationMessages.Count);
            Assert.True(_logger.InformationMessages.TryPeek(out string actualMessage));

            switch (storePurpose)
            {
                case X509StorePurpose.CodeSigning:
                    Assert.Equal(Strings.ChainBuilding_UsingDefaultTrustStoreForCodeSigning, actualMessage);
                    break;

                case X509StorePurpose.Timestamping:
                    Assert.Equal(Strings.ChainBuilding_UsingDefaultTrustStoreForTimestamping, actualMessage);
                    break;
            }
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData(X509StorePurpose.CodeSigning)]
        [InlineData(X509StorePurpose.Timestamping)]
        public void CreateX509ChainFactoryForDotNetSdk_OnLinuxAlways_ReturnsInstance(X509StorePurpose storePurpose)
        {
            FileInfo certificateBundle = GetCertificateBundle(storePurpose);
            IX509ChainFactory factory = X509TrustStore.CreateX509ChainFactoryForDotNetSdk(
                (NuGet.Packaging.Signing.X509StorePurpose)storePurpose,
                _logger,
                certificateBundle);

            bool wasFound = TryReadFirstBundle(
                SystemCertificateBundleX509ChainFactory.ProbePaths,
                out X509Certificate2Collection certificates,
                out string _);
            var certificateBundleFactory = (CertificateBundleX509ChainFactory)factory;

            if (wasFound)
            {
                Assert.IsType<SystemCertificateBundleX509ChainFactory>(factory);
                Assert.Equal(certificates.Count, certificateBundleFactory.Certificates.Count);
            }
            else
            {
                Assert.IsType<FallbackCertificateBundleX509ChainFactory>(factory);
                Assert.True(certificateBundleFactory.Certificates.Count > 0);
            }

            // 1 message from the API under test and 4 messages from this test class's constructor
            Assert.Equal(5, _logger.Messages.Count);
            Assert.Equal(1, _logger.InformationMessages.Count);
            Assert.True(_logger.InformationMessages.TryPeek(out string actualMessage));

            string expectedMessage;

            if (wasFound)
            {
                expectedMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ChainBuilding_UsingSystemCertificateBundle,
                    certificateBundleFactory.FilePath);
            }
            else
            {
                expectedMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ChainBuilding_UsingFallbackCertificateBundle,
                    certificateBundleFactory.FilePath);
            }

            Assert.Equal(expectedMessage, actualMessage);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData(X509StorePurpose.CodeSigning)]
        [InlineData(X509StorePurpose.Timestamping)]
        public void CreateX509ChainFactoryForDotNetSdk_OnMacOsAlways_ReturnsInstance(X509StorePurpose storePurpose)
        {
            FileInfo certificateBundle = GetCertificateBundle(storePurpose);
            IX509ChainFactory factory = X509TrustStore.CreateX509ChainFactoryForDotNetSdk(
                (NuGet.Packaging.Signing.X509StorePurpose)storePurpose,
                _logger,
                certificateBundle);

            Assert.IsType<FallbackCertificateBundleX509ChainFactory>(factory);

            string expectedMessage = string.Format(
                CultureInfo.CurrentCulture,
                Strings.ChainBuilding_UsingFallbackCertificateBundle,
                ((CertificateBundleX509ChainFactory)factory).FilePath);

            // 1 message from the API under test and 4 messages from this test class's constructor
            Assert.Equal(5, _logger.Messages.Count);
            Assert.Equal(1, _logger.InformationMessages.Count);
            Assert.True(_logger.InformationMessages.TryPeek(out string actualMessage));
            Assert.Equal(expectedMessage, actualMessage);
        }

        private static bool TryReadFirstBundle(
            IReadOnlyList<string> probePaths,
            out X509Certificate2Collection certificates,
            out string successfulProbePath)
        {
            certificates = null;
            successfulProbePath = null;

            var oneProbePath = new string[1];

            foreach (string probePath in probePaths)
            {
                oneProbePath[0] = probePath;

                if (SystemCertificateBundleX509ChainFactory.TryCreate(
                    oneProbePath,
                    out SystemCertificateBundleX509ChainFactory factory))
                {
                    certificates = factory.Certificates;
                    successfulProbePath = probePath;

                    return true;
                }
            }

            return false;
        }

        private FileInfo GetCertificateBundle(X509StorePurpose storePurpose)
        {
            if (storePurpose == X509StorePurpose.CodeSigning)
            {
                return _codeSigningCertificateBundle;
            }

            return _timestampingCertificateBundle;
        }
    }
}
