namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HeadersUtilitiesTest
    {
        [TestMethod]
        public void HttpHeaderSanitizeShouldRemoveNonAsciiCharacters()
        {
            string input1 = "test-string-ø¥µé";
            string expected1 = "test-string-";
            string actual1 = HeadersUtilities.SanitizeString(input1);

            Assert.AreEqual(expected1, actual1);
        }

        [TestMethod]
        public void HttpHeaderSanitizeShouldRemoveNewlineCharacters()
        {
            string input1 = "test\nstring";
            string expected1 = "teststring";
            string actual1 = HeadersUtilities.SanitizeString(input1);

            Assert.AreEqual(expected1, actual1);
        }

        [TestMethod]
        public void HttpHeaderSanitizeValidateStrings()
        {
            string input1 = "27e073fd-bf45-4adb-8c12-07599e4dd990";
            string output1 = HeadersUtilities.SanitizeString(input1);

            Assert.AreEqual(input1, output1);

            string input2 = "27e073fd-bf45-4adb-8c12-07599e4dd990:app-id";
            string output2 = HeadersUtilities.SanitizeString(input2);

            Assert.AreEqual(input2, output2);
        }
    }
}
