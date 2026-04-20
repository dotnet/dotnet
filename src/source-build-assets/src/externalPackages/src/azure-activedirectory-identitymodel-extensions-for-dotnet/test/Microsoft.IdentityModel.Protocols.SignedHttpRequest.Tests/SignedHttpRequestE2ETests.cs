// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestE2ETests
    {
        public static Func<CryptoProviderFactory> CreateCryptoProviderFactory = new Func<CryptoProviderFactory>(() =>
        {
            return new CryptoProviderFactory(new InMemoryCryptoProviderCache(new CryptoProviderCacheOptions(), TaskCreationOptions.None, 50));
        });


        [Theory, MemberData(nameof(RoundtripTheoryData))]
        public async Task Roundtrips(RoundtripSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Roundtrips", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(theoryData.AccessToken, theoryData.HttpRequestData, theoryData.SigningCredentials, theoryData.SignedHttpRequestCreationParameters);
                signedHttpRequestDescriptor.CnfClaimValue = theoryData.CnfClaimValue;
                var signedHttpRequest = handler.CreateSignedHttpRequest(signedHttpRequestDescriptor);
                var cryptoProviderFactory = signedHttpRequestDescriptor.SigningCredentials.CryptoProviderFactory ?? signedHttpRequestDescriptor.SigningCredentials.Key.CryptoProviderFactory;
                if (cryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(
                    signedHttpRequestDescriptor.SigningCredentials.Key,
                    signedHttpRequestDescriptor.SigningCredentials.Algorithm,
                    signedHttpRequestDescriptor.SigningCredentials.Key is AsymmetricSecurityKey ? typeof(AsymmetricSignatureProvider).ToString() : typeof(SymmetricSignatureProvider).ToString(),
                    true,
                    out _))
                    context.Diffs.Add(LogHelper.FormatInvariant("SignedHttpRequest cached SignatureProvider (Signing), Key: '{0}', Algorithm: '{1}'", signedHttpRequestDescriptor.SigningCredentials.Key, LogHelper.MarkAsNonPII(signedHttpRequestDescriptor.SigningCredentials.Algorithm)));


                var signedHttpRequestValidationContext = new SignedHttpRequestValidationContext(signedHttpRequest, theoryData.HttpRequestData, theoryData.TokenValidationParameters, theoryData.SignedHttpRequestValidationParameters);
                var result = await handler.ValidateSignedHttpRequestAsync(signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);
                if (cryptoProviderFactory.CryptoProviderCache.TryGetSignatureProvider(
                    signedHttpRequestDescriptor.SigningCredentials.Key,
                    signedHttpRequestDescriptor.SigningCredentials.Algorithm,
                    signedHttpRequestDescriptor.SigningCredentials.Key is AsymmetricSecurityKey ? typeof(AsymmetricSignatureProvider).ToString() : typeof(SymmetricSignatureProvider).ToString(),
                    false,
                    out _))
                    context.Diffs.Add(LogHelper.FormatInvariant("SignedHttpRequest cached SignatureProvider (Validate), Key: '{0}', Algorithm: '{1}'", signedHttpRequestDescriptor.SigningCredentials.Key, LogHelper.MarkAsNonPII(signedHttpRequestDescriptor.SigningCredentials.Algorithm)));

                IdentityComparer.AreBoolsEqual(result.IsValid, theoryData.IsValid, context);

                if (result.Exception != null)
                    throw result.Exception;

                Assert.NotNull(result);
                Assert.NotNull(result.SignedHttpRequest);
                Assert.NotNull(result.ValidatedSignedHttpRequest);
                Assert.NotNull(result.AccessTokenValidationResult);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<RoundtripSignedHttpRequestTheoryData> RoundtripTheoryData
        {
            get
            {
                var body = Guid.NewGuid().ToByteArray();
                var httpRequestData = new HttpRequestData()
                {
                    Method = "GET",
                    Uri = new Uri("https://www.contoso.com:443/it/requests?b=bar&a=foo&c=duck"),
                    Body = body,
                    Headers = new Dictionary<string, IEnumerable<string>>
                    {
                        { "Content-Type", new List<string> { "application/json" } },
                        { "Content-Length", new List<string> { body.Length.ToString() } },
                        { "Etag", new List<string> { "742-3u8f34-3r2nvv3" } },
                    }
                };

                var httpRequestMessage = SignedHttpRequestTestUtils.CreateHttpRequestMessage
                (
                    HttpMethod.Get,
                    new Uri("https://www.contoso.com:443/it/requests?b=bar&a=foo&c=duck"),
                    new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Etag", "742-3u8f34-3r2nvv3")
                    },
                    body,
                    new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Content-Type", "application/json")
                    }
                );

                var creationParameters = new SignedHttpRequestCreationParameters()
                {
                    CreateTs = true,
                    CreateM = true,
                    CreateP = true,
                    CreateU = true,
                    CreateH = true,
                    CreateB = true,
                    CreateQ = true,
                };

                var validationParameters = new SignedHttpRequestValidationParameters()
                {
                    ValidateTs = true,
                    ValidateM = true,
                    ValidateP = true,
                    ValidateU = true,
                    ValidateH = true,
                    ValidateB = true,
                    ValidateQ = true,
                };

                var tvpWrongIssuerSigningKey = SignedHttpRequestTestUtils.DefaultTokenValidationParameters;
                tvpWrongIssuerSigningKey.IssuerSigningKey = KeyingMaterial.RsaSecurityKey2;
                var ecdsaSigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256);

#if NET_CORE
                var adHocRsa = RSA.Create();
                adHocRsa.KeySize = 2048;
#else
                var adHocRsa = new RSACryptoServiceProvider(2048);
#endif
                var adHocRsaSecurityKey = new RsaSecurityKey(adHocRsa);
                var adHocRsaSigningCredentials = new SigningCredentials(adHocRsaSecurityKey, SecurityAlgorithms.RsaSha256);
                var adHocRsaCnfKeyId = new JObject
                {
                    { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromRSASecurityKey(adHocRsaSecurityKey).ComputeJwkThumbprint()) },
                };

                var incorrectCnfClaimValue = new JObject
                {
                    { JwtHeaderParameterNames.Jwk, new JObject
                        {
                            { "kty", "RSA" },
                            { "n",   Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters2.Modulus)}, //wrong modulus
                            { "e",  Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters_2048.Exponent) },
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.RsaSha256 },
                            { JwtHeaderParameterNames.Kid, KeyingMaterial.RsaSecurityKey_2048.KeyId }
                        }
                    }
                }.ToString(Formatting.None);

                var symmetricSigningCredentials = new SigningCredentials(KeyingMaterial.DefaultSymmetricSecurityKey_1024, SecurityAlgorithms.HmacSha256);
                var symmetricKeyCnfKeyId = new JObject
                {
                    { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromSecurityKey(symmetricSigningCredentials.Key as SymmetricSecurityKey).ComputeJwkThumbprint()) },
                };
                var symmetricKeyCnfClaimValue = new JObject
                {
                    { ConfirmationClaimTypes.Jwe, SignedHttpRequestTestUtils.EncryptToken($@"{{""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.Octet}"",""{JsonWebKeyParameterNames.K}"":""{Base64UrlEncoder.Encode((symmetricSigningCredentials.Key as SymmetricSecurityKey).Key)}""}}") },
                }.ToString(Formatting.None);

                var x509KeyCnfKeyId = new JObject
                {
                    { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromX509SecurityKey(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, true).ComputeJwkThumbprint()) },
                };

                var ecKeyCnfKeyId = new JObject
                {
                    { JwtHeaderParameterNames.Kid, Base64UrlEncoder.Encode(KeyingMaterial.JsonWebKeyP256.ComputeJwkThumbprint()) },
                };

                return new TheoryData<RoundtripSignedHttpRequestTheoryData>
                {
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        First = true,
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        TestId = "ValidJwkRsa",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwkThumprint, false),
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        TestId = "ValidRsaThumbprint",
                    },
