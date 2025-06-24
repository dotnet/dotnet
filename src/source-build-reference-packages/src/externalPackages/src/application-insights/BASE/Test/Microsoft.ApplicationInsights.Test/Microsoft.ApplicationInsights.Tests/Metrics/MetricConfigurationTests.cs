using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.ApplicationInsights.Metrics.Extensibility;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.Metrics
{
    /// <summary />
    [TestClass]
    public class MetricConfigurationTests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            {
                var config = new MetricConfiguration(
                                    seriesCountLimit: 1000,
                                    valuesPerDimensionLimit: 100,
                                    seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Assert.IsFalse(config.ApplyDimensionCapping);
            }

            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimit:    100,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Assert.IsNotNull(config);
                Assert.AreEqual(1000, config.SeriesCountLimit);
                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(10));
                Assert.IsNotNull(config.SeriesConfig);
                Assert.IsInstanceOfType(config.SeriesConfig, typeof(MetricSeriesConfigurationForMeasurement));
                Assert.AreEqual(false, config.SeriesConfig.RequiresPersistentAggregation);
                Assert.AreEqual(false, ((MetricSeriesConfigurationForMeasurement) config.SeriesConfig).RestrictToUInt32Values);
            }
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1,
                                    valuesPerDimensionLimit:    0,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true));
                Assert.IsNotNull(config);
                Assert.AreEqual(1, config.SeriesCountLimit);
                Assert.AreEqual(0, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(0, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(0, config.GetValuesPerDimensionLimit(10));
                Assert.IsNotNull(config.SeriesConfig);
                Assert.IsInstanceOfType(config.SeriesConfig, typeof(MetricSeriesConfigurationForMeasurement));
                Assert.AreEqual(false, config.SeriesConfig.RequiresPersistentAggregation);
                Assert.AreEqual(true, ((MetricSeriesConfigurationForMeasurement) config.SeriesConfig).RestrictToUInt32Values);
            }
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           Int32.MaxValue,
                                    valuesPerDimensionLimit:    Int32.MaxValue,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));
                Assert.IsNotNull(config);
                Assert.AreEqual(Int32.MaxValue, config.SeriesCountLimit);
                Assert.AreEqual(Int32.MaxValue, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(Int32.MaxValue, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(Int32.MaxValue, config.GetValuesPerDimensionLimit(10));
                Assert.IsNotNull(config.SeriesConfig);
                Assert.IsInstanceOfType(config.SeriesConfig, typeof(MetricSeriesConfigurationForMeasurement));
                Assert.AreEqual(false, config.SeriesConfig.RequiresPersistentAggregation);
                Assert.AreEqual(false, ((MetricSeriesConfigurationForMeasurement) config.SeriesConfig).RestrictToUInt32Values);
            }
            { 
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => new MetricConfiguration(
                                    seriesCountLimit:           0,
                                    valuesPerDimensionLimit:    100,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false)) );
            }
            { 
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => new MetricConfiguration(
                                    seriesCountLimit:           -1,
                                    valuesPerDimensionLimit:    100,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false)) );
            }
            { 
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimit:    -1,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false)) );
            }
            {
                Assert.ThrowsException<ArgumentNullException>(() => new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimit:    100,
                                    seriesConfig:               null) );
            }
        }

        /// <summary />
        [TestMethod]
        public void SeriesCountLimit()
        {
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimit:    100,
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(-1) );
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(0) );

                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(100, config.GetValuesPerDimensionLimit(10));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(11) );
            }
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimits:   new[] { 42 },
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(-1) );
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(0) );

                Assert.AreEqual(42, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(42, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(42, config.GetValuesPerDimensionLimit(10));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(11) );
            }
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimits:   new[] { 11, 12, 13, 14 },
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(-1) );
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(0) );

                Assert.AreEqual(11, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(12, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(13, config.GetValuesPerDimensionLimit(3));
                Assert.AreEqual(14, config.GetValuesPerDimensionLimit(4));
                Assert.AreEqual(14, config.GetValuesPerDimensionLimit(6));
                Assert.AreEqual(14, config.GetValuesPerDimensionLimit(10));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(11) );
            }
            { 
                var config = new MetricConfiguration(
                                    seriesCountLimit:           1000,
                                    valuesPerDimensionLimits:   new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 },
                                    seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(-1) );
                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(0) );

                Assert.AreEqual(11, config.GetValuesPerDimensionLimit(1));
                Assert.AreEqual(12, config.GetValuesPerDimensionLimit(2));
                Assert.AreEqual(13, config.GetValuesPerDimensionLimit(3));
                Assert.AreEqual(14, config.GetValuesPerDimensionLimit(4));
                Assert.AreEqual(15, config.GetValuesPerDimensionLimit(5));
                Assert.AreEqual(16, config.GetValuesPerDimensionLimit(6));
                Assert.AreEqual(17, config.GetValuesPerDimensionLimit(7));
                Assert.AreEqual(18, config.GetValuesPerDimensionLimit(8));
                Assert.AreEqual(19, config.GetValuesPerDimensionLimit(9));
                Assert.AreEqual(20, config.GetValuesPerDimensionLimit(10));

                Assert.ThrowsException<ArgumentOutOfRangeException>( () => config.GetValuesPerDimensionLimit(11) );
            }
        }

        /// <summary />
        [TestMethod]
        public void ValuesPerDimensionLimit()
        {
            // Covered in this test:
            Ctor();
        }

        /// <summary />
        [TestMethod]
        public void SeriesConfig()
        {
            // Covered in this test:
            Ctor();
        }

        /// <summary />
        [TestMethod]
        public void TestEquals()
        {
            var config1 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config1));

            var config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config2));
            Assert.IsTrue(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true));

            Assert.IsFalse(config1.Equals(config2));
            Assert.IsFalse(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    101,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsFalse(config1.Equals(config2));
            Assert.IsFalse(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1001,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsFalse(config1.Equals(config2));
            Assert.IsFalse(config2.Equals(config1));

            config1 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 42 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    42,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config2));
            Assert.IsTrue(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 42, 42, 42 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config2));
            Assert.IsTrue(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 42, 18, 42 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsFalse(config1.Equals(config2));
            Assert.IsFalse(config2.Equals(config1));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config2));
            Assert.IsTrue(config2.Equals(config1));

            config1 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 30 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 40 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsTrue(config1.Equals(config2));
            Assert.IsTrue(config2.Equals(config1));

            config1 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 25, 30 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimits:   new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 26, 40 },
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.IsFalse(config1.Equals(config2));
            Assert.IsFalse(config2.Equals(config1));
        }

        /// <summary />
        [TestMethod]
        public void TestGetHashCode()
        {
            var config1 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(0, config1.GetHashCode());

            var config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1000,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: true));

            Assert.AreNotEqual(config1.GetHashCode(), config2.GetHashCode());
            Assert.AreNotEqual(0, config2.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit:           1001,
                                valuesPerDimensionLimit:    100,
                                seriesConfig:               new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(config1.GetHashCode(), config2.GetHashCode());
            Assert.AreNotEqual(0, config2.GetHashCode());

            config1 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 42 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(0, config1.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimit: 42,
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 42, 42, 42 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 42, 18, 42 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(0, config2.GetHashCode());
            Assert.AreNotEqual(config1.GetHashCode(), config2.GetHashCode());

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());

            config1 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 30 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 40 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(0, config1.GetHashCode());
            Assert.AreNotEqual(0, config2.GetHashCode());
            Assert.AreEqual(config1.GetHashCode(), config2.GetHashCode());

            config1 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 25, 30 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            config2 = new MetricConfiguration(
                                seriesCountLimit: 1000,
                                valuesPerDimensionLimits: new[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 26, 40 },
                                seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

            Assert.AreNotEqual(0, config1.GetHashCode());
            Assert.AreNotEqual(0, config2.GetHashCode());
            Assert.AreNotEqual(config1.GetHashCode(), config2.GetHashCode());
        }
    }
}
