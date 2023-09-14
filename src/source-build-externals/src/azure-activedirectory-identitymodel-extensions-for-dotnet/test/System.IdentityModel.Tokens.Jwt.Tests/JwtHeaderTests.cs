// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtHeaderTests
    {
        [Fact]
        public void Constructors_Default()
        {
            var jwtHeader = new JwtHeader();

            Assert.True(jwtHeader.Typ == null, "jwtHeader.Typ != null");
            Assert.True(jwtHeader.Alg == null, "jwtHeader.Alg != null");
            Assert.True(jwtHeader.SigningCredentials == null, "jwtHeader.SigningCredentials != null");
            Assert.True(jwtHeader.Kid == null, "jwtHeader.Kid == null");
            Assert.True(jwtHeader.Comparer.GetType() == StringComparer.Ordinal.GetType(), "jwtHeader.Comparer.GetType() != StringComparer.Ordinal.GetType()");
        }

        [Fact]
        public void Constructors_Null_SigningCredentials()
        {
            JwtHeader jwtHeader = new JwtHeader((SigningCredentials)null);
            Assert.True(jwtHeader.Typ == JwtConstants.HeaderType, "jwtHeader.ContainsValue( JwtConstants.HeaderType )");
            Assert.True(jwtHeader.Alg == SecurityAlgorithms.None, "jwtHeader.SignatureAlgorithm == null");
            Assert.True(jwtHeader.SigningCredentials == null, "jwtHeader.SigningCredentials != null");
            Assert.True(jwtHeader.Kid == null, "jwtHeader.Kid == null");
            Assert.True(jwtHeader.Comparer.GetType() == StringComparer.Ordinal.GetType(), "jwtHeader.Comparer.GetType() != StringComparer.Ordinal.GetType()");
        }

        [Theory, MemberData(nameof(ConstructorTheoryData))]
        public void Constructors(JwtHeaderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Constructors", theoryData);

            try
            {
                var jwtHeader = new JwtHeader(theoryData.SigningCredentials, theoryData.OutboundAlgorithmMap);
                theoryData.ExpectedException.ProcessNoException();
                if (theoryData.SigningCredentials != null)
                {
                    if (theoryData.OutboundAlgorithmMap != null)
                    {
                        if (theoryData.OutboundAlgorithmMap.TryGetValue(theoryData.SigningCredentials.Algorithm, out string alg))
                        {
                            if (!jwtHeader.Alg.Equals(alg))
                                context.AddDiff($"!jwtHeader.Alg.Equals(alg), '{jwtHeader.Alg}' : '{alg}', using OutboundAlgorithmMap");
                        }
                        else
                        {
                            if (jwtHeader.Alg != theoryData.SigningCredentials.Algorithm)
                                context.AddDiff($"jwtHeader.Alg != theoryData.SigningCredentials.Algorithm, '{jwtHeader.Alg}' : '{alg}'");
                        }
                    }

                    if (string.IsNullOrEmpty(theoryData.SigningCredentials.Key.KeyId))
                    {
                        if (!string.IsNullOrEmpty(jwtHeader.Kid))
                            context.AddDiff($"Kid should not be set as SigningCredentials.Key.KeyId is Null or Empty. Kid : '{jwtHeader.Kid}'");
                    }
                    else if (!theoryData.SigningCredentials.Key.KeyId.Equals(jwtHeader.Kid))
                    {
                        context.AddDiff($"!theoryData.SigningCredentials.Key.KeyId.Equals(jwtHeader.Kid)");
                    }

                    if (theoryData.SigningCredentials is X509SigningCredentials x509SigningCredentials)
                    {
                        var x5t = jwtHeader[JwtHeaderParameterNames.X5t] as string;
                        if (string.IsNullOrEmpty(x5t))
                            context.AddDiff("!theoryData.SigningCredentials.Key.KeyId.Equals(jwtHeader.Kid)");
                        else if (!x5t.Equals(Base64UrlEncoder.Encode(x509SigningCredentials.Certificate.GetCertHash())))
                            context.AddDiff("!x5t.Equals(Base64UrlEncoder.Encode(x509SigningCredentials.Certificate.GetCertHash()))");
                    }
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtHeaderTheoryData> ConstructorTheoryData
        {
            get => new TheoryData<JwtHeaderTheoryData>
            {
                new JwtHeaderTheoryData
                {
                    First = true,
                    SigningCredentials = Default.AsymmetricSigningCredentials,
                    TestId = "Test1"
                },
                new JwtHeaderTheoryData
                {
                    SigningCredentials = new X509SigningCredentials(Default.Certificate),
                    TestId = "Test2"
                },
                new JwtHeaderTheoryData
                {
                    SigningCredentials = new X509SigningCredentials(Default.Certificate, SecurityAlgorithms.RsaSha512),
                    TestId = "Test3"
                },
                new JwtHeaderTheoryData
                {
                    OutboundAlgorithmMap = new Dictionary<string, string>{ { SecurityAlgorithms.RsaSha512, SecurityAlgorithms.RsaSha384} },
                    SigningCredentials = new X509SigningCredentials(Default.Certificate, SecurityAlgorithms.RsaSha512),
                    TestId = "Test4"
                }
            };
        }

        [Fact]
        public void Kid()
        {
            var jsonWebKey = new JsonWebKey(DataSets.JsonWebKeyString);
            var credentials = new SigningCredentials(jsonWebKey, SecurityAlgorithms.RsaSha256Signature);
            var token = new JwtSecurityToken(claims: Default.Claims, signingCredentials: credentials);
            Assert.Equal(jsonWebKey.Kid, token.Header.Kid);
        }

        // Test checks to make sure that GetStandardClaim() returns null (not "null") if the value associated with the claimType parameter is null.
        [Fact]
        public void GetStandardClaimNull()
        {
            var jwtHeader = new JwtHeader();
            jwtHeader[JwtHeaderParameterNames.Kid] = null;
            var kid = jwtHeader.Kid;
            Assert.True(kid == null);
        }
    }

    public class JwtHeaderTheoryData : TheoryDataBase
    {
        public IDictionary<string, string > OutboundAlgorithmMap { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
   }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
