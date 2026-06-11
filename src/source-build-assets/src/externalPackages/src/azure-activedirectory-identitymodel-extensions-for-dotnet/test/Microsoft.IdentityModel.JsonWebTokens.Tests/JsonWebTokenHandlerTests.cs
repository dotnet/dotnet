// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt.Tests;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.IdentityModel.Tokens.Json;
using Microsoft.IdentityModel.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenHandlerTests
    {
        [Fact]
        public void JsonWebTokenHandler_CreateToken_SameTypeMultipleValues()
        {
            var identity = new CaseSensitiveClaimsIdentity("Test");

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
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken(null, Default.SymmetricEncryptingCredentials, new Dictionary<string, object> { { "key", "value" } }));
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken("Payload", (EncryptingCredentials)null, new Dictionary<string, object> { { "key", "value" } }));
            Assert.Throws<ArgumentNullException>(() => handler.CreateToken("Payload", Default.SymmetricEncryptingCredentials, (Dictionary<string, object>)null));
        }

        [Theory, MemberData(nameof(TokenValidationClaimsTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenValidationResult(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResult");
            var tokenValidationResult = await theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);
            Assert.Equal(tokenValidationResult.Claims, TokenUtilities.CreateDictionaryFromClaims(tokenValidationResult.ClaimsIdentity.Claims));
        }

        [Theory, MemberData(nameof(TokenValidationClaimsTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenDerivedHandlerValidationResult(JsonWebTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateTokenDerivedHandlerValidationResult", theoryData);
            var derivedJsonWebTokenHandler = new DerivedJsonWebTokenHandler();
            var tokenValidationResult = await theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);
            var tokenValidationDerivedResult = await derivedJsonWebTokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);
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
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadAllShortClaims),
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

        [Theory, MemberData(nameof(TokenValidationTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenValidationResultThrowsWarning(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultThrowsWarning");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = await theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);

            //access claims without checking IsValid or Exception
            var claims = tokenValidationResult.Claims;

            //check if warning message was logged
            var warningId = "IDX10109";
            Assert.Contains(warningId, listener.TraceBuffer);
        }

        [Theory, MemberData(nameof(TokenValidationTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenValidationResultDoesNotThrowWarningWithIsValidRead(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultDoesNotThrowWarningWithIsValidRead");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = await theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);

            //checking IsValid first, then access claims
            var isValid = tokenValidationResult.IsValid;
            var claims = tokenValidationResult.Claims;

            //check if warning message was logged
            var warningId = "IDX10109";
            Assert.DoesNotContain(warningId, listener.TraceBuffer);
        }

        [Theory, MemberData(nameof(TokenValidationTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenValidationResultDoesNotThrowWarningWithExceptionRead(JsonWebTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenValidationResultDoesNotThrowWarningWithExceptionRead");

            //create a listener and enable it for logs
            SampleListener listener = SampleListener.CreateLoggerListener(EventLevel.Warning);

            //validate token
            var tokenValidationResult = await theoryData.TokenHandler.ValidateTokenAsync(theoryData.AccessToken, theoryData.ValidationParameters);

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
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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

        [Theory, MemberData(nameof(SegmentTheoryData), DisableDiscoveryEnumeration = true)]
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

        [Theory, MemberData(nameof(CreateTokenWithEmptyPayloadUsingSecurityTokenDescriptorTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateTokenWithEmptyPayloadUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateEmptyJWSUsingSecurityTokenDescriptor", theoryData);
            try
            {
                string jwtFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);
                var tokenValidationResultFromSecurityTokenDescriptor = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtFromSecurityTokenDescriptor, theoryData.ValidationParameters);
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

        /// <summary>
        /// Verify the results from ValidateToken() and ValidateTokenAsync() should match.
        /// </summary>
        /// <param name="theoryData">The test data.</param>
        [Theory, MemberData(nameof(CreateJWEWithAesGcmTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task TokenValidationResultsShouldMatch(CreateTokenTheoryData theoryData)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Throws<PlatformNotSupportedException>(() => new AuthenticatedEncryptionProvider(Default.SymmetricEncryptionKey256, theoryData.EncryptingCredentials.Enc));
            }
            else
            {
                var context = TestUtilities.WriteHeader($"{this}.TokenValidationResultCompare", theoryData);
                try
                {
                    string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);

                    theoryData.ValidationParameters.ValidateLifetime = false;
                    var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                    var validationResult = await theoryData.JwtSecurityTokenHandler.ValidateTokenAsync(jweFromJwtHandler, theoryData.ValidationParameters);

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
        }

        [Theory, MemberData(nameof(CreateJWEWithAesGcmTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWEWithAesGcm(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEWithAesGcm", theoryData);
            try
            {
                string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jweFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                theoryData.ValidationParameters.ValidateLifetime = false;
                var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                var validationResult = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJsonHandler, theoryData.ValidationParameters);
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
                encryptionCredentials.CryptoProviderFactory = new CryptoProviderFactoryForGcm();
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
        [Theory, MemberData(nameof(CreateJWETheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWE(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWE", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                string jweFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jweFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                var claimsPrincipalFromJwtHandler = theoryData.JwtSecurityTokenHandler.ValidateToken(jweFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedTokenFromJwtHandler);
                var validationResultFromJsonHandler = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJsonHandler, theoryData.ValidationParameters);
                IdentityComparer.AreEqual(validationResultFromJsonHandler.IsValid, theoryData.IsValid, context);

                var validatedTokenFromJsonHandler = validationResultFromJsonHandler.SecurityToken;
                var validationResultFromJwtJsonHandler = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromJwtHandler, theoryData.ValidationParameters);
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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

        [Theory, MemberData(nameof(SecurityTokenDecryptionTheoryData), DisableDiscoveryEnumeration = true)]
        public void GetEncryptionKeys(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.EncryptionKeysCheck", theoryData);
            try
            {
                string jweFromJsonHandlerWithKid = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.EncryptingCredentials);
                var jwtTokenFromJsonHandlerWithKid = new JsonWebToken(jweFromJsonHandlerWithKid);
                var encryptionKeysFromJsonHandlerWithKid = theoryData.JsonWebTokenHandler.GetContentEncryptionKeys(jwtTokenFromJsonHandlerWithKid, theoryData.ValidationParameters, theoryData.Configuration);

                var actualKeys = encryptionKeysFromJsonHandlerWithKid.ToList();

                foreach (var expectedKey in theoryData.ExpectedDecryptionKeys)
                {
                    if (jwtTokenFromJsonHandlerWithKid.Alg.Equals(JwtConstants.DirectKeyUseAlg, StringComparison.Ordinal)
                        || jwtTokenFromJsonHandlerWithKid.Alg.Equals(SecurityAlgorithms.EcdhEs, StringComparison.Ordinal))
                    {
                        // Direct key use — keys are returned as-is, exact match.
                        Assert.Contains(expectedKey, actualKeys);
                    }
                    else
                    {
                        // Key wrapping — returned keys are unwrapped CEKs.
                        // Verify the correct CEK is present by unwrapping ourselves.
                        if (expectedKey.CryptoProviderFactory.IsSupportedAlgorithm(jwtTokenFromJsonHandlerWithKid.Alg, expectedKey))
                        {
                            var kwp = expectedKey.CryptoProviderFactory.CreateKeyWrapProviderForUnwrap(expectedKey, jwtTokenFromJsonHandlerWithKid.Alg);
                            var expectedCekBytes = kwp.UnwrapKey(jwtTokenFromJsonHandlerWithKid.EncryptedKeyBytes);
                            var filteredKeys = actualKeys.Where(k => k is SymmetricSecurityKey sk && sk.Key.SequenceEqual(expectedCekBytes));
                            Assert.Single(filteredKeys);
                        }
                    }
                }
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

                var rsaKey = new RsaSecurityKey(KeyingMaterial.RsaParameters_2048) { KeyId = "CustomRsaSecurityKey_2048" };
                var configurationThatThrows = CreateCustomConfigurationThatThrows(rsaKey);

                var configurationWithMismatchedKeys = new OpenIdConnectConfiguration();
                configurationWithMismatchedKeys.TokenDecryptionKeys.Add(rsaKey);

                tokenHandler.InboundClaimTypeMap.Clear();
                return new TheoryData<CreateTokenTheoryData>
                {
                    new CreateTokenTheoryData
                    {
                        First = true,
                        TestId = "ValidKeyInConfig_KeysSetInConfig",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithDecryptionKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "ValidKeyInConfig_KeysSetInConfigAndTvp",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_512 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithDecryptionKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "ValidKeyInTvp_KeysSetInTvp",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2,
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256, KeyingMaterial.DefaultSymmetricSecurityKey_512 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "AlgorithmMismatch_ReturnsNoKeys",
                        Payload = Default.PayloadString,
                        // There is no error, just no decryption keys are returned.
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        ExpectedDecryptionKeys = new List<SecurityKey>(),
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKeys = new List<SecurityKey>(){ KeyingMaterial.DefaultSymmetricSecurityKey_256 },
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "ValidKeyInConfig_OneKeyThrows_SuccessfulKeyReturned",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationThatThrows,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ rsaKey },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "KeyIdMismatch_TryAllDecryptionKeysTrue_ReturnsKey",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TryAllDecryptionKeys = true,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithMismatchedKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(){ rsaKey },
                    },
                    new CreateTokenTheoryData
                    {
                        TestId = "KeyIdMismatch_TryAllDecryptionKeysFalse_ReturnsNoKeys",
                        Payload = Default.PayloadString,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                            Claims = Default.PayloadDictionary
                        },
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TryAllDecryptionKeys = false,
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer
                        },
                        Configuration = configurationWithMismatchedKeys,
                        ExpectedDecryptionKeys =  new List<SecurityKey>(),
                    }
                };
            }
        }

        private static OpenIdConnectConfiguration CreateCustomConfigurationThatThrows(SecurityKey rsaKey)
        {
            var customCryptoProviderFactory = new DerivedCryptoProviderFactory
            {
                IsSupportedAlgImpl = (alg, key) => key.KeySize == 512,
                CreateKeyWrapProviderForUnwrapImpl = (key, alg) => throw new InvalidOperationException("Test exception")
            };

            var sym512Hey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_512) { KeyId = "CustomSymmetricSecurityKey_512" };
            sym512Hey.CryptoProviderFactory = customCryptoProviderFactory;

            var configurationWithCustomCryptoProviderFactory = new OpenIdConnectConfiguration();
            configurationWithCustomCryptoProviderFactory.TokenDecryptionKeys.Add(rsaKey);
            configurationWithCustomCryptoProviderFactory.TokenDecryptionKeys.Add(sym512Hey);

            return configurationWithCustomCryptoProviderFactory;
        }

        // Tests checks to make sure that the token string (JWE) created by calling 
        // CreateToken(string payload, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
        // is equivalent to the token string created by calling CreateToken(SecurityTokenDescriptor tokenDescriptor).
        [Theory, MemberData(nameof(CreateJWEUsingSecurityTokenDescriptorTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWEUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEUsingSecurityTokenDescriptor", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            if (theoryData.TokenDescriptor != null && !theoryData.AudiencesForSecurityTokenDescriptor.IsNullOrEmpty())
            {
                foreach (var audience in theoryData.AudiencesForSecurityTokenDescriptor)
                    theoryData.TokenDescriptor.Audiences.Add(audience);
            }

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

                var validationResultFromSecurityTokenDescriptor = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromSecurityTokenDescriptor, theoryData.ValidationParameters);
                var validationResultFromString = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jweFromString, theoryData.ValidationParameters);

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
                        TestId = "SecurityTokenDescriptorMultipleAudiences",
                        Payload = Default.PayloadStringMultipleAudiences,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                            Claims = Default.PayloadDictionaryMultipleAudiences
                        },
                        AudiencesForSecurityTokenDescriptor = Default.Audiences,
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                            ValidAudiences = Default.Audiences,
                            ValidIssuer = Default.Issuer
                        },

                        // There is a known difference in the 'Aud' claim between the two tokens. Since the JsonWebTokenHandler
                        // will only include the SecurityTokenDescriptor Audience and Audiences members if one or both are set in
                        // the 'Aud' claim, while the JwtSecurityTokenHandler will also include any aud claims set in either the
                        // Claims or Subject members even if Audience/Audiences is defined (not both, Claims takes precedence).
                        PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                        {
                            { typeof(JsonWebToken), new List<string> {"EncodedPayload", "EncodedSignature"} },
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
        [Theory, MemberData(nameof(CreateJWSTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWS(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWS", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                string jwsFromJwtHandler = theoryData.JwtSecurityTokenHandler.CreateEncodedJwt(theoryData.TokenDescriptor);
                string jwsFromJsonHandler = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);

                var claimsPrincipal = theoryData.JwtSecurityTokenHandler.ValidateToken(jwsFromJwtHandler, theoryData.ValidationParameters, out SecurityToken validatedToken);
                var tokenValidationResult = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwsFromJsonHandler, theoryData.ValidationParameters);
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims)
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims)
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
                            Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims)
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

        // This test checks to make sure that SecurityTokenDescriptor.Audience, Expires, IssuedAt, NotBefore, Issuer have priority over SecurityTokenDescriptor.Claims.
        [Theory, MemberData(nameof(CreateJWSWithSecurityTokenDescriptorClaimsTheoryData), DisableDiscoveryEnumeration = true)]
        public void CreateJWSWithSecurityTokenDescriptorClaims(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWSWithSecurityTokenDescriptorClaims", theoryData);

            var jwtToken = new JsonWebTokenHandler().CreateToken(theoryData.TokenDescriptor);
            JsonWebToken jsonWebToken = new JsonWebToken(jwtToken);

            jsonWebToken.TryGetPayloadValue("iss", out string issuer);
            IdentityComparer.AreEqual(theoryData.ExpectedClaims["iss"], issuer, context);

            jsonWebToken.TryGetPayloadValue("aud", out string audience);
            IdentityComparer.AreEqual(theoryData.ExpectedClaims["aud"], audience, context);

            jsonWebToken.TryGetPayloadValue("exp", out long exp);
            IdentityComparer.AreEqual(theoryData.ExpectedClaims["exp"], exp, context);

            jsonWebToken.TryGetPayloadValue("iat", out long iat);
            IdentityComparer.AreEqual(theoryData.ExpectedClaims["iat"], iat, context);

            jsonWebToken.TryGetPayloadValue("nbf", out long nbf);
            IdentityComparer.AreEqual(theoryData.ExpectedClaims["nbf"], nbf, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> CreateJWSWithSecurityTokenDescriptorClaimsTheoryData
        {
            get
            {
                TheoryData<CreateTokenTheoryData> theoryData = new TheoryData<CreateTokenTheoryData>();

                SigningCredentials signingCredentials = new SigningCredentials(KeyingMaterial.DefaultSymmetricSecurityKey_256, SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Sha256);

                DateTime iat = DateTime.UtcNow;
                DateTime exp = iat + TimeSpan.FromDays(1);
                DateTime nbf = iat + TimeSpan.FromMinutes(1);
                string iss = Guid.NewGuid().ToString();
                string aud = Guid.NewGuid().ToString();

                Dictionary<string, object> claims = new Dictionary<string, object>()
                {
                    { JwtRegisteredClaimNames.Aud, aud },
                    { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(exp) },
                    { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(iat) },
                    { JwtRegisteredClaimNames.Iss, iss},
                    { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(nbf) }
                };

                // These values will be set on the SecurityTokenDescriptor
                DateTime iatSTD = DateTime.UtcNow + TimeSpan.FromHours(1);
                DateTime expSTD = iat + TimeSpan.FromDays(1);
                DateTime nbfSTD = iat + TimeSpan.FromMinutes(1);
                string issSTD = Guid.NewGuid().ToString();
                string audSTD = Guid.NewGuid().ToString();

                theoryData.Add(new CreateTokenTheoryData("ValuesFromClaims")
                {
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = signingCredentials,
                        Claims = claims
                    },
                    ExpectedClaims = claims
                });

                theoryData.Add(new CreateTokenTheoryData("AllValuesFromSTD")
                {
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = signingCredentials,
                        Claims = claims,
                        IssuedAt = iatSTD,
                        Expires = expSTD,
                        NotBefore = nbfSTD,
                        Audience = audSTD,
                        Issuer = issSTD
                    },
                    ExpectedClaims = new Dictionary<string, object>()
                    {
                        { JwtRegisteredClaimNames.Aud, audSTD },
                        { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expSTD) },
                        { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(iatSTD) },
                        { JwtRegisteredClaimNames.Iss, issSTD},
                        { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(nbfSTD) }
                    }
                });

                theoryData.Add(new CreateTokenTheoryData("ExpFromSTD")
                {
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = signingCredentials,
                        Claims = claims,
                        Expires = expSTD
                    },
                    ExpectedClaims = new Dictionary<string, object>()
                    {
                        { JwtRegisteredClaimNames.Aud, aud },
                        { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expSTD) },
                        { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(iat) },
                        { JwtRegisteredClaimNames.Iss, iss},
                        { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(nbf) }
                    }
                });

                theoryData.Add(new CreateTokenTheoryData("IatFromSTD")
                {
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = signingCredentials,
                        Claims = claims,
                        IssuedAt = iatSTD
                    },
                    ExpectedClaims = new Dictionary<string, object>()
                    {
                        { JwtRegisteredClaimNames.Aud, aud },
                        { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(exp) },
                        { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(iatSTD) },
                        { JwtRegisteredClaimNames.Iss, iss},
                        { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(nbf) }
                    }
                });

                theoryData.Add(new CreateTokenTheoryData("NbfFromSTD")
                {
                    TokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = signingCredentials,
                        Claims = claims,
                        NotBefore = nbfSTD
                    },
                    ExpectedClaims = new Dictionary<string, object>()
                    {
                        { JwtRegisteredClaimNames.Aud, aud },
                        { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(exp) },
                        { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(iat) },
                        { JwtRegisteredClaimNames.Iss, iss},
                        { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(nbfSTD) }
                    }
                });

                return theoryData;
            }
        }


        // This test checks to make sure that additional header claims are added as expected to the JWT token header.
        [Theory, MemberData(nameof(CreateJWSWithAdditionalHeaderClaimsTheoryData), DisableDiscoveryEnumeration = true)]
        public void CreateJWSWithAdditionalHeaderClaims(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWSWithAdditionalHeaderClaims", theoryData);
            var jwtToken = new JsonWebTokenHandler().CreateToken(theoryData.TokenDescriptor);
            JsonWebToken jsonWebToken = new JsonWebToken(jwtToken);
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

        [Theory, MemberData(nameof(CreateJWEWithPayloadStringTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWEWithPayloadString(CreateTokenTheoryData theoryData)
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
                    var result = await handler.ValidateTokenAsync(jwtTokenWithSigning, theoryData.ValidationParameters);
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
                        TestId = "JsonPayload_No_Cty",
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
        [Theory, MemberData(nameof(CreateJWEWithAdditionalHeaderClaimsTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWEWithAdditionalHeaderClaims(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWEWithAdditionalHeaderClaims", theoryData);
            var handler = new JsonWebTokenHandler();
            theoryData.ValidationParameters.ValidateLifetime = false;

            var jwtTokenFromDescriptor = handler.CreateToken(theoryData.TokenDescriptor);
            var validatedJwtTokenFromDescriptor = (await handler.ValidateTokenAsync(jwtTokenFromDescriptor, theoryData.ValidationParameters)).SecurityToken as JsonWebToken;
            var jwtTokenToCompare = (await handler.ValidateTokenAsync(theoryData.JwtToken, theoryData.ValidationParameters)).SecurityToken as JsonWebToken;

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
        [Theory, MemberData(nameof(CreateJWSUsingSecurityTokenDescriptorTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task CreateJWSUsingSecurityTokenDescriptor(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJWSUsingSecurityTokenDescriptor", theoryData);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                if (theoryData.TokenDescriptor != null && !theoryData.AudiencesForSecurityTokenDescriptor.IsNullOrEmpty())
                {
                    foreach (var audience in theoryData.AudiencesForSecurityTokenDescriptor)
                        theoryData.TokenDescriptor.Audiences.Add(audience);
                }

                string jwtFromSecurityTokenDescriptor = theoryData.JsonWebTokenHandler.CreateToken(theoryData.TokenDescriptor);
                string jwtPayloadAsString;

                if (theoryData.TokenDescriptor.SigningCredentials == null)
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload);
                else if (theoryData.TokenDescriptor.AdditionalHeaderClaims != null)
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials, theoryData.TokenDescriptor.AdditionalHeaderClaims);
                else
                    jwtPayloadAsString = theoryData.JsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.TokenDescriptor.SigningCredentials);

                var jwsTokenFromSecurityTokenDescriptor = new JsonWebToken(jwtFromSecurityTokenDescriptor);
                var jwsTokenFromString = new JsonWebToken(jwtPayloadAsString);

                var tokenValidationResultFromSecurityTokenDescriptor = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtFromSecurityTokenDescriptor, theoryData.ValidationParameters);
                var tokenValidationResultFromString = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtPayloadAsString, theoryData.ValidationParameters);

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
                CompareContext localContext = new CompareContext(context);

                localContext.PropertiesToIgnoreWhenComparing = new Dictionary<Type, List<string>>
                {
                    {typeof(JsonWebToken), new List<string> {"EncodedHeader", "EncodedToken", "EncodedPayload", "EncodedSignature"}},
                };

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
                            Subject = new CaseSensitiveClaimsIdentity(new List<Claim>()
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
                    new CreateTokenTheoryData("ValidUsingMultipleAudiences")
                    {
                        Payload = Default.PayloadStringMultipleAudiences,
                        TokenDescriptor =  new SecurityTokenDescriptor
                        {
                            SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                            Claims = Default.PayloadDictionaryMultipleAudiences
                        },
                        AudiencesForSecurityTokenDescriptor = Default.Audiences,
                        JsonWebTokenHandler = new JsonWebTokenHandler(),
                        ValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                            ValidAudiences = Default.Audiences,
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
            var payloadClaimsIdentity = new CaseSensitiveClaimsIdentity(new List<Claim>()
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

        [Fact]
        public async Task AdditionalHeaderValues()
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.AdditionalHeaderValues", "AdditionalHeaderValues", false);

            var tokenHandler = new JsonWebTokenHandler();
            List<string> x5cExpected = new List<string>() { KeyingMaterial.DefaultX509Data_2048_Public, KeyingMaterial.X509Data_AAD_Public };
            string jwtString = tokenHandler.CreateToken(
                Default.PayloadString,
                KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                new Dictionary<string, object>() {
                    { JwtHeaderParameterNames.X5c, x5cExpected },
                });

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = Default.Audience,
                ValidateLifetime = false,
                ValidIssuer = Default.Issuer,
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };

            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(jwtString, tokenValidationParameters);
            JsonWebToken validatedToken = tokenValidationResult.SecurityToken as JsonWebToken;

            if (!validatedToken.TryGetHeaderValue(JwtHeaderParameterNames.X5c, out string[] x5cFound))
                context.Diffs.Add("validatedToken.TryGetHeaderValue(JwtHeaderParameterNames.X5c, out string[] x5c) == false");
            else
                IdentityComparer.AreStringEnumsEqual(x5cExpected, x5cFound, context);

            TestUtilities.AssertFailIfErrors(context);
        }


        // Test checks to make sure that the token payload retrieved from ValidateToken is the same as the payload
        // the token was initially created with. 
        [Fact]
        public async Task RoundTripJWS()
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
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(jwtString, tokenValidationParameters);
            var validatedToken = tokenValidationResult.SecurityToken as JsonWebToken;
            IdentityComparer.AreEqual(Default.PayloadClaimsIdentity, tokenValidationResult.ClaimsIdentity, context);
            IdentityComparer.AreEqual(Default.PayloadString, Base64UrlEncoder.Decode(validatedToken.EncodedPayload), context);

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test ensure paths to creating a token with an empty payload are successful.
        [Theory, MemberData(nameof(RoundTripWithEmptyPayloadTestCases), DisableDiscoveryEnumeration = true)]
        public async Task RoundTripWithEmptyPayload(CreateTokenTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.RoundTripWithEmptyPayload");

            var context = new CompareContext();
            var jwtString = JsonWebTokenHandler.CreateToken(
                                    theoryData.Payload,
                                    theoryData.SigningCredentials,
                                    theoryData.EncryptingCredentials,
                                    theoryData.CompressionAlgorithm,
                                    theoryData.AdditionalHeaderClaims,
                                    theoryData.AdditionalInnerHeaderClaims,
                                    "JWT");

            var tokenValidationResult = await theoryData.JsonWebTokenHandler.ValidateTokenAsync(jwtString, theoryData.ValidationParameters);
            var validatedToken = tokenValidationResult.SecurityToken as JsonWebToken;
            var claimsIdentity = tokenValidationResult.ClaimsIdentity;
            IdentityComparer.AreEqual(new CaseSensitiveClaimsIdentity("AuthenticationTypes.Federation"), claimsIdentity, context);
            IdentityComparer.AreEqual("", Base64UrlEncoder.Decode(validatedToken.EncodedPayload), context);

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateTokenTheoryData> RoundTripWithEmptyPayloadTestCases
        {
            get
            {
                TheoryData<CreateTokenTheoryData> theoryData = new TheoryData<CreateTokenTheoryData>();

                Dictionary<string, object> additionalInnerHeaderClaims = new Dictionary<string, object>()
                {
                    { "intInnerHeader", 1233 },
                    { "stringInnerHeader", "stringInnerHeader" }
                };

                Dictionary<string, object> additionalHeaderClaims = new Dictionary<string, object>()
                {
                    { "int", 123 },
                    { "string", "string" }
                };

                EncryptingCredentials encryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256);

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                    RequireSignedTokens = false,
                    TokenDecryptionKey = KeyingMaterial.DefaultX509Key_2048,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuer = false
                };

                theoryData.Add(new CreateTokenTheoryData("EmptyPayload")
                {
                    Payload = "",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("additionalHeaderClaims")
                {
                    AdditionalHeaderClaims = additionalHeaderClaims,
                    Payload = "",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentials")
                {
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsAdditionalHeaderClaims")
                {
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsCompression")
                {
                    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsSigningCredentials")
                {
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsSigningCredentialsAdditionalHeaderClaims")
                {
                    AdditionalHeaderClaims = additionalHeaderClaims,
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsSigningCredentialsCompression")
                {
                    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("encryptingCredentialsSigningCredentialsCompressionAddionalHeaderClaimsAdditionalInnerHeaderClaims")
                {
                    AdditionalInnerHeaderClaims = additionalInnerHeaderClaims,
                    AdditionalHeaderClaims = additionalHeaderClaims,
                    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    EncryptingCredentials = encryptingCredentials,
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("signingCredentials")
                {
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                theoryData.Add(new CreateTokenTheoryData("signingCredentialsAdditionalHeaderClaims")
                {
                    AdditionalHeaderClaims = additionalHeaderClaims,
                    Payload = "",
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                    ValidationParameters = validationParameters
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(RoundTripJWEDirectTestCases), DisableDiscoveryEnumeration = true)]
        public async Task RoundTripJWEInnerJWSDirect(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEInnerJWSDirect", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var innerJwt = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials);
            var jweCreatedInMemory = jsonWebTokenHandler.EncryptToken(innerJwt, theoryData.EncryptingCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(RoundTripJWEDirectTestCases), DisableDiscoveryEnumeration = true)]
        public async Task RoundTripJWEDirect(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEDirect", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            var jweCreatedInMemory = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters);
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
                    GetTokenTheoryDataWithKeyId(),
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

                static CreateTokenTheoryData GetTokenTheoryDataWithKeyId()
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = Default.SymmetricSigningKey256,
                        TokenDecryptionKey = NotDefault.SymmetricSigningKey256,
                    };

                    var keysAttempted = new StringBuilder().AppendLine(validationParameters.TokenDecryptionKey.KeyId);
                    var incompleteExceptionMessage = new MessageDetail(
                                Tokens.LogMessages.IDX10603,
                                LogHelper.MarkAsNonPII(keysAttempted.ToString()),
                                string.Empty,   // Using empty since actual exception contains file paths, which are machine specific.
                                string.Empty)   // Using empty since EncodedToken is not available and the message is getting used partially below.
                        .Message;
                    // Get partial messages as the actual exception message contains file paths.
                    var partialExceptionMessage = incompleteExceptionMessage.Substring(
                        0,
                        incompleteExceptionMessage.IndexOf(validationParameters.TokenDecryptionKey.KeyId) + validationParameters.TokenDecryptionKey.KeyId.Length);

                    return new CreateTokenTheoryData()
                    {
                        TestId = "EncryptionKey-Not-Found-Returns-KeyId-In-Error-Message",
                        IsValid = false,
                        ValidationParameters = validationParameters,
                        Payload = Default.PayloadString,
                        SigningCredentials = Default.SymmetricSigningCredentials,
                        EncryptingCredentials = Default.SymmetricEncryptingCredentials,
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException(partialExceptionMessage)
                    };
                }
            }
        }

        [Theory, MemberData(nameof(RoundTripJWEKeyWrapTestCases), DisableDiscoveryEnumeration = true)]
        public async Task RoundTripJWEInnerJWSKeyWrap(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEInnerJWSKeyWrap", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var innerJws = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials);
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var jweCreatedInMemory = jsonWebTokenHandler.EncryptToken(innerJws, theoryData.EncryptingCredentials);
                var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(RoundTripJWEKeyWrapTestCases), DisableDiscoveryEnumeration = true)]
        public async Task RoundTripJWEKeyWrap(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.RoundTripJWEKeyWrap", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            theoryData.ValidationParameters.ValidateLifetime = false;
            try
            {
                var jweCreatedInMemory = jsonWebTokenHandler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials);
                var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(jweCreatedInMemory, theoryData.ValidationParameters);
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
        [Fact(Skip = "Rewrite test to use claims, string will not succeed")]
        public void SetDefaultTimesOnTokenCreation()
        {
            // when the payload is passed as a string to JsonWebTokenHandler.CreateToken, we no longer
            // crack the string and add times {exp, iat, nbf}
            TestUtilities.WriteHeader($"{this}.SetDefaultTimesOnTokenCreation");
            var context = new CompareContext();

            var tokenHandler7 = new JsonWebTokenHandler();
            var payloadWithoutTimeValues = new JObject()
            {
                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                { JwtRegisteredClaimNames.GivenName, "Bob" },
                { JwtRegisteredClaimNames.Iss, Default.Issuer },
                { JwtRegisteredClaimNames.Aud, Default.Audience },
            }.ToString(Formatting.None);

            var jwtString7 = tokenHandler7.CreateToken(payloadWithoutTimeValues, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jwt7 = new JsonWebToken(jwtString7);

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
        public async Task ValidateTokenClaims()
        {
            TestUtilities.WriteHeader($"{this}.ValidateTokenClaims");

            var tokenHandler = new JsonWebTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
            var jsonWebToken = tokenValidationResult.SecurityToken as JsonWebToken;
            var email = jsonWebToken.GetPayloadValue<string>(JwtRegisteredClaimNames.Email);

            if (!email.Equals("Bob@contoso.com"))
                throw new SecurityTokenException("Token does not contain the correct value for the 'email' claim.");
        }


        [Theory, MemberData(nameof(ValidateTypeTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateType(JwtTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateType", theoryData);

            var tokenValidationResult = await new JsonWebTokenHandler().ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
            if (tokenValidationResult.Exception != null)
                theoryData.ExpectedException.ProcessException(tokenValidationResult.Exception);
            else
                theoryData.ExpectedException.ProcessNoException();

            Assert.Equal(theoryData.TokenTypeHeader, tokenValidationResult.TokenType);
        }

        public static TheoryData<JwtTheoryData> ValidateTypeTheoryData = JwtSecurityTokenHandlerTests.ValidateTypeTheoryData;

        [Theory, MemberData(nameof(ValidateJweTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWE(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWE", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadAllShortClaims),
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

            if (jsonValidationResult.IsValid && jwtValidationResult.IsValid)
            {
                if (!IdentityComparer.AreEqual(jsonValidationResult, jwtValidationResult, context))
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
                Subject = new CaseSensitiveClaimsIdentity(Default.PayloadAllShortClaims),
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

        [Theory, MemberData(nameof(ValidateJweTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWEAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEAsync", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(theoryData.Token);
                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.ValidationParameters);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

                var validConfiguration = new OpenIdConnectConfiguration();
                validConfiguration.SigningKeys.Add(KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key);
                validConfiguration.TokenDecryptionKeys.Add(KeyingMaterial.DefaultX509Key_2048);

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
                        ExpectedException = new ExpectedException(typeof(SecurityTokenDecryptionFailedException), "IDX10697:"),
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
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeyWithKeyId_OnTokenDecryptFailure_KeysInConfig_SuccessOnRetry",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            ConfigurationManager = CreateConfigurationManager(true),
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeyWithoutKeyId_OnTokenDecryptFailure_KeysInConfig_SuccessOnRetry",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            ConfigurationManager = CreateConfigurationManager(false),
                        },
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeyWithKeyId_OnTokenDecryptFailure_KeysOnlyInTvp_ThrowsException",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_128,
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration()),
                        },
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10603:")
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeyWithoutKeyId_OnTokenDecryptFailure_KeysOnlyInTvp_ThrowsException",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                            TokenDecryptionKey = new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_128) { KeyId = null },
                            ConfigurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(new OpenIdConnectConfiguration()),
                        },
                        ExpectedException = ExpectedException.SecurityTokenDecryptionFailedException("IDX10603:")
                    },
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeysInConfiguration_Succeeds",
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256)),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = true,
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfiguration),
                        },
                    },
                };

                static BaseConfigurationManager CreateConfigurationManager(bool invalidKeyHasKeyId)
                {
                    var configWrongDecryptKeys = new OpenIdConnectConfiguration();
                    if (invalidKeyHasKeyId)
                        configWrongDecryptKeys.TokenDecryptionKeys.Add(KeyingMaterial.DefaultSymmetricSecurityKey_128);
                    else
                        configWrongDecryptKeys.TokenDecryptionKeys.Add(new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricKeyBytes_128) { KeyId = null });

                    var configWithDecryptKeys = new OpenIdConnectConfiguration();
                    configWithDecryptKeys.TokenDecryptionKeys.Add(KeyingMaterial.DefaultSymmetricSecurityKey_256);

                    var configManager = new MockConfigurationManager<OpenIdConnectConfiguration>(configWrongDecryptKeys);
                    configManager.RefreshedConfiguration = configWithDecryptKeys;

                    return configManager;
                }
            }
        }

        [Theory, MemberData(nameof(DecryptJweTestCases), DisableDiscoveryEnumeration = true)]
        public async Task DecryptJwe(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.DecryptJwe", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var decryptedToken = await handler.DecryptTokenWithConfigurationAsync(theoryData.JsonWebToken, theoryData.ValidationParameters, default);
                Assert.Equal(theoryData.Token, decryptedToken);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> DecryptJweTestCases
        {
            get
            {
                var handlerWithNoDefaultTimes = new JsonWebTokenHandler();
                handlerWithNoDefaultTimes.SetDefaultTimesOnTokenCreation = false;

                var validConfiguration = new OpenIdConnectConfiguration();
                validConfiguration.SigningKeys.Add(KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key);
                validConfiguration.TokenDecryptionKeys.Add(KeyingMaterial.DefaultX509Key_2048);

                return new TheoryData<JwtTheoryData>
                {
                    new JwtTheoryData
                    {
                        TestId = "JWE_KeysInConfiguration_Succeeds",
                        JsonWebToken = new JsonWebToken(new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                            new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256))),
                        Token = new JsonWebTokenHandler().CreateToken(
                            Default.PayloadString,
                            KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2),
                        ValidationParameters = new TokenValidationParameters
                        {
                            ValidAudience = Default.Audience,
                            ValidIssuer = Default.Issuer,
                            ValidateIssuerSigningKey = false,
                            ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(validConfiguration),
                        },
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateJwsTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWSAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSAsync", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(ValidateJwsTestCases), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWS(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWS", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(ValidateJwsWithConfigTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWSWithConfig(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithConfig", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(ValidateJwsWithConfigTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWSWithConfigAsync(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithConfigAsync", theoryData);
            try
            {
                // clear up static state.
                AadIssuerValidator.s_issuerValidators[Default.AadV1Authority] = new AadIssuerValidator(null, Default.AadV1Authority);

                // previous instance is captured in a closure during theorydata set setup.
                if (theoryData.ValidationParameters.IssuerValidator != null)
                    theoryData.ValidationParameters.IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate;

                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(theoryData.Token);
                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(jwt, theoryData.ValidationParameters);
                var rawTokenValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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
                    TestId = nameof(Default.AsymmetricJws) + "_TVPInvalid_ConfigSigningKeysInvalid_SignatureValidatorReturnsValidToken",
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
        [Theory, MemberData(nameof(ValidateJwsWithLastKnownGoodTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWSWithLastKnownGood(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWSWithLastKnownGood", theoryData);
            try
            {
                // clear up static state.
                AadIssuerValidator.s_issuerValidators[Default.AadV1Authority] = new AadIssuerValidator(null, Default.AadV1Authority);

                // previous instance is captured in a closure during theorydata set setup.
                if (theoryData.ValidationParameters.IssuerValidator != null)
                    theoryData.ValidationParameters.IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate;

                var handler = new JsonWebTokenHandler();

                if (theoryData.SetupIssuerLkg)
                {
                    // make a valid pass to initiate issuer LKG.
                    var issuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority);
                    issuerValidator.ConfigurationManagerV1 = theoryData.SetupIssuerLkgConfigurationManager;

                    var previousValidateWithLKG = theoryData.ValidationParameters.ValidateWithLKG;
                    theoryData.ValidationParameters.ValidateWithLKG = false;

                    var setupValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);

                    theoryData.ValidationParameters.ValidateWithLKG = previousValidateWithLKG;

                    if (setupValidationResult.Exception != null)
                    {
                        if (setupValidationResult.IsValid)
                            context.AddDiff("setupValidationResult.IsValid, setupValidationResult.Exception != null");
                        throw setupValidationResult.Exception;
                    }
                }

                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(ValidateJWEWithLastKnownGoodTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateJWEWithLastKnownGood(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateJWEWithLastKnownGood", theoryData);
            try
            {
                // clear up static state.
                AadIssuerValidator.s_issuerValidators[Default.AadV1Authority] = new AadIssuerValidator(null, Default.AadV1Authority);

                // previous instance is captured in a closure during theorydata set setup.
                if (theoryData.ValidationParameters.IssuerValidator != null)
                    theoryData.ValidationParameters.IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).Validate;

                var handler = new JsonWebTokenHandler();
                if (theoryData.SetupIssuerLkg)
                {
                    // make a valid pass to initiate issuer LKG.
                    var issuerValidator = AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority);
                    issuerValidator.ConfigurationManagerV1 = theoryData.SetupIssuerLkgConfigurationManager;

                    var previousValidateWithLKG = theoryData.ValidationParameters.ValidateWithLKG;
                    theoryData.ValidationParameters.ValidateWithLKG = false;

                    var setupValidationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);

                    theoryData.ValidationParameters.ValidateWithLKG = previousValidateWithLKG;

                    if (setupValidationResult.Exception != null)
                    {
                        if (setupValidationResult.IsValid)
                            context.AddDiff("setupValidationResult.IsValid, setupValidationResult.Exception != null");
                        throw setupValidationResult.Exception;
                    }
                }

                AadIssuerValidator.GetAadIssuerValidator(Default.AadV1Authority).ConfigurationManagerV1 = theoryData.ValidationParameters.ConfigurationManager;
                var validationResult = await handler.ValidateTokenAsync(theoryData.Token, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(JWECompressionTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task EncryptExistingJWSWithCompressionTest(CreateTokenTheoryData theoryData)
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
                var validationResult = await handler.ValidateTokenAsync(jwtToken, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(JWECompressionTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task JWECompressionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JWECompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                // We need to have a replacement model for custom compression
                // https://identitydivision.visualstudio.com/Engineering/_workitems/edit/2719954
                //CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                string jwtToken;
                if (theoryData.SigningCredentials == null)
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm);
                else if (theoryData.AdditionalHeaderClaims != null)
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm, theoryData.AdditionalHeaderClaims);
                else
                    jwtToken = handler.CreateToken(theoryData.Payload, theoryData.SigningCredentials, theoryData.EncryptingCredentials, theoryData.CompressionAlgorithm);

                var validationResult = await handler.ValidateTokenAsync(jwtToken, theoryData.ValidationParameters);
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
                    // Skip these tests as they set a static
                    // We need to have a replacement model for custom compression
                    // https://identitydivision.visualstudio.com/Engineering/_workitems/edit/2719954
                    //new CreateTokenTheoryData()
                    //{
                    //    TestId = "NullCompressionProviderFactory",
                    //    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    //    CompressionProviderFactory = null,
                    //    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    //    Payload = Default.PayloadString,
                    //    SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                    //    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                    //    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:")
                    //},
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
                    // Skip these tests as they set a static
                    // We need to have a replacement model for custom compression
                    // https://identitydivision.visualstudio.com/Engineering/_workitems/edit/2719954
                    //new CreateTokenTheoryData()
                    //{
                    //    TestId = "CustomCompressProviderSucceeds",
                    //    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    //    CompressionProviderFactory = compressionProviderFactoryForCustom,
                    //    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    //    Payload = Default.PayloadString,
                    //    SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                    //    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                    //},
                    //new CreateTokenTheoryData()
                    //{
                    //    TestId = "CustomCompressionProviderFails",
                    //    CompressionAlgorithm = CompressionAlgorithms.Deflate,
                    //    CompressionProviderFactory = compressionProviderFactoryForCustom2,
                    //    ValidationParameters = Default.JWECompressionTokenValidationParameters,
                    //    Payload = Default.PayloadString,
                    //    SigningCredentials = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2,
                    //    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.DefaultX509Key_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                    //    ExpectedException = new ExpectedException(typeof(SecurityTokenCompressionFailedException), "IDX10680:", typeof(InvalidOperationException))
                    //},
                };
            }
        }

        [Theory, MemberData(nameof(JweDecompressSizeTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task JWEDecompressionSizeTest(JWEDecompressionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JWEDecompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                var validationResult = await handler.ValidateTokenAsync(theoryData.JWECompressionString, theoryData.ValidationParameters);
                theoryData.ExpectedException.ProcessException(validationResult.Exception, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JWEDecompressionTheoryData> JweDecompressSizeTheoryData()
        {
            // The character 'U' compresses better because UUU in base 64, repeated characters compress best.
            JsonWebTokenHandler jwth = new JsonWebTokenHandler();
            SecurityKey key = new SymmetricSecurityKey(new byte[256 / 8]);
            EncryptingCredentials encryptingCredentials = new EncryptingCredentials(key, "dir", "A128CBC-HS256");
            TokenValidationParameters validationParameters = new TokenValidationParameters { TokenDecryptionKey = key };

            TheoryData<JWEDecompressionTheoryData> theoryData = new TheoryData<JWEDecompressionTheoryData>();

#if NETCOREAPP2_1
            string payload = System.Text.Json.JsonSerializer.Serialize(new { U = new string('U', 20_000_000), UU = new string('U', 15_000_000) });
#else
            string payload = System.Text.Json.JsonSerializer.Serialize(new { U = new string('U', 100_000_000), UU = new string('U', 40_000_000) });
#endif
            string token = jwth.CreateToken(payload, encryptingCredentials, "DEF");
            theoryData.Add(new JWEDecompressionTheoryData
            {
                CompressionProviderFactory = new CompressionProviderFactory(),
                ValidationParameters = validationParameters,
                JWECompressionString = token,
                TestId = "DeflateSizeExceeded",
                ExpectedException = new ExpectedException(
                    typeof(SecurityTokenDecompressionFailedException),
                    "IDX10679:",
                    typeof(SecurityTokenDecompressionFailedException))
            });

            payload = System.Text.Json.JsonSerializer.Serialize(new { U = new string('U', 100_000_000), UU = new string('U', 50_000_000) });
            token = jwth.CreateToken(payload, encryptingCredentials, "DEF");
            theoryData.Add(new JWEDecompressionTheoryData
            {
                CompressionProviderFactory = new CompressionProviderFactory(),
                ValidationParameters = validationParameters,
                JWECompressionString = token,
                TestId = "TokenSizeExceeded",
                ExpectedException = new ExpectedException(
                    typeof(ArgumentException),
                    "IDX10209:")
            });

            return theoryData;
        }

        [Theory, MemberData(nameof(JWEDecompressionTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task JWEDecompressionTest(JWEDecompressionTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.JWEDecompressionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                var validationResult = await handler.ValidateTokenAsync(theoryData.JWECompressionString, theoryData.ValidationParameters);
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

        [Theory, MemberData(nameof(SecurityKeyNotFoundExceptionTestTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task SecurityKeyNotFoundExceptionTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SecurityKeyNotFoundExceptionTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                var validationResult = await handler.ValidateTokenAsync(token, theoryData.ValidationParameters);
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaimsExpired),
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaimsExpired),
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaims),
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

        [Theory, MemberData(nameof(IncludeSecurityTokenOnFailureTestTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task IncludeSecurityTokenOnFailedValidationTest(CreateTokenTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.IncludeSecurityTokenOnFailedValidationTest", theoryData);

            try
            {
                var handler = new JsonWebTokenHandler();
                var token = handler.CreateToken(theoryData.TokenDescriptor);
                var validationResult = await handler.ValidateTokenAsync(token, theoryData.ValidationParameters);
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaimsExpired),
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
                        Subject = new CaseSensitiveClaimsIdentity(Default.PayloadClaimsExpired),
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

        [Theory, MemberData(nameof(ValidateAuthenticationTagLengthTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task ValidateTokenAsync_ModifiedAuthNTag(CreateTokenTheoryData theoryData)
        {
            // arrange
            var payload = new JObject()
            {
                { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                { JwtRegisteredClaimNames.GivenName, "Bob" },
                { JwtRegisteredClaimNames.Iss, "http://Default.Issuer.com"},
                { JwtRegisteredClaimNames.Aud, "http://Default.Audience.com" },
                { JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now).ToString() },
                { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.Now).ToString() },
                { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.Now.AddDays(1)).ToString() },
            }.ToString();

            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var signingCredentials = Default.SymmetricSigningCredentials;

            if (SupportedAlgorithms.IsAesGcm(theoryData.Algorithm))
            {
                theoryData.EncryptingCredentials.CryptoProviderFactory = new CryptoProviderFactoryForGcm();
            }

            var jwe = jsonWebTokenHandler.CreateToken(payload, signingCredentials, theoryData.EncryptingCredentials);
            var jweWithExtraCharacters = jwe + "_cannoli_hunts_truffles_";

            // act
            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(jweWithExtraCharacters, theoryData.ValidationParameters);

            // assert
            Assert.Equal(theoryData.IsValid, tokenValidationResult.IsValid);
        }

        public static TheoryData<CreateTokenTheoryData> ValidateAuthenticationTagLengthTheoryData()
        {
            var signingCredentials512 = new SigningCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Sha512);
            return new TheoryData<CreateTokenTheoryData>()
            {
                new("Aes256Gcm_IsNotValidByDefault")
                {
                    Algorithm = SecurityAlgorithms.Aes256Gcm,
                    EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_AesGcm256,
                    ValidationParameters = new TokenValidationParameters
                    {
                        TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSigningCreds_256_Sha2.Key,
                        IssuerSigningKey = Default.SymmetricSigningKey256,
                        ValidAudience = "http://Default.Audience.com",
                        ValidIssuer = "http://Default.Issuer.com",
                    },
                    IsValid = false
                },
                new("A128CBC-HS256_IsNotValidByDefault")
                {
                    Algorithm = SecurityAlgorithms.Aes128CbcHmacSha256,
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes128CbcHmacSha256),
                    ValidationParameters = new TokenValidationParameters
                    {
                        TokenDecryptionKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        IssuerSigningKey = Default.SymmetricSigningKey256,
                        ValidAudience = "http://Default.Audience.com",
                        ValidIssuer = "http://Default.Issuer.com",
                    },
                    IsValid = false
                },
                new("A192CBC-HS384_IsNotValidByDefault")
                {
                    Algorithm = SecurityAlgorithms.Aes192CbcHmacSha384,
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes192CbcHmacSha384),
                    ValidationParameters = new TokenValidationParameters
                    {
                        TokenDecryptionKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                        IssuerSigningKey = Default.SymmetricSigningKey256,
                        ValidAudience = "http://Default.Audience.com",
                        ValidIssuer = "http://Default.Issuer.com",
                    },
                    IsValid = false
                },
                new("A256CBC-HS512_IsNotValidByDefault")
                {
                    Algorithm = SecurityAlgorithms.Aes256CbcHmacSha512,
                    EncryptingCredentials = new EncryptingCredentials(KeyingMaterial.RsaSecurityKey_2048, SecurityAlgorithms.RsaPKCS1, SecurityAlgorithms.Aes256CbcHmacSha512),
                    ValidationParameters = new TokenValidationParameters
                    {
                        TokenDecryptionKey = signingCredentials512.Key,
                        IssuerSigningKey = Default.SymmetricSigningKey256,
                        ValidAudience = "http://Default.Audience.com",
                        ValidIssuer = "http://Default.Issuer.com",
                    },
                    IsValid = false
                }
            };
        }

        [Theory, MemberData(nameof(ReadJsonWebTokenSpanTheoryData))]
        public void ReadJsonWebToken_Span(JwtTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadJsonWebToken_Span", theoryData);
            try
            {
                var handler = new JsonWebTokenHandler();
                var readOnlySpan = theoryData.Token.AsMemory();
                var jwtFromSpan = handler.ReadJsonWebToken(readOnlySpan);
                var jwtFromString = handler.ReadJsonWebToken(theoryData.Token);

                if (theoryData.ExpectedException == null)
                {
                    // Results should match between span and string versions
                    IdentityComparer.AreEqual(jwtFromSpan, jwtFromString, context);
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<JwtTheoryData> ReadJsonWebTokenSpanTheoryData()
        {
            return new TheoryData<JwtTheoryData>
            {
                new JwtTheoryData
                {
                    TestId = "ValidToken",
                    Token = Default.AsymmetricJws,
                },
                new JwtTheoryData
                {
                    TestId = "TokenTooLong",
                    Token = new string('x', TokenValidationParameters.DefaultMaximumTokenSizeInBytes + 1),
                    ExpectedException = ExpectedException.ArgumentException("IDX10209:"),
                },
                new JwtTheoryData
                {
                    TestId = "EmptyToken",
                    Token = string.Empty,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                }
            };
        }
    }

    public class CreateTokenTheoryData : TheoryDataBase
    {
        public CreateTokenTheoryData()
        {
        }

        public CreateTokenTheoryData(string testId) : base(testId)
        {
        }

        public Dictionary<string, object> AdditionalHeaderClaims { get; set; }

        public Dictionary<string, object> AdditionalInnerHeaderClaims { get; set; }

        public string Payload { get; set; }

        public string CompressionAlgorithm { get; set; }

        public BaseConfiguration Configuration { get; set; }

        public CompressionProviderFactory CompressionProviderFactory { get; set; }

        public EncryptingCredentials EncryptingCredentials { get; set; }

        public bool IsValid { get; set; } = true;

        public SigningCredentials SigningCredentials { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public JsonWebTokenHandler JsonWebTokenHandler { get; set; } = new JsonWebTokenHandler();

        public JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }

        public string JwtToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public string Algorithm { get; set; }

        public IEnumerable<SecurityKey> ExpectedDecryptionKeys { get; set; }

        public Dictionary<string, object> ExpectedClaims { get; set; }

        public List<string> AudiencesForSecurityTokenDescriptor { get; set; }
    }

    // Overrides CryptoProviderFactory.CreateAuthenticatedEncryptionProvider to create AuthenticatedEncryptionProviderMock that provides AesGcm encryption.
    public class CryptoProviderFactoryForGcm : CryptoProviderFactory
    {
        public override AuthenticatedEncryptionProvider CreateAuthenticatedEncryptionProvider(SecurityKey key, string algorithm)
        {
            if (SupportedAlgorithms.IsSupportedEncryptionAlgorithm(algorithm, key) && SupportedAlgorithms.IsAesGcm(algorithm))
                return new AuthenticatedEncryptionProviderForGcm(key, algorithm);

            return null;
        }
    }

    // Overrides AuthenticatedEncryptionProvider.Encrypt to offer AesGcm encryption for testing.
    public class AuthenticatedEncryptionProviderForGcm : AuthenticatedEncryptionProvider
    {
        public AuthenticatedEncryptionProviderForGcm(SecurityKey key, string algorithm) : base(key, algorithm)
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

    class DerivedCryptoProviderFactory : CryptoProviderFactory
    {
        internal Func<string, SecurityKey, bool> IsSupportedAlgImpl { get; set; }
        internal Func<SecurityKey, string, KeyWrapProvider> CreateKeyWrapProviderForUnwrapImpl { get; set; }

        public override bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
        {
            if (IsSupportedAlgImpl != null)
                return IsSupportedAlgImpl(algorithm, key);

            return base.IsSupportedAlgorithm(algorithm, key);
        }

        public override KeyWrapProvider CreateKeyWrapProviderForUnwrap(SecurityKey key, string algorithm)
        {
            if (CreateKeyWrapProviderForUnwrapImpl != null)
                return CreateKeyWrapProviderForUnwrapImpl(key, algorithm);

            return base.CreateKeyWrapProviderForUnwrap(key, algorithm);
        }
    }
}
