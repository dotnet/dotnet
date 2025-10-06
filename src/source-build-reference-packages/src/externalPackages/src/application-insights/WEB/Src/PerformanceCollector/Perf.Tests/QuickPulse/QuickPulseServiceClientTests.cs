namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class QuickPulseServiceClientTests : IDisposable
    {
        private const string ServiceEndpointPropertyName = "ServiceEndpoint";

        private static readonly TimeSpan RequestProcessingTimeout = TimeSpan.FromSeconds(30.0);

        private static Random rand = new Random();

        /// <summary>
        /// Tuple of (Timestamp, CollectionConfigurationETag, MonitoringDataPoint).
        /// </summary>
        private readonly List<Tuple<DateTimeOffset, string, MonitoringDataPoint>> samples =
            new List<Tuple<DateTimeOffset, string, MonitoringDataPoint>>();

        private readonly List<Tuple<PingHeaders, string, MonitoringDataPoint>> pings = new List<Tuple<PingHeaders, string, MonitoringDataPoint>>();

        private readonly CollectionConfiguration emptyCollectionConfiguration;

        private readonly Dictionary<string, string> opaqueAuthHeaderValuesToRespondWith = new Dictionary<string, string>(StringComparer.Ordinal);

        private readonly Dictionary<string, string> lastOpaqueAuthHeaderValues = new Dictionary<string, string>(StringComparer.Ordinal);

        private Action<HttpListenerResponse> pingResponse;

        private Action<HttpListenerResponse> submitResponse;

        private int pingCount;

        private int submitCount;

        private DateTimeOffset? lastPingTimestamp;

        private string lastPingInstance;

        private string lastPingRoleName;

        private string lastVersion;

        private string lastAuthApiKey;

        private bool emulateTimeout;

        public QuickPulseServiceClientTests()
        {
            CollectionConfigurationError[] errors;
            this.emptyCollectionConfiguration =
                new CollectionConfiguration(
                    new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                    out errors,
                    new ClockMock());
        }

        public TestContext TestContext { get; set; }

        private SemaphoreSlim AssertionSync
        {
            get { return this.TestContext.Properties[nameof(this.AssertionSync)] as SemaphoreSlim; }
            set { this.TestContext.Properties[nameof(this.AssertionSync)] = value; }
        }

        private HttpListener Listener
        {
            get { return this.TestContext.Properties[nameof(this.Listener)] as HttpListener; }
            set { this.TestContext.Properties[nameof(this.Listener)] = value; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.pingCount = 0;
            this.submitCount = 0;
            this.lastPingTimestamp = null;
            this.lastPingInstance = string.Empty;
            this.lastPingRoleName = string.Empty;
            this.lastVersion = string.Empty;
            this.lastAuthApiKey = string.Empty;
            ArrayHelpers.ForEach(QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames, headerName => this.lastOpaqueAuthHeaderValues.Add(headerName, null));
            ArrayHelpers.ForEach(QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames, headerName => this.opaqueAuthHeaderValuesToRespondWith.Add(headerName, null));
            this.samples.Clear();
            this.emulateTimeout = false;

            this.pingResponse = response =>
            {
                response.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());

                foreach (string headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
                {
                    response.Headers.Add(headerName, opaqueAuthHeaderValuesToRespondWith[headerName]);
                }
            };

            this.submitResponse = response =>
            {
                response.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());

                foreach (string headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
                {
                    response.Headers.Add(headerName, opaqueAuthHeaderValuesToRespondWith[headerName]);
                }
            };

            // dynamic port range is [49152, 65535]
            int port;
            lock (rand)
            {
                port = rand.Next(49152, 65536);
            }

            Uri serviceEndpoint = new Uri(string.Format(CultureInfo.InvariantCulture, "http://localhost:{0}", port));
            this.TestContext.Properties[ServiceEndpointPropertyName] = serviceEndpoint;

            this.Listener = new HttpListener();
            string uriPrefix = string.Format(CultureInfo.InvariantCulture, "http://localhost:{0}/", port);
            this.Listener.Prefixes.Add(uriPrefix);
            this.Listener.Start();
            this.AssertionSync = new SemaphoreSlim(0);

            var eventListenerReady = new AutoResetEvent(false);
            new Thread(() => this.ProcessRequest(this.Listener, eventListenerReady)).Start();

            eventListenerReady.WaitOne(QuickPulseServiceClientTests.RequestProcessingTimeout);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                this.Listener.Close();
            }
            catch
            {
            }
            finally
            {
                this.AssertionSync.Dispose();
                this.AssertionSync = null;
            }
        }

        [TestMethod]
        public void QuickPulseServiceClientPingsTheService()
        {
            // ARRANGE
            string instance = Guid.NewGuid().ToString();
            string roleName = Guid.NewGuid().ToString();
            var timestamp = DateTimeOffset.UtcNow;

            var serviceClient = new QuickPulseServiceClient(this.TestContext.Properties[ServiceEndpointPropertyName] as Uri, instance, roleName, instance, instance, string.Empty, new Clock(), false, 0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(string.Empty, timestamp, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);
            serviceClient.Ping(string.Empty, timestamp, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);
            serviceClient.Ping(string.Empty, timestamp, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 3);

            // ASSERT
            Assert.AreEqual(3, this.pingCount);
            Assert.AreEqual(timestamp.DateTime.ToFileTimeUtc() / 10000, this.lastPingTimestamp.Value.DateTime.ToFileTimeUtc() / 10000);
            Assert.AreEqual(instance, this.lastPingInstance);
            Assert.AreEqual(roleName, this.lastPingRoleName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsSamplesToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample1 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIRequestSuccessCount = 5,
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            var sample2 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIDependencyCallSuccessCount = 10,
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            var sample3 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIExceptionCount = 15,
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            bool? sendMore = serviceClient.SubmitSamples(
                new[] { sample1, sample2, sample3 },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(true, sendMore);
            Assert.AreEqual(3, this.samples.Count);
            Assert.AreEqual(5, this.samples[0].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Requests Succeeded/Sec").Value);
            Assert.AreEqual(10, this.samples[1].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Dependency Calls Succeeded/Sec").Value);
            Assert.AreEqual(15, this.samples[2].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Exceptions/Sec").Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientSetsTransmissionTimeCorrectly()
        {
            // ARRANGE
            var dummy = new Dictionary<string, Tuple<PerformanceCounterData, double>>();
            var dummy2 = Enumerable.Empty<Tuple<string, int>>();
            var timeProvider = new ClockMock();

            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                timeProvider,
                false,
                0);
            var sample1 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = timeProvider.UtcNow.AddSeconds(-1),
                        EndTimestamp = timeProvider.UtcNow
                    },
                    dummy,
                    dummy2,
                    false);

            timeProvider.FastForward(TimeSpan.FromSeconds(1));
            var sample2 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = timeProvider.UtcNow.AddSeconds(-1),
                        EndTimestamp = timeProvider.UtcNow
                    },
                    dummy,
                    dummy2,
                    false);

            timeProvider.FastForward(TimeSpan.FromSeconds(1));
            var sample3 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = timeProvider.UtcNow.AddSeconds(-1),
                        EndTimestamp = timeProvider.UtcNow
                    },
                    dummy,
                    dummy2,
                    false);

            // ACT
            timeProvider.FastForward(TimeSpan.FromSeconds(5));
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(
                new[] { sample1, sample2, sample3 },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(3, this.samples.Count);
            Assert.IsTrue((timeProvider.UtcNow - this.samples[0].Item1).Duration() < TimeSpan.FromMilliseconds(1));
            Assert.IsTrue((timeProvider.UtcNow - this.samples[1].Item1).Duration() < TimeSpan.FromMilliseconds(1));
            Assert.IsTrue((timeProvider.UtcNow - this.samples[2].Item1).Duration() < TimeSpan.FromMilliseconds(1));
            Assert.IsTrue(this.samples.All(s => (s.Item3.Timestamp - timeProvider.UtcNow).Duration() > TimeSpan.FromSeconds(1)));
        }

        [TestMethod]
        public void QuickPulseServiceClientRoundsSampleValuesWhenSubmittingToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample1 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIRequestSuccessCount = 1,
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(3)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample1 }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(0.3333, this.samples[0].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Requests Succeeded/Sec").Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientFillsInSampleWeightWhenSubmittingToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample1 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIRequestCountAndDurationInTicks = QuickPulseDataAccumulator.EncodeCountAndDuration(3, 10000),
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            var sample2 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        AIDependencyCallCountAndDurationInTicks = QuickPulseDataAccumulator.EncodeCountAndDuration(4, 10000),
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample1, sample2 }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(3, this.samples[0].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Request Duration").Weight);
            Assert.AreEqual(4, this.samples[1].Item3.Metrics.Single(m => m.Name == @"\ApplicationInsights\Dependency Call Duration").Weight);
        }

        [TestMethod]
        public void QuickPulseServiceClientFillsInTelemetryDocumentsWhenSubmittingToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var properties = new Dictionary<string, string>() { { "Prop1", "Val1" } };
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1),
                        TelemetryDocuments =
                            new ConcurrentStack<ITelemetryDocument>(
                                new ITelemetryDocument[]
                                {
                                    new RequestTelemetryDocument()
                                    {
                                        DocumentStreamIds = new[] { "Stream1" },
                                        Name = "Request1",
                                        Properties = properties.ToArray()
                                    },
                                    new DependencyTelemetryDocument()
                                    {
                                        DocumentStreamIds = new[] { "Stream1" },
                                        Name = "Dependency1",
                                        Properties = properties.ToArray()
                                    },
                                    new ExceptionTelemetryDocument()
                                    {
                                        DocumentStreamIds = new[] { "Stream1" },
                                        Exception = "Exception1",
                                        Properties = properties.ToArray()
                                    },
                                    new EventTelemetryDocument()
                                    {
                                        DocumentStreamIds = new[] { "Stream1" },
                                        Name = "Event1",
                                        Properties = properties.ToArray()
                                    },
                                    new TraceTelemetryDocument()
                                    {
                                        DocumentStreamIds = new[] { "Stream1" },
                                        Message = "Trace1",
                                        Properties = properties.ToArray()
                                    }
                                })
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual("Request1", ((RequestTelemetryDocument)this.samples[0].Item3.Documents[0]).Name);
            Assert.AreEqual("Prop1", ((RequestTelemetryDocument)this.samples[0].Item3.Documents[0]).Properties.First().Key);
            Assert.AreEqual("Val1", ((RequestTelemetryDocument)this.samples[0].Item3.Documents[0]).Properties.First().Value);

            Assert.AreEqual("Dependency1", ((DependencyTelemetryDocument)this.samples[0].Item3.Documents[1]).Name);
            Assert.AreEqual("Prop1", ((DependencyTelemetryDocument)this.samples[0].Item3.Documents[1]).Properties.First().Key);
            Assert.AreEqual("Val1", ((DependencyTelemetryDocument)this.samples[0].Item3.Documents[1]).Properties.First().Value);

            Assert.AreEqual("Exception1", ((ExceptionTelemetryDocument)this.samples[0].Item3.Documents[2]).Exception);
            Assert.AreEqual("Prop1", ((ExceptionTelemetryDocument)this.samples[0].Item3.Documents[2]).Properties.First().Key);
            Assert.AreEqual("Val1", ((ExceptionTelemetryDocument)this.samples[0].Item3.Documents[2]).Properties.First().Value);

            Assert.AreEqual("Event1", ((EventTelemetryDocument)this.samples[0].Item3.Documents[3]).Name);
            Assert.AreEqual("Prop1", ((EventTelemetryDocument)this.samples[0].Item3.Documents[3]).Properties.First().Key);
            Assert.AreEqual("Val1", ((EventTelemetryDocument)this.samples[0].Item3.Documents[3]).Properties.First().Value);

            Assert.AreEqual("Trace1", ((TraceTelemetryDocument)this.samples[0].Item3.Documents[4]).Message);
            Assert.AreEqual("Prop1", ((TraceTelemetryDocument)this.samples[0].Item3.Documents[4]).Properties.First().Key);
            Assert.AreEqual("Val1", ((TraceTelemetryDocument)this.samples[0].Item3.Documents[4]).Properties.First().Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientFillsInGlobalDocumentQuotaReachedWhenSubmittingToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample1 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1),
                        GlobalDocumentQuotaReached = true
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);
            var sample2 =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = now,
                        EndTimestamp = now.AddSeconds(1),
                        GlobalDocumentQuotaReached = false
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample1, sample2 }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsTrue(this.samples[0].Item3.GlobalDocumentQuotaReached);
            Assert.IsFalse(this.samples[1].Item3.GlobalDocumentQuotaReached);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsPingResponseCorrectlyWhenHeaderTrue()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.pingResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString()); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.Ping(string.Empty, DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsTrue(response.Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsPingResponseCorrectlyWhenHeaderFalse()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.pingResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString()); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.Ping(string.Empty, DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsFalse(response.Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsPingResponseCorrectlyWhenHeaderInvalid()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.pingResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, "bla"); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.Ping(string.Empty, DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(response);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsPingResponseCorrectlyWhenHeaderMissing()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.pingResponse = r => { };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.Ping(string.Empty, DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(response);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsSubmitSamplesResponseCorrectlyWhenHeaderTrue()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.submitResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString()); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.SubmitSamples(
                new QuickPulseDataSample[] { },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsTrue(response.Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsSubmitSamplesResponseCorrectlyWhenHeaderFalse()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.submitResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString()); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.SubmitSamples(
                new QuickPulseDataSample[] { },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsFalse(response.Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsSubmitSamplesResponseCorrectlyWhenHeaderInvalid()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.submitResponse = r => { r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, "bla"); };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.SubmitSamples(
                new QuickPulseDataSample[] { },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(response);
        }

        [TestMethod]
        public void QuickPulseServiceClientInterpretsSubmitSamplesResponseCorrectlyWhenHeaderMissing()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            this.submitResponse = r => { };
            CollectionConfigurationInfo configurationInfo;
            bool? response = serviceClient.SubmitSamples(
                new QuickPulseDataSample[] { },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(response);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotRetryPing()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0,
                TimeSpan.FromSeconds(5));

            this.emulateTimeout = false;

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(string.Empty, DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pingCount);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotRetrySubmitSamples()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0,
                TimeSpan.FromSeconds(5));

            this.emulateTimeout = false;

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(
                new QuickPulseDataSample[] { },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.submitCount);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotReadCollectionConfigurationFromPingWhenNotSubscribed()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo()
                {
                    ETag = "ETag2",
                    Metrics = new[] { new CalculatedMetricInfo() { Id = "Id1" } }
                };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("ikey", now, "ETag1", string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(configurationInfo);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotReadCollectionConfigurationFromPostWhenNotSubscribed()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(3) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            this.submitResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo()
                {
                    ETag = "ETag2",
                    Metrics = new[] { new CalculatedMetricInfo() { Id = "Id1" } }
                };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, "ETag1", string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(configurationInfo);
        }

        [TestMethod]
        public void QuickPulseServiceClientReadsCollectionConfigurationFromPingWhenETagIsDifferent()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo()
                {
                    ETag = "ETag2",
                    Metrics = new[] { new CalculatedMetricInfo() { Id = "Id1" } }
                };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("ikey", now, "ETag1", string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNotNull(configurationInfo, "configurationInfo should not be null.");
            Assert.AreEqual("ETag2", configurationInfo.ETag);
            Assert.AreEqual("Id1", configurationInfo.Metrics.Single().Id);
        }

        [TestMethod]
        public void QuickPulseServiceClientReadsCollectionConfigurationFromPostWhenETagIsDifferent()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(3) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            this.submitResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo()
                {
                    ETag = "ETag2",
                    Metrics = new[] { new CalculatedMetricInfo() { Id = "Id1" } }
                };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, "ETag1", string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNotNull(configurationInfo, "configurationInfo should not be null.");
            Assert.AreEqual("ETag2", configurationInfo.ETag);
            Assert.AreEqual("Id1", configurationInfo.Metrics.Single().Id);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotReadCollectionConfigurationFromPingWhenETagIsSame()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag2", Metrics = null };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("ikey", now, "ETag2", string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(configurationInfo);
        }

        [TestMethod]
        public void QuickPulseServiceClientDoesNotReadCollectionConfigurationFromPostWhenETagIsSame()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(3) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            this.submitResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, true.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsConfigurationETagHeaderName, "ETag2");

                var collectionConfigurationInfo = new CollectionConfigurationInfo() { ETag = "ETag2", Metrics = null };

                var serializer = new DataContractJsonSerializer(typeof(CollectionConfigurationInfo));
                serializer.WriteObject(r.OutputStream, collectionConfigurationInfo);
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, "ETag2", string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.IsNull(configurationInfo);
        }

        [TestMethod]
        public void QuickPulseServiceClientProducesCalculatedMetricsCorrectly()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            var metrics = new[]
            {
                new CalculatedMetricInfo()
                {
                    Id = "Metric1",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Avg,
                    FilterGroups = new FilterConjunctionGroupInfo[0]
                },
                new CalculatedMetricInfo()
                {
                    Id = "Metric2",
                    TelemetryType = TelemetryType.Request,
                    Projection = "Id",
                    Aggregation = AggregationType.Sum,
                    FilterGroups = new FilterConjunctionGroupInfo[0]
                }
            };

            CollectionConfigurationError[] errors;
            var collectionConfiguration = new CollectionConfiguration(
                new CollectionConfigurationInfo() { Metrics = metrics },
                out errors,
                new ClockMock());

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(collectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(3) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            var accumulator = sample.CollectionConfigurationAccumulator.MetricAccumulators["Metric1"];
            accumulator.AddValue(1.0d);
            accumulator.AddValue(2.0d);

            accumulator = sample.CollectionConfigurationAccumulator.MetricAccumulators["Metric2"];
            accumulator.AddValue(1.0d);
            accumulator.AddValue(2.0d);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, "ETag1", string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            MetricPoint metric1 = this.samples.Single().Item3.Metrics.Single(m => m.Name == "Metric1");
            MetricPoint metric2 = this.samples.Single().Item3.Metrics.Single(m => m.Name == "Metric2");

            Assert.AreEqual(1.5d, metric1.Value);
            Assert.AreEqual(2, metric1.Weight);
            Assert.AreEqual(3.0d, metric2.Value);
            Assert.AreEqual(2, metric2.Weight);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsRoleAndInstanceNameToServiceWithPing()
        {
            // ARRANGE
            var instanceName = "this instance";
            var roleName = "this role name";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                instanceName,
                roleName,
                string.Empty,
                instanceName,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pings.Count);
            Assert.AreEqual(instanceName, this.pings[0].Item1.InstanceName);
            Assert.AreEqual(instanceName, this.pings[0].Item3.Instance);
            Assert.AreEqual(roleName, this.pings[0].Item3.RoleName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsRoleNameWithNullValueValidation()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                null,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);


            // ASSERT
            this.WaitForProcessing(requestCount: 1);
            Assert.AreEqual(1, this.pings.Count);
            Assert.IsNull(this.pings[0].Item3.RoleName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsRoleAndInstanceNameToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var instanceName = "this instance";
            var roleName = "this role name";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                instanceName,
                roleName,
                instanceName,
                instanceName,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(instanceName, this.samples[0].Item3.Instance);
            Assert.AreEqual(roleName, this.samples[0].Item3.RoleName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsStreamIdAndRoleNameToServiceWithPing()
        {
            // ARRANGE
            var streamId = "this stream";
            var roleName = "Role-Name-1";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                roleName,
                streamId,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pings.Count);
            Assert.AreEqual(streamId, this.pings[0].Item1.StreamId);
            Assert.AreEqual(streamId, this.pings[0].Item3.StreamId);
            Assert.AreEqual(roleName, this.pings[0].Item1.RoleName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsStreamIdToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var streamId = "this stream";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                streamId,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(streamId, this.samples[0].Item3.StreamId);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsMachineNameToServiceWithPing()
        {
            // ARRANGE
            var machineName = "this machine";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                machineName,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pings.Count);
            Assert.AreEqual(machineName, this.pings[0].Item1.MachineName);
            Assert.AreEqual(machineName, this.pings[0].Item3.MachineName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsMachineNameToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var machineName = "this machine";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                machineName,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(machineName, this.samples[0].Item3.MachineName);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsInvariantVersionToServiceWithPing()
        {
            // ARRANGE
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), DateTimeOffset.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pings.Count);
            Assert.AreEqual(5, this.pings[0].Item1.InvariantVersion);
            Assert.AreEqual(5, this.pings[0].Item3.InvariantVersion);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsInvariantVersionToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(5, this.samples[0].Item3.InvariantVersion);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsTransmissionTimeToServiceWithPing()
        {
            // ARRANGE
            var timeProvider = new ClockMock();
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                timeProvider,
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping(Guid.NewGuid().ToString(), timeProvider.UtcNow, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pings.Count);
            Assert.AreEqual(timeProvider.UtcNow.Ticks, this.pings[0].Item1.TransmissionTime.Ticks);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsTransmissionTimeToServiceWithSubmitSamples()
        {
            // ARRANGE
            var timeProvider = new ClockMock();
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                timeProvider,
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration)
                    {
                        StartTimestamp = timeProvider.UtcNow,
                        EndTimestamp = timeProvider.UtcNow.AddSeconds(1)
                    },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(timeProvider.UtcNow.Ticks, this.samples[0].Item1.Ticks);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsVersionToServiceWithPing()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var version = "this version";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                version,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("some ikey", now, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pingCount);
            Assert.AreEqual(version, this.lastVersion);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsVersionToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var version = "this version";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                version,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(version, this.samples[0].Item3.Version);
            Assert.AreEqual(MonitoringDataPoint.CurrentInvariantVersion, this.samples[0].Item3.InvariantVersion);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsAuthApiKeyToServiceWithPing()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var authApiKey = "this api key";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("some ikey", now, string.Empty, authApiKey, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.pingCount);
            Assert.AreEqual(authApiKey, this.lastAuthApiKey);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsAuthApiKeyToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var authApiKey = "this api key";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, authApiKey, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(authApiKey, this.lastAuthApiKey);
        }

        [TestMethod]
        public void QuickPulseServiceClientResubmitsAuthOpaqueHeadersToServiceWithPing()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            foreach (var pair in this.opaqueAuthHeaderValuesToRespondWith.ToList())
            {
                this.opaqueAuthHeaderValuesToRespondWith[pair.Key] = pair.Key + "1";
            }

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.Ping("some ikey", now, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // received the proper headers, now re-submit them
            serviceClient.Ping("some ikey", now, string.Empty, string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 2);

            // ASSERT
            Assert.AreEqual(2, this.pingCount);
            Assert.AreEqual("x-ms-qps-auth-app-id1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-app-id"]);
            Assert.AreEqual("x-ms-qps-auth-status1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-status"]);
            Assert.AreEqual("x-ms-qps-auth-token-expiry1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-expiry"]);
            Assert.AreEqual("x-ms-qps-auth-token-signature1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-signature"]);
            Assert.AreEqual("x-ms-qps-auth-token-signature-alg1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-signature-alg"]);
        }

        [TestMethod]
        public void QuickPulseServiceClientResubmitsAuthOpaqueHeadersToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            foreach (var pair in this.opaqueAuthHeaderValuesToRespondWith.ToList())
            {
                this.opaqueAuthHeaderValuesToRespondWith[pair.Key] = pair.Key + "1";
            }

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // received the proper headers, now re-submit them
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 2);

            // ASSERT
            Assert.AreEqual(2, this.samples.Count);
            Assert.AreEqual("x-ms-qps-auth-app-id1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-app-id"]);
            Assert.AreEqual("x-ms-qps-auth-status1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-status"]);
            Assert.AreEqual("x-ms-qps-auth-token-expiry1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-expiry"]);
            Assert.AreEqual("x-ms-qps-auth-token-signature1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-signature"]);
            Assert.AreEqual("x-ms-qps-auth-token-signature-alg1", this.lastOpaqueAuthHeaderValues["x-ms-qps-auth-token-signature-alg"]);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsCollectionConfigurationETagToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, string.Empty, "ETag1", string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);
            serviceClient.Ping(string.Empty, now, "ETag1", string.Empty, null, out configurationInfo, out TimeSpan? _);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual("ETag1", this.samples.Single().Item2);
            Assert.AreEqual("ETag1", this.pings.Single().Item2);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsCollectionConfigurationErrorsToServiceWithSubmitSamples()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);
            CollectionConfigurationError[] collectionConfigurationErrors =
            {
                CollectionConfigurationError.CreateError(
                    CollectionConfigurationErrorType.MetricDuplicateIds,
                    "Error1",
                    new Exception("Exception1"),
                    Tuple.Create("Prop1", "Val1")),
                CollectionConfigurationError.CreateError(
                    CollectionConfigurationErrorType.DocumentStreamFailureToCreate,
                    "Error2",
                    new Exception("Exception2"),
                    Tuple.Create("Prop2", "Val2")),
            };

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(
                new[] { sample },
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                out configurationInfo,
                collectionConfigurationErrors);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(2, this.samples.Single().Item3.CollectionConfigurationErrors.Length);

            Assert.AreEqual(CollectionConfigurationErrorType.MetricDuplicateIds, this.samples.Single().Item3.CollectionConfigurationErrors[0].ErrorType);
            Assert.AreEqual("Error1", this.samples.Single().Item3.CollectionConfigurationErrors[0].Message);
            Assert.AreEqual(new Exception("Exception1").ToString(), this.samples.Single().Item3.CollectionConfigurationErrors[0].FullException);
            Assert.AreEqual("Prop1", this.samples.Single().Item3.CollectionConfigurationErrors[0].Data.Single().Key);
            Assert.AreEqual("Val1", this.samples.Single().Item3.CollectionConfigurationErrors[0].Data.Single().Value);

            Assert.AreEqual(CollectionConfigurationErrorType.DocumentStreamFailureToCreate, this.samples.Single().Item3.CollectionConfigurationErrors[1].ErrorType);
            Assert.AreEqual("Error2", this.samples.Single().Item3.CollectionConfigurationErrors[1].Message);
            Assert.AreEqual(new Exception("Exception2").ToString(), this.samples.Single().Item3.CollectionConfigurationErrors[1].FullException);
            Assert.AreEqual("Prop2", this.samples.Single().Item3.CollectionConfigurationErrors[1].Data.Single().Key);
            Assert.AreEqual("Val2", this.samples.Single().Item3.CollectionConfigurationErrors[1].Data.Single().Value);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsInstrumentationKeyToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var ikey = "some ikey";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, ikey, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(ikey, this.samples[0].Item3.InstrumentationKey);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsIsWebAppToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var ikey = "some ikey";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                true,
                0);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, ikey, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.IsTrue(this.samples[0].Item3.IsWebApp);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsProcessorCountToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var ikey = "some ikey";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                true,
                7);
            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    Enumerable.Empty<Tuple<string, int>>(),
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, ikey, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(7, this.samples[0].Item3.ProcessorCount);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsTopCpuProcessesToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var ikey = "some ikey";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                true,
                0);
            var cpuData = new[] { Tuple.Create("Process1", 1), Tuple.Create("Process2", 2) };

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    cpuData,
                    false);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, ikey, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.AreEqual(2, this.samples[0].Item3.TopCpuProcesses.Count());
            Assert.AreEqual("Process1", this.samples[0].Item3.TopCpuProcesses[0].ProcessName);
            Assert.AreEqual(1, this.samples[0].Item3.TopCpuProcesses[0].CpuPercentage);
            Assert.AreEqual("Process2", this.samples[0].Item3.TopCpuProcesses[1].ProcessName);
            Assert.AreEqual(2, this.samples[0].Item3.TopCpuProcesses[1].CpuPercentage);
        }

        [TestMethod]
        public void QuickPulseServiceClientSubmitsTopCpuProcessesAccessDeniedToService()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var ikey = "some ikey";
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                true,
                0);

            var sample =
                new QuickPulseDataSample(
                    new QuickPulseDataAccumulator(this.emptyCollectionConfiguration) { StartTimestamp = now, EndTimestamp = now.AddSeconds(1) },
                    new Dictionary<string, Tuple<PerformanceCounterData, double>>(),
                    new Tuple<string, int>[] { },
                    true);

            // ACT
            CollectionConfigurationInfo configurationInfo;
            serviceClient.SubmitSamples(new[] { sample }, ikey, string.Empty, string.Empty, null, out configurationInfo, new CollectionConfigurationError[0]);

            // SYNC
            this.WaitForProcessing(requestCount: 1);

            // ASSERT
            Assert.AreEqual(1, this.samples.Count);
            Assert.IsTrue(this.samples[0].Item3.TopCpuDataAccessDenied);
        }

        [TestMethod]
        public void QuickPulseServiceClientReturnsServicePollingIntervalHintWhenValid()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            int ms = 12345;
            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServicePollingIntervalHintHeaderName, ms.ToString());
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out TimeSpan? servicePollingIntervalHint);

            // ASSERT
            Assert.AreEqual(TimeSpan.FromMilliseconds(ms), servicePollingIntervalHint);
        }

        [TestMethod]
        public void QuickPulseServiceClientReturnsNullForServicePollingIntervalHintWhenNaN()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServicePollingIntervalHintHeaderName, "not a number");
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out TimeSpan? servicePollingIntervalHint);

            // ASSERT
            Assert.IsNull(servicePollingIntervalHint);
        }

        [TestMethod]
        public void QuickPulseServiceClientReturnsNullForServicePollingIntervalHintWhenEmpty()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServicePollingIntervalHintHeaderName, string.Empty);
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out TimeSpan? servicePollingIntervalHint);

            // ASSERT
            Assert.IsNull(servicePollingIntervalHint);
        }

        [TestMethod]
        public void QuickPulseServiceClientUpdatesServiceEndpointWhenRedirectValid()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            string endpointRedirect = "https://bing.com";
            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServiceEndpointRedirectHeaderName, endpointRedirect);
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out _);

            // SYNC
            this.WaitForProcessing(1);

            // ASSERT
            Assert.AreEqual(new Uri(endpointRedirect), serviceClient.CurrentServiceUri);
        }

        [TestMethod]
        public void QuickPulseServiceClientIgnoresServiceEndpointWhenRedirectInvalid()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            Uri initialEndpoint = serviceClient.CurrentServiceUri;

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServiceEndpointRedirectHeaderName, "/not-a-valid-absolute-url");
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out _);

            // SYNC
            this.WaitForProcessing(1);

            // ASSERT
            Assert.AreEqual(initialEndpoint, serviceClient.CurrentServiceUri);
        }

        [TestMethod]
        public void QuickPulseServiceClientIgnoresServiceEndpointWhenRedirectEmpty()
        {
            // ARRANGE
            var now = DateTimeOffset.UtcNow;
            var serviceClient = new QuickPulseServiceClient(
                this.TestContext.Properties[ServiceEndpointPropertyName] as Uri,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                new Clock(),
                false,
                0);

            Uri initialEndpoint = serviceClient.CurrentServiceUri;

            this.pingResponse = r =>
            {
                r.Headers.Add(QuickPulseConstants.XMsQpsSubscribedHeaderName, false.ToString());
                r.Headers.Add(QuickPulseConstants.XMsQpsServiceEndpointRedirectHeaderName, string.Empty);
            };

            // ACT
            serviceClient.Ping("ikey", now, string.Empty, string.Empty, null, out _, out _);

            // SYNC
            this.WaitForProcessing(1);

            // ASSERT
            Assert.AreEqual(initialEndpoint, serviceClient.CurrentServiceUri);
        }

        public void Dispose()
        {
            try
            {
                ((IDisposable)this.Listener).Dispose();
                if (this.AssertionSync != null)
                {
                    this.AssertionSync.Dispose();
                    this.AssertionSync = null;
                }
            }
            catch (Exception)
            {
            }
        }

        #region Helpers

        private void ProcessRequest(HttpListener listener, AutoResetEvent ev)
        {
            var serializerDataPoint = new DataContractJsonSerializer(typeof(MonitoringDataPoint));
            var serializerDataPointArray = new DataContractJsonSerializer(typeof(MonitoringDataPoint[]));

            while (listener?.IsListening ?? false)
            {
                try
                {
                    ev.Set();

                    HttpListenerContext context = listener.GetContext();

                    var request = context.Request;

                    this.lastAuthApiKey = context.Request.Headers[QuickPulseConstants.XMsQpsAuthApiKeyHeaderName];
                    foreach (var headerName in QuickPulseConstants.XMsQpsAuthOpaqueHeaderNames)
                    {
                        this.lastOpaqueAuthHeaderValues[headerName] = context.Request.Headers[headerName];
                    }

                    switch (request.Url.LocalPath)
                    {
                        case "/ping":
                            {
                                this.pingCount++;

                                this.pingResponse(context.Response);

                                var dataPoint =
                                    (MonitoringDataPoint)serializerDataPoint.ReadObject(context.Request.InputStream);
                                var transmissionTime = long.Parse(
                                    context.Request.Headers[QuickPulseConstants.XMsQpsTransmissionTimeHeaderName],
                                    CultureInfo.InvariantCulture);
                                var instanceName =
                                    context.Request.Headers[QuickPulseConstants.XMsQpsInstanceNameHeaderName];
                                var machineName = context.Request.Headers[QuickPulseConstants.XMsQpsMachineNameHeaderName];
                                var invariantVersion =
                                    context.Request.Headers[QuickPulseConstants.XMsQpsInvariantVersionHeaderName];
                                var streamId = context.Request.Headers[QuickPulseConstants.XMsQpsStreamIdHeaderName];
                                var roleName = context.Request.Headers[QuickPulseConstants.XMsQpsRoleNameHeaderName];
                                var collectionConfigurationETag =
                                    context.Request.Headers[QuickPulseConstants.XMsQpsConfigurationETagHeaderName];

                                this.pings.Add(
                                    Tuple.Create(
                                        new PingHeaders()
                                        {
                                            TransmissionTime = new DateTimeOffset(transmissionTime, TimeSpan.Zero),
                                            InstanceName = instanceName,
                                            MachineName = machineName,
                                            RoleName = roleName,
                                            InvariantVersion = int.Parse(invariantVersion, CultureInfo.InvariantCulture),
                                            StreamId = streamId
                                        },
                                        collectionConfigurationETag,
                                        dataPoint));

                                this.lastPingTimestamp = dataPoint.Timestamp;
                                this.lastPingInstance = dataPoint.Instance;
                                this.lastPingRoleName = dataPoint.RoleName;
                                this.lastVersion = dataPoint.Version;
                            }

                            break;
                        case "/post":
                            {
                                this.submitCount++;

                                this.submitResponse(context.Response);

                                var dataPoints =
                                    serializerDataPointArray.ReadObject(context.Request.InputStream) as MonitoringDataPoint
                                        [];
                                var transmissionTime = long.Parse(
                                    context.Request.Headers[QuickPulseConstants.XMsQpsTransmissionTimeHeaderName],
                                    CultureInfo.InvariantCulture);
                                var collectionConfigurationETag =
                                    context.Request.Headers[QuickPulseConstants.XMsQpsConfigurationETagHeaderName];

                                this.samples.AddRange(
                                    dataPoints.Select(
                                        dp => Tuple.Create(new DateTimeOffset(transmissionTime, TimeSpan.Zero),
                                            collectionConfigurationETag, dp)));
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unknown request: " + request.Url.LocalPath);
                    }

                    if (!this.emulateTimeout)
                    {
                        context.Response.Close();
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine("Exception occuring in ProcessRequest. " + e.ToString());
                }
                finally
                {
                    this.AssertionSync?.Release();
                }
            }
        }

        private void WaitForProcessing(int requestCount)
        {
            TimeSpan timeout = QuickPulseServiceClientTests.RequestProcessingTimeout;

            Task<bool>[] waitTasks = Enumerable.Range(0, requestCount).Select(_ => Task.Run(() => this.AssertionSync.Wait(timeout))).ToArray();

            Assert.IsTrue(
                condition: waitTasks.All(task => task.Result),
                message: string.Format(CultureInfo.InvariantCulture, "Not all requests finished processing: expected {0}, actual: {1}", requestCount, waitTasks.Count(task => task.Result)));
        }

        #endregion

        private class PingHeaders
        {
            public DateTimeOffset TransmissionTime { get; set; }

            public string InstanceName { get; set; }

            public string MachineName { get; set; }

            public int InvariantVersion { get; set; }

            public string StreamId { get; set; }

            public string RoleName { get; set; }
        }
    }
}
