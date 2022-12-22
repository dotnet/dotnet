// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace NuGet.Packaging.Signing
{
    internal interface ICms : IDisposable
    {
        byte[] GetPrimarySignatureSignatureValue();

        byte[] GetRepositoryCountersignatureSignatureValue();

        void AddCertificates(IEnumerable<X509Certificate2> certificates);

        void AddCountersignature(CmsSigner cmsSigner, CngKey privateKey);

        void AddTimestampToRepositoryCountersignature(SignedCms timestamp);

        void AddTimestamp(SignedCms timestamp);

        byte[] Encode();
    }
}
