namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CloudRoleNameDimensionExtractorTest
    {
        [TestMethod]
        public void NullRoleName()
        {
            var item = new RequestTelemetry();
            var extractor = new CloudRoleNameDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.IsNull(extractedDimension);
        }

        [TestMethod]
        public void EmptyRoleName()
        {
            var item = new RequestTelemetry();
            item.Context.Cloud.RoleName = string.Empty;
            var extractor = new CloudRoleNameDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.IsNull(extractedDimension);
        }

        [TestMethod]
        public void RoleName()
        {
            var item = new RequestTelemetry();
            item.Context.Cloud.RoleName = "ExpectedRoleName";
            var extractor = new CloudRoleNameDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("ExpectedRoleName", extractedDimension);
        }
    }
}
