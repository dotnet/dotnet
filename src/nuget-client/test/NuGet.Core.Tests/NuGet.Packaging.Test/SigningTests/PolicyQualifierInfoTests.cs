// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Formats.Asn1;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Xunit;
using PolicyQualifierInfo = NuGet.Packaging.Signing.PolicyQualifierInfo;
using TestPolicyQualifierInfo = Microsoft.Internal.NuGet.Testing.SignedPackages.Asn1.PolicyQualifierInfo;

namespace NuGet.Packaging.Test
{
    public class PolicyQualifierInfoTests
    {
        [Fact]
        public void Read_WithInvalidAsn1_Throws()
        {
            Assert.Throws<CryptographicException>(
                () => PolicyQualifierInfo.Read(new byte[] { 0x30, 0x07 }));
        }

        [Fact]
        public void Read_WithOnlyPolicyQualifierId_ReturnsPolicyQualifierInfo()
        {
            Oid policyQualifierId = new("1.2.3");
            AsnWriter writer = new(AsnEncodingRules.DER);

            using (writer.PushSequence())
            {
                writer.WriteObjectIdentifier(policyQualifierId.Value!);
            }

            byte[] bytes = writer.Encode();

            PolicyQualifierInfo policyQualifierInfo = PolicyQualifierInfo.Read(bytes);

            Assert.Equal(policyQualifierId.Value, policyQualifierInfo.PolicyQualifierId.Value);
            Assert.Null(policyQualifierInfo.Qualifier);
        }

        [Fact]
        public void Read_WithQualifier_ReturnsPolicyQualifierInfo()
        {
            const string cpsUri = "http://test.test";

            AsnWriter writer = new(AsnEncodingRules.DER);

            writer.WriteCharacterString(UniversalTagNumber.IA5String, cpsUri);

            byte[] qualifier = writer.Encode();

            TestPolicyQualifierInfo testPolicyQualifierInfo = new(new Oid(Oids.IdQtCps), qualifier);

            writer.Reset();
            testPolicyQualifierInfo.Encode(writer);

            byte[] bytes = writer.Encode();

            PolicyQualifierInfo policyQualifierInfo = PolicyQualifierInfo.Read(bytes);

            Assert.Equal(Oids.IdQtCps, policyQualifierInfo.PolicyQualifierId.Value);
            Assert.Equal(qualifier, policyQualifierInfo.Qualifier);
        }
    }
}
