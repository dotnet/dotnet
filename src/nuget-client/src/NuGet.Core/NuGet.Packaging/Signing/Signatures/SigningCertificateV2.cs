// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NuGet.Common;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5035 (https://tools.ietf.org/html/rfc5035):

            SigningCertificateV2 ::= SEQUENCE {
                certs        SEQUENCE OF ESSCertIDv2,
                policies     SEQUENCE OF PolicyInformation OPTIONAL
            }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class SigningCertificateV2
    {
        public IReadOnlyList<EssCertIdV2> Certificates { get; }
        public IReadOnlyList<PolicyInformation> Policies { get; }

        private SigningCertificateV2(
            IReadOnlyList<EssCertIdV2> certificates,
            IReadOnlyList<PolicyInformation> policies)
        {
            Certificates = certificates;
            Policies = policies;
        }

        public static SigningCertificateV2 Create(X509Certificate2 certificate, HashAlgorithmName hashAlgorithmName)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var essCertIdV2 = EssCertIdV2.Create(certificate, hashAlgorithmName);

            return new SigningCertificateV2(new[] { essCertIdV2 }, policies: null);
        }

        public static SigningCertificateV2 Read(byte[] bytes)
        {
            var reader = new DerSequenceReader(bytes);

            return Read(reader);
        }

        internal static SigningCertificateV2 Read(DerSequenceReader reader)
        {
            var essCertIdV2Reader = reader.ReadSequence();
            var certificates = ReadCertificates(essCertIdV2Reader);
            IReadOnlyList<PolicyInformation> policies = null;

            if (reader.HasData)
            {
                var policiesReader = reader.ReadSequence();
                policies = ReadPolicies(policiesReader);

                if (reader.HasData)
                {
                    throw new SignatureException(Strings.InvalidAsn1);
                }
            }

            return new SigningCertificateV2(certificates, policies);
        }

        public byte[] Encode()
        {
            var entries = new List<byte[][]>(Certificates.Count);

            foreach (var essCertIdV2 in Certificates)
            {
                entries.Add(essCertIdV2.Encode());
            }

            return DerEncoder.ConstructSequence(DerEncoder.ConstructSegmentedSequence(entries));
        }

        private static IReadOnlyList<EssCertIdV2> ReadCertificates(DerSequenceReader reader)
        {
            var certificates = new List<EssCertIdV2>();

            while (reader.HasData)
            {
                var certificate = EssCertIdV2.Read(reader);

                certificates.Add(certificate);
            }

            return certificates.AsReadOnly();
        }

        private static IReadOnlyList<PolicyInformation> ReadPolicies(DerSequenceReader reader)
        {
            var policies = new List<PolicyInformation>();

            while (reader.HasData)
            {
                var policy = PolicyInformation.Read(reader);

                policies.Add(policy);
            }

            return policies.AsReadOnly();
        }
    }
}
