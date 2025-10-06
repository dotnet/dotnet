namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using CompareLogic = KellermanSoftware.CompareNetObjects.CompareLogic;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.TestFramework;
    using System.Runtime.CompilerServices;

    [TestClass]
    public class TraceTelemetryTest
    {
        [TestMethod]
        public void ClassIsPublic()
        {
            Assert.IsTrue(typeof(TraceTelemetry).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void TraceTelemetryImplementsITelemetryContract()
        {
            var test = new ITelemetryTest<TraceTelemetry, AI.MessageData>();
            test.Run();
        }

        [TestMethod]
        public void ConstructorInitializesDefaultTraceTelemetryInstance()
        {
            var item = new TraceTelemetry();
            Assert.IsNotNull(item.Context);
            Assert.IsNotNull(item.Properties);
            AssertEx.IsEmpty(item.Message);
            Assert.IsNull(item.SeverityLevel);
            Assert.AreEqual(SamplingDecision.None, item.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.Message, item.ItemTypeFlag);
        }

        [TestMethod]
        public void ConstructorInitializesTraceTelemetryInstanceWithGivenMessage()
        {
            var item = new TraceTelemetry("TestMessage");
            Assert.IsNotNull(item.Context);
            Assert.IsNotNull(item.Properties);
            Assert.AreEqual("TestMessage", item.Message);
            Assert.IsNull(item.SeverityLevel);
            Assert.AreEqual(SamplingDecision.None, item.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.Message, item.ItemTypeFlag);
        }

        [TestMethod]
        public void ConstructorInitializesTraceTelemetryInstanceWithGivenMessageAndSeverityLevel()
        {
            var trace = new TraceTelemetry("TestMessage", SeverityLevel.Critical);
            Assert.IsNotNull(trace.Context);
            Assert.IsNotNull(trace.Properties);
            Assert.AreEqual("TestMessage", trace.Message);
            Assert.AreEqual(SeverityLevel.Critical, trace.SeverityLevel);
            Assert.AreEqual(SamplingDecision.None, trace.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.Message, trace.ItemTypeFlag);
        }

        [TestMethod]
        public void TraceTelemetrySerializesToJsonCorrectly()
        {
            var expected = new TraceTelemetry();
            expected.Message = "My Test";
            expected.Properties.Add("Property2", "Value2");

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MessageData>(expected);

            // NOTE: It's correct that we use the v1 name here, and therefore we test against it.
            Assert.AreEqual(item.name, AI.ItemType.Message);
            Assert.AreEqual(nameof(AI.MessageData), item.data.baseType);
            Assert.AreEqual(2, item.data.baseData.ver);
            Assert.AreEqual(expected.Message, item.data.baseData.message);
            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void SerializeWritesItemSeverityLevelAsExpectedByEndpoint()
        {
            var expected = new TraceTelemetry { SeverityLevel = SeverityLevel.Information };
            ((ITelemetry)expected).Sanitize();

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MessageData>(expected);

            Assert.AreEqual(AI.SeverityLevel.Information, item.data.baseData.severityLevel.Value);
        }

        [TestMethod]
        public void SerializeWritesNullValuesAsExpectedByEndpoint()
        {
            TraceTelemetry original = new TraceTelemetry();
            original.Message = null;
            original.SeverityLevel = null;
            ((ITelemetry)original).Sanitize();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MessageData>(original);

            Assert.AreEqual(2, item.data.baseData.ver);
        }

        [TestMethod]
        public void SanitizeWillTrimAppropriateFields()
        {
            TraceTelemetry telemetry = new TraceTelemetry();
            telemetry.Message = new string('X', Property.MaxMessageLength + 1);
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'X', new string('X', Property.MaxValueLength + 1));
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'Y', new string('X', Property.MaxValueLength + 1));

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(new string('X', Property.MaxMessageLength), telemetry.Message);
            Assert.AreEqual(2, telemetry.Properties.Count);
            var t = new SortedList<string, string>(telemetry.Properties);

            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), t.Keys.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", t.Keys.ToArray()[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[0]);
        }

        [TestMethod]
        public void SanitizePopulatesMessageWithErrorBecauseItIsRequiredByEndpoint()
        {
            var telemetry = new TraceTelemetry { Message = null };

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual("n/a", telemetry.Message);
        }

        [TestMethod]
        public void TraceTelemetryImplementsISupportSamplingContract()
        {
            var telemetry = new TraceTelemetry();

            Assert.IsNotNull(telemetry as ISupportSampling);
        }

        [TestMethod]
        public void TraceTelemetryImplementsISupportAdvancedSamplingContract()
        {
            var telemetry = new TraceTelemetry();

            Assert.IsNotNull(telemetry as ISupportAdvancedSampling);
        }

        [TestMethod]
        public void TraceTelemetryHasCorrectValueOfSamplingPercentageAfterSerialization()
        {
            var telemetry = new TraceTelemetry("my trace");
            ((ISupportSampling)telemetry).SamplingPercentage = 10;

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MessageData>(telemetry);

            Assert.AreEqual(10, item.sampleRate);
        }

        [TestMethod]
        public void TraceTelemetryDeepCloneCopiesAllProperties()
        {
            var trace = new TraceTelemetry();
            trace.Message = "My Test";
            trace.Properties.Add("Property2", "Value2");
            trace.SeverityLevel = SeverityLevel.Warning;
            trace.Sequence = "123456";
            trace.Timestamp = DateTimeOffset.Now;
            trace.Extension = new MyTestExtension();
            var other = trace.DeepClone();

            var deepComparator = new CompareLogic();
            var result = deepComparator.Compare(trace, other);

            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void TraceTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var trace = new TraceTelemetry();        
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = trace.DeepClone();            
        }

        [TestMethod]
        public void TraceTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = new TraceTelemetry();
            expected.Context.GlobalProperties.Add("contextpropkey", "contextpropvalue");
            expected.Properties.Add("TestProperty", "TestPropertyValue");
            ((ITelemetry)expected).Sanitize();

            Assert.AreEqual(1, expected.Properties.Count);
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("contextpropkey"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.MessageData>(expected);

            // Items added to both MessageData.Properties, and MessageData.Context.GlobalProperties are serialized to properties.
            Assert.AreEqual(2, item.data.baseData.properties.Count);
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("contextpropkey"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
        }
    }
}
