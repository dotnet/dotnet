// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5280 (https://tools.ietf.org/html/rfc5280#section-4.1):

            Extension  ::=  SEQUENCE  {
                extnID      OBJECT IDENTIFIER,
                critical    BOOLEAN DEFAULT FALSE,
                extnValue   OCTET STRING
                            -- contains the DER encoding of an ASN.1 value
                            -- corresponding to the extension type identified
                            -- by extnID
                }
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class Extension
    {
        public Oid Id { get; }
        public bool Critical { get; }
        public byte[] Value { get; }

        private Extension(
            Oid id,
            bool critical,
            byte[] value)
        {
            Id = id;
            Critical = critical;
            Value = value;
        }

        internal static Extension Read(DerSequenceReader reader)
        {
            var extensionReader = reader.ReadSequence();
            var oid = extensionReader.ReadOid();
            var critical = false;

            if (extensionReader.HasTag(DerSequenceReader.DerTag.Boolean))
            {
                critical = extensionReader.ReadBoolean();
            }

            var value = extensionReader.ReadOctetString();

            if (extensionReader.HasData)
            {
                throw new CryptographicException(Strings.InvalidAsn1);
            }

            return new Extension(oid, critical, value);
        }
    }
}
