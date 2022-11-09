// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.Utility.Signing
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
