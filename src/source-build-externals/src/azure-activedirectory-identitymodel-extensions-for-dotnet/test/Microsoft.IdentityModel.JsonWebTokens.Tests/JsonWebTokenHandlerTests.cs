// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt.Tests;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;
using Microsoft.IdentityModel.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using JsonWebTokenHandler6x = Microsoft.IdentityModel.JsonWebTokens.Tests.JsonWebTokenHandler6x;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerTests
    {
        [Fact]
        public void JsonWebTokenHandler_CreateToken_SameTypeMultipleValues()
        {
            var identity = new ClaimsIdentity("Test");

            var claimValues = new List<string> { "value1", "value2", "value3", "value4" };

            foreach (var value in claimValues)
                identity.AddClaim(new Claim("a", value));

            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(Default.AsymmetricSigningKey, SecurityAlgorithms.RsaSsaPssSha256),
                Subject = identity
            };

            var handler = new JsonWebTokenHandler();

            var token = handler.CreateToken(descriptor);

            var jwt = new JsonWebToken(token);
            var claims = jwt.Claims.ToList();

            int defaultClaimsCount = 3;

            Assert.Equal(defaultClaimsCount + claimValues.Count, claims.Count);

            var aTypeClaims = claims.Where(c => c.Type == "a").ToList();

            Assert.Equal(4, aTypeClaims.Count);

            foreach (var value in claimValues)
                Assert.NotNull(aTypeClaims.SingleOrDefault(c => c.Value == value));
        }

        // This test checks to make sure that the value of JsonWebTokenHandler.Base64UrlEncodedUnsignedJWSHeader has remained unchanged.
        [Fact]
        public void Base64UrlEncodedUnsignedJwtHeader()
        {
            TestUtilities.WriteHeader($"{this}.Base64UrlEncodedUnsignedJwtHeader");
            var context = new CompareContext();

            var header = new JObject
            {
                { JwtHeaderParameterNames.Alg, SecurityAlgorithms.None }
            };
            var rawHeader = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(header.ToString(Formatting.None)));

            if (!JsonWebTokenHandler.Base64UrlEncodedUnsignedJWSHeader.Equals(rawHeader))
                context.AddDiff("!JsonWebTokenHandler.Base64UrlEncodedUnsignedJWSHeader.Equals(rawHeader)");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void CreateTokenThrowsNullArgumentException()
        {
            var handler = new JsonWebTokenHandler();
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken(null, Default.SymmetricEncryptingCredentials, new Dictionary<string, object> { {"key", "value" } }));
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken("Payload", (EncryptingCredentials) null, new Dictionary<string, object> { { "key", "value" } }));
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken("Payload", Default.SymmetricEncryptingCredentials, (Dictionary<string, object>) null));
        }

        [Theory, MemberData(nameof(TokenValidationClaimsTheoryData))]
        public void ValidateTokenValidationResult(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResult");
            var tokenValidationResult = theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;
            Assert.Equal(tokenValidationResult.Claims, TokenUtilities.CreateDictionaryFromClaims(tokenValidationResult.ClaimsIdentity.Claims));
        }

        [Theory, MemberData(nameof(TokenValidationClaimsTheoryData))]
        public void ValidateTokenDerivedHandlerValidationResult(JsonWebTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateTokenDerivedHandlerValidationResult", theoryData);
            var derivedJsonWebTokenHandler = new DerivedJsonWebTokenHandler();
            var tokenValidationResult = theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;
            var tokenValidationDerivedResult = derivedJsonWebTokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;
            IdentityComparer.AreEqual(tokenValidationResult.Claims, TokenUtilities.CreateDictionaryFromClaims(tokenValidationResult.ClaimsIdentity.Claims), context);
            IdentityComparer.AreEqual(tokenValidationDerivedResult.Claims, TokenUtilities.CreateDictionaryFromClaims(tokenValidationDerivedResult.ClaimsIdentity.Claims), context);
            IdentityComparer.AreEqual(tokenValidationResult.Claims, tokenValidationDerivedResult.Claims, context);
            IdentityComparer.AreEqual(tokenValidationResult.ClaimsIdentity.Claims, tokenValidationDerivedResult.ClaimsIdentity.Claims, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JsonWebTokenTheoryData> TokenValidationClaimsTheoryData()
        {
            var theoryData = new TheoryData<JsonWebTokenTheoryData>();
            var tokenHandler = new JsonWebTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            // similar to: "eyJhbGciOiJSUzI1NiIsImtpZCI6IlJzYVNlY3VyaXR5S2V5XzIwNDgiLCJ0eXAiOiJKV1QifQ.eyJlbWFpbCI6IkJvYkBjb250b3NvLmNvbSIsImdpdmVuX25hbWUiOiJCb2IiLCJpc3MiOiJodHRwOi8vRGVmYXVsdC5Jc3N1ZXIuY29tIiwiYXVkIjoiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwibmJmIjoiMTQ4OTc3NTYxNyIsImV4cCI6IjE2MTYwMDYwMTcifQ.GcIi6FGp1JS5VF70_ULa8g6GTRos9Y7rUZvPAo4hm10bBNfGhdd5uXgsJspiQzS8vwJQyPlq8a_BpL9TVKQyFIRQMnoZWe90htmNWszNYbd7zbLJZ9AuiDqDzqzomEmgcfkIrJ0VfbER57U46XPnUZQNng2XgMXrXmIKUqEph_vLGXYRQ4ndfwtRrR6BxQFd1PS1T5KpEoUTusI4VEsMcutzfXUygLDiRKIcnLFA0kQpeoHllO4Nb_Sxv63GCb0d1076FfSEYtyRxF4YSCz1In-ee5dwEK8Mw3nHscu-1hn0Fe98RBs-4OrUzI0WcV8mq9IIB3i-U-CqCJEP_hVCiA";

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://Default.Audience.com",
                ValidateLifetime = false,
                ValidIssuer = "http://Default.Issuer.com",
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };

            theoryData.Add(new JsonWebTokenTheoryData()
            {
                TokenHandler = tokenHandler,
                AccessToken = accessToken,
                ValidationParameters = tokenValidationParameters
            });

            tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadAllShortClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
            accessToken = tokenHandler.CreateToken(tokenDescriptor);

            theoryData.Add(new JsonWebTokenTheoryData()
            {
                TokenHandler = tokenHandler,
                AccessToken = accessToken,
                ValidationParameters = tokenValidationParameters
            });

            return theoryData;
        }

        [Theory, MemberData(nameof(TokenValidationTheoryData))]
        public void ValidateTokenValidationResultThrowsWarning(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultThrowsWarning");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;

            //access claims without checking IsValid or Exception
            var claims = tokenValidationResult.Claims;

            //check if warning message was logged
            var warningId = "IDX10109";
            Assert.Contains(warningId, listener.TraceBuffer);
        }

        [Theory, MemberData(nameof(TokenValidationTheoryData))]
        public void ValidateTokenValidationResultDoesNotThrowWarningWithIsValidRead(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultDoesNotThrowWarningWithIsValidRead");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;

            //checking IsValid first, then access claims
            var isValid = tokenValidationResult.IsValid;
            var claims = tokenValidationResult.Claims;

            //check if warning message was logged
            var warningId = "IDX10109";
            Assert.DoesNotContain(warningId, listener.TraceBuffer);
        }

        [Theory, MemberData(nameof(TokenValidationTheoryData))]
        public void ValidateTokenValidationResultDoesNotThrowWarningWithExceptionRead(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultDoesNotThrowWarningWithExceptionRead");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters).Result;

            //checking exception first, then access claims
            var exception = tokenValidationResult.Exception;
            var claims = tokenValidationResult.Claims;

            //check if warning message was logged
            var warningId = "IDX10109";
            Assert.DoesNotContain(warningId, listener.TraceBuffer);
        }

        public static TheoryData<JsonWebTokenTheoryData> TokenValidationTheoryData()
        {
            var theoryData = new TheoryData<JsonWebTokenTheoryData>();
            //create token and token validation parameters
            var tokenHandler = new JsonWebTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://Default.Audience.com",
                ValidateLifetime = false,
                ValidIssuer = "http://Default.Issuer.com",
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };

            theoryData.Add(new JsonWebTokenTheoryData()
            {
                ValidationParameters = tokenValidationParameters,
                TokenHandler = tokenHandler,
                AccessToken = accessToken
            });

            return theoryData;
        }

        [Theory, MemberData(nameof(SegmentTheoryData))]
        public void SegmentCanRead(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SegmentCanRead", theoryData);

            var handler = new JsonWebTokenHandler();
            if (theoryData.CanRead != handler.CanReadToken(theoryData.Token))
                context.Diffs.Add($"theoryData.CanRead != handler.CanReadToken(theoryData.Token))");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> SegmentTheoryData()
        {
            var theoryData = new TheoryData<JwtTheoryData>();

            JwtTestData.InvalidRegExSegmentsData(theoryData);
            JwtTestData.InvalidNumberOfSegmentsData(
                new List<string>
                {
                        "IDX14100:",
                        "IDX14120",
                        "IDX14121",
                        "IDX14121",
                        "IDX14310",
                        "IDX14122"
                },
                theoryData);


        JwtTestData.InvalidEncodedSegmentsData("", theoryData);
            JwtTestData.ValidEncodedSegmentsData(theoryData);

            return theoryData;
        }

        [Theory, MemberData(nameof(CreateTokenWithEmptyPayloadUsingSecurityTokenDescriptorTheoryData))]
        public void CreateTokenWithEmptyPayloadUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateEmptyJWSUsingSecurityTokenDescriptor", theoryData);
            try
            {
                string jwtFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);
                var tokenValidationResultFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtFromSecurityTokenDescriptor, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResultFromSecurityTokenDescriptor.IsValid, theoryData.IsValid, context);
                var jwsTokenFromSecurityTokenDescriptor = new JsonWebToken(jwtFromSecurityTokenDescriptor);

                if (theoryData.TokenDescriptor.SigningCredentials?.Key is X509SecurityKey x509SecurityKey)
                {
                    IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.X5t, x509SecurityKey.X5t, context);
                    IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Kid, x509SecurityKey.KeyId, context);
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateTokenWithEmptyPayloadUsingSecurityTokenDescriptorTheoryData()
        {
            return new TheoryData<CreateTokenTheoryData>()
            {
                new CreateTokenTheoryData
                {
                    TestId = "PayloadEmptyUnsignedJWSWithDefaultTimes",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>()
                    },
                    JsonWebTokenHandler = new JsonWebTokenHandler(),
                    ValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false,
                        RequireSignedTokens = false
                    }
                },
                new CreateTokenTheoryData
                {
                    TestId = "PayloadEmptyUnsignedJWSWithoutDefaultTimes",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object>()
                    },
                    JsonWebTokenHandler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false },
                    ValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false,
                        RequireSignedTokens = false
                    }
                },
                new CreateTokenTheoryData
                {
                    TestId = "PayloadEmptyJWSWithDefaultTimes",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        Claims = new Dictionary<string, object>()
                    },
                    JsonWebTokenHandler = new JsonWebTokenHandler(),
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false
                    }
                },
                new CreateTokenTheoryData
                {
                    TestId = "PayloadEmptyJWSWithoutDefaultTimes",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        Claims = new Dictionary<string, object>()
                    },
                    JsonWebTokenHandler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false
                    }
                },
                new CreateTokenTheoryData
                {
                    TestId = "PayloadEmptyJWE",
                    Payload = Default.PayloadString,
                    TokenDescriptor =  new SecurityTokenDescriptor
                    {
                        SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                        EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                        Claims = new Dictionary<string, object>()
                    },
                    JsonWebTokenHandler = new JsonWebTokenHandler(),
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuer = false
                    },
                }
            };
        }

