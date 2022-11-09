// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5126 (https://tools.ietf.org/html/rfc5126.html#section-5.11.1):

            CommitmentTypeIndication ::= SEQUENCE {
              commitmentTypeId CommitmentTypeIdentifier,
              commitmentTypeQualifier SEQUENCE SIZE (1..MAX) OF
                             CommitmentTypeQualifier OPTIONAL}

            CommitmentTypeIdentifier ::= OBJECT IDENTIFIER
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class CommitmentTypeIndication
    {
        public Oid CommitmentTypeId { get; }
        public IReadOnlyList<CommitmentTypeQualifier> Qualifiers { get; }

        private CommitmentTypeIndication(Oid commitmentTypeId, IReadOnlyList<CommitmentTypeQualifier> qualifiers)
        {
            CommitmentTypeId = commitmentTypeId;
            Qualifiers = qualifiers;
        }

        public static CommitmentTypeIndication Create(Oid commitmentTypeId)
        {
            if (commitmentTypeId == null)
            {
                throw new ArgumentNullException(nameof(commitmentTypeId));
            }

            return new CommitmentTypeIndication(commitmentTypeId, qualifiers: null);
        }

        public static CommitmentTypeIndication Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static CommitmentTypeIndication Read(DerSequenceReader reader)
        {
            var indicationReader = reader.ReadSequence();
            var commitmentTypeId = indicationReader.ReadOid();
            List<CommitmentTypeQualifier> qualifiers = null;

            if (indicationReader.HasData)
            {
                var qualifierReader = indicationReader.ReadSequence();

                qualifiers = new List<CommitmentTypeQualifier>();

                while (qualifierReader.HasData)
                {
                    var qualifier = CommitmentTypeQualifier.Read(qualifierReader);

                    qualifiers.Add(qualifier);
                }

                if (qualifiers.Count == 0)
                {
                    throw new SignatureException(Strings.CommitmentTypeIndicationAttributeInvalid);
                }
            }

            return new CommitmentTypeIndication(commitmentTypeId, qualifiers);
        }

        internal byte[] Encode()
        {
            return DerEncoder.ConstructSequence(DerEncoder.SegmentedEncodeOid(CommitmentTypeId));
        }
    }
}
