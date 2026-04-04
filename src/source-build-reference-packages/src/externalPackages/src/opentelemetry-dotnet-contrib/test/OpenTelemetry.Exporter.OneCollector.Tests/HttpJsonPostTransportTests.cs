// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.IO.Compression;
using System.Net;
#if NETFRAMEWORK
using System.Net.Http;
#endif
using System.Text;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

public class HttpJsonPostTransportTests
{
    [Fact]
    public void RequestWithoutCompressionTest()
    {
        var request = "{\"key1\":\"value1\"}";

        using var httpClient = new HttpClient();

        RunHttpServerTest(
            request,
            requestUri =>
            {
                return new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.None,
                    new HttpClientWrapper(httpClient));
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: false);
                Assert.True(string.IsNullOrWhiteSpace(req.Headers["Content-Encoding"]));
                Assert.Equal(request, Encoding.ASCII.GetString(body.ToArray()));
            });
    }

    [Fact]
    public void RequestUsingDeflateCompressionTest()
    {
        var request = "{\"key1\":\"value1\"}";

        using var httpClient = new HttpClient();

        RunHttpServerTest(
            request,
            requestUri =>
            {
                return new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.Deflate,
                    new HttpClientWrapper(httpClient));
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: false);
                Assert.Equal("deflate", req.Headers["Content-Encoding"]);

                using var uncompressedStream = new MemoryStream();

                using (var compressionStream = new DeflateStream(body, CompressionMode.Decompress))
                {
                    compressionStream.CopyTo(uncompressedStream);
                }

                uncompressedStream.Position = 0;

                Assert.Equal(request, Encoding.ASCII.GetString(uncompressedStream.ToArray()));
            });
    }

    [Fact]
    public void RegisterPayloadTransmittedCallbackTest()
    {
        var request = "{\"key1\":\"value1\"}";

        using var httpClient = new HttpClient();

        var lastCompletedIteration = -1;
        IDisposable? callbackRegistration = null;
        var callbackFired = false;

        /*
         * This test runs a few different iterations...
         *
         * 0) Callback is attached and verified to fire.
         * 1) Exisiting callback fires again and is verified. Then we remove the callback.
         * 2) Verifies callback is NOT attached and NOT fired.
         * 3) Callback is attached again and verified to fire. Then we remove the callback.
         * 4) Tests the callback on a failed message with includeFailures: false.
         * 5) Tests the callback on a failed message with includeFailures: true.
         */

        RunHttpServerTest(
            request,
            requestUri =>
            {
                var transport = new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.None,
                    new HttpClientWrapper(httpClient));

                return transport;
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: false);
                Assert.True(string.IsNullOrWhiteSpace(req.Headers["Content-Encoding"]));
                Assert.Equal(request, Encoding.ASCII.GetString(body.ToArray()));
            },
            shouldTestFailFunc: (iteration) => iteration is 4 or 5,
            testStartingAction: (iteration, transport) =>
            {
                switch (iteration)
                {
                    case 0:
                    case 3:
                        Assert.Null(callbackRegistration);
                        callbackRegistration = transport.RegisterPayloadTransmittedCallback(OnPayloadTransmitted, includeFailures: false);
                        break;
                    case 1:
                        Assert.NotNull(callbackRegistration);
                        break;
                    case 2:
                        Assert.Null(callbackRegistration);
                        break;
                    case 4:
                        Assert.Null(callbackRegistration);
                        callbackRegistration = transport.RegisterPayloadTransmittedCallback(OnPayloadTransmitted, includeFailures: false);
                        break;
                    case 5:
                        Assert.Null(callbackRegistration);
                        callbackRegistration = transport.RegisterPayloadTransmittedCallback(OnPayloadTransmitted, includeFailures: true);
                        break;
                    default:
                        break;
                }
            },
            testFinishedAction: (iteration, transport) =>
            {
                switch (iteration)
                {
                    case 0:
                        Assert.NotNull(callbackRegistration);
                        Assert.True(callbackFired);
                        break;
                    case 1:
                    case 3:
                    case 4:
                    case 5:
                        Assert.NotNull(callbackRegistration);
                        if (iteration == 4)
                        {
                            Assert.False(callbackFired);
                        }
                        else
                        {
                            Assert.True(callbackFired);
                        }

                        callbackRegistration.Dispose();
                        callbackRegistration = null;
                        break;
                    case 2:
                        Assert.Null(callbackRegistration);
                        Assert.False(callbackFired);
                        break;
                    default:
                        break;
                }

                callbackFired = false;
                lastCompletedIteration = iteration;
            },
            testIterations: 6);

        Assert.Equal(5, lastCompletedIteration);

        void OnPayloadTransmitted(in OneCollectorExporterPayloadTransmittedCallbackArguments arguments)
        {
            callbackFired = true;

            using var stream = new MemoryStream();
            arguments.CopyPayloadToStream(stream);

            Assert.Equal(Encoding.ASCII.GetBytes(request), stream.ToArray());
            Assert.Equal(OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream, arguments.PayloadSerializationFormat);
            Assert.Equal(OneCollectorExporterTransportProtocolType.HttpJsonPost, arguments.TransportProtocol);
            Assert.NotNull(arguments.TransportEndpoint);
            if (lastCompletedIteration == 4)
            {
                Assert.False(arguments.Succeeded);
            }
            else
            {
                Assert.True(arguments.Succeeded);
            }
        }
    }

    [Fact]
    public void RegisterPayloadTransmittedCallbackConnectionFailureTest()
    {
        using var httpClient = new HttpClient();

        using var transport = new HttpJsonPostTransport(
            "instrumentation-key",
            new("http://localhost:0"),
            OneCollectorExporterHttpTransportCompressionType.Deflate,
            new HttpClientWrapper(httpClient));

        transport.RegisterPayloadTransmittedCallback(OnPayloadTransmitted, includeFailures: true);

        var callbackFired = false;

        var result = transport.Send(
            new TransportSendRequest
            {
                ItemStream = new MemoryStream(Encoding.ASCII.GetBytes("{\"key1\":\"value1\"}")),
                ItemSerializationFormat = OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream,
                ItemType = "TestRequest",
                NumberOfItems = 1,
            });

        Assert.False(result);

        Assert.True(callbackFired);

        void OnPayloadTransmitted(in OneCollectorExporterPayloadTransmittedCallbackArguments arguments)
        {
            callbackFired = true;
        }
    }

    [Fact]
    public void TransportDataSentEventFiredTest()
    {
        var request = "{}";

        using var httpClient = new HttpClient();

        RunHttpServerTest(
            request,
            requestUri =>
            {
                return new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.None,
                    new HttpClientWrapper(httpClient));
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: true);
            },
            enabledListener: true);
    }

    [Fact]
    public void TransportExceptionThrownEventFiredTest()
    {
        var request = "{}";

        using var httpClient = new HttpClient();

        RunHttpServerTest(
            request,
            requestUri =>
            {
                return new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.None,
                    new HttpClientWrapper(httpClient));
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: true);
            },
            enabledListener: true,
            transportFailure: true);
    }

    [Fact]
    public void HttpTransportErrorResponseReceivedEventFiredTest()
    {
        var request = "{}";

        using var httpClient = new HttpClient();

        RunHttpServerTest(
            request,
            requestUri =>
            {
                return new HttpJsonPostTransport(
                    "instrumentation-key",
                    requestUri,
                    OneCollectorExporterHttpTransportCompressionType.None,
                    new HttpClientWrapper(httpClient));
            },
            (req, body) =>
            {
                AssertStandardHeaders(req, listenerEnabled: true);
            },
            enabledListener: true,
            shouldTestFailFunc: i => true);
    }

    private static void AssertStandardHeaders(HttpListenerRequest request, bool listenerEnabled)
    {
        Assert.Equal("POST", request.HttpMethod);
        Assert.True(!string.IsNullOrWhiteSpace(request.Headers["User-Agent"]));
        Assert.True(!string.IsNullOrWhiteSpace(request.Headers["sdk-version"]));
        Assert.True(!string.IsNullOrWhiteSpace(request.Headers["x-apikey"]));
        Assert.Equal("application/x-json-stream; charset=utf-8", request.Headers["Content-Type"]);
        if (listenerEnabled)
        {
            Assert.Null(request.Headers["NoResponseBody"]);
        }
        else
        {
            Assert.Equal("true", request.Headers["NoResponseBody"]);
        }
    }

    private static void RunHttpServerTest(
        string requestBody,
        Func<Uri, ITransport> createTransportFunc,
        Action<HttpListenerRequest, MemoryStream> assertRequestAction,
        int numberOfItemsInRequestBody = 1,
        int testIterations = 1,
        Func<int, bool>? shouldTestFailFunc = null,
        Action<int, ITransport>? testStartingAction = null,
        Action<int, ITransport>? testFinishedAction = null,
        bool enabledListener = false,
        bool transportFailure = false)
    {
        using var eventListener = enabledListener
            ? new InMemoryEventListener(OneCollectorExporterEventSource.Log)
            : null;

        shouldTestFailFunc ??= static iteration => false;
        var failTest = false;
        var requestReceivedAndAsserted = false;
        Exception? testException = null;

        using var testServer = TestHttpServer.RunServer(
            context =>
            {
                context.Response.StatusCode = failTest ? 400 : 200;

                using var requestBody = new MemoryStream();

                context.Request.InputStream.CopyTo(requestBody);

                try
                {
                    requestBody.Position = 0;

                    assertRequestAction(context.Request, requestBody);

                    requestReceivedAndAsserted = true;
                }
                catch (Exception ex)
                {
                    testException = ex;
                }
                finally
                {
                    context.Response.OutputStream.Close();
                }
            },
            out var testServerHost,
            out var testServerPort);

        var transport = createTransportFunc(
            new Uri($"http://{testServerHost}:{(transportFailure ? 0 : testServerPort)}/"));

        try
        {
            var requestBodyBytes = Encoding.ASCII.GetBytes(requestBody);

            for (var i = 0; i < testIterations; i++)
            {
                failTest = shouldTestFailFunc(i);

                testStartingAction?.Invoke(i, transport);

                using var requestBodyStream = new MemoryStream(requestBodyBytes);

                var result = transport.Send(
                    new TransportSendRequest
                    {
                        ItemStream = requestBodyStream,
                        ItemSerializationFormat = OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream,
                        ItemType = "TestRequest",
                        NumberOfItems = numberOfItemsInRequestBody,
                    });

                if (testException != null)
                {
                    throw testException;
                }

                if (transportFailure)
                {
                    Assert.False(result);
                    Assert.False(requestReceivedAndAsserted);
                }
                else
                {
                    Assert.NotEqual(failTest, result);
                    Assert.True(requestReceivedAndAsserted);
                }

                testFinishedAction?.Invoke(i, transport);
            }
        }
        finally
        {
            (transport as IDisposable)?.Dispose();
        }

        if (eventListener != null)
        {
            if (failTest)
            {
                Assert.Contains(eventListener.Events, e => e.EventId == 6);
            }
            else if (transportFailure)
            {
                Assert.Contains(eventListener.Events, e => e.EventId == 5);
            }
            else
            {
                Assert.Contains(eventListener.Events, e => e.EventId == 2);
            }
        }
    }
}
