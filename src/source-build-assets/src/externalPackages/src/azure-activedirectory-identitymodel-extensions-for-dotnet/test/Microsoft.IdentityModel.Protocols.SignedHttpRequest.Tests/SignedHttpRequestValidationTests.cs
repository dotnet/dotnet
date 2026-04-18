// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class SignedHttpRequestValidationTests
    {
        [Fact]
        public async void SignedHttpRequestReplayValidation()
        {
            HashSet<string> nonceCache = new HashSet<string>();

            var handler = new SignedHttpRequestHandler();
            var signedHttpRequest1 = CreateDefaultSHRWithCustomNonce(handler, "nonce1");
            var signedHttpRequest2 = CreateDefaultSHRWithCustomNonce(handler, "nonce2");
            var signedHttpRequest3 = CreateDefaultSHRWithCustomNonce(handler, "nonce1");

            var signedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
            {
                ReplayValidatorAsync = (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext validationContext, CancellationToken cancellationToken)  => 
                {
                    var jwtSignedHttpRequest = signedHttpRequest as JsonWebToken;

                    var nonce = jwtSignedHttpRequest.GetPayloadValue<string>(SignedHttpRequestClaimTypes.Nonce);
                    if (nonceCache.Contains(nonce))
                        throw new InvalidOperationException("Replay detected");
                    else
                        nonceCache.Add(nonce);

                    return Task.FromResult<object>(null);
                },
                ValidateM = false,
                ValidateP = false,
                ValidateU = false,
                ValidateH = false,
                ValidateB = false,
                ValidateQ = false,
            };

            var signedHttpRequestValidationContext1 = new SignedHttpRequestValidationContext(signedHttpRequest1, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultTokenValidationParameters, signedHttpRequestValidationParameters);
            var result1 = await handler.ValidateSignedHttpRequestAsync(signedHttpRequestValidationContext1, CancellationToken.None).ConfigureAwait(false);
            Assert.True(result1.IsValid);

            var signedHttpRequestValidationContext2 = new SignedHttpRequestValidationContext(signedHttpRequest2, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultTokenValidationParameters, signedHttpRequestValidationParameters);
            var result2 = await handler.ValidateSignedHttpRequestAsync(signedHttpRequestValidationContext2, CancellationToken.None).ConfigureAwait(false);
            Assert.True(result2.IsValid);

            var signedHttpRequestValidationContext3 = new SignedHttpRequestValidationContext(signedHttpRequest3, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultTokenValidationParameters, signedHttpRequestValidationParameters);
            var result3 = await handler.ValidateSignedHttpRequestAsync(signedHttpRequestValidationContext3, CancellationToken.None).ConfigureAwait(false);
            Assert.False(result3.IsValid);
            Assert.IsType<InvalidOperationException>(result3.Exception);
            Assert.Equal("Replay detected", result3.Exception.Message);

        }

        private string CreateDefaultSHRWithCustomNonce(SignedHttpRequestHandler handler, string nonce)
        {
            var signedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
            {
                CreateM = false,
                CreateP = false,
                CreateU = false,
                CreateH = false,
                CreateB = false,
                CreateQ = false,
            };
            var descriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, signedHttpRequestCreationParameters);
            descriptor.CustomNonceValue = nonce;
            return handler.CreateSignedHttpRequest(descriptor);
        }

        [Theory, MemberData(nameof(ValidateTsClaimTheoryData))]
        public void ValidateTsClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateTsClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateTsClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateTsClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Ts, (long)(DateTime.UtcNow - EpochTime.UnixEpoch).TotalSeconds) ),
                        TestId = "ValidTs",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Ts, (long)(DateTime.UtcNow.AddHours(1) - EpochTime.UnixEpoch).TotalSeconds) ),
                        TestId = "ValidTs2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Ts, (long)(DateTime.UtcNow.AddMinutes(-6) - EpochTime.UnixEpoch).TotalSeconds) ),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidTsClaimException), "IDX23010"),
                        TestId = "Expired",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Ts, (DateTime.UtcNow- EpochTime.UnixEpoch).TotalSeconds.ToString())),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidTsClaimException), "IDX23003"),
                        TestId = "InvalidNotLong",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Ts, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidTsClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateMClaimTheoryData))]
        public void ValidateMClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateMClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateMClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateMClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestMethod = "GET",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, "GET")),
                        TestId = "ValidM1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = "GET",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, "get")),
                        TestId = "ValidM2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = " GET  ",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, "  gEt     ")),
                        TestId = "ValidM3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = "POST",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, "GET")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidMClaimException), "IDX23011"),
                        TestId = "InvalidMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = "",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, null)),
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "EmptyExpectedMethod",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = null,
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, null)),
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullExpectedMethod",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestMethod = "GET",
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.M, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidMClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullToken",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateUClaimTheoryData))]
        public void ValidateUClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateUClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateUClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateUClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso.com")),
                        TestId = "ValidU1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.CONTOSO.com")),
                        TestId = "ValidU2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso.com:443")),
                        TestId = "ValidU3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com:443"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso.com")),
                        TestId = "ValidU4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com:443"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso.com:443")),
                        TestId = "ValidU5",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso2.com")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidUClaimException), "IDX23012"),
                        TestId = "InvalidMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("/path1", UriKind.Relative),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, "www.contoso.com")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidUClaimException), "IDX23001"),
                        TestId = "InvalidRelativeUri",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, null)),
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.U, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidUClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullToken",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidatePClaimTheoryData))]
        public void ValidatePClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidatePClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidatePClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidatePClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1")),
                        TestId = "ValidP1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1/")),
                        TestId = "ValidP2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "path1")),
                        TestId = "ValidP3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "path1/")),
                        TestId = "ValidP4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("/path1", UriKind.Relative),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1")),
                        TestId = "ValidP5",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/pa th1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/pa%20th1")),
                        TestId = "ValidP6",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1/more/andmore"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1/more/andmore")),
                        TestId = "ValidP8",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1/more/andmore"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1/more/andmore/")),
                        TestId = "ValidP9",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1/more/andmore/"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path1/more/andmore/")),
                        TestId = "ValidP10",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, "/path2")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPClaimException), "IDX23011"),
                        TestId = "InvalidMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, null)),
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.P, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidPClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullToken",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateHClaimTheoryData))]
        public void ValidateHClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateHClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateHClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateHClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    // the specification has incorrect hash value ("bZA981YJBrPlIzOvplbu3e7ueREXXr38vSkxIBYOaxI")
                    // because authors used "\r\n" as a separator instead of "\n" as stated in the spec.
                    // "P6z5XN4tTzHkfwe3XO1YvVUIurSuhvh_UG10N_j-aGs" is the value if "\n" is used as a separator.
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "content-type" , new List<string> { "application/json" } },
                            { "etag" , new List<string> { "742-3u8f34-3r2nvv3" } },
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"content-type\", \"etag\"],\"P6z5XN4tTzHkfwe3XO1YvVUIurSuhvh_UG10N_j-aGs\"]"))),
                        TestId = "ValidHSpecTest",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        TestId = "ValidH1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidH2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { SignedHttpRequestConstants.AuthorizationHeader , new List<string> { "exyz...." } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        TestId = "ValidH3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { SignedHttpRequestConstants.AuthorizationHeader.ToLower() , new List<string> { "exyz...." } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        TestId = "ValidH4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } }

                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\",\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2")}\"]"))),
                        TestId = "ValidH5",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } },
                            { "headerName3" , new List<string> { "headerValue3" } },

                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\",\"headername2\",\"headername3\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2\nheadername3: headerValue3")}\"]"))),
                        TestId = "ValidH6",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1", "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidHRepeated1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME1" , new List<string> { "headerValue1" } },
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidHRepeated2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1", "headerValue2" } },
                            { "headerName2" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername2: headerValue2")}\"]"))),
                        TestId = "ValidHRepeated3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HeaderNAME1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername2: headerValue2")}\"]"))),
                        TestId = "ValidHRepeated4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME1" , new List<string> { "headerValue1" } },
                            { "HeaderNAME1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername2: headerValue2")}\"]"))),
                        TestId = "ValidHRepeated5",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME1" , new List<string> { "headerValue1" } },
                            { "HeaderNAME1" , new List<string> { "headerValue1" } },
                            { "headerName3" , new List<string> { "headerValue3" } },
                            { "headerName2" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername2\",\"headername3\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername2: headerValue2\nheadername3: headerValue3")}\"]"))),
                        TestId = "ValidHRepeated6",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME1" , new List<string> { "headerValue1" } },
                            { "HeaderNAME1" , new List<string> { "headerValue1" } },
                            { "headerName3" , new List<string> { "headerValue3" } },
                            { "headerName2" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername3\",\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername3: headerValue3\nheadername2: headerValue2")}\"]"))),
                        TestId = "ValidHRepeated7",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } }

                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        TestId = "ValidAcceptUnsignedHeaders",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } }

                        },
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateH = true,
                            AcceptUnsignedHeaders = false,
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23026"),
                        TestId = "InvalidDontAcceptUnsignedHeaders",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue2" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23011"),
                        TestId = "InvalidMismatchValue",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue1" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23025", typeof(SignedHttpRequestInvalidHClaimException)),
                        TestId = "InvalidHeaderNameMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue1" } }
                        },
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, JArray.Parse($"[\"headername1\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23024", null, true),
                        TestId = "InvalidNumberOfArguments",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, "notAnArray")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23024", innerTypeExpected: typeof(ArgumentOutOfRangeException)),
                        TestId = "InvalidClaimType",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.H, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidHClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateQClaimTheoryData))]
        public void ValidateQClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateQClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateQClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateQClaimTheoryData
        {
            get
            {
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestUri = new Uri("https://www.contoso.com:443/it/requests?b=bar&a=foo&c=duck"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"b\", \"a\", \"c\"],\"u4LgkGUWhP9MsKrEjA4dizIllDXluDku6ZqCeyuR-JY\"]"))),
                        TestId = "ValidQSpecTest",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        TestId = "ValidQ1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam2=value2"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]"))),
                        TestId = "ValidQ2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam2=value2&queryParam1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]"))),
                        TestId = "ValidQ3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("/path1?queryParam1=value1", UriKind.Relative),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        TestId = "ValidQ4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidQ5",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?query Param1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"query%20Param1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("query%20Param1=value1")}\"]"))),
                        TestId = "ValidQ6",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=val ue1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=val%20ue1")}\"]"))),
                        TestId = "ValidQ7",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&QUERYParam1=value2"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\",\"QUERYParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&QUERYParam1=value2")}\"]"))),
                        TestId = "ValidQ8",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam2=value2&queryParam1=value1&queryParam2=value3"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        TestId = "ValidQRepeated1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam2=value2&queryParam2=value22&queryParam1=value1&queryParam2=value3"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        TestId = "ValidQRepeated2",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam1=value2"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidQRepeated3",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam1=value2&queryParam1=value3"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]"))),
                        TestId = "ValidQRepeated4",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam2=value2"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        TestId = "ValidAcceptUnsignedQueryParams",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam2=value2"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23029"),
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateH = true,
                            AcceptUnsignedQueryParameters = false,
                        },
                        TestId = "InvalidDontAcceptUnsignedQueryParams",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[\"queryParam1\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23024", null, true),
                        TestId = "InvalidNumberOfArguments",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23025", typeof(SignedHttpRequestInvalidQClaimException)),
                        TestId = "InvalidQueryParamNameMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, JArray.Parse($"[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value2")}\"]"))),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23011"),
                        TestId = "InvalidValueMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = null,
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, null)),
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullUri",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q, null)),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestUri = new Uri("https://www.contoso.com"),
                        SignedHttpRequestToken = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.Q,  "notAnArray")),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidQClaimException), "IDX23024", innerTypeExpected: typeof(ArgumentOutOfRangeException)),
                        TestId = "InvalidClaimType",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        SignedHttpRequestToken = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "NullToken",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateBClaimTheoryData))]
        public void ValidateBClaim(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateBClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                handler.ValidateBClaim(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateBClaimTheoryData
        {
            get
            {
                var signedHttpRequestWithCustomB = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.B, SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("abcd")));
                var signedHttpRequestWithEmptyB = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.B, SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")));
                var signedHttpRequestWithNullB = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.B, null));
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        HttpRequestBody = Encoding.UTF8.GetBytes("abcd"),
                        SignedHttpRequestToken = signedHttpRequestWithCustomB,
                        TestId = "ValidB1",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestBody = Encoding.UTF8.GetBytes("aaaa"),
                        SignedHttpRequestToken = signedHttpRequestWithCustomB,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidBClaimException), "IDX23011"),
                        TestId = "InvalidMismatch",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        HttpRequestBody = null,
                        SignedHttpRequestToken = signedHttpRequestWithEmptyB,
                        TestId = "NullBytesValid",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithNullB,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidBClaimException), "IDX23003"),
                        TestId = "InvalidClaimNotPresent",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateSignedHttpRequestCallsTheoryData))]
        public async Task ValidateSignedHttpRequestCalls(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var containsClaim = new Func<SignedHttpRequestValidationParameters, string, bool>((validationParams, claim) =>
            {
                return validationParams.ValidatePresentClaims && validationParams.ClaimsToValidateWhenPresent.Contains(claim);
            });

            var context = TestUtilities.WriteHeader($"{this}.ValidateSignedHttpRequestCalls", theoryData);
            var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();

            var handler = new SignedHttpRequestHandlerPublic();
            var signedHttpRequest = await handler.ValidateSignedHttpRequestPayloadAsync(theoryData.SignedHttpRequestToken, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

            var methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateTsClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateTs &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.Ts))
                context.AddDiff($"ValidationParameters.ValidateTs={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateTs}, ValidateTsClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateMClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateM &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.M))
                context.AddDiff($"ValidationParameters.ValidateM={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateM}, ValidateMClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateUClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateU &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.U))
                context.AddDiff($"ValidationParameters.ValidateU={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateU}, ValidateUClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidatePClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateP &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.P))
                context.AddDiff($"ValidationParameters.ValidateP={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateP}, ValidatePClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateQClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateQ &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.Q))
                context.AddDiff($"ValidationParameters.ValidateQ={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateQ}, ValidateQClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateHClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateH &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.H))
                context.AddDiff($"ValidationParameters.ValidateH={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateH}, ValidateHClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ValidateBClaimCall"];
            if (methodCalledStatus != signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateB &&
                methodCalledStatus != containsClaim(signedHttpRequestValidationContext.SignedHttpRequestValidationParameters, SignedHttpRequestClaimTypes.B))
                context.AddDiff($"ValidationParameters.ValidateB={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ValidateB}, ValidateBClaim method call status: {methodCalledStatus}.");

            methodCalledStatus = (bool)signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"];
            if (methodCalledStatus != (signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ReplayValidatorAsync != null))
                context.AddDiff($"ValidationParameters.ReplayValidatorAsync={signedHttpRequestValidationContext.SignedHttpRequestValidationParameters.ReplayValidatorAsync != null}, ReplayValidator call status: {methodCalledStatus}.");

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateSignedHttpRequestCallsTheoryData
        {
            get
            {
                var signedHttpRequestToken = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString());
                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateTs = true,
                            ValidateM = true,
                            ValidateP = true,
                            ValidateQ = true,
                            ValidateU = true,
                            ValidateH = true,
                            ValidateB = true,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidAllCalls",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = null,
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidNoCalls",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidatePresentClaims = true,
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidPresentDefaultClaimsCalls",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidatePresentClaims = true,
                            ClaimsToValidateWhenPresent = new List<string>
                            {
                                SignedHttpRequestClaimTypes.Ts,
                                SignedHttpRequestClaimTypes.M,
                                SignedHttpRequestClaimTypes.U,
                                SignedHttpRequestClaimTypes.P,
                                SignedHttpRequestClaimTypes.Q,
                                SignedHttpRequestClaimTypes.H,
                                SignedHttpRequestClaimTypes.B,
                            },
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidPresentAllClaimsCalls",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidatePresentClaims = true,
                            ClaimsToValidateWhenPresent = null,
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidTokenNullClaimsValidateWhenPresentList",
                    },
                     new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestToken,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidatePresentClaims = true,
                            ClaimsToValidateWhenPresent = new List<string> { "not a valid claim" },
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidTokenSpuriousClaimsInValidateWhenPresentList",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = new JsonWebToken(new JsonWebTokenHandler().CreateToken(
                            new JObject().ToString(),
                            SignedHttpRequestTestUtils.DefaultSigningCredentials,
                            new Dictionary<string, object>() { { System.IdentityModel.Tokens.Jwt.JwtHeaderParameterNames.Typ, SignedHttpRequestConstants.TokenType } })),
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidatePresentClaims = true,
                            ClaimsToValidateWhenPresent = new List<string>
                            {
                                SignedHttpRequestClaimTypes.Ts,
                                SignedHttpRequestClaimTypes.M,
                                SignedHttpRequestClaimTypes.U,
                                SignedHttpRequestClaimTypes.P,
                                SignedHttpRequestClaimTypes.Q,
                                SignedHttpRequestClaimTypes.H,
                                SignedHttpRequestClaimTypes.B,
                            },
                            ValidateTs = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateU = false,
                            ValidateH = false,
                            ValidateB = false,
                            ReplayValidatorAsync = async (SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                signedHttpRequestValidationContext.CallContext.PropertyBag["onlyTrack_ReplayValidatorCall"] = true;
                                await Task.FromResult<object>(null);
                            }
                        },
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateSignedHttpRequestSignatureAsync", null },
                                {"onlyTrack_ValidateTsClaimCall", false },
                                {"onlyTrack_ValidateMClaimCall", false },
                                {"onlyTrack_ValidateUClaimCall", false },
                                {"onlyTrack_ValidatePClaimCall", false },
                                {"onlyTrack_ValidateQClaimCall", false },
                                {"onlyTrack_ValidateHClaimCall", false },
                                {"onlyTrack_ValidateBClaimCall", false },
                                {"onlyTrack_AdditionalClaimValidatorCall", false },
                                {"onlyTrack_ReplayValidatorCall", false },
                            }
                        },
                        TestId = "ValidNoClaimsPresent",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ValidateSignedHttpRequestSignatureTheoryData))]
        public async Task ValidateSignedHttpRequestSignature(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateSignedHttpRequestSignature", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();
                var signingKey = await handler.ValidateSignatureAsync(theoryData.SignedHttpRequestToken, theoryData.PopKey, signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);
                IdentityComparer.AreSecurityKeysEqual(signingKey, theoryData.ExpectedPopKey, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateSignedHttpRequestSignatureTheoryData
        {
            get
            {
                var signedHttpRequest = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString(Formatting.None));
                var validPopKey = SignedHttpRequestTestUtils.DefaultSigningCredentials.Key;
                var invalidPopKey = KeyingMaterial.RsaSecurityKey1;
                var theoryData = new TheoryData<ValidateSignedHttpRequestTheoryData>();

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        SignedHttpRequestToken = signedHttpRequest,
                        PopKey = null,
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException)),
                        TestId = "InvalidNullPopKey",
                    });

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        PopKey = invalidPopKey,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidSignatureException), "IDX23034"),
                        TestId = "InvalidPopKeySignatureValidationFails",
                    });

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        PopKey = validPopKey,
                        ExpectedPopKey = validPopKey,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            SignatureValidatorAsync = (SecurityKey popKeys, SecurityToken signedHttpRequestToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                throw new NotImplementedException();
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(NotImplementedException)),
                        TestId = "InvalidDelegateThrows",
                    });

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = new JsonWebToken(signedHttpRequest.EncodedHeader + "." + signedHttpRequest.EncodedPayload + "."),
                        PopKey = validPopKey,
                        ExpectedPopKey = validPopKey,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidSignatureException), "IDX23009", typeof(ArgumentNullException)),
                        TestId = "InvalidUnsignedRequest",
                    });

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        PopKey = validPopKey,
                        ExpectedPopKey = validPopKey,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            SignatureValidatorAsync = async (SecurityKey popKey, SecurityToken signedHttpRequestToken, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken) =>
                            {
                                return await Task.FromResult(popKey);
                            }
                        },
                        TestId = "ValidDelegate",
                    });

                theoryData.Add(
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        PopKey = validPopKey,
                        ExpectedPopKey = validPopKey,
                        TestId = "ValidTest",
                    });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(ValidateSignedHttpRequestTheoryData))]
        public async Task ValidateSignedHttpRequest(ValidateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateSignedHttpRequest", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandlerPublic();
                var signedHttpRequestValidationContext = theoryData.BuildSignedHttpRequestValidationContext();

                if (signedHttpRequestValidationContext.CallContext.PropertyBag != null && signedHttpRequestValidationContext.CallContext.PropertyBag.ContainsKey("makeSignedHttpRequestValidationContextNull"))
                    signedHttpRequestValidationContext = null;

                var result = await handler.ValidateSignedHttpRequestAsync(signedHttpRequestValidationContext, CancellationToken.None).ConfigureAwait(false);

                if (result.Exception != null)
                {
                    if (result.IsValid)
                        context.AddDiff("result.IsValid, result.Exception != null");

                    throw result.Exception;
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ValidateSignedHttpRequestTheoryData> ValidateSignedHttpRequestTheoryData
        {
            get
            {
                var encodedAccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwk, false);
                var encodedEncryptedAccessToken = SignedHttpRequestTestUtils.CreateAt(SignedHttpRequestTestUtils.DefaultCnfJwk, true);

                var signedHttpRequest = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString(Formatting.None));
                var signedHttpRequestWithEncryptedAt = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.At, encodedEncryptedAccessToken));
                var signedHttpRequestWithNullAt = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.At, null));
                var signedHttpRequestWithEmptyAt = SignedHttpRequestTestUtils.ReplaceOrAddPropertyAndCreateDefaultSignedHttpRequest(new JProperty(SignedHttpRequestClaimTypes.At, string.Empty));

                var validationResult = new JsonWebTokenHandler().ValidateTokenAsync(encodedAccessToken, SignedHttpRequestTestUtils.DefaultTokenValidationParameters).Result;
                var resultingClaimsIdentity = validationResult.ClaimsIdentity;
                var validatedToken = validationResult.SecurityToken;

                return new TheoryData<ValidateSignedHttpRequestTheoryData>
                {
                    new ValidateSignedHttpRequestTheoryData
                    {
                        First = true,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"makeSignedHttpRequestValidationContextNull", null },
                            }
                        },
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullSignedHttpRequestValidationContext",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = null,
                        ExpectedException = new ExpectedException(typeof(SecurityTokenMalformedException), "IDX14100"),
                        TestId = "InvalidToken",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithNullAt,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23003"),
                        TestId = "InvalidNoAccessToken",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEmptyAt,
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23003"),
                        TestId = "InvalidEmptyAcccessToken",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockValidateAccessTokenAsync_returnInvalidResult", null },
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23013", typeof(SecurityTokenValidationException)),
                        TestId = "InvalidAccessTokenValidationFailed",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        CallContext = new CallContext()
                        {
                            PropertyBag = new Dictionary<string, object>()
                            {
                                {"mockResolvePopKeyAsync_returnNullKey", null },
                            }
                        },
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException)),
                        TestId = "InvalidResolvedPopKeyIsNull",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt, 
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ExpectedSignedHttpRequestValidationResult = new SignedHttpRequestValidationResult()
                        {
                            AccessTokenValidationResult = new TokenValidationResult()
                            {
                                IsValid = true,
                                SecurityToken = validatedToken,
                                ClaimsIdentity = resultingClaimsIdentity
                            },
                            IsValid = true,
                            SignedHttpRequest = signedHttpRequestWithEncryptedAt.EncodedToken,
                            ValidatedSignedHttpRequest = signedHttpRequestWithEncryptedAt,

                        },
                        TestId = "ValidEncryptedAcccessToken",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.RsaSha256 },
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23013", typeof(SecurityTokenDecryptionFailedException)),
                        TestId = "ValidEncryptedAcccessToken_DecryptionAlgorithmNotListed",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.Aes128CbcHmacSha256 },
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23013", typeof(SecurityTokenSignatureKeyNotFoundException)),
                        TestId = "ValidEncryptedAcccessToken_IssuerAlgorithmNotListed",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(false),
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidAtClaimException), "IDX23013", typeof(SecurityTokenDecryptionFailedException)),
                        TestId = "ValidEncryptedAcccessToken_AcceptedAlgorithmValidatorFails",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string> { SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.RsaSha256 },
                        },
                        TestId = "ValidEncryptedAcccessToken_AcceptedAlgorithmListed",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            ValidAlgorithms = new List<string>(),
                        },
                        TestId = "ValidEncryptedAcccessToken_EmptyAcceptedAlgorithms",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequestWithEncryptedAt,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ValidationParameters =  new TokenValidationParameters()
                        {
                            IssuerSigningKey = KeyingMaterial.RsaSigningCreds_2048_Public.Key,
                            ValidIssuer = Default.Issuer,
                            ValidAudience = Default.Audience,
                            TokenDecryptionKey = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes128_Sha2.Key,
                            ValidateLifetime = false,
                            AlgorithmValidator = ValidationDelegates.AlgorithmValidatorBuilder(true),
                        },
                        TestId = "ValidEncryptedAcccessToken_AcceptedAlgorithmValidatorValidates",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                        },
                        ExpectedSignedHttpRequestValidationResult = new SignedHttpRequestValidationResult()
                        {
                            AccessTokenValidationResult = new TokenValidationResult()
                            {
                                IsValid = true,
                                SecurityToken = validatedToken,
                                ClaimsIdentity = resultingClaimsIdentity
                            },
                            IsValid = true,
                            SignedHttpRequest = signedHttpRequest.EncodedToken,
                            ValidatedSignedHttpRequest = signedHttpRequest,
                        },
                        TestId = "ValidTest",
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            NonceValidatorAsync =  (popKey, signedHttpRequestToken, signedHttpRequestValidationContext, cancellationToken) => false
                        },
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestInvalidNonceClaimException), "IDX23036", typeof(SignedHttpRequestInvalidNonceClaimException)),
                        TestId = "InValidNonceValidationFailed"
                    },
                    new ValidateSignedHttpRequestTheoryData
                    {
                        SignedHttpRequestToken = signedHttpRequest,
                        SignedHttpRequestValidationParameters = new SignedHttpRequestValidationParameters()
                        {
                            ValidateB = false,
                            ValidateH = false,
                            ValidateM = false,
                            ValidateP = false,
                            ValidateQ = false,
                            ValidateTs = false,
                            ValidateU = false,
                            NonceValidatorAsync =  (popKey, signedHttpRequestToken, signedHttpRequestValidationContext, cancellationToken) =>
                            {
                                var jwtSignedHttpRequest = signedHttpRequestToken as JsonWebToken;
                                var nonce = jwtSignedHttpRequest.GetPayloadValue<string>(SignedHttpRequestClaimTypes.Nonce);
                                return nonce == SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.GetValue(SignedHttpRequestClaimTypes.Nonce).ToString();
                            }
                        },
                        ExpectedSignedHttpRequestValidationResult = new SignedHttpRequestValidationResult()
                        {
                            AccessTokenValidationResult = new TokenValidationResult()
                            {
                                IsValid = true,
                                SecurityToken = validatedToken,
                                ClaimsIdentity = resultingClaimsIdentity
                            },
                            IsValid = true,
                            SignedHttpRequest = signedHttpRequest.EncodedToken,
                            ValidatedSignedHttpRequest = signedHttpRequest,
                        },
                        TestId = "ValidTestWithNonce"
                    }
                };
            }
        }
    }

    public class ValidateSignedHttpRequestTheoryData : TheoryDataBase
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

            var tokenValidationParameters = ValidationParameters ?? SignedHttpRequestTestUtils.DefaultTokenValidationParameters;

            // add testId for debugging purposes
            var callContext = CallContext;
            if (callContext.PropertyBag == null)
                callContext.PropertyBag = new Dictionary<string, object>() { { "testId", TestId } };
            else
                callContext.PropertyBag.Add("testId", TestId);

            // set SignedHttpRequestToken if set and if JsonWebToken, otherwise set "dummy" value
            return new SignedHttpRequestValidationContext(SignedHttpRequestToken is JsonWebToken jwt ? jwt.EncodedToken : "dummy", httpRequestData, tokenValidationParameters, SignedHttpRequestValidationParameters, callContext);
        }

        public SignedHttpRequestValidationResult ExpectedSignedHttpRequestValidationResult { get; set; }

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

        public SecurityKey PopKey { get; set; }

        public SecurityKey ExpectedPopKey { get; set; }

        internal JsonWebToken SignedHttpRequestToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
