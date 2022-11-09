//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestCreationTests
    {
        [Fact]
        public void CreateSignedHttpRequest()
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignedHttpRequest", "", true);

            var handler = new SignedHttpRequestHandlerPublic();

            var signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, new SignedHttpRequestCreationParameters() { CreateM = false, CreateP = false, CreateU = false });
            var signedHttpRequestString = handler.CreateSignedHttpRequest(signedHttpRequestDescriptor);

            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                IssuerSigningKey = SignedHttpRequestTestUtils.DefaultSigningCredentials.Key
            };
            var result = new JsonWebTokenHandler().ValidateToken(signedHttpRequestString, tvp);

            if (result.IsValid == false)
                context.AddDiff($"Not able to create and validate signed http request token");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void CreateSignedHttpRequestWithAdditionalHeaderClaims()
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignedHttpRequestWithAdditionalHeaderClaims", "", true);

            var handler = new SignedHttpRequestHandlerPublic();

            // The 'alg', 'kid', and 'x5t' claims are added by default based on the provided <see cref="SigningCredentials"/> and SHOULD NOT be included in this dictionary as this
            /// will result in an exception being thrown.  
            var signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, new SignedHttpRequestCreationParameters() { CreateM = false, CreateP = false, CreateU = false })
            {
                AdditionalHeaderClaims = new Dictionary<string, object>() { { "kid", "kid_is_not_allowd" } }
            };
            Assert.Throws<SecurityTokenException>(() => handler.CreateSignedHttpRequest(signedHttpRequestDescriptor));

            // allowed additional header claims 
            signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, new SignedHttpRequestCreationParameters() { CreateM = false, CreateP = false, CreateU = false })
            {
                AdditionalHeaderClaims = new Dictionary<string, object>() { { "additionalHeaderClaim1", "val1" }, { "additionalHeaderClaim2", "val2" } }
            };
            var signedHttpRequestString = handler.CreateSignedHttpRequest(signedHttpRequestDescriptor);

            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                IssuerSigningKey = SignedHttpRequestTestUtils.DefaultSigningCredentials.Key
            };
            var result = new JsonWebTokenHandler().ValidateToken(signedHttpRequestString, tvp);

            if (result.IsValid == false)
                context.AddDiff($"Not able to create and validate signed http request token");

            TestUtilities.AssertFailIfErrors(context);
        }


        [Theory, MemberData(nameof(CreateClaimCallsTheoryData))]
        public void CreateClaimCalls(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateClaimCalls", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                var payloadString = handler.CreateHttpRequestPayloadPublic(signedHttpRequestDescriptor, theoryData.CallContext);
                var payload = JObject.Parse(payloadString);

                foreach (var payloadItem in payload)
                {
                    if (!theoryData.ExpectedPayloadClaims.Contains(payloadItem.Key))
                        context.AddDiff($"ExpectedPayloadClaims doesn't contain the claim '{payloadItem.Key}'");
                }

                foreach (var expectedClaim in theoryData.ExpectedPayloadClaims)
                {
                    if (!payload.ContainsKey(expectedClaim))
                        context.AddDiff($"Payload doesn't contain the claim '{expectedClaim}'");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateClaimCallsTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedPayloadClaims = new List<string>() { "at" },
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateCnf = false,
                            CreateB = false,
                            CreateH = false,
                            CreateM = false,
                            CreateNonce = false,
                            CreateP = false,
                            CreateQ = false,
                            CreateTs = false,
                            CreateU = false,
                        },
                        TestId = "NoClaimsCreated",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedPayloadClaims = new List<string>() { "at", "b", "h", "m", "nonce", "p", "q", "ts", "u", "additionalClaim", "cnf" },
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateCnf = true,
                            CreateB = true,
                            CreateH = true,
                            CreateM = true,
                            CreateNonce = true,
                            CreateP = true,
                            CreateQ = true,
                            CreateTs = true,
                            CreateU = true,
                        },
                        AdditionalPayloadClaims = new Dictionary<string, object>() { {"additionalClaim", "additionalClaimValue" } },
                        HttpRequestBody = Guid.NewGuid().ToByteArray(),
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "header1", new List<string>() {"headerValue1"} }
                        },
                        HttpRequestMethod = "GET",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=quertValue1"),
                        TestId = "AllClaimsCreated",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateAtClaimTheoryData))]
        public void CreateAtClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateAtClaimTheoryData", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddAtClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateAtClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.At,
                        ExpectedClaimValue = SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                        TestId = "ValidAt",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                };
            }
        }

        [Theory(Skip = "This test failed on build server due to some EpochTime changes, should be fixed later"), MemberData(nameof(CreateTsClaimTheoryData))]
        public void CreateTsClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateTsClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddTsClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                var delta = 5;
                var expectedTs = (long)theoryData.ExpectedClaimValue;
                var actualTs = payload.Value<long>(theoryData.ExpectedClaim);
                if (Math.Abs(expectedTs - actualTs) > delta)
                {
                    context.AddDiff($"Expected ts '{expectedTs}' was not the same as the actual ts '{actualTs}' within a tolerance of '{delta}' seconds.");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateTsClaimTheoryData
        {
            get
            {
                var timeNow = DateTime.UtcNow;
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds,
                        TestId = "ValidTs",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters() { TimeAdjustment = TimeSpan.FromMinutes(-1) },
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds - 60,
                        TestId = "ValidTsWithTimeAdjustmentMinus",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters() { TimeAdjustment = TimeSpan.FromMinutes(1) },
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds + 60,
                        TestId = "ValidTsWithTimeAdjustmentPlus",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateMClaimTheoryData))]
        public void CreateMClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateMClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddMClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateMClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.M,
                        ExpectedClaimValue = "GET",
                        HttpRequestMethod = "GET",
                        TestId = "ValidM",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaimValue = "GET",
                        HttpRequestMethod = "get",
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23002"),
                        TestId = "InvalidLowercaseM",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = "",
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "EmptyM",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateUClaimTheoryData))]
        public void CreateUClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateUClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddUClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!theoryData.Payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateUClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = "www.contoso.com",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        TestId = "ValidU1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = "www.contoso.com",
                        HttpRequestUri = new Uri("http://www.Contoso.com/"),
                        TestId = "ValidU2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = "www.contoso.com",
                        HttpRequestUri = new Uri("https://www.contoso.com:443"),
                        TestId = "ValidU3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = "www.contoso.com:81",
                        HttpRequestUri = new Uri("https://www.contoso.com:81"),
                        TestId = "ValidU4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        HttpRequestUri = new Uri("/relativePath", UriKind.Relative),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23001"),
                        TestId = "InvalidRelativeUri",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreatePClaimTheoryData))]
        public void CreatePClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreatePClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddPClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreatePClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/path1",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        TestId = "ValidP1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/path1/",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1/"),
                        TestId = "ValidP2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/path1",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        TestId = "ValidP3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/path1",
                        HttpRequestUri = new Uri("http://www.contoso.com:81/path1"),
                        TestId = "ValidP4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/pa%20th1",
                        HttpRequestUri = new Uri("http://www.contoso.com:81/pa th1"),
                        TestId = "ValidP5",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/",
                        HttpRequestUri = new Uri("http://www.contoso.com"),
                        TestId = "NoPath",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = "/relativePath",
                        HttpRequestUri = new Uri("/relativePath", UriKind.Relative),
                        TestId = "ValidRelativeUri",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateQClaimTheoryData))]
        public void CreateQClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateQClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddQClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<JArray>(theoryData.ExpectedClaim).ToString(Formatting.None), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<JArray>(theoryData.ExpectedClaim).ToString(Formatting.None)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateQClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        TestId = "ValidQ1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam2=value2"),
                        TestId = "ValidQ2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&queryParam1=value1&queryParam2=value2"),
                        TestId = "ValidQ3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"query%20Param1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("query%20Param1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?query Param1=value1"),
                        TestId = "ValidQ4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"query%20Param1%20\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("query%20Param1%20=value1%20")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?query Param1 =value1%20"),
                        TestId = "ValidQ5",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&queryParam1=value1&query=Param2=value2"),
                        TestId = "ValidQ6",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        TestId = "ValidNoQueryParams1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&"),
                        TestId = "ValidNoQueryParams2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&t"),
                        TestId = "ValidNoQueryParams3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&t="),
                        TestId = "ValidNoQueryParams4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&repeated=repeated1&repeated=repeate2"),
                        TestId = "ValidRepeatedQ1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&queryParam1=value1&repeated=repeate2"),
                        TestId = "ValidRepeatedQ2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2&queryParam1=value1"),
                        TestId = "ValidRepeatedQ3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2&queryParam1=value1&repeated=repeate3"),
                        TestId = "ValidRepeatedQ4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2"),
                        TestId = "RepeatedQEmpty",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("/relativePath?queryParam1=value1", UriKind.Relative),
                        TestId = "ValidRelativeUri",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        Payload = new Dictionary<string, object>() { {SignedHttpRequestClaimTypes.Q, null } },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008: Exception caught while creating the 'q' claim.", typeof(ArgumentException)),
                        TestId = "PayloadAlreadyHasQClaim"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateHClaimTheoryData))]
        public void CreateHClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateHClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddHClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<JArray>(theoryData.ExpectedClaim).ToString(Formatting.None), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<JArray>(theoryData.ExpectedClaim).ToString(Formatting.None)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateHClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        },
                        TestId = "ValidH1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\",\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } },
                        },
                        TestId = "ValidH2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\",\"headername2\",\"headername3\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2\nheadername3: headerValue3")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } },
                            { "headerName3" , new List<string> { "headerValue3" } },
                        },
                        TestId = "ValidH3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"header name1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("header name1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "header Name1" , new List<string> { "headerValue1" } }
                        },
                        TestId = "ValidH4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "" , new List<string> { "headerValue1" } }
                        },
                        TestId = "ValidH5",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "h1" , new List<string> { "" } }
                        },
                        TestId = "ValidH6",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { SignedHttpRequestConstants.AuthorizationHeader , new List<string> { "exyxz..." } },
                        },
                        TestId = "ValidH7",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        },
                        TestId = "NoHeaders",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                        },
                        TestId = "ValidRepeatedH1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                        },
                        TestId = "ValidRepeatedH2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                        },
                        TestId = "ValidRepeatedH3",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "HeaDerName2" , new List<string> { "headerValue2" } },
                            { "headername2" , new List<string> { "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                        },
                        TestId = "ValidRepeatedH4",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "HeaDerName2" , new List<string> { "headerValue2" } },
                            { "headername2" , new List<string> { "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME2" , new List<string> { "headerValue22" } },
                        },
                        TestId = "ValidRepeatedH5",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1", "headerValue10" } },
                        },
                        TestId = "ValidRepeatedH6",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        TestId = "EmptyHeaders",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        },
                        Payload = new Dictionary<string, object>() { {SignedHttpRequestClaimTypes.H, null } },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008: Exception caught while creating the 'h' claim.", typeof(ArgumentException)),
                        TestId = "PayloadAlreadyHasHClaim"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateBClaimTheoryData))]
        public void CreateBClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateBClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddBClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateBClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes("abcd")),
                        HttpRequestBody = Encoding.UTF8.GetBytes("abcd"),
                        TestId = "ValidB1",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes("")),
                        HttpRequestBody = new byte[0],
                        TestId = "ValidB2",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes("")),
                        HttpRequestBody = null,
                        TestId = "NullBytes",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes("abcd")),
                        HttpRequestBody = Encoding.UTF8.GetBytes("abcd"),
                        Payload = new Dictionary<string, object>() { {SignedHttpRequestClaimTypes.B, null } },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008: Exception caught while creating the 'b' claim.", typeof(ArgumentException)),
                        TestId = "PayloadAlreadyHasBClaim"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateCnfClaimTheoryData))]
        public void CreateCnfClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateCnfClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddCnfClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!IdentityComparer.AreStringsEqual(payload[theoryData.ExpectedClaim].ToString(Formatting.None), theoryData.ExpectedClaimValue, context))
                    context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload[theoryData.ExpectedClaim].ToString(Formatting.None)}', but expected value was '{theoryData.ExpectedClaimValue}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateCnfClaimTheoryData
        {
            get
            {
                var testCnf = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{ConfirmationClaimTypes.Kid}"":""test""}}}}";
                var rsaJwkFromX509Key = JsonWebKeyConverter.ConvertFromX509SecurityKey(KeyingMaterial.X509SecurityKey1, true);
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullPayload",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        Cnf = testCnf,
                        ExpectedClaimValue = testCnf,
                        TestId = "ValidManualCnfClaim",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeyRsa_1024, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(KeyingMaterial.JsonWebKeyRsa_1024.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{KeyingMaterial.JsonWebKeyRsa_1024.E}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{KeyingMaterial.JsonWebKeyRsa_1024.N}""}}}}",
                        TestId = "ValidJwkRsaKey",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeyP256, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(KeyingMaterial.JsonWebKeyP256.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.Crv}"":""{KeyingMaterial.JsonWebKeyP256.Crv}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.EllipticCurve}"",""{JsonWebKeyParameterNames.X}"":""{KeyingMaterial.JsonWebKeyP256.X}"",""{JsonWebKeyParameterNames.Y}"":""{KeyingMaterial.JsonWebKeyP256.Y}""}}}}",
                        TestId = "ValidJwkECKey",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.SymmetricSecurityKey2_1024, SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008", typeof(SignedHttpRequestCreationException)),
                        TestId = "InvalidJwkSymmetricKey",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeySymmetric128, SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008", typeof(ArgumentException)),
                        TestId = "InvalidJwkSymmetricKey",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKey1, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromRSASecurityKey(KeyingMaterial.RsaSecurityKey1).ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters1.Exponent)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters1.Modulus)}""}}}}",
                        TestId = "ValidRsaKey",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKey1, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(rsaJwkFromX509Key.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{rsaJwkFromX509Key.E}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{rsaJwkFromX509Key.N}""}}}}",
                        TestId = "ValidX509Key",
                    },
