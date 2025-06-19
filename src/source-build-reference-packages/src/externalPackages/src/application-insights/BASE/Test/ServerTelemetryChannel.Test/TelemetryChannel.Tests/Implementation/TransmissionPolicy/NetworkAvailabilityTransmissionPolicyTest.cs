namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Net.Sockets;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Channel.Helpers;
    using TaskEx = System.Threading.Tasks.Task;

    [TestClass]
    [TestCategory("TransmissionPolicy")]
    public class NetworkAvailabilityTransmissionPolicyTest
    {
        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class Class : NetworkAvailabilityTransmissionPolicyTest
        {
            [TestMethod]
            public void ConstructorDoesNotSubscribeToNetworkChangeEventsToPreventExceptionsBeforePolicyIsInitialized()
            {
                var network = new StubNetwork { OnIsAvailable = () => false };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                network.OnStatusChanged(EventArgs.Empty);

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void DisposeUnsubscribesNetworkChangeEvents()
            {
                bool unsubscribeCalled = false;
                var network = new StubNetwork { OnRemoveAddressChangedEventHandler = (h) => { unsubscribeCalled = true; } };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                policy.Dispose();

                Assert.IsTrue(unsubscribeCalled);
            }
        }

        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class Initialize : NetworkAvailabilityTransmissionPolicyTest
        {
            [TestMethod]
            public void SetsMaxSenderAndBufferCapacitiesToZeroWhenNetworkIsUnavailable()
            {
                var network = new StubNetwork { OnIsAvailable = () => false };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                policy.Initialize(new StubTransmitter());

                Assert.AreEqual(0, policy.MaxSenderCapacity);
                Assert.AreEqual(0, policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void InitializeCatchAllExceptionsAndDoesNotSetCapacity()
            {
                var network = new StubNetwork { OnIsAvailable = () => { throw new Exception("error"); } };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                try
                {
                    policy.Initialize(new StubTransmitter());
                }
                catch(Exception ex)
                {
                    Assert.Fail("No exception should have been thrown from Initialize. Exception thrown: " + ex.ToString());
                }

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void DoesNotSetMaxSenderAndBufferCapacitiesToZeroWhenNetworkIsAvailable()
            {
                var network = new StubNetwork { OnIsAvailable = () => true };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                policy.Initialize(new StubTransmitter());

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void DoesNotSetMaxSenderAndBufferCapacitiesToZeroWhenNetworkAvailabilityCheckFailsHopingThatTransmissionsThemselvesMaySucceed()
            {
                var network = new StubNetwork { OnIsAvailable = () => { throw new SocketException(); } };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);

                policy.Initialize(new StubTransmitter());

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void HandlesExceptionsThrownByNetworkWhenAddingAddressChangedEventHandler()
            {
                using (var listener = new TestEventListener())
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Warning, (EventKeywords)AllKeywords);
                    
                    var exception = new SocketException();
                    var network = new StubNetwork { OnAddAddressChangedEventHandler = handler => { throw exception; } };
                    var policy = new NetworkAvailabilityTransmissionPolicy(network);

                    policy.Initialize(new StubTransmitter());

                    EventWrittenEventArgs error = listener.Messages.First(arg => arg.EventId == 38);
                    AssertEx.Contains(exception.Message, (string)error.Payload[0], StringComparison.CurrentCulture);
                }
            }
        }

        [TestClass]
        [TestCategory("TransmissionPolicy")]
        public class HandleNetworkStatusChangedEvent : NetworkAvailabilityTransmissionPolicyTest
        {
            [TestMethod]
            public void SetsMaxSenderAndBufferCapacitiesToZeroWhenNetworkBecomesUnavailable()
            {
                bool isNetworkAvailable = true;
                var network = new StubNetwork { OnIsAvailable = () => isNetworkAvailable };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);
                policy.Initialize(new StubTransmitter());

                isNetworkAvailable = false;
                network.OnStatusChanged(EventArgs.Empty);

                Assert.AreEqual(0, policy.MaxSenderCapacity);
                Assert.AreEqual(0, policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void ResetsMaxSenderAndBufferCapacitiesToNullWhenNetworkBecomesAvailable()
            {
                bool isNetworkAvailable = false;
                var network = new StubNetwork { OnIsAvailable = () => isNetworkAvailable };
                var policy = new NetworkAvailabilityTransmissionPolicy(network);
                policy.Initialize(new StubTransmitter());

                isNetworkAvailable = true;
                network.OnStatusChanged(EventArgs.Empty);

                Assert.IsNull(policy.MaxSenderCapacity);
                Assert.IsNull(policy.MaxBufferCapacity);
            }

            [TestMethod]
            public void AsksTransmitterToApplyPoliciesWhenNetworkAvailabilityChanges()
            {
                bool policiesApplied = false;
                var transmitter = new StubTransmitter();
                transmitter.OnApplyPolicies = () => 
                {
                    policiesApplied = true;
                };

                var network = new StubNetwork();
                var policy = new NetworkAvailabilityTransmissionPolicy(network);
                policy.Initialize(transmitter);

                network.OnStatusChanged(EventArgs.Empty);

                Assert.IsTrue(policiesApplied);
            }

            [TestMethod]
            public void HandlesExceptionsThrownByNetworkIsAvailable()
            {
                using (var listener = new TestEventListener())
                {
                    const long AllKeywords = -1;
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Warning, (EventKeywords)AllKeywords);

                    var exception = new SocketException();
                    var network = new StubNetwork { OnIsAvailable = () => { throw exception; } };
                    var policy = new NetworkAvailabilityTransmissionPolicy(network);
                    policy.Initialize(new StubTransmitter());

                    network.OnStatusChanged(EventArgs.Empty);

                    EventWrittenEventArgs error = listener.Messages.First();
                    AssertEx.Contains(exception.Message, (string)error.Payload[0], StringComparison.Ordinal);
                }
            }
        }
    }
}
