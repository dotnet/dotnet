namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DiagnosticListeners;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Xunit;

    public class HttpHeadersUtilitiesTest
    {
        [Fact]
        public void GetHeaderValuesShouldReturnHeader()
        {
            IHeaderDictionary headerDictionary = new HeaderDictionary(
                new Dictionary<string, StringValues>() {
                    { "HeaderName", new StringValues("app=id") },
                    { "NoizyName", new StringValues("noizy=noizy-id") }
            });

            IEnumerable<string> headerValues = HttpHeadersUtilities.GetHeaderValues(headerDictionary, "HeaderName");

            Assert.NotNull(headerValues);
            Assert.True(headerValues.Count() == 1);
            Assert.Equal("app=id", headerValues.First());
        }

        [Fact]
        public void GetHeaderKeyValueShouldReturnValue()
        {
            IHeaderDictionary headerDictionary = new HeaderDictionary(
                new Dictionary<string, StringValues>() {
                                { "HeaderName", new StringValues("app=id") },
                                { "NoizyName", new StringValues("noizy=noizy-id") }
            });

            string actual = HttpHeadersUtilities.GetHeaderKeyValue(headerDictionary, "HeaderName", "app");

            Assert.Equal("id", actual);
        }

        [Fact]
        public void GetRequestContextKeyValueShouldReturnContextKeyValue()
        {
            IHeaderDictionary headerDictionary = new HeaderDictionary(
                new Dictionary<string, StringValues>()
                {
                    {RequestResponseHeaders.RequestContextHeader, new StringValues("app=id")},
                    {"NoizyName", new StringValues("noizy=noizy-id")}
                });

            string actual = HttpHeadersUtilities.GetRequestContextKeyValue(headerDictionary, "app");

            Assert.Equal("id", actual);
        }

        [Fact]
        public void SetHeaderKeyValueShouldRequireHeaders()
        {
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                IHeaderDictionary nullDictionary = null;
                HttpHeadersUtilities.SetHeaderKeyValue(nullDictionary, "header", "key", "value");
            });
        }

        [Fact]
        public void SetHeaderKeyValueShouldAddTheHeader()
        {
            IHeaderDictionary headers = new HeaderDictionary();
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "newHeader", "key", "value");

            Assert.Equal(1, headers.Count);

            Assert.Equal("newHeader", headers.First().Key);
            Assert.Equal("key=value", headers.First().Value);
        }

        [Fact]
        public void SetHeaderKeyValuesShouldAppendToExistingHeaders()
        {
            IHeaderDictionary headers = new HeaderDictionary(
                new Dictionary<string, StringValues>() { { "HeaderName", new StringValues("app=id") } });
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "newHeader", "key", "value");

            Assert.Equal(2, headers.Count);

            Assert.Equal("HeaderName", headers.First().Key);
            Assert.Equal("app=id", headers.First().Value);
            Assert.Equal("newHeader", headers.Skip(1).First().Key);
            Assert.Equal("key=value", headers.Skip(1).First().Value);
        }

        [Fact]
        public void SetHeaderKeyValuesShouldUpdateExistingHeaders()
        {
            IHeaderDictionary headers = new HeaderDictionary(
                new Dictionary<string, StringValues>() { { "HeaderName", new StringValues("app=id") } });
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "HeaderName", "key", "value");

            Assert.Equal(1, headers.Count);

            Assert.Equal("HeaderName", headers.First().Key);
            Assert.Equal("app=id,key=value", headers.First().Value);
        }

        [Fact]
        public void SetRequestContextKeyValueShouldSetTheHeaders()
        {
            IHeaderDictionary headers = new HeaderDictionary();
            HttpHeadersUtilities.SetRequestContextKeyValue(headers, "key", "value");

            Assert.Equal(1, headers.Count);

            Assert.Equal(RequestResponseHeaders.RequestContextHeader, headers.First().Key);
            Assert.Equal("key=value", headers.First().Value);
        }

        [Fact]
        public void ContainsRequestContextKeyValueShouldReturnTrueWhenExists()
        {
            IHeaderDictionary headers = new HeaderDictionary(
                new Dictionary<string, StringValues> { { RequestResponseHeaders.RequestContextHeader, new StringValues("app=id,other=otherValue") } });
            Assert.True(HttpHeadersUtilities.ContainsRequestContextKeyValue(headers, "app"));
            Assert.True(HttpHeadersUtilities.ContainsRequestContextKeyValue(headers, "other"));
            Assert.False(HttpHeadersUtilities.ContainsRequestContextKeyValue(headers, "Non-exists"));
        }

        [Fact]
        public void GetHeaderValueEmpty()
        {
            IHeaderDictionary headers = new HeaderDictionary();
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, 100500, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Empty(values);
        }

        [Fact]
        public void GetHeaderValueNoMax1()
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2" });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, 100500, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Equal(2, values.Count);
            Assert.Equal("k1=v1", values.First());
            Assert.Equal("k2=v2", values.Last());
        }

        [Fact]
        public void GetHeaderValueNoMax2()
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = new []{"k1=v1,k2=v2", "k3=v3,k4=v4" }});
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, 100500, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Equal(4, values.Count);
            Assert.Equal("k1=v1", values[0]);
            Assert.Equal("k2=v2", values[1]);
            Assert.Equal("k3=v3", values[2]);
            Assert.Equal("k4=v4", values[3]);
        }

        [Theory]
        [InlineData(12)] // k1=v1,k2=v2,".Length
        [InlineData(11)] // k1=v1,k2=v2".Length
        [InlineData(15)] // k1=v1,k2=v2,k3=".Length
        [InlineData(13)] // k1=v1,k2=v2,k".Length
        public void GetHeaderValueMaxLenTruncatesEnd(int maxLength)
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2,k3=v3,k4=v4" });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, maxLength, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Equal(2, values.Count);
            Assert.Equal("k1=v1", values.First());
            Assert.Equal("k2=v2", values.Last());
        }

        [Theory]
        [InlineData(12)] // k1=v1,k2=v2,".Length
        [InlineData(11)] // k1=v1,k2=v2".Length
        [InlineData(15)] // k1=v1,k2=v2,k3=".Length
        [InlineData(13)] // k1=v1,k2=v2,k".Length
        public void GetHeaderValueMaxLenTruncatesEnd2(int maxLength)
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = new[] { "k1=v1,k2=v2", "k3=v3,k4=v4" } });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, maxLength, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Equal(2, values.Count);
            Assert.Equal("k1=v1", values.First());
            Assert.Equal("k2=v2", values.Last());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void GetHeaderValueMaxLenTruncatesEndInvalid(int maxLength)
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2" });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, maxLength, 100500)?.ToList();
            Assert.NotNull(values);
            Assert.Empty(values);
        }

        [Fact]
        public void GetHeaderValueMaxItemsTruncatesEndInvalid()
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2" });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, 100500, 0)?.ToList();
            Assert.NotNull(values);
            Assert.Empty(values);
        }

        [Fact]
        public void GetHeaderValueMaxItemsTruncatesEnd()
        {
            IHeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues> { [W3C.W3CConstants.TraceStateHeader] = "k1=v1,k2=v2,k3=v3,k4=v4" });
            var values = HttpHeadersUtilities.SafeGetCommaSeparatedHeaderValues(headers, W3C.W3CConstants.TraceStateHeader, 100500, 2)?.ToList();
            Assert.NotNull(values);
            Assert.Equal(2, values.Count);
            Assert.Equal("k1=v1", values.First());
            Assert.Equal("k2=v2", values.Last());
        }
    }
}
