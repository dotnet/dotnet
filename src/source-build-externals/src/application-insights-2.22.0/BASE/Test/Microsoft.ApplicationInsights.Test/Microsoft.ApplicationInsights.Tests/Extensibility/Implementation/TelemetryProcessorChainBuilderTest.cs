namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Text;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TelemetryProcessorChainBuilderTest
    {
        [TestMethod]
        public void NoExceptionOnReturningNullFromUse()
        {
            var configuration = new TelemetryConfiguration();

            var builder = new TelemetryProcessorChainBuilder(configuration);
            builder.Use(next => null);

            builder.Build();

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count); // Transmission is added by default
        }

        [TestMethod]
        public void NullProcessorsAreSkipped()
        {
            var configuration = new TelemetryConfiguration();

            var builder = new TelemetryProcessorChainBuilder(configuration);
            builder.Use(next => new StubTelemetryProcessor(next));
            builder.Use(next => null);
            builder.Use(next => new StubTelemetryProcessor(next));

            builder.Build();

            Assert.AreEqual(3, configuration.TelemetryProcessors.Count); // Transmission is added by default
            Assert.AreSame(((StubTelemetryProcessor)configuration.TelemetryProcessors[0]).next, ((StubTelemetryProcessor)configuration.TelemetryProcessors[1]));
        }

        [TestMethod]
        public void TransmissionProcessorIsAddedDefaultWhenNoOtherTelemetryProcessorsAreConfigured()
        {
            var config = new TelemetryConfiguration();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Build();
            AssertEx.IsType<TransmissionProcessor>(config.DefaultTelemetrySink.TelemetryProcessorChain.FirstTelemetryProcessor);
        }

        [TestMethod]
        public void UsesTelemetryProcessorGivenInUseToBuild()
        {
            var config = new TelemetryConfiguration();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Use(next => new StubTelemetryProcessor(next));
            builder.Build();
            AssertEx.IsType<StubTelemetryProcessor>(config.TelemetryProcessorChain.FirstTelemetryProcessor);
        }

        [TestMethod]
        public void BuildUsesTelemetryProcesorFactoryOnEachCall()
        {
            var tc1 = new TelemetryConfiguration();
            var tc2 = new TelemetryConfiguration();
            var builder1 = new TelemetryProcessorChainBuilder(tc1);
            builder1.Use((next) => new StubTelemetryProcessor(next));
            builder1.Build();

            var builder2 = new TelemetryProcessorChainBuilder(tc2);
            builder2.Use((next) => new StubTelemetryProcessor(next));
            builder2.Build();

            Assert.AreNotSame(tc1.TelemetryProcessors, tc2.TelemetryProcessors);
        }

        [TestMethod]
        public void BuildOrdersTelemetryChannelsInOrderOfUseCalls()
        {
            var config = new TelemetryConfiguration(string.Empty, new StubTelemetryChannel());
            StringBuilder outputCollector = new StringBuilder();
            var builder = new TelemetryProcessorChainBuilder(config);
            builder.Use((next) => new StubTelemetryProcessor(next) { OnProcess = (item) => { outputCollector.Append("processor1"); } });
            builder.Use((next) => new StubTelemetryProcessor(next) { OnProcess = (item) => { outputCollector.Append("processor2"); } });
            builder.Build();
            config.TelemetryProcessorChain.Process(new StubTelemetry());

            Assert.AreEqual("processor1processor2", outputCollector.ToString());
        }

        [TestMethod]
        public void BuildWillInitializeModules()
        {
            var tc1 = new TelemetryConfiguration();
            var builder1 = new TelemetryProcessorChainBuilder(tc1);
            builder1.Use((next) => new MockProcessorModule());
            builder1.Build();

            Assert.AreEqual(2, tc1.TelemetryProcessors.Count); // Transmission is added by default
            Assert.IsTrue(((MockProcessorModule)tc1.TelemetryProcessors[0]).ModuleInitialized, "Module was not initialized.");
        }
    }
}
