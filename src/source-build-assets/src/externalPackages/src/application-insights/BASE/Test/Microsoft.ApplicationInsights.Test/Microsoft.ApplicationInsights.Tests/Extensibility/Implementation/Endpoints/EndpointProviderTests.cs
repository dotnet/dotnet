namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("Endpoints")]
    public class EndpointProviderTests
    {
        [TestMethod]
        public void TestDefaultEndpoints()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000",
                expectedBreezeEndpoint: Constants.DefaultIngestionEndpoint,
                expectedLiveMetricsEndpoint: Constants.DefaultLiveMetricsEndpoint,
                expectedProfilerEndpoint: Constants.DefaultProfilerEndpoint,
                expectedSnapshotEndpoint: Constants.DefaultSnapshotEndpoint);
        }

        [TestMethod]
        public void TestEndpointSuffix()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com",
                expectedBreezeEndpoint: "https://dc.ai.contoso.com/",
                expectedLiveMetricsEndpoint: "https://live.ai.contoso.com/",
                expectedProfilerEndpoint: "https://profiler.ai.contoso.com/",
                expectedSnapshotEndpoint: "https://snapshot.ai.contoso.com/");
        }

        [TestMethod]
        public void TestEndpointSuffix_WithExplicitOverride()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com;ProfilerEndpoint=https://custom.profiler.contoso.com:444/",
                expectedBreezeEndpoint: "https://dc.ai.contoso.com/",
                expectedLiveMetricsEndpoint: "https://live.ai.contoso.com/",
                expectedProfilerEndpoint: "https://custom.profiler.contoso.com:444/",
                expectedSnapshotEndpoint: "https://snapshot.ai.contoso.com/"); 
        }

        [TestMethod]
        public void TestEndpointSuffix_WithLocation()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com;Location=westus2",
                expectedBreezeEndpoint: "https://westus2.dc.ai.contoso.com/",
                expectedLiveMetricsEndpoint: "https://westus2.live.ai.contoso.com/",
                expectedProfilerEndpoint: "https://westus2.profiler.ai.contoso.com/",
                expectedSnapshotEndpoint: "https://westus2.snapshot.ai.contoso.com/");
        }

        [TestMethod]
        public void TestEndpointSuffix_WithLocation_WithExplicitOverride()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=ai.contoso.com;Location=westus2;ProfilerEndpoint=https://custom.profiler.contoso.com:444/",
                expectedBreezeEndpoint: "https://westus2.dc.ai.contoso.com/",
                expectedLiveMetricsEndpoint: "https://westus2.live.ai.contoso.com/",
                expectedProfilerEndpoint: "https://custom.profiler.contoso.com:444/",
                expectedSnapshotEndpoint: "https://westus2.snapshot.ai.contoso.com/");
        }

        [TestMethod]
        public void TestExpliticOverride_PreservesSchema()
        {
            RunTest(
                connectionString: "InstrumentationKey=00000000-0000-0000-0000-000000000000;ProfilerEndpoint=http://custom.profiler.contoso.com:444/",
                expectedBreezeEndpoint: Constants.DefaultIngestionEndpoint,
                expectedLiveMetricsEndpoint: Constants.DefaultLiveMetricsEndpoint,
                expectedProfilerEndpoint: "http://custom.profiler.contoso.com:444/",
                expectedSnapshotEndpoint: Constants.DefaultSnapshotEndpoint);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Connection String Invalid: The value for IngestionEndpoint is invalid.")]
        public void TestExpliticOverride_InvalidValue()
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https:////custom.profiler.contoso.com"
            };

            endpoint.GetEndpoint(EndpointName.Ingestion);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Connection String Invalid: The value for IngestionEndpoint is invalid.")]
        public void TestExpliticOverride_InvalidValue2()
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://www.~!@#$%&^*()_{}{}><?<?>:L\":\"_+_+_"
            };

            endpoint.GetEndpoint(EndpointName.Ingestion);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Connection String Invalid: The value for EndpointSuffix is invalid.")]
        public void TestExpliticOverride_InvalidValue3()
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=~!@#$%&^*()_{}{}><?<?>:L\":\"_+_+_"
            };

            endpoint.GetEndpoint(EndpointName.Ingestion);
        }


        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Connection String Invalid: Location must not contain special characters.")]
        public void TestExpliticOverride_InvalidLocation()
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;EndpointSuffix=contoso.com;Location=~!@#$%&^*()_{}{}><?<?>:L\":\"_+_+_"
            };

            endpoint.GetEndpoint(EndpointName.Ingestion);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Connection String Invalid: InstrumentationKey is required.")]
        public void TestEndpointProvider_NoInstrumentationKey()
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = "key1=value1;key2=value2;key3=value3"
            };

            endpoint.GetInstrumentationKey();
        }

        [TestMethod]
        public void TestEndpointProvider_NoConnectionStringShouldReturnDefaultEndpoints()
        {
            var endpoint = new EndpointProvider();

            Assert.AreEqual(Constants.DefaultIngestionEndpoint, endpoint.GetEndpoint(EndpointName.Ingestion).AbsoluteUri);
            Assert.AreEqual(Constants.DefaultLiveMetricsEndpoint, endpoint.GetEndpoint(EndpointName.Live).AbsoluteUri);
            Assert.AreEqual(Constants.DefaultProfilerEndpoint, endpoint.GetEndpoint(EndpointName.Profiler).AbsoluteUri);
            Assert.AreEqual(Constants.DefaultSnapshotEndpoint, endpoint.GetEndpoint(EndpointName.Snapshot).AbsoluteUri);
        }

        private void RunTest(string connectionString, string expectedBreezeEndpoint, string expectedLiveMetricsEndpoint, string expectedProfilerEndpoint, string expectedSnapshotEndpoint)
        {
            var endpoint = new EndpointProvider()
            {
                ConnectionString = connectionString
            };

            var breezeTest = endpoint.GetEndpoint(EndpointName.Ingestion);
            Assert.AreEqual(expectedBreezeEndpoint, breezeTest.AbsoluteUri);

            var liveMetricsTest = endpoint.GetEndpoint(EndpointName.Live);
            Assert.AreEqual(expectedLiveMetricsEndpoint, liveMetricsTest.AbsoluteUri);

            var profilerTest = endpoint.GetEndpoint(EndpointName.Profiler);
            Assert.AreEqual(expectedProfilerEndpoint, profilerTest.AbsoluteUri);

            var snapshotTest = endpoint.GetEndpoint(EndpointName.Snapshot);
            Assert.AreEqual(expectedSnapshotEndpoint, snapshotTest.AbsoluteUri);
        }
    }
}
