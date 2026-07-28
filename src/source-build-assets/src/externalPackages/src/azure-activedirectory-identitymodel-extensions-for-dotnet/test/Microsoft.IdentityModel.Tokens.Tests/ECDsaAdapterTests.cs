// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

using KEY = Microsoft.IdentityModel.TestUtils.KeyingMaterial;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class ECDsaAdapterTests
    {
        [Theory, MemberData(nameof(CreateECDsaFromJsonWebKeyTheoryData), DisableDiscoveryEnumeration = true)]
        public void CreateECDsa(JsonWebKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateECDsa", theoryData);
            try
            {
                var jsonWebKey = new JsonWebKey
                {
                    Crv = theoryData.Crv,
                    X = theoryData.X,
                    Y = theoryData.Y,
                    D = theoryData.D,
                };
                var ecdsaAdapter = ECDsaAdapter.Instance;
                ecdsaAdapter.CreateECDsa(jsonWebKey, theoryData.UsePrivateKey);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebKeyTheoryData> CreateECDsaFromJsonWebKeyTheoryData
        {
            get => new TheoryData<JsonWebKeyTheoryData>
            {
                new JsonWebKeyTheoryData {
                    First = true,
                    UsePrivateKey = false,
                    Crv = null,
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "nullCrv",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-255",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "unknownCrv",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = null,
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "nullXparam",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = null,
                    TestId = "nullYparam",
                    ExpectedException = ExpectedException.ArgumentNullException(),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = null,
                    TestId = "nullDparam",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", typeof(ArgumentNullException)),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X + "_dummy_data",
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "xLongerThanY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X.Remove(KEY.JsonWebKeyP256.X.Length - 1),
                    Y = KEY.JsonWebKeyP256.Y,
                    TestId = "xShorterThanY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y + "_dummy_data",
                    TestId = "YLongerThanX",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = KEY.JsonWebKeyP256.D + "_dummy_data",
                    TestId = "dLongerThanXandY",
                    // ignoreInnerException - throws different inner exceptions on different ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP384.X,
                    Y = KEY.JsonWebKeyP384.Y,
                    D = KEY.JsonWebKeyP384.D,
                    TestId = "paramsMoreBytesThanCurve",
                    // ignoreInnerException - throws different inner exceptions on different platforms and ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = "",
                    Y = "",
                    D = "",
                    TestId = "emptyParams",
                    // ignoreInnerException - throws different inner exceptions on different platforms and ECDsaAdapter flows
                    ExpectedException = ExpectedException.CryptographicException("IDX10689:", ignoreInnerException: true),
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = false,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = null,
                    TestId = "successNullDnoPrivateKey",
                },
                new JsonWebKeyTheoryData {
                    UsePrivateKey = true,
                    Crv = "P-256",
                    X = KEY.JsonWebKeyP256.X,
                    Y = KEY.JsonWebKeyP256.Y,
                    D = KEY.JsonWebKeyP256.D,
                    TestId = "successfulCall",
                },
            };
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
