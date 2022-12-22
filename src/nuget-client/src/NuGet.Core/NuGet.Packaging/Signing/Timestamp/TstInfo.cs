// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 3161 (https://tools.ietf.org/html/rfc3161#section-2.4.2):

            TSTInfo ::= SEQUENCE  {
               version                      INTEGER  { v1(1) },
               policy                       TSAPolicyId,
               messageImprint               MessageImprint,
                 -- MUST have the same value as the similar field in
                 -- TimeStampReq
               serialNumber                 INTEGER,
                -- Time-Stamping users MUST be ready to accommodate integers
                -- up to 160 bits.
               genTime                      GeneralizedTime,
               accuracy                     Accuracy                 OPTIONAL,
               ordering                     BOOLEAN             DEFAULT FALSE,
               nonce                        INTEGER                  OPTIONAL,
                 -- MUST be present if the similar field was present
                 -- in TimeStampReq.  In that case it MUST have the same value.
               tsa                          [0] GeneralName          OPTIONAL,
               extensions                   [1] IMPLICIT Extensions   OPTIONAL  }

            TSAPolicyId ::= OBJECT IDENTIFIER
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class TstInfo
    {
        public int Version { get; }
        public Oid Policy { get; }
        public MessageImprint MessageImprint { get; }
        public byte[] SerialNumber { get; }
        public DateTimeOffset GenTime { get; }
        public Accuracy Accuracy { get; }
        public bool Ordering { get; }
        public byte[] Nonce { get; } // big endian!
        public byte[] Tsa { get; }
        public X509ExtensionCollection Extensions { get; }

        private TstInfo(
            int version,
            Oid policy,
            MessageImprint messageImprint,
            byte[] serialNumber,
            DateTimeOffset genTime,
            Accuracy accuracy,
            bool ordering,
            byte[] nonce,
            byte[] tsa,
            X509ExtensionCollection extensions)
        {
            Version = version;
            Policy = policy;
            MessageImprint = messageImprint;
            SerialNumber = serialNumber;
            GenTime = genTime;
            Accuracy = accuracy;
            Ordering = ordering;
            Nonce = nonce;
            Tsa = tsa;
            Extensions = extensions;
        }

        public static TstInfo Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static TstInfo Read(DerSequenceReader reader)
        {
            var tstInfoReader = reader.ReadSequence();
            var version = tstInfoReader.ReadInteger();
            var policy = tstInfoReader.ReadOid();
            var messageImprint = MessageImprint.Read(tstInfoReader);
            var serialNumber = tstInfoReader.ReadIntegerBytes();

            if (serialNumber == null || serialNumber.Length == 0)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            var genTime = tstInfoReader.ReadGeneralizedTime();

            Accuracy accuracy = null;

            if (tstInfoReader.HasTag(DerSequenceReader.ConstructedSequence))
            {
                accuracy = Accuracy.Read(tstInfoReader);
            }

            var ordering = false;

            if (tstInfoReader.HasTag(DerSequenceReader.DerTag.Boolean))
            {
                ordering = tstInfoReader.ReadBoolean();
            }

            byte[] nonce = null;

            if (tstInfoReader.HasTag(DerSequenceReader.DerTag.Integer))
            {
                nonce = tstInfoReader.ReadIntegerBytes();
            }

            byte[] tsa = null;

            if (tstInfoReader.HasData && tstInfoReader.HasTag(DerSequenceReader.ContextSpecificConstructedTag0))
            {
                tsa = tstInfoReader.ReadValue((DerSequenceReader.DerTag)DerSequenceReader.ContextSpecificConstructedTag0);
            }

            X509ExtensionCollection extensions = null;

            if (tstInfoReader.HasData && tstInfoReader.HasTag(DerSequenceReader.ContextSpecificConstructedTag1))
            {
                extensions = new X509ExtensionCollection();

                var rawExtensions = Signing.Extensions.Read(tstInfoReader);

                foreach (var rawExtension in rawExtensions.ExtensionsList)
                {
                    extensions.Add(
                        new X509Extension(
                            rawExtension.Id,
                            rawExtension.Value,
                            rawExtension.Critical));
                }

                if (extensions.Count == 0)
                {
                    throw new CryptographicException(Strings.InvalidAsn1);
                }
            }

            if (tstInfoReader.HasData)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            return new TstInfo(
                version,
                policy,
                messageImprint,
                serialNumber,
                genTime.ToUniversalTime(),
                accuracy,
                ordering,
                nonce,
                tsa,
                extensions);
        }
    }
}
