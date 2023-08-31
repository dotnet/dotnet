// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestCreationTests
    {
        [Fact]
        public void CreateSignedHttpRequest()
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignedHttpRequest", "", true);

            var handler = new SignedHttpRequestHandler();
            var signedHttpRequestDescriptor =
                new SignedHttpRequestDescriptor(
                    SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                    new HttpRequestData(),
                    SignedHttpRequestTestUtils.DefaultSigningCredentials,
                    new SignedHttpRequestCreationParameters()
                    {
                        CreateM = false,
                        CreateP = false,
                        CreateU = false
                    });

            var signedHttpRequestString = handler.CreateSignedHttpRequest(signedHttpRequestDescriptor);
            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                IssuerSigningKey = SignedHttpRequestTestUtils.DefaultSigningCredentials.Key
            };

            var result = new JsonWebTokenHandler().ValidateTokenAsync(signedHttpRequestString, tvp).Result;
            if (result.IsValid == false)
                context.AddDiff($"Not able to create and validate signed http request token");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void CreateSignedHttpRequestWithAdditionalHeaderClaims()
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateSignedHttpRequestWithAdditionalHeaderClaims", "", true);

            var handler = new SignedHttpRequestHandler();
            // The 'alg', 'kid', and 'x5t' claims are added by default based on the provided <see cref="SigningCredentials"/> and SHOULD NOT be included in this dictionary as this
            // will result in an exception being thrown.
            var signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, new SignedHttpRequestCreationParameters() { CreateM = false, CreateP = false, CreateU = false })
            {
                AdditionalHeaderClaims = new Dictionary<string, object>() { { "kid", "kid_is_not_allowed" } }
            };

            Assert.Throws<SecurityTokenException>(() => handler.CreateSignedHttpRequest(signedHttpRequestDescriptor));

            // allowed additional header claims 
            signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(SignedHttpRequestTestUtils.DefaultEncodedAccessToken, new HttpRequestData(), SignedHttpRequestTestUtils.DefaultSigningCredentials, new SignedHttpRequestCreationParameters() { CreateM = false, CreateP = false, CreateU = false })
            {
                AdditionalHeaderClaims = new Dictionary<string, object>() { { "additionalHeaderClaim1", "val1" }, { "additionalHeaderClaim2", "val2" } }
            };

            var signedHttpRequestString = handler.CreateSignedHttpRequest(signedHttpRequestDescriptor);
            var tvp = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                IssuerSigningKey = SignedHttpRequestTestUtils.DefaultSigningCredentials.Key
            };

            var result = new JsonWebTokenHandler().ValidateTokenAsync(signedHttpRequestString, tvp).Result;

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
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                var payloadString = handler.CreateHttpRequestPayload(signedHttpRequestDescriptor, theoryData.CallContext);
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
                    new CreateSignedHttpRequestTheoryData("NoClaimsCreated")
                    {
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
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("AllClaimsCreated")
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
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=quertValue1")
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateAtClaimTheoryData))]
        public void CreateAtClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateAtClaimTheoryData", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddAtClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context); theoryData.ExpectedException.ProcessNoException(context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateAtClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("validAt")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.At,
                        ExpectedClaimValue = $"{{\"at\":\"{SignedHttpRequestTestUtils.DefaultEncodedAccessToken}\"}}",
                    }
                };
            }
        }

        [Theory(Skip = "This test failed on build server due to some EpochTime changes, should be fixed later"), MemberData(nameof(CreateTsClaimTheoryData))]
        public void CreateTsClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateTsClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddTsClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
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
                    new CreateSignedHttpRequestTheoryData("ValidTs")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds
                    },
                    new CreateSignedHttpRequestTheoryData("ValidTsWithTimeAdjustmentMinus")
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters() { TimeAdjustment = TimeSpan.FromMinutes(-1) },
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds - 60
                    },
                    new CreateSignedHttpRequestTheoryData("ValidTsWithTimeAdjustmentPlus")
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters() { TimeAdjustment = TimeSpan.FromMinutes(1) },
                        ExpectedClaim = SignedHttpRequestClaimTypes.Ts,
                        ExpectedClaimValue = (long)(timeNow - EpochTime.UnixEpoch).TotalSeconds + 60
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateMClaimTheoryData))]
        public void CreateMClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateMClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddMClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateMClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidM")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.M,
                        ExpectedClaimValue = $@"{{""m"":""GET""}}",
                        HttpRequestMethod = "GET"
                    },
                    new CreateSignedHttpRequestTheoryData("InvalidLowercaseM")
                    {
                        ExpectedClaimValue = $@"{{""m"":""GET""}}",
                        HttpRequestMethod = "get",
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23002")
                    },
                    new CreateSignedHttpRequestTheoryData("EmptyM")
                    {
                        HttpRequestMethod = "",
                        ExpectedException = ExpectedException.ArgumentNullException()
                    },
                };
            }
        }

        [Theory, MemberData(nameof(CreateUClaimTheoryData))]
        public void CreateUClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateUClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddUClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateUClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidU1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue =  $@"{{""u"":""www.contoso.com""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidU2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = $@"{{""u"":""www.contoso.com""}}",
                        HttpRequestUri = new Uri("http://www.Contoso.com/")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidU3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = $@"{{""u"":""www.contoso.com""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com:443")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidU4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        ExpectedClaimValue = $@"{{""u"":""www.contoso.com:81""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com:81")
                    },
                    new CreateSignedHttpRequestTheoryData("InvalidRelativeUri")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.U,
                        HttpRequestUri = new Uri("/relativePath", UriKind.Relative),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23001")
                    },
                    new CreateSignedHttpRequestTheoryData("NullUri")
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreatePClaimTheoryData))]
        public void CreatePClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreatePClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddPClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreatePClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidP1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/path1""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidP2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/path1/""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1/")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidP3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/path1""}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidP4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/path1""}}",
                        HttpRequestUri = new Uri("http://www.contoso.com:81/path1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidP5")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/pa%20th1""}}",
                        HttpRequestUri = new Uri("http://www.contoso.com:81/pa th1")
                    },
                    new CreateSignedHttpRequestTheoryData("NoPath")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/""}}",
                        HttpRequestUri = new Uri("http://www.contoso.com")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRelativeUri")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.P,
                        ExpectedClaimValue = $@"{{""p"":""/relativePath""}}",
                        HttpRequestUri = new Uri("/relativePath", UriKind.Relative)
                    },
                    new CreateSignedHttpRequestTheoryData("NullUri")
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateQClaimTheoryData))]
        public void CreateQClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateQClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddQClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateQClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidQ1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidQ2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&queryParam2=value2"),

                    },
                    new CreateSignedHttpRequestTheoryData("ValidQ3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\",\"queryParam2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1&queryParam2=value2")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&queryParam1=value1&queryParam2=value2"),

                    },
                    new CreateSignedHttpRequestTheoryData("ValidQ4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"query%20Param1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("query%20Param1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?query Param1=value1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidQ5")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"query%20Param1%20\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("query%20Param1%20=value1%20")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?query Param1 =value1%20")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidQ6")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&queryParam1=value1&query=Param2=value2")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidNoQueryParams1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidNoQueryParams2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidNoQueryParams3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&t")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidNoQueryParams4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1&t=")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedQ1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?queryParam1=value1&repeated=repeated1&repeated=repeate2")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedQ2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&queryParam1=value1&repeated=repeate2")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedQ3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2&queryParam1=value1")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedQ4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2&queryParam1=value1&repeated=repeate3")
                    },
                    new CreateSignedHttpRequestTheoryData("RepeatedQEmpty")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestUri = new Uri("https://www.contoso.com/path1?&repeated=repeated1&repeated=repeate2")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRelativeUri")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Q,
                        ExpectedClaimValue = $"{{\"q\":[[\"queryParam1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("queryParam1=value1")}\"]}}",
                        HttpRequestUri = new Uri("/relativePath?queryParam1=value1", UriKind.Relative)
                    },
                    new CreateSignedHttpRequestTheoryData("NullUri")
                    {
                        HttpRequestUri = null,
                        ExpectedException = ExpectedException.ArgumentNullException()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateHClaimTheoryData))]
        public void CreateHClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateHClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddHClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateHClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidH1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\",\"headername2\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\",\"headername2\",\"headername3\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1\nheadername2: headerValue2\nheadername3: headerValue3")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2" } },
                            { "headerName3" , new List<string> { "headerValue3" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"header name1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("header name1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "header Name1" , new List<string> { "headerValue1" } }
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH5")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "" , new List<string> { "headerValue1" } }
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH6")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "h1" , new List<string> { "" } }
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidH7")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { SignedHttpRequestConstants.AuthorizationHeader , new List<string> { "exyxz..." } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("NoHeaders")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } }
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH3")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName2" , new List<string> { "headerValue2", "headerValue10" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH4")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "HeaDerName2" , new List<string> { "headerValue2" } },
                            { "headername2" , new List<string> { "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH5")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[\"headername1\"],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("headername1: headerValue1")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "HeaDerName2" , new List<string> { "headerValue2" } },
                            { "headername2" , new List<string> { "headerValue10" } },
                            { "headerName1" , new List<string> { "headerValue1" } },
                            { "HEADERNAME2" , new List<string> { "headerValue22" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRepeatedH6")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "headerName1" , new List<string> { "headerValue1", "headerValue10" } },
                        }
                    },
                    new CreateSignedHttpRequestTheoryData("EmptyHeaders")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.H,
                        ExpectedClaimValue = $"{{\"h\":[[],\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash("")}\"]}}",
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateBClaimTheoryData))]
        public void CreateBClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateBClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddBClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateBClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidB1")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = $"{{\"b\":\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes("abcd"))}\"}}",
                        HttpRequestBody = Encoding.UTF8.GetBytes("abcd")
                    },
                    new CreateSignedHttpRequestTheoryData("ValidB2")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = $"{{\"b\":\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes(""))}\"}}",
                        HttpRequestBody = new byte[0]
                    },
                    new CreateSignedHttpRequestTheoryData("NullBytes")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.B,
                        ExpectedClaimValue = $"{{\"b\":\"{SignedHttpRequestTestUtils.CalculateBase64UrlEncodedHash(Encoding.UTF8.GetBytes(""))}\"}}",
                        HttpRequestBody = null
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateCnfClaimTheoryData))]
        public void CreateCnfClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateCnfClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddCnfClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
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
                    new CreateSignedHttpRequestTheoryData("ValidManualCnfClaim")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        Cnf = testCnf,
                        ExpectedClaimValue = $@"{{""cnf"":{testCnf}}}"
                    },
                    new CreateSignedHttpRequestTheoryData("ValidJwkRsaKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeyRsa_1024, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""cnf"":{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(KeyingMaterial.JsonWebKeyRsa_1024.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{KeyingMaterial.JsonWebKeyRsa_1024.E}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{KeyingMaterial.JsonWebKeyRsa_1024.N}""}}}}}}"
                    },
                    new CreateSignedHttpRequestTheoryData("ValidJwkECKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeyP256, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""cnf"":{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(KeyingMaterial.JsonWebKeyP256.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.Crv}"":""{KeyingMaterial.JsonWebKeyP256.Crv}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.EllipticCurve}"",""{JsonWebKeyParameterNames.X}"":""{KeyingMaterial.JsonWebKeyP256.X}"",""{JsonWebKeyParameterNames.Y}"":""{KeyingMaterial.JsonWebKeyP256.Y}""}}}}}}"
                    },
                    new CreateSignedHttpRequestTheoryData("InvalidJwkSymmetricKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.SymmetricSecurityKey2_1024, SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008", typeof(SignedHttpRequestCreationException))
                    },
                    new CreateSignedHttpRequestTheoryData("InvalidJwkSymmetricKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.JsonWebKeySymmetric128, SecurityAlgorithms.Aes128CbcHmacSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX23008", typeof(ArgumentException))
                    },
                    new CreateSignedHttpRequestTheoryData("ValidRsaKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.RsaSecurityKey1, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""cnf"":{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromRSASecurityKey(KeyingMaterial.RsaSecurityKey1).ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters1.Exponent)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{Base64UrlEncoder.Encode(KeyingMaterial.RsaParameters1.Modulus)}""}}}}}}"
                    },
                    new CreateSignedHttpRequestTheoryData("ValidX509Key")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.X509SecurityKey1, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""cnf"":{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(rsaJwkFromX509Key.ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.E}"":""{rsaJwkFromX509Key.E}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.RSA}"",""{JsonWebKeyParameterNames.N}"":""{rsaJwkFromX509Key.N}""}}}}}}"
                    },
