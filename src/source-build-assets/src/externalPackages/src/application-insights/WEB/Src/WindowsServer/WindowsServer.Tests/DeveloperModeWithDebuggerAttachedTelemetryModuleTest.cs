namespace Microsoft.ApplicationInsights.WindowsServer
{
    using Microsoft.ApplicationInsights.Extensibility;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class DeveloperModeWithDebuggerAttachedTelemetryModuleTest
    {
        [TestMethod]
        public void DevModeModuleValidateValueNotOverridden()
        {
            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.TelemetryChannel.DeveloperMode = true;
            DeveloperModeWithDebuggerAttachedTelemetryModule module = new DeveloperModeWithDebuggerAttachedTelemetryModule();
            module.Initialize(telemetryConfiguration);
            Assert.AreEqual(true, telemetryConfiguration.TelemetryChannel.DeveloperMode);

            telemetryConfiguration.TelemetryChannel.DeveloperMode = false;
            module.Initialize(telemetryConfiguration);
            Assert.AreEqual(false, telemetryConfiguration.TelemetryChannel.DeveloperMode);
        }

        [TestMethod]
        public void DevModeModuleValidateValueSetToDebuggerAttachedValueWhenNull()
        {
            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.TelemetryChannel.DeveloperMode = null;

            DeveloperModeWithDebuggerAttachedTelemetryModule.IsDebuggerAttached = () => true;
            DeveloperModeWithDebuggerAttachedTelemetryModule module = new DeveloperModeWithDebuggerAttachedTelemetryModule();
            module.Initialize(telemetryConfiguration);
            Assert.AreEqual(true, telemetryConfiguration.TelemetryChannel.DeveloperMode);

            telemetryConfiguration.TelemetryChannel.DeveloperMode = null;
            DeveloperModeWithDebuggerAttachedTelemetryModule.IsDebuggerAttached = () => false;
            module = new DeveloperModeWithDebuggerAttachedTelemetryModule();
            module.Initialize(telemetryConfiguration);
            
            Assert.AreEqual(null, telemetryConfiguration.TelemetryChannel.DeveloperMode);
        }
    }
}
