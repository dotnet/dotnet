namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    [TestClass]
    public class TelemetryProcessorChainBuilderExtensionsTest
    {
        [TestMethod]
        public void UseSamplingThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseSampling(null, 5));
        }

        [TestMethod]
        public void UseSamplingSetsAddsSamplingProcessorToTheChainWithCorrectPercentage()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseSampling(5);
            channelBuilder.Build();

            Assert.AreEqual(5, ((SamplingTelemetryProcessor) tc.TelemetryProcessorChain.FirstTelemetryProcessor).SamplingPercentage);
        }

        [TestMethod]
        public void UseSamplingWithExcluedTypesParameterThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseSampling(null, 5, "request"));
        }

        [TestMethod]
        public void UseSamplingSetsAddsSamplingProcessorToTheChainWithCorrectExcludedTypes()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseSampling(5, "request");
            channelBuilder.Build();

            Assert.AreEqual(5, ((SamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).SamplingPercentage);
            Assert.AreEqual("request", ((SamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).ExcludedTypes);
        }

        [TestMethod]
        public void UseAdaptiveSamplingThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChain()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling();
            channelBuilder.Build();

            AssertEx.IsType<AdaptiveSamplingTelemetryProcessor>(tc.TelemetryProcessorChain.FirstTelemetryProcessor);
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithExcludedTypesThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null, "request"));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectExcludedTypes()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling("request");
            channelBuilder.Build();

            Assert.AreEqual("request", ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).ExcludedTypes);
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithMaxItemsParameterThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null, 5));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectMaxTelemetryItemsPerSecond()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(5);
            channelBuilder.Build();

            Assert.AreEqual(5, ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).MaxTelemetryItemsPerSecond);
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithMaxItemsAndExcludedTypesParametersThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null, 5, "request"));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectMaxTelemetryItemsPerSecondAndExcludedTypes()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(5, "request");
            channelBuilder.Build();

            Assert.AreEqual(5, ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).MaxTelemetryItemsPerSecond);
            Assert.AreEqual("request", ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).ExcludedTypes);
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithSettingsParameterThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null));
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithSettingsParameterThrowsArgumentNullExceptionSettingsIsNull()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);

            AssertEx.Throws<ArgumentNullException>(() => channelBuilder.UseAdaptiveSampling(default(SamplingPercentageEstimatorSettings), null));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectSettings()
        {
            SamplingPercentageEstimatorSettings settings = new SamplingPercentageEstimatorSettings
            {
                MaxSamplingPercentage = 13
            };
            AdaptiveSamplingPercentageEvaluatedCallback callback = (second, percentage, samplingPercentage, changed, estimatorSettings) => { };

            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(settings, callback);
            channelBuilder.Build();

            Assert.AreEqual(13, ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).MaxSamplingPercentage);
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithSettingsParameterAndExcludedTypesThrowsArgumentNullExceptionBuilderIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() => TelemetryProcessorChainBuilderExtensions.UseAdaptiveSampling(null, null, null, "request"));
        }

        [TestMethod]
        public void UseAdaptiveSamplingWithSettingsParameterAndExcludedTypesThrowsArgumentNullExceptionSettingsIsNull()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);

            AssertEx.Throws<ArgumentNullException>(() => channelBuilder.UseAdaptiveSampling(null, null, "request"));
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectSettingsAndExcludedTypes()
        {
            SamplingPercentageEstimatorSettings settings = new SamplingPercentageEstimatorSettings
            {
                MaxSamplingPercentage = 13
            };
            AdaptiveSamplingPercentageEvaluatedCallback callback = (second, percentage, samplingPercentage, changed, estimatorSettings) => { };

            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(settings, callback, "request");
            channelBuilder.Build();

            Assert.AreEqual(13, ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).MaxSamplingPercentage);
            Assert.AreEqual("request", ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).ExcludedTypes);
        }

        [TestMethod]
        public void UseAdaptiveSamplingAddsAdaptiveSamplingProcessorToTheChainWithCorrectSettingsAndIncludedTypes()
        {
            SamplingPercentageEstimatorSettings settings = new SamplingPercentageEstimatorSettings
            {
                MaxSamplingPercentage = 13
            };
            AdaptiveSamplingPercentageEvaluatedCallback callback = (second, percentage, samplingPercentage, changed, estimatorSettings) => { };

            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(settings, callback, null, "request");
            channelBuilder.Build();

            Assert.AreEqual(13, ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).MaxSamplingPercentage);
            Assert.AreEqual("request", ((AdaptiveSamplingTelemetryProcessor)tc.TelemetryProcessorChain.FirstTelemetryProcessor).IncludedTypes);
        }
    }
}
