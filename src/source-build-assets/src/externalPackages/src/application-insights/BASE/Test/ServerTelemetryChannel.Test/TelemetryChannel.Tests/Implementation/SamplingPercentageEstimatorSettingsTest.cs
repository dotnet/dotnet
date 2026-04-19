namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SamplingPercentageEstimatorSettingsTest
    {
        [TestMethod]
        public void MaxTelemetryItemsPerSecondAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();
            settings.MaxTelemetryItemsPerSecond = -10;

            Assert.AreEqual(1E-12, settings.EffectiveMaxTelemetryItemsPerSecond, 12);
        }

        [TestMethod]
        public void MinSamplingRateAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();

            settings.MaxSamplingPercentage = 200;
            Assert.AreEqual(1, settings.EffectiveMinSamplingRate);

            settings.MaxSamplingPercentage = -1;
            Assert.AreEqual(1E8, settings.EffectiveMinSamplingRate);
        }

        [TestMethod]
        public void MaxSamplingRateAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();

            settings.MinSamplingPercentage = 200;
            Assert.AreEqual(1, settings.EffectiveMaxSamplingRate);

            settings.MinSamplingPercentage = -1;
            Assert.AreEqual(1E8, settings.EffectiveMaxSamplingRate);
        }

        [TestMethod]
        public void EvaluationIntervalSecondsAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();
            settings.EvaluationInterval = TimeSpan.Zero;

            Assert.AreEqual(TimeSpan.FromSeconds(15), settings.EffectiveEvaluationInterval);
        }

        [TestMethod]
        public void SamplingPercentageDecreaseTimeoutSecondsAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();
            settings.SamplingPercentageDecreaseTimeout = TimeSpan.Zero;

            Assert.AreEqual(TimeSpan.FromMinutes(2), settings.EffectiveSamplingPercentageDecreaseTimeout);
        }

        [TestMethod]
        public void SamplingPercentageIncreaseTimeoutSecondsAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();
            settings.SamplingPercentageIncreaseTimeout = TimeSpan.Zero;

            Assert.AreEqual(TimeSpan.FromMinutes(15), settings.EffectiveSamplingPercentageIncreaseTimeout);
        }

        [TestMethod]
        public void MovingAverageRatioSecondsAdjustedIfSetToIncorrectValue()
        {
            var settings = new SamplingPercentageEstimatorSettings();
            settings.MovingAverageRatio = -3;

            Assert.AreEqual(0.25, settings.EffectiveMovingAverageRatio, 12);
        }
    }
}
