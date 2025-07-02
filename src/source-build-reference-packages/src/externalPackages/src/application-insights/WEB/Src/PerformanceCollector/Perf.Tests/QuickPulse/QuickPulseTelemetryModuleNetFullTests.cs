#if NET452
namespace Microsoft.ApplicationInsights.Tests.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuickPulseTelemetryModuleNetFullTests
    {
        private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.config");

        [TestMethod]
        public void QuickPulseTelemetryModuleInteractsWithTelemetryProcessorCorrectlyWhenLoadedBySdkModuleFirst()
        {
            // ARRANGE
            var configXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
  <InstrumentationKey>some ikey</InstrumentationKey>
  <TelemetryModules>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryModules>
  <TelemetryProcessors>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryProcessors>
</ApplicationInsights>";

            File.WriteAllText(this.configFilePath, configXml);

            // ACT
            var config = TelemetryConfiguration.Active;

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module);

            var telemetryProcessor = config.TelemetryProcessors.OfType<QuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(telemetryProcessor);

            Assert.AreEqual(telemetryProcessor, module.TelemetryProcessors.SingleOrDefault());
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleInteractsWithTelemetryProcessorCorrectlyWhenLoadedBySdkProcessorFirst()
        {
            // ARRANGE
            var configXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
  <InstrumentationKey>some ikey</InstrumentationKey>
  <TelemetryProcessors>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryProcessors>
  <TelemetryModules>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryModules>
</ApplicationInsights>";

            File.WriteAllText(this.configFilePath, configXml);

            // ACT
            var config = TelemetryConfiguration.Active;

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();
            Assert.IsNotNull(module);

            var telemetryProcessor = config.TelemetryProcessors.OfType<QuickPulseTelemetryProcessor>().SingleOrDefault();
            Assert.IsNotNull(telemetryProcessor);

            Assert.AreEqual(telemetryProcessor, module.TelemetryProcessors.SingleOrDefault());
        }

        [TestMethod]
        public void QuickPulseTelemetryModuleSupportsMultipleTelemetryProcessorsForSingleConfiguration()
        {
            // ARRANGE
            var configXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
  <InstrumentationKey>some ikey</InstrumentationKey>
  <TelemetryModules>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryModule, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryModules>
  <TelemetryProcessors>
    <Add Type=""Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse.QuickPulseTelemetryProcessor, Microsoft.AI.PerfCounterCollector""/>
  </TelemetryProcessors>
</ApplicationInsights>";

            File.WriteAllText(this.configFilePath, configXml);

            var config = TelemetryConfiguration.Active;

            // ACT
            var telemetryProcessors = new List<IQuickPulseTelemetryProcessor>();
            const int TelemetryProcessorCount = 4;
            for (int i = 0; i < TelemetryProcessorCount; i++)
            {
                // this recreates config.TelemetryProcessors collection, and all its members are reinstantiated
                var builder = config.TelemetryProcessorChainBuilder;
                builder = builder.Use(current => new SimpleTelemetryProcessorSpy());
                builder.Build();

                telemetryProcessors.Add(config.TelemetryProcessors.OfType<QuickPulseTelemetryProcessor>().Single());
            }

            // ASSERT
            var module = TelemetryModules.Instance.Modules.OfType<QuickPulseTelemetryModule>().SingleOrDefault();

            Assert.AreEqual(TelemetryProcessorCount + 1, module.TelemetryProcessors.Count); // one was there after the initial configuration loading
            Assert.IsTrue(telemetryProcessors.TrueForAll(module.TelemetryProcessors.Contains));
        }
    }
}
#endif