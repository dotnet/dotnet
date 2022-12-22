// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Cryptography;
using NuGet.Packaging.Signing.DerEncoding;

namespace NuGet.Packaging.Signing
{
    /*
        From RFC 5280 (https://tools.ietf.org/html/rfc5280#appendix-A.2):

            PolicyInformation ::= SEQUENCE {
                policyIdentifier   CertPolicyId,
                policyQualifiers   SEQUENCE SIZE (1..MAX) OF
                                        PolicyQualifierInfo OPTIONAL }

            CertPolicyId ::= OBJECT IDENTIFIER
    */
    /// <remarks>This is public only to facilitate testing.</remarks>
    public sealed class PolicyInformation
    {
        public Oid PolicyIdentifier { get; }
        public IReadOnlyList<PolicyQualifierInfo> PolicyQualifiers { get; }

        private PolicyInformation(Oid policyIdentifier, IReadOnlyList<PolicyQualifierInfo> policyQualifiers)
        {
            PolicyIdentifier = policyIdentifier;
            PolicyQualifiers = policyQualifiers;
        }

        public static PolicyInformation Read(byte[] bytes)
        {
            var reader = DerSequenceReader.CreateForPayload(bytes);

            return Read(reader);
        }

        internal static PolicyInformation Read(DerSequenceReader reader)
        {
            var policyInfoReader = reader.ReadSequence();
            var policyIdentifier = policyInfoReader.ReadOid();
            var isAnyPolicy = policyIdentifier.Value == Oids.AnyPolicy;
            IReadOnlyList<PolicyQualifierInfo> policyQualifiers = null;

            if (policyInfoReader.HasData)
            {
                policyQualifiers = ReadPolicyQualifiers(policyInfoReader, isAnyPolicy);
            }

            return new PolicyInformation(policyIdentifier, policyQualifiers);
        }

        private static IReadOnlyList<PolicyQualifierInfo> ReadPolicyQualifiers(
            DerSequenceReader reader,
            bool isAnyPolicy)
        {
            var policyQualifiersReader = reader.ReadSequence();
            var policyQualifiers = new List<PolicyQualifierInfo>();

            while (policyQualifiersReader.HasData)
            {
                var policyQualifier = PolicyQualifierInfo.Read(policyQualifiersReader);

                if (isAnyPolicy)
                {
                    if (policyQualifier.PolicyQualifierId.Value != Oids.IdQtCps &&
                        policyQualifier.PolicyQualifierId.Value != Oids.IdQtUnotice)
                    {
                        throw new SignatureException(Strings.InvalidAsn1);
                    }
                }

                policyQualifiers.Add(policyQualifier);
            }

            if (policyQualifiers.Count == 0)
            {
                throw new SignatureException(Strings.InvalidAsn1);
            }

            return policyQualifiers.AsReadOnly();
        }
    }
}
