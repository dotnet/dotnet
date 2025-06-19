namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseQuotaTrackerTests
    {
        [TestMethod]
        public void QuickPulseQuotaTrackerQuotaNotEmptyAtStart()
        {
            // ARRANGE
            int startQuota = 10;
            bool counted;
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, 30, startQuota);

            // ACT & ASSERT
            while (startQuota > 0)
            {
                Assert.IsFalse(quotaTracker.QuotaExhausted);
                counted = quotaTracker.ApplyQuota();
                Assert.IsTrue(counted);
                --startQuota;
            }

            // Quota should be exhausted.
            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted);
            Assert.AreEqual(0f, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);
        }

        [TestMethod]
        public void QuickPulseQuotaTrackerQuotaGetsRefilled()
        {
            // ARRANGE
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, 30, 0);
            bool counted;

            // ACT & ASSERT
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // No quota yet
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            mockTimeProvider.FastForward(TimeSpan.FromSeconds(1)); // 0.5 quota accumulated
            
            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // No quota yet
            Assert.AreEqual(0.5f, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            mockTimeProvider.FastForward(TimeSpan.FromSeconds(1)); // 1 quota accumulated
            
            counted = quotaTracker.ApplyQuota();
            Assert.IsTrue(counted);
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);

            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // Quota was already exhausted.
            Assert.AreEqual(0, quotaTracker.CurrentQuota);
            Assert.IsTrue(quotaTracker.QuotaExhausted);
        }

        [TestMethod]
        public void QuickPulseQuotaTrackerQuotaDoesNotExceedMax()
        {
            // ARRANGE
            int startQuota = 10;
            int maxQuota = 30;
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, maxQuota, startQuota);
            bool counted;

            mockTimeProvider.FastForward(TimeSpan.FromDays(1));

            // ACT & ASSERT
            while (maxQuota > 0)
            {
                counted = quotaTracker.ApplyQuota();
                Assert.IsTrue(counted);
                --maxQuota;
            }

            counted = quotaTracker.ApplyQuota();
            Assert.IsFalse(counted); // We should exhaust quota by this time
        }

        [TestMethod]
        public void QuickPulseQuotaTrackerShouldNotGetBursts()
        {
            // ARRANGE
            int maxQuota = 30;
            var mockTimeProvider = new ClockMock();
            QuickPulseQuotaTracker quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, maxQuota, 0);

            mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));

            // ACT & ASSERT
            // Emulate that every second we try to track 100 of documents. We should expect
            // only one document every 2nd second (quota = 30 documents per min).
            for (int i = 0; i < 1000; i++)
            {
                int count = 0;
                for (int j = 0; j < 100; j++)
                {
                    bool counted = quotaTracker.ApplyQuota();
                    if (counted)
                    {
                        ++count;
                    }
                }

                Assert.AreEqual((i % 2) == 0 ? 0 : 1, count);

                mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
        public void QuickPulseQuotaTrackerIsThreadSafe()
        {
            // ARRANGE
            int maxQuota = 100 * 60;
            int experimentLengthInSeconds = 1000;
            int concurrency = 1000;

            var mockTimeProvider = new ClockMock();
            var quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, maxQuota, 0);

            var quotaApplicationResults = new ConcurrentQueue<bool>();

            // ACT
            for (int i = 0; i < experimentLengthInSeconds; i++)
            {
                mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));

                var tasks = new List<Action>();
                for (int j = 0; j < concurrency; j++)
                {
                    tasks.Add(() => quotaApplicationResults.Enqueue(quotaTracker.ApplyQuota()));
                }

                Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = concurrency }, tasks.ToArray());
            }

            // ASSERT
            var passedQuotaCount = quotaApplicationResults.Count(result => result);
            var correctResult = maxQuota / 60 * experimentLengthInSeconds;

            Assert.AreEqual(correctResult, passedQuotaCount);
            Assert.IsFalse(quotaTracker.ApplyQuota());
        }

        [TestMethod]
        public void QuickPulseQuotaTrackerParameters()
        {
            // ARRANGE
            int maxQuota = 500;
            int experimentLengthInSeconds = 499;
            var mockTimeProvider = new ClockMock();
            var quotaTracker = new QuickPulseQuotaTracker(mockTimeProvider, maxQuota, 900, 1);

            // ACT
            for (int i = 0; i < experimentLengthInSeconds; i++)
            {
                mockTimeProvider.FastForward(TimeSpan.FromSeconds(1));

                quotaTracker.ApplyQuota();
            }

            // ASSERT
            Assert.AreEqual(499, quotaTracker.CurrentQuota);
            Assert.AreEqual(500, quotaTracker.MaxQuota);
            Assert.AreEqual(false, quotaTracker.QuotaExhausted);
        }
    }
}