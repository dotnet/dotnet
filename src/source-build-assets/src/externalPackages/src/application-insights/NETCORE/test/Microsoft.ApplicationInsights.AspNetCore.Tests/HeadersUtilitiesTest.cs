namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System.Collections.Generic;
    using DiagnosticListeners;
    using Xunit;

    public class HeadersUtilitiesTest
    {
        [Fact]
        public void ShouldReturnHeaderValueWhenKeyExists()
        {
            List<string> headers = new List<string>() {
                "Key = Value"
            };

            string expectedKey = "Value";
            Assert.Equal(expectedKey, HeadersUtilities.GetHeaderKeyValue(headers, "Key"));
        }

        [Fact]
        public void ShouldReturnNullWhenKeyNotExists()
        {
            List<string> headers = new List<string>() {
                "Key = Value"
            };
            Assert.Null(HeadersUtilities.GetHeaderKeyValue(headers, "Non-Exist-Key"));
        }

        [Fact]
        public void ShouldReturnHeadersWhenNoHeaderValues()
        {
            string[] newHeaders = HeadersUtilities.SetHeaderKeyValue(null, "Key", "Value");

            Assert.NotNull(newHeaders);
            Assert.Single(newHeaders);
            Assert.Equal("Key=Value", newHeaders[0]);
        }

        [Fact]
        public void ShouldAppendHeaders()
        {
            string[] existing = new string[] { "ExistKey=ExistValue" };
            string[] result = HeadersUtilities.SetHeaderKeyValue(existing, "NewKey", "NewValue");

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("ExistKey=ExistValue", result[0]);
            Assert.Equal("NewKey=NewValue", result[1]);
        }

        [Fact]
        public void ShouldUpdateExistingHeader()
        {
            string[] existing = new string[] { "ExistKey=ExistValue", "NoiseKey=NoiseValue" };
            string[] result = HeadersUtilities.SetHeaderKeyValue(existing, "ExistKey", "NewValue");

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("ExistKey=NewValue", result[0]);
            Assert.Equal("NoiseKey=NoiseValue", result[1]);
        }
    }
}
