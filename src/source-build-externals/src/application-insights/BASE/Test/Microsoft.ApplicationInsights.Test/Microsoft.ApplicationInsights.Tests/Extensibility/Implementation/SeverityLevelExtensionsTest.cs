namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    
    using DpSeverityLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;
    
    [TestClass]
    public class SeverityLevelExtensionsTest
    {
        [TestMethod]
        public void TranslateSeverityLevelConvertsAllValueFromSdkToDp()
        {
            Assert.AreEqual(DpSeverityLevel.Verbose, SeverityLevelExtensions.TranslateSeverityLevel(SeverityLevel.Verbose));
            Assert.AreEqual(DpSeverityLevel.Warning, SeverityLevelExtensions.TranslateSeverityLevel(SeverityLevel.Warning));
            Assert.AreEqual(DpSeverityLevel.Information, SeverityLevelExtensions.TranslateSeverityLevel(SeverityLevel.Information));
            Assert.AreEqual(DpSeverityLevel.Error, SeverityLevelExtensions.TranslateSeverityLevel(SeverityLevel.Error));
            Assert.AreEqual(DpSeverityLevel.Critical, SeverityLevelExtensions.TranslateSeverityLevel(SeverityLevel.Critical));
        }

        [TestMethod]
        public void TranslateSeverityLevelConvertsAllValueFromDpToSdk()
        {
            Assert.AreEqual(SeverityLevel.Verbose, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Verbose));
            Assert.AreEqual(SeverityLevel.Warning, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Warning));
            Assert.AreEqual(SeverityLevel.Information, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Information));
            Assert.AreEqual(SeverityLevel.Error, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Error));
            Assert.AreEqual(SeverityLevel.Critical, SeverityLevelExtensions.TranslateSeverityLevel(DpSeverityLevel.Critical));
        }
    }
}
