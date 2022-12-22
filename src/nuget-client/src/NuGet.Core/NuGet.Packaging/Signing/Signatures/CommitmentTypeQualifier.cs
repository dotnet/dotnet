// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5126 (https://tools.ietf.org/html/rfc5126.html#section-5.11.1):

            CommitmentTypeQualifier ::= SEQUENCE {
               commitmentTypeIdentifier   CommitmentTypeIdentifier,
               qualifier                  ANY DEFINED BY commitmentTypeIdentifier }

            CommitmentTypeIdentifier ::= OBJECT IDENTIFIER
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class CommitmentTypeQualifier
    {
        public Oid CommitmentTypeIdentifier { get; }
        public byte[] Qualifier { get; }

        private CommitmentTypeQualifier(Oid commitmentTypeIdentifier, byte[] qualifier)
        {
            CommitmentTypeIdentifier = commitmentTypeIdentifier;
            Qualifier = qualifier;
        }

        public static CommitmentTypeQualifier Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static CommitmentTypeQualifier Read(DerSequenceReader reader)
        {
            var commitmentTypeQualifierReader = reader.ReadSequence();
            var commitmentTypeIdentifier = commitmentTypeQualifierReader.ReadOid();
            byte[] qualifier = null;

            if (commitmentTypeQualifierReader.HasData)
            {
                qualifier = commitmentTypeQualifierReader.ReadNextEncodedValue();

                if (commitmentTypeQualifierReader.HasData)
                {
                    throw new SignatureException(Strings.InvalidAsn1);
                }
            }

            return new CommitmentTypeQualifier(commitmentTypeIdentifier, qualifier);
        }

        internal byte[] Encode()
        {
            return DerEncoder.ConstructSequence(DerEncoder.SegmentedEncodeOid(CommitmentTypeIdentifier));
        }
    }
}
