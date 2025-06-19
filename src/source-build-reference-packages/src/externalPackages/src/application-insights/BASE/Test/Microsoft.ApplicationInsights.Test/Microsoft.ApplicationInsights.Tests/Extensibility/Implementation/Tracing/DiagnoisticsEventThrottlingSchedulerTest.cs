namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DiagnoisticsEventThrottlingSchedulerTest : IDisposable
    {
        private const int SchedulingRoutineRunInterval = 10;
        private const int ExecuteTimes = 3;

        private readonly DiagnoisticsEventThrottlingScheduler scheduler = new DiagnoisticsEventThrottlingScheduler();

        public void Dispose()
        {
            this.scheduler.Dispose();
        }

        [TestMethod]
        public void TestStateAfterInitialization()
        {
            Assert.AreEqual(
                0, 
                this.scheduler.Tokens.Count, 
                "Unexpected number of timer tokens");
        }

        [TestMethod]
        public void TestScheduingIntervalIsZeroOrLessException()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.ScheduleToRunEveryTimeIntervalInMilliseconds(0, () => { });
            }
            catch (ArgumentOutOfRangeException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }

        [TestMethod]
        public void TestScheduingActionIsNullException()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.ScheduleToRunEveryTimeIntervalInMilliseconds(1, null);
            }
            catch (ArgumentNullException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }
        
        [TestMethod]
        public void TestRemovingScheduledActionsIsNullException()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.RemoveScheduledRoutine(null);
            }
            catch (ArgumentNullException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }

        [TestMethod]
        public void TestRemovingScheduledActionsIsNotOfExpectedType()
        {
            bool failedWithExpectedException = false;
            try
            {
                this.scheduler.RemoveScheduledRoutine(new object());
            }
            catch (ArgumentException)
            {
                failedWithExpectedException = true;
            }

            Assert.IsTrue(failedWithExpectedException);
        }
        
        private class RoutineCounter
        {
            public int ExecutedTimes { get; private set; }

            public void Execute()
            {
                this.ExecutedTimes += 1;
            }
        }
    }
}
