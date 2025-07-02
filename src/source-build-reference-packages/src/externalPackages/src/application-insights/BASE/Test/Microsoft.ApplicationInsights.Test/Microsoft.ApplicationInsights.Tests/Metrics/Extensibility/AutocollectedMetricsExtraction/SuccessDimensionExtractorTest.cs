namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SuccessDimensionExtractorTest
    {
        [TestMethod]
        public void NullSuccess()
        {
            var item = new DependencyTelemetry();
            var extractor = new SuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }

        [TestMethod]
        public void TrueSucess()
        {
            var item = new DependencyTelemetry();
            item.Success = true;
            var extractor = new SuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }

        [TestMethod]
        public void FalseSucess()
        {
            var item = new DependencyTelemetry();
            item.Success = false;
            var extractor = new SuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.FalseString, extractedDimension);
        }

        [TestMethod]
        public void Null()
        {
            var item = new DependencyTelemetry();
            item.Success = null;
            var extractor = new SuccessDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
    }
}
