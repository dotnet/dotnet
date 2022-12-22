// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5280 (https://tools.ietf.org/html/rfc5280#appendix-A.2):

            PolicyQualifierInfo ::= SEQUENCE {
                policyQualifierId  PolicyQualifierId,
                qualifier          ANY DEFINED BY policyQualifierId }

            -- policyQualifierIds for Internet policy qualifiers

            id-qt          OBJECT IDENTIFIER ::=  { id-pkix 2 }
            id-qt-cps      OBJECT IDENTIFIER ::=  { id-qt 1 }
            id-qt-unotice  OBJECT IDENTIFIER ::=  { id-qt 2 }

            PolicyQualifierId ::= OBJECT IDENTIFIER ( id-qt-cps | id-qt-unotice )
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class PolicyQualifierInfo
    {
        public Oid PolicyQualifierId { get; }
        public byte[] Qualifier { get; }

        private PolicyQualifierInfo(Oid policyQualifierId, byte[] qualifier)
        {
            PolicyQualifierId = policyQualifierId;
            Qualifier = qualifier;
        }

        public static PolicyQualifierInfo Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static PolicyQualifierInfo Read(DerSequenceReader reader)
        {
            var policyQualifierReader = reader.ReadSequence();
            var policyQualifierId = policyQualifierReader.ReadOid();
            byte[] qualifier = null;

            if (policyQualifierReader.HasData)
            {
                qualifier = policyQualifierReader.ReadNextEncodedValue();

                if (policyQualifierReader.HasData)
                {
                    throw new SignatureException(Strings.InvalidAsn1);
                }
            }

            return new PolicyQualifierInfo(policyQualifierId, qualifier);
        }
    }
}
