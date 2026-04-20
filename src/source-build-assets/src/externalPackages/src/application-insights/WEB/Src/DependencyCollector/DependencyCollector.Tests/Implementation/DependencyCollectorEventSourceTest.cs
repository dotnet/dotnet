namespace Microsoft.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyCollectorEventSourceTest
    {
        [TestMethod]
        public void MethodsAreImplementedConsistentlyWithTheirAttributes()
        {
            EventSourceTest.MethodsAreImplementedConsistentlyWithTheirAttributes(DependencyCollectorEventSource.Log);
        }
    }
}
