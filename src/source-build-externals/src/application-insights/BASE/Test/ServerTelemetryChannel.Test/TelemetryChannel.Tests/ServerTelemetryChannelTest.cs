namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    
    using Helpers;
    using System.Collections.Generic;
    using Extensibility.Implementation;

    using System.Diagnostics.Tracing;
    using TaskEx = System.Threading.Tasks.Task;

    public class ServerTelemetryChannelTest : IDisposable
    {
        private readonly TelemetryConfiguration configuration;

        public ServerTelemetryChannelTest()
        {
            this.configuration = new TelemetryConfiguration();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.configuration.Dispose();            
        }

        [TestClass]
        public class Constructor : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void InitializesTransmitterWithNetworkAvailabilityPolicy()
            {
                var network = new StubNetwork { OnIsAvailable = () => false };

                var channel = new ServerTelemetryChannel(network, new StubApplicationLifecycle());
                channel.Initialize(new TelemetryConfiguration());
                Thread.Sleep(50);

                Assert.AreEqual(0, channel.Transmitter.Sender.Capacity);
            }
        }

        [TestClass]
        public class DefaultBackoffEnabledReportingInterval
        {
            [TestMethod]
            public void DefaultBackoffEnabledReportingIntervalUpdatesBackoffLogicManager()
            {
                ServerTelemetryChannel channel = new ServerTelemetryChannel
                {
                    DefaultBackoffEnabledReportingInterval = TimeSpan.FromHours(42)
                };

                Assert.AreEqual(channel.Transmitter.BackoffLogicManager.DefaultBackoffEnabledReportingInterval, TimeSpan.FromHours(42));
            }
        }

        [TestClass]
        public class DeveloperMode : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void DeveloperModeIsNullByDefault()
            {
                var channel = new ServerTelemetryChannel();
                Assert.IsNull(channel.DeveloperMode);
            }

            [TestMethod]
            public void DeveloperModeCanBeModifiedByConfiguration()
            {
                var channel = new ServerTelemetryChannel();             
                channel.DeveloperMode = true;
                Assert.IsTrue(channel.DeveloperMode.Value);
            }

            [TestMethod]
            public void WhenSetToTrueChangesTelemetryBufferCapacityToOneForImmediateTransmission()
            {
                var channel = new ServerTelemetryChannel();
                channel.DeveloperMode = true;
                Assert.AreEqual(1, channel.TelemetryBuffer.Capacity);
            }

            [TestMethod]
            public void WhenSetToFalseChangesTelemetryBufferCapacityToOriginalValueForBufferedTransmission()
            {
                var channel = new ServerTelemetryChannel();
                int originalTelemetryBufferSize = channel.TelemetryBuffer.Capacity;

                channel.DeveloperMode = true;
                channel.DeveloperMode = false;

                Assert.AreEqual(originalTelemetryBufferSize, channel.TelemetryBuffer.Capacity);
            }

            [TestMethod]
            public void DoesNotModifyComponentConfigurationWhenNewValueIsSameAsOldValue()
            {
                var channel = new ServerTelemetryChannel();
                int oldTelemetryBufferSize = channel.TelemetryBuffer.Capacity;

                channel.DeveloperMode = false;

                Assert.AreEqual(oldTelemetryBufferSize, channel.TelemetryBuffer.Capacity);
            }
        }

        [TestClass]
        public class EndpointAddress : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void EndpointAddressCanBeModifiedByConfiguration()
            {
                var channel = new ServerTelemetryChannel();
               
                Uri expectedEndpoint = new Uri("http://abc.com");
                channel.EndpointAddress = expectedEndpoint.AbsoluteUri;

                Assert.AreEqual(expectedEndpoint, new Uri(channel.EndpointAddress));
            }

            [TestMethod]
            public void EndpointAddressIsStoredBySerializer()
            {
                var channel = new ServerTelemetryChannel();
                
                Uri expectedEndpoint = new Uri("http://abc.com");
                channel.EndpointAddress = expectedEndpoint.AbsoluteUri;
                
                Assert.AreEqual(expectedEndpoint, channel.TelemetrySerializer.EndpointAddress);
            }
        }

        [TestClass]
        public class MaxTelemetryBufferDelay : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void DataUploadIntervalInSecondsIsStoredByTelemetryBuffer()
            {
                var channel = new ServerTelemetryChannel();
               
                TimeSpan expectedUploadInterval = TimeSpan.FromSeconds(42);
                channel.MaxTelemetryBufferDelay = expectedUploadInterval;

                Assert.AreEqual(expectedUploadInterval, channel.MaxTelemetryBufferDelay);
            }
        }

        [TestClass]
        public class MaxTelemetryBufferCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void GetterReturnsTelemetryBufferCapacity()
            {
                var channel = new ServerTelemetryChannel();
                channel.TelemetryBuffer.Capacity = 42;
                Assert.AreEqual(42, channel.MaxTelemetryBufferCapacity);
            }

            [TestMethod]
            public void SetterChangesTelemetryBufferCapacity()
            {
                var channel = new ServerTelemetryChannel();
                channel.MaxTelemetryBufferCapacity = 42;
                Assert.AreEqual(42, channel.TelemetryBuffer.Capacity);
            }
        }

        [TestClass]
        public class MaxTransmissionBufferCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void ReturnsMaxBufferCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.MaxBufferCapacity = 42;
                Assert.AreEqual(42, channel.MaxTransmissionBufferCapacity);
            }

            [TestMethod]
            public void ChangesMaxBufferCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.MaxTransmissionBufferCapacity = 42;
                Assert.AreEqual(42, channel.Transmitter.MaxBufferCapacity);
            }
        }

        [TestClass]
        public class MaxTransmissionSenderCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void ReturnsMaxSenderCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.MaxSenderCapacity = 42;
                Assert.AreEqual(42, channel.MaxTransmissionSenderCapacity);
            }

            [TestMethod]
            public void ChangesMaxSenderCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.MaxTransmissionSenderCapacity = 42;
                Assert.AreEqual(42, channel.Transmitter.MaxSenderCapacity);
            }
        }

        [TestClass]
        public class MaxTransmissionStorageCapacity : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void GetterReturnsMaxStorageCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.MaxStorageCapacity = 42000;
                Assert.AreEqual(42000, channel.MaxTransmissionStorageCapacity);
            }

            [TestMethod]
            public void SetterChangesStorageCapacityOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.MaxTransmissionStorageCapacity = 42000;
                Assert.AreEqual(42000, channel.Transmitter.MaxStorageCapacity);
            }
        }

        [TestClass]
        public class StorageFolder : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void GetterReturnsStorageFolderOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.Transmitter.StorageFolder = "test";
                Assert.AreEqual("test", channel.StorageFolder);
            }

            [TestMethod]
            public void SetterChangesStorageFolderOfTransmitter()
            {
                var channel = new ServerTelemetryChannel { Transmitter = new StubTransmitter() };
                channel.StorageFolder = "test";
                Assert.AreEqual("test", channel.Transmitter.StorageFolder);
            }
        }

        [TestClass]
        public class Flush : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void FlushesTelemetryBuffer()
            {
                var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
                var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                channel.Flush();

                mockTelemetryBuffer.Verify(x => x.FlushAsync());
            }

            [TestMethod]
            public void WaitsForAsynchronousFlushToCompleteAndAllowsItsExceptionsToBubbleUp()
            {
                var expectedException = new Exception();
                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(expectedException);
                var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
                mockTelemetryBuffer.Setup(x => x.FlushAsync()).Returns(tcs.Task);
                var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                var actualException = AssertEx.Throws<Exception>(() => channel.Flush());

                Assert.AreSame(expectedException, actualException);
            }
        }

        [TestClass]
        public class Initialize : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void AppliesTransmissionPoliciesToBeginSendingStoredTelemetry()
            {
                var transmissionPoliciesApplied = new ManualResetEvent(false);
                var transmitter = new StubTransmitter();
                transmitter.OnApplyPolicies = () =>
                {
                    transmissionPoliciesApplied.Set();
                };
                var channel = new ServerTelemetryChannel { Transmitter = transmitter };
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                var initializedConfiguration = new TelemetryConfiguration();
                channel.Initialize(initializedConfiguration);

                Assert.IsTrue(transmissionPoliciesApplied.WaitOne(1000));
            }

            [TestMethod]
            public void InitializeCallsTransmitterInitialize()
            {
                var transmitterInitialized = new ManualResetEvent(false);
                var transmitter = new StubTransmitter();
                transmitter.OnInitialize = () =>
                {
                    transmitterInitialized.Set();
                };
                var channel = new ServerTelemetryChannel { Transmitter = transmitter };

                var initializedConfiguration = new TelemetryConfiguration();
                channel.Initialize(initializedConfiguration);

                Assert.IsTrue(transmitterInitialized.WaitOne(1000));
            }
        }

        [TestClass]
        public class Send : ServerTelemetryChannelTest
        {
            [TestMethod]
            public void PassesTelemetryToTelemetryProcessor()
            {
                ITelemetry sentTelemetry = null;
                var channel = new ServerTelemetryChannel();
                channel.Initialize(TelemetryConfiguration.CreateDefault());
                channel.TelemetryProcessor = new StubTelemetryProcessor(null) { OnProcess = (t) => sentTelemetry = t };

                var telemetry = new StubTelemetry();
                telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                channel.Send(telemetry);

                Assert.AreEqual(telemetry, sentTelemetry);
            }

            [TestMethod]
            public void DropsTelemetryWithNoInstrumentationKey()
            {
                ITelemetry sentTelemetry = null;
                var channel = new ServerTelemetryChannel();
                channel.Initialize(TelemetryConfiguration.CreateDefault());
                channel.TelemetryProcessor = new StubTelemetryProcessor(null) { OnProcess = (t) => sentTelemetry = t };

                var telemetry = new StubTelemetry();
                // No instrumentation key

                using (TestEventListener listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.Verbose);

                    channel.Send(telemetry);

                    Assert.IsNull(sentTelemetry);
                    var expectedMessage = listener.Messages.First();
                    Assert.AreEqual(67, expectedMessage.EventId);
                }
            }
        }

        [TestClass]
        public class InternalOperation : ServerTelemetryChannelTest
        {
            class TransmissionStubChecksInternalOperation : Transmission
            {
                public Action<bool> WasCalled;

                public override Task<HttpWebResponseWrapper> SendAsync()
                {
                    Assert.IsTrue(SdkInternalOperationsMonitor.IsEntered());
                    this.WasCalled(true);
                    return base.SendAsync();
                }
            }

            class TelemetrySerializerStub : TelemetrySerializer
            {
                public Action<bool> WasCalled;

                public TelemetrySerializerStub(Transmitter t) : base(t)
                {
                }

                public override void Serialize(ICollection<ITelemetry> items)
                {
                    var transmission = new TransmissionStubChecksInternalOperation();
                    transmission.WasCalled = this.WasCalled;
                    base.Transmitter.Enqueue(transmission);
                }
            }

            [TestMethod]
            public void SendWillBeMarkedAsInternalOperation()
            {
                bool wasCalled = false;
                var channel = new ServerTelemetryChannel();
                channel.TelemetrySerializer = new TelemetrySerializerStub(channel.Transmitter) { WasCalled = (called) => { wasCalled = called; } };
#if NETCOREAPP
                channel.TelemetryBuffer = new TelemetryChannel.Implementation.TelemetryBuffer(channel.TelemetrySerializer, null);
#else
                channel.TelemetryBuffer = new TelemetryChannel.Implementation.TelemetryBuffer(channel.TelemetrySerializer, new WebApplicationLifecycle());
#endif
                channel.TelemetryProcessor = channel.TelemetryBuffer;
                channel.MaxTelemetryBufferCapacity = 1;
                channel.Initialize(TelemetryConfiguration.CreateDefault());

                var telemetry = new StubTelemetry();
                telemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                channel.Send(telemetry);
                Thread.Sleep(TimeSpan.FromSeconds(1));

                Assert.IsTrue(wasCalled);
            }
        }

        [TestClass]
        public class ServerTelemetryChannelConfigurationTests
        {
            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_DefaultScenario()
            {
                var channel = new ServerTelemetryChannel();
                Assert.AreEqual(null, channel.EndpointAddress);

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                };

                Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress);
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromConfiguration_DefaultEndpoint()
            {
                var connectionstring = $"instrumentationkey=00000000-0000-0000-0000-000000000000";

                var channel = new ServerTelemetryChannel();

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                    ConnectionString = connectionstring,
                };

                Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress);
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromConfiguration_ExplicitEndpoint_WithTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1/";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var channel = new ServerTelemetryChannel();

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                    ConnectionString = connectionString
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromConfiguration_ExplicitEndpoint_WithoutTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var channel = new ServerTelemetryChannel();

                var configuration = new TelemetryConfiguration
                {
                    TelemetryChannel = channel,
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromInitialize_ExplicitEndpoint_WithTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1/";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var configuration = new TelemetryConfiguration
                {
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                var channel = new ServerTelemetryChannel();
                channel.Initialize(configuration);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
            }


            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromInitialize_ExplicitEndpoint_WithoutTrailingSlash()
            {
                var explicitEndpoint = "https://127.0.0.1";
                var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

                var configuration = new TelemetryConfiguration
                {
                    ConnectionString = connectionString,
                };

                Assert.AreEqual("https://127.0.0.1/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                var channel = new ServerTelemetryChannel();
                channel.Initialize(configuration);
                Assert.AreEqual("https://127.0.0.1/v2/track", channel.EndpointAddress);
            }

            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyEndpointConnectionString_SetFromInitialize_DefaultEndpoint()
            {
                var channel = new ServerTelemetryChannel();
                Assert.AreEqual(null, channel.EndpointAddress, "channel endpoint should be null before Initialize");

                var configuration = new TelemetryConfiguration();
                Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                channel.Initialize(configuration);
                Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress, "channel endpoint should match config after Initialize");
            }
            
            [TestMethod]
            [TestCategory("ConnectionString")]
            public void VerifyCanChangeEndpointAfterInitialize()
            {
                var channel = new ServerTelemetryChannel();
                Assert.AreEqual(null, channel.EndpointAddress, "channel endpoint should be null before Initialize");

                var configuration = new TelemetryConfiguration();
                Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

                channel.Initialize(configuration);
                Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress, "channel endpoint should match config after Initialize");

                channel.EndpointAddress = "http://localhost:1234";
                Assert.AreEqual("http://localhost:1234/", channel.EndpointAddress, "channel endpoint was not set");
            }
        }
    }

    [TestClass]
    public class FlushAsyncTask : ServerTelemetryChannelTest
    {
        [TestMethod]
        public void FlushesTelemetryBuffer()
        {
            var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
            var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
            channel.Initialize(TelemetryConfiguration.CreateDefault());

            channel.FlushAsync(default);

            mockTelemetryBuffer.Verify(x => x.FlushAsync(default));
        }

        [TestMethod]
        public void WaitsForAsynchronousFlushToCompleteAndAllowsItsExceptionsToBubbleUp()
        {
            var expectedException = new Exception();
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetException(expectedException);
            var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
            mockTelemetryBuffer.Setup(x => x.FlushAsync(CancellationToken.None)).Returns(tcs.Task);
            var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
            channel.Initialize(TelemetryConfiguration.CreateDefault());

            var actualException = AssertEx.ThrowsAsync<Exception>(async () => await channel.FlushAsync(default));

            Assert.AreSame(expectedException, actualException.Result);
        }

        [TestMethod]
        public async Task FlushAsyncRespectsCancellationToken()
        {
            var mockTelemetryBuffer = new Mock<TelemetryChannel.Implementation.TelemetryBuffer>();
            var channel = new ServerTelemetryChannel { TelemetryBuffer = mockTelemetryBuffer.Object };
            channel.Initialize(TelemetryConfiguration.CreateDefault());

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => channel.FlushAsync(new CancellationToken(true)));
         }
    }
}
