#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseTopCpuCollectorTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorReturnsNothingWhenCalledForTheFirstTime()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock();
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", TimeSpan.FromSeconds(50)),
                                                new QuickPulseProcess("Process2", TimeSpan.FromSeconds(100)),
                                                new QuickPulseProcess("Process3", TimeSpan.FromSeconds(75)),
                                                new QuickPulseProcess("Process4", TimeSpan.FromSeconds(25)),
                                                new QuickPulseProcess("Process5", TimeSpan.FromSeconds(125)),
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            var topProcesses = collector.GetTopProcessesByCpu(3).ToList();

            // ASSERT
            Assert.AreEqual(0, topProcesses.Count);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorReturnsTopProcessesByCpuWhenTotalTimeIsUnavailable()
        {
            // ARRANGE
            TimeSpan interval = TimeSpan.FromSeconds(2);
            var processProvider = new QuickPulseProcessProviderMock() { OverallTimeValue = null };
            var baseProcessorTime = TimeSpan.FromSeconds(100);
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", baseProcessorTime),
                                                new QuickPulseProcess("Process2", baseProcessorTime),
                                                new QuickPulseProcess("Process3", baseProcessorTime),
                                                new QuickPulseProcess("Process4", baseProcessorTime),
                                                new QuickPulseProcess("Process5", baseProcessorTime),
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.GetTopProcessesByCpu(3);

            timeProvider.FastForward(interval);

            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", baseProcessorTime + TimeSpan.FromMilliseconds(50)),
                                                new QuickPulseProcess("Process2", baseProcessorTime + TimeSpan.FromMilliseconds(100)),
                                                new QuickPulseProcess("Process3", baseProcessorTime + TimeSpan.FromMilliseconds(75)),
                                                new QuickPulseProcess("Process4", baseProcessorTime + TimeSpan.FromMilliseconds(25)),
                                                new QuickPulseProcess("Process5", baseProcessorTime + TimeSpan.FromMilliseconds(125)),
                                            };

            var topProcesses = collector.GetTopProcessesByCpu(3).ToList();

            // ASSERT
            Assert.AreEqual(3, topProcesses.Count);

            Assert.AreEqual("Process5", topProcesses[0].Item1);
            Assert.AreEqual((int)((125.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[0].Item2);

            Assert.AreEqual("Process2", topProcesses[1].Item1);
            Assert.AreEqual((int)((100.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[1].Item2);

            Assert.AreEqual("Process3", topProcesses[2].Item1);
            Assert.AreEqual((int)((75.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[2].Item2);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorReturnsTopProcessesByCpuWhenTotalTimeIsAvailable()
        {
            // ARRANGE
            TimeSpan interval = TimeSpan.FromSeconds(2);
            var processProvider = new QuickPulseProcessProviderMock();
            var baseProcessorTime = TimeSpan.FromSeconds(100);
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", baseProcessorTime),
                                                new QuickPulseProcess("Process2", baseProcessorTime),
                                                new QuickPulseProcess("Process3", baseProcessorTime),
                                                new QuickPulseProcess("Process4", baseProcessorTime),
                                                new QuickPulseProcess("Process5", baseProcessorTime)
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT

            // doesn't matter, some large value
            processProvider.OverallTimeValue = TimeSpan.FromTicks(1000 * baseProcessorTime.Ticks);
            collector.GetTopProcessesByCpu(3);

            timeProvider.FastForward(interval);

            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", baseProcessorTime + TimeSpan.FromMilliseconds(50)),
                                                new QuickPulseProcess("Process2", baseProcessorTime + TimeSpan.FromMilliseconds(100)),
                                                new QuickPulseProcess("Process3", baseProcessorTime + TimeSpan.FromMilliseconds(75)),
                                                new QuickPulseProcess("Process4", baseProcessorTime + TimeSpan.FromMilliseconds(25)),
                                                new QuickPulseProcess("Process5", baseProcessorTime + TimeSpan.FromMilliseconds(125))
                                            };
            processProvider.OverallTimeValue += TimeSpan.FromTicks(Environment.ProcessorCount * interval.Ticks);

            var topProcesses = collector.GetTopProcessesByCpu(3).ToList();

            // ASSERT
            Assert.AreEqual(3, topProcesses.Count);

            Assert.AreEqual("Process5", topProcesses[0].Item1);
            Assert.AreEqual((int)((125.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[0].Item2);

            Assert.AreEqual("Process2", topProcesses[1].Item1);
            Assert.AreEqual((int)((100.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[1].Item2);

            Assert.AreEqual("Process3", topProcesses[2].Item1);
            Assert.AreEqual((int)((75.0 / (Environment.ProcessorCount * interval.TotalMilliseconds)) * 100), topProcesses[2].Item2);
        }
        
        [TestMethod]
        public void QuickPulseTopCpuCollectorHandlesExceptionFromProcessProvider()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock() { AlwaysThrow = new Exception() };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            var topProcesses = collector.GetTopProcessesByCpu(3);
            
            // ASSERT
            Assert.AreEqual(0, topProcesses.Count());
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorCleansUpStateWhenProcessesGoAway()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock();
            var baseProcessorTime = TimeSpan.FromSeconds(100);
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", baseProcessorTime),
                                                new QuickPulseProcess("Process2", baseProcessorTime),
                                                new QuickPulseProcess("Process3", baseProcessorTime),
                                                new QuickPulseProcess("Process4", baseProcessorTime),
                                                new QuickPulseProcess("Process5", baseProcessorTime),
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.GetTopProcessesByCpu(3);
            int itemCount1 = collector.ProcessObservations.Count;

            timeProvider.FastForward(TimeSpan.FromSeconds(1));

            processProvider.Processes = new List<QuickPulseProcess>() { new QuickPulseProcess("Process1", baseProcessorTime) };
            collector.GetTopProcessesByCpu(3);
            int itemCount3 = collector.ProcessObservations.Count;

            // ASSERT
            Assert.AreEqual(5, itemCount1);
            Assert.AreEqual(1, itemCount3);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorSetsInitializationStatusCorrectlyWhenUnknownExceptionIsThrown()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock { AlwaysThrow = new Exception() };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);
            
            // ACT
            collector.Initialize();
            
            // ASSERT
            Assert.IsTrue(collector.InitializationFailed);
            Assert.IsFalse(collector.AccessDenied);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorSetsInitializationStatusCorrectlyWhenUnauthorizedExceptionIsThrown()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock { AlwaysThrow = new UnauthorizedAccessException() };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.Initialize();

            // ASSERT
            Assert.IsTrue(collector.InitializationFailed);
            Assert.IsTrue(collector.AccessDenied);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorSetsInitializationStatusCorrectlyWhenSecurityExceptionIsThrown()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock { AlwaysThrow = new SecurityException() };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.Initialize();

            // ASSERT
            Assert.IsTrue(collector.InitializationFailed);
            Assert.IsTrue(collector.AccessDenied);
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorReturnsNothingIfInitializationFailed()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock { AlwaysThrow = new Exception() };
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", TimeSpan.FromSeconds(1))
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.Initialize();
            collector.GetTopProcessesByCpu(5);
            timeProvider.FastForward(TimeSpan.FromSeconds(1));

            var result = collector.GetTopProcessesByCpu(5);

            // ASSERT
            Assert.IsTrue(collector.InitializationFailed);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void QuickPulseTopCpuCollectorRetriesAfterIntervalWhenAccessDenied()
        {
            // ARRANGE
            var processProvider = new QuickPulseProcessProviderMock();
            processProvider.Processes = new List<QuickPulseProcess>()
                                            {
                                                new QuickPulseProcess("Process1", TimeSpan.FromSeconds(1))
                                            };
            var timeProvider = new ClockMock();
            var collector = new QuickPulseTopCpuCollector(timeProvider, processProvider);

            // ACT
            collector.Initialize();
            collector.GetTopProcessesByCpu(5);
            timeProvider.FastForward(TimeSpan.FromSeconds(1));

            var resultWhenEverythingIsFine = collector.GetTopProcessesByCpu(5);

            timeProvider.FastForward(TimeSpan.FromSeconds(1));
            processProvider.AlwaysThrow = new UnauthorizedAccessException();
            var resultWhileAccessDenied = collector.GetTopProcessesByCpu(5);
            bool flagWhileAccessDenied = collector.AccessDenied;

            // 60 second retry interval
            timeProvider.FastForward(TimeSpan.FromSeconds(59));
            processProvider.AlwaysThrow = null;
            var resultWhenEverythingIsFineAgainButNotEnoughTimePassedToRetry = collector.GetTopProcessesByCpu(5);
            bool flagWhenAccessIsOkButNotEnoughTimePassed = collector.AccessDenied;

            timeProvider.FastForward(TimeSpan.FromSeconds(1));
            processProvider.AlwaysThrow = null;
            var resultWhenRetryIntervalHasPassed = collector.GetTopProcessesByCpu(5);
            bool flagWhenRetryIntervalHasPassed = collector.AccessDenied;

            timeProvider.FastForward(TimeSpan.FromSeconds(1));
            processProvider.AlwaysThrow = null;
            var resultWhenEverythingIsBackToNormalForGood = collector.GetTopProcessesByCpu(5);
            bool flagWhenEverythingIsBackToNormalForGood = collector.AccessDenied;

            // ASSERT
            Assert.IsTrue(resultWhenEverythingIsFine.Any());
            Assert.IsFalse(resultWhileAccessDenied.Any());
            Assert.IsTrue(flagWhileAccessDenied);
            Assert.IsFalse(resultWhenEverythingIsFineAgainButNotEnoughTimePassedToRetry.Any());
            Assert.IsTrue(flagWhenAccessIsOkButNotEnoughTimePassed);
            Assert.IsTrue(resultWhenRetryIntervalHasPassed.Any());
            Assert.IsFalse(flagWhenRetryIntervalHasPassed);
            Assert.IsTrue(resultWhenEverythingIsBackToNormalForGood.Any());
            Assert.IsFalse(flagWhenEverythingIsBackToNormalForGood);
        }
    }
}
#endif