// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 2634 (https://tools.ietf.org/html/rfc2634#section-5.4.1):

            ESSCertID ::=  SEQUENCE {
                certHash                 Hash,
                issuerSerial             IssuerSerial OPTIONAL
            }

            Hash ::= OCTET STRING -- SHA1 hash of entire certificate

            IssuerSerial ::= SEQUENCE {
                issuer                   GeneralNames,
                serialNumber             CertificateSerialNumber
            }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class EssCertId
    {
        public byte[] CertificateHash { get; }
        public IssuerSerial IssuerSerial { get; }

        private EssCertId(byte[] hash, IssuerSerial issuerSerial)
        {
            CertificateHash = hash;
            IssuerSerial = issuerSerial;
        }

        public static EssCertId Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static EssCertId Read(DerSequenceReader reader)
        {
            var sequenceReader = reader.ReadSequence();
            var hash = sequenceReader.ReadOctetString();
            IssuerSerial issuerSerial = null;

            if (sequenceReader.HasData)
            {
                issuerSerial = IssuerSerial.Read(sequenceReader);

                if (sequenceReader.HasData)
                {
                    throw new SignatureException(Strings.SigningCertificateInvalid);
                }
            }

            return new EssCertId(hash, issuerSerial);
        }
    }
}
