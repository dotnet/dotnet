// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Org.BouncyCastle.Asn1;
using Xunit;
using BcPolicyQualifierInfo = Org.BouncyCastle.Asn1.X509.PolicyQualifierInfo;

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
            var policyQualifierId = "1.2.3";
            var bcPolicyQualifierInfo = new BcPolicyQualifierInfo(
                new DerObjectIdentifier(policyQualifierId), DerNull.Instance);
            var bytes = bcPolicyQualifierInfo.GetDerEncoded();

            var policyQualifierInfo = PolicyQualifierInfo.Read(bytes);

            Assert.Equal(policyQualifierId, policyQualifierInfo.PolicyQualifierId.Value);
            Assert.Equal(DerNull.Instance.GetDerEncoded(), policyQualifierInfo.Qualifier);
        }

        [Fact]
        public void Read_WithQualifier_ReturnsPolicyQualifierInfo()
        {
            var cpsUri = new DerIA5String("http://test.test");
            var bcPolicyQualifierInfo = new BcPolicyQualifierInfo(
                new DerObjectIdentifier(Oids.IdQtCps), cpsUri);
            var bytes = bcPolicyQualifierInfo.GetDerEncoded();

            var policyQualifierInfo = PolicyQualifierInfo.Read(bytes);

            Assert.Equal(Oids.IdQtCps, policyQualifierInfo.PolicyQualifierId.Value);
            Assert.Equal(cpsUri.GetDerEncoded(), policyQualifierInfo.Qualifier);
        }
    }
}
