// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;
using PolicyInformation = NuGet.Packaging.Signing.PolicyInformation;
using TestPolicyInformation = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.PolicyInformation;
using TestPolicyQualifierInfo = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.PolicyQualifierInfo;

namespace NuGet.Packaging.Test
{
    public class PolicyInformationTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => PolicyInformation.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithOnlyPolicyIdentifier_ReturnsPolicyInformation()
        {
            Oid policyId = new("1.2.3");
            TestPolicyInformation testPolicyInformation = new(policyId);
            byte[] bytes = Encode(testPolicyInformation);

            PolicyInformation policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(policyId.Value, policyInformation.PolicyIdentifier.Value);
            Assert.Null(policyInformation.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithAnyPolicyIdentifierAndNoPolicyQualifier_ReturnsPolicyInformation()
        {
            TestPolicyInformation testPolicyInformation = new(new Oid(Oids.AnyPolicy));
            byte[] bytes = Encode(testPolicyInformation);

            PolicyInformation policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(Oids.AnyPolicy, policyInformation.PolicyIdentifier.Value);
            Assert.Null(policyInformation.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithAnyPolicyIdentifierAndIdQtCpsPolicyQualifier_ReturnsPolicyInformation()
        {
            const string cpsUri = "http://test.test";

            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteCharacterString(UniversalTagNumber.IA5String, cpsUri);
            }

            ReadOnlyMemory<byte> qualifier = writer.Encode();

            TestPolicyQualifierInfo testPolicyQualifierInfo = new(new Oid(Oids.IdQtCps), qualifier);
            TestPolicyInformation testPolicyInformation = new(new Oid(Oids.AnyPolicy), new[] { testPolicyQualifierInfo });
            byte[] bytes = Encode(testPolicyInformation);

            PolicyInformation policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(Oids.AnyPolicy, policyInformation.PolicyIdentifier.Value);
            Assert.Equal(1, policyInformation.PolicyQualifiers.Count);
            Assert.Equal(qualifier.Span.ToArray(), policyInformation.PolicyQualifiers[0].Qualifier);
        }

        private static byte[] Encode(TestPolicyInformation testPolicyInformation)
        {
            AsnWriter writer = new(AsnEncodingRules.DER);

            testPolicyInformation.Encode(writer);

            return writer.Encode();
        }
    }
}
