namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the HttpHeadersUtilities class.
    /// </summary>
    [TestClass]
    public class HttpHeadersUtilitiesTests
    {
        /// <summary>
        /// Ensure that GetHeaderValues() returns an empty IEnumerable when the headers argument is null.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithNullHeaders()
        {
            EnumerableAssert.AreEqual(Enumerable.Empty<string>(), HttpHeadersUtilities.GetHeaderValues(null, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderValues() returns an empty IEnumerable when the headers argument is empty.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithEmptyHeaders()
        {
            HttpHeaders headers = CreateHeaders();
            EnumerableAssert.AreEqual(Enumerable.Empty<string>(), HttpHeadersUtilities.GetHeaderValues(headers, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderValues() returns an IEnumerable that contains the key value when the headers argument contains the key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithMatchingHeader()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("MOCK_HEADER_NAME", "MOCK_HEADER_VALUE");
            EnumerableAssert.AreEqual(new[] { "MOCK_HEADER_VALUE" }, HttpHeadersUtilities.GetHeaderValues(headers, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderValues() returns an IEnumerable that contains all of the values when the headers argument contains multiple values for the key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderValuesWithMultipleMatchingHeaders()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("MOCK_HEADER_NAME", "A");
            headers.Add("MOCK_HEADER_NAME", "B");
            headers.Add("MOCK_HEADER_NAME", "C");
            EnumerableAssert.AreEqual(new[] { "A", "B", "C" }, HttpHeadersUtilities.GetHeaderValues(headers, "MOCK_HEADER_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns null when the headers argument is null.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithNullHeaders()
        {
            Assert.AreEqual(null, HttpHeadersUtilities.GetHeaderKeyValue(null, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns null when the headers argument is empty.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithEmptyHeaders()
        {
            HttpHeaders headers = CreateHeaders();
            Assert.AreEqual(null, HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns key value when the headers argument contains header key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithMatchingHeader()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "KEY_NAME=KEY_VALUE");
            Assert.AreEqual("KEY_VALUE", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns first key value when the headers argument contains multiple key name/value pairs for header name.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithMultipleMatchingHeaderNamesButOnlyOneMatchingKeyName()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "A=a");
            headers.Add("HEADER_NAME", "B=b");
            headers.Add("HEADER_NAME", "C=c");
            Assert.AreEqual("b", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "B"));
        }

        /// <summary>
        /// Ensure that GetHeaderKeyValue() returns first key value when the headers argument contains multiple key values for the key name.
        /// </summary>
        [TestMethod]
        public void GetHeaderKeyValuesWithMultipleMatchingHeaderNamesAndMultipleMatchingKeyNames()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "A=a");
            headers.Add("HEADER_NAME", "B=b");
            headers.Add("HEADER_NAME", "C=c1");
            headers.Add("HEADER_NAME", "C=c2");
            Assert.AreEqual("c1", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "C"));
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() throws an ArgumentNullException when headers is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetHeaderKeyValueWithNullHeaders()
        {
            HttpHeadersUtilities.SetHeaderKeyValue(null, "HEADER_NAME", "KEY_NAME", "KEY_VALUE");
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() sets the proper key value when the headers argument is empty.
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithEmptyHeaders()
        {
            HttpHeaders headers = CreateHeaders();
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME", "KEY_VALUE");
            Assert.AreEqual("KEY_VALUE", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() overwrites an existing key value.
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithMatchingHeader()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "KEY_NAME=KEY_VALUE1");
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME", "KEY_VALUE2");
            Assert.AreEqual("KEY_VALUE2", HttpHeadersUtilities.GetHeaderKeyValue(headers, "HEADER_NAME", "KEY_NAME"));
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() overwrites an existing key value when multiple key name/value pairs exist for a single header.
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithMultipleMatchingHeaderNamesButOnlyOneMatchingKeyName()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "A=a1");
            headers.Add("HEADER_NAME", "B=b1");
            headers.Add("HEADER_NAME", "C=c1");
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "HEADER_NAME", "B", "b2");
            EnumerableAssert.AreEqual(new[] { "A=a1", "C=c1", "B=b2" }, HttpHeadersUtilities.GetHeaderValues(headers, "HEADER_NAME"));
            Assert.IsTrue(HttpHeadersUtilities.ContainsHeaderKeyValue(headers, "HEADER_NAME", "B"));
        }

        /// <summary>
        /// Ensure that SetHeaderKeyValue() overwrites all existing key values.
        /// </summary>
        [TestMethod]
        public void SetHeaderKeyValueWithMultipleMatchingHeaderNamesAndMultipleMatchingKeyNames()
        {
            HttpHeaders headers = CreateHeaders();
            headers.Add("HEADER_NAME", "A=a");
            headers.Add("HEADER_NAME", "B=b");
            headers.Add("HEADER_NAME", "C=c1");
            headers.Add("HEADER_NAME", "C=c2");
            HttpHeadersUtilities.SetHeaderKeyValue(headers, "HEADER_NAME", "C", "c3");
            EnumerableAssert.AreEqual(new[] { "A=a", "B=b", "C=c3" }, HttpHeadersUtilities.GetHeaderValues(headers, "HEADER_NAME"));
        }

        /// <summary>
        /// Create a HttpHeaders object for testing.
        /// </summary>
        private static HttpHeaders CreateHeaders()
        {
            HttpHeaders result = new HttpRequestMessage().Headers;
            Assert.IsNotNull(result);
            return result;
        }
    }
}
