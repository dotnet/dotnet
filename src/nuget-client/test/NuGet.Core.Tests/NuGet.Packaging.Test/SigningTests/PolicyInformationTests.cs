// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using BcPolicyInformation = Org.BouncyCastle.Asn1.X509.PolicyInformation;
using BcPolicyQualifierInfo = Org.BouncyCastle.Asn1.X509.PolicyQualifierInfo;

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
            var policyId = "1.2.3";
            var bcPolicyInformation = new BcPolicyInformation(new DerObjectIdentifier(policyId));
            var bytes = bcPolicyInformation.GetDerEncoded();

            var policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(policyId, policyInformation.PolicyIdentifier.Value);
            Assert.Null(policyInformation.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithAnyPolicyIdentifierAndNoPolicyQualifier_ReturnsPolicyInformation()
        {
            var bcPolicyInformation = new BcPolicyInformation(new DerObjectIdentifier(Oids.AnyPolicy));
            var bytes = bcPolicyInformation.GetDerEncoded();

            var policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(Oids.AnyPolicy, policyInformation.PolicyIdentifier.Value);
            Assert.Null(policyInformation.PolicyQualifiers);
        }

        [Fact]
        public void Read_WithAnyPolicyIdentifierAndIdQtCpsPolicyQualifier_ReturnsPolicyInformation()
        {
            var cpsUri = new DerIA5String("http://test.test");
            var bcPolicyInformation = new BcPolicyInformation(
                new DerObjectIdentifier(Oids.AnyPolicy),
                new DerSequence(new BcPolicyQualifierInfo(new DerObjectIdentifier(Oids.IdQtCps), cpsUri)));
            var bytes = bcPolicyInformation.GetDerEncoded();

            var policyInformation = PolicyInformation.Read(bytes);

            Assert.Equal(Oids.AnyPolicy, policyInformation.PolicyIdentifier.Value);
            Assert.Equal(1, policyInformation.PolicyQualifiers.Count);
            Assert.Equal(cpsUri.GetDerEncoded(), policyInformation.PolicyQualifiers[0].Qualifier);
        }
    }
}
