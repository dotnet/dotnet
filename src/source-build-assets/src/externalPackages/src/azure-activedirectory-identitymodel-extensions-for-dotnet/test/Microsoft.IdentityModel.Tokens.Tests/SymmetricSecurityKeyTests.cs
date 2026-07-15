// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

using ALG = Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using EE = Microsoft.IdentityModel.TestUtils.ExpectedException;
using KEY = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class SymmetricSecurityKeyTests
    {

        [Theory, MemberData(nameof(ConstructorDataSet), DisableDiscoveryEnumeration = true)]
        public void Constructor(byte[] key, EE ee)
        {
            try
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(key);
                ee.ProcessNoException();
            }
            catch (Exception exception)
            {
                ee.ProcessException(exception);
            }
        }

        public static TheoryData<byte[], EE> ConstructorDataSet
        {
            get
            {
                var dataset = new TheoryData<byte[], EE>();
                dataset.Add(KEY.DefaultSymmetricKeyBytes_256, EE.NoExceptionExpected);
                dataset.Add(null, EE.ArgumentNullException());
                dataset.Add(new byte[0], EE.ArgumentException());
                return dataset;
            }
        }

        [Fact]
        public void CanComputeJwkThumbprint()
        {
            Assert.True(KEY.DefaultSymmetricSecurityKey_256.CanComputeJwkThumbprint(), "Couldn't compute JWK thumbprint on a SymmetricSecurityKey.");
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
