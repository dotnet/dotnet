// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5035 (https://tools.ietf.org/html/rfc5035):

            ESSCertIDv2 ::= SEQUENCE {
                hashAlgorithm            AlgorithmIdentifier
                       DEFAULT {algorithm id-sha256},
                certHash                 Hash,
                issuerSerial             IssuerSerial OPTIONAL
            }

            Hash ::= OCTET STRING

            IssuerSerial ::= SEQUENCE {
                issuer                   GeneralNames,
                serialNumber             CertificateSerialNumber
           }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class EssCertIdV2
    {
        public AlgorithmIdentifier HashAlgorithm { get; }
        public byte[] CertificateHash { get; }
        public IssuerSerial IssuerSerial { get; }

        private EssCertIdV2(AlgorithmIdentifier hashAlgorithm, byte[] hash, IssuerSerial issuerSerial)
        {
            HashAlgorithm = hashAlgorithm;
            CertificateHash = hash;
            IssuerSerial = issuerSerial;
        }

        public static EssCertIdV2 Create(X509Certificate2 certificate, Common.HashAlgorithmName hashAlgorithmName)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var algorithm = new AlgorithmIdentifier(hashAlgorithmName.ConvertToOid());
            var hash = CertificateUtility.GetHash(certificate, hashAlgorithmName);
            var issuerSerial = IssuerSerial.Create(certificate);

            return new EssCertIdV2(algorithm, hash, issuerSerial);
        }

        public static EssCertIdV2 Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static EssCertIdV2 Read(DerSequenceReader reader)
        {
            var sequenceReader = reader.ReadSequence();

            AlgorithmIdentifier algorithm;

            if (sequenceReader.HasTag(DerSequenceReader.ConstructedSequence))
            {
                algorithm = AlgorithmIdentifier.Read(sequenceReader);
            }
            else
            {
                algorithm = new AlgorithmIdentifier(new Oid(Oids.Sha256));
            }

            var hash = sequenceReader.ReadOctetString();
            IssuerSerial issuerSerial = null;

            if (sequenceReader.HasData)
            {
                issuerSerial = IssuerSerial.Read(sequenceReader);

                if (sequenceReader.HasData)
                {
                    throw new SignatureException(Strings.SigningCertificateV2Invalid);
                }
            }

            return new EssCertIdV2(algorithm, hash, issuerSerial);
        }

        internal byte[][] Encode()
        {
            return DerEncoder.ConstructSegmentedSequence(
                HashAlgorithm.Encode(),
                DerEncoder.SegmentedEncodeOctetString(CertificateHash),
                IssuerSerial.Encode());
        }
    }
}
