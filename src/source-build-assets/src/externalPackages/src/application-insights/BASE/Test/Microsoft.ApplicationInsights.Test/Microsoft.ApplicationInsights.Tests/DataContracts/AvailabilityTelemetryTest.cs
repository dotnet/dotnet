namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using KellermanSoftware.CompareNetObjects;

    [TestClass]
    public class AvailabilityTelemetryTest
    {
        [TestMethod]
        public void AvailabilityTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = CreateAvailabilityTelemetry();

            ((ITelemetry)expected).Sanitize();

            Assert.AreEqual(1, expected.Properties.Count);
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("TestPropertyGlobal"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.AvailabilityData>(expected);

            // Items added to both availability.Properties, and availability.Context.GlobalProperties are serialized to properties.
            // IExtension object in CreateAvailabilityTelemetry adds 2 more properties: myIntField and myStringField
            Assert.AreEqual(4, item.data.baseData.properties.Count);
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
        }

        [TestMethod]
        public void AvailabilityTelemetrySerializesToJson()
        {
            AvailabilityTelemetry expected = this.CreateAvailabilityTelemetry();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.AvailabilityData>(expected);

            Assert.AreEqual<DateTimeOffset>(expected.Timestamp, DateTimeOffset.Parse(item.time, null, System.Globalization.DateTimeStyles.AssumeUniversal));
            Assert.AreEqual(expected.Sequence, item.seq);
            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
            AssertEx.AreEqual(expected.Context.SanitizedTags.ToArray(), item.tags.ToArray());
            Assert.AreEqual(nameof(AI.AvailabilityData), item.data.baseType);

            Assert.AreEqual(expected.Duration, TimeSpan.Parse(item.data.baseData.duration));
            Assert.AreEqual(expected.Message, item.data.baseData.message);
            Assert.AreEqual(expected.Success, item.data.baseData.success);
            Assert.AreEqual(expected.RunLocation, item.data.baseData.runLocation);
            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Id.ToString(), item.data.baseData.id);

            // IExtension is currently flattened into the properties by serialization
            Utils.CopyDictionary(((MyTestExtension)expected.Extension).SerializeIntoDictionary(), expected.Properties);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void SanitizeWillTrimAppropriateFieldsForAvailability()
        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();
            telemetry.Message = new string('Z', Property.MaxAvailabilityMessageLength + 1);
            telemetry.RunLocation = new string('Y', Property.MaxRunLocationLength + 1);
            telemetry.Name = new string('D', Property.MaxTestNameLength + 1);
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'X', new string('X', Property.MaxValueLength + 1));
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'Y', new string('X', Property.MaxValueLength + 1));

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(new string('Z', Property.MaxAvailabilityMessageLength), telemetry.Message);
            Assert.AreEqual(new string('Y', Property.MaxRunLocationLength), telemetry.RunLocation);
            Assert.AreEqual(new string('D', Property.MaxTestNameLength), telemetry.Name);

            Assert.AreEqual(2, telemetry.Properties.Count);
            var t = new SortedList<string, string>(telemetry.Properties);

            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), t.Keys.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", t.Keys.ToArray()[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[0]);

            Assert.AreSame(telemetry.Properties, telemetry.Properties);
        }

        [TestMethod]
        public void SanitizeWillLeaveCustomMessageForAvailability()
        {
            AvailabilityTelemetry telemetry = CreateAvailabilityTelemetry();

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(telemetry.Data.message, "Test Message");
        }

        [TestMethod]
        public void SanitizeWillSetFailedMessageForAvailability()
        {
            AvailabilityTelemetry telemetry = CreateAvailabilityTelemetry();
            telemetry.Message = string.Empty;
            telemetry.Success = false;

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(telemetry.Data.message, "Failed");
        }

        [TestMethod]
        public void SanitizeWillSetPassedMessageForAvailability()
        {
            AvailabilityTelemetry telemetry = CreateAvailabilityTelemetry();
            telemetry.Message = string.Empty;

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(telemetry.Data.message, "Passed");
        }

        [TestMethod]
        public void ConstructorWillCastSuccessToResult()
        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();

            Assert.AreEqual(telemetry.Data.success, true);
        }

        [TestMethod]
        public void AssignmentWillCastSuccessToResult()
        {
            AvailabilityTelemetry telemetry = new AvailabilityTelemetry();
            telemetry.Success = false;

            Assert.AreEqual(telemetry.Data.success, false);
        }

        [TestMethod]
        
        public void AvailabilityTelemetryDeepCloneCopiesAllProperties()
        {
            AvailabilityTelemetry telemetry = CreateAvailabilityTelemetry();
            AvailabilityTelemetry other = (AvailabilityTelemetry)telemetry.DeepClone();

            ComparisonConfig comparisonConfig = new ComparisonConfig();
            CompareLogic deepComparator = new CompareLogic(comparisonConfig);

            ComparisonResult result = deepComparator.Compare(telemetry, other);            
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void AvailabilityTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var telemetry = new AvailabilityTelemetry();
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = telemetry.DeepClone();
        }

        private AvailabilityTelemetry CreateAvailabilityTelemetry()
        {
            AvailabilityTelemetry item = new AvailabilityTelemetry
            {
                Message = "Test Message",
                RunLocation = "Test Location",
                Name = "Test Name",
                Duration = TimeSpan.FromSeconds(30),
                Success = true
            };
            item.Context.InstrumentationKey = Guid.NewGuid().ToString();
            item.Properties.Add("TestProperty", "TestValue");
            item.Context.GlobalProperties.Add("TestPropertyGlobal", "TestValue");
            item.Sequence = "12";
            item.Extension = new MyTestExtension() { myIntField = 42, myStringField = "value" };
            return item;
        }

        private AvailabilityTelemetry CreateAvailabilityTelemetry(string testName)
        {
            AvailabilityTelemetry item = this.CreateAvailabilityTelemetry();
            item.Name = testName;
            return item;
        }
    }
}