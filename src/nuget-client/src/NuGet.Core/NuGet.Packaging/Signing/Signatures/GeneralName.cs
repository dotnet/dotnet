// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 3280 (https://tools.ietf.org/html/rfc3280#section-4.2.1.7):

            GeneralName ::= CHOICE {
                    otherName                       [0]     OtherName,
                    rfc822Name                      [1]     IA5String,
                    dNSName                         [2]     IA5String,
                    x400Address                     [3]     ORAddress,
                    directoryName                   [4]     Name,
                    ediPartyName                    [5]     EDIPartyName,
                    uniformResourceIdentifier       [6]     IA5String,
                    iPAddress                       [7]     OCTET STRING,
                    registeredID                    [8]     OBJECT IDENTIFIER }

                OtherName ::= SEQUENCE {
                    type-id    OBJECT IDENTIFIER,
                    value      [0] EXPLICIT ANY DEFINED BY type-id }

                EDIPartyName ::= SEQUENCE {
                    nameAssigner            [0]     DirectoryString OPTIONAL,
                    partyName               [1]     DirectoryString }


        From RFC 2459 (https://tools.ietf.org/html/rfc2459.html#section-4.1.2.4):

            Name ::= CHOICE {
                RDNSequence }

            RDNSequence ::= SEQUENCE OF RelativeDistinguishedName

            RelativeDistinguishedName ::=
                SET OF AttributeTypeAndValue

            AttributeTypeAndValue ::= SEQUENCE {
                type     AttributeType,
                value    AttributeValue }

            AttributeType ::= OBJECT IDENTIFIER

            AttributeValue ::= ANY DEFINED BY AttributeType
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class GeneralName
    {
        public X500DistinguishedName DirectoryName { get; }

        private GeneralName(X500DistinguishedName directoryName)
        {
            DirectoryName = directoryName;
        }

        public static GeneralName Create(X500DistinguishedName distinguishedName)
        {
            if (distinguishedName == null)
            {
                throw new ArgumentNullException(nameof(distinguishedName));
            }

            return new GeneralName(distinguishedName);
        }

        public static GeneralName Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static GeneralName Read(DerSequenceReader reader)
        {
            var tag = reader.PeekTag();

            // Per RFC 2634 section 5.4.1 (https://tools.ietf.org/html/rfc2634#section-5.4.1)
            // only the directory name choice (#4) is allowed.
            if (tag == DerSequenceReader.ContextSpecificConstructedTag4)
            {
                var value = reader.ReadValue((DerSequenceReader.DerTag)DerSequenceReader.ContextSpecificConstructedTag4);

                if (reader.HasData)
                {
                    throw new SignatureException(Strings.InvalidAsn1);
                }

                var directoryName = new X500DistinguishedName(value);

                return new GeneralName(directoryName);
            }

            while (reader.HasData)
            {
                reader.ValidateAndSkipDerValue();
            }

            throw new SignatureException(Strings.UnsupportedAsn1);
        }

        internal byte[][] Encode()
        {
            var bytes = DirectoryName.RawData;
            var reader = DerSequenceReader.CreateForPayload(bytes);

            var tag = reader.PeekTag();
            var value = reader.ReadValue((DerSequenceReader.DerTag)tag);
            var lengthByteCount = reader.ContentLength - 1 - value.Length;
            var length = new byte[lengthByteCount];

            Array.Copy(bytes, sourceIndex: 1, destinationArray: length, destinationIndex: 0, length: length.Length);

            const int contextId = 4;

            return DerEncoder.ConstructSegmentedContextSpecificValue(
                contextId,
                new byte[][]
                {
                    new byte[1] { tag },
                    length,
                    value
                });
        }
    }
}
