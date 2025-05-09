namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class QuickPulseDataAccumulatorTests
    {
        private const long MaxCount = 524287;
        private const long MaxDuration = 17592186044415;

        [TestMethod]
        public void QuickPulseDataAccumulatorEncodesCountAndDuration()
        {
            // ARRANGE
            long count = 42;
            long duration = 102;

            // ACT
            long encodedValue = QuickPulseDataAccumulator.EncodeCountAndDuration(count, duration);
            Tuple<long, long> decodedValues = QuickPulseDataAccumulator.DecodeCountAndDuration(encodedValue);

            // ASSERT
            Assert.AreEqual(count, decodedValues.Item1);
            Assert.AreEqual(duration, decodedValues.Item2);
        }

        [TestMethod]
        public void QuickPulseDataAccumulatorEncodesCountAndDurationMaxValues()
        {
            // ARRANGE
            
            // ACT
            long encodedValue = QuickPulseDataAccumulator.EncodeCountAndDuration(MaxCount, MaxDuration);
            Tuple<long, long> decodedValues = QuickPulseDataAccumulator.DecodeCountAndDuration(encodedValue);

            // ASSERT
            Assert.AreEqual(MaxCount, decodedValues.Item1);
            Assert.AreEqual(MaxDuration, decodedValues.Item2);
        }

        [TestMethod]
        public void QuickPulseDataAccumulatorEncodesCountAndDurationOverflowCount()
        {
            // ARRANGE
         
            // ACT
            long encodedValue = QuickPulseDataAccumulator.EncodeCountAndDuration(MaxCount + 1, MaxDuration);
            Tuple<long, long> decodedValues = QuickPulseDataAccumulator.DecodeCountAndDuration(encodedValue);

            // ASSERT
            Assert.AreEqual(0, decodedValues.Item1);
            Assert.AreEqual(0, decodedValues.Item2);
        }

        [TestMethod]
        public void QuickPulseDataAccumulatorEncodesCountAndDurationOverflowDuration()
        {
            // ARRANGE

            // ACT
            long encodedValue = QuickPulseDataAccumulator.EncodeCountAndDuration(MaxCount, MaxDuration + 1);
            Tuple<long, long> decodedValues = QuickPulseDataAccumulator.DecodeCountAndDuration(encodedValue);

            // ASSERT
            Assert.AreEqual(0, decodedValues.Item1);
            Assert.AreEqual(0, decodedValues.Item2);
        }

        [TestMethod]
        public void QuickPulseDataAccumulatorCollectsTelemetryItemsInThreadSafeManner()
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            var accumulator =
                new QuickPulseDataAccumulator(
                    new CollectionConfiguration(
                        new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                        out errors,
                        new ClockMock()));

            // ACT
            var iterationCount = 1000;
            var concurrency = 12;

            Action addItemTask =
                () =>
                Enumerable.Range(0, iterationCount)
                    .ToList()
                    .ForEach(
                        i => accumulator.TelemetryDocuments.Push(new RequestTelemetryDocument()
                                                                     {
                                                                         Name = i.ToString(CultureInfo.InvariantCulture)
                                                                     }));

            var tasks = new List<Action>();
            for (int i = 0; i < concurrency; i++)
            {
                tasks.Add(addItemTask);
            }

            Parallel.Invoke(new ParallelOptions() { MaxDegreeOfParallelism = concurrency }, tasks.ToArray());

            // ASSERT
            var dict = new Dictionary<int, int>();
            foreach (var item in accumulator.TelemetryDocuments)
            {
                int requestNumber = int.Parse(((RequestTelemetryDocument)item).Name, CultureInfo.InvariantCulture);
                if (dict.ContainsKey(requestNumber))
                {
                    dict[requestNumber]++;
                }
                else
                {
                    dict[requestNumber] = 1;
                }
            }

            Assert.AreEqual(iterationCount, dict.Count);
            Assert.IsTrue(dict.All(pair => pair.Value == concurrency));
        }
    }
}
