namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class TaskTimerInternalTest
    {
        [TestClass]
        public class Delay
        {
            [TestMethod]
            public void DefaultValueIsOneMinuteBecauseItHasToBeSomethingValid()
            {
                var timer = new TaskTimerInternal();
                Assert.AreEqual(TimeSpan.FromMinutes(1), timer.Delay);
            }

            [TestMethod]
            public void CanBeChangedByConfigurableChannelComponents()
            {
                var timer = new TaskTimerInternal();
                timer.Delay = TimeSpan.FromSeconds(42);
                Assert.AreEqual(42, timer.Delay.TotalSeconds);
            }

            [TestMethod]
            public void CanBeSetToInfiniteToPreventTimerFromFiring()
            {
                var timer = new TaskTimerInternal();
                timer.Delay = new TimeSpan(0, 0, 0, 0, -1);
                Assert.AreEqual(new TimeSpan(0, 0, 0, 0, -1), timer.Delay);
            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenNewValueIsZeroOrLess()
            {
                var timer = new TaskTimerInternal();
                AssertEx.Throws<ArgumentOutOfRangeException>(() => timer.Delay = TimeSpan.Zero);
            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenNewValueIsMoreThanMaxIntMilliseconds()
            {
                var timer = new TaskTimerInternal();
                AssertEx.Throws<ArgumentOutOfRangeException>(() => timer.Delay = TimeSpan.FromMilliseconds((double)int.MaxValue + 1));
            }
        }

        [TestClass]
        public class IsStarted
        {
            [TestMethod]
            public void ReturnsFalseIfTimerWasNeverStarted()
            {
                var timer = new TaskTimerInternal();
                Assert.IsFalse(timer.IsStarted);
            }

            [TestMethod]
            public void ReturnsTrueWhileUntilActionIsInvoked()
            {
                var timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                var actionStarted = new ManualResetEventSlim();
                var actionCanFinish = new ManualResetEventSlim();
                timer.Start(
                    () => Task.Factory.StartNew(
                        () =>
                        {
                            actionStarted.Set();
                            actionCanFinish.Wait();
                        }));

                Assert.IsTrue(timer.IsStarted);

                actionStarted.Wait(1000);

                Assert.IsFalse(timer.IsStarted);

                actionCanFinish.Set();
            }
        }

        [TestClass]
        public class Start
        {
#if NET452
            [TestMethod]
            public void DoesNotLogErrorsIfCallbackReturnsNull()
            {
                using (TestEventListener listener = new TestEventListener())
                {
                    listener.EnableEvents(CoreEventSource.Log, EventLevel.Error);

                    var timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };
                    var actionInvoked = new ManualResetEventSlim();

                    timer.Start(() => { actionInvoked.Set(); return null; });

                    Assert.IsTrue(actionInvoked.Wait(1000));
                    // Listener will wait for up to 5 seconds for incoming messages so no need to delay/sleep here.
                    Assert.IsNull(listener.Messages.FirstOrDefault());
                }
            }
#endif

            [TestMethod]
            public void InvokesActionAfterDelay()
            {
                var timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                var actionInvoked = new ManualResetEventSlim();
                timer.Start(() => Task.Factory.StartNew(actionInvoked.Set));

                Assert.IsFalse(actionInvoked.IsSet);
                Assert.IsTrue(actionInvoked.Wait(1000));
            }

            [TestMethod]
            public void CancelsPreviousActionWhenStartIsCalledMultipleTimes()
            {
                var timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                int invokationCount = 0;
                var lastActionInvoked = new ManualResetEventSlim();
                timer.Start(() => Task.Factory.StartNew(() => Interlocked.Increment(ref invokationCount)));
                timer.Start(
                    () => Task.Factory.StartNew(
                        () =>
                        {
                            Interlocked.Increment(ref invokationCount);
                            lastActionInvoked.Set();
                        }));

                Assert.IsTrue(lastActionInvoked.Wait(1000));
                Assert.AreEqual(1, invokationCount);
            }

            [TestMethod]
            [Timeout(1500)]
            public void HandlesAsyncExceptionThrownByTheDelegate()
            {
                TaskTimerInternal timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                using (TestEventListener listener = new TestEventListener())
                {
                    listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways);

                    timer.Start(() => Task.Factory.StartNew(() => { throw new Exception(); }));

                    Assert.IsNotNull(listener.Messages.FirstOrDefault());
                }
            }

            [TestMethod]
            [Timeout(1500)]
            public void HandlesSyncExceptionThrownByTheDelegate()
            {
                TaskTimerInternal timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                using (TestEventListener listener = new TestEventListener())
                {
                    listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways);
                    timer.Start(() => { throw new Exception(); });

                    Assert.IsNotNull(listener.Messages.FirstOrDefault());
                }
            }
        }

        [TestClass]
        public class Cancel
        {
            [TestMethod]
            public async Task AbortsPreviousAction()
            {
                var timer = new TaskTimerInternal { Delay = TimeSpan.FromMilliseconds(1) };

                bool actionInvoked = false;
                timer.Start(() => Task.Factory.StartNew(() => actionInvoked = true));
                timer.Cancel();
        
                await Task.Delay(TimeSpan.FromMilliseconds(20));
        
                Assert.IsFalse(actionInvoked);
            }
        }
    }
}
