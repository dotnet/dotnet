namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebEventSourceTest
    {
#if !NET5_0_OR_GREATER // TODO: WHY DOES THIS NOT WORK?
        [TestMethod]
        public void MethodsAreImplementedConsistentlyWithTheirAttributes()
        {
            EventSourceTest.MethodsAreImplementedConsistentlyWithTheirAttributes(TelemetryChannelEventSource.Log);
        }
#endif
    }
}
