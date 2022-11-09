namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class TransmissionBufferTest
    {
        [TestClass]
        public class Class : TransmissionBufferTest
        {
            [TestMethod]
            [Timeout(15000)]
            public void EnqueueAndDequeueMethodsAreThreadSafe()
            {
                var buffer = new TransmissionBuffer();

                const int NumberOfThreads = 16;
                const int NumberOfIterations = 1000;
                var tasks = new Task[NumberOfThreads];
                for (int t = 0; t < NumberOfThreads; t++)
                {
                    tasks[t] = Task.Run(() =>
                    {
                        for (int i = 0; i < NumberOfIterations; i++)
                        {
                            buffer.Enqueue(() => new StubTransmission());
                            buffer.Dequeue();
                        }
                    });
                }

                Task.WhenAll(tasks).GetAwaiter().GetResult();
            }
        }

        [TestClass]
        public class Capacity : TransmissionBufferTest
        {
            [TestMethod]
            public void DefaultValueIsAppropriateForMostApps()
            {
                var buffer = new TransmissionBuffer();
                Assert.AreEqual(5120 * 1024, buffer.Capacity);
            }

            [TestMethod]
            public void CanBeSetToZeroToDsiableBufferingOfTransmissions()
            {
                var buffer = new TransmissionBuffer { Capacity = 0 };
                Assert.AreEqual(0, buffer.Capacity);
            }

            [TestMethod]
            public void ThrowsArgumentOutOfRangeExceptionWhenValueIsLessThanZero()
            {
                var buffer = new TransmissionBuffer();
                AssertEx.Throws<ArgumentOutOfRangeException>(() => buffer.Capacity = -1);
            }
        }

        [TestClass]
        public class Enqueue : TransmissionBufferTest
        {
            [TestMethod]
            public void ReturnsTrueWhenNewTransmissionLengthDoesNotExceedBufferCapacity()
            {
                var buffer = new TransmissionBuffer { Capacity = 1 };
                Assert.IsTrue(buffer.Enqueue(() => new StubTransmission(new byte[1])));
            }

            [TestMethod]
            public void ReturnsFalseWhenNewTransmissionLengthExceedsBufferCapacity()
            {
                var buffer = new TransmissionBuffer { Capacity = 0 };
                Assert.IsFalse(buffer.Enqueue(() => new StubTransmission(new byte[1])));
            }

            [TestMethod]
            public void DoesNotCountRejectedTransmissionsAgainstMaxNumber()
            {
                var buffer = new TransmissionBuffer { Capacity = 0 };
                buffer.Enqueue(() => new StubTransmission(new byte[1]));

                buffer.Capacity = 1;

                Assert.IsTrue(buffer.Enqueue(() => new StubTransmission(new byte[1])));
            }

            [TestMethod]
            public void DoesNotInvokeTransmissionGetterWhenMaxNumberOfTransmissionsIsExceededToKeepItStored()
            {
                bool transmissionGetterInvoked = false;
                Func<Transmission> transmissionGetter = () =>
                {
                    transmissionGetterInvoked = true;
                    return new StubTransmission(new byte[1]);
                };
                var buffer = new TransmissionBuffer { Capacity = 0 };

                buffer.Enqueue(transmissionGetter);

                Assert.IsFalse(transmissionGetterInvoked);
            }

            [TestMethod]
            public void ReturnsFalseWhenTransmissionGetterReturedNullIndicatingEmptyStorage()
            {
                var buffer = new TransmissionBuffer();
                Assert.IsFalse(buffer.Enqueue(() => null));
            }

            [TestMethod]
            public void DoesNotCountNullTransmissionsReturnedFromEmptyStorageAgainstMaxNumber()
            {
                var buffer = new TransmissionBuffer { Capacity = 1 };
                buffer.Enqueue(() => null);

                Transmission transmission2 = new StubTransmission();
                Assert.IsTrue(buffer.Enqueue(() => transmission2));
            }

            [TestMethod]
            public void DoesNotContinueAsyncOperationsOnCapturedSynchronizationContextToImprovePerformance()
            {
                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    var buffer = new TransmissionBuffer();
                    buffer.Enqueue(() => new StubTransmission());
                }

                Assert.IsFalse(postedBack);
            }
        }

        [TestClass]
        public class Dequeue : TransmissionBufferTest
        {
            [TestMethod]
            public void ReturnsOldestEnquedTransmission()
            {
                var buffer = new TransmissionBuffer();

                Transmission transmission1 = new StubTransmission();
                buffer.Enqueue(() => transmission1);

                Transmission transmission2 = new StubTransmission();
                buffer.Enqueue(() => transmission2);

                Assert.AreSame(transmission1, buffer.Dequeue());
                Assert.AreSame(transmission2, buffer.Dequeue());
            }

            [TestMethod]
            public void ReturnsNullWhenBufferIsEmpty()
            {
                var buffer = new TransmissionBuffer();
                Assert.IsNull(buffer.Dequeue());
            }

            [TestMethod]
            public void MakesSpaceForOneNewTransmissionWhenOldTransmissionDequeuedSuccessfully()
            {
                var buffer = new TransmissionBuffer { Capacity = 1 };
                buffer.Enqueue(() => new StubTransmission());
                buffer.Dequeue();
                Assert.IsTrue(buffer.Enqueue(() => new StubTransmission()));
            }

            [TestMethod]
            public void DoesNotMakesSpaceForNewTransmissionWhenBufferIsEmpty()
            {
                var buffer = new TransmissionBuffer { Capacity = 0 };
                buffer.Dequeue();
                Assert.IsFalse(buffer.Enqueue(() => new StubTransmission()));
            }
        }

        [TestClass]
        public class Size
        {
            [TestMethod]
            public void StartsAtZero()
            {
                var buffer = new TransmissionBuffer();                
                Assert.AreEqual(0, buffer.Size);
            }

            [TestMethod]
            public void ReflectsContentLengthOfTransmissionsAddedByEnqueueAsync()
            {
                Transmission transmission = new StubTransmission(new byte[42]);
                var buffer = new TransmissionBuffer();

                buffer.Enqueue(() => transmission);

                Assert.AreEqual(transmission.Content.Length, buffer.Size);
            }

            [TestMethod]
            public void ReflectsContentLengthOfTransmissionsRemovedByDequeueAsync()
            {
                var buffer = new TransmissionBuffer();

                buffer.Enqueue(() => new StubTransmission(new byte[10]));
                buffer.Dequeue();

                Assert.AreEqual(0, buffer.Size);
            }
        }

        [TestClass]
        public class TransmissionDequeued
        {
            [TestMethod]
            public void IsRaisedWhenTransmissionWasDequeuedSuccessfully()
            {
                object eventSender = null;
                TransmissionProcessedEventArgs eventArgs = null;
                var buffer = new TransmissionBuffer();
                buffer.TransmissionDequeued += (sender, args) =>
                {
                    eventSender = sender;
                    eventArgs = args;
                };

                buffer.Enqueue(() => new StubTransmission());

                Transmission dequeuedTransmission = buffer.Dequeue();

                Assert.AreSame(buffer, eventSender);
                Assert.AreSame(dequeuedTransmission, eventArgs.Transmission);
            }

            [TestMethod]
            public void RaisedWhenDequeueFromEmptyBufferWasAttempted()
            {
                object eventSender = null;
                TransmissionProcessedEventArgs eventArgs = null;
                var buffer = new TransmissionBuffer();
                buffer.TransmissionDequeued += (sender, args) =>
                {
                    eventSender = sender;
                    eventArgs = args;
                };

                buffer.Dequeue();

                Assert.AreSame(buffer, eventSender);
                Assert.AreSame(null, eventArgs.Transmission);
            }
        }
    }
}