#if NET_CORE
        [PlatformSpecific(TestPlatforms.Windows)]
#endif
        /// <summary>
        /// Verify the results from ValidateToken() and ValidateTokenAsync() should match.
        /// </summary>
        /// <param name="theoryData">The test data.</param>
        [Theory, MemberData(nameof(CreateJWEWithAesGcmTheoryData))]
        public void TokenValidationResultsShouldMatch(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.TokenValidationResultCompare", theoryData);
            try
            {
                string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);

                theoryData.ValidationParameters.ValidateLifetime = false;
                var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                var validationResult = theoryData.JwtSecurityTokenHandler.ValidateTokenAsync(jweFromJwtHandler, theoryData.ValidationParameters).Result;

                // verify the results from asynchronous and synchronous are the same
                IdentityComparer.AreClaimsIdentitiesEqual(claimsPrincipal.Identity as ClaimsIdentity, validationResult.ClaimsIdentity, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(CreateJWEWithAesGcmTheoryData))]
        public void CreateJWEWithAesGcm(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEWithAesGcm", theoryData);
            try
            {
                string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jweFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                theoryData.ValidationParameters.ValidateLifetime = false;
                var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                var validationResult = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJsonHandler, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(validationResult.IsValid, theoryData.IsValid, context);
                var validatedTokenFromJsonHandler = validationResult.SecurityToken;
                IdentityComparer.AreEqual(validationResult.IsValid, theoryData.IsValid, context);
                IdentityComparer.AreEqual(claimsPrincipal.Identity, validationResult.ClaimsIdentity, context);
                IdentityComparer.AreEqual((validatedTokenFromJwtHandler as JwtSecurityToken).Claims, (validatedTokenFromJsonHandler as JsonWebToken).Claims, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWEWithAesGcmTheoryData
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler
                {
                    SetDefaultTimesOnTokenCreation = false
                };

                tokenHandler.InboundClaimTypeMap.Clear();
                var encryptionCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_AesGcm128;
                encryptionCredentials.CryptoProviderFactory = new CryptoProviderFactoryMock();
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "AesGcm128EncryptionWithMock",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = encryptionCredentials,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                            TokenType = "TokenType"
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_128,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "AesGcm256Encryption",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_AesGcm256,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                            TokenType = "TokenType"
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ExpectedException = ExpectedException.SecurityTokenEncryptionFailedException("IDX10616:", typeof(NotSupportedException))
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "AesGcm_InvalidDecryptionKeySize",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = encryptionCredentials,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                            TokenType = "TokenType"
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_64,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10653:")
                    }
                };
            }
        }

        // Tests checks to make sure that the token string created by the JsonWebTokenHandler is consistent with the 
        // token string created by the JwtSecurityTokenHandler.
        [Theory, MemberData(nameof(CreateJWETheoryData))]
        public void CreateJWE(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWE", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jweFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                var claimsPrincipalFromJwtHandler = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                var validationResultFromJsonHandler = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJsonHandler, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(validationResultFromJsonHandler.IsValid, theoryData.IsValid, context);

                var validatedTokenFromJsonHandler = validationResultFromJsonHandler.SecurityToken;
                var validationResultFromJwtJsonHandler = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJwtHandler, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(validationResultFromJwtJsonHandler.IsValid, theoryData.IsValid, context);
                IdentityComparer.AreEqual(claimsPrincipalFromJwtHandler.Identity, validationResultFromJsonHandler.ClaimsIdentity, context);
                IEnumerable<Claim> jwtHandlerClaims = (validatedTokenFromJwtHandler as JwtSecurityToken).Claims;
                IEnumerable<Claim> jsonHandlerClaims = (validatedTokenFromJsonHandler as JsonWebToken).Claims;
                IdentityComparer.AreEqual((validatedTokenFromJwtHandler as JwtSecurityToken).Claims, (validatedTokenFromJsonHandler as JsonWebToken).Claims, context);

                theoryData.ExpectedException.ProcessNoException(context);
                context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(JsonWebToken), new List<string> { "EncodedToken", "AuthenticationTag", "Ciphertext", "InitializationVector" } },
                };

                var jweTokenFromJwtHandler = new JsonWebToken(jweFromJwtHandler);
                var jweTokenFromHandler = new JsonWebToken(jweFromJsonHandler);

                if (!string.IsNullOrEmpty(theoryData.TokenDescriptor.TokenType))
                {
                    IdentityComparer.AreEqual(jweTokenFromJwtHandler.Typ, theoryData.TokenDescriptor.TokenType, context);
                    IdentityComparer.AreEqual(jweTokenFromHandler.Typ, theoryData.TokenDescriptor.TokenType, context);
                }

                IdentityComparer.AreEqual(validationResultFromJsonHandler.SecurityToken as JsonWebToken, validationResultFromJwtJsonHandler.SecurityToken as JsonWebToken, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWETheoryData
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler
                {
                    SetDefaultTimesOnTokenCreation = false
                };

                tokenHandler.InboundClaimTypeMap.Clear();
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData("TestCase1")
                    {
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                            TokenType = "TokenType",
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("TestCase2")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("TestCase3")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            IssuerValidator = ValidationDelegates.IssuerValidatorReturnsDifferentIssuer,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SecurityTokenDecryptionTheoryData))]
        public void GetEncryptionKeys(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EncryptionKeysCheck", theoryData);
            try
            {
                string jweFromJsonHandlerWithKid = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.EncryptingCredentials);
                var jwtTokenFromJsonHandlerWithKid = new JsonWebToken(jweFromJsonHandlerWithKid);
                var encryptionKeysFromJsonHandlerWithKid = theoryData.JsonWebTokenHandler.GetContentEncryptionKeys(jwtTokenFromJsonHandlerWithKid, theoryData.ValidationParameters, theoryData.Configuration);

                IdentityComparer.AreEqual(encryptionKeysFromJsonHandlerWithKid, theoryData.ExpectedDecryptionKeys);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> SecurityTokenDecryptionTheoryData
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler
                {
                    SetDefaultTimesOnTokenCreation = false
                };

                var configurationWithDecryptionKeys = new OpenIdConnectConfiguration();
                configurationWithDecryptionKeys.TokenDecryptionKeys.Add(KeyingMaterial.DefaultSymmetricSecurityKey_256);
                configurationWithDecryptionKeys.TokenDecryptionKeys.Add(KeyingMaterial.DefaultSymmetricSecurityKey_512);

                tokenHandler.InboundClaimTypeMap.Clear();
                return new TheoryData<CreateTokenTheoryData>
                {
                   new CreateTokenTheoryData
                   {
                        First = true,
                        TestId = "EncryptionKeyInConfig",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithDecryptionKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                        Algorithm = JwtConstants.DirectKeyUseAlg,
                        EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2_NoKeyId
                   },
                   new CreateTokenTheoryData
                   {
                        TestId = "ValidEncryptionKeyInConfig",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_512 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithDecryptionKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                        Algorithm = JwtConstants.DirectKeyUseAlg,
                        EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2_NoKeyId
                   },
                   new CreateTokenTheoryData
                   {
                        TestId = "Valid",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256, KeyingMaterial.DefaultSymmetricSecurityKey_512 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                        Algorithm = JwtConstants.DirectKeyUseAlg,
                        EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2_NoKeyId
                   },
                   new CreateTokenTheoryData
                   {
                        TestId = "AlgorithmMisMatch",
                        Payload = Default.PayloadString,
                        ExpectedException = ExpectedException.KeyWrapException("IDX10618:"),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Algorithm = SecurityAlgorithms.Aes256CbcHmacSha512,
                        EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2_NoKeyId
                   }
                };
            }
        }

        // Tests checks to make sure that the token string (JWE) created by calling 
        // CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
        // is equivalent to the token string created by calling CreateToken(SecurityTokenDescriptor tokenDescriptor).
        [Theory, MemberData(nameof(CreateJWEUsingSecurityTokenDescriptorTheoryData))]
        public void CreateJWEUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEUsingSecurityTokenDescriptor", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                string jweFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);
                string jweFromString;
                if (theoryData.TokenDescriptor.SigningCredentials == null)
                    jweFromString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.EncryptingCredentials);
                else if (theoryData.TokenDescriptor.AdditionalHeaderClaims != null)
                    jweFromString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.EncryptingCredentials, theoryData.TokenDescriptor.AdditionalHeaderClaims);
                else
                    jweFromString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.EncryptingCredentials);

                var validationResultFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromSecurityTokenDescriptor, theoryData.ValidationParameters).Result;
                var validationResultFromString = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromString, theoryData.ValidationParameters).Result;

                IdentityComparer.AreEqual(validationResultFromSecurityTokenDescriptor.IsValid, theoryData.IsValid, context);
                IdentityComparer.AreEqual(validationResultFromString.IsValid, theoryData.IsValid, context);

                var jweTokenFromSecurityTokenDescriptor = validationResultFromSecurityTokenDescriptor.SecurityToken as JsonWebToken;
                var jweTokenFromString = validationResultFromString.SecurityToken as JsonWebToken;

                // If the signing key used was an x509SecurityKey, make sure that the 'X5t' property was set properly and
                // that the values of 'X5t' and 'Kid' on the JsonWebToken are equal to each other.
                if (theoryData.TokenDescriptor.SigningCredentials?.Key is X509SecurityKey x509SecurityKey)
                {
                    var innerTokenFromSecurityTokenDescriptor = jweTokenFromSecurityTokenDescriptor.InnerToken as JsonWebToken;
                    var innerTokenFromString = jweTokenFromString.InnerToken as JsonWebToken;

                    IdentityComparer.AreEqual(innerTokenFromSecurityTokenDescriptor.X5t, x509SecurityKey.X5t, context);
                    IdentityComparer.AreEqual(innerTokenFromSecurityTokenDescriptor.Kid, x509SecurityKey.KeyId, context);
                    IdentityComparer.AreEqual(innerTokenFromString.X5t, x509SecurityKey.X5t, context);
                    IdentityComparer.AreEqual(innerTokenFromString.Kid, x509SecurityKey.KeyId, context);
                }

                context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(JsonWebToken), new List<string> { "EncodedHeader", "EncodedToken", "AuthenticationTag", "Ciphertext", "InitializationVector" } },
                };

                if (theoryData.PropertiesToIgnoreWhenComparing.Count > 0)
                {
                    foreach (var ignore in theoryData.PropertiesToIgnoreWhenComparing)
                    {
                        if (context.PropertiesToIgnoreWhenComparing.TryGetValue(ignore.Key, out List<string> list))
                        {
                            list.AddRange(ignore.Value);
                        }
                        else
                        {
                            context.PropertiesToIgnoreWhenComparing[ignore.Key] = ignore.Value;
                        }
                    }
                }

                IdentityComparer.AreEqual(jweTokenFromSecurityTokenDescriptor, jweTokenFromString, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWEUsingSecurityTokenDescriptorTheoryData
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "Valid",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "ValidUsingX509SecurityKey",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "TokenDescriptorNull",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  null,
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "TokenDescriptorClaimsNull",
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant) },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            IssuedAt = Default.NotBefore,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = null
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidateIssuer = false
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "TokenDescriptorClaimsEmpty",
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant) },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            IssuedAt = Default.NotBefore,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = new Dictionary<string, object>()
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidateIssuer = false,
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "TokenDescriptorSigningCredentialsNullRequireSignedTokensFalse",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = null,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            RequireSignedTokens = false,
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "TokenDescriptorSigningCredentialsNullRequireSignedTokensTrue",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = null,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                        },
                        IsValid = false
                    },
                    new CreateTokenTheoryData // Test checks that values in SecurityTokenDescriptor.Payload
                    // are properly replaced with the properties that are explicitly specified on the SecurityTokenDescriptor.
                    {
                        TestId = "UseSecurityTokenDescriptorProperties",
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Azp, Default.Azp },
                            { JwtRegisteredClaimNames.Aud, "Audience" },
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.Parse("2023-03-17T18:33:37.080Z")) },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Parse("2038-03-17T18:33:37.080Z")) },
                            { JwtRegisteredClaimNames.Iss, "Issuer" },
                            { JwtRegisteredClaimNames.Jti, Default.Jti },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.Parse("2018-03-17T18:33:37.080Z")) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary,
                            Issuer = "Issuer",
                            Audience = "Audience",
                            IssuedAt = DateTime.Parse("2038-03-17T18:33:37.080Z"),
                            NotBefore = DateTime.Parse("2018-03-17T18:33:37.080Z"),
                            Expires = DateTime.Parse("2023-03-17T18:33:37.080Z")
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = "Audience",
                            ValidIssuer = "Issuer"
                        },
                        PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                        {
                            { typeof(JsonWebToken), new List<string> { "InnerToken", "EncodedPayload", "EncodedSignature" } },
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "SingleAdditionalHeaderClaim",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "MultipleAdditionalHeaderClaims",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 }, { "string", "string" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "DuplicateAdditionalHeaderClaim",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { JwtHeaderParameterNames.Alg, "alg" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenException("IDX14116:")
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "DuplicateAdditionalHeaderClaimDifferentCase",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { JwtHeaderParameterNames.Alg.ToUpper(), "alg" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenException("IDX14116:")
                    }
                };
            }
        }

        // Tests checks to make sure that the token string created by the JsonWebTokenHandler is consistent with the 
        // token string created by the JwtSecurityTokenHandler.
        [Theory, MemberData(nameof(CreateJWSTheoryData))]
        public void CreateJWS(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWS", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                string jwsFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jwsFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jwsFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedToken);
                var tokenValidationResult = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwsFromJsonHandler, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResult.IsValid, theoryData.IsValid, context);
                IdentityComparer.AreEqual(claimsPrincipal.Identity, tokenValidationResult.ClaimsIdentity, context);

                theoryData.ExpectedException.ProcessNoException(context);
                var jwsTokenFromJwtHandler = new JsonWebToken(jwsFromJwtHandler);
                var jwsTokenFromHandler = new JsonWebToken(jwsFromJsonHandler);

                if (!string.IsNullOrEmpty(theoryData.TokenDescriptor.TokenType))
                {
                    IdentityComparer.AreEqual(jwsTokenFromJwtHandler.Typ, theoryData.TokenDescriptor.TokenType, context);
                    IdentityComparer.AreEqual(jwsTokenFromHandler.Typ, theoryData.TokenDescriptor.TokenType, context);
                }

                IdentityComparer.AreEqual(jwsTokenFromJwtHandler, jwsTokenFromHandler, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWSTheoryData
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler
                {
                    SetDefaultTimesOnTokenCreation = false
                };

                tokenHandler.InboundClaimTypeMap.Clear();

                var signingCredentialsNoKeyId = new SigningCredentials(KeyingMaterial.JsonWebKeyRsa_2048, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256);
                signingCredentialsNoKeyId.Key.KeyId = null;

                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "ValidUsingTokenType",
                        TokenDescriptor = new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Subject = new ClaimsIdentity(Default.PayloadClaims),
                            TokenType = "TokenType"
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "Valid",
                        Payload = Default.PayloadString,
                        TokenDescriptor = new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Subject = new ClaimsIdentity(Default.PayloadClaims)
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "IssuerValidator",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Subject = new ClaimsIdentity(Default.PayloadClaims)
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            IssuerValidator = ValidationDelegates.IssuerValidatorReturnsDifferentIssuer,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "NoKeyId",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = signingCredentialsNoKeyId,
                            Subject = new ClaimsIdentity(Default.PayloadClaims)
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        JwtSecurityTokenHandler = tokenHandler,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = signingCredentialsNoKeyId.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    }
                };
            }
        }

        // This test checks to make sure that additional header claims are added as expected to the JWT token header.
        [Theory, MemberData(nameof(CreateJWSWithAdditionalHeaderClaimsTheoryData))]
        public void CreateJWSWithAdditionalHeaderClaims(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWSWithAdditionalHeaderClaims", theoryData);

            var jwtToken = new JsonWebTokenHandler().CreateToken(theoryData.TokenDescriptor);
            var jwtToken6x = new JsonWebTokenHandler6x().CreateToken(theoryData.TokenDescriptor);

            JsonWebToken jsonWebToken = new JsonWebToken(jwtToken);
            JsonWebToken jsonWebToken6x = new JsonWebToken(jwtToken6x);

            if (!IdentityComparer.AreEqual(jsonWebToken.Header, jsonWebToken6x.Header, context))
            {
                context.AddDiff("jsonWebToken.Header != jsonWebToken6x.Header");
                context.AddDiff("********************************************");
            }

            IdentityComparer.AreEqual(jwtToken6x, theoryData.JwtToken, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWSWithAdditionalHeaderClaimsTheoryData
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                       TestId = "DifferentTypHeaderValue",
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { JwtHeaderParameterNames.Typ, "TEST" } }
                       },
                       JwtToken = ReferenceTokens.JWSWithDifferentTyp
                    },
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "MultipleAdditionalHeaderClaims",
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = ReferenceTokens.PayloadDictionary,
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { "int", 123 }, { "string", "string" } }
                        },
                        JwtToken = ReferenceTokens.JWSWithMultipleAdditionalHeaderClaims
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "SingleAdditionalHeaderClaim",
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 } }
                       },
                       JwtToken = ReferenceTokens.JWSWithSingleAdditionalHeaderClaim
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "EmptyAdditionalHeaderClaims",
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object>()
                       },
                       JwtToken = new JsonWebTokenHandler().CreateToken(ReferenceTokens.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials)
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "UnsignedJWS",
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 } }
                       },
                       JwtToken = ReferenceTokens.UnsignedJWSWithSingleAdditionalHeaderClaim
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateJWEWithPayloadStringTheoryData))]
        public void CreateJWEWithPayloadString(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEWithPayloadString", theoryData);
            var handler = new JsonWebTokenHandler();
            string jwtTokenWithSigning = null;
            JsonWebToken jsonTokenWithSigning = null;
            CompressionProviderFactory.Default = new CompressionProviderFactory();
            try
            {
                var jwtToken = handler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.EncryptingCredentials, theoryData.TokenDescriptor.AdditionalHeaderClaims);
                var jsonToken = new JsonWebToken(jwtToken);

                if (theoryData.TokenDescriptor.SigningCredentials != null)
                {
                    jwtTokenWithSigning = handler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.EncryptingCredentials, CompressionAlgorithms.Deflate, theoryData.TokenDescriptor.AdditionalHeaderClaims, theoryData.TokenDescriptor.AdditionalInnerHeaderClaims);
                    jsonTokenWithSigning = new JsonWebToken(jwtTokenWithSigning);
                }

                if (theoryData.TokenDescriptor.AdditionalHeaderClaims.TryGetValue(JwtHeaderParameterNames.Cty, out object ctyValue))
                {
                    if (!jsonToken.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object headerCtyValue) || (jsonTokenWithSigning != null && !jsonTokenWithSigning.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object _)))
                    {
                        context.AddDiff($"'Cty' claim does not exist in the outer header but present in theoryData.AdditionalHeaderClaims.");
                    }
                    else
                        IdentityComparer.AreEqual(ctyValue.ToString(), headerCtyValue.ToString(), context);
                }
                else if (theoryData.TokenDescriptor.EncryptingCredentials.SetDefaultCtyClaim)
                {
                    if (!jsonToken.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object headerCtyValue) || (jsonTokenWithSigning != null && !jsonTokenWithSigning.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object _)))
                    {
                        context.AddDiff($"'Cty' claim does not exist in the outer header. It is expected to have the default value '{JwtConstants.HeaderType}'.");
                    }
                    else
                        IdentityComparer.AreEqual(JwtConstants.HeaderType, headerCtyValue.ToString(), context);
                }
                else
                {
                    if (jsonToken.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object headerCtyValue) || (jsonTokenWithSigning != null && jsonTokenWithSigning.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object _)))
                    {
                        context.AddDiff($"'Cty' claim does exist in the outer header. It is not expected to exist since SetDefaultCtyClaim is '{theoryData.EncryptingCredentials.SetDefaultCtyClaim}'.");
                    }
                }

                if (theoryData.TokenDescriptor.AdditionalInnerHeaderClaims != null)
                {
                    theoryData.ValidationParameters.ValidateLifetime = false;
                    var result = handler.ValidateTokenAsync(jwtTokenWithSigning, theoryData.ValidationParameters).Result;
                    var token = result.SecurityToken as JsonWebToken;
                    if (theoryData.TokenDescriptor.AdditionalInnerHeaderClaims.TryGetValue(JwtHeaderParameterNames.Cty, out object innerCtyValue))
                    {
                        if (!token.InnerToken.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object headerCtyValue))
                        {
                            context.AddDiff($"'Cty' claim does not exist in the inner header but present in theoryData.AdditionalHeaderClaims.");
                        }
                        else
                            IdentityComparer.AreEqual(innerCtyValue.ToString(), headerCtyValue.ToString(), context);
                    }
                    else
                    {
                        if (token.InnerToken.TryGetHeaderValue(JwtHeaderParameterNames.Cty, out object headerCtyValue))
                        {
                            context.AddDiff($"It is not expected to have 'Cty' claim in the inner header.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWEWithPayloadStringTheoryData
        {
            get
            {
                var NoCtyEncryptionCreds = Default.SymmetricEncryptingCredentials;
                NoCtyEncryptionCreds.SetDefaultCtyClaim = false;
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "JsonPayload",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{ {"int", "123" } },
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "JsonPayload",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            EncryptingCredentials = NoCtyEncryptionCreds,
                            AdditionalHeaderClaims = new Dictionary<string, object>{ {"int", "123" } },
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "JsonPayload_CtyInAdditionalClaims",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{{JwtHeaderParameterNames.Cty, "str"}}
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "NonJsonPayload",
                        Payload = Guid.NewGuid().ToString(),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{{JwtHeaderParameterNames.Cty, "NonJWT"}}
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "CtyInBothAdditionalClaims",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{{JwtHeaderParameterNames.Cty, "str_outer"}},
                            AdditionalInnerHeaderClaims = new Dictionary<string, object>{{JwtHeaderParameterNames.Cty, "str_inner"}}
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "CtyInOuterAdditionalClaims",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{{JwtHeaderParameterNames.Cty, "str"}},
                            AdditionalInnerHeaderClaims = new Dictionary<string, object>{ {"int", "123" } },
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "DefaultParameterinAdditionalInnerHeaderClaims",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>{ { JwtHeaderParameterNames.Cty, "str" } },
                            AdditionalInnerHeaderClaims = new Dictionary<string, object>{ { JwtHeaderParameterNames.Enc, "str" } },
                        },
                        ExpectedException = ExpectedException.SecurityTokenException("IDX14116:")
                    },
                };
            }
        }

        // This test checks to make sure that additional header claims are added as expected to the outer token header.
        [Theory, MemberData(nameof(CreateJWEWithAdditionalHeaderClaimsTheoryData))]
        public void CreateJWEWithAdditionalHeaderClaims(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEWithAdditionalHeaderClaims", theoryData);
            var handler = new JsonWebTokenHandler();
            theoryData.ValidationParameters.ValidateLifetime = false;

            var jwtTokenFromDescriptor = handler.CreateToken(theoryData.TokenDescriptor);
            var validatedJwtTokenFromDescriptor = handler.ValidateTokenAsync(jwtTokenFromDescriptor, theoryData.ValidationParameters).Result.SecurityToken as JsonWebToken;
            var jwtTokenToCompare = handler.ValidateTokenAsync(theoryData.JwtToken, theoryData.ValidationParameters).Result.SecurityToken as JsonWebToken;

            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
            {
                { typeof(JsonWebToken), new List<string> { "EncodedHeader", "EncodedToken", "AuthenticationTag", "Ciphertext", "InitializationVector", "EncryptedKey" } },
            };

            IdentityComparer.AreEqual(validatedJwtTokenFromDescriptor, jwtTokenToCompare, context);

            foreach (var key in theoryData.TokenDescriptor.AdditionalHeaderClaims.Keys)
            {
                if (!validatedJwtTokenFromDescriptor.TryGetHeaderValue(key, out string headerValue))
                    context.AddDiff($"JWE header does not contain the '{key}' claim.");

                var headerValueToCompare = jwtTokenToCompare.GetHeaderValue<string>(key);

                if (headerValue != null && !headerValue.Equals(headerValueToCompare))
                    context.AddDiff($"The value for the '{key}' header claim should be '{headerValueToCompare}' but was '{headerValue}'.");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWEWithAdditionalHeaderClaimsTheoryData
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "JWEDirectEncryption",
                        Payload = ReferenceTokens.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = ReferenceTokens.PayloadDictionary,
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { "int", 123 }, { "string", "string" } }
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer
                        },
                        JwtToken = ReferenceTokens.JWEDirectEcryptionWithAdditionalHeaderClaims
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "JWEDirectEncryptionWithCty",
                        Payload = ReferenceTokens.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = ReferenceTokens.PayloadDictionary,
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { JwtHeaderParameterNames.Cty, JwtConstants.HeaderType} }
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer
                        },
                        JwtToken = ReferenceTokens.JWEDirectEcryptionWithCtyInAdditionalHeaderClaims
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "JWEDirectEncryptionWithDifferentTyp",
                        Payload = ReferenceTokens.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = ReferenceTokens.PayloadDictionary,
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { JwtHeaderParameterNames.Typ, "TEST" } }
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer
                        },
                        JwtToken = ReferenceTokens.JWEDirectEcryptionWithDifferentTyp
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "JWEKeyWrapping",
                       Payload = ReferenceTokens.PayloadString,
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { "int", 123 }, { "string", "string" } }
                       },
                       ValidationParameters = new TokenValidationParameters
                       {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.RsaSecurityKey_2048,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer
                       },
                       JwtToken = ReferenceTokens.JWEKeyWrappingWithAdditionalHeaderClaims
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "JWEKeyWrappingDifferentTyp",
                       Payload = ReferenceTokens.PayloadString,
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            SigningCredentials = Default.SymmetricSigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { JwtHeaderParameterNames.Typ, "TEST" } }
                       },
                       ValidationParameters = new TokenValidationParameters
                       {
                            IssuerSigningKey = Default.SymmetricSigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.RsaSecurityKey_2048,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer
                       },
                       JwtToken = ReferenceTokens.JWEKeyWrappingWithDifferentTyp
                    },
                    new CreateTokenTheoryData
                    {
                       TestId = "JWEKeyWrappingUnsignedInnerJwt",
                       Payload = ReferenceTokens.PayloadString,
                       TokenDescriptor =  new SecurityTokenDescriptor
                       {
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = ReferenceTokens.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { "int", 123 }, { "string", "string" } }
                       },
                       ValidationParameters = new TokenValidationParameters
                       {
                            TokenDecryptionKey = KeyingMaterial.RsaSecurityKey_2048,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer,
                            RequireSignedTokens = false
                       },
                       JwtToken = ReferenceTokens.JWEKeyWrappingUnsignedInnerJWTWithAdditionalHeaderClaims
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "JWEDirectEncryptionUnsignedInnerJWT",
                        Payload = ReferenceTokens.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = ReferenceTokens.PayloadDictionary,
                            EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                            AdditionalHeaderClaims = new Dictionary<string, object>() { { "int", 123 }, { "string", "string" } }
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            TokenDecryptionKey = Default.SymmetricEncryptingCredentials.Key,
                            ValidAudience = ReferenceTokens.Audience,
                            ValidIssuer = ReferenceTokens.Issuer,
                            RequireSignedTokens = false
                        },
                        JwtToken = ReferenceTokens.JWEDirectEncryptionUnsignedInnerJWTWithAdditionalHeaderClaims
                    }
                };
            }
        }

        // Tests checks to make sure that the token string (JWS) created by calling CreateToken(string payload, SigningCredentials signingCredentials)
        // is equivalent to the token string created by calling CreateToken(SecurityTokenDescriptor tokenDescriptor).
        [Theory, MemberData(nameof(CreateJWSUsingSecurityTokenDescriptorTheoryData))]
        public void CreateJWSUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWSUsingSecurityTokenDescriptor", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                JsonWebTokenHandler6x jsonWebTokenHandler6x = new JsonWebTokenHandler6x();

                string jwtFromSecurityTokenDescriptor6x = jwtFromSecurityTokenDescriptor6x = jsonWebTokenHandler6x.CreateToken(theoryData.TokenDescriptor6x ?? theoryData.TokenDescriptor);
                string jwtFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);
                string jwtPayloadAsString;

                if (theoryData.TokenDescriptor.SigningCredentials == null)
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload);
                else if (theoryData.TokenDescriptor.AdditionalHeaderClaims != null)
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.AdditionalHeaderClaims);
                else
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials);

                var jwsTokenFromSecurityTokenDescriptor = new JsonWebToken(jwtFromSecurityTokenDescriptor);
                var jwsTokenFromSecurityTokenDescriptor6x = new JsonWebToken(jwtFromSecurityTokenDescriptor6x);
                var jwsTokenFromString = new JsonWebToken(jwtPayloadAsString);

                var tokenValidationResultFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtFromSecurityTokenDescriptor, theoryData.ValidationParameters).Result;
                var tokenValidationResultFromString = theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtPayloadAsString, theoryData.ValidationParameters).Result;

                IdentityComparer.AreEqual(tokenValidationResultFromSecurityTokenDescriptor.IsValid, theoryData.IsValid, context);
                IdentityComparer.AreEqual(tokenValidationResultFromString.IsValid, theoryData.IsValid, context);

                // If the signing key used was an x509SecurityKey, make sure that the 'X5t' property was set properly and
                // that the values of 'X5t' and 'Kid' on the JsonWebToken are equal to each other.
                if (theoryData.TokenDescriptor.SigningCredentials?.Key is X509SecurityKey x509SecurityKey)
                {
                    IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.X5t, x509SecurityKey.X5t, context);
                    IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Kid, x509SecurityKey.KeyId, context);
                    IdentityComparer.AreEqual(jwsTokenFromString.X5t, x509SecurityKey.X5t, context);
                    IdentityComparer.AreEqual(jwsTokenFromString.Kid, x509SecurityKey.KeyId, context);
                }

                context.PropertiesToIgnoreWhenComparing = theoryData.PropertiesToIgnoreWhenComparing;

                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Header, jwsTokenFromSecurityTokenDescriptor6x.Header, context))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor.Header != jwsTokenFromSecurityTokenDescriptor6x.Header");
                    context.AddDiff("******************************************************************************************");
                    context.AddDiff(" ");
                }

                bool claimsEqual;
                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Claims, jwsTokenFromSecurityTokenDescriptor6x.Claims, context))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor.Claims != jwsTokenFromSecurityTokenDescriptor6x.Claims");
                    context.AddDiff("****************************************************************************");
                    context.AddDiff(" ");
                    claimsEqual = false;
                }
                else
                {
                    claimsEqual = true;
                }

                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Header, jwsTokenFromSecurityTokenDescriptor6x.Header))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor.Header != jwsTokenFromSecurityTokenDescriptor6x.Header");
                    context.AddDiff("****************************************************************************");
                    context.AddDiff(" ");
                }

                // if the claims are the same some properties could be different because of ordering
                CompareContext localContext = new CompareContext(context);
                if (claimsEqual)
                    localContext.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                    {
                        {typeof(JsonWebToken), new List<string> {"EncodedHeader", "EncodedToken", "EncodedPayload", "EncodedSignature"}},
                    };

                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor, jwsTokenFromSecurityTokenDescriptor6x, localContext))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor != jwsTokenFromSecurityTokenDescriptor6x");
                    context.AddDiff("****************************************************************************");
                    context.AddDiff(" ");
                }

                context.Merge(localContext);

                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor.Claims, jwsTokenFromString.Claims, context))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor.Claims != jwsTokenFromString.Claims");
                    context.AddDiff("****************************************************************************");
                    context.AddDiff(" ");
                    claimsEqual = false;
                }
                else
                {
                    claimsEqual = true;
                }

                localContext.Diffs.Clear();
                // if the claims are the same some properties could be different because of ordering
                if (!IdentityComparer.AreEqual(jwsTokenFromSecurityTokenDescriptor, jwsTokenFromString, localContext))
                {
                    context.AddDiff("jwsTokenFromSecurityTokenDescriptor != jwsTokenFromString");
                    context.AddDiff("****************************************************************************");
                    context.AddDiff(" ");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWSUsingSecurityTokenDescriptorTheoryData
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    // Test checks that the values in SecurityTokenDescriptor.Subject.Claims
                    // are properly combined with those specified in SecurityTokenDescriptor.Claims.
                    // Duplicate values (if present with different case) should not be overridden. 
                    // For example, the 'aud' claim on TokenDescriptor.Claims will not be overridden
                    // by the 'AUD' claim on TokenDescriptor.Subject.Claims, but the 'exp' claim will.
                    new CreateTokenTheoryData("TokenDescriptorWithBothSubjectAndClaims")
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, Default.Issuer },
                            { JwtRegisteredClaimNames.Aud.ToUpper(), JArray.FromObject(new List<string>() {"Audience1", "Audience2"}) },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString() },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString()},
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString() },
                            { JwtRegisteredClaimNames.Aud, JArray.FromObject(Default.Audiences) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = new Dictionary<string, object>()
                            {
                                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                                { JwtRegisteredClaimNames.GivenName, "Bob" },
                                { JwtRegisteredClaimNames.Iss, Default.Issuer },
                                { JwtRegisteredClaimNames.Aud, JsonSerializerPrimitives.CreateJsonElement(Default.Audiences) },
                                { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString() },
                                { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString()},
                                { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString() },
                            },
                            Subject = new ClaimsIdentity(new List<Claim>()
                            {
                                new Claim(JwtRegisteredClaimNames.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Iss, "Issuer", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Aud.ToUpper(), "Audience1", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Aud.ToUpper(), "Audience2", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                            }, "AuthenticationTypes.Federation")
                        },
                        TokenDescriptor6x =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = new Dictionary<string, object>()
                            {
                                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                                { JwtRegisteredClaimNames.GivenName, "Bob" },
                                { JwtRegisteredClaimNames.Iss, Default.Issuer },
                                { JwtRegisteredClaimNames.Aud, JArray.FromObject(Default.Audiences) },
                                { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString() },
                                { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString()},
                                { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString() },
                            },
                            Subject = new ClaimsIdentity(new List<Claim>()
                            {
                                new Claim(JwtRegisteredClaimNames.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Iss, "Issuer", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Aud.ToUpper(), "Audience1", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Aud.ToUpper(), "Audience2", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                                new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                            }, "AuthenticationTypes.Federation")
                        },

                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audiences.First(),
                            ValidIssuer = Default.Issuer,
                        }
                    },
                    new CreateTokenTheoryData("ValidUsingClaims")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("ValidUsingSubject")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Subject = Default.PayloadClaimsIdentity
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("ValidUsingClaimsAndX509SecurityKey")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("TokenDescriptorNull")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  null,
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                    },
                    new CreateTokenTheoryData("TokenDescriptorClaimsNull")
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant) },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            IssuedAt = Default.NotBefore,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = null
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidateIssuer = false
                        }
                    },
                    new CreateTokenTheoryData("TokenDescriptorClaimsEmpty")
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant) },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Audience = Default.Audience,
                            IssuedAt = Default.NotBefore,
                            NotBefore = Default.NotBefore,
                            Expires = Default.Expires,
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = new Dictionary<string, object>()
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidateIssuer = false
                        }
                    },
                    new CreateTokenTheoryData("TokenDescriptorSigningCredentialsNullRequireSignedTokensFalse")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = null,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            RequireSignedTokens = false
                        },
                    },
                    new CreateTokenTheoryData("TokenDescriptorSigningCredentialsNullRequireSignedTokensTrue")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = null,
                            Claims = Default.PayloadDictionary
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                        },
                        IsValid = false
                    },
                    new CreateTokenTheoryData("UseSecurityTokenDescriptorProperties")
                    // Test checks that values in SecurityTokenDescriptor.Payload
                    // are properly replaced with the properties that are explicitly specified on the SecurityTokenDescriptor.
                    {
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Aud, "Audience" },
                            { JwtRegisteredClaimNames.Azp, Default.Azp },
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.Parse("2023-03-17T18:33:37.080Z")) },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, "Issuer" },
                            { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Parse("2018-03-17T18:33:37.080Z")) },
                            { JwtRegisteredClaimNames.Jti, Default.Jti },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.Parse("2038-03-17T18:33:37.080Z")) },
                        }.ToString(Formatting.None),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary,
                            Issuer = "Issuer",
                            Audience = "Audience",
                            IssuedAt = DateTime.Parse("2018-03-17T18:33:37.080Z"),
                            NotBefore = DateTime.Parse("2038-03-17T18:33:37.080Z"),
                            Expires = DateTime.Parse("2023-03-17T18:33:37.080Z")
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = "Audience",
                            ValidIssuer = "Issuer"
                        },
                    },
                    new CreateTokenTheoryData("SingleAdditionalHeaderClaim")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("MultipleAdditionalHeaderClaims")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 }, { "string", "string" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        }
                    },
                    new CreateTokenTheoryData("DuplicateAdditionalHeaderClaim")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { JwtHeaderParameterNames.Alg, "alg" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenException("IDX14116:")
                    },
                    new CreateTokenTheoryData("DuplicateAdditionalHeaderClaimDifferentCase")
                    {
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary,
                            AdditionalHeaderClaims = new Dictionary<string, object> () { { JwtHeaderParameterNames.Alg.ToUpper(), "alg" } }
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenException("IDX14116:")
                    },
                    new CreateTokenTheoryData("RsaPss")
                    {
                        Payload = Default.PayloadString,
                        //RsaPss produces different signatures
                        PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                        {
                            { typeof(JsonWebToken), new List<string> { "EncodedToken", "EncodedSignature", "SignatureBytes" } },
                        },
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            Claims = Default.PayloadDictionary,
                            SigningCredentials = new SigningCredentials(Default.AsymmetricSigningKey, SecurityAlgorithms.RsaSsaPssSha256),
                        },
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.AsymmetricSigningKey,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuer = false,
                        }
                    }
               };
            }
        }

        [Fact]
        public void CreateJWSWithDuplicateClaimsRoundTrip()
        {
            TestUtilities.WriteHeader($"{this}.CreateJWSWithDuplicateClaimsRoundTrip");
            var context = new CompareContext();

            var utcNow = DateTime.UtcNow;
            var jsonWebTokenHandler = new JsonWebTokenHandler();

            // This JObject has two duplicate claims (with different case): "aud"/"AUD" and "iat"/"IAT".
            var payload = new JObject()
            {
                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                { JwtRegisteredClaimNames.GivenName, "Bob" },
                { JwtRegisteredClaimNames.Iss, Default.Issuer },
                { JwtRegisteredClaimNames.Aud, Default.Audience },
                { JwtRegisteredClaimNames.Aud.ToUpper(), "Audience" },
                { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString() },
                { JwtRegisteredClaimNames.Iat.ToUpper(), EpochTime.GetIntDate(utcNow).ToString() },
                { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString()},
                { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString() },
            };

            // This ClaimsIdentity has two duplicate claims (with different case): "aud"/"AUD" and "iat"/"IAT".
            var payloadClaimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, Default.Issuer, ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, Default.Audience, ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud.ToUpper(), "Audience", ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(Default.IssueInstant).ToString(), ClaimValueTypes.Integer64, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat.ToUpper(), EpochTime.GetIntDate(utcNow).ToString(), ClaimValueTypes.String, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore).ToString(), ClaimValueTypes.Integer64, Default.Issuer, Default.Issuer),
                new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires).ToString(), ClaimValueTypes.Integer64, Default.Issuer, Default.Issuer),
            });

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Claims = payload.ToObject<Dictionary<string, object>>()
            };

            var jwtFromJObject = jsonWebTokenHandler.CreateToken(payload.ToString());
            var jwtFromDictionary = jsonWebTokenHandler.CreateToken(securityTokenDescriptor);
            var jwtFromSubject = jsonWebTokenHandler.CreateToken(
                new SecurityTokenDescriptor
                {
                    Subject = payloadClaimsIdentity
                });

            var jsonWebTokenFromPayload = new JsonWebToken(jwtFromJObject);
            var jsonWebTokenFromDictionary = new JsonWebToken(jwtFromDictionary);
            var jsonWebTokenFromSubject = new JsonWebToken(jwtFromSubject);

            if (!IdentityComparer.AreEqual(payloadClaimsIdentity.Claims, jsonWebTokenFromPayload.Claims, context))
            {
                context.AddDiff("payloadClaimsIdentity.Claims != jsonWebTokenFromPayload.Claims");
                context.AddDiff("**************************************************************");
            }

            if (!IdentityComparer.AreEqual(payloadClaimsIdentity.Claims, jsonWebTokenFromDictionary.Claims, context))
            {
                context.AddDiff("payloadClaimsIdentity.Claims != jsonWebTokenFromDictionary.Claims");
                context.AddDiff("**************************************************************");
            }

            context.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>> { { typeof(JsonWebToken), new List<string> { "EncodedPayload", "EncodedToken" } } };
            if (!IdentityComparer.AreEqual(jsonWebTokenFromPayload, jsonWebTokenFromDictionary, context))
            {
                context.AddDiff("jsonWebTokenFromPayload != jsonWebTokenFromDictionary");
                context.AddDiff("*****************************************************");
            }

            if (!IdentityComparer.AreEqual(jsonWebTokenFromPayload, jsonWebTokenFromSubject, context))
            {
                context.AddDiff("jsonWebTokenFromPayload != jsonWebTokenFromSubject");
                context.AddDiff("**************************************************");
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the token payload retrieved from ValidateToken is the same as the payload
        // the token was initially created with. 
        [Fact]
        public void RoundTripJWS()
        {
            TestUtilities.WriteHeader($"{this}.RoundTripToken");
            var context = new CompareContext();

            var tokenHandler = new JsonWebTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = Default.Audience,
                ValidateLifetime = false,
                ValidIssuer = Default.Issuer,
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };

            string jwtString = tokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var tokenValidationResult = tokenHandler.ValidateTokenAsync(jwtString, tokenValidationParameters).Result;
            var validatedToken = tokenValidationResult.SecurityToken as JsonWebToken;
            var claimsIdentity = tokenValidationResult.ClaimsIdentity;
            IdentityComparer.AreEqual(Default.PayloadClaimsIdentity, claimsIdentity, context);
            IdentityComparer.AreEqual(Default.PayloadString, Base64UrlEncoder.Decode(validatedToken.EncodedPayload), context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(RoundTripJWEDirectTestCases))]
        public void RoundTripJWEInnerJWSDirect(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEInnerJWSDirect", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var innerJwt = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials);
            var jweCreatedInMemory = jsonWebTokenHandler.EncryptToken(innerJwt, theoryData.EncryptingCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var tokenValidationResult = jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResult.IsValid, theoryData.IsValid, context);
                if (tokenValidationResult.Exception != null)
                    throw tokenValidationResult.Exception;

                var outerToken = tokenValidationResult.SecurityToken as JsonWebToken;

                Assert.True(outerToken != null, "ValidateToken should not return a null token for the JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken, theoryData.TestId);

                Assert.True(outerToken.InnerToken != null, "ValidateToken should not return a null token for the inner JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken.InnerToken, theoryData.TestId);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(RoundTripJWEDirectTestCases))]
        public void RoundTripJWEDirect(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEDirect", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            var jweCreatedInMemory = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var tokenValidationResult = jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResult.IsValid, theoryData.IsValid, context);
                if (tokenValidationResult.Exception != null)
                    throw tokenValidationResult.Exception;

                var outerToken = tokenValidationResult.SecurityToken as JsonWebToken;
                var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jweCreatedInMemory, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);

                IdentityComparer.AreEqual((validatedTokenFromJwtHandler as JwtSecurityToken).Claims, outerToken.Claims, context);
                IdentityComparer.AreEqual(claimsPrincipal.Identity, tokenValidationResult.ClaimsIdentity, context);

                Assert.True(outerToken != null, "ValidateToken should not return a null token for the JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken, theoryData.TestId);

                Assert.True(outerToken.InnerToken != null, "ValidateToken should not return a null token for the inner JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken.InnerToken, theoryData.TestId);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> RoundTripJWEDirectTestCases
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData()
                    {
                        First = true,
                        TestId = "RoundTripJWEValid",
                        ValidationParameters = Default.SymmetricEncryptSignTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = Default.SymmetricEncryptingCredentials
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId =  "SigningKey-Not-Found",
                        IsValid = false,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = NotDefault.SymmetricSigningKey256,
                            TokenDecryptionKey = Default.SymmetricEncryptionKey256,
                        },
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                        ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:")
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "EncryptionKey-Not-Found",
                        IsValid = false,
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = Default.SymmetricSigningKey256,
                            TokenDecryptionKey = NotDefault.SymmetricEncryptionKey,
                        },
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10603:")
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "EncryptionAlgorithmNotSupported",
                        IsValid = false,
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = Default.TokenValidationParameters(new EncryptingCredentials(
                            KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes256CbcHmacSha512).Key,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key),
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10619:")
                    },
                };
            }
        }

        [Theory, MemberData(nameof(RoundTripJWEKeyWrapTestCases))]
        public void RoundTripJWEInnerJWSKeyWrap(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEInnerJWSKeyWrap", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var innerJws = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var jweCreatedInMemory = jsonWebTokenHandler.EncryptToken(innerJws, theoryData.EncryptingCredentials);
                var tokenValidationResult = jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResult.IsValid, theoryData.IsValid, context);
                if (tokenValidationResult.Exception != null)
                    throw tokenValidationResult.Exception;

                var outerToken = tokenValidationResult.SecurityToken as JsonWebToken;

                Assert.True(outerToken != null, "ValidateToken should not return a null token for the JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken, theoryData.TestId);

                Assert.True(outerToken.InnerToken != null, "ValidateToken should not return a null token for the inner JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken.InnerToken, theoryData.TestId);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(RoundTripJWEKeyWrapTestCases))]
        public void RoundTripJWEKeyWrap(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEKeyWrap", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var jweCreatedInMemory = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials);
                var tokenValidationResult = jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(tokenValidationResult.IsValid, theoryData.IsValid, context);
                if (tokenValidationResult.Exception != null)
                    throw tokenValidationResult.Exception;

                var outerToken = tokenValidationResult.SecurityToken as JsonWebToken;
                var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jweCreatedInMemory, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);

                IdentityComparer.AreEqual((validatedTokenFromJwtHandler as JwtSecurityToken).Claims, outerToken.Claims, context);
                IdentityComparer.AreEqual(claimsPrincipal.Identity, tokenValidationResult.ClaimsIdentity, context);

                Assert.True(outerToken != null, "ValidateToken should not return a null token for the JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken, theoryData.TestId);

                Assert.True(outerToken.InnerToken != null, "ValidateToken should not return a null token for the inner JWE token.");
                TestUtilities.CallAllPublicInstanceAndStaticPropertyGets(outerToken.InnerToken, theoryData.TestId);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> RoundTripJWEKeyWrapTestCases
        {
            get
            {
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "RsaPKCS1_Aes128CbcHmacSha256",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaPKCS1_Aes192CbcHmacSha384",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes192CbcHmacSha384)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaPKCS1_Aes256CbcHmacSha512",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes256CbcHmacSha512)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOAEP_Aes128CbcHmacSha256",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOAEP_Aes192CbcHmacSha384",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes192CbcHmacSha384)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOAEP_Aes256CbcHmacSha512",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOaepKeyWrap_Aes128CbcHmacSha256",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOaepKeyWrap, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOaepKeyWrap_Aes192CbcHmacSha384",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOaepKeyWrap, SecurityAlgorithms.Aes192CbcHmacSha384)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "RsaOaepKeyWrap_Aes256CbcHmacSha512",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.RsaSecurityKey_2048, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaOaepKeyWrap, SecurityAlgorithms.Aes256CbcHmacSha512)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "Aes128KeyWrap_Aes128CbcHmacSha256",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.SymmetricSecurityKey2_128, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.SymmetricSecurityKey2_128, SecurityAlgorithms.Aes128KeyWrap, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "Aes256KeyWrap_Aes128CbcHmacSha256",
                        ValidationParameters = Default.TokenValidationParameters(KeyingMaterial.SymmetricSecurityKey2_128, Default.SymmetricSigningKey256),
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.SymmetricSecurityKey2_128, SecurityAlgorithms.Aes256KeyWrap, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = ExpectedException.SecurityTokenKeyWrapException("IDX10662:")
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "Aes128KW_Aes128CbcHmacSha256",
                        ValidationParameters = Default.SymmetricEncryptSignTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = ExpectedException.SecurityTokenKeyWrapException("IDX10662:")
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "Aes256KW_Aes128CbcHmacSha256",
                        ValidationParameters = Default.SymmetricEncryptSignTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = new EncryptingCredentials(Default.SymmetricEncryptionKey256, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                };
            }
        }

        // Test checks to make sure that default times are correctly added to the token
        // upon token creation.
        [Fact (Skip = "Rewrite test to use claims, string will not succeed")]
        public void SetDefaultTimesOnTokenCreation()
        {
            // when the payload is passed as a string to JsonWebTokenHandler.CreateToken, we no longer
            // crack the string and add times {exp, iat, nbf}
            TestUtilities.WriteHeader($"{this}.SetDefaultTimesOnTokenCreation");
            var context = new CompareContext();

            var tokenHandler7 = new JsonWebTokenHandler();
            var tokenHandler6 = new JsonWebTokenHandler6x();
            var payloadWithoutTimeValues = new JObject()
            {
                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                { JwtRegisteredClaimNames.GivenName, "Bob" },
                { JwtRegisteredClaimNames.Iss, Default.Issuer },
                { JwtRegisteredClaimNames.Aud, Default.Audience },
            }.ToString(Formatting.None);

            var jwtString7 = tokenHandler7.CreateToken(payloadWithoutTimeValues, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jwt7 = new JsonWebToken(jwtString7);

            var jwtString6 = tokenHandler6.CreateToken(payloadWithoutTimeValues, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jwt6 = new JsonWebToken(jwtString6);

            if (!IdentityComparer.AreEqual(jwt7, jwt6, context))
            {
                context.AddDiff("jwt7 != jwt6");
                context.AddDiff("********************************************");
            }

            // DateTime.MinValue is returned if the value of a DateTime claim is not found in the payload
            if (DateTime.MinValue.Equals(jwt7.IssuedAt))
                context.AddDiff("DateTime.MinValue.Equals(jwt.IssuedAt). Value for the 'iat' claim not found in the payload.");
            if (DateTime.MinValue.Equals(jwt7.ValidFrom))
                context.AddDiff("DateTime.MinValue.Equals(jwt.ValidFrom). Value for the 'nbf' claim not found in the payload.");
            if (DateTime.MinValue.Equals(jwt7.ValidTo))
                context.AddDiff("DateTime.MinValue.Equals(jwt.ValidTo). Value for the 'exp' claim not found in the payload.");

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that an access token can be successfully validated by the JsonWebTokenHandler.
        // Also ensures that a non-standard claim can be successfully retrieved from the payload and validated.
        [Fact]
        public void ValidateTokenClaims()
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenClaims");

            var tokenHandler = new JsonWebTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            // similar to: "eyJhbGciOiJSUzI1NiIsImtpZCI6IlJzYVNlY3VyaXR5S2V5XzIwNDgiLCJ0eXAiOiJKV1QifQ.eyJlbWFpbCI6IkJvYkBjb250b3NvLmNvbSIsImdpdmVuX25hbWUiOiJCb2IiLCJpc3MiOiJodHRwOi8vRGVmYXVsdC5Jc3N1ZXIuY29tIiwiYXVkIjoiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwibmJmIjoiMTQ4OTc3NTYxNyIsImV4cCI6IjE2MTYwMDYwMTcifQ.GcIi6FGp1JS5VF70_ULa8g6GTRos9Y7rUZvPAo4hm10bBNfGhdd5uXgsJspiQzS8vwJQyPlq8a_BpL9TVKQyFIRQMnoZWe90htmNWszNYbd7zbLJZ9AuiDqDzqzomEmgcfkIrJ0VfbER57U46XPnUZQNng2XgMXrXmIKUqEph_vLGXYRQ4ndfwtRrR6BxQFd1PS1T5KpEoUTusI4VEsMcutzfXUygLDiRKIcnLFA0kQpeoHllO4Nb_Sxv63GCb0d1076FfSEYtyRxF4YSCz1In-ee5dwEK8Mw3nHscu-1hn0Fe98RBs-4OrUzI0WcV8mq9IIB3i-U-CqCJEP_hVCiA";

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://Default.Audience.com",
                ValidateLifetime = false,
                ValidIssuer = "http://Default.Issuer.com",
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };
            var tokenValidationResult = tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters).Result;
            var jsonWebToken = tokenValidationResult.SecurityToken as JsonWebToken;
            var email = jsonWebToken.GetPayloadValue<string>(JwtRegisteredClaimNames.Email);

            if (!email.Equals("Bob@contoso.com"))
                throw new SecurityTokenException("Token does not contain the correct value for the 'email' claim.");
        }


        [Theory, MemberData(nameof(ValidateTypeTheoryData))]
        public void ValidateType(JwtTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateType", theoryData);

            var tokenValidationResult = new JsonWebTokenHandler().ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
            if (tokenValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(tokenValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            Assert.Equal(theoryData.TokenTypeHeader, tokenValidationResult.TokenType);
        }

        public static TheoryData<JwtTheoryData> ValidateTypeTheoryData = JwtSecurityTokenHandlerTests.ValidateTypeTheoryData;

        [Theory, MemberData(nameof(ValidateJweTestCases))]
        public void ValidateJWE(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWE", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test creates a JWT with every mapped claim and then checks that the result of validation from the
        // JwtSecurityTokenHandler and JsonWebTokenHandler are the same, both in the mapped and unmapped case.
        [Fact]
        public async Task ValidateJsonWebTokenClaimMapping()
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler() { MapInboundClaims = false };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadAllShortClaims),
                SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
            };

            var accessToken = jsonWebTokenHandler.CreateToken(tokenDescriptor);

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = Default.Audience,
                ValidIssuer = Default.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true),
                RequireExpirationTime = false,
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler() { MapInboundClaims = false };

            TokenValidationResult jsonValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(accessToken, validationParameters);
            TokenValidationResult jwtValidationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

            var context = new CompareContext
            {
                PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    { typeof(TokenValidationResult),  new List<string> { "SecurityToken", "TokenType" } }
                }
            };

            if(jsonValidationResult.IsValid && jwtValidationResult.IsValid)
            {
                if(!IdentityComparer.AreEqual(jsonValidationResult, jwtValidationResult, context))
                {
                    context.AddDiff("jsonValidationResult.IsValid && jwtValidationResult.IsValid, Validation results are not equal");
                }
            }

            jsonWebTokenHandler.MapInboundClaims = true;
            jwtSecurityTokenHandler.MapInboundClaims = true;

            jsonValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(accessToken, validationParameters);
            jwtValidationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

            if (jsonValidationResult.IsValid && jwtValidationResult.IsValid)
            {
                if (!IdentityComparer.AreEqual(jsonValidationResult, jwtValidationResult, context))
                {
                    context.AddDiff("jsonValidationResult.IsValid && jwtValidationResult.IsValid, Validation results are not equal");
                }
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test shows if the JwtSecurityTokenHandler has mapping OFF and 
        // the JsonWebTokenHandler has mapping ON,the claims are different.
        [Fact]
        public async Task ValidateDifferentClaimsBetweenHandlers()
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler() { MapInboundClaims = true };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadAllShortClaims),
                SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
            };

            var accessToken = jsonWebTokenHandler.CreateToken(tokenDescriptor);

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = Default.Audience,
                ValidIssuer = Default.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true),
                RequireExpirationTime = false,
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler() { MapInboundClaims = false };

            TokenValidationResult jsonValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(accessToken, validationParameters);
            TokenValidationResult jwtValidationResult = await jwtSecurityTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

            var context = new CompareContext();

            if (jsonValidationResult.IsValid && jwtValidationResult.IsValid)
            {
                if (IdentityComparer.AreEqual(jsonValidationResult.Claims, jwtValidationResult.Claims, CompareContext.Default))
                {
                    context.AddDiff("jsonValidationResult.IsValid && jwtValidationResult.IsValid, Claims between validation results are equal");
                }
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ValidateJweTestCases))]
        public async Task ValidateJWEAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEAsync", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(theoryData.Token);
                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.ValidationParameters).ConfigureAwait(false);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).ConfigureAwait(false);
                IdentityComparer.AreEqual(validationResult, rawTokenValidationResult, context);

                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ValidateJweTestCases
        {
            get
            {
                var handlerWithNoDefaultTimes = new JsonWebTokenHandler();
                handlerWithNoDefaultTimes.SetDefaultTimesOnTokenCreation = false;
                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        TestId = "JWE_AcceptedAlgorithmsValidator_DoesNotValidate",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(false)
                        },
                        ExpectedException = new ExpectedException(typeof(SecurityTokenDecryptionFailedException), "IDX10697"),
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_AcceptedAlgorithms_AlgorithmsNotInList",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256 }
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10696:")
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_NoGivenAcceptedAlgorithms",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_AcceptedAlgorithms_AlgorithmsInList",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.HmacSha256Signature }
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_AcceptedAlgorithmsValidator_Validates",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true)
                        },
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateJwsTestCases))]
        public void ValidateJWSAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSAsync", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                var rawTokenValidationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                IdentityComparer.AreEqual(validationResult, rawTokenValidationResult, context);

                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ValidateJwsTestCases))]
        public void ValidateJWS(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWS", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult =handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ValidateJwsTestCases
        {
            get
            {
                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData("SymmetricJwsWithNoKid_RequireSignedTokens_NoKid_WrongSigningKey")
                    {
                        Token = Default.SymmetricJwsWithNoKid,
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10503"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = Default.SymmetricSigningKey1024,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("AsymmetricJws_RequireSignedTokens")
                    {
                        Token = Default.AsymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = Default.AsymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("SymmetricJws_RequireSignedTokens_KeyNotFound")
                    {
                        Token = Default.SymmetricJws,
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500"),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = Default.AsymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            TryAllIssuerSigningKeys = false,
                        }
                    },
                    new JwtTheoryData("SymmetricJws_RequireSignedTokens")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("SymmetricJws_RequireSignedTokensNullSigningKey")
                    {
                        ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10500:"),
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = null,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("SymmetricJws_DontRequireSignedTokens")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("UnsignedJwt_DontRequireSignedTokensNullSigningKey")
                    {
                        Token = Default.UnsignedJwt,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = null,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                        }
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithms_AlgorithmInList")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 }
                        }
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithms_EmptyList")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string>()
                        }
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithms_AlgorithmNotInList")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha256 }
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511")
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithmValidator_Validates")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true)
                        }
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithmValidator_DoesNotValidate")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(false)
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511")
                    },
                    new JwtTheoryData("SymmetricJws_SpecifyAcceptedAlgorithmValidator_Throws")
                    {
                        Token = Default.SymmetricJws,
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = false,
                            IssuerSigningKey = Default.SymmetricSigningKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            AlgorithmValidator = (alg, key, token, validationParameters) => throw new TestException("expected error validating algorithm")
                        },
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10511")
                    },
                    new JwtTheoryData("JWS_NoExp")
                    {
                        Token = (new JsonWebTokenHandler(){SetDefaultTimesOnTokenCreation = false }).CreateToken(new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionary.RemoveClaim(JwtRegisteredClaimNames.Exp)
                        }),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedException = ExpectedException.SecurityTokenNoExpirationException("IDX10225:")
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateJwsWithConfigTheoryData))]
        public void ValidateJWSWithConfig(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithConfig", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                if (validationResult.IsValid)
                {
                    if (theoryData.ShouldSetLastKnownConfiguration && theoryData.ValidationParameters.ConfigurationManager.LastKnownGoodConfiguration == null)
                        context.AddDiff("validationResult.IsValid, but the configuration was not set as the LastKnownGoodConfiguration");
                }
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ValidateJwsWithConfigTheoryData))]
        public async Task ValidateJWSWithConfigAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithConfigAsync", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(theoryData.Token);
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.ValidationParameters).ConfigureAwait(false);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).ConfigureAwait(false);
                IdentityComparer.AreEqual(validationResult, rawTokenValidationResult, context);

                if (validationResult.IsValid)
                {
                    if (theoryData.ShouldSetLastKnownConfiguration && theoryData.ValidationParameters.ConfigurationManager.LastKnownGoodConfiguration == null)
                        context.AddDiff("validationResult.IsValid, but the configuration was not set as the LastKnownGoodConfiguration");
                }
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ValidateJwsWithConfigTheoryData
        {
            get
            {
                var theoryData = new TheoryData<JwtTheoryData>();
                foreach (var sharedTheoryData in JwtTestDatasets.ValidateJwsWithConfigTheoryData)
                    theoryData.Add(sharedTheoryData);

                var incorrectSigningKeysConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
                incorrectSigningKeysConfig.SigningKeys.Add(KeyingMaterial.X509SecurityKey2);
                theoryData.Add(new JwtTheoryData
                {
                    TestId = nameof(Default.AsymmetricJws) + "_" + "TVPInvalid" + "_" + "ConfigSigningKeysInvalid" + "_SignatureValidatorReturnsValidToken",
                    Token = Default.AsymmetricJws,
                    ValidationParameters = new TokenValidationParameters
                    {
                        ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(incorrectSigningKeysConfig),
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        SignatureValidatorUsingConfiguration = (token, validationParameters, configuration) => { return new JsonWebToken(Default.AsymmetricJwt) { SigningKey = KeyingMaterial.DefaultX509Key_2048 }; },
                    },
                });

                return theoryData;
            }
        }
        [Theory, MemberData(nameof(ValidateJwsWithLastKnownGoodTheoryData))]
        public void ValidateJWSWithLastKnownGood(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithLastKnownGood", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ValidateJwsWithLastKnownGoodTheoryData => JwtTestDatasets.ValidateJwsWithLastKnownGoodTheoryData;

        [Theory, MemberData(nameof(ValidateJWEWithLastKnownGoodTheoryData))]
        public void ValidateJWEWithLastKnownGood(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEWithLastKnownGood", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ValidateJWEWithLastKnownGoodTheoryData => JwtTestDatasets.ValidateJWEWithLastKnownGoodTheoryData;

        [Theory, MemberData(nameof(JWECompressionTheoryData))]
        public void EncryptExistingJWSWithCompressionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EncryptExistingJWSWithCompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                string innerJwt;
                if (theoryData.SigningCredentials != null)
                    innerJwt = handler.CreateToken(theoryData.Payload, theoryData.SigningCredentials);
                else
                    innerJwt = handler.CreateToken(theoryData.Payload);

                var jwtToken = handler.EncryptToken(innerJwt, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm);
                var validationResult = handler.ValidateTokenAsync(jwtToken, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                    throw validationResult.Exception;

                IdentityComparer.AreEqual(theoryData.Payload, Base64UrlEncoder.Decode((validationResult.SecurityToken as JsonWebToken).InnerToken.EncodedPayload), context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(JWECompressionTheoryData))]
        public void JWECompressionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JWECompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                string jwtToken;
                if (theoryData.SigningCredentials == null)
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm);
                else if (theoryData.AdditionalHeaderClaims != null)
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm, theoryData.AdditionalHeaderClaims);
                else
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm);

                var validationResult = handler.ValidateTokenAsync(jwtToken, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                    throw validationResult.Exception;

                IdentityComparer.AreEqual(theoryData.Payload, Base64UrlEncoder.Decode((validationResult.SecurityToken as JsonWebToken).InnerToken.EncodedPayload), context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> JWECompressionTheoryData
        {
            get
            {
                var compressionProviderFactoryForCustom = new CompressionProviderFactory
                {
                    CustomCompressionProvider = new SampleCustomCompressionProvider("MyAlgorithm")
                };

                var compressionProviderFactoryForCustom2 = new CompressionProviderFactory
                {
                    CustomCompressionProvider = new SampleCustomCompressionProviderDecompressAndCompressAlwaysFail("MyAlgorithm")
                };

                var tokenValidationParametersRequireSignedTokensFalse = Default.TokenValidationParameters(KeyingMaterial.DefaultX509Key_2048, KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key);
                tokenValidationParametersRequireSignedTokensFalse.ValidateLifetime = false;
                tokenValidationParametersRequireSignedTokensFalse.RequireSignedTokens = false;

                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData()
                    {
                        First = true,
                        TestId = "ValidAlgorithm",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "ValidAlgorithmWithAdditionalHeaderClaims",
                        AdditionalHeaderClaims = new Dictionary<string, object> () { { "int", 123 }, { "string", "string" } },
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "InvalidAlgorithm",
                        CompressionAlgorithm = "UNSUPPORTED",
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = new ExpectedException(typeof(SecurityTokenCompressionFailedException), "IDX10680:", typeof(NotSupportedException))
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "NullCompressionProviderFactory",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = null,
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "NullSigningCredentialsRequireSignedTokensFalse",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = tokenValidationParametersRequireSignedTokensFalse,
                        Payload = Default.PayloadString,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "NullSigningCredentialsRequireSignedTokensTrue",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = new CompressionProviderFactory(),
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10504:")
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "CustomCompressProviderSucceeds",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = compressionProviderFactoryForCustom,
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                    },
                    new CreateTokenTheoryData()
                    {
                        TestId = "CustomCompressionProviderFails",
                        CompressionAlgorithm = CompressionAlgorithms.Deflate,
                        CompressionProviderFactory = compressionProviderFactoryForCustom2,
                        ValidationParameters = Default.JWECompressionTokenValidationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                        EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                        ExpectedException = new ExpectedException(typeof(SecurityTokenCompressionFailedException), "IDX10680:", typeof(InvalidOperationException))
                    },
                };
            }
        }

        [Theory, MemberData(nameof(JWEDecompressionTheoryData))]
        public void JWEDecompressionTest(JWEDecompressionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JWEDecompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                var validationResult = handler.ValidateTokenAsync(theoryData.JWECompressionString, theoryData.ValidationParameters).Result;
                var validatedToken = validationResult.SecurityToken as JsonWebToken;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }

                if (validationResult.IsValid)
                {
                    if (!validatedToken.Claims.Any())
                        context.Diffs.Add("validatedToken.Claims is empty.");
                }
                else
                {
                    theoryData.ExpectedException.ProcessException(validationResult.Exception, context);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JWEDecompressionTheoryData> JWEDecompressionTheoryData()
        {
            var compressionProviderFactoryForCustom = new CompressionProviderFactory()
            {
                CustomCompressionProvider = new SampleCustomCompressionProvider("MyAlgorithm")
            };

            var compressionProviderFactoryForCustom2 = new CompressionProviderFactory()
            {
                CustomCompressionProvider = new SampleCustomCompressionProviderDecompressAndCompressAlwaysFail("MyAlgorithm")
            };

            var tokenValidationParametersRequireSignedTokensFalse = Default.TokenValidationParameters(KeyingMaterial.DefaultX509Key_2048, KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key);
            tokenValidationParametersRequireSignedTokensFalse.ValidateLifetime = false;
            tokenValidationParametersRequireSignedTokensFalse.RequireSignedTokens = false;

            return new TheoryData<JWEDecompressionTheoryData>() {
                new JWEDecompressionTheoryData
                {
                    First = true,
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithDEF,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    TestId = "ValidAlgorithm"
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = tokenValidationParametersRequireSignedTokensFalse,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithUnsignedInnerJWS,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    TestId = "ValidAlgorithmUnsignedInnerJWSRequireSignedTokensFalse"
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithUnsignedInnerJWS,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    TestId = "ValidAlgorithmUnsignedInnerJWSRequireSignedTokensTrue",
                    ExpectedException = ExpectedException.SecurityTokenInvalidSignatureException("IDX10504:")
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithUnsupportedAlgorithm,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    TestId = "InvalidAlgorithm",
                    ExpectedException = new ExpectedException(typeof(SecurityTokenDecompressionFailedException), "IDX10679:", typeof(NotSupportedException))
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWEInvalidCompressionTokenWithDEF,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    TestId = "InvalidToken",
                    ExpectedException = new ExpectedException(typeof(SecurityTokenDecompressionFailedException), "IDX10679:", typeof(InvalidDataException))
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithDEF,
                    CompressionProviderFactory = null,
                    TestId = "NullCompressionProviderFactory",
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    CompressionProviderFactory = compressionProviderFactoryForCustom,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithCustomAlgorithm,
                    TestId = "CustomCompressionProviderSucceeds"
                },
                new JWEDecompressionTheoryData
                {
                    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    JWECompressionString = ReferenceTokens.JWECompressionTokenWithDEF,
                    CompressionProviderFactory = compressionProviderFactoryForCustom2,
                    TestId = "CustomCompressionProviderFails",
                    ExpectedException = new ExpectedException(typeof(SecurityTokenDecompressionFailedException), "IDX10679:", typeof(SecurityTokenDecompressionFailedException))
                }
            };
        }

        [Theory, MemberData(nameof(SecurityKeyNotFoundExceptionTestTheoryData))]
        public void SecurityKeyNotFoundExceptionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SecurityKeyNotFoundExceptionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                var validationResult = handler.ValidateTokenAsync(token, theoryData.ValidationParameters).Result;
                if (validationResult.Exception != null)
                {
                    if (validationResult.IsValid)
                        context.AddDiff("validationResult.IsValid, validationResult.Exception != null");

                    throw validationResult.Exception;
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> SecurityKeyNotFoundExceptionTestTheoryData()
        {
            return new TheoryData<CreateTokenTheoryData>()
            {
                new CreateTokenTheoryData
                {
                    First = true,
                    TestId = "TokenExpired",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaimsExpired),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer
                    },
                    ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "InvalidIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                    },
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10204:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "InvalidIssuerAndExpired",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaimsExpired),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                    },
                    ExpectedException = ExpectedException.SecurityTokenExpiredException("IDX10223:")
                },
                new CreateTokenTheoryData
                {
                    TestId = "KeysDontMatch-ValidLifeTimeAndIssuer",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaims),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                        ValidateAudience = false
                    },
                    ExpectedException = ExpectedException.SecurityTokenSignatureKeyNotFoundException("IDX10503:")
                },
            };
        }

        [Theory, MemberData(nameof(IncludeSecurityTokenOnFailureTestTheoryData))]
        public void IncludeSecurityTokenOnFailedValidationTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.IncludeSecurityTokenOnFailedValidationTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                var validationResult = handler.ValidateTokenAsync(token, theoryData.ValidationParameters).Result;
                if (theoryData.ValidationParameters.IncludeTokenOnFailedValidation)
                {
                    Assert.NotNull(validationResult.TokenOnFailedValidation);
                }
                else
                {
                    Assert.Null(validationResult.TokenOnFailedValidation);
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> IncludeSecurityTokenOnFailureTestTheoryData()
        {
            return new TheoryData<CreateTokenTheoryData>()
            {
                new CreateTokenTheoryData
                {
                    First = true,
                    TestId = "TokenExpiredIncludeTokenOnFailedValidation",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaimsExpired),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                        IncludeTokenOnFailedValidation = true
                    }
                },
                new CreateTokenTheoryData
                {
                    First = true,
                    TestId = "TokenExpiredNotIncludeTokenOnFailedValidation",
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(Default.PayloadClaimsExpired),
                        Expires = DateTime.UtcNow.Subtract(new TimeSpan(0, 10, 0)),
                        IssuedAt = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        NotBefore = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0)),
                        SigningCredentials = Default.AsymmetricSigningCredentials,
                    },
                    ValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey,
                        ValidIssuer = Default.Issuer,
                    }
                },
            };
        }
    }

    public class CreateTokenTheoryData : TheoryDataBase
    {
        public CreateTokenTheoryData()
        {
        }

        public CreateTokenTheoryData(string testId)
        {
            TestId = testId;
        }

        public Dictionary<string, object> AdditionalHeaderClaims { get; set; }

        public string Payload { get; set; }

        public string CompressionAlgorithm { get; set; }

        public BaseConfiguration Configuration { get; set; }

        public CompressionProviderFactory CompressionProviderFactory { get; set; }

        public EncryptingCredentials EncryptingCredentials { get; set; }

        public bool IsValid { get; set; } = true;

        public SigningCredentials SigningCredentials { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public SecurityTokenDescriptor TokenDescriptor6x { get; set; }

        public JsonWebTokenHandler JsonWebTokenHandler { get; set; }

        public JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }

        public string JwtToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public string Algorithm { get; set; }

        public IEnumerable<SecurityKey> ExpectedDecryptionKeys { get; set; }
    }

    // Overrides CryptoProviderFactory.CreateAuthenticatedEncryptionProvider to create AuthenticatedEncryptionProviderMock that provides AesGcm encryption.
    public class CryptoProviderFactoryMock: CryptoProviderFactory
    {
        public override AuthenticatedEncryptionProvider CreateAuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
        {
            if (SupportedAlgorithms.IsSupportedEncryptionAlgorithm(algorithm, key) && SupportedAlgorithms.IsAesGcm(algorithm))
                return new AuthenticatedEncryptionProviderMock(key, algorithm);

            return null;
        }
    }

    // Overrides AuthenticatedEncryptionProvider.Encrypt to offer AesGcm encryption for testing.
    public class AuthenticatedEncryptionProviderMock: AuthenticatedEncryptionProvider
    {
        public AuthenticatedEncryptionProviderMock(SecurityKey key, string algorithm): base(key, algorithm)
        { }

        public override AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData)
        {
            byte[] nonce = new byte[Tokens.AesGcm.NonceSize];

            // Generate random nonce
            var random = RandomNumberGenerator.Create();
            random.GetBytes(nonce);

            return Encrypt(plaintext, authenticatedData, nonce);
        }

        public override AuthenticatedEncryptionResult Encrypt(byte[] plaintext, byte[] authenticatedData, byte[] iv)
        {
            byte[] authenticationTag = new byte[Tokens.AesGcm.TagSize];
            byte[] ciphertext = new byte[plaintext.Length];

            using (var aes = new Tokens.AesGcm(GetKeyBytes(Key)))
            {
                aes.Encrypt(iv, plaintext, ciphertext, authenticationTag, authenticatedData);
            }

            return new AuthenticatedEncryptionResult(Key, ciphertext, iv, authenticationTag); 
        }
    }

    public class DerivedJsonWebTokenHandler : JsonWebTokenHandler
    {
        /// <summary>
        /// Creates a <see cref="ClaimsIdentity"/> from a <see cref="JsonWebToken"/>.
        /// </summary>
        /// <param name="jwtToken">The <see cref="JsonWebToken"/> to use as a <see cref="Claim"/> source.</param>
        /// <param name="validationParameters">Contains parameters for validating the token.</param>
        /// <param name="issuer">Specifies the issuer for the <see cref="ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the <see cref="JsonWebToken.Claims"/>.</returns>
        protected override ClaimsIdentity CreateClaimsIdentity(JsonWebToken jwtToken, TokenValidationParameters validationParameters, string issuer)
        {
            return base.CreateClaimsIdentity(jwtToken, validationParameters, issuer);
        }
    }
}
