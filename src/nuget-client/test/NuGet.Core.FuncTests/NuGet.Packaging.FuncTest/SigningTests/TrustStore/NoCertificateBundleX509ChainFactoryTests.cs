// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NET5_0_OR_GREATER

using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.FuncTest.SigningTests
{
    [Collection(SigningTestCollection.Name)]
    public class NoCertificateBundleX509ChainFactoryTests : CertificateBundleX509ChainFactoryTests
    {
        public NoCertificateBundleX509ChainFactoryTests(SigningTestFixture fixture)
            : base(fixture)
        {
        }

        [CIOnlyFact]
        public void AdditionalContext_WhenRootCertificateIsUntrusted_ReturnsLogMessage()
        {
            NoCertificateBundleX509ChainFactory factory = new();

            using (IX509Chain chain = factory.Create())
            {
                X509Certificate2 certificate = Fixture.UntrustedTestCertificate.Cert;

                string expectedMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.UntrustedRoot_WithoutCertificateBundle,
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

#endif
