namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyTargetDimesnionExtractorTest
    {
        [TestMethod]
        public void NullTarget()
        {
            var item = new DependencyTelemetry();
            var extractor = new TargetDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(string.Empty,extractedDimension);
        }

        [TestMethod]
        public void EmptyTarget()
        {
            var item = new DependencyTelemetry();
            item.Target = string.Empty;
            var extractor = new TargetDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual(string.Empty, extractedDimension);
        }

        [TestMethod]
        public void ActualTarget()
        {
            var item = new DependencyTelemetry();
            item.Target = "ExpectedTarget";
            var extractor = new TargetDimensionExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("ExpectedTarget", extractedDimension);
        }
    }
}
