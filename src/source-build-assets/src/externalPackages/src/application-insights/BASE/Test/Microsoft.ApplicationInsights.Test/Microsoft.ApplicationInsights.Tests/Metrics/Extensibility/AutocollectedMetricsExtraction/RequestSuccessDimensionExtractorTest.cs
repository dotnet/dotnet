namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RequestSuccessDimensionExtractorTest
    {
        [TestMethod]
        public void NullSuccess()
        {
            var item = new RequestTelemetry();
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }

        [TestMethod]
        public void TrueSucess()
        {
            var item = new RequestTelemetry();
            item.Success = true;
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }

        [TestMethod]
        public void FalseSucess()
        {
            var item = new RequestTelemetry();
            item.Success = false;
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.FalseString, extractedDimension);
        }

        [TestMethod]
        public void Null()
        {
            var item = new RequestTelemetry();
            item.Success = null;
            var extractor = new RequestSuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
    }
}
