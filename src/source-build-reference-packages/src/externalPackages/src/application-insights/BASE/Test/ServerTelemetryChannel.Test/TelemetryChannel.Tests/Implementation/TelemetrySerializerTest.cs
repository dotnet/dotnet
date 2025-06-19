namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class TelemetrySerializerTest
    {
        [TestClass]
        public class Class : TelemetrySerializerTest
        {
            [TestMethod]
            public void ConstructorThrowsArgumentNullExceptionIfTransmitterIsNull()
            {
                AssertEx.Throws<ArgumentNullException>(() => new TelemetrySerializer(null));
            }
        }

        [TestClass]
        public class EndpointAddress
        {
            [TestMethod]
            public void DefaultValueIsNull()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                Assert.AreEqual(null, serializer.EndpointAddress);
            }

            [TestMethod]
            public void SetterThrowsArgumentNullExceptionToPreventUsageErrors()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                AssertEx.Throws<ArgumentNullException>(() => serializer.EndpointAddress = null);
            }

            [TestMethod]
            public void CanBeChangedByChannelBasedOnConfigurationToRedirectTelemetryToDifferentEnvironment()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                var expectedValue = new Uri("int://environment");
                serializer.EndpointAddress = expectedValue;
                Assert.AreEqual(expectedValue, serializer.EndpointAddress);
            }
        }

        [TestClass]
        public class SerializeAsync
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionWhenTelemetryIsNullToPreventUsageErrors()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                AssertEx.Throws<ArgumentNullException>(() => serializer.Serialize(null));
            }

            [TestMethod]
            public void ThrowsArgumentExceptionWhenTelemetryIsEmptyToPreventUsageErrors()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                AssertEx.Throws<ArgumentException>(() => serializer.Serialize(new List<ITelemetry>()));
            }

            [TestMethod]
            public void SerializesTelemetryOnSameThreadBecauseItAssumesItIsAlreadyOnThreadPool()
            {
                int serializationThreadId = -1;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t => 
                {
                    serializationThreadId = Thread.CurrentThread.ManagedThreadId;
                };

                var serializer = new TelemetrySerializer(transmitter)
                {
                    EndpointAddress = new Uri("https://127.0.0.1")
                };

                serializer.Serialize(new[] { new StubTelemetry() });

                Assert.AreEqual(serializationThreadId, Thread.CurrentThread.ManagedThreadId);
            }

            [TestMethod]
            public void EnqueuesTransmissionWithExpectedPropertiesForUnknownTelemetry()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                serializer.Serialize(new[] { new StubTelemetry() });

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);
                Assert.AreEqual("{" +
                    "\"name\":\"AppEvents\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"EventData\"," +
                        "\"baseData\":{\"ver\":2," +
                            "\"name\":\"ConvertedTelemetry\"}" +
                        "}" +
                    "}", Unzip(transmission.Content));
            }

            [TestMethod]
            public void EnqueuesTransmissionWithExpectedPropertiesForKnownTelemetry()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                serializer.Serialize(new[] { new StubSerializableTelemetry() });

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);

                var expectedContent = "{" +
                    "\"name\":\"StubTelemetryName\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"StubTelemetryBaseType\"," +
                        "\"baseData\":{}" +
                        "}" +
                    "}";
                Assert.AreEqual(expectedContent, Unzip(transmission.Content));
            }

            [TestMethod]
            public void DoesNotContinueAsyncOperationsOnCapturedSynchronizationContextToImprovePerformance()
            {
                var transmitter = new StubTransmitter() { OnEnqueue = transmission => Task.Run(() => { }) };
                var serializer = new TelemetrySerializer(transmitter)
                {
                    EndpointAddress = new Uri("https://127.0.0.1")
                };

                bool postedBack = false;
                using (var context = new StubSynchronizationContext())
                {
                    context.OnPost = (callback, state) =>
                    {
                        postedBack = true;
                        callback(state);
                    };

                    serializer.Serialize(new[] { new StubTelemetry() });
                }

                Assert.IsFalse(postedBack);
            }

            [TestMethod]
            public void EnqueuesTransmissionWithSetTransmissionStatusEvent()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                // Set TransmissionStatusEvent does not change the behavior, only wires up event to transmission.
                serializer.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args) { };
                serializer.Serialize(new[] { new StubSerializableTelemetry() });

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);

                var expectedContent = "{" +
                    "\"name\":\"StubTelemetryName\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"StubTelemetryBaseType\"," +
                        "\"baseData\":{}" +
                        "}" +
                    "}";
                Assert.AreEqual(expectedContent, Unzip(transmission.Content));
            }

            private static string Unzip(byte[] content)
            {
                var memoryStream = new MemoryStream(content);
                var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                using (var streamReader = new StreamReader(gzipStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        [TestClass]
        public class SerializeAsyncTask
        {
            [TestMethod]
            public void ThrowsExceptionWhenEndpointAddressIsNull()
            {
                var serializer = new TelemetrySerializer(new StubTransmitter());
                AssertEx.Throws<Exception>(() => serializer.SerializeAsync(null, default));
            }

            [TestMethod]
            public void ReturnsBoolenTaskWhenTelemetryIsNullOrEmpty()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                var result = serializer.SerializeAsync(null, default);
                Assert.IsInstanceOfType(result, typeof(Task<bool>));
                Assert.IsTrue(result.Result);

                result = serializer.SerializeAsync(new List<ITelemetry>(), default);
                Assert.IsInstanceOfType(result, typeof(Task<bool>));
                Assert.IsTrue(result.Result);
            }

            [TestMethod]
            public async Task EnqueuesTransmissionWithExpectedPropertiesForUnknownTelemetry()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                    transmission.IsFlushAsyncInProgress = true;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                var taskResult = await serializer.SerializeAsync(new[] { new StubTelemetry() }, default);

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);
                Assert.AreEqual("{" +
                    "\"name\":\"AppEvents\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"EventData\"," +
                        "\"baseData\":{\"ver\":2," +
                            "\"name\":\"ConvertedTelemetry\"}" +
                        "}" +
                    "}", Unzip(transmission.Content));
                Assert.IsTrue(taskResult);
            }

            [TestMethod]
            public async Task EnqueuesTransmissionWithExpectedPropertiesForKnownTelemetry()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                var taskResult = await serializer.SerializeAsync(new[] { new StubSerializableTelemetry() }, default);

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);

                var expectedContent = "{" +
                    "\"name\":\"StubTelemetryName\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"StubTelemetryBaseType\"," +
                        "\"baseData\":{}" +
                        "}" +
                    "}";
                Assert.AreEqual(expectedContent, Unzip(transmission.Content));
                Assert.IsTrue(taskResult);
            }

            [TestMethod]
            public async Task ReturnsFalseWhenTransmissionIsNotSentOrStored()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                    transmission.IsFlushAsyncInProgress = false;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                var taskResult = await serializer.SerializeAsync(new[] { new StubSerializableTelemetry() }, default);
                Assert.IsFalse(taskResult);
            }

            [TestMethod]
            public async Task EnqueuesTransmissionWithSetTransmissionStatusEvent()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                // Set TransmissionStatusEvent does not change the behavior, only wires up event to transmission.
                serializer.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args) { };
                var taskResult = await serializer.SerializeAsync(new[] { new StubSerializableTelemetry() }, default);

                Assert.AreEqual(serializer.EndpointAddress, transmission.EndpointAddress);
                Assert.AreEqual("application/x-json-stream", transmission.ContentType);
                Assert.AreEqual("gzip", transmission.ContentEncoding);

                var expectedContent = "{" +
                    "\"name\":\"StubTelemetryName\"," +
                    "\"time\":\"0001-01-01T00:00:00.0000000Z\"," +
                    "\"data\":{\"baseType\":\"StubTelemetryBaseType\"," +
                        "\"baseData\":{}" +
                        "}" +
                    "}";
                Assert.AreEqual(expectedContent, Unzip(transmission.Content));
            }

            [TestMethod]
            public async Task SerializeAsyncRespectsCancellationTokenWhenTelemetryIsEmpty()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => serializer.SerializeAsync(null, new CancellationToken(true)));
            }

            [TestMethod]
            public async Task SerializeAsyncRespectsCancellationToken()
            {
                Transmission transmission = null;
                var transmitter = new StubTransmitter();
                transmitter.OnEnqueue = t =>
                {
                    transmission = t;
                };

                var serializer = new TelemetrySerializer(transmitter) { EndpointAddress = new Uri("http://expected.uri") };
                await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => serializer.SerializeAsync(new[] { new StubTelemetry() }, new CancellationToken(true)));
            }

            private static string Unzip(byte[] content)
            {
                var memoryStream = new MemoryStream(content);
                var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                using (var streamReader = new StreamReader(gzipStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
