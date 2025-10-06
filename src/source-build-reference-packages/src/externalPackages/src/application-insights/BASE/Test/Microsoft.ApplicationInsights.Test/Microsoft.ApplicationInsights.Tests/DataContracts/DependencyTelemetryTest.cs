namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyTelemetryTest
    {
        /// <summary>
        /// The SDKs (and our customers) expect specific default values.
        /// This test is to verify that changes to the schema don't unexpectedly change our public api.
        /// </summary>
        [TestMethod]
        public void VerifyExpectedDefaultValue()
        {
            var defaultDependencyTelemetry = new DependencyTelemetry();
            Assert.AreEqual(true, defaultDependencyTelemetry.Success, "Success is expected to be true");
            Assert.IsNotNull(defaultDependencyTelemetry.Target);
            Assert.IsNotNull(defaultDependencyTelemetry.Name);
            Assert.IsNotNull(defaultDependencyTelemetry.Data);
            Assert.IsNotNull(defaultDependencyTelemetry.ResultCode);            
            Assert.IsNotNull(defaultDependencyTelemetry.Type);
            Assert.IsNotNull(defaultDependencyTelemetry.Id);
            Assert.AreEqual(SamplingDecision.None, defaultDependencyTelemetry.ProactiveSamplingDecision);
            Assert.AreEqual(SamplingTelemetryItemTypes.RemoteDependency, defaultDependencyTelemetry.ItemTypeFlag);
            Assert.IsTrue(defaultDependencyTelemetry.Id.Length >= 1);
        }

        [TestMethod]
        public void DependencyTelemetryITelemetryContractConsistentlyWithOtherTelemetryTypes()
        {
            new ITelemetryTest<DependencyTelemetry, AI.RemoteDependencyData>().Run();
        }

        [TestMethod]
        public void DependencyTelemetryPropertiesFromContextAndItemSerializesToPropertiesInJson()
        {
            var expected = CreateRemoteDependencyTelemetry();

            ((ITelemetry)expected).Sanitize();

            Assert.AreEqual(1, expected.Properties.Count);
            Assert.AreEqual(1, expected.Context.GlobalProperties.Count);

            Assert.IsTrue(expected.Properties.ContainsKey("TestProperty"));
            Assert.IsTrue(expected.Context.GlobalProperties.ContainsKey("TestPropertyGlobal"));

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.AvailabilityData>(expected);

            // Items added to both dependency.Properties, and dependency.Context.GlobalProperties are serialized to properties.
            // IExtension object in CreateDependencyTelemetry adds 2 more properties: myIntField and myStringField
            Assert.AreEqual(4, item.data.baseData.properties.Count);            
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestPropertyGlobal"));
            Assert.IsTrue(item.data.baseData.properties.ContainsKey("TestProperty"));
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializesToJson()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);

            Assert.AreEqual<DateTimeOffset>(expected.Timestamp, DateTimeOffset.Parse(item.time, null, System.Globalization.DateTimeStyles.AssumeUniversal));
            Assert.AreEqual(expected.Sequence, item.seq);
            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
            AssertEx.AreEqual(expected.Context.SanitizedTags.ToArray(), item.tags.ToArray());
            Assert.AreEqual(nameof(AI.RemoteDependencyData), item.data.baseType);

            Assert.AreEqual(expected.Id, item.data.baseData.id);
            Assert.AreEqual(expected.ResultCode, item.data.baseData.resultCode);
            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Duration, TimeSpan.Parse(item.data.baseData.duration));
            Assert.AreEqual(expected.Type, item.data.baseData.type);
            Assert.AreEqual(expected.Success, item.data.baseData.success);

            // IExtension is currently flattened into the properties by serialization
            Utils.CopyDictionary(((MyTestExtension)expected.Extension).SerializeIntoDictionary(), expected.Properties);

            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        /// Test validates that if Serialize is called multiple times, and telemetry is modified
        /// in between, serialize always gives the latest state.
        public void RemoteDependencySerializationPicksUpCorrectState()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry();
            ((ITelemetry)expected).Sanitize();
            byte[] buf = new byte[1000000];
            expected.SerializeData(new JsonSerializationWriter(new StreamWriter(new MemoryStream(buf))));

            // Change the telemetry after serialization.
            expected.Name = expected.Name + "new";

            // Validate that the newly updated Name is picked up.
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);

            Assert.AreEqual<DateTimeOffset>(expected.Timestamp, DateTimeOffset.Parse(item.time, null, System.Globalization.DateTimeStyles.AssumeUniversal));
            Assert.AreEqual(expected.Sequence, item.seq);
            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
            AssertEx.AreEqual(expected.Context.SanitizedTags.ToArray(), item.tags.ToArray());
            Assert.AreEqual(nameof(AI.RemoteDependencyData), item.data.baseType);

            Assert.AreEqual(expected.Id, item.data.baseData.id);
            Assert.AreEqual(expected.ResultCode, item.data.baseData.resultCode);
            Assert.AreEqual(expected.Name, item.data.baseData.name);
            Assert.AreEqual(expected.Duration, TimeSpan.Parse(item.data.baseData.duration));
            Assert.AreEqual(expected.Type, item.data.baseData.type);

            Assert.AreEqual(expected.Success, item.data.baseData.success);
            AssertEx.AreEqual(expected.Properties.ToArray(), item.data.baseData.properties.ToArray());
        }

        [TestMethod]
        public void SerializeWritesNullValuesAsExpectedByEndpoint()
        {
            DependencyTelemetry original = new DependencyTelemetry();
            original.Name = null;
            original.Data = null;
            original.Type = null;
            original.Success = null;
            ((ITelemetry)original).Sanitize();
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(original);

            Assert.AreEqual(2, item.data.baseData.ver);
        }

        [TestMethod]
        public void SerializePopulatesRequiredFieldsOfDependencyTelemetry()
        {
            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var depTelemetry = new DependencyTelemetry();
                depTelemetry.Context.InstrumentationKey = Guid.NewGuid().ToString();
                ((ITelemetry)depTelemetry).Sanitize();
                var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(depTelemetry);

                Assert.AreEqual(2, item.data.baseData.ver);
                Assert.IsNotNull(item.data.baseData.id);
                Assert.IsNotNull(item.time);
                Assert.AreEqual(new TimeSpan(), TimeSpan.Parse(item.data.baseData.duration));
                Assert.IsTrue(item.data.baseData.success);
            }
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializesStructuredIKeyToJsonCorrectlyPreservingPrefixCasing()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry();
            expected.Context.InstrumentationKey = "AIC-" + expected.Context.InstrumentationKey;
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);

            Assert.AreEqual(expected.Context.InstrumentationKey, item.iKey);
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializeCommandNameToJson()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry("Select * from Customers where CustomerID=@1");
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);
            AI.RemoteDependencyData dp = item.data.baseData;
            Assert.AreEqual(expected.Data, dp.data);
        }

        [TestMethod]
        public void RemoteDependencyTelemetrySerializeNullCommandNameToJson()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry(null);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);
            AI.RemoteDependencyData dp = item.data.baseData;
            Assert.IsTrue(string.IsNullOrEmpty(dp.data));
        }
        
        [TestMethod]
        public void RemoteDependencyTelemetrySerializeEmptyCommandNameToJson()
        {
            DependencyTelemetry expected = this.CreateRemoteDependencyTelemetry(string.Empty);
            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(expected);
            AI.RemoteDependencyData dp = item.data.baseData;
            Assert.IsTrue(string.IsNullOrEmpty(dp.data));
        }

        [TestMethod]
        public void DependencyTypeNameDefaultsToEmptyInConstructor()
        {
#pragma warning disable 618
            var dependency = new DependencyTelemetry("name", "command name", DateTimeOffset.Now, TimeSpan.FromSeconds(42), false);

            AssertEx.IsEmpty(dependency.DependencyKind);
#pragma warning restore 618

            AssertEx.IsEmpty(dependency.Type);
        }

        [TestMethod]
        public void SerttingDependencyKindSetsDependencyTypeName()
        {
            DependencyTelemetry expected = new DependencyTelemetry();
#pragma warning disable 618
            expected.DependencyKind = "Http";

            Assert.AreEqual("Http", expected.DependencyKind);
            Assert.AreEqual("Http", expected.DependencyTypeName);
#pragma warning restore 618

        }

        [TestMethod]
        public void SanitizeWillTrimAppropriateFields()
        {
            DependencyTelemetry telemetry = new DependencyTelemetry();
            telemetry.Name = new string('Z', Property.MaxNameLength + 1);
            telemetry.Data = new string('Y', Property.MaxDataLength + 1);
            telemetry.Type = new string('D', Property.MaxDependencyTypeLength + 1);
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'X', new string('X', Property.MaxValueLength + 1));
            telemetry.Properties.Add(new string('X', Property.MaxDictionaryNameLength) + 'Y', new string('X', Property.MaxValueLength + 1));
            telemetry.Metrics.Add(new string('Y', Property.MaxDictionaryNameLength) + 'X', 42.0);
            telemetry.Metrics.Add(new string('Y', Property.MaxDictionaryNameLength) + 'Y', 42.0);

            ((ITelemetry)telemetry).Sanitize();

            Assert.AreEqual(new string('Z', Property.MaxNameLength), telemetry.Name);
            Assert.AreEqual(new string('Y', Property.MaxDataLength), telemetry.Data);
            Assert.AreEqual(new string('D', Property.MaxDependencyTypeLength), telemetry.Type);

            Assert.AreEqual(2, telemetry.Properties.Count);
            var t = new SortedList<string, string>(telemetry.Properties);

            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength), t.Keys.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[1]);
            Assert.AreEqual(new string('X', Property.MaxDictionaryNameLength - 3) + "1", t.Keys.ToArray()[0]);
            Assert.AreEqual(new string('X', Property.MaxValueLength), t.Values.ToArray()[0]);

            Assert.AreSame(telemetry.Properties, telemetry.Properties);

            Assert.AreEqual(2, telemetry.Metrics.Count);
            var keys = telemetry.Metrics.Keys.OrderBy(s => s).ToArray();
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength), keys[1]);
            Assert.AreEqual(new string('Y', Property.MaxDictionaryNameLength - 3) + "1", keys[0]);
        }

        [TestMethod]
        public void DependencyTelemetryImplementsISupportSamplingContract()
        {
            var telemetry = new DependencyTelemetry();

            Assert.IsNotNull(telemetry as ISupportSampling);
        }

        [TestMethod]
        public void DependencyTelemetryImplementsISupportAdvancedSamplingContract()
        {
            var telemetry = new DependencyTelemetry();

            Assert.IsNotNull(telemetry as ISupportAdvancedSampling);
        }

        [TestMethod]
        public void DependencyTelemetryHasCorrectValueOfSamplingPercentageAfterSerialization()
        {
            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            ((ISupportSampling)telemetry).SamplingPercentage = 10;

            var item = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<AI.RemoteDependencyData>(telemetry);

            Assert.AreEqual(10, item.sampleRate);
        }

        [TestMethod]
        public void DependencyTelemetrySetGetOperationDetail()
        {
            const string key = "foo";
            const string detail = "bar";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(key, detail);
            Assert.IsTrue(telemetry.TryGetOperationDetail(key, out object retrievedValue));
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(detail, retrievedValue.ToString());

            // Clear and verify the detail is no longer present            
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);            
            Assert.IsFalse(telemetry.TryGetOperationDetail(key, out retrievedValue));
        }

        [TestMethod]
        public void DependencyTelemetryGetUnsetOperationDetail()
        {
            const string key = "foo";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            Assert.IsFalse(telemetry.TryGetOperationDetail(key, out object retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryDeepCloneCopiesAllProperties()
        {
            DependencyTelemetry telemetry = CreateRemoteDependencyTelemetry();
            DependencyTelemetry other = (DependencyTelemetry)telemetry.DeepClone();

            ComparisonConfig comparisonConfig = new ComparisonConfig();
            CompareLogic deepComparator = new CompareLogic(comparisonConfig);

            ComparisonResult result = deepComparator.Compare(telemetry, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void DependencyTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var telemetry = new DependencyTelemetry();
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = telemetry.DeepClone();
        }

        private DependencyTelemetry CreateRemoteDependencyTelemetry()
        {
            DependencyTelemetry item = new DependencyTelemetry
                                            {
                                                Timestamp = DateTimeOffset.Now,
                                                Sequence = "4:2",
                                                Name = "MyWebServer.cloudapp.net",
                                                Duration = TimeSpan.FromMilliseconds(42),
                                                Success = true,
                                                Id = "DepID",
                                                ResultCode = "200",
                                                Type = "external call"
                                            };
            item.Context.InstrumentationKey = Guid.NewGuid().ToString();
            item.Properties.Add("TestProperty", "TestValue");
            item.Context.GlobalProperties.Add("TestPropertyGlobal", "TestValue");
            item.Extension = new MyTestExtension() { myIntField = 42, myStringField = "value" };
            return item;
        }

        private DependencyTelemetry CreateRemoteDependencyTelemetry(string commandName)
        {
            DependencyTelemetry item = this.CreateRemoteDependencyTelemetry();
            item.Data = commandName;
            return item;
        }
    }
}