namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class QuickPulseDataAccumulatorManagerTests
    {
        [TestMethod]
        public void QuickPulseDataAccumulatorManagerLocksInSampleCorrectly()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            CollectionConfiguration collectionConfiguration =
                new CollectionConfiguration(
                    new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                    out errors,
                    new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount = 5;
            
            // ACT
            var completedSample = accumulatorManager.CompleteCurrentDataAccumulator(collectionConfiguration);

            // ASSERT
            Assert.AreEqual(5, completedSample.AIRequestSuccessCount);
            Assert.AreEqual(0, accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);
            Assert.AreNotSame(completedSample, accumulatorManager.CurrentDataAccumulator);
        }

        [TestMethod]
        public void QuickPulseDataAccumulatorManagerLocksInSampleCorrectlyMultithreaded()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            CollectionConfiguration collectionConfiguration =
                new CollectionConfiguration(
                    new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                    out errors,
                    new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            int taskCount = 100;
            var writeTasks = new List<Task>(taskCount);
            var pause = TimeSpan.FromMilliseconds(10);

            for (int i = 0; i < taskCount; i++)
            {
                var task = new Task(() =>
                {
                    Interlocked.Increment(ref accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);

                    // sleep to increase the probability of sample completion happening right now
                    Thread.Sleep(pause);

                    Interlocked.Increment(ref accumulatorManager.CurrentDataAccumulator.AIDependencyCallSuccessCount);
                });

                writeTasks.Add(task);
            }

            var completionTask = new Task(() =>
            {
                // sleep to increase the probability of more write tasks being between the two writes
                Thread.Sleep(TimeSpan.FromTicks(pause.Ticks / 2));

                accumulatorManager.CompleteCurrentDataAccumulator(collectionConfiguration);
            });

            // shuffle the completion task into the middle of the pile to have it fire roughly halfway through
            writeTasks.Insert(writeTasks.Count / 2, completionTask);

            // ACT
            var sample1 = accumulatorManager.CurrentDataAccumulator;

            var result = Parallel.For(0, writeTasks.Count, new ParallelOptions() { MaxDegreeOfParallelism = taskCount }, i => writeTasks[i].RunSynchronously());

            while (!result.IsCompleted)
            {
            }

            var sample2 = accumulatorManager.CurrentDataAccumulator;

            // ASSERT
            // we expect some "telemetry items" to get "sprayed" over the two neighboring samples
            Assert.IsTrue(sample1.AIRequestSuccessCount > sample1.AIDependencyCallSuccessCount);
            Assert.IsTrue(sample2.AIRequestSuccessCount < sample2.AIDependencyCallSuccessCount);

            // overall numbers should match exactly
            Assert.AreEqual(taskCount, sample1.AIRequestSuccessCount + sample2.AIRequestSuccessCount);
            Assert.AreEqual(taskCount, sample1.AIDependencyCallSuccessCount + sample2.AIDependencyCallSuccessCount);
        }
    }
}
