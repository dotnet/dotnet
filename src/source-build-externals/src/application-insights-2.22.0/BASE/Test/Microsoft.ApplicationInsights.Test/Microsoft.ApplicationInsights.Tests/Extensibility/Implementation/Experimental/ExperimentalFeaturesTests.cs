using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental
{
    [TestClass]
    public class ExperimentalFeaturesTests
    {
        [TestMethod]
        public void VerifyExperimentalFeaturesExtensionIfConfigEmpty()
        {
            var telemetryConfiguration = new TelemetryConfiguration();

            bool value = telemetryConfiguration.EvaluateExperimentalFeature("abc");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void VerifyExperimentalFeaturesExtension()
        {
            var telemetryConfiguration = new TelemetryConfiguration();
            telemetryConfiguration.ExperimentalFeatures.Add("123");

            bool value = telemetryConfiguration.EvaluateExperimentalFeature("123");
            Assert.IsTrue(value);

            bool fakeValue = telemetryConfiguration.EvaluateExperimentalFeature("fake");
            Assert.IsFalse(fakeValue);
        }
    }
}
