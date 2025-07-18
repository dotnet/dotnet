// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Metrics;
using System.Net.Sockets;
using System.Net.Test.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.RemoteExecutor;
using Microsoft.DotNet.XUnitExtensions;
using Xunit;
using Xunit.Abstractions;

namespace System.Net.Http.Functional.Tests
{
    public abstract class DiagnosticsTestBase : HttpClientHandlerTestBase
    {
        protected DiagnosticsTestBase(ITestOutputHelper output) : base(output)
        {
        }

        protected static void VerifyTag<T>(IEnumerable<KeyValuePair<string, object?>> tags, string name, T value)
        {
            if (value is null)
            {
                Assert.DoesNotContain(tags, t => t.Key == name);
            }
            else
            {
                Assert.True(tags.Any(t => t.Key == name), $"Tag {name} not found in tags.");
                object? actualValue = tags.Single(t => t.Key == name).Value;
                Assert.Equal(value, (T)actualValue);
            }
        }


        protected static void VerifySchemeHostPortTags(IEnumerable<KeyValuePair<string, object?>> tags, Uri uri)
        {
            VerifyTag(tags, "url.scheme", uri.Scheme);
            VerifyTag(tags, "server.address", uri.Host);
            VerifyTag(tags, "server.port", uri.Port);
        }

        protected static string? GetVersionString(Version? version) => version == null ? null : version.Major switch
        {
            1 => "1.1",
            2 => "2",
            _ => "3"
        };
    }

    public abstract class HttpMetricsTestBase : DiagnosticsTestBase
    {
        protected static class InstrumentNames
        {
            public const string RequestDuration = "http.client.request.duration";
            public const string ActiveRequests = "http.client.active_requests";
            public const string OpenConnections = "http.client.open_connections";
            public const string IdleConnections = "http-client-current-idle-connections";
            public const string ConnectionDuration = "http.client.connection.duration";
            public const string TimeInQueue = "http.client.request.time_in_queue";
        }

        protected HttpMetricsTestBase(ITestOutputHelper output) : base(output)
        {
        }


        private static void VerifyPeerAddress(KeyValuePair<string, object?>[] tags, IPAddress[] validPeerAddresses = null)
        {
            string ipString = (string)tags.Single(t => t.Key == "network.peer.address").Value;
            validPeerAddresses ??= [IPAddress.Loopback.MapToIPv6(), IPAddress.Loopback, IPAddress.IPv6Loopback];
            IPAddress ip = IPAddress.Parse(ipString);
            Assert.Contains(ip, validPeerAddresses);
        }


        protected static void VerifyRequestDuration(Measurement<double> measurement,
            Uri uri,
            Version? protocolVersion = null,
            int? statusCode = null,
            string method = "GET",
            string[] acceptedErrorTypes = null) =>
            VerifyRequestDuration(InstrumentNames.RequestDuration, measurement.Value, measurement.Tags.ToArray(), uri, protocolVersion, statusCode, method, acceptedErrorTypes);

        protected static void VerifyRequestDuration(string instrumentName,
            double measurement,
            KeyValuePair<string, object?>[] tags,
            Uri uri,
            Version? protocolVersion,
            int? statusCode,
            string method = "GET",
            string[] acceptedErrorTypes = null)
        {
            Assert.Equal(InstrumentNames.RequestDuration, instrumentName);
            Assert.InRange(measurement, double.Epsilon, 60);
            VerifySchemeHostPortTags(tags, uri);
            VerifyTag(tags, "http.request.method", method);
            VerifyTag(tags, "network.protocol.version", GetVersionString(protocolVersion));
            VerifyTag(tags, "http.response.status_code", statusCode);
            if (acceptedErrorTypes == null)
            {
                Assert.DoesNotContain(tags, t => t.Key == "error.type");
            }
            else
            {
                string errorReason = (string)tags.Single(t => t.Key == "error.type").Value;
                Assert.Contains(errorReason, acceptedErrorTypes);
            }
        }

        protected static void VerifyActiveRequests(Measurement<long> measurement, long expectedValue, Uri uri, string method = "GET") =>
            VerifyActiveRequests(InstrumentNames.ActiveRequests, measurement.Value, measurement.Tags.ToArray(), expectedValue, uri, method);

        protected static void VerifyActiveRequests(string instrumentName, long measurement, KeyValuePair<string, object?>[] tags, long expectedValue, Uri uri, string method = "GET")
        {
            Assert.Equal(InstrumentNames.ActiveRequests, instrumentName);
            Assert.Equal(expectedValue, measurement);
            VerifySchemeHostPortTags(tags, uri);
            Assert.Equal(method, tags.Single(t => t.Key == "http.request.method").Value);
        }

        protected static void VerifyOpenConnections(string actualName, object measurement, KeyValuePair<string, object?>[] tags, long expectedValue, Uri uri, Version? protocolVersion, string state, IPAddress[] validPeerAddresses = null)
        {
            Assert.Equal(InstrumentNames.OpenConnections, actualName);
            Assert.Equal(expectedValue, Assert.IsType<long>(measurement));
            VerifySchemeHostPortTags(tags, uri);
            VerifyTag(tags, "network.protocol.version", GetVersionString(protocolVersion));
            VerifyTag(tags, "http.connection.state", state);
            VerifyPeerAddress(tags, validPeerAddresses);
        }

        protected static void VerifyConnectionDuration(string instrumentName, object measurement, KeyValuePair<string, object?>[] tags, Uri uri, Version? protocolVersion, IPAddress[] validPeerAddresses = null)
        {
            Assert.Equal(InstrumentNames.ConnectionDuration, instrumentName);
            double value = Assert.IsType<double>(measurement);

            // This flakes for remote requests on CI.
            if (validPeerAddresses is null)
            {
                Assert.InRange(value, double.Epsilon, 60);
            }
            VerifySchemeHostPortTags(tags, uri);
            VerifyTag(tags, "network.protocol.version", GetVersionString(protocolVersion));
            VerifyPeerAddress(tags, validPeerAddresses);
        }

        protected static void VerifyTimeInQueue(string instrumentName, object measurement, KeyValuePair<string, object?>[] tags, Uri uri, Version? protocolVersion, string method = "GET")
        {
            Assert.Equal(InstrumentNames.TimeInQueue, instrumentName);
            double value = Assert.IsType<double>(measurement);
            Assert.InRange(value, double.Epsilon, 60);
            VerifySchemeHostPortTags(tags, uri);
            VerifyTag(tags, "network.protocol.version", GetVersionString(protocolVersion));
            VerifyTag(tags, "http.request.method", method);
        }

        protected static async Task WaitForEnvironmentTicksToAdvance()
        {
            long start = Environment.TickCount64;
            while (Environment.TickCount64 == start)
            {
                await Task.Delay(1);
            }
        }

        protected sealed class InstrumentRecorder<T> : IDisposable where T : struct
        {
            private readonly MeterListener _meterListener = new();
            private readonly ConcurrentQueue<Measurement<T>> _values = new();
            private Meter? _meter;