#if NET472 || NET_CORE
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromECDsaSecurityKey(KeyingMaterial.Ecdsa256Key).ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.Crv}"":""{ECDsaAdapter.GetCrvParameterValue(KeyingMaterial.Ecdsa256Parameters.Curve)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.EllipticCurve}"",""{JsonWebKeyParameterNames.X}"":""{Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.X)}"",""{JsonWebKeyParameterNames.Y}"":""{Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.Y)}""}}}}",
                        TestId = "ValidEcdsaKey",
                    },
#else
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX10674", typeof(NotSupportedException)),
                        TestId = "InvalidEcdsaKeyDesktop",
                    },
#endif
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        Cnf = testCnf,
                        Payload = new Dictionary<string, object>() { {ConfirmationClaimTypes.Cnf, null } },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008: Exception caught while creating the 'cnf' claim.", typeof(ArgumentException)),
                        TestId = "InvalidPayloadAlreadyHasCnfClaim"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateNonceClaimTheoryData))]
        public void CreateNonceClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateNonceClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                handler.AddNonceClaimPublic(theoryData.Payload, signedHttpRequestDescriptor);
                var payload = JObject.FromObject(theoryData.Payload);

                if (!payload.ContainsKey(theoryData.ExpectedClaim))
                    context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                if (!string.IsNullOrEmpty(signedHttpRequestDescriptor.CustomNonceValue))
                {
                    if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                        context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateNonceClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = SignedHttpRequestClaimTypes.Nonce,
                        TestId = "ValidDefaultNonce",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = "nonce",
                        ExpectedClaimValue = "nonce1",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters(),
                        CustomNonceValue = "nonce1",
                        TestId = "ValidCustomNonce",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateAdditionalClaimTheoryData))]
        public void CreateAdditionalClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateAdditionalClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                var payloadString =  handler.CreateHttpRequestPayloadPublic(signedHttpRequestDescriptor, theoryData.CallContext);
                var payload = JObject.Parse(payloadString);

                if (signedHttpRequestDescriptor.AdditionalPayloadClaims != null)
                {
                    if (!payload.ContainsKey(theoryData.ExpectedClaim))
                        context.AddDiff($"Payload doesn't contain the claim '{theoryData.ExpectedClaim}'");

                    if (!IdentityComparer.AreStringsEqual(payload.Value<string>(theoryData.ExpectedClaim), theoryData.ExpectedClaimValue, context))
                        context.AddDiff($"Value of '{theoryData.ExpectedClaim}' claim is '{payload.Value<string>(theoryData.ExpectedClaim)}', but expected value was '{theoryData.ExpectedClaimValue}'");
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateAdditionalClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData
                    {
                        First = true,
                        ExpectedClaim = "customClaim",
                        ExpectedClaimValue = "customClaimValue",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        },
                        AdditionalPayloadClaims = new Dictionary<string, object>() { { "customClaim", "customClaimValue" } },
                        TestId = "ValidAdditionalClaim",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        ExpectedClaim = "customClaim",
                        ExpectedClaimValue = "customClaimValue",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        },
                        AdditionalPayloadClaims = new Dictionary<string, object>() { { SignedHttpRequestClaimTypes.M, "will_not_be_overwritten" },  {"customClaim", "customClaimValue" } },
                        TestId = "ValidAdditionalClaims",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        },
                        TestId = "AdditionalCustomClaimsNotSet",
                    },
                    new CreateSignedHttpRequestTheoryData
                    {
                        Payload = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullPayload",
                    },
                };
            }
        }
    }

    public class CreateSignedHttpRequestTheoryData : TheoryDataBase
    {
        public SignedHttpRequestDescriptor BuildSignedHttpRequestDescriptor()
        {
            var httpRequestData = new HttpRequestData()
            {
                Body = HttpRequestBody,
                Uri = HttpRequestUri,
                Method = HttpRequestMethod,
                Headers = HttpRequestHeaders
            };

            return new SignedHttpRequestDescriptor(Token, httpRequestData, SigningCredentials, SignedHttpRequestCreationParameters)
            {
                AdditionalHeaderClaims = AdditionalHeaderClaims,
                AdditionalPayloadClaims = AdditionalPayloadClaims,
                CnfClaimValue = Cnf,
                CustomNonceValue = CustomNonceValue,
            };
        }

        public Dictionary<string, object> AdditionalHeaderClaims { get; set; }

        public Dictionary<string, object> AdditionalPayloadClaims { get; set; }

        public CallContext CallContext { get; set; } = new CallContext();

        public string CustomNonceValue { get; set; }

        public object ExpectedClaimValue { get; set; }

        public string ExpectedClaim { get; set; }

        public List<string> ExpectedPayloadClaims { get; set; }

        public Uri HttpRequestUri { get; set; }

        public string HttpRequestMethod { get; set; }

        public IDictionary<string, IEnumerable<string>> HttpRequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();

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

        public string Cnf { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
