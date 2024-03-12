#if NETCOREAPP
namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{    
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;    
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;        
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;    
    using Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;    
    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using System.IO;


    [TestClass]
    [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
    public class ServerTelemetryChannelE2ETests
    {
        private const string Localurl = "http://localhost:6090";
        private const string LocalurlNotRunning = "http://localhost:6091";
        private const long AllKeywords = -1;
        private const int SleepInMilliseconds = 10000;
        private const int DelayfromWebServerInMilliseconds = 2000;
        private const int CancellationTimeOutInMilliseconds = 1000;

        [TestMethod]
        [Ignore("Ignored as unstable in Test/Build machines. Run locally when making changes to ServerChannel")]
        public void ChannelSendsTransmission()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                IList<ITelemetry> telemetryItems = new List<ITelemetry>();
                var telemetry = new EventTelemetry("test event name");    
                telemetry.Context.InstrumentationKey = "dummy";
                telemetryItems.Add((telemetry));
                var serializedExpected = JsonSerializer.Serialize(telemetryItems);

                localServer.ServerLogic = async (ctx) =>
                {
                    byte[] buffer = new byte[2000];
                    await ctx.Request.Body.ReadAsync(buffer, 0, 2000);     
                    Assert.AreEqual(serializedExpected, buffer);
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    DeveloperMode = true,
                    EndpointAddress = Localurl
                };

                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                // ACT 
                // Data would be sent to the LocalServer which validates it.
                channel.Send(telemetry);                    
            }
        }

        [TestMethod]
        [Ignore("Ignored as unstable in Test/Build machines. Run locally when making changes to ServerChannel")]
        public void ChannelLogsSuccessfulTransmission()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {                
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    DeveloperMode = true,
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords) AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();    
                    // Event 22 is logged upon successful transmission.
                    var traces = allTraces.Where(item => item.EventId == 22).ToList();
                    Assert.AreEqual(1, traces.Count);                    
                }
            }            
        }

        [TestMethod]
        [Ignore("Ignored as unstable in Test/Build machines. Run locally when making changes to ServerChannel")]
        public void ChannelLogsFailedTransmissionDueToServerError()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {                
                localServer.ServerLogic = async (ctx) =>
                {
                    // Error from AI Backend.
                    ctx.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    await ctx.Response.WriteAsync("InternalError");
                };

                var channel = new ServerTelemetryChannel
                {
                    DeveloperMode = true,
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces
                    var allTraces = listener.Messages.ToList();
                    
                    // Event 54 is logged upon successful transmission.
                    var traces = allTraces.Where(item => item.EventId == 54).ToList();
                    Assert.AreEqual(1, traces.Count);
                    // 500 is the response code.
                    Assert.AreEqual("500", traces[0].Payload[1]);
                }
            }
        }

        [TestMethod]
        [Ignore("Ignored as unstable in Test/Build machines. Run locally when making changes to ServerChannel")]
        public void ChannelHandlesFailedTransmissionDueToUnknownNetworkError()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // This code does not matter as Channel is configured to 
                   // with an incorrect endpoint.
                    await ctx.Response.WriteAsync("InternalError");
                };

                var channel = new ServerTelemetryChannel
                {
                    DeveloperMode = true,
                    EndpointAddress = LocalurlNotRunning
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    Thread.Sleep(SleepInMilliseconds);

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    var traces = allTraces.Where(item => item.EventId == 54).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.IsTrue(traces[0].Payload[1].ToString().Contains("An error occurred while sending the request"));
                }
            }
        }

        [TestMethod]
        [Ignore("Ignored as unstable in Test/Build machines. Run locally when making changes to ServerChannel")]
        public void ChannelLogsResponseBodyFromTransmissionWhenVerboseEnabled()
        {
            var expectedResponseContents = "this is the expected response";

            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {                                        
                    await ctx.Response.WriteAsync(expectedResponseContents);
                };

                var channel = new ServerTelemetryChannel
                {
                    DeveloperMode = true,
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // Enable CoreEventSource as Transmission logic is in base sdk.
                    // and it'll parse response only on Verbose enabled.
                    using (var listenerCore = new TestEventListener())
                    {
                        listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways,
                            (EventKeywords) AllKeywords);


                        // ACT
                        var telemetry = new EventTelemetry("test event name");
                        telemetry.Context.InstrumentationKey = "dummy";
                        channel.Send(telemetry);
                        Thread.Sleep(SleepInMilliseconds);
                    }

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    var traces = allTraces.Where(item => item.EventId == 70).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.IsTrue(traces[0].Payload[1].ToString().Contains(expectedResponseContents));
                }
            }
        }

        [TestMethod]
        public async Task ChannelSendsTransmissionOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                IList<ITelemetry> telemetryItems = new List<ITelemetry>();
                var telemetry = new EventTelemetry("test event name");
                telemetry.Context.InstrumentationKey = "dummy";
                telemetryItems.Add((telemetry));
                var serializedExpected = JsonSerializer.Serialize(telemetryItems);

                localServer.ServerLogic = async (ctx) =>
                {
                    byte[] buffer = new byte[2000];
                    await ctx.Request.Body.ReadAsync(buffer, 0, 2000);
                    Assert.AreEqual(serializedExpected, buffer);
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };

                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                // ACT 
                // Data would be sent to the LocalServer which validates it.
                channel.Send(telemetry);
                var flushResult = await channel.FlushAsync(default);
                Assert.IsTrue(flushResult);
            }
        }

        [TestMethod]
        public async Task ChannelSendsTransmissionAndMovesBufferToStorageOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                IList<ITelemetry> telemetryItems = new List<ITelemetry>();
                var telemetry = new EventTelemetry("test event name");
                telemetry.Context.InstrumentationKey = "dummy";
                telemetryItems.Add((telemetry));
                var serializedExpected = JsonSerializer.Serialize(telemetryItems);

                localServer.ServerLogic = async (ctx) =>
                {
                    await Task.Delay(DelayfromWebServerInMilliseconds);
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };

                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);
                    // ACT 
                    // Data would be sent to the LocalServer which validates it.

                    for (int i = 0; i < 10; i++)
                    {
                        channel.Transmitter.Buffer.Enqueue(() => new StubTransmission());
                    }

                    channel.Send(telemetry);
                    var flushTask = channel.FlushAsync(default);
                    
                    try
                    {
                        await flushTask;
                    }
                    catch
                    {
                    }

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 22 is logged upon successful transmission.
                    var traces = allTraces.Where(item => item.EventId == 22).ToList();
                    Assert.AreEqual(1, traces.Count);
                    // Event 26 is logged when items are moved to Storage.
                    traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    // Returns success, telemetry items are in storage as transmission. Control has transferred out of process. 
                    Assert.IsTrue(flushTask.Result);
                }
            }
        }

        [TestMethod]
        public async Task ChannelLogsSuccessfulTransmissionOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 22 is logged upon successful transmission.
                    var traces = allTraces.Where(item => item.EventId == 22).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccessDueToServerErrorOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Error from AI Backend.
                    ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await ctx.Response.WriteAsync("InternalError");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces
                    var allTraces = listener.Messages.ToList();

                    // Event 54 is logged upon transmission failure.
                    var traces = allTraces.Where(item => item.EventId == 54).ToList();
                    Assert.IsTrue(traces.Count > 0);
                    // 500 is the response code.
                    Assert.AreEqual("500", traces[0].Payload[1]);
                    // Event 26 is logged when items are moved to Storage.
                    traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    // Returns success, telemetry items are in storage as transmission. Control has transferred out of process. 
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelDropsTransmissionDueToBadRequestOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Error from AI Backend.
                    ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await ctx.Response.WriteAsync("Invalid instrumentation key");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces
                    var allTraces = listener.Messages.ToList();

                    // Event 71 is logged upon transmission failure.
                    var traces = allTraces.Where(item => item.EventId == 71).ToList();
                    Assert.IsTrue(traces.Count > 0);
                    // 400 is the response code.
                    Assert.AreEqual("400", traces[0].Payload[1]);
                    // Returns failure, non-whitelisted error code.
                    Assert.IsFalse(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccessDueToUnknownNetworkErrorOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = (ctx) =>
                {
                    // This code does not matter as Channel is configured to 
                    // with an incorrect endpoint.
                    return null;
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };

                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    // Event 54 is logged upon transmission failure.
                    var traces = allTraces.Where(item => item.EventId == 54).ToList();
                    Assert.IsTrue(traces.Count > 0);
                    Assert.AreEqual("500", traces[0].Payload[1].ToString());
                    // Event 26 is logged when items are moved to Storage.
                    traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    // Returns success, telemetry items are in storage as transmission. Control has transferred out of process. 
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelDropsTransmissionDueToResponseCodeTooManyRequestsOverExtendedTimeOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Error from AI Backend.
                    ctx.Response.StatusCode = (int)ResponseStatusCodes.ResponseCodeTooManyRequestsOverExtendedTime;
                    await ctx.Response.WriteAsync("ResponseCodeTooManyRequestsOverExtendedTime");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    // Event 54 is logged upon transmission failure.
                    var traces = allTraces.Where(item => item.EventId == 54).ToList();
                    Assert.AreEqual(1, traces.Count);
                    // 439 is the response code.
                    Assert.AreEqual("439", traces[0].Payload[1]);
                    Assert.IsFalse(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelLogsResponseBodyFromTransmissionWhenVerboseEnabledOnFlushAsync()
        {
            var expectedResponseContents = "this is the expected response";

            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    await ctx.Response.WriteAsync(expectedResponseContents);
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // Enable CoreEventSource as Transmission logic is in base sdk.
                    // and it'll parse response only on Verbose enabled.
                    using (var listenerCore = new TestEventListener())
                    {
                        listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways,
                            (EventKeywords)AllKeywords);


                        // ACT
                        var telemetry = new EventTelemetry("test event name");
                        telemetry.Context.InstrumentationKey = "dummy";
                        channel.Send(telemetry);
                        var flushResult = await channel.FlushAsync(default);
                        Assert.IsTrue(flushResult);
                        Thread.Sleep(SleepInMilliseconds);
                    }

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    // Event 70 is logged upon raw response content from backend.
                    var traces = allTraces.Where(item => item.EventId == 70).ToList();
                    Assert.AreEqual(1, traces.Count);
                    Assert.IsTrue(traces[0].Payload[1].ToString().Contains(expectedResponseContents));
                }
            }
        }

        [TestMethod]
        public async Task ChannelRespectsCancellationTokenTimeOutOnFlushAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    cancellationTokenSource.Cancel();
                    // Delay response from AI Backend.
                    await Task.Delay(DelayfromWebServerInMilliseconds);
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);

                    await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => channel.FlushAsync(cancellationTokenSource.Token));
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionMovedToStorageDueToMaxTransmissionSenderCapacityZeroOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl,
                    MaxTransmissionSenderCapacity = 0
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 26 is logged when items are moved to Storage.
                    var traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    // All items are moved to storage
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelDropsTransmissionDueToMaxTransmissionStorageCapacityZeroOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl,
                    MaxTransmissionSenderCapacity = 0,
                    MaxTransmissionStorageCapacity = 0
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 25 is logged when storage enqueue has no capacity.
                    var traces = allTraces.Where(item => item.EventId == 25).ToList();
                    Assert.IsTrue(traces.Count > 0);
                    // Returns failure as telemetry items did not store either in webserver or storage, failure is within the process. 
                    Assert.IsFalse(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionMovedtoStorageDueToMaxTransmissionBufferCapacityZeroOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl,
                    MaxTransmissionSenderCapacity = 0,
                    MaxTransmissionBufferCapacity = 0
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 26 is logged when items are moved to storage.
                    var traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelDropsTransmissionDueToAllCapacityZeroOnFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    // Success from AI Backend.
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl,
                    MaxTransmissionSenderCapacity = 0,
                    MaxTransmissionBufferCapacity = 0,
                    MaxTransmissionStorageCapacity = 0
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushResult = await channel.FlushAsync(default);
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 42 is logged when items are not stored in storage.
                    var traces = allTraces.Where(item => item.EventId == 42).ToList();
                    Assert.AreEqual(1, traces.Count);
                    // We lose telemetry.
                    Assert.IsFalse(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccesWithInFlightTransmissionOnFlushAsync()
        {
            bool isAsyncCall = false;
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    if (!isAsyncCall)
                    {
                        // Delay response from AI Backend.
                        await Task.Delay(DelayfromWebServerInMilliseconds);
                    }

                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    Parallel.ForEach(
                       new int[8],
                       new ParallelOptions
                       {
                           MaxDegreeOfParallelism = 8
                       },
                       (value) =>
                       {
                           var eventTelemetry = new EventTelemetry("test event name");
                           eventTelemetry.Context.InstrumentationKey = "dummy";
                           channel.Send(eventTelemetry);
                           channel.Flush();
                       });

                    await Task.Delay(1000);

                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 17 is logged when transmission start.
                    var traces = allTraces.Where(item => item.EventId == 17).ToList();
                    Assert.IsTrue(traces.Count >= 1);

                    isAsyncCall = true;
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);                    
                    var flushResult = await channel.FlushAsync(default);
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccessDueToMaxTransmissionSenderCapacityExceedWithFlushAsync()
        {
            bool isAsyncCall = false;
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    if (!isAsyncCall)
                    {
                        // Delay response from AI Backend.
                        await Task.Delay(DelayfromWebServerInMilliseconds);
                    }

                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    Parallel.ForEach(
                       new int[32],
                       new ParallelOptions
                       {
                           MaxDegreeOfParallelism = 32
                       },
                       (value) =>
                       {
                           var eventTelemetry = new EventTelemetry("test event name");
                           eventTelemetry.Context.InstrumentationKey = "dummy";
                           channel.Send(eventTelemetry);
                           channel.Flush();
                       });

                    await Task.Delay(1000);

                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 16 is logged when we exceed max capacity.
                    var traces = allTraces.Where(item => item.EventId == 16).ToList();
                    Assert.IsTrue(traces.Count > 0);                  

                    isAsyncCall = true;
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);                    
                    var flushResult = await channel.FlushAsync(default);
                    Assert.IsTrue(flushResult);

                    // We validate by checking SDK traces.
                    allTraces = listener.Messages.ToList();
                    // Event 26 is logged when items are moved to storage.
                    traces = allTraces.Where(item => item.EventId == 26).ToList();
                    // Transmission is moved to storage when Sender is out of capacity.
                    Assert.IsTrue(traces.Count >= 1);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccesDueToThrottleWithFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    channel.EnableLocalThrottling = true;
                    channel.LocalThrottleLimit = 100;
                    for (int i = 0; i < 200; i++)
                    {
                        var telemetry = new EventTelemetry("test event name");
                        telemetry.Context.InstrumentationKey = "dummy";
                        channel.Send(telemetry);
                    }

                    var flushResult = await channel.FlushAsync(default(CancellationToken));
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 26 is logged once items are moved to Storage.
                    var traces = allTraces.Where(item => item.EventId == 26).ToList();
                    Assert.IsTrue(traces.Count >= 1);
                    Assert.IsTrue(flushResult);
                }
            }
        }

        [TestMethod]
        public void ChannelTransmissionSuccessWithMultipleFlushAsync()
        {
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    Parallel.ForEach(
                       new int[8],
                       new ParallelOptions
                       {
                           MaxDegreeOfParallelism = 8
                       },
                       async (value) =>
                       {
                           var eventTelemetry = new EventTelemetry("test event name");
                           eventTelemetry.Context.InstrumentationKey = "dummy";
                           channel.Send(eventTelemetry);
                           var flushResult = await channel.FlushAsync(default);
                           Assert.IsTrue(flushResult);
                       });
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 17 is logged when transmission start.
                    var traces = allTraces.Where(item => item.EventId == 17).ToList();
                    Assert.IsTrue(traces.Count > 1);
                }
            }
        }

        [TestMethod]
        public async Task ChannelTransmissionSuccesWithFlushAndOnFlushAsync()
        {
            bool isAsyncCall = false;
            using (var localServer = new LocalInProcHttpServer(Localurl))
            {
                localServer.ServerLogic = async (ctx) =>
                {
                    if (!isAsyncCall)
                    {
                        // Delay response from AI Backend.
                        await Task.Delay(DelayfromWebServerInMilliseconds);
                    }

                    await ctx.Response.WriteAsync("Ok");
                };

                var channel = new ServerTelemetryChannel
                {
                    EndpointAddress = Localurl
                };
                var config = new TelemetryConfiguration("dummy")
                {
                    TelemetryChannel = channel
                };
                channel.Initialize(config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(TelemetryChannelEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    // ACT
                    Parallel.ForEach(
                       new int[4],
                       new ParallelOptions
                       {
                           MaxDegreeOfParallelism = 4
                       },
                       (value) =>
                       {
                           var eventTelemetry = new EventTelemetry("test event name");
                           eventTelemetry.Context.InstrumentationKey = "dummy";
                           channel.Send(eventTelemetry);
                           channel.Flush();
                       });

                    await Task.Delay(1000);

                    // We validate by checking SDK traces.
                    var allTraces = listener.Messages.ToList();
                    // Event 17 is logged when transmission start.
                    var traces = allTraces.Where(item => item.EventId == 17).ToList();
                    Assert.IsTrue(traces.Count >= 1);

                    isAsyncCall = true;
                    var telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushTask1 = channel.FlushAsync(default);

                    isAsyncCall = false;
                    // ACT
                    Parallel.ForEach(
                       new int[4],
                       new ParallelOptions
                       {
                           MaxDegreeOfParallelism = 4
                       },
                       (value) =>
                       {
                           var eventTelemetry = new EventTelemetry("test event name");
                           eventTelemetry.Context.InstrumentationKey = "dummy";
                           channel.Send(eventTelemetry);
                           channel.Flush();
                       });

                    await Task.Delay(1000);

                    // We validate by checking SDK traces.
                    allTraces = listener.Messages.ToList();
                    // Event 17 is logged when transmission start.
                    traces = allTraces.Where(item => item.EventId == 17).ToList();
                    Assert.IsTrue(traces.Count >= 1);

                    isAsyncCall = true;
                    telemetry = new EventTelemetry("test event name");
                    telemetry.Context.InstrumentationKey = "dummy";
                    channel.Send(telemetry);
                    var flushTask2 = channel.FlushAsync(default);

                    try
                    {
                        await flushTask1;
                        await flushTask2;
                    }
                    catch
                    {
                    }
                    Thread.Sleep(SleepInMilliseconds);

                    // VERIFY
                    // We validate by checking SDK traces.
                    allTraces = listener.Messages.ToList();
                    // Event 22 is logged upon successful transmission.
                    traces = allTraces.Where(item => item.EventId == 22).ToList();
                    Assert.IsTrue(traces.Count >= 3);
                    Assert.IsTrue(flushTask1.Result);
                    Assert.IsTrue(flushTask2.Result);
                }
            }
        }
    }
}
#endif