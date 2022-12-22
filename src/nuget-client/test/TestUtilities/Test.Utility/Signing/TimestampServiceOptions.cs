// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using BcAccuracy = Org.BouncyCastle.Asn1.Tsp.Accuracy;

namespace Test.Utility.Signing
{
    public sealed class TimestampServiceOptions
    {
        public BcAccuracy Accuracy { get; set; }
        public Oid Policy { get; set; }
        public bool ReturnFailure { get; set; }
        public bool ReturnSigningCertificate { get; set; }
        public Oid SignatureHashAlgorithm { get; set; }
        public DateTimeOffset? IssuedCertificateNotBefore { get; set; }
        public DateTimeOffset? IssuedCertificateNotAfter { get; set; }
        public DateTimeOffset? GeneralizedTime { get; set; }
        public SigningCertificateUsage SigningCertificateUsage { get; set; }
        public byte[] SigningCertificateV1Hash { get; set; }

        public TimestampServiceOptions()
        {
            Accuracy = new BcAccuracy(seconds: new DerInteger(1), micros: null, millis: null);
            Policy = new Oid("2.999");
            ReturnSigningCertificate = true;
            SignatureHashAlgorithm = new Oid(Oids.Sha256);
            SigningCertificateUsage = SigningCertificateUsage.V2;
        }
    }
}
