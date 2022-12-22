// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace Dotnet.Integration.Test
{
    using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;

    [Collection(DotnetIntegrationCollection.Name)]
    public class X509ChainHolderTests
    {
        [Fact]
        public void CreateForCodeSigning_Always_ReturnsRootCertificatesValidForCodeSigning()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            {
                X509ChainPolicy policy = chainHolder.Chain.ChainPolicy;

                // Code signing certificates that chain to this root certificate are widely used on nuget.org.
                // CN=DigiCert Assured ID Root CA, OU=www.digicert.com, O=DigiCert Inc, C=US
                Verify(policy, expectedFingerprint: "3e9099b5015e8f486c00bcea9d111ee721faba355a89bcf1df69561e3dc6325c");
            }
        }

        [Fact]
        public void CreateForTimestamping_Always_ReturnsRootCertificatesValidForTimestamping()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForTimestamping())
            {
                X509ChainPolicy policy = chainHolder.Chain.ChainPolicy;

                // Timestamping certificates that chain to this root certificate are widely used on nuget.org.
                // CN=VeriSign Universal Root Certification Authority, OU="(c) 2008 VeriSign, Inc. - For authorized use only", OU=VeriSign Trust Network, O="VeriSign, Inc.", C=US
                Verify(policy, expectedFingerprint: "2399561127a57125de8cefea610ddf2fa078b5c8067f4e828290bfb860e84b3c");
            }
        }

        private void Verify(X509ChainPolicy policy, string expectedFingerprint)
        {
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                Assert.Equal(X509ChainTrustMode.System, policy.TrustMode);
            }
            else if (RuntimeEnvironmentHelper.IsLinux || RuntimeEnvironmentHelper.IsMacOSX)
            {
                Assert.Equal(X509ChainTrustMode.CustomRootTrust, policy.TrustMode);

                using (SHA256 hashAlgorithm = SHA256.Create())
                {
                    Assert.Contains(policy.CustomTrustStore, certificate =>
                    {
                        string actualFingerprint = certificate.GetCertHashString(HashAlgorithmName.SHA256);

                        return string.Equals(expectedFingerprint, actualFingerprint, StringComparison.OrdinalIgnoreCase);
                    });
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
