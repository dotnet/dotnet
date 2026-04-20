namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InterlockedThrottleTests
    {
        [TestMethod]
        public void VerifyInterlockedWorksAsExpected()
        {
            int testInterval = 10;
            var counter = 0;

            var its = new InterlockedThrottle(TimeSpan.FromSeconds(testInterval));

            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);

            Assert.AreEqual(1, counter);

            Thread.Sleep(TimeSpan.FromSeconds(testInterval +1));

            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);
            its.PerformThrottledAction(() => counter++);

            Assert.AreEqual(2, counter);
        }
    }
}
