namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Metrics
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class RequestDurationBucketExtractorTest
    {
        [TestMethod]
        public void RequestDurationBucket()
        {
            var item = new RequestTelemetry();            
            var extractor = new DurationBucketExtractor();

            Dictionary<int, string> durationAndBucket = new Dictionary<int, string>();
            durationAndBucket.Add(0, "<250ms");
            durationAndBucket.Add(249, "<250ms");
            durationAndBucket.Add(250, "250ms-500ms");
            durationAndBucket.Add(499, "250ms-500ms");
            durationAndBucket.Add(500, "500ms-1sec");
            durationAndBucket.Add(999, "500ms-1sec");
            durationAndBucket.Add(1000, "1sec-3sec");
            durationAndBucket.Add(2999, "1sec-3sec");
            durationAndBucket.Add(3000, "3sec-7sec");
            durationAndBucket.Add(6000, "3sec-7sec");
            durationAndBucket.Add(6999, "3sec-7sec");
            durationAndBucket.Add(7000, "7sec-15sec");
            durationAndBucket.Add(14000, "7sec-15sec");
            durationAndBucket.Add(14999, "7sec-15sec");
            durationAndBucket.Add(15000, "15sec-30sec");
            durationAndBucket.Add(20000, "15sec-30sec");
            durationAndBucket.Add(29999, "15sec-30sec");
            durationAndBucket.Add(30000, "30sec-1min");
            durationAndBucket.Add(59000, "30sec-1min");
            durationAndBucket.Add(59999, "30sec-1min");            
            durationAndBucket.Add(60000, "1min-2min");
            durationAndBucket.Add(119999, "1min-2min");
            durationAndBucket.Add(120000, "2min-5min");
            durationAndBucket.Add(240000, "2min-5min");
            durationAndBucket.Add(300000, ">=5min");
            durationAndBucket.Add(int.MaxValue -1 , ">=5min");
            durationAndBucket.Add(int.MaxValue, ">=5min");

            foreach (var entry in durationAndBucket)
            {
                item.Duration = TimeSpan.FromMilliseconds(entry.Key);
                var extractedDimension = extractor.ExtractDimension(item);
                Assert.AreEqual(entry.Value, extractedDimension, "duration:" + entry.Key);
            }
        }

        [TestMethod]
        public void RequestDurationEmptyBucket()
        {
            var item = new RequestTelemetry();
            var extractor = new DurationBucketExtractor();
            var extractedDimension = extractor.ExtractDimension(item);
            Assert.AreEqual("<250ms", extractedDimension);
        }
    }
}
