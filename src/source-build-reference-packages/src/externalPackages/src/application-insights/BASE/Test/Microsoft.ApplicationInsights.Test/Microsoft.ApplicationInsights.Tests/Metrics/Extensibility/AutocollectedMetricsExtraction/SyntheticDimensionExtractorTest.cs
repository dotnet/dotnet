namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SyntheticDimensionExtractorTest
    {
        [TestMethod]
        public void NullSynthetic()
        {
            var item = new RequestTelemetry();
            var extractor = new SyntheticDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.FalseString, extractedDimension);
        }

        [TestMethod]
        public void EmptySynthetic()
        {
            var item = new RequestTelemetry();
            item.Context.Operation.SyntheticSource = string.Empty;
            var extractor = new SyntheticDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.FalseString, extractedDimension);
        }

        [TestMethod]
        public void Synthetic()
        {
            var item = new RequestTelemetry();
            item.Context.Operation.SyntheticSource = "SomeSource";
            var extractor = new SyntheticDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(bool.TrueString, extractedDimension);
        }
    }
}
