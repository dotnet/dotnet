namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AutocollectedMetricsExtractorTest
    {
        #region General Tests

        [TestMethod]
        public void CanConstruct()
        {
            var extractor = new AutocollectedMetricsExtractor(null);
        }

        [TestMethod]
        public void DoesNotProcessSampledItems()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                // track items which has sampling percentage set to simulate they are seen by SamplingProcessor.
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                var req = new RequestTelemetry("Test Request 1", DateTimeOffset.Now, TimeSpan.FromMilliseconds(10), "200", success: true);
                (req as ISupportSampling).SamplingPercentage = 1;
                var dep = new DependencyTelemetry("Type", "Target", "depName1", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(1000), "ResultCode100", true);
                (dep as ISupportSampling).SamplingPercentage = 1;
                var exp = new ExceptionTelemetry(new ArgumentException("Test"));
                (exp as ISupportSampling).SamplingPercentage = 1;
                var trace = new TraceTelemetry("Test", SeverityLevel.Error);
                (trace as ISupportSampling).SamplingPercentage = 1;

                client.TrackRequest(req);
                client.TrackDependency(dep);
                client.TrackException(exp);
                client.TrackTrace(trace);
            }

            Assert.AreEqual(4, telemetrySentToChannel.Count);

            // Validate that Preaggregator does not process items which are sampled.
            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual(false, ((RequestTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual(false,
                         ((RequestTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));

            AssertEx.IsType<DependencyTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual(false, ((DependencyTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual(false,
                         ((DependencyTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual(false, ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual(false,
                         ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[3]);
            Assert.AreEqual(false, ((TraceTelemetry)telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual(false,
                         ((TraceTelemetry)telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));

        }

        [TestMethod]
        public void DisposeIsIdempotent()
        {
            AutocollectedMetricsExtractor extractor = null;

            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                    (nextProc) =>
                    {
                        extractor = new AutocollectedMetricsExtractor(nextProc);
                        return extractor;
                    };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                ;
            }

            extractor.Dispose();
            extractor.Dispose();
        }

        #endregion General Tests

        #region Request-metrics-related Tests

        [TestMethod]
        public void Request_TelemetryMarkedAsProcessedCorrectly()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                client.TrackEvent("Test Event");
                client.TrackRequest("Test Request 1", DateTimeOffset.Now, TimeSpan.FromMilliseconds(10), "200", success: true);
                client.TrackRequest("Test Request 2", DateTimeOffset.Now, TimeSpan.FromMilliseconds(11), "200", success: true);
            }

            Assert.AreEqual(4, telemetrySentToChannel.Count);

        }

        [TestMethod]
        public void Request_TelemetryRespectsDimLimitResponseCode()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = 
                (nextProc) => { 
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxRequestResponseCodeValuesToDiscover = 0;
                    return metricExtractor; 
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 3 requests with 3 different values for Result code - 200,201,202.
                // As MaxRequestResponseCodeValuesToDiscover = 0, we expect all responde code to be rolled into Other
                client.TrackRequest("Test Request 1", DateTimeOffset.Now, TimeSpan.FromMilliseconds(10), "200", success: true);
                client.TrackRequest("Test Request 2", DateTimeOffset.Now, TimeSpan.FromMilliseconds(11), "201", success: true);
                client.TrackRequest("Test Request 3", DateTimeOffset.Now, TimeSpan.FromMilliseconds(11), "202", success: true);
            }

            Assert.AreEqual(4, telemetrySentToChannel.Count);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test Request 1", ((RequestTelemetry)telemetrySentToChannel[0]).Name);
            Assert.AreEqual(true, ((RequestTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Test Request 2", ((RequestTelemetry) telemetrySentToChannel[1]).Name);
            Assert.AreEqual(true, ((RequestTelemetry) telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry) telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Test Request 3", ((RequestTelemetry) telemetrySentToChannel[2]).Name);
            Assert.AreEqual(true, ((RequestTelemetry) telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry) telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[3]);
            var metricTel = telemetrySentToChannel[3] as MetricTelemetry;
            // validate standard fields
            Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
            Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
            Assert.AreEqual("requests/duration", metricTel.Context.GlobalProperties["_MS.MetricId"]);

            // validate dimensions exist
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("Request.Success"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/resultCode"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/performanceBucket"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));

            var resultCodeDimension = metricTel.Properties["request/resultCode"];
            // As MaxRequestResponseCodeValuesToDiscover = 0, we expect all responde code to be rolled into Other
            Assert.AreEqual("Other", resultCodeDimension);
        }

        [TestMethod]
        public void Request_TelemetryRespectsDimLimitCloudRoleInstance()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxRequestCloudRoleInstanceValuesToDiscover = 2;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 4 requests with 3 different values for RoleInstance - A B C D.
                // As MaxRequestCloudRoleInstanceValuesToDiscover = 2, the first 2 values encountered (A,B) 
                // will be used as such at which the DimensionCap is hit.
                // Newly incoming values (C,D) will be rolled into "DIMENSION-CAPPED"

                client.TrackRequest(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), "200", true, false, "RoleNameA", "RoleInstanceA"));
                client.TrackRequest(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), "200", true, false, "RoleNameA", "RoleInstanceB"));
                client.TrackRequest(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), "200", true, false, "RoleNameA", "RoleInstanceC"));
                client.TrackRequest(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), "200", true, false, "RoleNameA", "RoleInstanceD"));
            }

            // 4 requests + 3 metric
            Assert.AreEqual(7, telemetrySentToChannel.Count);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Req1", ((RequestTelemetry)telemetrySentToChannel[0]).Name);
            Assert.AreEqual(true, ((RequestTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Req1", ((RequestTelemetry)telemetrySentToChannel[1]).Name);
            Assert.AreEqual(true, ((RequestTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry)telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Req1", ((RequestTelemetry)telemetrySentToChannel[2]).Name);
            Assert.AreEqual(true, ((RequestTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[3]);
            Assert.AreEqual("Req1", ((RequestTelemetry)telemetrySentToChannel[3]).Name);
            Assert.AreEqual(true, ((RequestTelemetry)telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry)telemetrySentToChannel[3]).Properties["_MS.ProcessedByMetricExtractors"]);

            for(int i = 4; i < 7; i++)
            {
                AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[i]);
                var metricTel = telemetrySentToChannel[i] as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("requests/duration", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("Request.Success"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/resultCode"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/performanceBucket"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
            }

            // We expect RoleInstanceA to be tracked correctly
            var cloudRoleInstanceA = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceA")));

            Assert.IsTrue(cloudRoleInstanceA.Count() == 1);

            // We expect RoleInstanceB to be tracked correctly
            var cloudRoleInstanceB = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceB")));

            Assert.IsTrue(cloudRoleInstanceB.Count() == 1);

            // We expect RoleInstanceC to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceC = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceC")));

            Assert.IsTrue(cloudRoleInstanceC.Count() == 0);

            // We expect RoleInstanceD to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceD = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceD")));

            Assert.IsTrue(cloudRoleInstanceD.Count() == 0);

            // We expect a DIMENSION-CAPPED series, which represents RoleInstanceC and RoleInstanceD
            var dimCappedSeries = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "DIMENSION-CAPPED")));

            Assert.IsTrue(dimCappedSeries.Count() == 1);
        }

        [TestMethod]
        public void Request_CorrectlyExtractsMetric()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            // default set of dimensions with test values.
            bool[] success = new bool[] { true, false };
            bool[] synthetic = new bool[] { true, false };
            string[] responseCode = new string[] { "200", "500", "401" };
            string[] cloudRoleNames = new string[] { "RoleA", "RoleB" };
            string[] cloudRoleInstances = new string[] { "RoleInstanceA", "RoleInstanceB" };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                List<RequestTelemetry> requests = new List<RequestTelemetry>();

                // Produces telemetry with every combination of dimension values.
                for(int i = 0; i < success.Length; i++)
                {
                    for (int j = 0; j < responseCode.Length; j++)
                    {
                        for (int k = 0; k < cloudRoleNames.Length; k++)
                        {
                            for (int l = 0; l < cloudRoleInstances.Length; l++)
                            {
                                for (int m = 0; m < synthetic.Length; m++)
                                {
                                    // For ease of validation 4 calls are tracked.
                                    // with 100, 100. 600. 600.
                                    // This will fall into 2 buckets <250msec, and 500ms-1sec
                                    requests.Add(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), responseCode[j], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                    requests.Add(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(100), responseCode[j], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                    requests.Add(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(600), responseCode[j], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                    requests.Add(CreateRequestTelemetry(
                                        TimeSpan.FromMilliseconds(600), responseCode[j], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                }
                            }
                        }
                    }
                }

                foreach(var req in requests)
                {
                    client.TrackRequest(req);
                }
                
                // The # of iteration is 48 = 2 * 2 * 3 * 2 * 2
                //  success * synthetic * responseCode * RoleName * RoleInstance
                // 4 Track calls are made in every iteration,
                // hence 48 * 4 requests gives 192 total requests
                Assert.AreEqual(192, telemetrySentToChannel.Count);

                // The total # of timeseries is 96
                // 2 * 2 * 3 * 2 * 2 * 2 = 96
                // success * synthetic * responseCode * RoleName * RoleInstance * DurationBucket
                // Duration bucket is auto calculated, hence not included in iteration count.

                // The above did not include Metrics as they are sent upon dispose only.                
            } // dispose occurs here, and hence metrics get flushed out.

            // 2 * 2 * 3 * 2 * 2 * 2 = 96
            // success * synthetic * responseCode * RoleName * RoleInstance * DurationBucket
            int totalTimeSeries = 96;

            // 288 = 192 requests + 96 metrics as there are 96 unique combination of dimension
            Assert.AreEqual(288, telemetrySentToChannel.Count);

            // These are pre-agg metric
            var serverResponseMetric = telemetrySentToChannel.Where(
                (tel) => "Server response time".Equals((tel as MetricTelemetry)?.Name));
            Assert.AreEqual(totalTimeSeries, serverResponseMetric.Count());

            foreach(var metric in serverResponseMetric)
            {
                var metricTel = metric as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("requests/duration", metricTel.Context.GlobalProperties["_MS.MetricId"]);
                
                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("Request.Success"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/resultCode"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("request/performanceBucket"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
            }

            // Validate success dimension
            for (int i = 0; i < success.Length; i++)
            {
                var metricCollection = serverResponseMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["Request.Success"] == success[i].ToString());
                int expectedCount = totalTimeSeries / success.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate synthetic dimension
            for (int i = 0; i < synthetic.Length; i++)
            {
                var metricCollection = serverResponseMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["operation/synthetic"] == synthetic[i].ToString());
                int expectedCount = totalTimeSeries / synthetic.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate RoleName dimesion
            for (int i = 0; i < cloudRoleNames.Length; i++)
            {
                var metricCollection = serverResponseMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleName"] == cloudRoleNames[i]);
                int expectedCount = totalTimeSeries / cloudRoleNames.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate RoleInstance dimension
            for (int i = 0; i < cloudRoleInstances.Length; i++)
            {
                var metricCollection = serverResponseMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleInstance"] == cloudRoleInstances[i]);
                int expectedCount = totalTimeSeries / cloudRoleInstances.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate ResponseCode dimension
            for (int i = 0; i < responseCode.Length; i++)
            {
                var metricCollection = serverResponseMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["request/resultCode"] == responseCode[i]);
                int expectedCount = totalTimeSeries / responseCode.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate Duration Bucket dimension
            {
                var metricCollectionBelow250 = serverResponseMetric.Where(
                    (tel) => (tel as MetricTelemetry).Properties["request/performanceBucket"] == "<250ms");

                var metricCollection500mSecTo1Sec = serverResponseMetric.Where(
                    (tel) => (tel as MetricTelemetry).Properties["request/performanceBucket"] == "500ms-1sec");


                int expectedCount = totalTimeSeries / 2;
                Assert.AreEqual(expectedCount, metricCollectionBelow250.Count());
                ValidateAllMetric(metricCollectionBelow250);

                Assert.AreEqual(expectedCount, metricCollection500mSecTo1Sec.Count());
                ValidateAllMetric(metricCollection500mSecTo1Sec);
            }
        }

        private RequestTelemetry CreateRequestTelemetry(TimeSpan duration, string resp, bool success, bool synthetic,
            string role, string instance)
        {
            var req = new RequestTelemetry("Req1", DateTimeOffset.Now, duration, resp, success);
            req.Context.Cloud.RoleName = role;
            req.Context.Cloud.RoleInstance = instance;
            if (synthetic)
            {
                req.Context.Operation.SyntheticSource = "synthetic";
            }

            return req;
        }

        private DependencyTelemetry CreateDependencyTelemetry(TimeSpan duration, string target, string resultCode, string type,
            bool success, bool synthetic, string role, string instance)
        {            
            var dep = new DependencyTelemetry(type, target, "Dep1", "data", DateTimeOffset.Now, duration, resultCode, success);
            dep.Context.Cloud.RoleName = role;
            dep.Context.Cloud.RoleInstance = instance;
            if (synthetic)
            {
                dep.Context.Operation.SyntheticSource = "synthetic";
            }

            return dep;
        }

        private ExceptionTelemetry CreateExceptionTelemetry(Exception exception, string role, string instance)
        {
            var exp = new ExceptionTelemetry(exception);
            exp.Context.Cloud.RoleName = role;
            exp.Context.Cloud.RoleInstance = instance;
            return exp;
        }

        private void ValidateAllMetric(IEnumerable<ITelemetry> metricCollection)
        {
            foreach (var singleMetric in metricCollection)
            {
                var m = singleMetric as MetricTelemetry;
                // 2 is the count - its either 100,100 or 600,600 leading to sum of either 200 or 1200
                Assert.AreEqual(2, m.Count);                
                Assert.IsTrue(m.Sum == 200 || m.Sum == 1200);
            }
        }

        [TestMethod]
        public void Request_CorrectlyWorksWithResponseSuccess()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);

                client.TrackRequest(new RequestTelemetry()
                                    {
                                        Name = "Test Request 1",
                                        Timestamp = DateTimeOffset.Now,
                                        Duration = TimeSpan.FromMilliseconds(5),
                                        ResponseCode = "xxx",
                                        Success = true
                                    });

                client.TrackRequest(new RequestTelemetry()
                                    {
                                        Name = "Test Request 2",
                                        Timestamp = DateTimeOffset.Now,
                                        Duration = TimeSpan.FromMilliseconds(10),
                                        ResponseCode = "xxx",
                                        Success = false
                                    });

                client.TrackRequest(new RequestTelemetry()
                                    {
                                        Name = "Test Request 3",
                                        Timestamp = DateTimeOffset.Now,
                                        Duration = TimeSpan.FromMilliseconds(15),
                                        ResponseCode = "xxx",
                                        Success = null
                                    });
            }

            Assert.AreEqual(5, telemetrySentToChannel.Count);

            var t = new SortedList<string, MetricTelemetry>();

            Assert.IsNotNull(telemetrySentToChannel[3]);
            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[3]);
            var m = (MetricTelemetry)telemetrySentToChannel[3];
            t.Add(m.Properties["Request.Success"], m);

            Assert.IsNotNull(telemetrySentToChannel[4]);
            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[4]);
            m = (MetricTelemetry)telemetrySentToChannel[4];
            t.Add(m.Properties["Request.Success"], m);

            var metricF = t.Values[0];
            var metricT = t.Values[1];

            Assert.AreEqual("Server response time", metricT.Name);
            Assert.AreEqual(2, metricT.Count);
            Assert.AreEqual(15, metricT.Max);
            Assert.AreEqual(5, metricT.Min);
            Assert.AreEqual(20, metricT.Sum);
            Assert.AreEqual(true, metricT.Properties.ContainsKey("Request.Success"));
            Assert.AreEqual(Boolean.TrueString, metricT.Properties["Request.Success"]);

            Assert.AreEqual("Server response time", metricF.Name);
            Assert.AreEqual(1, metricF.Count);
            Assert.AreEqual(10, metricF.Max);
            Assert.AreEqual(10, metricF.Min);
            Assert.AreEqual(10, metricF.Sum);
            Assert.AreEqual(true, metricF.Properties.ContainsKey("Request.Success"));
            Assert.AreEqual(Boolean.FalseString, metricF.Properties["Request.Success"]);
        }

        [TestMethod]
        public void Request_DefaultDimensionLimitsValidation()
        {
            var reqExtractor = new RequestMetricsExtractor();
            Assert.AreEqual(30, reqExtractor.MaxResponseCodeToDiscover);
            Assert.AreEqual(2, reqExtractor.MaxCloudRoleNameValuesToDiscover);
            Assert.AreEqual(2, reqExtractor.MaxCloudRoleInstanceValuesToDiscover);
        }
        #endregion Request-metrics-related Tests

        #region Dependency-metrics-related Tests

        [TestMethod]
        public void Dependency_DefaultDimensionLimitsValidation()
        {
            var depExtractor = new DependencyMetricsExtractor();
            Assert.AreEqual(15, depExtractor.MaxDependencyTypesToDiscover);
            Assert.AreEqual(125, depExtractor.MaxDependencyTargetValuesToDiscover);
            Assert.AreEqual(2, depExtractor.MaxCloudRoleNameValuesToDiscover);
            Assert.AreEqual(2, depExtractor.MaxCloudRoleInstanceValuesToDiscover);
        }

        [TestMethod]
        public void Dependency_TelemetryMarkedAsProcessedCorrectly()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc) { MaxDependencyTypesToDiscover = 0 };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                
                client.TrackRequest("Test Request", DateTimeOffset.Now, TimeSpan.FromMilliseconds(10), "200", success: true);
                client.TrackDependency("Type", "Target", "depName1", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(1000), "ResultCode100", true);
                client.TrackDependency("Type", "Target", "depName2", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(1000), "ResultCode100", true);
                client.TrackDependency("Type", "Target", "depName3", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(1000), "ResultCode100", true);
                client.TrackEvent("Test Event");
            }

            Assert.AreEqual(7, telemetrySentToChannel.Count);
            
            AssertEx.IsType<RequestTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual(true, ((RequestTelemetry) telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Requests', Ver:'1.1')",
                         ((RequestTelemetry) telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<DependencyTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("depName1", ((DependencyTelemetry) telemetrySentToChannel[1]).Name);
            Assert.AreEqual(true, ((DependencyTelemetry) telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Dependencies', Ver:'1.1')",
                         ((DependencyTelemetry) telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<DependencyTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("depName2", ((DependencyTelemetry)telemetrySentToChannel[2]).Name);
            Assert.AreEqual(true, ((DependencyTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Dependencies', Ver:'1.1')",
                         ((DependencyTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<DependencyTelemetry>(telemetrySentToChannel[3]);
            Assert.AreEqual("depName3", ((DependencyTelemetry) telemetrySentToChannel[3]).Name);
            Assert.AreEqual(true, ((DependencyTelemetry) telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Dependencies', Ver:'1.1')",
                         ((DependencyTelemetry) telemetrySentToChannel[3]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<EventTelemetry>(telemetrySentToChannel[4]);
            Assert.AreEqual("Test Event", ((EventTelemetry) telemetrySentToChannel[4]).Name);
            Assert.AreEqual(false, ((EventTelemetry) telemetrySentToChannel[4]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));


            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[5]);
            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[6]);

            Assert.AreEqual(1, telemetrySentToChannel.Where( (t) => "Server response time".Equals((t as MetricTelemetry)?.Name) ).Count());
            Assert.AreEqual(1, telemetrySentToChannel.Where( (t) => "Dependency duration".Equals((t as MetricTelemetry)?.Name) ).Count());
        }

        [TestMethod]
        public void Dependency_TelemetryRespectsDimLimitTargetZero()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxDependencyTargetValuesToDiscover = 0;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 3 requests with 3 different values for Result code - 200,201,202.
                // As MaxDependencyTargetValuesToDiscover = 0, we expect all target to be rolled into Other
                client.TrackDependency("Type", "TargetA", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
                client.TrackDependency("Type", "TargetB", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
                client.TrackDependency("Type", "TargetC", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
            }

            ValidateTelemetryAndMetricWithRestrictedDimension(telemetrySentToChannel, 4, 3, "dependency/target", "Other");
        }

        [TestMethod]
        public void Dependency_TelemetryRespectsDimLimitResultCodeZero()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxDependencyResultCodesToDiscover = 0;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 3 requests with 3 different values for Result code - resA,resB,resC.
                // As MaxDependencyResultCodesToDiscover = 0, we expect all target to be rolled into Other
                client.TrackDependency("Type", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "resA", true);
                client.TrackDependency("Type", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "resB", true);
                client.TrackDependency("Type", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "resC", true);
            }

            ValidateTelemetryAndMetricWithRestrictedDimension(telemetrySentToChannel, 4, 3, "dependency/resultCode", "Other");
        }

        [TestMethod]
        public void Dependency_TelemetryRespectsDimLimitTypeZero()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxDependencyTypesToDiscover = 0;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 3 requests with 3 different values for Result code - A,B,C.
                // As MaxDependencyTypesToDiscover = 0, we expect all type to be rolled into Other
                client.TrackDependency("TypeA", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
                client.TrackDependency("TypeB", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
                client.TrackDependency("TypeC", "Target", "Name", "data", DateTimeOffset.Now, TimeSpan.FromMilliseconds(100), "res", true);
            }

            ValidateTelemetryAndMetricWithRestrictedDimension(telemetrySentToChannel, 4, 3, "Dependency.Type", "Other");
        }

        private void ValidateTelemetryAndMetricWithRestrictedDimension(List<ITelemetry> telemetrySentToChannel, int expectedCount, int metricStartIndex,
            string restrictedPropertyName, string expectedValueForRestrictedProperty)
        {
            Assert.AreEqual(expectedCount, telemetrySentToChannel.Count);
            for(int i = 0; i< metricStartIndex; i++)
            {
                AssertEx.IsType<DependencyTelemetry>(telemetrySentToChannel[i]);
                Assert.AreEqual("Name", ((DependencyTelemetry)telemetrySentToChannel[i]).Name);
                Assert.AreEqual(true, ((DependencyTelemetry)telemetrySentToChannel[i]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
                Assert.AreEqual("(Name:'Dependencies', Ver:'1.1')",
                             ((DependencyTelemetry)telemetrySentToChannel[i]).Properties["_MS.ProcessedByMetricExtractors"]);
            }

            // validate metric

            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[metricStartIndex]);
            var metricTel = telemetrySentToChannel[metricStartIndex] as MetricTelemetry;

            // validate standard fields
            Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
            Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
            Assert.AreEqual("dependencies/duration", metricTel.Context.GlobalProperties["_MS.MetricId"]);

            // validate dimensions exist
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("Dependency.Success"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("Dependency.Type"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/target"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/performanceBucket"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/resultCode"));

            var resultCodeDimension = metricTel.Properties[restrictedPropertyName];
            Assert.AreEqual(expectedValueForRestrictedProperty, resultCodeDimension);
        }

        [TestMethod]
        public void Dependency_CorrectlyExtractsMetric()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            // default set of dimensions with test values.
            bool[] success = new bool[] { true, false };
            bool[] synthetic = new bool[] { true, false };
            string[] targets = new string[] { "TargetA", "TargetB", "TargetC" };
            string[] resultCodes = new string[] { "ResCodeA", "ResCodeB", "ResCodeC" };
            string[] types = new string[] { "TypeA", "TypeB", "TypeC" };
            string[] cloudRoleNames = new string[] { "RoleA", "RoleB" };
            string[] cloudRoleInstances = new string[] { "RoleInstanceA", "RoleInstanceB" };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                List<DependencyTelemetry> dependencies = new List<DependencyTelemetry>();

                // Produces telemetry with every combination of dimension values.
                for (int i = 0; i < success.Length; i++)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        for (int k = 0; k < cloudRoleNames.Length; k++)
                        {
                            for (int l = 0; l < cloudRoleInstances.Length; l++)
                            {
                                for (int m = 0; m < synthetic.Length; m++)
                                {
                                    for (int n = 0; n < types.Length; n++)
                                    {
                                        for (int o = 0; o < resultCodes.Length; o++)
                                        {
                                            // For ease of validation 4 calls are tracked.
                                            // with 100, 100. 600. 600.
                                            // This will fall into 2 buckets <250msec, and 500ms-1sec
                                            dependencies.Add(CreateDependencyTelemetry(
                                            TimeSpan.FromMilliseconds(100), targets[j], resultCodes[o], types[n], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                            dependencies.Add(CreateDependencyTelemetry(
                                                TimeSpan.FromMilliseconds(100), targets[j], resultCodes[o], types[n], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                            dependencies.Add(CreateDependencyTelemetry(
                                                TimeSpan.FromMilliseconds(600), targets[j], resultCodes[o], types[n], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));
                                            dependencies.Add(CreateDependencyTelemetry(
                                                TimeSpan.FromMilliseconds(600), targets[j], resultCodes[o], types[n], success[i], synthetic[m], cloudRoleNames[k], cloudRoleInstances[l]));

                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }

                foreach (var dep in dependencies)
                {
                    client.TrackDependency(dep);
                }


                // 2 * 2 * 3 * 3 *  2 * 2 * 3= 432 iteration
                // DurationBucket is not included in interation as its automatically extracted.
                // 4 Track calls are made in every iteration,
                // hence 432 * 4 dependencies gives 1728 total dependencies
                Assert.AreEqual(1728, telemetrySentToChannel.Count);

                // The above did not include Metrics as they are sent upon dispose only.                
            } // dispose occurs here, and hence metrics get flushed out.

            // 2 * 2 * 3 * 3 *  2 * 2 * 2 * 3 = 864 timeseries
            // success * synthetic * target * type * RoleName * RoleInstance *DurationBucket * ResultCode
            int totalTimeSeries = 864;

            // 2592 = 1728 requests + 864 metrics as there are 864 unique combination of dimension
            Assert.AreEqual(2592, telemetrySentToChannel.Count);

            // These are pre-agg metric
            var depDurationMetric = telemetrySentToChannel.Where(
                (tel) => "Dependency duration".Equals((tel as MetricTelemetry)?.Name));
            Assert.AreEqual(totalTimeSeries, depDurationMetric.Count());

            foreach (var metric in depDurationMetric)
            {
                var metricTel = metric as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("dependencies/duration", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("Dependency.Success"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("Dependency.Type"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/target"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/performanceBucket"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("dependency/resultCode"));
            }

            // Validate success dimension
            for (int i = 0; i < success.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["Dependency.Success"] == success[i].ToString());
                int expectedCount = totalTimeSeries / success.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate synthetic dimension
            for (int i = 0; i < synthetic.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["operation/synthetic"] == synthetic[i].ToString());
                int expectedCount = totalTimeSeries / synthetic.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate RoleName dimesion
            for (int i = 0; i < cloudRoleNames.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleName"] == cloudRoleNames[i]);
                int expectedCount = totalTimeSeries / cloudRoleNames.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate RoleInstance dimension
            for (int i = 0; i < cloudRoleInstances.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleInstance"] == cloudRoleInstances[i]);
                int expectedCount = totalTimeSeries / cloudRoleInstances.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate Dep Type dimension
            for (int i = 0; i < types.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["Dependency.Type"] == types[i]);
                int expectedCount = totalTimeSeries / types.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate Dep Target dimension
            for (int i = 0; i < targets.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["dependency/target"] == targets[i]);
                int expectedCount = totalTimeSeries / types.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate Dep ResultCode dimension
            for (int i = 0; i < resultCodes.Length; i++)
            {
                var metricCollection = depDurationMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["dependency/resultCode"] == resultCodes[i]);
                int expectedCount = totalTimeSeries / resultCodes.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetric(metricCollection);
            }

            // Validate Duration Bucket dimension
            {
                var metricCollectionBelow250 = depDurationMetric.Where(
                    (tel) => (tel as MetricTelemetry).Properties["dependency/performanceBucket"] == "<250ms");

                var metricCollection500mSecTo1Sec = depDurationMetric.Where(
                    (tel) => (tel as MetricTelemetry).Properties["dependency/performanceBucket"] == "500ms-1sec");


                int expectedCount = totalTimeSeries / 2;
                Assert.AreEqual(expectedCount, metricCollectionBelow250.Count());
                ValidateAllMetric(metricCollectionBelow250);

                Assert.AreEqual(expectedCount, metricCollection500mSecTo1Sec.Count());
                ValidateAllMetric(metricCollection500mSecTo1Sec);
            }
        }

        #endregion Dependency-metrics-related Tests

        #region Exception-metrics-related Tests

        [TestMethod]
        public void Exception_TelemetryMarkedAsProcessedCorrectly()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                client.TrackException(new ExceptionTelemetry(new Exception("Test A")));
                client.TrackException(new ExceptionTelemetry(new NullReferenceException("Test B")));
                client.TrackException(new ExceptionTelemetry(new ArgumentException("Test C")));
            }

            Assert.AreEqual(4, telemetrySentToChannel.Count);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test A", ((ExceptionTelemetry)telemetrySentToChannel[0]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Test B", ((ExceptionTelemetry)telemetrySentToChannel[1]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Test C", ((ExceptionTelemetry)telemetrySentToChannel[2]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[3]);
            var metricTel = telemetrySentToChannel[3] as MetricTelemetry;
            // validate standard fields
            Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
            Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
            Assert.AreEqual("exceptions/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

            // validate dimensions exist
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
        }

        [TestMethod]
        public void Exception_TelemetryRespectsDimLimitCloudRoleInstance()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxExceptionCloudRoleInstanceValuesToDiscover = 2;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 4 exceptions with 4 different values for RoleInstance - A B C D.
                // As MaxExceptionCloudRoleInstanceValuesToDiscover = 2, the first 2 values encountered (A,B) 
                // will be used as such at which the DimensionCap is hit.
                // Newly incoming values (C,D) will be rolled into "DIMENSION-CAPPED"

                client.TrackException(CreateExceptionTelemetry(
                                        new ArgumentException("Test A"), "RoleNameA", "RoleInstanceA"));
                client.TrackException(CreateExceptionTelemetry(
                                        new NullReferenceException("Test B"), "RoleNameA", "RoleInstanceB"));
                client.TrackException(CreateExceptionTelemetry(
                                        new NullReferenceException("Test C"), "RoleNameA", "RoleInstanceC"));
                client.TrackException(CreateExceptionTelemetry(
                                        new ArgumentException("Test D"), "RoleNameA", "RoleInstanceD"));
            }

            // 4 exceptions + 3 metric
            Assert.AreEqual(7, telemetrySentToChannel.Count);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test A", ((ExceptionTelemetry)telemetrySentToChannel[0]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Test B", ((ExceptionTelemetry)telemetrySentToChannel[1]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Test C", ((ExceptionTelemetry)telemetrySentToChannel[2]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<ExceptionTelemetry>(telemetrySentToChannel[3]);
            Assert.AreEqual("Test D", ((ExceptionTelemetry)telemetrySentToChannel[3]).Exception.Message);
            Assert.AreEqual(true, ((ExceptionTelemetry)telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Exceptions', Ver:'1.1')",
                         ((ExceptionTelemetry)telemetrySentToChannel[3]).Properties["_MS.ProcessedByMetricExtractors"]);

            for (int i = 4; i < 7; i++)
            {
                AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[i]);
                var metricTel = telemetrySentToChannel[i] as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("exceptions/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
            }

            // We expect RoleInstanceA to be tracked correctly
            var cloudRoleInstanceA = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceA")));

            Assert.IsTrue(cloudRoleInstanceA.Count() == 1);

            // We expect RoleInstanceB to be tracked correctly
            var cloudRoleInstanceB = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceB")));

            Assert.IsTrue(cloudRoleInstanceB.Count() == 1);

            // We expect RoleInstanceC to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceC = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceC")));

            Assert.IsTrue(cloudRoleInstanceC.Count() == 0);

            // We expect RoleInstanceD to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceD = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceD")));

            Assert.IsTrue(cloudRoleInstanceD.Count() == 0);

            // We expect a DIMENSION-CAPPED series, which represents RoleInstanceC and RoleInstanceD
            var dimCappedSeries = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "DIMENSION-CAPPED")));

            Assert.IsTrue(dimCappedSeries.Count() == 1);
        }

        [TestMethod]
        public void Exception_CorrectlyExtractsMetric()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            // default set of dimensions with test values.
            string[] cloudRoleNames = new string[] { "RoleA", "RoleB" };
            string[] cloudRoleInstances = new string[] { "RoleInstanceA", "RoleInstanceB" };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                List<ExceptionTelemetry> exceptions = new List<ExceptionTelemetry>();

                // Produces telemetry with every combination of dimension values.
                for (int i = 0; i < cloudRoleNames.Length; i++)
                {
                    for (int j = 0; j < cloudRoleInstances.Length; j++)
                    {
                        exceptions.Add(CreateExceptionTelemetry(
                            new NullReferenceException("Test"), cloudRoleNames[i], cloudRoleInstances[j]));
                        exceptions.Add(CreateExceptionTelemetry(
                            new ArgumentException("Test"), cloudRoleNames[i], cloudRoleInstances[j]));
                        exceptions.Add(CreateExceptionTelemetry(
                            new NullReferenceException("Test"), cloudRoleNames[i], cloudRoleInstances[j]));
                        exceptions.Add(CreateExceptionTelemetry(
                            new ArgumentException("Test"), cloudRoleNames[i], cloudRoleInstances[j]));
                    }
                }


                foreach (var exp in exceptions)
                {
                    client.TrackException(exp);
                }

                // The # of iteration is 4  = 2 * 2
                //  RoleName * RoleInstance
                // 4 Track calls are made in every iteration,
                // hence 4 * 4 exceptions gives 16 total exceptions
                Assert.AreEqual(16, telemetrySentToChannel.Count);

                // The total # of timeseries is 4
                // 2 * 2 = 4
                // RoleInstance

                // The above did not include Metrics as they are sent upon dispose only.                
            } // dispose occurs here, and hence metrics get flushed out.

            // 2 * 2 = 4
            // RoleName * RoleInstance
            int totalTimeSeries = 4;

            // 20 = 16 exceptions + 4 metrics as there are 4 unique combination of dimension
            Assert.AreEqual(20, telemetrySentToChannel.Count);

            // These are pre-agg metric
            var serverExceptionMetric = telemetrySentToChannel.Where(
                (tel) => "Exceptions".Equals((tel as MetricTelemetry)?.Name));
            Assert.AreEqual(totalTimeSeries, serverExceptionMetric.Count());

            foreach (var metric in serverExceptionMetric)
            {
                var metricTel = metric as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("exceptions/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
            }

            // Validate RoleName dimesion
            for (int i = 0; i < cloudRoleNames.Length; i++)
            {
                var metricCollection = serverExceptionMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleName"] == cloudRoleNames[i]);
                int expectedCount = totalTimeSeries / cloudRoleNames.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
            }

            // Validate RoleInstance dimension
            for (int i = 0; i < cloudRoleInstances.Length; i++)
            {
                var metricCollection = serverExceptionMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleInstance"] == cloudRoleInstances[i]);
                int expectedCount = totalTimeSeries / cloudRoleInstances.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
            }
        }

        [TestMethod]
        public void Exception_DefaultDimensionLimitsValidation()
        {
            var expExtractor = new ExceptionMetricsExtractor();
            Assert.AreEqual(2, expExtractor.MaxCloudRoleNameValuesToDiscover);
            Assert.AreEqual(2, expExtractor.MaxCloudRoleInstanceValuesToDiscover);
        }
        #endregion Exception-metrics-related Tests

        #region Trace-metrics-related Tests

        [TestMethod]
        public void Trace_TelemetryMarkedAsProcessedCorrectly()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                client.TrackTrace("Test 1", SeverityLevel.Error);
                client.TrackTrace("Test 2", SeverityLevel.Error);
                client.TrackTrace("Test 3", SeverityLevel.Error);
            }

            Assert.AreEqual(4, telemetrySentToChannel.Count);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test 1", ((TraceTelemetry)telemetrySentToChannel[0]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Test 2", ((TraceTelemetry)telemetrySentToChannel[1]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Test 3", ((TraceTelemetry)telemetrySentToChannel[2]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[3]);
            var metricTel = telemetrySentToChannel[3] as MetricTelemetry;
            // validate standard fields
            Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
            Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
            Assert.AreEqual("traces/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

            // validate dimensions exist
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("trace/severityLevel"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
        }

        [TestMethod]
        public void Trace_TelemetryRespectsNoSeverityLevel()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                client.TrackTrace("Test 1");
            }

            Assert.AreEqual(2, telemetrySentToChannel.Count);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test 1", ((TraceTelemetry)telemetrySentToChannel[0]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[1]);
            var metricTel = telemetrySentToChannel[1] as MetricTelemetry;
            // validate standard fields
            Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
            Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
            Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
            Assert.AreEqual("traces/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

            // validate dimensions exist
            Assert.AreEqual(true, metricTel.Properties.ContainsKey("trace/severityLevel"));
            Assert.AreEqual("Unspecified", metricTel.Properties["trace/severityLevel"]);
        }

        [TestMethod]
        public void Trace_TelemetryRespectsDimLimitCloudRoleInstance()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory =
                (nextProc) => {
                    var metricExtractor = new AutocollectedMetricsExtractor(nextProc);
                    metricExtractor.MaxTraceCloudRoleInstanceValuesToDiscover = 2;
                    return metricExtractor;
                };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                // Track 4 traces with 4 different values for RoleInstance - A B C D.
                // As MaxTraceCloudRoleInstanceValuesToDiscover = 2, the first 2 values encountered (A,B) 
                // will be used as such at which the DimensionCap is hit.
                // Newly incoming values (C,D) will be rolled into "DIMENSION-CAPPED"

                client.TrackTrace(CreateTraceTelemetry(
                                        "Test 1", 0, false, "RoleNameA", "RoleInstanceA"));
                client.TrackTrace(CreateTraceTelemetry(
                                        "Test 2", 0, false, "RoleNameA", "RoleInstanceB"));
                client.TrackTrace(CreateTraceTelemetry(
                                        "Test 3", 0, false, "RoleNameA", "RoleInstanceC"));
                client.TrackTrace(CreateTraceTelemetry(
                                        "Test 4", 0, false, "RoleNameA", "RoleInstanceD"));
            }

            // 4 traces + 3 metric
            Assert.AreEqual(7, telemetrySentToChannel.Count);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[0]);
            Assert.AreEqual("Test 1", ((TraceTelemetry)telemetrySentToChannel[0]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[0]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[0]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[1]);
            Assert.AreEqual("Test 2", ((TraceTelemetry)telemetrySentToChannel[1]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[1]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[1]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[2]);
            Assert.AreEqual("Test 3", ((TraceTelemetry)telemetrySentToChannel[2]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[2]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[2]).Properties["_MS.ProcessedByMetricExtractors"]);

            AssertEx.IsType<TraceTelemetry>(telemetrySentToChannel[3]);
            Assert.AreEqual("Test 4", ((TraceTelemetry)telemetrySentToChannel[3]).Message);
            Assert.AreEqual(true, ((TraceTelemetry)telemetrySentToChannel[3]).Properties.ContainsKey("_MS.ProcessedByMetricExtractors"));
            Assert.AreEqual("(Name:'Traces', Ver:'1.1')",
                         ((TraceTelemetry)telemetrySentToChannel[3]).Properties["_MS.ProcessedByMetricExtractors"]);

            for (int i = 4; i < 7; i++)
            {
                AssertEx.IsType<MetricTelemetry>(telemetrySentToChannel[i]);
                var metricTel = telemetrySentToChannel[i] as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("traces/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("trace/severityLevel"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
            }

            // We expect RoleInstanceA to be tracked correctly
            var cloudRoleInstanceA = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceA")));

            Assert.IsTrue(cloudRoleInstanceA.Count() == 1);

            // We expect RoleInstanceB to be tracked correctly
            var cloudRoleInstanceB = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceB")));

            Assert.IsTrue(cloudRoleInstanceB.Count() == 1);

            // We expect RoleInstanceC to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceC = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceC")));

            Assert.IsTrue(cloudRoleInstanceC.Count() == 0);

            // We expect RoleInstanceD to be not present as a dimension, as dimension cap of 2 is already hit.
            var cloudRoleInstanceD = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "RoleInstanceD")));

            Assert.IsTrue(cloudRoleInstanceD.Count() == 0);

            // We expect a DIMENSION-CAPPED series, which represents RoleInstanceC and RoleInstanceD
            var dimCappedSeries = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name)
                && (tel as MetricTelemetry).Properties.Contains(new KeyValuePair<string, string>("cloud/roleInstance", "DIMENSION-CAPPED")));

            Assert.IsTrue(dimCappedSeries.Count() == 1);
        }

        [TestMethod]
        public void Trace_CorrectlyExtractsMetric()
        {
            List<ITelemetry> telemetrySentToChannel = new List<ITelemetry>();
            Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory = (nextProc) => new AutocollectedMetricsExtractor(nextProc);

            // default set of dimensions with test values.
            int[] severityLevels = new int[] { 0, 1, 2, 3, 4}; //Backend stores SeverityLevel as integer values
            bool[] synthetic = new bool[] { true, false };
            string[] cloudRoleNames = new string[] { "RoleA", "RoleB" };
            string[] cloudRoleInstances = new string[] { "RoleInstanceA", "RoleInstanceB" };

            TelemetryConfiguration telemetryConfig = CreateTelemetryConfigWithExtractor(telemetrySentToChannel, extractorFactory);
            using (telemetryConfig)
            {
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                List<TraceTelemetry> traces = new List<TraceTelemetry>();

                // Produces telemetry with every combination of dimension values.
                for (int i = 0; i < severityLevels.Length; i++)
                {
                    for (int j = 0; j < cloudRoleNames.Length; j++)
                    {
                        for (int k = 0; k < cloudRoleInstances.Length; k++)
                        {
                            for (int l = 0; l < synthetic.Length; l++)
                            {
                                // For ease of validation 4 calls are tracked.
                                traces.Add(CreateTraceTelemetry(
                                    "Test 1", severityLevels[i], synthetic[l], cloudRoleNames[j], cloudRoleInstances[k]));
                                traces.Add(CreateTraceTelemetry(
                                    "Test 2", severityLevels[i], synthetic[l], cloudRoleNames[j], cloudRoleInstances[k]));
                                traces.Add(CreateTraceTelemetry(
                                     "Test 3", severityLevels[i], synthetic[l], cloudRoleNames[j], cloudRoleInstances[k]));
                                traces.Add(CreateTraceTelemetry(
                                     "Test 4", severityLevels[i], synthetic[l], cloudRoleNames[j], cloudRoleInstances[k]));
                            }
                        }
                    }
                }

                foreach (var trace in traces)
                {
                    client.TrackTrace(trace);
                }

                // The # of iteration is 40 = 5 * 2 * 2 * 2
                //  SeverityLevel * synthetic * RoleName * RoleInstance
                // 4 Track calls are made in every iteration,
                // hence 40 * 4 requests gives 192 total requests
                Assert.AreEqual(160, telemetrySentToChannel.Count);

                // The total # of timeseries is 40
                // 5 * 2 * 2 * 2  = 40
                // SeverityLevel * synthetic * RoleName * RoleInstance

                // The above did not include Metrics as they are sent upon dispose only.                
            } // dispose occurs here, and hence metrics get flushed out.

            // 5 * 2 * 2 * 2  = 40
            // SeverityLevel * synthetic * RoleName * RoleInstance
            int totalTimeSeries = 40;

            // 200 = 160 requests + 40 metrics as there are 40 unique combination of dimension
            Assert.AreEqual(200, telemetrySentToChannel.Count);

            // These are pre-agg metric
            var traceCountMetric = telemetrySentToChannel.Where(
                (tel) => "Traces".Equals((tel as MetricTelemetry)?.Name));
            Assert.AreEqual(totalTimeSeries, traceCountMetric.Count());

            foreach (var metric in traceCountMetric)
            {
                var metricTel = metric as MetricTelemetry;
                // validate standard fields
                Assert.IsTrue(metricTel.Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.IsAutocollected"));
                Assert.AreEqual("True", metricTel.Context.GlobalProperties["_MS.IsAutocollected"]);
                Assert.IsTrue(metricTel.Context.GlobalProperties.ContainsKey("_MS.MetricId"));
                Assert.AreEqual("traces/count", metricTel.Context.GlobalProperties["_MS.MetricId"]);

                // validate dimensions exist
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("trace/severityLevel"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleInstance"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("cloud/roleName"));
                Assert.AreEqual(true, metricTel.Properties.ContainsKey("operation/synthetic"));
            }

            // Validate SeverityLevel dimension
            for (int i = 0; i < severityLevels.Length; i++)
            {
                var metricCollection = traceCountMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["trace/severityLevel"] == severityLevels[i].ToString());
                int expectedCount = totalTimeSeries / severityLevels.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetricForTrace(metricCollection);
            }

            // Validate synthetic dimension
            for (int i = 0; i < synthetic.Length; i++)
            {
                var metricCollection = traceCountMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["operation/synthetic"] == synthetic[i].ToString());
                int expectedCount = totalTimeSeries / synthetic.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetricForTrace(metricCollection);
            }

            // Validate RoleName dimesion
            for (int i = 0; i < cloudRoleNames.Length; i++)
            {
                var metricCollection = traceCountMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleName"] == cloudRoleNames[i]);
                int expectedCount = totalTimeSeries / cloudRoleNames.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetricForTrace(metricCollection);
            }

            // Validate RoleInstance dimension
            for (int i = 0; i < cloudRoleInstances.Length; i++)
            {
                var metricCollection = traceCountMetric.Where(
                (tel) => (tel as MetricTelemetry).Properties["cloud/roleInstance"] == cloudRoleInstances[i]);
                int expectedCount = totalTimeSeries / cloudRoleInstances.Length;
                Assert.AreEqual(expectedCount, metricCollection.Count());
                ValidateAllMetricForTrace(metricCollection);
            }
        }

        private TraceTelemetry CreateTraceTelemetry(string message, int severityLevel, bool synthetic, string role, string instance)
        {
            var trace = new TraceTelemetry(message, (SeverityLevel)severityLevel);
            trace.Context.Cloud.RoleName = role;
            trace.Context.Cloud.RoleInstance = instance;

            if (synthetic)
            {
                trace.Context.Operation.SyntheticSource = "synthetic";
            }

            return trace;
        }

        [TestMethod]
        public void Trace_DefaultDimensionLimitsValidation()
        {
            var traceExtractor = new TraceMetricsExtractor();
            Assert.AreEqual(2, traceExtractor.MaxCloudRoleNameValuesToDiscover);
            Assert.AreEqual(2, traceExtractor.MaxCloudRoleInstanceValuesToDiscover);
        }

        private void ValidateAllMetricForTrace(IEnumerable<ITelemetry> metricCollection)
        {
            foreach (var singleMetric in metricCollection)
            {
                var m = singleMetric as MetricTelemetry;
                Assert.AreEqual(4, m.Count);
            }
        }
        #endregion Trace-metrics-related Tests

        #region Common Tools

        internal static TelemetryConfiguration CreateTelemetryConfigWithExtractor(IList<ITelemetry> telemetrySentToChannel,
                                                                                  Func<ITelemetryProcessor, AutocollectedMetricsExtractor> extractorFactory)
        {
            ITelemetryChannel channel = new StubTelemetryChannel 
            { 
                OnSend = (t) => telemetrySentToChannel.Add(t) 
            };
            string iKey = Guid.NewGuid().ToString("D");
            TelemetryConfiguration telemetryConfig = new TelemetryConfiguration(iKey, channel);

            var channelBuilder = new TelemetryProcessorChainBuilder(telemetryConfig);
            channelBuilder.Use(extractorFactory);
            channelBuilder.Build();


            return telemetryConfig;
        }

        #endregion Common Tools
    }
}
