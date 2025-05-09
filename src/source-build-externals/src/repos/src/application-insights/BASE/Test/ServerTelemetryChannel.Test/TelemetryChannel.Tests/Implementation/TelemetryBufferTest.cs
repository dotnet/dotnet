namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using ITelemetry = Microsoft.ApplicationInsights.Channel.ITelemetry;
    using Channel.Helpers;

    public class TelemetryBufferTest
    {
        [TestClass]
        public class Class : TelemetryBufferTest
        {
            [TestMethod]
            public void ImplementsIEnumerableToAllowInspectingBufferContentsInTests()
            {
                TelemetryBuffer instance = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                Assert.IsTrue(instance is IEnumerable<ITelemetry>);
            }

            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionWhenSerializerIsNull()
            {
                AssertEx.Throws<ArgumentNullException>(() => new TelemetryBuffer(null, new StubApplicationLifecycle()));
            }

#if NETFRAMEWORK
            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionWhenApplicationLifecycleIsNull()
            {
                AssertEx.Throws<ArgumentNullException>(() => new TelemetryBuffer(new StubTelemetrySerializer(), null));
            }
#endif
        }

        [TestClass]
        public class MaxTransmissionDelay
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForProductionEnvironmentAndUnitTests()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                Assert.AreEqual(TimeSpan.FromSeconds(30), buffer.MaxTransmissionDelay);
            }

            [TestMethod]
            public void CanBeChangedByChannelToTunePerformance()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());

                var expectedValue = TimeSpan.FromSeconds(42);
                buffer.MaxTransmissionDelay = expectedValue;

                Assert.AreEqual(expectedValue, buffer.MaxTransmissionDelay);
            }
        }

        [TestClass]
        public class MaxNumberOfItemsPerTransmission : TelemetryBufferTest
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForProductionEnvironmentAndUnitTests()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                Assert.AreEqual(500, buffer.Capacity);
                Assert.AreEqual(1000000, buffer.BacklogSize);
            }

            [TestMethod]
            public void CanBeSetByChannelToTunePerformance()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 42;
                buffer.BacklogSize = 3900;
                Assert.AreEqual(42, buffer.Capacity);
                Assert.AreEqual(3900, buffer.BacklogSize);
            }

            [TestMethod]
            public void ThrowsArgumentExceptionWhenBacklogSizeIsLowerThanCapacity()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 1111;
                AssertEx.Throws<ArgumentException>(() => buffer.BacklogSize = 1110);

                buffer.BacklogSize = 8000;
                AssertEx.Throws<ArgumentException>(() => buffer.Capacity = 8001);

            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenNewValueIsLessThanMinimum()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.Capacity = 0);
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.BacklogSize = 0);
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.BacklogSize = 1000); // 1001 is minimum anything low would throw.

                bool exceptionThrown = false;
                try
                {
                    buffer.BacklogSize = 1001; // 1001 is valid and should not throw.
                }
                catch(Exception)
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown == false, "No exception should be thrown when trying to set backlog size to 1001");                
            }
        }

        // TODO: Test that TelemetryBuffer.Send synchronously clears the buffer to prevent item # 501 from flushing again
        [TestClass]
        [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
        public class Send : TelemetryBufferTest
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionWhenTelemetryIsNull()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                AssertEx.Throws<ArgumentNullException>(() => buffer.Process((ITelemetry)null));
            }

            [TestMethod]
            public void AddsTelemetryToBufferUntilItReachesMax()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 42;

                buffer.Process(new StubTelemetry());

                Assert.AreEqual(1, buffer.Count());
            }

            [TestMethod]
            public void TelemetryBufferDoesNotGrowBeyondMaxBacklogSize()
            {
                //TelemetryBufferWhichDoesNothingOnFlush does not flush items on buffer full,to test ItemDrop scenario.
                var buffer = new TelemetryBufferWhichDoesNothingOnFlush( new StubTelemetrySerializer(), new StubApplicationLifecycle() );
                buffer.Capacity = 2;
                buffer.BacklogSize = 1002;

                buffer.Process(new StubTelemetry());

                // Add more items (1005) to buffer than the max backlog size(1002)
                for (int i = 0; i < 1005; i++)
                {
                    buffer.Process(new StubTelemetry());
                }
                                
                // validate that items are not added after maxunsentbacklogsize is reached.
                // this also validate that items can still be added after Capacity is reached as it is only a soft limit.
                int bufferItemCount = buffer.Count();
                Assert.AreEqual(1002, bufferItemCount);
            }

            [TestMethod]
            
            public void FlushesBufferWhenNumberOfTelemetryItemsReachesMax()
            {
                var bufferFlushed = new ManualResetEventSlim();
                IEnumerable<ITelemetry> flushedTelemetry = null;
                var serializer = new StubTelemetrySerializer
                {
                    OnSerialize = telemetry =>
                    {
                        flushedTelemetry = telemetry.ToList();
                        bufferFlushed.Set();
                    },
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                var sentTelemetry = new List<ITelemetry> { new StubTelemetry(), new StubTelemetry() };
                telemetryBuffer.Capacity = sentTelemetry.Count;
                foreach (ITelemetry item in sentTelemetry)
                {
                    telemetryBuffer.Process(item);
                }

                Assert.IsTrue(bufferFlushed.Wait(30));
                AssertEx.AreEqual(sentTelemetry, flushedTelemetry);
            }

            [TestMethod]
            public void WaitsUntilTelemetryBufferIsSafeToModify()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 1;

                Task anotherThread;
                lock (buffer)
                {
                    anotherThread = Task.Run(() => buffer.Process(new StubTelemetry()));
                    Assert.IsFalse(anotherThread.Wait(10));
                }

                Assert.IsTrue(anotherThread.Wait(50));
            }

            [TestMethod]
            public void StartsTimerThatFlushesBufferAfterMaxTransmissionDelay()
            {
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry => 
                { 
                    telemetrySerialized.Set();
                };

                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                buffer.MaxTransmissionDelay = TimeSpan.FromMilliseconds(1);
                buffer.Process(new StubTelemetry());

                Assert.IsTrue(telemetrySerialized.Wait(1000));
            }

            [TestMethod]
            public void DoesNotCancelPreviousFlush()
            {
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry =>
                {
                    telemetrySerialized.Set();
                };
                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                buffer.MaxTransmissionDelay = TimeSpan.FromMilliseconds(1);
                buffer.Process(new StubTelemetry());

                buffer.MaxTransmissionDelay = TimeSpan.FromSeconds(42);
                buffer.Process(new StubTelemetry());

                Assert.IsTrue(telemetrySerialized.Wait(TimeSpan.FromMilliseconds(100)));
            }
        }

        [TestClass]
        public class FlushAsync : TelemetryBufferTest
        {
            [TestMethod]
            [Timeout(10000)]
            public void DoesntSerializeTelemetryIfBufferIsEmpty()
            {
                bool telemetrySerialized = false;
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry =>
                {
                    telemetrySerialized = true;
                };
                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                telemetryBuffer.FlushAsync().GetAwaiter().GetResult();
        
                Assert.IsFalse(telemetrySerialized);
            }

            [TestMethod]
            [Timeout(10000)]
            public void SerializesTelemetryIfBufferIsNotEmpty()
            {
                List<ITelemetry> serializedTelemetry = null;
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry => 
                {
                    serializedTelemetry = new List<ITelemetry>(telemetry);
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
        
                var expectedTelemetry = new StubTelemetry();
                telemetryBuffer.Process(expectedTelemetry);
        
                telemetryBuffer.FlushAsync().GetAwaiter().GetResult();
        
                Assert.AreSame(expectedTelemetry, serializedTelemetry.Single());
            }

            [TestMethod]
            public void SerializesBufferOnThreadPoolToFreeUpCustomersThread()
            {
                int serializerThreadId = -1;
                var serializerInvoked = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry =>
                {
                    serializerThreadId = Thread.CurrentThread.ManagedThreadId;
                    serializerInvoked.Set();
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                telemetryBuffer.Process(new StubTelemetry());

                Task dontWait = telemetryBuffer.FlushAsync();

                Assert.IsTrue(serializerInvoked.Wait(100));
                Assert.AreNotEqual(serializerThreadId, Thread.CurrentThread.ManagedThreadId);
            }

            [TestMethod]
            [Timeout(10000)]
            public void EmptiesBufferAfterSerialization()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Capacity = 10;
                buffer.Process(new StubTelemetry());

                buffer.FlushAsync().GetAwaiter().GetResult();
        
                Assert.AreEqual(0, buffer.Count());
            }

            [TestMethod]
            public void WaitsUntilTelemetryBufferIsSafeToModify()
            {
                var telemetryBuffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                telemetryBuffer.Process(new StubTelemetry());

                Task anotherThread;
                lock (telemetryBuffer)
                {
                    anotherThread = Task.Run(() => telemetryBuffer.FlushAsync());
                    Assert.IsFalse(anotherThread.Wait(10));
                }

                Assert.IsTrue(anotherThread.Wait(50));
            }

            [TestMethod]
            [Timeout(10000)]
            public void CancelsPreviouslyStartedAutomaticFlushToPreventPreventPrematureTransmission()
            {
                var serializer = new StubTelemetrySerializer();
                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
        
                buffer.MaxTransmissionDelay = TimeSpan.FromMilliseconds(1);
                buffer.Process(new StubTelemetry());
        
                buffer.MaxTransmissionDelay = TimeSpan.FromMilliseconds(100);
                buffer.FlushAsync().Wait();
        
                var autoFlushed = new ManualResetEventSlim();
                serializer.OnSerialize = telemetry =>
                {
                    autoFlushed.Set();
                };
                buffer.Process(new StubTelemetry());
        
                Assert.IsFalse(autoFlushed.Wait(30));
            }

            [TestMethod]
            [Timeout(10000)]
            public void DoesNotContinueOnCapturedSynchronizationContextToImprovePerformance()
            {
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                buffer.Process(new StubTelemetry());
        
                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    buffer.FlushAsync().GetAwaiter().GetResult();
        
                    Assert.IsFalse(postedBack);
                }
            }
        }

        [TestClass]
        public class HandleApplicationStoppingEvent : TelemetryBufferTest
        {
            [TestMethod]
            public void FlushesBufferToPreventLossOfTelemetry()
            {
                var applicationLifecycle = new StubApplicationLifecycle();
                var telemetrySerialized = new ManualResetEventSlim();
                var serializer = new StubTelemetrySerializer();
                serializer.OnSerialize = telemetry => 
                {
                    telemetrySerialized.Set();
                };
                var buffer = new TelemetryBuffer(serializer, applicationLifecycle);
                buffer.Process(new StubTelemetry());

                applicationLifecycle.OnStopping(ApplicationStoppingEventArgs.Empty);

                Assert.IsTrue(telemetrySerialized.Wait(50));
            }

            [TestMethod]
            public void PreventsOperatingSystemFromSuspendingAsynchronousOperations()
            {
                var applicationLifecycle = new StubApplicationLifecycle();
                var buffer = new TelemetryBuffer(new StubTelemetrySerializer(), applicationLifecycle);
                buffer.Process(new StubTelemetry());

                bool deferralAcquired = false;
                Func<Func<Task>, Task> asyncTaskRunner = asyncMethod =>
                {
                    deferralAcquired = true;
                    return asyncMethod();
                };
                applicationLifecycle.OnStopping(new ApplicationStoppingEventArgs(asyncTaskRunner));

                Assert.IsTrue(deferralAcquired);
            }
        }

        [TestClass]
        public class FlushAsyncTask : TelemetryBufferTest
        {
            [TestMethod]
            [Timeout(10000)]
            public async Task CallsSerializeTelemetryIfBufferIsEmpty()
            {
                bool telemetrySerialized = false;
                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        telemetrySerialized = true;
                        return Task.FromResult(true);
                    }
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                var taskResult = await telemetryBuffer.FlushAsync(default);

                Assert.IsTrue(telemetrySerialized);
                Assert.IsTrue(taskResult);
            }

            [TestMethod]
            [Timeout(10000)]
            public async Task SerializesTelemetryIfBufferIsNotEmpty()
            {
                List<ITelemetry> serializedTelemetry = null;

                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        serializedTelemetry = new List<ITelemetry>(telemetry);
                        return Task.FromResult(true);
                    }
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());

                var expectedTelemetry = new StubTelemetry();
                telemetryBuffer.Process(expectedTelemetry);

                var taskResult = await telemetryBuffer.FlushAsync(default);

                Assert.AreSame(expectedTelemetry, serializedTelemetry.Single());
                Assert.IsTrue(taskResult);
            }

            [TestMethod]
            [Timeout(10000)]
            public async Task EmptiesBufferAfterSerialization()
            {
                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        return Task.FromResult(true);
                    }
                };

                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                buffer.Capacity = 10;
                buffer.Process(new StubTelemetry());

                var taskResult = await buffer.FlushAsync(default);

                Assert.AreEqual(0, buffer.Count());
                Assert.IsTrue(taskResult);
            }

            [TestMethod]
            [Timeout(10000)]
            public async Task DoesNotContinueOnCapturedSynchronizationContextToImprovePerformance()
            {
                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        return Task.FromResult(true);
                    }
                };

                var buffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                buffer.Process(new StubTelemetry());

                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    var taskResult = await buffer.FlushAsync(default);

                    Assert.IsFalse(postedBack);
                    Assert.IsTrue(taskResult);
                }
            }

            [TestMethod]
            public async Task BufferFlushAsyncTaskRespectCancellationToken()
            {
                var telemetryBuffer = new TelemetryBuffer(new StubTelemetrySerializer(), new StubApplicationLifecycle());
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => telemetryBuffer.FlushAsync(new CancellationToken(true)));
            }

            [TestMethod]
            public void WaitsUntilTelemetryBufferIsSafeToModify()
            {
                var serializer = new StubTelemetrySerializer
                {
                    OnSerializeAsync = (telemetry, cancellationToken) =>
                    {
                        return Task.FromResult(true);
                    }
                };

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                telemetryBuffer.Process(new StubTelemetry());

                Task anotherThread;
                lock (telemetryBuffer)
                {
                    anotherThread = Task.Run(() => telemetryBuffer.FlushAsync(default));
                    Assert.IsFalse(anotherThread.Wait(10));
                }

                Assert.IsTrue(anotherThread.Wait(50));
            }

            [TestMethod]
            public void SerializerThrowsExceptionWhenEndPointIsNull()
            {
                var serializer = new TelemetrySerializer(new Transmitter());

                var telemetryBuffer = new TelemetryBuffer(serializer, new StubApplicationLifecycle());
                AssertEx.Throws<Exception>(() => telemetryBuffer.FlushAsync(default));
            }
        }
    }
}
