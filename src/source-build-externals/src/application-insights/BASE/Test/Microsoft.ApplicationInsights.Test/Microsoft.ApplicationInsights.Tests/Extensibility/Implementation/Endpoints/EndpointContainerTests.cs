namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("Endpoints")]
    public class EndpointContainerTests
    {
        [TestMethod]
        public void VerifyContainer_WithoutConnectionStringShouldReturnDefaultEndpoints()
        {
            var container = new EndpointContainer(new EndpointProvider());

            Assert.AreEqual(Constants.DefaultIngestionEndpoint, container.Ingestion.AbsoluteUri);
            Assert.AreEqual(Constants.DefaultLiveMetricsEndpoint, container.Live.AbsoluteUri);
            Assert.AreEqual(Constants.DefaultProfilerEndpoint, container.Profiler.AbsoluteUri);
            Assert.AreEqual(Constants.DefaultSnapshotEndpoint, container.Snapshot.AbsoluteUri);
        }
    }
}
