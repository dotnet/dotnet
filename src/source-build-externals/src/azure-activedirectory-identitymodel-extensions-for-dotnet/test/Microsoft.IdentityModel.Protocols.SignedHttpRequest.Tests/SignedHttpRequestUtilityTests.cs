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
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestUtilityTests
    {
        [Fact]
        public void CreateSignedHttpRequestHeader()
        {
            Assert.Throws<ArgumentNullException>(() => SignedHttpRequestUtilities.CreateSignedHttpRequestHeader(null));
            Assert.Throws<ArgumentNullException>(() => SignedHttpRequestUtilities.CreateSignedHttpRequestHeader(string.Empty));
            Assert.Equal("PoP abcd", SignedHttpRequestUtilities.CreateSignedHttpRequestHeader("abcd"));
        }

        [Theory, MemberData(nameof(AppendHeadersTheoryData))]
        public void AppendHeaders(SignedHttpRequestUtilityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.AppendHeaders", theoryData);
            try
            {
                var httpRequestData = new HttpRequestData
                {
                    Headers = theoryData.HttpRequestHeaders
                };

                httpRequestData.AppendHeaders(theoryData.HttpHeaders);
                IdentityComparer.AreStingEnumDictionariesEqual(httpRequestData.Headers, theoryData.ExpectedHttpRequestHeaders, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(CreateJwkClaimTheoryData))]
        public void CreateJwkClaim(SignedHttpRequestUtilityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CreateJwkClaim", theoryData);
            try
            {
                var jwkClaim = SignedHttpRequestUtilities.CreateJwkClaim(theoryData.JsonWebKey);

                if (!string.IsNullOrEmpty(theoryData.ExpectedJwkClaim))
                    IdentityComparer.AreStringsEqual(jwkClaim, theoryData.ExpectedJwkClaim, context);

                var jwkJwt = JObject.Parse(jwkClaim);
                var privateKeyPropertyNames = new List<string>()
                {
                    JsonWebKeyParameterNames.D,
                    JsonWebKeyParameterNames.DP,
                    JsonWebKeyParameterNames.DQ,
                    JsonWebKeyParameterNames.Oth,
                    JsonWebKeyParameterNames.P,
                    JsonWebKeyParameterNames.Q,
                    JsonWebKeyParameterNames.QI,
                };

                foreach (var privateKeyPropertyName in privateKeyPropertyNames)
                {
                    if (jwkJwt.ContainsKey(privateKeyPropertyName))
                        context.AddDiff($"The resulting jwk claim contains '{privateKeyPropertyName}' field, that represents a private key.");
                }

                if (new JsonWebKey(jwkClaim).HasPrivateKey)
                    context.AddDiff($"The resulting jwk claim contains a private key.");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignedHttpRequestUtilityTheoryData> CreateJwkClaimTheoryData
        {
            get
            {
                var rsaKey = KeyingMaterial.DefaultX509Key_2048.PrivateKey as RSA;
                var rsaParams = rsaKey.ExportParameters(true);
                var jsonWebKeyP256_NoKid = KeyingMaterial.JsonWebKeyP256;
                jsonWebKeyP256_NoKid.Kid = string.Empty;
                var jsonWebKeyP256_NoX = KeyingMaterial.JsonWebKeyP256;
                jsonWebKeyP256_NoX.X = string.Empty;

                return new TheoryData<SignedHttpRequestUtilityTheoryData>
                {
                    new SignedHttpRequestUtilityTheoryData
                    {
                        First = true,
                        JsonWebKey = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidJsonWebKeyNull",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeySymmetric128,
                        ExpectedException = ExpectedException.ArgumentException("IDX10707"),
                        TestId = "InvalidKty",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_Public,
                        ExpectedException  = ExpectedException.ArgumentException("IDX10709"),
                        TestId = "InvalidRsaNoExponent",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey =jsonWebKeyP256_NoX,
                        ExpectedException  = ExpectedException.ArgumentException("IDX10708"),
                        TestId = "InvalidEcNoX",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyP256_Public,
                        TestId = "ValidNoPrivateInfo1",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyP384,
                        TestId = "ValidNoPrivateInfo2",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyRsa_1024,
                        TestId = "ValidNoPrivateInfo3",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyP256,
                        ExpectedJwkClaim = $@"{{""jwk"":{{""kid"":""JsonWebKeyP256"",""crv"":""P-256"",""kty"":""EC"",""x"":""{KeyingMaterial.P256_X}"",""y"":""{KeyingMaterial.P256_Y}""}}}}",
                        TestId = "ValidEC256",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        JsonWebKey = KeyingMaterial.JsonWebKeyX509_2048_As_RSA,
                        ExpectedJwkClaim = $@"{{""jwk"":{{""kid"":""{KeyingMaterial.DefaultCert_2048.Thumbprint}"",""e"":""{Base64UrlEncoder.Encode(rsaParams.Exponent)}"",""kty"":""RSA"",""n"":""{Base64UrlEncoder.Encode(rsaParams.Modulus)}""}}}}",
                        TestId = "ValidRsa",
                    },
                };
            }
        }

        public static TheoryData<SignedHttpRequestUtilityTheoryData> AppendHeadersTheoryData
        {
            get
            {
                return new TheoryData<SignedHttpRequestUtilityTheoryData>
                {
                    new SignedHttpRequestUtilityTheoryData
                    {
                        First = true,
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        HttpHeaders = null,
                        ExpectedHttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        TestId = "Valid1",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                        },
                        HttpHeaders = null,
                        ExpectedHttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                        },
                        TestId = "Valid2",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>(),
                        HttpHeaders = SignedHttpRequestTestUtils.CreateHttpHeaders(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string> ("h1", "value1")
                        }),
                        ExpectedHttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "h1", new List<string> { "value1" } },
                        },
                        TestId = "Valid3",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                        },
                        HttpHeaders = SignedHttpRequestTestUtils.CreateHttpHeaders(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string> ("h2", "value2")
                        }),
                        ExpectedHttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                            { "h2", new List<string> { "value2" } },
                        },
                        TestId = "Valid4",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                            { "h1", new List<string> { "value1" } },
                        },
                        HttpHeaders = SignedHttpRequestTestUtils.CreateHttpHeaders(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string> ("h1", "value2"),
                            new KeyValuePair<string, string> ("h2", "value2")
                        }),
                        ExpectedHttpRequestHeaders = new Dictionary<string, IEnumerable<string>>()
                        {
                            { "Content-Type", new List<string> { "application/json" } },
                            { "h2", new List<string> { "value2" } },
                            { "h1", new List<string> { "value1", "value2" } },
                        },
                        TestId = "Valid5",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestHeaders = null,
                        HttpHeaders = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidNullHeaders",
                    },
                };
            }
        }

        [Theory, MemberData(nameof(ToHttpRequestDataAsyncTheoryData))]
        public async Task ToHttpRequestDataAsync(SignedHttpRequestUtilityTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ToHttpRequestDataAsync", theoryData);
            try
            {
                var httpRequestData = await SignedHttpRequestUtilities.ToHttpRequestDataAsync(theoryData.HttpRequestMessage).ConfigureAwait(false);
                IdentityComparer.AreStringsEqual(httpRequestData.Method, theoryData.ExpectedHttpRequestData.Method, context);
                IdentityComparer.AreUrisEqual(httpRequestData.Uri, theoryData.ExpectedHttpRequestData.Uri, context);
                IdentityComparer.AreBytesEqual(httpRequestData.Body, theoryData.ExpectedHttpRequestData.Body, context);
                IdentityComparer.AreStingEnumDictionariesEqual(httpRequestData.Headers, theoryData.ExpectedHttpRequestData.Headers, context);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignedHttpRequestUtilityTheoryData> ToHttpRequestDataAsyncTheoryData
        {
            get
            {
                return new TheoryData<SignedHttpRequestUtilityTheoryData>
                {
                    new SignedHttpRequestUtilityTheoryData
                    {
                        First = true,
                        HttpRequestMessage = null,
                        ExpectedHttpRequestData = null,
                        ExpectedException = ExpectedException.ArgumentNullException(),
                        TestId = "InvalidttpRequestMessageNull",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, string.Empty),
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "GET",
                            Headers = new Dictionary<string, IEnumerable<string>>()
                        },
                        TestId = "Valid1",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("https://www.contoso.com/path1&qp1=value1")),
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "POST",
                            Uri = new Uri("https://www.contoso.com/path1&qp1=value1"),
                            Headers = new Dictionary<string, IEnumerable<string>>()
                        },
                        TestId = "Valid2",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("https://www.contoso.com/path1&qp1=val ue1")),
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "POST",
                            Uri = new Uri("https://www.contoso.com/path1&qp1=val ue1"),
                            Headers = new Dictionary<string, IEnumerable<string>>()
                        },
                        TestId = "Valid3",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = new HttpRequestMessage()
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri("https://www.contoso.com/"),
                        },
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Uri = new Uri("https://www.contoso.com/"),
                            Headers = new Dictionary<string, IEnumerable<string>>(),
                            Method = "GET",
                        },
                        TestId = "Valid4",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = SignedHttpRequestTestUtils.CreateHttpRequestMessage(
                            HttpMethod.Get,
                            new Uri("https://www.contoso.com/"),
                            new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string> ("h1", "value1"),
                                new KeyValuePair<string, string> ("h2", "value2")
                            },
                            null
                        ),
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "GET",
                            Uri = new Uri("https://www.contoso.com/"),
                            Headers = new Dictionary<string, IEnumerable<string>>()
                            {
                                { "h1", new List<string> { "value1" } },
                                { "h2", new List<string> { "value2" } },
                            },
                        },
                        TestId = "Valid5",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = SignedHttpRequestTestUtils.CreateHttpRequestMessage(
                            HttpMethod.Get, 
                            new Uri("https://www.contoso.com/"), 
                            new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string> ("h1", "value1"),
                                new KeyValuePair<string, string> ("h2", "value2")
                            },
                            Encoding.UTF8.GetBytes("abcd")
                        ), 
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "GET",
                            Uri = new Uri("https://www.contoso.com/"),
                            Headers = new Dictionary<string, IEnumerable<string>>()
                            {
                                { "h1", new List<string> { "value1" } },
                                { "h2", new List<string> { "value2" } },
                                { "Content-Length", new List<string> { "abcd".Length.ToString() } },
                            },
                            Body = Encoding.UTF8.GetBytes("abcd")
                        },
                        TestId = "Valid6",
                    },
                    new SignedHttpRequestUtilityTheoryData
                    {
                        HttpRequestMessage = SignedHttpRequestTestUtils.CreateHttpRequestMessage(
                            HttpMethod.Get,
                            new Uri("https://www.contoso.com/"),
                            new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string> ("h1", "value1"),
                                new KeyValuePair<string, string> ("h2", "value2")
                            },
                            Encoding.UTF8.GetBytes("abcd"),
                            new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string> ("Content-Type", "application/json"),
                            }
                        ),
                        ExpectedHttpRequestData = new HttpRequestData()
                        {
                            Method = "GET",
                            Uri = new Uri("https://www.contoso.com/"),
                            Headers = new Dictionary<string, IEnumerable<string>>()
                            {
                                { "h1", new List<string> { "value1" } },
                                { "h2", new List<string> { "value2" } },
                                { "Content-Type",  new List<string> { "application/json" } },
                                { "Content-Length", new List<string> { "abcd".Length.ToString() } },
                            },
                            Body = Encoding.UTF8.GetBytes("abcd"),
                        },
                        TestId = "Valid7",
                    },
                };
            }
        }
    }

    public class SignedHttpRequestUtilityTheoryData : TheoryDataBase
    {
        public CallContext CallContext { get; set; } = new CallContext();

        public Uri HttpRequestUri { get; set; }

        public string HttpRequestMethod { get; set; }

        public JsonWebKey JsonWebKey { get; set; }

        public IDictionary<string, IEnumerable<string>> HttpRequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public string ExpectedJwkClaim { get; set; }

        public IDictionary<string, IEnumerable<string>> ExpectedHttpRequestHeaders { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public HttpRequestData ExpectedHttpRequestData { get; set; }

        public byte[] HttpRequestBody { get; set; }

        public HttpHeaders HttpHeaders { get; set; }

        public HttpRequestMessage HttpRequestMessage { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
