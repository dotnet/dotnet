namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpRequestExtensionsTest
    {
        [TestMethod]
        public void GetUserHostAddressReturnsNullIfRequestIsNull()
        {
            HttpRequest wr = null;
            Assert.IsNull(wr.GetUserHostAddress());
        }

        [TestMethod]
        public void GetUserHostAddressReturnsNullInCaseOfArgumentException()
        {
            var ctx = HttpModuleHelper.GetFakeHttpContext(null, () => { throw new ArgumentException(); });

            Assert.IsNull(ctx.Request.GetUserHostAddress());
        }

        [TestMethod]
        public void GetUserHostAddressReturnsUserHostAddress()
        {
            var ctx = HttpModuleHelper.GetFakeHttpContext(null, () => { return "123"; });

            Assert.AreEqual("123", ctx.Request.GetUserHostAddress());
        }
    }
}
