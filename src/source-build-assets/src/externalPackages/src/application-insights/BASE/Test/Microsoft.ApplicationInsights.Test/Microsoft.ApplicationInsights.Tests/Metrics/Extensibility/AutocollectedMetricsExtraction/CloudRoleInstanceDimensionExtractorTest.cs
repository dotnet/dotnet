namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CloudRoleInstanceDimensionExtractorTest
    {
        [TestMethod]
        public void NullRoleInstance()
        {
            var item = new RequestTelemetry();
            var extractor = new CloudRoleInstanceDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.IsNull(extractedDimension);
        }

        [TestMethod]
        public void EmptyRoleInstance()
        {
            var item = new RequestTelemetry();
            item.Context.Cloud.RoleInstance = string.Empty;
            var extractor = new CloudRoleInstanceDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.IsNull(extractedDimension);
        }

        [TestMethod]
        public void RoleInstance()
        {
            var item = new RequestTelemetry();
            item.Context.Cloud.RoleInstance = "ExpectedRoleInstance";
            var extractor = new CloudRoleInstanceDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("ExpectedRoleInstance", extractedDimension);
        }
    }
}
