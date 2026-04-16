namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation.SamplingInternals
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.SamplingInternals;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SamplingIncludesUtilityTests
    {
        [TestMethod]
        public void VerifyIncludes()
        {
            string input = "DEPENDENCY;EVENT";

            var test = SamplingIncludesUtility.CalculateFromIncludes(input);

            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Event));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Exception));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.PageView));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Request));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Message));
        }

        [TestMethod]
        public void VerifyExcludes()
        {
            string input = "DEPENDENCY;EVENT";

            var test = SamplingIncludesUtility.CalculateFromExcludes(input);

            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Event));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Exception));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.PageView));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Request));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Message));
        }

        [TestMethod]
        public void VerifyIncludesBehaviorWhenBadConfig()
        {
            string input = "car;truck;train;";

            var test = SamplingIncludesUtility.CalculateFromIncludes(input);

            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Event));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Exception));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.PageView));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Request));
            Assert.IsFalse(test.HasFlag(SamplingTelemetryItemTypes.Message));
        }

        [TestMethod]
        public void VerifyExcludesBehaviorWhenBadConfig()
        {
            string input = "car;truck;train;";

            var test = SamplingIncludesUtility.CalculateFromExcludes(input);

            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.RemoteDependency));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Event));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Exception));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.PageView));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Request));
            Assert.IsTrue(test.HasFlag(SamplingTelemetryItemTypes.Message));
        }
    }
}