            public Action? MeasurementRecorded;
            public Action<IReadOnlyList<T>> VerifyHistogramBucketBoundaries;
            public int MeasurementCount => _values.Count;

            public InstrumentRecorder(string instrumentName)
            {
                _meterListener.InstrumentPublished = (instrument, listener) =>
                {
                    if (instrument.Meter.Name == "System.Net.Http" && instrument.Name == instrumentName)
                    {
                        listener.EnableMeasurementEvents(instrument);
                    }
                };
                _meterListener.SetMeasurementEventCallback<T>(OnMeasurementRecorded);
                _meterListener.Start();
            }

            public InstrumentRecorder(IMeterFactory meterFactory, string instrumentName)
            {
                _meter = meterFactory.Create("System.Net.Http");
                _meterListener.InstrumentPublished = (instrument, listener) =>
                {
                    if (instrument.Meter == _meter && instrument.Name == instrumentName)
                    {
                        listener.EnableMeasurementEvents(instrument);
                    }
                };
                _meterListener.SetMeasurementEventCallback<T>(OnMeasurementRecorded);
                _meterListener.Start();
            }

            private void OnMeasurementRecorded(Instrument instrument, T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
            {
                _values.Enqueue(new Measurement<T>(measurement, tags));
                MeasurementRecorded?.Invoke();
                if (VerifyHistogramBucketBoundaries is not null)
                {
                    Histogram<T> histogram = (Histogram<T>)instrument;
                    IReadOnlyList<T> boundaries = histogram.Advice.HistogramBucketBoundaries;
                    Assert.NotNull(boundaries);
                    VerifyHistogramBucketBoundaries(boundaries);
                }
            }

            public IReadOnlyList<Measurement<T>> GetMeasurements() => _values.ToArray();
            public void Dispose() => _meterListener.Dispose();
        }

        protected record RecordedCounter(string InstrumentName, object Value, KeyValuePair<string, object?>[] Tags)
        {
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{InstrumentName}={Value} [");
                for (int i = 0; i < Tags.Length - 1; i++)
                {
                    sb.Append($"{Tags[i].Key}={Tags[i].Value}, ");
                }
                sb.Append($"{Tags.Last().Key}={Tags.Last().Value}]");
                return sb.ToString();
            }
        }

        protected sealed class MultiInstrumentRecorder : IDisposable
        {
            private readonly MeterListener _meterListener = new();
            private readonly ConcurrentQueue<RecordedCounter> _values = new();

            public MultiInstrumentRecorder()
                : this(meter: null)
            { }

            public MultiInstrumentRecorder(IMeterFactory meterFactory)
                : this(meterFactory.Create("System.Net.Http"))
            { }

            private MultiInstrumentRecorder(Meter? meter)
            {
                _meterListener.InstrumentPublished = (instrument, listener) =>
                {
                    if (instrument.Meter == meter || (meter is null && instrument.Meter.Name == "System.Net.Http"))
                    {
                        listener.EnableMeasurementEvents(instrument);
                    }
                };

                _meterListener.SetMeasurementEventCallback<long>((instrument, measurement, tags, _) =>
                    _values.Enqueue(new RecordedCounter(instrument.Name, measurement, tags.ToArray())));

                _meterListener.SetMeasurementEventCallback<double>((instrument, measurement, tags, _) =>
                    _values.Enqueue(new RecordedCounter(instrument.Name, measurement, tags.ToArray())));

                _meterListener.Start();
            }

            public IReadOnlyList<RecordedCounter> GetMeasurements() => _values.ToArray();
            public void Dispose() => _meterListener.Dispose();
        }
    }

    public abstract class HttpMetricsTest : HttpMetricsTestBase
    {
        public static readonly bool SupportsSeparateHttpSpansForRedirects = PlatformDetection.IsNotMobile && PlatformDetection.IsNotBrowser;
        private IMeterFactory _meterFactory = new TestMeterFactory();
        protected HttpClientHandler Handler { get; }
        protected virtual bool TestHttpMessageInvoker => false;
        public HttpMetricsTest(ITestOutputHelper output) : base(output)
        {
            Handler = CreateHttpClientHandler();
        }

        [Fact]
        public Task ActiveRequests_Success_Recorded()
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<long> recorder = SetupInstrumentRecorder<long>(InstrumentNames.ActiveRequests);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };

                HttpResponseMessage response = await SendAsync(client, request);
                response.Dispose(); // Make sure disposal doesn't interfere with recording by enforcing early disposal.

