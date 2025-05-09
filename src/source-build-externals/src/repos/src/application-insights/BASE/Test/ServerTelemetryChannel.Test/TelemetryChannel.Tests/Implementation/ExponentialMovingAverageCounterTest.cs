namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    /// <summary>
    /// Note: exponential moving average information may be found at https://en.wikipedia.org/wiki/Moving_average
    /// </summary>
    [TestClass]
    public class ExponentialMovingAverageCounterTest
    {
        [TestMethod]
        public void AverageValueIsZeroPriorToStart()
        {
            var counter = new ExponentialMovingAverageCounter(.1);

            Assert.AreEqual(0, counter.Average);
        }

        [TestMethod]
        public void AverageValueIsFirstIntervalValuePriorToClosingOfFirstInterval()
        {
            var counter = new ExponentialMovingAverageCounter(.1);

            const int IncrementCount = 3;

            for (int i = 0; i < IncrementCount; i++)
            {
                counter.Increment();
            }

            Assert.AreEqual(IncrementCount, counter.Average);
        }

        [TestMethod]
        public void AverageValueIsFirstIntervalValueAfterClosingOfFirstInterval()
        {
            var counter = new ExponentialMovingAverageCounter(.1);

            const int IncrementCount = 3;

            for (int i = 0; i < IncrementCount; i++)
            {
                counter.Increment();
            }

            counter.StartNewInterval();

            Assert.AreEqual(IncrementCount, counter.Average);
        }

        [TestMethod]
        public void AverageValueIsMovingAverageAfterClosingOfAtLeastTwoIntervals()
        {
            var counter = new ExponentialMovingAverageCounter(.1);

            const int Increment1Count = 3;
            const int Increment2Count = 5;

            for (int i = 0; i < Increment1Count; i++)
            {
                counter.Increment();
            }

            counter.StartNewInterval();

            for (int i = 0; i < Increment2Count; i++)
            {
                counter.Increment();
            }

            counter.StartNewInterval();

            Assert.AreEqual(Increment1Count * .9 + Increment2Count * .1, counter.Average, 10);
        }
    }
}
