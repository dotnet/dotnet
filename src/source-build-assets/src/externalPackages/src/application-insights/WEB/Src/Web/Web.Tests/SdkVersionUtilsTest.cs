namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SdkVersionUtilsTest
    {
        [TestMethod]
        public void GetSdkVersionReturnsVersionWithoutPrefixForNull()
        {
            string expected = SdkVersionHelper.GetExpectedSdkVersion(typeof(RequestTrackingTelemetryModule), prefix: string.Empty);

            Assert.AreEqual(expected, SdkVersionUtils.GetSdkVersion(null));
        }

        [TestMethod]
        public void GetSdkVersionReturnsVersionWithoutPrefixForStringEmpty()
        {
            string expected = SdkVersionHelper.GetExpectedSdkVersion(typeof(RequestTrackingTelemetryModule), prefix: string.Empty);

            Assert.AreEqual(expected, SdkVersionUtils.GetSdkVersion(string.Empty));
        }

        [TestMethod]
        public void GetSdkVersionReturnsVersionWithPrefix()
        {
            string expected = SdkVersionHelper.GetExpectedSdkVersion(typeof(RequestTrackingTelemetryModule), prefix: "lala");

            Assert.AreEqual(expected, SdkVersionUtils.GetSdkVersion("lala"));
        }
    }
}
