// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

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

        [Theory, MemberData(nameof(ConstructorTheoryData), DisableDiscoveryEnumeration = true)]
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

        [Fact]
        public void Getx5cDirectlyFromHeader_x5cIsUnsupportedType()
        {
            var arrayWithUnsupportedTypes = new List<object>
            {
                new List<string>()
            };

            JwtHeader header = new JwtHeader
            {
                { JwtHeaderParameterNames.X5c, arrayWithUnsupportedTypes }
            };

            var exception = Assert.Throws<JsonException>(() => header.X5c);

            Assert.Contains("IDX11026", exception.Message);
        }

        [Fact]
        public void Getx5cDirectlyFromHeader_x5cIsList()
        {
            X509Chain ch = new X509Chain();
            ch.Build(KeyingMaterial.CertSelfSigned1024_SHA256);

            var x5cArray = new List<string>();

            foreach (var element in ch.ChainElements)
                x5cArray.Add(Convert.ToBase64String(element.Certificate.Export(X509ContentType.Cert)));

            JwtHeader header = new JwtHeader
            {
                { JwtHeaderParameterNames.X5c, x5cArray }
            };

            var expectedX5c = JsonSerializer.Serialize(x5cArray, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            Assert.Equal(expectedX5c, header.X5c);
        }

        [Fact]
        public void Getx5cDirectlyFromHeader_x5cIsJsonElement()
        {
            X509Chain ch = new X509Chain();
            ch.Build(KeyingMaterial.CertSelfSigned1024_SHA256);

            var x5cArray = new List<string>();

            foreach (var element in ch.ChainElements)
                x5cArray.Add(Convert.ToBase64String(element.Certificate.Export(X509ContentType.Cert)));

            var x5cJsonElement = JsonSerializer.Serialize(x5cArray);

            JwtHeader header = new JwtHeader
            {
                { JwtHeaderParameterNames.X5c, x5cJsonElement }
            };

            var expectedX5c = JsonSerializer.Serialize(x5cArray);
            Assert.Equal(expectedX5c, header.X5c);
        }

        [Fact]
        public void Getx5cRoundTrip()
        {
            X509Chain ch = new X509Chain();
            ch.Build(KeyingMaterial.CertSelfSigned1024_SHA256);

            var x5CArray = new List<string>();

            foreach (var element in ch.ChainElements)
                x5CArray.Add(Convert.ToBase64String(element.Certificate.Export(X509ContentType.Cert)));

            JwtHeader header = new JwtHeader
            {
                { JwtHeaderParameterNames.X5c, x5CArray }
            };

            var payload = new JwtPayload();

            SecurityToken securityToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(securityToken);

            var jsonWebToken = new JsonWebToken(jwt);

            var x5cFromJsonWebToken = jsonWebToken.Header.GetValue<string>(JwtHeaderParameterNames.X5c);

            JwtSecurityToken token = tokenHandler.ReadJwtToken(jwt);

            string x5CFromJwtSecurityToken = token.Header.X5c;
            Assert.NotEmpty(x5CFromJwtSecurityToken);
            Assert.Equal(x5CFromJwtSecurityToken, x5cFromJsonWebToken);
        }
    }

    public class JwtHeaderTheoryData : TheoryDataBase
    {
        public IDictionary<string, string> OutboundAlgorithmMap { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}