#if NET_CORE
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwkEcdsaThumbprint, false),
                        SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256){CryptoProviderFactory = new CryptoProviderFactory()},
                        TestId = "ValidECThumbprint",
                    },
#endif
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(ecKeyCnfKeyId, false),
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeyP256, SecurityAlgorithms.EcdsaSha256){CryptoProviderFactory = CreateCryptoProviderFactory()},
                        TestId = "ValidJwkECThumbprint",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(x509KeyCnfKeyId, false),
                        SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256){CryptoProviderFactory = CreateCryptoProviderFactory()},
                        TestId = "ValidJwkX509Thumbprint",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(adHocRsaCnfKeyId, false),
                        SigningCredentials = adHocRsaSigningCredentials,
                        TestId = "ValidAdHocJwkRsaThumbprint",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = SignedHttpRequestUtilities.ToHttpRequestDataAsync(httpRequestMessage).ConfigureAwait(false).GetAwaiter().GetResult(),
                        AccessToken = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        TestId = "ValidJwkRsaUsingHttpRequestMessage",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken),
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        TestId = "ValidEncryptedAt",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters  = new SignedHttpRequestValidationParameters()
                        {
                            ValidateTs = true,
                            ValidateM = true,
                            ValidateP = true,
                            ValidateU = true,
                            ValidateH = true,
                            ValidateB = true,
                            ValidateQ = true,
                            CnfDecryptionKeys = new List<SecurityKey>() { KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key }
                        },
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(symmetricKeyCnfKeyId, false),
                        CnfClaimValue = symmetricKeyCnfClaimValue,
                        SigningCredentials = symmetricSigningCredentials,
                        TestId = "ValidJweSymmetricKeyThumbprint",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters  = new SignedHttpRequestValidationParameters()
                        {
                            ValidateTs = true,
                            ValidateM = true,
                            ValidateP = true,
                            ValidateU = true,
                            ValidateH = true,
                            ValidateB = true,
                            ValidateQ = true,
                        },
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(x509KeyCnfKeyId, false),
                        CnfClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":" +
                                        $@"{{""{JsonWebKeyParameterNames.Kid}"":""{KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256.KeyId}""," +
                                        $@"""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}""," +
                                        $@"""{JsonWebKeyParameterNames.X5c}"":[""{Convert.ToBase64String(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256.Certificate.RawData)}""]}}}}",
                        SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256, SecurityAlgorithms.RsaSha256){CryptoProviderFactory = CreateCryptoProviderFactory() },
                        TestId = "ValidX5cThumbprint",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateCnf = false,
                            CreateTs = true,
                            CreateM = true,
                            CreateP = true,
                            CreateU = true,
                            CreateH = true,
                            CreateB = true,
                            CreateQ = true,
                        },
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwkEcdsa, false),
                        SigningCredentials = ecdsaSigningCredentials,
                        TestId = "ValidJwkEcdsa",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateB = true,
                            CreateH = true,
                            CreateM = true,
                            CreateP = true,
                            CreateQ = true,
                            CreateTs = true
                        },
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken),
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidUClaimException), "IDX23003"),
                        IsValid = false,
                        TestId = "InvalidNoUClaim",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = tvpWrongIssuerSigningKey,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken),
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23013", typeof(SecurityTokenSignatureKeyNotFoundException)),
                        IsValid = false,
                        TestId = "InvalidBadIssuerSigningKey",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken),
                        SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKeySelfSigned2048_SHA512, SecurityAlgorithms.RsaSha512){CryptoProviderFactory = CreateCryptoProviderFactory() },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidSignatureException), "IDX23034"),
                        IsValid = false,
                        TestId = "InvalidBadPopSigningKey",
                    },
                    new RoundtripSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = creationParameters,
                        SignedHttpRequestValidationParameters = validationParameters,
                        TokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters,
                        HttpRequestData = httpRequestData,
                        AccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwkThumprint, false),
                        SigningCredentials = SignedHttpRequestTestUtils.DefaultSigningCredentials,
                        CnfClaimValue = incorrectCnfClaimValue,
                        IsValid = false,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23033"),
                        TestId = "InvalidCnfReference",
                    },
                };
            }
        }
    }

    public class RoundtripSignedHttpRequestTheoryData : TheoryDataBase
    {
        public string AccessToken { get; set; }

        public string CnfClaimValue { get; set; }

        public SignedHttpRequestValidationParameters SignedHttpRequestValidationParameters { get; set; }

        public TokenValidationParameters TokenValidationParameters { get; set; }

        public HttpRequestData HttpRequestData { get; set; }

        public bool IsValid { get; set; } = true;

        public Uri HttpRequestUri { get; set; }

        public string HttpRequestMethod { get; set; }

        public IDictionary<string, IEnumerable<string>> HttpRequestHeaders { get; set; }

        public byte[] HttpRequestBody { get; set; }

        public SignedHttpRequestCreationParameters SignedHttpRequestCreationParameters { get; set; } = new SignedHttpRequestCreationParameters()
        {
            CreateB = true,
            CreateH = true,
            CreateM = true,
            CreateNonce = true,
            CreateP = true,
            CreateQ = true,
            CreateTs = true,
            CreateU = true
        };

        public Dictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();

        public SigningCredentials SigningCredentials { get; set; } = SignedHttpRequestTestUtils.DefaultSigningCredentials;

        public string Token { get; set; } = SignedHttpRequestTestUtils.DefaultEncodedAccessToken;

        public string HeaderString { get; set; }

        public string PayloadString { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
