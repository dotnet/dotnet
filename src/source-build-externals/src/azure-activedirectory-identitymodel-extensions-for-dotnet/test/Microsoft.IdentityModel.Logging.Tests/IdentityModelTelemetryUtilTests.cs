
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
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Logging.Tests
{
    public class IdentityModelTelemetryUtilTests
    {
        [Theory, MemberData(nameof(SetTelemetryDataTheoryData), DisableDiscoveryEnumeration = true)]
        public void SetTelemetry(TelemetryTheoryData theoryData)
        {
            var testContext = new CompareContext();

            try
            {
                IdentityModelTelemetryUtil.SetTelemetryData(theoryData.HttpRequestMessage, theoryData.AdditionalHeaders);
                // check if the resulting headers are as expected
                if (!IdentityComparer.AreEqual(theoryData.ExpectedHeaders, theoryData.HttpRequestMessage?.Headers))
                    throw new ArgumentException("resulting headers do not match the expected headers.");

                theoryData.ExpectedException.ProcessNoException(testContext);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, testContext);
            }

            TestUtilities.AssertFailIfErrors(testContext);
        }

        public static TheoryData<TelemetryTheoryData> SetTelemetryDataTheoryData
        {
            get
            {
                return new TheoryData<TelemetryTheoryData>
                {
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = null,
                        TestId = "nullRequestMessage"
                    },
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = BuildHttpRequestMessage(),
                        ExpectedHeaders = BuildHttpRequestHeaders(),
                        TestId = "noAdditionalHeaders"
                    },
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = BuildHttpRequestMessage(new Dictionary<string, string> { {"header1", "value1"} }),
                        ExpectedHeaders = BuildHttpRequestHeaders(new Dictionary<string, string> { {"header1", "value1"} }),
                        TestId = "withAdditionalIncomingHeaders"
                    },
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = BuildHttpRequestMessage(new Dictionary<string, string> { { IdentityModelTelemetryUtil.skuTelemetry, "some-other-value"} }),
                        ExpectedHeaders = BuildHttpRequestHeaders(),
                        TestId = "overwriteExistingHeaders"
                    },
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = BuildHttpRequestMessage(new Dictionary<string, string> { { "header1", "value1" }, { IdentityModelTelemetryUtil.skuTelemetry, "some-other-value"}, {"header2", "value2"} }),
                        ExpectedHeaders = BuildHttpRequestHeaders(new Dictionary<string, string> { {"header1", "value1"}, {"header2", "value2"} }),
                        TestId = "overwriteExistingButKeepOtherHeaders"
                    },
                    new TelemetryTheoryData
                    {
                        HttpRequestMessage = BuildHttpRequestMessage(),
                        ExpectedHeaders = BuildHttpRequestHeaders(new Dictionary<string, string> { {"header1", "value1"} }),
                        AdditionalHeaders = new Dictionary<string, string> { { "header1", "value1" } },
                        TestId = "withAdditionalInputHeader"
                    },
                };
            }
        }

        [Fact]
        public void AddTelemetryDataTest()
        {
            IdentityModelTelemetryUtil.AddTelemetryData("parameter1", "value1");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1") && IdentityModelTelemetryUtil.telemetryData["parameter1"] == "value1");
            IdentityModelTelemetryUtil.AddTelemetryData("parameter1", "value2");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1") && IdentityModelTelemetryUtil.telemetryData["parameter1"] == "value2");

            Assert.False(IdentityModelTelemetryUtil.AddTelemetryData(null, "value1"));
            Assert.False(IdentityModelTelemetryUtil.AddTelemetryData(IdentityModelTelemetryUtil.skuTelemetry, "value1"));
            Assert.False(IdentityModelTelemetryUtil.AddTelemetryData(IdentityModelTelemetryUtil.versionTelemetry, "value1"));
            Assert.False(IdentityModelTelemetryUtil.AddTelemetryData(IdentityModelTelemetryUtil.skuTelemetry, null));
        }

        [Fact]
        public void RemoveTelemetryDataTest()
        {
            IdentityModelTelemetryUtil.AddTelemetryData("parameter1", "value1");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1") && IdentityModelTelemetryUtil.telemetryData["parameter1"] == "value1");
            IdentityModelTelemetryUtil.RemoveTelemetryData("parameter1");
            Assert.True(!IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1"));

            Assert.False(IdentityModelTelemetryUtil.RemoveTelemetryData(null));
            Assert.False(IdentityModelTelemetryUtil.RemoveTelemetryData(IdentityModelTelemetryUtil.skuTelemetry));
            Assert.False(IdentityModelTelemetryUtil.RemoveTelemetryData(IdentityModelTelemetryUtil.versionTelemetry));
        }

        [Fact]
        public void UpdateTelemetryDataTest()
        {
            IdentityModelTelemetryUtil.UpdateDefaultTelemetryData("parameter1", "value1");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1") && IdentityModelTelemetryUtil.telemetryData["parameter1"] == "value1");
            IdentityModelTelemetryUtil.UpdateDefaultTelemetryData("parameter1", "value2");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey("parameter1") && IdentityModelTelemetryUtil.telemetryData["parameter1"] == "value2");

            IdentityModelTelemetryUtil.UpdateDefaultTelemetryData(IdentityModelTelemetryUtil.skuTelemetry, "value1");
            Assert.True(IdentityModelTelemetryUtil.telemetryData.ContainsKey(IdentityModelTelemetryUtil.skuTelemetry) && IdentityModelTelemetryUtil.telemetryData[IdentityModelTelemetryUtil.skuTelemetry] == "value1");

            Assert.False(IdentityModelTelemetryUtil.UpdateDefaultTelemetryData(IdentityModelTelemetryUtil.skuTelemetry, null));
            Assert.False(IdentityModelTelemetryUtil.UpdateDefaultTelemetryData(null, "value1"));
        }
        private static HttpRequestHeaders BuildHttpRequestHeaders(IDictionary<string, string> additionalHeaders = null, bool addDefaultTelemetryData = true)
        {
            var headers = new HttpClient().DefaultRequestHeaders;

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                    headers.Add(header.Key, header.Value);
            }

            if (addDefaultTelemetryData)
            {
                foreach (var header in IdentityModelTelemetryUtil.telemetryData)
                    headers.Add(header.Key, header.Value);
            }

            return headers;
        }

        private static HttpRequestMessage BuildHttpRequestMessage(IDictionary<string, string> headers = null, string defaultUri = "https://gotJwt.onmicrosoft.com/signedIn")
        {
            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(defaultUri),
            };

            if (headers != null)
            {
                foreach (var header in headers)
                    message.Headers.Add(header.Key, header.Value);
            }

            return message;
        }

        public class TelemetryTheoryData : TheoryDataBase
        {
            public TelemetryTheoryData(): base(false)
            {}
            public bool SendTelemetry { get; set; } = true;

            public HttpRequestMessage HttpRequestMessage { get; set; }

            public HttpRequestHeaders ExpectedHeaders { get; set; }

            public IDictionary<string, string> AdditionalHeaders { get; set; }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
