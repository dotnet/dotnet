namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RequestResponseCodeDimensionExtractorTest
    {
        [TestMethod]
        public void NullResponseCode()
        {
            var item = new RequestTelemetry();
            var extractor = new RequestResponseCodeDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(string.Empty,extractedDimension);
        }

        [TestMethod]
        public void EmptyResponseCode()
        {
            var item = new RequestTelemetry();
            item.ResponseCode = string.Empty;
            var extractor = new RequestResponseCodeDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(string.Empty, extractedDimension);
        }

        [TestMethod]
        public void ActualTarget()
        {
            var item = new RequestTelemetry();
            item.ResponseCode = "ExpectedResponseCode";
            var extractor = new RequestResponseCodeDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("ExpectedResponseCode", extractedDimension);
        }
    }
}
