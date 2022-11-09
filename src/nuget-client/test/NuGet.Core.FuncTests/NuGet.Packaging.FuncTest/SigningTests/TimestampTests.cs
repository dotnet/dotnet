// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    using X509StorePurpose = global::Test.Utility.Signing.X509StorePurpose;

    [Collection(SigningTestCollection.Name)]
    public class TimestampTests
    {
        private readonly SigningTestFixture _testFixture;
        private readonly TrustedTestCert<TestCertificate> _trustedTestCert;

        public TimestampTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = _testFixture.TrustedTestCertificate;
        }

        // https://github.com/NuGet/Home/issues/11459
        [PlatformFact(Platform.Windows, Platform.Linux, CIOnly = true)]
        public async Task Timestamp_Verify_WithOfflineRevocation_ReturnsCorrectFlagsAndLogsAsync()
        {
            var nupkg = new SimpleTestPackageContext();

            using (var testServer = await SigningTestServer.CreateAsync())
            using (var responders = new DisposableList<IDisposable>())
            using (var packageStream = await nupkg.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                CertificateAuthority rootCa = CertificateAuthority.Create(testServer.Url);
                CertificateAuthority intermediateCa = rootCa.CreateIntermediateCertificateAuthority();

                responders.Add(testServer.RegisterResponder(intermediateCa));
                responders.Add(testServer.RegisterResponder(rootCa));

                StoreLocation storeLocation = CertificateStoreUtilities.GetTrustedCertificateStoreLocation();

                using (var trustedServerRoot = TrustedTestCert.Create(
                    new X509Certificate2(rootCa.Certificate.GetEncoded()),
                    X509StorePurpose.Timestamping,
                    StoreName.Root,
                    storeLocation))
                {
                    var timestampService = TimestampService.Create(intermediateCa);

                    responders.Add(testServer.RegisterResponder(timestampService));

                    var timestampProvider = new Rfc3161TimestampProvider(timestampService.Url);

                    AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream, timestampProvider);
                    var timestamp = signature.Timestamps.First();

                    var settings = new SignedPackageVerifierSettings(
                        allowUnsigned: false,
                        allowUntrusted: false,
                        allowIllegal: false,
                        allowIgnoreTimestamp: false,
                        allowMultipleTimestamps: false,
                        allowNoTimestamp: false,
                        allowUnknownRevocation: false,
                        reportUnknownRevocation: true,
                        verificationTarget: VerificationTarget.All,
                        signaturePlacement: SignaturePlacement.Any,
                        repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.Always,
                        revocationMode: RevocationMode.Online);

                    var logs = new List<SignatureLog>();

                    var result = timestamp.Verify(signature, settings, HashAlgorithmName.SHA256, logs);

                    result.HasFlag(SignatureVerificationStatusFlags.UnknownRevocation).Should().BeTrue();

                    var errors = logs.Where(l => l.Level == LogLevel.Error);

                    if (RuntimeEnvironmentHelper.IsMacOSX)
                    {
                        errors.Count().Should().Be(1);
                    }
                    else
                    {
                        errors.Count().Should().Be(2);
                        SigningTestUtility.AssertOfflineRevocationOnlineMode(errors, LogLevel.Error, NuGetLogCode.NU3028);
                    }
                    SigningTestUtility.AssertRevocationStatusUnknown(errors, LogLevel.Error, NuGetLogCode.NU3028);
                }
            }
        }
    }
}
#endif
