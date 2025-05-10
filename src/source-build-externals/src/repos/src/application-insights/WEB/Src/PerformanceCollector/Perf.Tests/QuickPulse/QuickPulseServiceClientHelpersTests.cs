#if NETCOREAPP
namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Tests.QuickPulse
{
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.Common.Internal;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseServiceClientHelpersTests
    {
        private const int QuickPulseResponseHeaderHeaderMaxLength = InjectionGuardConstants.QuickPulseResponseHeaderMaxLength;
        private const string SecretString = "12345abcd";
        private const string HeaderName = "myheader";
        private const string FakeHeaderName = "fake";

        [TestMethod]
        public void VerifyHeaderGetValue()
        {
            // setup
            var headers = new TestHttpHeaders();
            foreach (string headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
            {
                headers.Add(headerName, headerName + SecretString);
            }

            // assert
            foreach (string headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
            {
                var value = headers.GetValueSafe(headerName);
                Assert.AreEqual(headerName + SecretString, value);
            }
        }

        [TestMethod]
        public void VerifyHeaderGetValues()
        {
            // setup
            var headers = new TestHttpHeaders();
            headers.Add(HeaderName, "one");
            headers.Add(HeaderName, "two");
            headers.Add(HeaderName, "three");

            // assert
            var result = headers.GetValueSafe(HeaderName);
            Assert.AreEqual("one", result);

            var result2 = headers.GetValueSafe(FakeHeaderName);
            Assert.AreEqual(default(string), result2);
        }

        [TestMethod]
        public void VerifyHeadersLengthIsProtected()
        {
            // setup
            var headers = new TestHttpHeaders();
            headers.Add(HeaderName, new string('*', QuickPulseResponseHeaderHeaderMaxLength));

            // assert
            var result = headers.GetValueSafe(HeaderName);
            Assert.AreEqual(QuickPulseResponseHeaderHeaderMaxLength, result.Length);
        }

        [TestMethod]
        public void VerifyInvalidHeadersLengthIsProtected()
        {
            // setup
            var headers = new TestHttpHeaders();
            headers.Add(HeaderName, new string('*', QuickPulseResponseHeaderHeaderMaxLength + 1));

            // assert
            var result = headers.GetValueSafe(HeaderName);
            Assert.AreEqual(QuickPulseResponseHeaderHeaderMaxLength, result.Length);
        }

        /// <summary>
        /// HttpHeaders is an abstract class. I need to initialize my own class for tests. The class I'm testing is built on HttpHeaders so this is fine.
        /// </summary>
        private class TestHttpHeaders : HttpHeaders
        {
        }
    }
}
#endif