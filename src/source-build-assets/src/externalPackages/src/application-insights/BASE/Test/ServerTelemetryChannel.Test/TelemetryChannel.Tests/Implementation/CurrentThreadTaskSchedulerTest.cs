namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;

    [TestClass]
    public class CurrentThreadTaskSchedulerTest
    {
        [TestMethod]
        public void InstanceReturnsSingletonThatCanBeReusedToImprovePerformance()
        {
            TaskScheduler instance = CurrentThreadTaskScheduler.Instance;
            Assert.IsNotNull(instance);
            AssertEx.IsType<CurrentThreadTaskScheduler>(instance);
        }

        [TestMethod]
        public void MaximumConcurrencyLevelReturnsOneBecauseSchedulerExecutesTasksSynchronously()
        {
            var scheduler = new CurrentThreadTaskScheduler();
            Assert.AreEqual(1, scheduler.MaximumConcurrencyLevel);
        }

        [TestMethod]
        public void QueueTaskExecutesTaskSynchronously()
        {
            int taskThreadId = 0;
            var task = new Task(() => { taskThreadId = Thread.CurrentThread.ManagedThreadId; });

            task.Start(new CurrentThreadTaskScheduler());

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, taskThreadId);
        }

        [TestMethod]
        public void TryExecuteTaskInlineExecutesTaskSynchronously()
        {
            int taskThreadId = 0;
            var task = new Task(() => { taskThreadId = Thread.CurrentThread.ManagedThreadId; });

            task.RunSynchronously(new CurrentThreadTaskScheduler());

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, taskThreadId);
        }
    }
}
