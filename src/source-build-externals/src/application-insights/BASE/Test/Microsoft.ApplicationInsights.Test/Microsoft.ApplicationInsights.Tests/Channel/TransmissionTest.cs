namespace Microsoft.ApplicationInsights.Channel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.CodeDom;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using System.Diagnostics.Tracing;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Runtime.Serialization.Json;

    public class TransmissionTest
    {
        [TestClass]
        public class Constructor
        {
            private readonly Uri testUri = new Uri("https://127.0.0.1/");

            [TestMethod]
            public void SetsEndpointAddressPropertyToSpecifiedValue()
            {
                var transmission = new Transmission(testUri, new byte[1], "content/type", "content/encoding");
                Assert.AreEqual(testUri, transmission.EndpointAddress);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenEndpointAddressIsNull() => new Transmission(null, new byte[1], "content/type", "content/encoding");

            [TestMethod]
            public void SetsContentPropertyToSpecifiedValue()
            {
                var expectedContent = new byte[42];
                var transmission = new Transmission(testUri, expectedContent, "content/type", "content/encoding");
                Assert.AreSame(expectedContent, transmission.Content);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenContentIsNull() => new Transmission(testUri, (byte[])null, "content/type", "content/encoding");

            [TestMethod]
            public void SetsContentTypePropertyToSpecifiedValue()
            {
                string expectedContentType = "TestContentType123";
                var transmission = new Transmission(testUri, new byte[1], expectedContentType, "content/encoding");
                Assert.AreSame(expectedContentType, transmission.ContentType);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionWhenContentTypeIsNull() => new Transmission(testUri, new byte[1], null, "content/encoding");

            [TestMethod]
            public void SetContentEncodingPropertyToSpecifiedValue()
            {
                string expectedContentEncoding = "gzip";
                var transmission = new Transmission(testUri, new byte[1], "any/content", expectedContentEncoding);
                Assert.AreSame(expectedContentEncoding, transmission.ContentEncoding);
            }

            [TestMethod]
            public void SetsTimeoutTo100SecondsByDefaultToMatchHttpWebRequest()
            {
                var transmission = new Transmission(testUri, new byte[1], "content/type", "content/encoding");
                Assert.AreEqual(TimeSpan.FromSeconds(100), transmission.Timeout);
            }

            [TestMethod]
            public void SetsTimeoutToSpecifiedValue()
            {
                var expectedValue = TimeSpan.FromSeconds(42);
                var transmission = new Transmission(testUri, new byte[1], "content/type", "content/encoding", expectedValue);
                Assert.AreEqual(expectedValue, transmission.Timeout);
            }

            [TestMethod]
            public void FlushAsyncIdGetsIncrementedOnEveryTransmission()
            {
                var transmission1 = new Transmission();
                var transmission2 = new Transmission();
                var transmission3 = new Transmission(testUri, new byte[1], "content/type", "content/encoding");

                Assert.AreEqual(transmission1.FlushAsyncId + 1, transmission2.FlushAsyncId);
                Assert.AreEqual(transmission1.FlushAsyncId + 2, transmission3.FlushAsyncId);
            }
        }

        [TestClass]
        [TestCategory("WindowsOnly")] // these tests are not reliable and block PRs
        public class SendAsync
        {
            private readonly Uri testUri = new Uri("https://127.0.0.1/");
            private const long AllKeywords = -1;

            [TestMethod]
            public async Task SendAsyncUsesPostMethodToSpecifiedHttpEndpoint()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        // VALIDATE
                        Assert.AreEqual(testUri, req.RequestUri);
                        Assert.AreEqual(HttpMethod.Post, req.Method);
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);
                    // transmission.Timeout = TimeSpan.FromMilliseconds(1);

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task SendAsyncUsesSpecifiedContentTypeAndEncoding()
            {
                var expectedContentType = "content/type";
                var expectedContentEncoding = "contentEncoding";
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        // VALIDATE
                        Assert.AreEqual(expectedContentType, req.Content.Headers.ContentType.MediaType);
                        Assert.AreEqual(expectedContentEncoding, req.Content.Headers.ContentEncoding.FirstOrDefault());

                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

                    // Instantiate Transmission with the mock HttpClient
                    var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, expectedContentType, expectedContentEncoding);

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task SendAsyncUsesEmptyContentTypeIfNoneSpecifiedInConstructor()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        // VALIDATE
                        Assert.IsNull(req.Content.Headers.ContentType);

                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    var items = new List<ITelemetry> { new EventTelemetry(), new EventTelemetry() };

                    // Instantiate Transmission with the mock HttpClient
                    var transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, "ContentEncoding");

                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task ThrowsInvalidOperationExceptionWhenTransmissionIsAlreadySending()
            {
                Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, new HttpClient(), string.Empty, string.Empty); FieldInfo isSendingField = typeof(Transmission).GetField("isSending", BindingFlags.NonPublic | BindingFlags.Instance);
                isSendingField.SetValue(transmission, 1);
                await AssertEx.ThrowsAsync<InvalidOperationException>(() => transmission.SendAsync());
            }

            [TestMethod]
            public async Task SendAsyncHandleResponseForPartialContentResponse()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.PartialContent;
                        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(5));
                        return Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    // VALIDATE
                    Assert.AreEqual(206, result.StatusCode);
                    Assert.AreEqual("5", result.RetryAfterHeader);

#if NET5_0_OR_GREATER
                    Assert.IsTrue(result.Content == string.Empty);
#else
                    Assert.IsNull(result.Content);
#endif
                }
            }

            [TestMethod]
            public async Task SendAsyncSendsContentPassedInConstructor()
            {
                var expectedContent = new byte[] {1, 2, 3, 4, 5};
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = async (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        byte[] actualContent = await req.Content.ReadAsByteArrayAsync();
                        AssertEx.AreEqual(expectedContent, actualContent);
                        return await Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, expectedContent, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task SendAsyncHandlesTimeout()
            {
                int clientTimeoutInMillisecs = 1;

                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        // Intentionally delay for atleast client timeout.
                        // By this time, client's cancellation token would definitely expire.
                        Task.Delay(100 * clientTimeoutInMillisecs).Wait();
                        // This simulates actual timeout
                        cancellationToken.ThrowIfCancellationRequested();
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient and Timeout to be just 1 msec to force Timeout.
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty,
                        string.Empty, TimeSpan.FromMilliseconds(clientTimeoutInMillisecs));

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    // VALIDATE
                    Assert.IsNotNull(result);
                    Assert.AreEqual((int) HttpStatusCode.RequestTimeout, result.StatusCode);
                    Assert.IsNull(result.Content, "Content is not to be read except in partial response (206) status.");
                }
            }

            [TestMethod]
            public async Task SendAsyncPropogatesHttpRequestException()
            {
                //ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        throw new HttpRequestException();
                    }
                };

                using (var fakeHttpClient = new HttpClient())
                {
                    // Instantiate Transmission with the mock HttpClient and Timeout to be just 1 msec to force Timeout.
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty,
                        string.Empty);

                    // ACT & VALIDATE
                    await AssertEx.ThrowsAsync<HttpRequestException>(() => transmission.SendAsync());
                }
            }

            [TestMethod]
            public async Task SendAsyncReturnsCorrectHttpResponseWrapperWhenNoExceptionOccurs()
            {
                // HttpClient.SendAsync throws HttpRequestException only on the following scenarios:
                // "The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout."
                // For every other case, a response is returned, and we expect Transmission.SendAsync to properly return HttpWebResponseWrapper.                

                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.ServiceUnavailable;
                        return Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    // VALIDATE
                    Assert.IsNotNull(result);
                    Assert.AreEqual(503, result.StatusCode);
                    Assert.IsNull(result.Content, "Content is not to be read except in partial response (206) status.");
                }

            }

            [TestMethod]
            public async Task SendAsyncReturnsCorrectHttpResponseWrapperWithRetryHeaderWhenNoExceptionOccur()
            {
                // HttpClient.SendAsync throws HttpRequestException only on the following scenarios:
                // "The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout."
                // For every other case, a response is returned, and we expect Transmission.SendAsync to properly return HttpWebResponseWrapper.                

                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.ServiceUnavailable;
                        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(5));
                        return Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();

                    // VALIDATE
                    Assert.IsNotNull(result);
                    Assert.AreEqual(503, result.StatusCode);
                    Assert.AreEqual("5", result.RetryAfterHeader);
                    Assert.IsNull(result.Content, "Content is not to be read except in partial response (206) status.");
                }

            }

