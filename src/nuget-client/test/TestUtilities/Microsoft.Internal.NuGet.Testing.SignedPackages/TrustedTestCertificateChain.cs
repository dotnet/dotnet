// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Internal.NuGet.Testing.SignedPackages
{
    public class TrustedTestCertificateChain : IDisposable
    {
        public IList<TrustedTestCert<TestCertificate>> Certificates { get; set; } = new List<TrustedTestCert<TestCertificate>>();

        public TrustedTestCert<TestCertificate> Root => Certificates?.First();

        public TrustedTestCert<TestCertificate> Leaf => Certificates?.Last();

        public void Dispose()
        {
            if (Certificates != null)
            {
                foreach (var certificate in Certificates)
                {
                    certificate.Dispose();
                }
            }
        }
    }
}
