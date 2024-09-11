// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_DESKTOP
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
#endif

namespace NuGet.Packaging.Signing
{
#if IS_SIGNING_SUPPORTED && IS_DESKTOP
    internal sealed class NativeCmsWrapper : ICms
    {
        private readonly NativeCms _nativeCms;

        public NativeCmsWrapper(NativeCms nativeCms)
        {
            _nativeCms = nativeCms;
        }

        public byte[] GetPrimarySignatureSignatureValue()
        {
            return _nativeCms.GetPrimarySignatureSignatureValue();
        }

        public byte[] GetRepositoryCountersignatureSignatureValue()
        {
            return _nativeCms.GetRepositoryCountersignatureSignatureValue();
        }

        public void AddCertificates(IEnumerable<X509Certificate2> certificates)
        {
            _nativeCms.AddCertificates(certificates);
        }

        public void AddCountersignature(CmsSigner cmsSigner, CngKey privateKey)
        {
            _nativeCms.AddCountersignature(cmsSigner, privateKey);
        }

        public void AddTimestampToRepositoryCountersignature(SignedCms timestamp)
        {
            _nativeCms.AddTimestampToRepositoryCountersignature(timestamp);
        }

        public void AddTimestamp(SignedCms timestamp)
        {
            _nativeCms.AddTimestamp(timestamp);
        }

        public byte[] Encode()
        {
            return _nativeCms.Encode();
        }

        public void Dispose()
        {
            _nativeCms.Dispose();
        }
    }
#endif
}

