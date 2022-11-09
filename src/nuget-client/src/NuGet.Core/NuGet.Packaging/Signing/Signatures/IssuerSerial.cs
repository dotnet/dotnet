// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 2634 (https://tools.ietf.org/html/rfc2634#section-5.4.1):

            IssuerSerial ::= SEQUENCE {
                issuer                   GeneralNames,
                serialNumber             CertificateSerialNumber
            }

        From RFC 2634 (https://tools.ietf.org/html/rfc3280#section-4.2.1.7):

            GeneralNames ::= SEQUENCE SIZE (1..MAX) OF GeneralName

        From RFC 3280 (https://tools.ietf.org/html/rfc3280#section-4.1):

            CertificateSerialNumber  ::=  INTEGER
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class IssuerSerial
    {
        public IReadOnlyList<GeneralName> GeneralNames { get; }
        public byte[] SerialNumber { get; }

        private IssuerSerial(IReadOnlyList<GeneralName> generalNames, byte[] serialNumber)
        {
            GeneralNames = generalNames;
            SerialNumber = serialNumber;
        }

        public static IssuerSerial Create(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var generalNames = new[] { GeneralName.Create(certificate.IssuerName) };
            var serialNumber = certificate.GetSerialNumber();

            // Convert from little endian to big endian.
            Array.Reverse(serialNumber);

            return new IssuerSerial(generalNames, serialNumber);
        }

        public static IssuerSerial Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static IssuerSerial Read(DerSequenceReader reader)
        {
            var sequenceReader = reader.ReadSequence();
            var generalNames = ReadGeneralNames(sequenceReader);
            var serialNumber = sequenceReader.ReadIntegerBytes();

            return new IssuerSerial(generalNames, serialNumber);
        }

        internal byte[][] Encode()
        {
            // Per RFC 5280 section 4.1.2.2 (https://tools.ietf.org/html/rfc5280#section-4.1.2.2)
            // serial number must be an unsigned integer.
            return DerEncoder.ConstructSegmentedSequence(
                DerEncoder.ConstructSegmentedSequence(GeneralNames.First().Encode()),
                DerEncoder.SegmentedEncodeUnsignedInteger(SerialNumber));
        }

        private static IReadOnlyList<GeneralName> ReadGeneralNames(DerSequenceReader reader)
        {
            var sequenceReader = reader.ReadSequence();
            var generalNames = new List<GeneralName>(capacity: 1);

            var generalName = GeneralName.Read(sequenceReader);

            if (generalName != null)
            {
                generalNames.Add(generalName);
            }

            if (sequenceReader.HasData)
            {
                throw new SignatureException(Strings.InvalidAsn1);
            }

            return generalNames.AsReadOnly();
        }
    }
}
