namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DiagnoisticsEventThrottlingInitializationTest
    {
        private const int ThrottleAfterCountDefault = DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount;

        [TestMethod]
        public void TestParametrizedStateAfterInitialization()
        {
            var throttling = new DiagnoisticsEventThrottling(ThrottleAfterCountDefault);

            Assert.AreEqual(
                ThrottleAfterCountDefault,
                throttling.ThrottleAfterCount,
                "Unexpected ThrottleAfterCount state");

            Assert.AreEqual(0, throttling.CollectSnapshot().Count, "Unexpected snapshot state");
        }

        [TestMethod]
        public void TestSettingAfterCountArgument()
        {
            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount).ThrottleAfterCount);

            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount + 1,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount + 1).ThrottleAfterCount);

            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount).ThrottleAfterCount);

            Assert.AreEqual(
                DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount - 1,
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount - 1).ThrottleAfterCount);
        }

        [TestMethod]
        public void TestLowerBoundOfThrottleAfterCountArgument()
        {
            bool failedWithExpectedException = false;
            try
            {
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MinimalThrottleAfterCount - 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }

        [TestMethod]
        public void TestUpperBoundOfThrottleAfterCountArgument()
        {
            bool failedWithExpectedException = false;
            try
            {
                new DiagnoisticsEventThrottling(DiagnoisticsEventThrottlingDefaults.MaxThrottleAfterCount + 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }
    }
}
