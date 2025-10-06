// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Internal.NuGet.Testing.SignedPackages;

namespace NuGet.Tests.Apex
{
    public static class SigningUtility
    {
        private static readonly TimeSpan SoonDuration = TimeSpan.FromSeconds(20);

        internal static TrustedTestCert<TestCertificate> GenerateTrustedTestCertificateThatWillExpireSoon()
        {
            return SigningTestUtility.GenerateTrustedTestCertificateThatWillExpireSoon(SoonDuration);
        }

        internal static bool IsCertificateExpired(X509Certificate2 certificate)
        {
            return DateTime.Now > certificate.NotAfter;
        }

        internal static void WaitForCertificateToExpire(X509Certificate2 certificate)
        {
            while (DateTimeOffset.Now < certificate.NotAfter)
            {
                Thread.Sleep(100);
            }

            Assert.IsTrue(SigningUtility.IsCertificateExpired(certificate));
        }

    }
}
