// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    [Collection(SigningTestCollection.Name)]
    public class FallbackCertificateBundleX509ChainFactoryTests : CertificateBundleX509ChainFactoryTests
    {
        public FallbackCertificateBundleX509ChainFactoryTests(SigningTestFixture fixture)
            : base(fixture)
        {
        }

        [CIOnlyFact]
        public void AdditionalContext_WhenRootCertificateIsUntrusted_ReturnsLogMessage()
        {
            using (TestDirectory directory = TestDirectory.Create())
            {
                FileInfo certificateBundle = CreateCertificateBundle(directory);
                Assert.True(FallbackCertificateBundleX509ChainFactory.TryCreate(
                    X509StorePurpose.CodeSigning,
                    certificateBundle.FullName,
                    out FallbackCertificateBundleX509ChainFactory factory));

                using (IX509Chain chain = factory.Create())
                {
                    X509Certificate2 certificate = Fixture.UntrustedTestCertificate.Cert;

                    string expectedMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.UntrustedRoot_WithCertificateBundle,
                        certificateBundle.FullName,
                        certificate.Subject,
                        GetCertificateFingerprint(certificate),
                        GetPemEncodedCertificate(certificate));

                    Assert.False(chain.Build(certificate));
                    Assert.NotNull(chain.AdditionalContext);

                    ILogMessage logMessage = chain.AdditionalContext;

                    Assert.Equal(NuGetLogCode.NU3042, logMessage.Code);
                    Assert.Equal(LogLevel.Warning, logMessage.Level);
                    Assert.Equal(expectedMessage, logMessage.Message);
                }
            }
        }
    }
}

#endif