#if NET472 || NET_CORE
                    new CreateSignedHttpRequestTheoryData("ValidEcdsaKey")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedClaimValue = $@"{{""cnf"":{{""{ConfirmationClaimTypes.Jwk}"":{{""{JsonWebKeyParameterNames.Kid}"":""{Base64UrlEncoder.Encode(JsonWebKeyConverter.ConvertFromECDsaSecurityKey(KeyingMaterial.Ecdsa256Key).ComputeJwkThumbprint())}"",""{JsonWebKeyParameterNames.Crv}"":""{ECDsaAdapter.GetCrvParameterValue(KeyingMaterial.Ecdsa256Parameters.Curve)}"",""{JsonWebKeyParameterNames.Kty}"":""{JsonWebAlgorithmsKeyTypes.EllipticCurve}"",""{JsonWebKeyParameterNames.X}"":""{Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.X)}"",""{JsonWebKeyParameterNames.Y}"":""{Base64UrlEncoder.Encode(KeyingMaterial.Ecdsa256Parameters.Q.Y)}""}}}}}}"
                    },
#else
                    new CreateSignedHttpRequestTheoryData("InvalidEcdsaKeyDesktop")
                    {
                        ExpectedClaim = ConfirmationClaimTypes.Cnf,
                        SigningCredentials = new SigningCredentials(KeyingMaterial.Ecdsa256Key, SecurityAlgorithms.EcdsaSha256, SecurityAlgorithms.Sha256),
                        ExpectedException = new ExpectedException(typeof(SignedHttpRequestCreationException), "IDX10674", typeof(NotSupportedException))
                    },