                Assert.Collection(recorder.GetMeasurements(),
                    m => VerifyActiveRequests(m, 1, uri),
                    m => VerifyActiveRequests(m, -1, uri));
            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });
        }

        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public async Task ActiveRequests_InstrumentEnabledAfterSending_NotRecorded()
        {
            if (UseVersion == HttpVersion.Version30)
            {
                return; // This test depends on ConnectCallback.
            }

            TaskCompletionSource connectionStarted = new TaskCompletionSource();

            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                GetUnderlyingSocketsHttpHandler(Handler).ConnectCallback = async (ctx, cancellationToken) =>
                {
                    connectionStarted.SetResult();

                    return await DefaultConnectCallback(ctx.DnsEndPoint, cancellationToken);
                };

                // Enable recording request-duration to test the path with metrics enabled.
                using InstrumentRecorder<double> unrelatedRecorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                Task<HttpResponseMessage> clientTask = Task.Run(() => SendAsync(client, request));
                await connectionStarted.Task;
                using InstrumentRecorder<long> recorder = new(Handler.MeterFactory, InstrumentNames.ActiveRequests);
                using HttpResponseMessage response = await clientTask;

                Assert.Empty(recorder.GetMeasurements());
            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsNotNodeJSOrFirefox))]
        [InlineData("GET", HttpStatusCode.OK)]
        [InlineData("PUT", HttpStatusCode.Created)]
        public Task RequestDuration_Success_Recorded(string method, HttpStatusCode statusCode)
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

                using HttpRequestMessage request = new(HttpMethod.Parse(method), uri) { Version = UseVersion };

                using HttpResponseMessage response = await SendAsync(client, request);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, (int)statusCode, method);

            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync(statusCode);
            });
        }

        [OuterLoop("Uses external server.")]
        [ConditionalFact]
        public async Task ExternalServer_DurationMetrics_Recorded()
        {
            if (UseVersion == HttpVersion.Version30)
            {
                throw new SkipTestException("No remote HTTP/3 server available for testing.");
            }

            using InstrumentRecorder<double> requestDurationRecorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
            using InstrumentRecorder<double> connectionDurationRecorder = SetupInstrumentRecorder<double>(InstrumentNames.ConnectionDuration);
            using InstrumentRecorder<long> openConnectionsRecorder = SetupInstrumentRecorder<long>(InstrumentNames.OpenConnections);

            Uri uri = UseVersion == HttpVersion.Version11
                ? Test.Common.Configuration.Http.RemoteHttp11Server.EchoUri
                : Test.Common.Configuration.Http.RemoteHttp2Server.EchoUri;
            IPAddress[] addresses = await Dns.GetHostAddressesAsync(uri.Host);
            addresses = addresses.Union(addresses.Select(a => a.MapToIPv6())).ToArray();

            using (HttpMessageInvoker client = CreateHttpMessageInvoker())
            {
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                request.Headers.ConnectionClose = true;
                using HttpResponseMessage response = await SendAsync(client, request);
                await response.Content.LoadIntoBufferAsync();
                await WaitForEnvironmentTicksToAdvance();
            }

            VerifyRequestDuration(Assert.Single(requestDurationRecorder.GetMeasurements()), uri, UseVersion, 200, "GET");
            Measurement<double> cd = Assert.Single(connectionDurationRecorder.GetMeasurements());
            VerifyConnectionDuration(InstrumentNames.ConnectionDuration, cd.Value, cd.Tags.ToArray(), uri, UseVersion, addresses);
            Measurement<long> oc = openConnectionsRecorder.GetMeasurements().First();
            VerifyOpenConnections(InstrumentNames.OpenConnections, oc.Value, oc.Tags.ToArray(), 1, uri, UseVersion, "idle", addresses);
        }

        [ConditionalFact(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        public async Task RequestDuration_HttpTracingEnabled_RecordedWhileRequestActivityRunning()
        {
            await RemoteExecutor.Invoke(static testClass =>
            {
                HttpMetricsTest test = (HttpMetricsTest)Activator.CreateInstance(Type.GetType(testClass), (ITestOutputHelper)null);

                return test.LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
                {
                    using HttpMessageInvoker client = test.CreateHttpMessageInvoker();
                    using InstrumentRecorder<double> recorder = test.SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

                    Activity? activity = null;
                    bool stopped = false;

                    ActivitySource.AddActivityListener(new ActivityListener
                    {
                        ShouldListenTo = s => s.Name is "System.Net.Http",
                        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
                        ActivityStarted = created => activity = created,
                        ActivityStopped = _ => stopped = true
                    });

                    recorder.MeasurementRecorded = () =>
                    {
                        Assert.NotNull(activity);
                        Assert.False(stopped);
                        Assert.Same(activity, Activity.Current);
                    };

                    using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = test.UseVersion };
                    using HttpResponseMessage response = await test.SendAsync(client, request);

                    Assert.NotNull(activity);

                    Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                    VerifyRequestDuration(m, uri, test.UseVersion, 200, "GET");

                }, async server =>
                {
                    await server.AcceptConnectionSendResponseAndCloseAsync();
                });
            }, GetType().FullName).DisposeAsync();
        }

        [Fact]
        public Task RequestDuration_CustomTags_Recorded()
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };

                HttpMetricsEnrichmentContext.AddCallback(request, static ctx =>
                {
                    ctx.AddCustomTag("route", "/test");
                });

                using HttpResponseMessage response = await SendAsync(client, request);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, 200);
                Assert.Equal("/test", m.Tags.ToArray().Single(t => t.Key == "route").Value);

            }, async server =>
            {
                await server.HandleRequestAsync();
            });
        }

        [Fact]
        public Task RequestDuration_MultipleCallbacksPerRequest_AllCalledInOrder()
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };

                int lastCallback = -1;

                HttpMetricsEnrichmentContext.AddCallback(request, ctx =>
                {
                    Assert.Equal(-1, lastCallback);
                    lastCallback = 1;
                    ctx.AddCustomTag("custom1", "foo");
                });
                HttpMetricsEnrichmentContext.AddCallback(request, ctx =>
                {
                    Assert.Equal(1, lastCallback);
                    lastCallback = 2;
                    ctx.AddCustomTag("custom2", "bar");
                });
                HttpMetricsEnrichmentContext.AddCallback(request, ctx =>
                {
                    Assert.Equal(2, lastCallback);
                    ctx.AddCustomTag("custom3", "baz");
                });

                using HttpResponseMessage response = await SendAsync(client, request);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, 200);
                Assert.Equal("foo", Assert.Single(m.Tags.ToArray(), t => t.Key == "custom1").Value);
                Assert.Equal("bar", Assert.Single(m.Tags.ToArray(), t => t.Key == "custom2").Value);
                Assert.Equal("baz", Assert.Single(m.Tags.ToArray(), t => t.Key == "custom3").Value);

            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });
        }

        [ConditionalTheory(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        [InlineData("System.Net.Http.HttpRequestOut.Start")]
        [InlineData("System.Net.Http.Request")]
        public async Task RequestDuration_CustomTags_DiagnosticListener_Recorded(string eventName)
        {
            await RemoteExecutor.Invoke(static async (testClassName, eventNameInner) =>
            {
                using HttpMetricsTest test = (HttpMetricsTest)Activator.CreateInstance(Type.GetType(testClassName), (ITestOutputHelper)null);
                await test.RequestDuration_CustomTags_DiagnosticListener_Recorded_Core(eventNameInner);
            }, GetType().FullName, eventName).DisposeAsync();
        }

        private async Task RequestDuration_CustomTags_DiagnosticListener_Recorded_Core(string eventName)
        {
            FakeDiagnosticListenerObserver diagnosticListenerObserver = new(kv =>
            {
                if (kv.Key == eventName)
                {
                    HttpRequestMessage request = GetProperty<HttpRequestMessage>(kv.Value, "Request");
                    HttpMetricsEnrichmentContext.AddCallback(request, static ctx =>
                    {
                        ctx.AddCustomTag("observed?", "observed!");
                        Assert.NotNull(ctx.Response);
                    });
                }
            });

            using IDisposable subscription = DiagnosticListener.AllListeners.Subscribe(diagnosticListenerObserver);

            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                diagnosticListenerObserver.Enable();
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                HttpMetricsEnrichmentContext.AddCallback(request, static ctx =>
                {
                    ctx.AddCustomTag("route", "/test");
                });

                using HttpResponseMessage response = await SendAsync(client, request);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, 200);
                Assert.Equal("/test", m.Tags.ToArray().Single(t => t.Key == "route").Value);
                Assert.Equal("observed!", m.Tags.ToArray().Single(t => t.Key == "observed?").Value);

            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });

            static T GetProperty<T>(object obj, string propertyName)
            {
                Type t = obj.GetType();

                PropertyInfo p = t.GetRuntimeProperty(propertyName);

                object propertyValue = p.GetValue(obj);
                Assert.NotNull(propertyValue);
                Assert.IsAssignableFrom<T>(propertyValue);

                return (T)propertyValue;
            }
        }

        public enum ResponseContentType
        {
            Empty,
            ContentLength,
            TransferEncodingChunked
        }

        [Theory]
        [InlineData(HttpCompletionOption.ResponseContentRead, ResponseContentType.Empty)]
        [InlineData(HttpCompletionOption.ResponseContentRead, ResponseContentType.ContentLength)]
        [InlineData(HttpCompletionOption.ResponseContentRead, ResponseContentType.TransferEncodingChunked)]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, ResponseContentType.Empty)]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, ResponseContentType.ContentLength)]
        [InlineData(HttpCompletionOption.ResponseHeadersRead, ResponseContentType.TransferEncodingChunked)]
        public async Task RequestDuration_EnrichmentHandler_Success_Recorded(HttpCompletionOption completionOption, ResponseContentType responseContentType)
        {
            if (TestHttpMessageInvoker)
            {
                // HttpCompletionOption not supported for HttpMessageInvoker, skipping.
                return;
            }

            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpClient client = CreateHttpClient(new EnrichmentHandler(Handler));
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                using HttpResponseMessage response = await client.SendAsync(TestAsync, request, completionOption);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (responseContentType == ResponseContentType.ContentLength)
                {
                    Assert.NotNull(response.Content.Headers.ContentLength);
                }
                else if (responseContentType == ResponseContentType.TransferEncodingChunked)
                {
                    Assert.NotNull(response.Headers.TransferEncodingChunked);
                }
                else
                {
                    // Empty
                    Assert.Empty(responseContent);
                }

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, 200); ;
                Assert.Equal("before!", m.Tags.ToArray().Single(t => t.Key == "before").Value);
            }, async server =>
            {
                if (responseContentType == ResponseContentType.ContentLength)
                {
                    string content = string.Join(' ', Enumerable.Range(0, 100));
                    int contentLength = Encoding.ASCII.GetByteCount(content);
                    await server.AcceptConnectionSendResponseAndCloseAsync(content: content, additionalHeaders: new[] { new HttpHeaderData("Content-Length", $"{contentLength}") });
                }
                else if (responseContentType == ResponseContentType.TransferEncodingChunked)
                {
                    string content = "3\r\nfoo\r\n3\r\nbar\r\n0\r\n\r\n";
                    await server.AcceptConnectionSendResponseAndCloseAsync(content: content, additionalHeaders: new[] { new HttpHeaderData("Transfer-Encoding", "chunked") });
                }
                else
                {
                    // Empty
                    await server.AcceptConnectionSendResponseAndCloseAsync();
                }
            });
        }

        private class CustomCredentials : ICredentials
        {
            public NetworkCredential? GetCredential(Uri uri, string authType) => null;
        }

        [ConditionalTheory(nameof(SupportsSeparateHttpSpansForRedirects))]
        [InlineData(0)] // null
        [InlineData(1)] // CredentialCache
        [InlineData(2)] // CustomCredentials
        public Task ActiveRequests_Redirect_RecordedForEachHttpSpan(int credentialsMode)
        {
            if (credentialsMode > 0)
            {
                Handler.Credentials = credentialsMode == 1 ? new CredentialCache() : new CustomCredentials();
            }

            return LoopbackServerFactory.CreateServerAsync((originalServer, originalUri) =>
            {
                return LoopbackServerFactory.CreateServerAsync(async (redirectServer, redirectUri) =>
                {
                    using HttpMessageInvoker client = CreateHttpMessageInvoker();
                    using InstrumentRecorder<long> recorder = SetupInstrumentRecorder<long>(InstrumentNames.ActiveRequests);
                    using HttpRequestMessage request = new(HttpMethod.Get, originalUri) { Version = UseVersion };

                    Task clientTask = SendAsync(client, request);
                    Task serverTask = originalServer.HandleRequestAsync(HttpStatusCode.Redirect, new[] { new HttpHeaderData("Location", redirectUri.AbsoluteUri) });

                    await Task.WhenAny(clientTask, serverTask);
                    Assert.False(clientTask.IsCompleted, $"{clientTask.Status}: {clientTask.Exception}");
                    await serverTask;

                    serverTask = redirectServer.HandleRequestAsync();
                    await TestHelper.WhenAllCompletedOrAnyFailed(clientTask, serverTask);
                    await clientTask;

                    Assert.Collection(recorder.GetMeasurements(),
                        m => VerifyActiveRequests(m, 1, originalUri),
                        m => VerifyActiveRequests(m, -1, originalUri),
                        m => VerifyActiveRequests(m, 1, redirectUri),
                        m => VerifyActiveRequests(m, -1, redirectUri));
                });
            });
        }

        public static TheoryData<string, string> MethodData = new TheoryData<string, string>()
        {
            { "GET", "GET" },
            { "get", "GET" },
            { "PUT", "PUT" },
            { "Put", "PUT" },
            { "POST", "POST" },
            { "pOst", "POST" },
            { "delete", "DELETE" },
            { "head", "HEAD" },
            { "options", "OPTIONS" },
            { "trace", "TRACE" },
            { "patch", "PATCH" },
            { "connect", "CONNECT" },
            { "g3t", "_OTHER" },
        };

        [Theory]
        [PlatformSpecific(~TestPlatforms.Browser)] // BrowserHttpHandler supports only a limited set of methods.
        [MemberData(nameof(MethodData))]
        public async Task RequestMetrics_EmitNormalizedMethodTags(string method, string expectedMethodTag)
        {
            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> requestDuration = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using InstrumentRecorder<long> activeRequests = SetupInstrumentRecorder<long>(InstrumentNames.ActiveRequests);
                using InstrumentRecorder<double> timeInQueue = SetupInstrumentRecorder<double>(InstrumentNames.TimeInQueue);

                using HttpRequestMessage request = new(new HttpMethod(method), uri) { Version = UseVersion };
                if (expectedMethodTag == "CONNECT")
                {
                    request.Headers.Host = "localhost";
                }

                using HttpResponseMessage response = await client.SendAsync(TestAsync, request);

                Assert.All(requestDuration.GetMeasurements(), m => VerifyTag(m.Tags.ToArray(), "http.request.method", expectedMethodTag));
                Assert.All(activeRequests.GetMeasurements(), m => VerifyTag(m.Tags.ToArray(), "http.request.method", expectedMethodTag));
                Assert.All(timeInQueue.GetMeasurements(), m => VerifyTag(m.Tags.ToArray(), "http.request.method", expectedMethodTag));
            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });
        }

        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public async Task AllSocketsHttpHandlerCounters_Success_Recorded()
        {
            TaskCompletionSource clientWaitingTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
            TaskCompletionSource clientDisposedTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using MultiInstrumentRecorder recorder = new(_meterFactory);

                using (HttpMessageInvoker invoker = CreateHttpMessageInvoker())
                {
                    Handler.MeterFactory = _meterFactory;

                    using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                    Task<HttpResponseMessage> sendAsyncTask = SendAsync(invoker, request);
                    clientWaitingTcs.SetResult();
                    using HttpResponseMessage response = await sendAsyncTask;

                    await WaitForEnvironmentTicksToAdvance();
                }

                clientDisposedTcs.SetResult();

                Action<RecordedCounter> requestsQueueDuration = m =>
                    VerifyTimeInQueue(m.InstrumentName, m.Value, m.Tags, uri, UseVersion);
                Action<RecordedCounter> connectionNoLongerIdle = m =>
                    VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, UseVersion, "idle");
                Action<RecordedCounter> connectionIsActive = m =>
                    VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, UseVersion, "active");

                Action<RecordedCounter> check1 = requestsQueueDuration;
                Action<RecordedCounter> check2 = connectionNoLongerIdle;
                Action<RecordedCounter> check3 = connectionIsActive;

                if (UseVersion.Major > 2)
                {
                    // With HTTP/3, the idle state change is emitted before RequestsQueueDuration.
                    check1 = connectionNoLongerIdle;
                    check2 = connectionIsActive;
                    check3 = requestsQueueDuration;
                }

                IReadOnlyList<RecordedCounter> measurements = recorder.GetMeasurements();
                foreach (RecordedCounter m in measurements)
                {
                    _output.WriteLine(m.ToString());
                }

                Assert.Collection(measurements,
                    m => VerifyActiveRequests(m.InstrumentName, (long)m.Value, m.Tags, 1, uri),
                    m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, UseVersion, "idle"),
                    check1, // requestsQueueDuration, connectionNoLongerIdle, connectionIsActive in the appropriate order.
                    check2,
                    check3,
                    m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, UseVersion, "active"),
                    m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, UseVersion, "idle"),

                    m => VerifyActiveRequests(m.InstrumentName, (long)m.Value, m.Tags, -1, uri),
                    m => VerifyRequestDuration(m.InstrumentName, (double)m.Value, m.Tags, uri, UseVersion, 200),
                    m => VerifyConnectionDuration(m.InstrumentName, m.Value, m.Tags, uri, UseVersion),
                    m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, UseVersion, "idle"));
            },
            async server =>
            {
                await clientWaitingTcs.Task.WaitAsync(TestHelper.PassingTestTimeout);

                await server.AcceptConnectionAsync(async connection =>
                {
                    await connection.ReadRequestDataAsync();
                    await connection.SendResponseAsync();
                    await clientDisposedTcs.Task.WaitAsync(TestHelper.PassingTestTimeout);
                });
            });
        }

        [Fact]
        public async Task RequestDuration_RequestCancelled_ErrorReasonIsExceptionType()
        {
            TaskCompletionSource clientCompleted = new(TaskCreationOptions.RunContinuationsAsynchronously);
            TaskCompletionSource requestReceived = new(TaskCreationOptions.RunContinuationsAsynchronously);

            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                using CancellationTokenSource requestCts = new();

                Task clientTask = SendAsync(client, request, requestCts.Token);

                await requestReceived.Task.WaitAsync(TestHelper.PassingTestTimeout);
                requestCts.Cancel();

                Exception clientException = await Assert.ThrowsAnyAsync<Exception>(() => clientTask);
                _output.WriteLine($"Client exception: {clientException}");

                string[] expectedExceptionTypes = TestAsync
                    ? [typeof(TaskCanceledException).FullName]
                    : [typeof(TaskCanceledException).FullName, typeof(OperationCanceledException).FullName];

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, acceptedErrorTypes: expectedExceptionTypes);

                clientCompleted.SetResult();
            },
            async server =>
            {
                await IgnoreExceptions(async () =>
                {
                    await server.AcceptConnectionAsync(async connection =>
                    {
                        await connection.ReadRequestDataAsync();
                        requestReceived.SetResult();
                        await clientCompleted.Task.WaitAsync(TestHelper.PassingTestTimeout);
                    });
                });
            });
        }

        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsNotBrowser))]
        public async Task RequestDuration_ConnectionError_LogsExpectedErrorReason()
        {
            if (UseVersion.Major == 3)
            {
                // HTTP/3 doesn't use the ConnectCallback that this test is relying on.
                return;
            }

            Uri uri = new("https://dummy:8080");

            using HttpMessageInvoker client = CreateHttpMessageInvoker();
            using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
            using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
            using CancellationTokenSource requestCts = new();

            GetUnderlyingSocketsHttpHandler(Handler).ConnectCallback = (_, _) => throw new Exception();

            Exception ex = await Assert.ThrowsAsync<HttpRequestException>(() => SendAsync(client, request));
            _output.WriteLine($"Client exception: {ex}");

            Measurement<double> m = Assert.Single(recorder.GetMeasurements());
            VerifyRequestDuration(m, uri, acceptedErrorTypes: ["connection_error"]);
        }

        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public Task TimeInQueue_RecordedForNewConnectionsOnly()
        {
            const int RequestCount = 3;

            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> timeInQueueRecorder = SetupInstrumentRecorder<double>(InstrumentNames.TimeInQueue);

                for (int i = 0; i < RequestCount; i++)
                {
                    using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                    using HttpResponseMessage response = await SendAsync(client, request);
                }

                // Only the first request is supposed to record time_in_queue.
                // For follow up requests, the connection should be immediately available.
                Assert.Equal(1, timeInQueueRecorder.MeasurementCount);

            }, async server =>
            {
                await server.AcceptConnectionAsync(async conn =>
                {
                    for (int i = 0; i < RequestCount; i++)
                    {
                        await conn.ReadRequestDataAsync();
                        await conn.SendResponseAsync(isFinal: true);
                        conn.CompleteRequestProcessing();
                    }
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Handler.Dispose();
                _meterFactory.Dispose();
            }

            base.Dispose(disposing);
        }

        protected Task<HttpResponseMessage> SendAsync(HttpMessageInvoker invoker, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            if (TestHttpMessageInvoker)
            {
                return TestAsync
                    ? invoker.SendAsync(request, cancellationToken)
                    : Task.Run(() => invoker.Send(request, cancellationToken));
            }

            return ((HttpClient)invoker).SendAsync(TestAsync, request, cancellationToken);
        }

        protected HttpMessageInvoker CreateHttpMessageInvoker(HttpMessageHandler? handler = null) =>
            TestHttpMessageInvoker ?
            new HttpMessageInvoker(handler ?? Handler) :
            CreateHttpClient(handler ?? Handler);

        protected InstrumentRecorder<T> SetupInstrumentRecorder<T>(string instrumentName)
            where T : struct
        {
            Handler.MeterFactory = _meterFactory;
            return new InstrumentRecorder<T>(_meterFactory, instrumentName);
        }

        protected sealed class EnrichmentHandler : DelegatingHandler
        {
            public EnrichmentHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpMetricsEnrichmentContext.AddCallback(request, Enrich);
                return base.Send(request, cancellationToken);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpMetricsEnrichmentContext.AddCallback(request, Enrich);
                return base.SendAsync(request, cancellationToken);
            }

            private static void Enrich(HttpMetricsEnrichmentContext context) => context.AddCustomTag("before", "before!");
        }
    }

    public abstract class HttpMetricsTest_Http11 : HttpMetricsTest
    {
        protected override Version UseVersion => HttpVersion.Version11;
        public HttpMetricsTest_Http11(ITestOutputHelper output) : base(output)
        {
        }

        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsNotNodeJSOrFirefox))]
        public async Task RequestDuration_EnrichmentHandler_ContentLengthError_Recorded()
        {
            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker(new EnrichmentHandler(Handler));
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };

                if (TestHttpMessageInvoker)
                {
                    using HttpResponseMessage response = await SendAsync(client, request);
                }
                else
                {
                    await Assert.ThrowsAsync<HttpRequestException>(async () =>
                    {
                        using HttpResponseMessage response = await SendAsync(client, request);
                    });
                }

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, 200);
                Assert.Equal("before!", m.Tags.ToArray().Single(t => t.Key == "before").Value);

            }, server => server.HandleRequestAsync(headers: new[] {
                new HttpHeaderData("Content-Length", "1000")
            }, content: "x"));
        }

        [Theory]
        [InlineData(400)]
        [InlineData(404)]
        [InlineData(599)]
        public Task RequestDuration_ErrorStatus_ErrorTypeRecorded(int statusCode)
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };

                using HttpResponseMessage response = await SendAsync(client, request);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, UseVersion, statusCode, "GET", acceptedErrorTypes: new[] { $"{statusCode}" });

            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync(statusCode: (HttpStatusCode)statusCode);
            });
        }

        [Fact]
        [SkipOnPlatform(TestPlatforms.Browser, "Browser is relaxed about validating HTTP headers")]
        public async Task RequestDuration_ConnectionClosedWhileReceivingHeaders_Recorded()
        {
            using CancellationTokenSource cancelServerCts = new CancellationTokenSource();
            await LoopbackServer.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Post, uri) { Version = UseVersion };
                request.Content = new StringContent("{}");

                Exception ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
                {
                    // To avoid unlimited blocking, lets bound it to 20 seconds.
                    using CancellationTokenSource cts = new CancellationTokenSource(20_000);
                    using HttpResponseMessage response = await SendAsync(client, request, cts.Token);
                });
                cancelServerCts.Cancel();
                Assert.True(ex is HttpRequestException or TaskCanceledException);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                VerifyRequestDuration(m, uri, acceptedErrorTypes: [typeof(TaskCanceledException).FullName, "response_ended"], method: "POST");
            }, async server =>
            {
                await IgnoreExceptions(async () =>
                {
                    LoopbackServer.Connection connection = await server.EstablishConnectionAsync().WaitAsync(cancelServerCts.Token);
                    connection.Socket.Shutdown(SocketShutdown.Send);
                });
            });
        }

        [Fact]
        public Task DurationHistograms_HaveBucketSizeHints()
        {
            return LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> requestDurationRecorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using InstrumentRecorder<double> timeInQueueRecorder = SetupInstrumentRecorder<double>(InstrumentNames.TimeInQueue);
                using InstrumentRecorder<double> connectionDurationRecorder = SetupInstrumentRecorder<double>(InstrumentNames.ConnectionDuration);

                requestDurationRecorder.VerifyHistogramBucketBoundaries = b =>
                {
                    // Verify first and last value of the boundaries defined in
                    // https://github.com/open-telemetry/semantic-conventions/blob/release/v1.23.x/docs/http/http-metrics.md#metric-httpserverrequestduration
                    Assert.Equal(0.005, b.First());
                    Assert.Equal(10, b.Last());
                };
                timeInQueueRecorder.VerifyHistogramBucketBoundaries = requestDurationRecorder.VerifyHistogramBucketBoundaries;
                connectionDurationRecorder.VerifyHistogramBucketBoundaries =
                    b => Assert.True(b.Last() > 180); // At least 3 minutes for the highest bucket.

                using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = UseVersion };
                using HttpResponseMessage response = await SendAsync(client, request);


                Assert.Equal(1, requestDurationRecorder.MeasurementCount);
                if (SocketsHttpHandler.IsSupported) Assert.Equal(1, timeInQueueRecorder.MeasurementCount);
                client.Dispose(); // terminate the connection

                if (SocketsHttpHandler.IsSupported) Assert.Equal(1, connectionDurationRecorder.MeasurementCount);
            }, async server =>
            {
                await server.AcceptConnectionSendResponseAndCloseAsync();
            });
        }
    }

    public class HttpMetricsTest_Http11_Async : HttpMetricsTest_Http11
    {
        public HttpMetricsTest_Http11_Async(ITestOutputHelper output) : base(output)
        {
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.SupportsAlpn))]
        [InlineData(false)]
        [InlineData(true)]
        public async Task RequestDuration_HttpVersionDowngrade_LogsActualProtocol(bool malformedResponse)
        {
            await LoopbackServer.CreateServerAsync(async server =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                using HttpRequestMessage request = new(HttpMethod.Get, server.Address)
                {
                    Version = HttpVersion.Version20,
                    VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
                };

                Task<HttpResponseMessage> clientTask = SendAsync(client, request);

                await server.AcceptConnectionAsync(async connection =>
                {
                    if (malformedResponse)
                    {
                        await connection.ReadRequestHeaderAndSendCustomResponseAsync("!malformed!");
                    }
                    else
                    {
                        await connection.ReadRequestHeaderAndSendResponseAsync();
                    }
                });

                if (malformedResponse)
                {
                    await Assert.ThrowsAsync<HttpRequestException>(() => clientTask);
                    Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                    VerifyRequestDuration(m, server.Address, acceptedErrorTypes: ["response_ended"]);
                }
                else
                {
                    using HttpResponseMessage response = await clientTask;

                    Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                    VerifyRequestDuration(m, server.Address, HttpVersion.Version11, 200);
                }

            }, new LoopbackServer.Options() { UseSsl = true });
        }
    }

    [ConditionalClass(typeof(PlatformDetection), nameof(PlatformDetection.IsNotMobile))]
    public class HttpMetricsTest_Http11_Async_HttpMessageInvoker : HttpMetricsTest_Http11_Async
    {
        protected override bool TestHttpMessageInvoker => true;
        public HttpMetricsTest_Http11_Async_HttpMessageInvoker(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task RequestDuration_RequestReused_EnrichmentCallbacksAreCleared()
        {
            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

                using HttpRequestMessage request = new(HttpMethod.Get, uri);

                int firstCallbackCalls = 0;

                HttpMetricsEnrichmentContext.AddCallback(request, ctx =>
                {
                    firstCallbackCalls++;
                    ctx.AddCustomTag("key1", "foo");
                });

                (await SendAsync(client, request)).Dispose();
                Assert.Equal(1, firstCallbackCalls);

                Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                Assert.Equal("key1", Assert.Single(m.Tags.ToArray(), t => t.Value as string == "foo").Key);

                HttpMetricsEnrichmentContext.AddCallback(request, static ctx =>
                {
                    ctx.AddCustomTag("key2", "foo");
                });

                (await SendAsync(client, request)).Dispose();
                Assert.Equal(1, firstCallbackCalls);

                Assert.Equal(2, recorder.GetMeasurements().Count);
                m = recorder.GetMeasurements()[1];
                Assert.Equal("key2", Assert.Single(m.Tags.ToArray(), t => t.Value as string == "foo").Key);
            }, async server =>
            {
                await server.HandleRequestAsync();
                await server.HandleRequestAsync();
            });
        }

        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsThreadingSupported))]
        public async Task RequestDuration_ConcurrentRequestsSeeDifferentContexts()
        {
            await LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
            {
                using HttpMessageInvoker client = CreateHttpMessageInvoker();
                using var _ = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

                using HttpRequestMessage request1 = new(HttpMethod.Get, uri);
                using HttpRequestMessage request2 = new(HttpMethod.Get, uri);

                HttpMetricsEnrichmentContext.AddCallback(request1, _ => { });
                (await client.SendAsync(request1, CancellationToken.None)).Dispose();

                HttpMetricsEnrichmentContext context1 = null;
                HttpMetricsEnrichmentContext context2 = null;
                CountdownEvent countdownEvent = new(2);

                HttpMetricsEnrichmentContext.AddCallback(request1, ctx =>
                {
                    context1 = ctx;
                    countdownEvent.Signal();
                    Assert.True(countdownEvent.Wait(TestHelper.PassingTestTimeout));
                });
                HttpMetricsEnrichmentContext.AddCallback(request2, ctx =>
                {
                    context2 = ctx;
                    countdownEvent.Signal();
                    Assert.True(countdownEvent.Wait(TestHelper.PassingTestTimeout));
                });

                Task<HttpResponseMessage> task1 = Task.Run(() => client.SendAsync(request1, CancellationToken.None));
                Task<HttpResponseMessage> task2 = Task.Run(() => client.SendAsync(request2, CancellationToken.None));

                (await task1).Dispose();
                (await task2).Dispose();

                Assert.NotSame(context1, context2);
            }, async server =>
            {
                await server.HandleRequestAsync();

                await Task.WhenAll(
                    server.HandleRequestAsync(),
                    server.HandleRequestAsync());
            }, options: new GenericLoopbackOptions { ListenBacklog = 2 });
        }
    }

    [ConditionalClass(typeof(PlatformDetection), nameof(PlatformDetection.IsNotMobile))]
    public class HttpMetricsTest_Http11_Sync : HttpMetricsTest_Http11
    {
        protected override bool TestAsync => false;
        public HttpMetricsTest_Http11_Sync(ITestOutputHelper output) : base(output)
        {
        }
    }

    [ConditionalClass(typeof(HttpMetricsTest_Http20), nameof(IsEnabled))]
    public class HttpMetricsTest_Http20 : HttpMetricsTest
    {
        public static bool IsEnabled = PlatformDetection.IsNotMobile && PlatformDetection.SupportsAlpn;
        protected override Version UseVersion => HttpVersion.Version20;
        public HttpMetricsTest_Http20(ITestOutputHelper output) : base(output)
        {
        }

        [ConditionalFact(nameof(SupportsSeparateHttpSpansForRedirects))]
        public Task RequestDuration_Redirect_RecordedForEachHttpSpan()
        {
            return GetFactoryForVersion(HttpVersion.Version11).CreateServerAsync((originalServer, originalUri) =>
            {
                return GetFactoryForVersion(HttpVersion.Version20).CreateServerAsync(async (redirectServer, redirectUri) =>
                {
                    using HttpMessageInvoker client = CreateHttpMessageInvoker();
                    using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);
                    using HttpRequestMessage request = new(HttpMethod.Get, originalUri) { Version = HttpVersion.Version20 };

                    Task clientTask = SendAsync(client, request);
                    Task serverTask = originalServer.HandleRequestAsync(HttpStatusCode.Redirect, new[] { new HttpHeaderData("Location", redirectUri.AbsoluteUri) });

                    await Task.WhenAny(clientTask, serverTask);
                    Assert.False(clientTask.IsCompleted, $"{clientTask.Status}: {clientTask.Exception}");
                    await serverTask;

                    serverTask = redirectServer.HandleRequestAsync();
                    await TestHelper.WhenAllCompletedOrAnyFailed(clientTask, serverTask);
                    await clientTask;

                    Assert.Collection(recorder.GetMeasurements(), m0 =>
                    {
                        VerifyRequestDuration(m0, originalUri, HttpVersion.Version11, (int)HttpStatusCode.Redirect);
                    }, m1 =>
                    {
                        VerifyRequestDuration(m1, redirectUri, HttpVersion.Version20, (int)HttpStatusCode.OK);
                    });

                }, options: new GenericLoopbackOptions() { UseSsl = true });
            }, options: new GenericLoopbackOptions() { UseSsl = false});
        }

        [Fact]
        public async Task RequestDuration_ProtocolError_Recorded()
        {
            using Http2LoopbackServer server = Http2LoopbackServer.CreateServer();
            using HttpMessageInvoker client = CreateHttpMessageInvoker();
            using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.RequestDuration);

            using HttpRequestMessage request = new(HttpMethod.Get, server.Address) { Version = HttpVersion.Version20 };
            Task<HttpResponseMessage> sendTask = SendAsync(client, request);

            Http2LoopbackConnection connection = await server.EstablishConnectionAsync();
            int streamId = await connection.ReadRequestHeaderAsync();

            // Send a reset stream frame so that the stream moves to a terminal state.
            RstStreamFrame resetStream = new RstStreamFrame(FrameFlags.None, (int)ProtocolErrors.INTERNAL_ERROR, streamId);
            await connection.WriteFrameAsync(resetStream);

            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                using HttpResponseMessage response = await sendTask;
            });

            Measurement<double> m = Assert.Single(recorder.GetMeasurements());
            VerifyRequestDuration(m, server.Address, acceptedErrorTypes: ["http_protocol_error"]);
        }
    }

    public class HttpMetricsTest_Http20_HttpMessageInvoker : HttpMetricsTest_Http20
    {
        protected override bool TestHttpMessageInvoker => true;
        public HttpMetricsTest_Http20_HttpMessageInvoker(ITestOutputHelper output) : base(output)
        {
        }
    }

    [ConditionalClass(typeof(HttpClientHandlerTestBase), nameof(IsHttp3Supported))]
    public class HttpMetricsTest_Http30 : HttpMetricsTest
    {
        protected override Version UseVersion => HttpVersion.Version30;
        public HttpMetricsTest_Http30(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task H3ConnectionFailure_TimeInQueueRecorded()
        {
            using Http3LoopbackServer server = CreateHttp3LoopbackServer(new Http3Options()
            {
                Alpn = "shall-not-work" // anything other than "h3"
            });

            using HttpMessageInvoker client = CreateHttpMessageInvoker();
            using InstrumentRecorder<double> recorder = SetupInstrumentRecorder<double>(InstrumentNames.TimeInQueue);
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, server.Address)
            {
                Version = HttpVersion30,
                VersionPolicy = HttpVersionPolicy.RequestVersionExact
            };
            await Assert.ThrowsAsync<HttpRequestException>(() => SendAsync(client, request));

            Assert.Equal(1, recorder.GetMeasurements().Count);
        }
    }

    public class HttpMetricsTest_Http30_HttpMessageInvoker : HttpMetricsTest_Http30
    {
        protected override bool TestHttpMessageInvoker => true;
        public HttpMetricsTest_Http30_HttpMessageInvoker(ITestOutputHelper output) : base(output)
        {
        }
    }

    // Make sure the instruments are working with the default Meter.
    public class HttpMetricsTest_DefaultMeter : HttpMetricsTestBase
    {
        public HttpMetricsTest_DefaultMeter(ITestOutputHelper output) : base(output)
        {
        }

        [ConditionalFact(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        public async Task ActiveRequests_Success_Recorded()
        {
            await RemoteExecutor.Invoke(static async Task () =>
            {
                using HttpMetricsTest_DefaultMeter test = new(null);
                await test.LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
                {
                    using HttpClient client = test.CreateHttpClient();
                    using InstrumentRecorder<long> recorder = new InstrumentRecorder<long>(InstrumentNames.ActiveRequests);
                    using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = test.UseVersion };

                    HttpResponseMessage response = await client.SendAsync(request);
                    response.Dispose(); // Make sure disposal doesn't interfere with recording by enforcing early disposal.

                    Assert.Collection(recorder.GetMeasurements(),
                        m => VerifyActiveRequests(m, 1, uri),
                        m => VerifyActiveRequests(m, -1, uri));
                }, async server =>
                {
                    await server.AcceptConnectionSendResponseAndCloseAsync();
                });
            }).DisposeAsync();
        }

        public static bool RemoteExecutorAndSocketsHttpHandlerSupported => RemoteExecutor.IsSupported && SocketsHttpHandler.IsSupported;

        [ConditionalFact(nameof(RemoteExecutorAndSocketsHttpHandlerSupported))]
        public async Task AllSocketsHttpHandlerCounters_Success_Recorded()
        {
            await RemoteExecutor.Invoke(static async Task () =>
            {
                TaskCompletionSource clientWaitingTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

                using HttpMetricsTest_DefaultMeter test = new(null);
                await test.LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
                {
                    using MultiInstrumentRecorder recorder = new();

                    using (HttpClient client = test.CreateHttpClient())
                    {
                        using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = test.UseVersion };
                        Task<HttpResponseMessage> sendAsyncTask = client.SendAsync(request);
                        clientWaitingTcs.SetResult();
                        using HttpResponseMessage response = await sendAsyncTask;

                        await WaitForEnvironmentTicksToAdvance();
                    }

                    Version version = HttpVersion.Version11;
                    Assert.Collection(recorder.GetMeasurements(),
                        m => VerifyActiveRequests(m.InstrumentName, (long)m.Value, m.Tags, 1, uri),
                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, version, "idle"),
                        m => VerifyTimeInQueue(m.InstrumentName, m.Value, m.Tags, uri, version),

                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, version, "idle"),
                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, version, "active"),
                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, version, "active"),
                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, 1, uri, version, "idle"),

                        m => VerifyActiveRequests(m.InstrumentName, (long)m.Value, m.Tags, -1, uri),
                        m => VerifyRequestDuration(m.InstrumentName, (double)m.Value, m.Tags, uri, version, 200),
                        m => VerifyConnectionDuration(m.InstrumentName, m.Value, m.Tags, uri, version),
                        m => VerifyOpenConnections(m.InstrumentName, m.Value, m.Tags, -1, uri, version, "idle"));
                },
                async server =>
                {
                    await clientWaitingTcs.Task.WaitAsync(TestHelper.PassingTestTimeout);

                    await server.AcceptConnectionAsync(async connection =>
                    {
                        await connection.ReadRequestDataAsync();
                        await connection.SendResponseAsync(isFinal: false);
                        await connection.WaitForCloseAsync(CancellationToken.None);
                    });
                });
            }).DisposeAsync();
        }

        [ConditionalFact(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        public async Task RequestDuration_Success_Recorded()
        {
            await RemoteExecutor.Invoke(static async Task () =>
            {
                using HttpMetricsTest_DefaultMeter test = new(null);
                await test.LoopbackServerFactory.CreateClientAndServerAsync(async uri =>
                {
                    using HttpClient client = test.CreateHttpClient();
                    using InstrumentRecorder<double> recorder = new InstrumentRecorder<double>(InstrumentNames.RequestDuration);
                    using HttpRequestMessage request = new(HttpMethod.Get, uri) { Version = test.UseVersion };

                    using HttpResponseMessage response = await client.SendAsync(request);
                    Measurement<double> m = Assert.Single(recorder.GetMeasurements());
                    VerifyRequestDuration(m, uri, HttpVersion.Version11, (int)HttpStatusCode.OK, "GET");
                }, async server =>
                {
                    await server.AcceptConnectionSendResponseAndCloseAsync(HttpStatusCode.OK);
                });
            }).DisposeAsync();
        }
    }

    public class HttpMetricsTest_General
    {
        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public void SocketsHttpHandler_Dispose_DoesNotDisposeMeterFactory()
        {
            using TestMeterFactory factory = new();
            SocketsHttpHandler handler = new()
            {
                MeterFactory = factory
            };
            handler.Dispose();
            Assert.False(factory.IsDisposed);
        }

        [Fact]
        public void HttpClientHandler_Dispose_DoesNotDisposeMeter()
        {
            using TestMeterFactory factory = new();
            HttpClientHandler handler = new()
            {
                MeterFactory = factory
            };
            handler.Dispose();
            Assert.False(factory.IsDisposed);
        }

        [Fact]
        public void HttpClientHandler_SetMeterFactoryAfterDispose_ThrowsObjectDisposedException()
        {
            HttpClientHandler handler = new();
            handler.Dispose();
            Assert.ThrowsAny<ObjectDisposedException>(() => handler.MeterFactory = new TestMeterFactory());
        }

        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public void SocketsHttpHandler_SetMeterFactoryAfterDispose_ThrowsObjectDisposedException()
        {
            SocketsHttpHandler handler = new();
            handler.Dispose();
            Assert.ThrowsAny<ObjectDisposedException>(() => handler.MeterFactory = new TestMeterFactory());
        }

        [Fact]
        public void HttpClientHandler_SetMeterFactoryAfterStart_ThrowsInvalidOperationException()
        {
            Http11LoopbackServerFactory.Singleton.CreateClientAndServerAsync(async uri =>
            {
                using HttpClientHandler handler = new();
                using HttpClient client = new HttpClient(handler);
                await client.GetAsync(uri);

                Assert.Throws<InvalidOperationException>(() => handler.MeterFactory = new TestMeterFactory());
            }, server => server.AcceptConnectionSendResponseAndCloseAsync());
        }

        [ConditionalFact(typeof(SocketsHttpHandler), nameof(SocketsHttpHandler.IsSupported))]
        public void SocketsHttpHandler_SetMeterFactoryAfterStart_ThrowsInvalidOperationException()
        {
            Http11LoopbackServerFactory.Singleton.CreateClientAndServerAsync(async uri =>
            {
                using SocketsHttpHandler handler = new();
                using HttpClient client = new HttpClient(handler);
                await client.GetAsync(uri);

                Assert.Throws<InvalidOperationException>(() => handler.MeterFactory = new TestMeterFactory());
            }, server => server.AcceptConnectionSendResponseAndCloseAsync());
        }
    }

    internal sealed class TestMeterFactory : IMeterFactory
    {
        private Meter? _meter;
        public bool IsDisposed => _meter is null;

        public TestMeterFactory() => _meter = new Meter("System.Net.Http", null, null, this);
        public Meter Create(MeterOptions options)
        {
            Assert.Equal("System.Net.Http", options.Name);
            Assert.Same(this, options.Scope);
            Assert.False(IsDisposed);

            return _meter;
        }
        public void Dispose()
        {
            _meter?.Dispose();
            _meter = null;
        }
    }

}
