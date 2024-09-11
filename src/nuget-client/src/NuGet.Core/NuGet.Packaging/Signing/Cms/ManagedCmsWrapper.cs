// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED && IS_CORECLR
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
#endif

namespace NuGet.Packaging.Signing
{
#if IS_SIGNING_SUPPORTED && IS_CORECLR
    internal sealed class ManagedCmsWrapper : ICms
    {
        private readonly SignedCms _signedCms;

        public ManagedCmsWrapper(SignedCms signedCms)
        {
            _signedCms = signedCms;
        }

        public byte[] GetPrimarySignatureSignatureValue()
        {
            if (_signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(NuGetLogCode.NU3009, Strings.Error_NotOnePrimarySignature);
            }

            return _signedCms.SignerInfos[0].GetSignature();
        }

        public byte[] GetRepositoryCountersignatureSignatureValue()
        {
            if (_signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(NuGetLogCode.NU3009, Strings.Error_NotOnePrimarySignature);
            }
            else if (_signedCms.SignerInfos[0].CounterSignerInfos.Count == 0)
            {
                return null;
            }

            return _signedCms.SignerInfos[0].CounterSignerInfos[0].GetSignature();
        }

        public void AddCertificates(IEnumerable<X509Certificate2> certificates)
        {
            foreach (var cert in certificates)
            {
                _signedCms.AddCertificate(cert);
            }
        }

        public void AddCountersignature(CmsSigner cmsSigner, CngKey privateKey)
        {
            if (_signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(NuGetLogCode.NU3009, Strings.Error_NotOnePrimarySignature);
            }

            _signedCms.SignerInfos[0].ComputeCounterSignature(cmsSigner);
        }

        public void AddTimestampToRepositoryCountersignature(SignedCms timestamp)
        {
            var bytes = timestamp.Encode();

            var unsignedAttribute = new AsnEncodedData(Oids.SignatureTimeStampTokenAttribute, bytes);

            if (_signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(NuGetLogCode.NU3009, Strings.Error_NotOnePrimarySignature);
            }
            else if (_signedCms.SignerInfos[0].CounterSignerInfos.Count != 1)
            {
                throw new SignatureException(Strings.Error_NotOneRepositoryCounterSignature);
            }

            _signedCms.SignerInfos[0].CounterSignerInfos[0].AddUnsignedAttribute(unsignedAttribute);
        }

        public void AddTimestamp(SignedCms timestamp)
        {
            var bytes = timestamp.Encode();

            var unsignedAttribute = new AsnEncodedData(Oids.SignatureTimeStampTokenAttribute, bytes);

            if (_signedCms.SignerInfos.Count != 1)
            {
                throw new SignatureException(NuGetLogCode.NU3009, Strings.Error_NotOnePrimarySignature);
            }

            _signedCms.SignerInfos[0].AddUnsignedAttribute(unsignedAttribute);
        }

        public byte[] Encode()
        {
            return _signedCms.Encode();
        }

        public void Dispose()
        {
        }
    }
#endif
}

