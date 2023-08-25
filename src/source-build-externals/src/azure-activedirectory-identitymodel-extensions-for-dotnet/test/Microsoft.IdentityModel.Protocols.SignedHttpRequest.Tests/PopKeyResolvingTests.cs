// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class PopKeyResolvingTests
    {
        [Theory, MemberData(nameof(ResolvePopKeyFromCnfClaimAsyncTheoryData))]
        public async Task ResolvePopKeyFromCnfClaimAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyFromCnfClaimAsync", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandlerPublic();

                Cnf cnf = theoryData.ConfirmationClaim != null ? new Cnf(theoryData.ConfirmationClaim.ToString(Formatting.None)) : null;
                _ = await handler.ResolvePopKeyFromCnfClaimAsync(cnf, theoryData.SignedHttpRequestToken, theoryData.ValidatedAccessToken, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if ((bool)signedHttpRequestValidationContext.CallContext.PropertyBag[theoryData.MethodToCall] == false)
                    context.AddDiff($"{theoryData.MethodToCall} was not called.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromCnfClaimAsyncTheoryData
        {
            get
            {
                var accessToken = new JsonWebToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken);
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        MethodToCall = "trackResolvePopKeyFromJwk",
                        ConfirmationClaim = SignedHttpRequestTestUtils.DefaultCnfJwk,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackResolvePopKeyFromJwk", false }
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidJwk",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        MethodToCall = "trackResolvePopKeyFromJwe",
                        ConfirmationClaim = SignedHttpRequestTestUtils.DefaultCnfJwe,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackResolvePopKeyFromJwe", false }
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidJwe",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        MethodToCall = "trackResolvePopKeyFromJku",
                        ConfirmationClaim = SignedHttpRequestTestUtils.DefaultJku,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackResolvePopKeyFromJku", false }
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidJku",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        MethodToCall = "trackResolvePopKeyFromJku",
                        ConfirmationClaim = SignedHttpRequestTestUtils.DefaultJkuKid,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackResolvePopKeyFromJku", false }
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidJkuKid",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        MethodToCall = "trackResolvePopKeyFromKid",
                        ConfirmationClaim = SignedHttpRequestTestUtils.DefaultKid,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackResolvePopKeyFromKid", false }
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidKid",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ConfirmationClaim = JObject.Parse(@"{""unknown_claim"": 1}"),
                        ValidatedAccessToken = accessToken,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidCnfClaimException), "IDX23014"),
                        TestId = "UnknownCnfClaim",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ConfirmationClaim = null,
                        ValidatedAccessToken = accessToken,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidCnfClaim",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyAsyncTheoryData))]
        public async Task ResolvePopKeyAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyTheoryData", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandler();
                _ = await handler.ResolvePopKeyAsync(theoryData.SignedHttpRequestToken, theoryData.ValidatedAccessToken, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if ((bool)signedHttpRequestValidationContext.CallContext.PropertyBag[theoryData.MethodToCall] == false)
                    context.AddDiff($"{theoryData.MethodToCall} was not called.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyAsyncTheoryData
        {
            get
            {
                var accessToken = new JsonWebToken(SignedHttpRequestTestUtils.DefaultEncodedAccessToken);
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        MethodToCall = "trackPopKeyResolver",
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                { "trackPopKeyResolver", false }
                            }
                        },
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            PopKeyResolverAsync = async (SecurityToken validatedAccessToken, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["trackPopKeyResolver"] = true;
                                return await Task.FromResult<SecurityKey>(null);
                            }
                        },
                        ValidatedAccessToken = accessToken,
                        TestId = "ValidResolverCall",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyFromJwkTheoryData))]
        public void ResolvePopKeyFromJwk(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyTheoryData", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandler();
                _ = handler.ResolvePopKeyFromJwk(new JsonWebKey(theoryData.PopKeyString), signedHttpRequestValidationContext);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromJwkTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        PopKeyString = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullPopKey",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = string.Empty,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidEmptyPopKey",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = "dummy",
                        ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX10805", null, true),
                        TestId = "InvalidPopKeyNotAJWK",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = SignedHttpRequestTestUtils.DefaultJwe.ToString(Formatting.None),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23015"),
                        TestId = "InvalidPopKeyNotSymmetricKey",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = SignedHttpRequestTestUtils.InvalidJwk.ToString(Formatting.None),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23016"),
                        TestId = "InvalidPopKeyRsa",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = SignedHttpRequestTestUtils.DefaultJwkEcdsa.ToString(Formatting.None),
                        TestId = "ValidEcdsa",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = SignedHttpRequestTestUtils.DefaultJwk.ToString(Formatting.None),
                        TestId = "ValidRsa",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyFromJweTheoryData))]
        public async Task ResolvePopKeyFromJweAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyFromJwe", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandler();
                _ = await handler.ResolvePopKeyFromJweAsync(theoryData.PopKeyString, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromJweTheoryData
        {
            get
            {
                var jwe = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultJwe.ToString(Formatting.None));
                var jweRsa = SignedHttpRequestTestUtils.EncryptToken(SignedHttpRequestTestUtils.DefaultJwk.ToString(Formatting.None));
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        PopKeyString = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullPopKey",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = string.Empty,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidEmptyPopKey",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = "dummy",
                        ExpectedException = new ExpectedException(typeof(SecurityTokenMalformedException), "IDX14100", null, true),
                        TestId = "InvalidPopKeyNotAJWK",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        PopKeyString = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23017"),
                        TestId = "InvalidNoDecryptionKeysSet",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeysResolverAsync = async (SecurityToken jweCnf, CancellationToken cancellationToken) =>
                            {
                                return await Task.FromResult<IEnumerable<SecurityKey>>(null);
                            }
                        },
                        PopKeyString = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23017"),
                        TestId = "InvalidCnfDelegateReturnsNull"
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeysResolverAsync = async (SecurityToken jweCnf, CancellationToken cancellationToken) =>
                            {
                                return await Task.FromResult<IEnumerable<SecurityKey>>(new List<SecurityKey>());
                            }
                        },
                        PopKeyString = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23017"),
                        TestId = "InvalidCnfDelegateReturnsEmptyList"
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeys = new List<SecurityKey>() { SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key }
                        },
                        PopKeyString = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23018", null, true),
                        TestId = "InvalidBadToken"
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeys = new List<SecurityKey>() { SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key }
                        },
                        PopKeyString = jweRsa,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23019"),
                        TestId = "InvalidNotSymmetricKey"
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeysResolverAsync = async (SecurityToken jweCnf, CancellationToken cancellationToken) =>
                            {
                                return await Task.FromResult<IEnumerable<SecurityKey>>(new List<SecurityKey>() { SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key });
                            }
                        },
                        PopKeyString = jwe,
                        TestId = "ValidCnfDelegateReturnsCorrectKey"
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            CnfDecryptionKeys = new List<SecurityKey>() { SignedHttpRequestTestUtils.DefaultEncryptingCredentials.Key }
                        },
                        PopKeyString = jwe,
                        TestId = "ValidTest"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyFromJkuTheoryData))]
        public async Task ResolvePopKeyFromJkuAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyFromJkuAsync", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandlerPublic();
                var popKey = await handler.ResolvePopKeyFromJkuAsync(string.Empty, null, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if (popKey == null)
                    context.AddDiff("Resolved Pop key is null.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromJkuTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return0Keys", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23031"),
                        TestId = "InvalidZeroKeysReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_returnNull", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23031"),
                        TestId = "InvalidNullReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return1Key", null }
                            }
                        },
                        TestId = "ValidOneKeyReturned",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(GetCnfClaimValueTheoryData))]
        public void GetCnfClaimValue(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetCnfClaimValue", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                _ = handler.GetCnfClaimValue(null, theoryData.ValidatedAccessToken, null);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> GetCnfClaimValueTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        ValidatedAccessToken = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullAccessToken",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ValidatedAccessToken = new JsonWebToken(SignedHttpRequestTestUtils.CreateAt(null, false)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidCnfClaimException), "IDX23003"),
                        TestId = "InvalidNullAccessToken",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ValidatedAccessToken = new JsonWebToken(SignedHttpRequestTestUtils.CreateAt(null, false, false)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidCnfClaimException), "IDX23003"),
                        TestId = "InvalidNoCnfClaimFound",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ValidatedAccessToken = new JsonWebToken(SignedHttpRequestTestUtils.CreateAt(null, false)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidCnfClaimException), "IDX23003"),
                        TestId = "InvalidCnfIsNull",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ValidatedAccessToken = new JsonWebToken(SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwk, false, true, true)),
                        TestId = "ValidTestCnfAsAsString",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        ValidatedAccessToken = new JsonWebToken(SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwk, false)),
                        TestId = "ValidTest",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyFromJkuKidTheoryData))]
        public async Task ResolvePopKeyFromJkuKidAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyFromJkuKidAsync", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandlerPublic();
                Cnf cnf = new Cnf { Kid = theoryData.Kid };
                var popKey = await handler.ResolvePopKeyFromJkuAsync(string.Empty, cnf, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if (popKey == null)
                    context.AddDiff("Resolved Pop key is null.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromJkuKidTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        Kid ="bad_kid",
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return2Keys", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23021"),
                        TestId = "InvalidNoKidMatch",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        Kid = "irelevantForThisTest",
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return0Keys", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23031"),
                        TestId = "InvalidZeroKeysReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        Kid = "irelevantForThisTest",
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_returnNull", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23031"),
                        TestId = "InvalidNullReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        Kid ="bad_kid",
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return2Keys", null }
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23021"),
                        TestId = "InvalidNoKidMatch",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        Kid = SignedHttpRequestTestUtils.DefaultSigningCredentials.Kid,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return2Keys", null }
                            }
                        },
                        TestId = "ValidOneKidMatch",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        Kid = SignedHttpRequestTestUtils.DefaultSigningCredentials.Kid,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockGetPopKeysFromJkuAsync_return1Key", null }
                            }
                        },
                        TestId = "ValidKidMatch",
                    },
                };
            }
        }

        [Theory (Skip = "flaky"), MemberData(nameof(GetPopKeysFromJkuAsyncTheoryData))]
        public async Task GetPopKeysFromJkuAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetPopKeysFromJkuAsync", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandler();
                var popKeys = await handler.GetPopKeysFromJkuAsync(theoryData.JkuSetUrl, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if (popKeys.Count != theoryData.ExpectedNumberOfPopKeysReturned)
                    context.AddDiff($"Number of returned pop keys {popKeys.Count} is not the same as the expected: {theoryData.ExpectedNumberOfPopKeysReturned}.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> GetPopKeysFromJkuAsyncTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        JkuSetUrl = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidJkuUrlNull",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = string.Empty,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidJkuUrlEmptyString",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "http://www.contoso.com",
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23006"),
                        TestId = "InvalidHttpsRequired",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "https://www.contoso.com",
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            HttpClientProvider = () => HttpResponseMessageUtils.SetupHttpClientThatReturns(string.Empty),
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23022", null, true),
                        TestId = "InvalidNoContentReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "http://www.contoso.com",
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            RequireHttpsForJkuResourceRetrieval = false,
                            HttpClientProvider = () => HttpResponseMessageUtils.SetupHttpClientThatReturns(string.Empty),
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23022", null, true),
                        TestId = "InvalidHttpNoContentReturned",
                    },
                    // TODO - find out why test is timing out in the AzureDevOps build, appears to be unrelated to the caching changes
                    //new ResolvePopKeyTheoryData
                    //{
                    //    JkuSetUrl = "http://www.contoso.com",
                    //    SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                    //    {
                    //        RequireHttpsForJkuResourceRetrieval = false,
                    //    },
                    //    ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23022", typeof(ArgumentException)),
                    //    TestId = "Valid0KeysReturnedLive",
                    //},
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "https://www.contoso.com",
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            HttpClientProvider = () => HttpResponseMessageUtils.SetupHttpClientThatReturns("{\"test\": 1}"),
                        },
                        ExpectedNumberOfPopKeysReturned = 0,
                        TestId = "Valid0KeysReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "https://www.contoso.com",
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            HttpClientProvider = () => HttpResponseMessageUtils.SetupHttpClientThatReturns(DataSets.JsonWebKeySetString1),
                        },
                        ExpectedNumberOfPopKeysReturned = 2,
                        TestId = "Valid2KeysReturned",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        JkuSetUrl = "https://www.contoso.com",
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            HttpClientProvider = () => HttpResponseMessageUtils.SetupHttpClientThatReturns(DataSets.JsonWebKeySetECCString),
                        },
                        ExpectedNumberOfPopKeysReturned = 3,
                        TestId = "Valid3KeysReturned",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ResolvePopKeyFromKeyIdentifierAsyncTheoryData))]
        public async Task ResolvePopKeyFromKeyIdentifierAsync(ResolvePopKeyTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ResolvePopKeyFromKeyIdentifierAsync", theoryData);
            try
            {
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var handler = new SignedHttpRequestHandlerPublic();
                var popKeys = await handler.ResolvePopKeyFromKeyIdentifierAsync(theoryData.Kid, theoryData.SignedHttpRequestToken, theoryData.ValidatedAccessToken, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ResolvePopKeyTheoryData> ResolvePopKeyFromKeyIdentifierAsyncTheoryData
        {
            get
            {
                return new TheoryData<ResolvePopKeyTheoryData>
                {
                    new ResolvePopKeyTheoryData
                    {
                        First = true,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23023"),
                        TestId = "InvalidDelegateNotSet",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            PopKeyResolverFromKeyIdAsync = (string kid, SecurityToken validatedAccessToken, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                throw new NotImplementedException();
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(NotImplementedException)),
                        TestId = "InvalidDelegatThrows",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            PopKeyResolverFromKeyIdAsync = async (string kid, SecurityToken validatedAccessToken, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                return await Task.FromResult(SignedHttpRequestTestUtils.DefaultSigningCredentials.Key);
                            }
                        },
                        TestId = "ValidTest",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(ConfirmationClaimTypes.Cnf, SignedHttpRequestTestUtils.DefaultCnfJwkThumprint)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23023"),
                        Kid = "irrelevant",
                        TestId = "InvalidTestCheckRecursion",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString(Formatting.None)),
                        Kid = "incorrect_cnf_reference",
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPopKeyException), "IDX23033"),
                        TestId = "InvalidCnfReference",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString(Formatting.None)),
                        Kid = Base64UrlEncoder.Encode(new JsonWebKey(SignedHttpRequestTestUtils.DefaultJwk.ToString(Formatting.None)).ComputeJwkThumbprint()),
                        TestId = "ValidCnfReference",
                    },
                    new ResolvePopKeyTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString(Formatting.None)),
                        Kid = Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromRSASecurityKey(KeyingMaterial.RsaSecurityKey_2048).ComputeJwkThumbprint()),
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockResolvePopKeyFromCnfClaimAsync_returnRsa",  KeyingMaterial.RsaSecurityKey_2048 }
                            }
                        },
                        TestId = "ValidCnfReferenceRsaKey",
                    },
                };
            }
        }
    }
    public class ResolvePopKeyTheoryData : TheoryDataBase
    {
        public SignedHttpRequestValidationContext BuildSignedHttpRequestValidationContext()
        {
            var httpRequestData = new HttpRequestData()
            {
                Body = HttpRequestBody,
                Uri = HttpRequestUri,
                Method = HttpRequestMethod,
                Headers = HttpRequestHeaders
            };

            // add testId for debugging purposes
            var callContext = CallContext;
            if (callContext.PropertyBag == null)
                callContext.PropertyBag = new Dictionary<string, object>() { { "testId", TestId } };
            else
                callContext.PropertyBag.Add("testId", TestId);

            // set SignedHttpRequestToken if set and if JsonWebToken, otherwise set "dummy" value
            return new SignedHttpRequestValidationContext(SignedHttpRequestToken is JsonWebToken jwt ? jwt.EncodedToken : "dummy", httpRequestData, SignedHttpRequestTestUtils.DefaultTokenValidationParameters, SignedHttpRequestValidationParameters, callContext);
        }

        internal JObject ConfirmationClaim { get; set; }

        public string MethodToCall { get; set; }

        public Uri HttpRequestUri { get; set; }

        public string HttpRequestMethod { get; set; }

        public IDictionary<string, IEnumerable<string>> HttpRequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public byte[] HttpRequestBody { get; set; }

        public SignedHttpRequestValidationParameters SignedHttpRequestValidationParameters { get; set; } = new SignedHttpRequestValidationParameters()
        {
            ValidateB = true,
            ValidateH = true,
            ValidateM = true,
            ValidateP = true,
            ValidateQ = true,
            ValidateTs = true,
            ValidateU = true
        };

        public SigningCredentials SigningCredentials { get; set; } = SignedHttpRequestTestUtils.DefaultSigningCredentials;

        public string Token { get; set; } = SignedHttpRequestTestUtils.DefaultEncodedAccessToken;

        public JsonWebToken SignedHttpRequestToken { get; set; }

        public JsonWebToken ValidatedAccessToken { get; set; }

        public string PopKeyString { get; set; }

        public string Kid { get; set; }

        public string JkuSetUrl { get; set; }

        public int ExpectedNumberOfPopKeysReturned { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