#if NETCOREAPP
            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeEventCounter()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(30));
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    using (var listener = new EventCounterListener())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            HttpWebResponseWrapper result = await transmission.SendAsync();
                        }
                        //Sleep for few seconds as the event counter is sampled on a second basis
                        Thread.Sleep(TimeSpan.FromSeconds(3));

                        // VERIFY
                        // We validate by checking SDK traces.
                        var allTraces = listener.EventsReceived.ToList();
                        var traces = allTraces.Where(item => item.EventName == "EventCounters").ToList();
                        Assert.IsTrue(traces?.Count >= 1);
                        var payload = (IDictionary<string, object>)traces[0].Payload[0];
                        Assert.AreEqual("IngestionEndpoint-ResponseTimeMsec", payload["Name"].ToString());
                        Assert.IsTrue((int)payload["Count"] >= 5);
                        // Max should be more than 30 ms, as we introduced a delay of 30ms in SendAsync.
#if NETCOREAPP
                        Assert.IsTrue((double)payload["Max"] >= 30);
#endif
                    }
                }
            }

            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeOnFailureEventCounter()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(30));
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.ServiceUnavailable;
                        return Task.FromResult<HttpResponseMessage>(response);
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    using (var listener = new EventCounterListener())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            HttpWebResponseWrapper result = await transmission.SendAsync();
                        }
                        //Sleep for few seconds as the event counter is sampled on a second basis
                        Thread.Sleep(TimeSpan.FromSeconds(3));

                        // VERIFY
                        // We validate by checking SDK traces.
                        var allTraces = listener.EventsReceived.ToList();
                        var traces = allTraces.Where(item => item.EventName == "EventCounters").ToList();
                        Assert.IsTrue(traces?.Count >= 1);
                        var payload = (IDictionary<string, object>)traces[0].Payload[0];
                        Assert.AreEqual("IngestionEndpoint-ResponseTimeMsec", payload["Name"].ToString());
                        Assert.IsTrue((int)payload["Count"] >= 5);
                        // Mean should be more than 30 ms, as we introduced a delay of 30ms in SendAsync.
