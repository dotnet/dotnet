namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
#if NETFRAMEWORK
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PlatformReferencesTests
    {
        [TestMethod]
        public void NoSystemWebReferences()
        {
            // Validate Platform assembly
            foreach (var assembly in typeof(TelemetryDebugWriter).Assembly.GetReferencedAssemblies())
            {
                Assert.IsTrue(!assembly.FullName.Contains("System.Web"));
            }

            // Validate Core assembly
            foreach (var assembly in typeof(EventTelemetry).Assembly.GetReferencedAssemblies())
            {
                Assert.IsTrue(!assembly.FullName.Contains("System.Web"));
            }
        }
    }
#endif
}
