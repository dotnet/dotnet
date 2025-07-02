namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.Channel;

    [TestClass]
    public class SamplingScoreGeneratorTest
    {
        private static readonly Random Rand = new Random();

        // We have a predefined set of strings and their hash values
        // the test allows us to make sure we can produce the same hashing
        // results in different versions of sdk
        private static Dictionary<string, int> StringHash = new Dictionary<string, int>()
                                                 {
                                                    { "ss", 1179811869},
                                                    {"kxi", 34202699},
                                                    {"wr", 1281077591},
                                                    {"ynehgfhyuiltaiqovbpyhpm", 2139623659},
                                                    {"iaxxtklcw", 1941943012},
                                                    {"hjwvqjiiwhoxrtsjma", 1824011880},
                                                    {"rpiauyg", 251412007},
                                                    {"jekvjvh", 9189387},
                                                    {"hq", 1807146729},
                                                    {"kgqxrftjhefkwlufcxibwjcy", 270215819},
                                                    {"lkfc", 1228617029},
                                                    {"skrnpybqqu", 223230949},
                                                    {"px", 70671963},
                                                    {"dtn", 904623389},
                                                    {"nqfcxobaequ", 397313566},
                                                    {"togxlt", 948170633},
                                                    {"jvvdkhnahkaujxarkd", 1486894898},
                                                    {"mcloukvkamiaqja", 56804453},
                                                    {"ornuu", 1588005865},
                                                    {"otodvlhtvu", 1544494884},
                                                    {"uhpwhasnvmnykjkitla", 981289895},
                                                    {"itbnryqnjcgpmjemdghqtg", 1469591400},
                                                    {"wauetkdnivwlafbfhiedsfx", 2114415420},
                                                    {"fniwmeidbvd", 508699380},
                                                    {"vuwdgoxspstvj", 1821547235},
                                                    {"y", 1406544563},
                                                    {"pceqcixfb", 1282453766},
                                                    {"aentke", 255756533},
                                                    {"ni", 1696510239},
                                                    {"lbwehevltlnl", 1466602040},
                                                    {"ymxql", 1974582171},
                                                    {"mvqbaosfuip", 1560556398},
                                                    {"urmwofajwmmlornynglm", 701710403},
                                                    {"buptyvonyacerrt", 1315240646},
                                                    {"cxsqcnyieliatqnwc", 76148095},
                                                    {"svvco", 1849105799},
                                                    {"luwmjhwyt", 553630912},
                                                    {"lisvmmug", 822987687},
                                                    {"mmntilfbmxwuyij", 882214597},
                                                    {"hqmyv", 1510970959}
                                                 };

        [TestMethod]
        public void SamplingScoreIsntChangedByUserId()
        {
            string opId = GenerateRandomOperaitonId();

            var eventTelemetry = new EventTelemetry();
            eventTelemetry.Context.Operation.Id = opId;
            eventTelemetry.Context.User.Id = GenerateRandomUserId();

            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Operation.Id = opId;
            requestTelemetry.Context.User.Id = GenerateRandomUserId();

            var eventTelemetrySamplingScoreNoUserId = SamplingScoreGenerator.GetSamplingScore(eventTelemetry);
            var requestTelemetrySamplingScoreNoUserId = SamplingScoreGenerator.GetSamplingScore(requestTelemetry);

            Assert.AreEqual(eventTelemetrySamplingScoreNoUserId, requestTelemetrySamplingScoreNoUserId, 12);

            eventTelemetry.Context.User.Id = string.Empty;
            requestTelemetry.Context.User.Id = string.Empty;
            var eventTelemetrySamplingScoreWithUserId = SamplingScoreGenerator.GetSamplingScore(eventTelemetry);
            var requestTelemetrySamplingScoreWithUserId = SamplingScoreGenerator.GetSamplingScore(requestTelemetry);

            Assert.AreEqual(eventTelemetrySamplingScoreNoUserId, eventTelemetrySamplingScoreWithUserId);
            Assert.AreEqual(requestTelemetrySamplingScoreNoUserId, requestTelemetrySamplingScoreWithUserId);
        }

        [TestMethod]
        public void SamplingScoreGeneratedUsingOperationIdIfPresent()
        {
            string operationId = GenerateRandomOperaitonId();

            var eventTelemetry = new EventTelemetry();
            eventTelemetry.Context.Operation.Id = operationId;

            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Operation.Id = operationId;

            var eventTelemetrySamplingScore = SamplingScoreGenerator.GetSamplingScore(eventTelemetry);
            var requestTelemetrySamplingScore = SamplingScoreGenerator.GetSamplingScore(requestTelemetry);

            Assert.AreEqual(eventTelemetrySamplingScore, requestTelemetrySamplingScore, 12);
        }

        [TestMethod]
        public void SamplingScoreIsRandomIfUserIdAndOperationIdAreNotPresent()
        {
            var eventTelemetry = new EventTelemetry();
            var traceTelemetry = new TraceTelemetry();

            var eventTelemetrySamplingScore = SamplingScoreGenerator.GetSamplingScore(eventTelemetry);
            var traceTelemetrySamplingScore = SamplingScoreGenerator.GetSamplingScore(traceTelemetry);

            Assert.AreNotEqual(eventTelemetrySamplingScore, traceTelemetrySamplingScore);
        }

        [TestMethod]
        public void StringSamplingHashCodeProducesConsistentValues()
        {
            foreach (string input in StringHash.Keys)
            {
                int calculatedHash = input.GetSamplingHashCode();

                Assert.AreEqual(StringHash[input], calculatedHash);
            }
        }

        [TestMethod]
        public void SamplingScoreIsCalculatedCorrectlyForStringInput()
        {
            foreach (string input in StringHash.Keys)
            {
                double calculatedHash = SamplingScoreGenerator.GetSamplingScore(input);

                Assert.AreEqual((double)StringHash[input] / int.MaxValue * 100, calculatedHash, delta: 0.01);
            }
        }

        private static string GenerateRandomUserId()
        {
            var userIdLength = Rand.Next(3, 12);

            string userId = string.Empty;

            for (int i = 0; i < userIdLength; i++)
            {
                userId += (char)('a' + Rand.Next(0, 25));
            }

            return userId;
        }

        private static string GenerateRandomOperaitonId()
        {
            return WeakConcurrentRandom.Instance.Next().ToString(CultureInfo.InvariantCulture);
        }
    }
}