#endif
                };
            }
        }

        [Theory, MemberData(nameof(CreateNonceClaimTheoryData))]
        public void CreateNonceClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateNonceClaim", theoryData);
            Utf8JsonWriter writer = null;
            try
            {
                writer = theoryData.GetWriter();
                theoryData.Handler.AddNonceClaim(ref writer, theoryData.BuildSignedHttpRequestDescriptor());
                CheckClaimValue(ref writer, theoryData, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                writer?.Dispose();
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CreateSignedHttpRequestTheoryData> CreateNonceClaimTheoryData
        {
            get
            {
                return new TheoryData<CreateSignedHttpRequestTheoryData>
                {
                    new CreateSignedHttpRequestTheoryData("ValidDefaultNonce")
                    {
                        ExpectedClaim = SignedHttpRequestClaimTypes.Nonce,
                    },
                    new CreateSignedHttpRequestTheoryData("ValidCustomNonce")
                    {
                        ExpectedClaim = "nonce",
                        ExpectedClaimValue = $@"{{""nonce"":""nonce1""}}",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters(),
                        CustomNonceValue = "nonce1"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(CreateAdditionalClaimTheoryData))]
        public void CreateAdditionalClaim(CreateSignedHttpRequestTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateAdditionalClaim", theoryData);
            try
            {
                var handler = new SignedHttpRequestHandler();
                var signedHttpRequestDescriptor = theoryData.BuildSignedHttpRequestDescriptor();

                var payloadString =  handler.CreateHttpRequestPayload(signedHttpRequestDescriptor, theoryData.CallContext);
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
                    new CreateSignedHttpRequestTheoryData("ValidAdditionalClaim")
                    {
                        ExpectedClaim = "customClaim",
                        ExpectedClaimValue = "customClaimValue",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        },
                        AdditionalPayloadClaims = new Dictionary<string, object>() { { "customClaim", "customClaimValue" } }
                    },
                    new CreateSignedHttpRequestTheoryData("ValidAdditionalClaims")
                    {
                        ExpectedClaim = "customClaim",
                        ExpectedClaimValue = "customClaimValue",
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        },
                        AdditionalPayloadClaims = new Dictionary<string, object>() { { SignedHttpRequestClaimTypes.M, "will_not_be_overwritten" },  {"customClaim", "customClaimValue" } }
                    },
                    new CreateSignedHttpRequestTheoryData("AdditionalCustomClaimsNotSet")
                    {
                        SignedHttpRequestCreationParameters = new SignedHttpRequestCreationParameters()
                        {
                            CreateU = false,
                            CreateM = false,
                            CreateP = false,
                        }
                    }
                };
            }
        }

        internal static void CheckClaimValue(ref Utf8JsonWriter writer, CreateSignedHttpRequestTheoryData theoryData, CompareContext context)
        {
            writer.WriteEndObject();
            writer.Flush();
            string claim = Encoding.UTF8.GetString(theoryData.MemoryStream.ToArray());
            if (theoryData.ExpectedClaimValue != null)
                IdentityComparer.AreEqual(claim, theoryData.ExpectedClaimValue as string, context);
        }
    }

    public class CreateSignedHttpRequestTheoryData : TheoryDataBase
    {
        public CreateSignedHttpRequestTheoryData() { }

        public CreateSignedHttpRequestTheoryData(string testId) : base(testId) { }

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

        public SignedHttpRequestHandler Handler { get; set; } = new SignedHttpRequestHandler();

        public Dictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();

        public SigningCredentials SigningCredentials { get; set; } = SignedHttpRequestTestUtils.DefaultSigningCredentials;

        public string Token { get; set; } = SignedHttpRequestTestUtils.DefaultEncodedAccessToken;

        public string HeaderString { get; set; }

        public string PayloadString { get; set; }

        public string Cnf { get; set; }

        public MemoryStream MemoryStream { get; set; }

        public Utf8JsonWriter GetWriter()
        {
            MemoryStream = new MemoryStream();
            Utf8JsonWriter writer = new Utf8JsonWriter(MemoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            writer.WriteStartObject();

            return writer;
        }
    }
}