#if NETCOREAPP
                        Assert.IsTrue((double)payload["Mean"] >= 30);
#endif
                    }
                }
            }
#endif
            [TestMethod]
            public async Task SendAsyncLogsIngestionReponseTimeAndStatusCode()
            {
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    using (var listener = new TestEventListener())
                    {
                        var eventCounterArguments = new Dictionary<string, string>
                        {
                            {"EventCounterIntervalSec", "1"}
                        };

                        listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways, (EventKeywords)AllKeywords, eventCounterArguments);

                        HttpWebResponseWrapper result = await transmission.SendAsync();

                        // VERIFY
                        // We validate by checking SDK traces.
                        var allTraces = listener.Messages.ToList();
                        // Event 67 is logged after response from Ingestion Service.
                        var traces = allTraces.Where(item => item.EventId == 67).ToList();
                        Assert.AreEqual(1, traces.Count);
                    }
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventHandlerWithSuccessTransmission()
            {
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                    // VALIDATE
                    transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                    {
                        Assert.IsTrue(sender is Transmission);
                        Assert.AreEqual((int)HttpStatusCode.OK, args.Response.StatusCode);
                        Assert.AreNotEqual(0, args.ResponseDurationInMs);
                    };

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventHandlerWithKnownFailureTransmission()
            {
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);
                    transmission.Timeout = TimeSpan.Zero;

                    // VALIDATE
                    transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                    {
                        Assert.AreEqual((int)HttpStatusCode.RequestTimeout, args.Response.StatusCode);
                        Assert.AreEqual(0, args.ResponseDurationInMs);
                    };

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventHandlerWithUnKnownFailureTransmission()
            {
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        throw new Exception("test");
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);
                    transmission.Timeout = TimeSpan.Zero;

                    // VALIDATE
                    transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                    {
                        Assert.AreEqual(999, args.Response.StatusCode);
                        Assert.AreEqual(0, args.ResponseDurationInMs);
                    };

                    // ACT
                    try
                    {
                        HttpWebResponseWrapper result = await transmission.SendAsync();
                    }
                    catch (Exception ex)
                    {
                        Assert.AreEqual("test", ex.Message);
                    }
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventHandlerFails()
            {
                // ARRANGE
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
                    }
                };

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(CoreEventSource.Log, EventLevel.LogAlways,
                        (EventKeywords)AllKeywords);

                    using (var fakeHttpClient = new HttpClient(handler))
                    {
                        // Instantiate Transmission with the mock HttpClient                  
                        Transmission transmission = new Transmission(testUri, new byte[] { 1, 2, 3, 4, 5 }, fakeHttpClient, string.Empty, string.Empty);

                        // VALIDATE
                        transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                        {
                            throw new Exception("test");
                        };

                        // ACT
                        HttpWebResponseWrapper result = await transmission.SendAsync();
                    }

                    // Assert:
                    var allTraces = listener.Messages.ToList();
                    var traces = allTraces.Where(item => item.EventId == 71).ToList();
                    Assert.AreEqual(1, traces.Count);
                }
            }

            [TestMethod]
            public async Task TestTransmissionStatusEventWithEventsFromMultipleIKey()
            {
                // ARRANGE
                // Raw response from backend for partial response
                var ingestionResponse = "{" +
                    "\r\n  \"itemsReceived\": 5,\r\n  \"itemsAccepted\": 2,\r\n  " +
                    "\"errors\": [\r\n    {\r\n      " +
                    "\"index\": 0,\r\n      \"statusCode\": 400,\r\n      \"message\": \"Error 1\"\r\n    },\r\n    {\r\n      " +
                    "\"index\": 2,\r\n      \"statusCode\": 503,\r\n      \"message\": \"Error 2\"\r\n    },\r\n    {\r\n      " +
                    "\"index\": 3,\r\n      \"statusCode\": 500,\r\n      \"message\": \"Error 3\"\r\n    }\r\n  ]\r\n}";

                // Fake HttpClient will respond back with partial content
                var handler = new HandlerForFakeHttpClient
                {
                    InnerHandler = new HttpClientHandler(),
                    OnSendAsync = (req, cancellationToken) =>
                    {
                        return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage { StatusCode = HttpStatusCode.PartialContent, Content = new StringContent(ingestionResponse) });
                    }
                };

                using (var fakeHttpClient = new HttpClient(handler))
                {
                    // Create a list of telemetry which could send information to different instrumentation keys
                    var telemetryItems = new List<ITelemetry>();

                    EventTelemetry eventTelemetry1 = new EventTelemetry("Event1");
                    eventTelemetry1.Context.InstrumentationKey = "IKEY_1";
                    telemetryItems.Add(eventTelemetry1);

                    EventTelemetry eventTelemetry2 = new EventTelemetry("Event2");
                    eventTelemetry2.Context.InstrumentationKey = "IKEY_2";
                    telemetryItems.Add(eventTelemetry2);

                    EventTelemetry eventTelemetry3 = new EventTelemetry("Event3");
                    eventTelemetry3.Context.InstrumentationKey = "IKEY_3";
                    telemetryItems.Add(eventTelemetry3);

                    EventTelemetry eventTelemetry4 = new EventTelemetry("Event3");
                    eventTelemetry4.Context.InstrumentationKey = "IKEY_2";
                    telemetryItems.Add(eventTelemetry4);

                    EventTelemetry eventTelemetry5 = new EventTelemetry("Event5");
                    eventTelemetry5.Context.InstrumentationKey = "IKEY_1";
                    telemetryItems.Add(eventTelemetry5);

                    // Serialize the telemetry items before passing to transmission
                    var serializedData = JsonSerializer.Serialize(telemetryItems, true);

                    // Instantiate Transmission with the mock HttpClient                  
                    Transmission transmission = new Transmission(testUri, serializedData, fakeHttpClient, string.Empty, string.Empty);

                    // VALIDATE
                    transmission.TransmissionStatusEvent += delegate (object sender, TransmissionStatusEventArgs args)
                    {
                        var sendertransmission = sender as Transmission;
                        // convert raw JSON response to Backendresponse object
                        BackendResponse backendResponse = GetBackendResponse(args.Response.Content);

                        // Deserialize telemetry items to identify which items has failed
                        string[] items = JsonSerializer
                                            .Deserialize(sendertransmission.Content)
                                            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        string[] failedItems = new string[3];
                        int i = 0;

                        // Create a list of failed items
                        foreach (var error in backendResponse.Errors)
                        {
                            failedItems[i++] = items[error.Index];
                        }

                        Assert.AreEqual((int)HttpStatusCode.PartialContent, args.Response.StatusCode);
                        Assert.AreEqual(5, backendResponse.ItemsReceived);
                        Assert.AreEqual(2, backendResponse.ItemsAccepted);

                        //IKEY_1
                        int totalItemsForIkey = items.Where(x => x.Contains("IKEY_1")).Count();
                        int failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_1")).Count();
                        Assert.AreEqual(2, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);

                        //IKEY_2
                        totalItemsForIkey = items.Where(x => x.Contains("IKEY_2")).Count();
                        failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_2")).Count();
                        Assert.AreEqual(2, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);

                        //IKEY_3
                        totalItemsForIkey = items.Where(x => x.Contains("IKEY_3")).Count();
                        failedItemsForIkey = failedItems.Where(x => x.Contains("IKEY_3")).Count();
                        Assert.AreEqual(1, totalItemsForIkey);
                        Assert.AreEqual(1, failedItemsForIkey);
                    };

                    // ACT
                    HttpWebResponseWrapper result = await transmission.SendAsync();
                }
            }

            /// <summary>
            /// Serializes response from ingestion service to BackendResponse object.
            /// </summary>
            /// <param name="response">Response from ingestion service.</param>
            /// <returns></returns>
            private BackendResponse GetBackendResponse(string responseContent)
            {
                BackendResponse backendResponse = null;
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(BackendResponse));

                try
                {
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(responseContent)))
                        {
                            backendResponse = Serializer.ReadObject(ms) as BackendResponse;
                        }
                    }
                }
                catch
                {
                    backendResponse = null;
                }

                return backendResponse;
            }
        }
    }

    /// <summary>
    /// DataContract class to hold response from ingestion service.
    /// </summary>
    [DataContract]
    internal class BackendResponse
    {
        [DataMember(Name = "itemsReceived", IsRequired = true)]
        public int ItemsReceived { get; set; }

        [DataMember(Name = "itemsAccepted", IsRequired = true)]
        public int ItemsAccepted { get; set; }

        [DataMember(Name = "errors")]
        public Error[] Errors { get; set; }

        [DataContract]
        internal class Error
        {
            [DataMember(Name = "index")]
            public int Index { get; set; }

            [DataMember(Name = "statusCode")]
            public int StatusCode { get; set; }

            [DataMember(Name = "message")]
            public string Message { get; set; }
        }
    }

    /// <summary>
    /// Handler to control the behaviour of HttpClient. HttpClient instance created with this
    /// Unit tests provide the behaviour of this handler.
    /// </summary>
    internal class HandlerForFakeHttpClient : DelegatingHandler
    {
        public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> OnSendAsync;
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await OnSendAsync(request, cancellationToken);
        }
    }
}