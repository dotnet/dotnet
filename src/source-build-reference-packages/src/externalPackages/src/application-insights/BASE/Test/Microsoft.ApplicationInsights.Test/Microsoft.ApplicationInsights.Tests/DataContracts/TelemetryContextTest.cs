namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;
    using System.Collections.Generic;

    [TestClass]
    public class TelemetryContextTest
    {
        [TestMethod]
        public void TelemetryContextIsPublicAndMeantToBeUsedByCustomers()
        {
            Assert.IsTrue(typeof(TelemetryContext).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void TelemetryContextIsSealedToSupportCompilationAsWinmd()
        {
            Assert.IsTrue(typeof(TelemetryContext).GetTypeInfo().IsSealed);
        }

        [TestMethod]
        public void InstrumentationKeyIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.InstrumentationKey);
        }

        [TestMethod]
        public void InstrumentationKeySetterThrowsArgumentNullExceptionWhenValueIsNullToPreventNullReferenceExceptionsLater()
        {
            var context = new TelemetryContext();
            AssertEx.Throws<ArgumentNullException>(() => context.InstrumentationKey = null);
        }

        [TestMethod]
        public void FlagsIsZeroByDefault()
        {
            var context = new TelemetryContext();
            Assert.AreEqual(0, context.Flags);
        }

        [TestMethod]
        public void FlagsCanBeSetAndGet()
        {
            var context = new TelemetryContext();
            context.Flags |= 0x00100000;
            Assert.AreEqual(0x00100000, context.Flags);
        }

        [TestMethod]
        public void ComponentIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Component);
        }

        [TestMethod]
        public void DeviceIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Device);
        }

        [TestMethod]
        public void SessionIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Session);
        }

        [TestMethod]
        public void UserIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.User);
        }

        [TestMethod]
        public void OperationIsNotNullByDefaultToPreventNullReferenceExceptionsInUserCode()
        {
            var context = new TelemetryContext();
            Assert.IsNotNull(context.Operation);
        }

        [TestMethod]
        public void LocationIsNotNullByDefaultToPreventNullReferenceExceptionInUserCode()
        {
            TelemetryContext context = new TelemetryContext();
            Assert.IsNotNull(context.Location);
        }

        [TestMethod]
        public void InternalIsNotNullByDefaultToPreventNullReferenceExceptionInUserCode()
        {
            TelemetryContext context = new TelemetryContext();
            Assert.IsNotNull(context.Internal);
        }

        [TestMethod]
        public void InitializeSetsTelemetryInstrumentationKeyFromSource()
        {
            var source = new TelemetryContext { InstrumentationKey = "TestValue" };
            var target = new TelemetryContext();

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual("TestValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeInstrumentationKeySetsTelemetryInstrumentationKey()
        {
            var sourceInstrumentationKey = "TestValue";
            var target = new TelemetryContext();

            target.InitializeInstrumentationkey(sourceInstrumentationKey);

            Assert.AreEqual("TestValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeSetsTelemetryInstrumentationKeyFromArgument()
        {
            var source = new TelemetryContext { InstrumentationKey = "TestValue" };
            var target = new TelemetryContext();

            target.Initialize(source, "OtherTestValue");

            Assert.AreEqual("OtherTestValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeDoesNotOverrideTelemetryInstrumentationKey()
        {
            var source = new TelemetryContext { InstrumentationKey = "SourceValue" };
            var target = new TelemetryContext { InstrumentationKey = "TargetValue" };

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual("TargetValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeInstrumentationKeyDoesNotOverrideTelemetryInstrumentationKey()
        {
            var sourceInstrumentationKey = "SourceValue";
            var target = new TelemetryContext { InstrumentationKey = "TargetValue" };

            target.InitializeInstrumentationkey(sourceInstrumentationKey);

            Assert.AreEqual("TargetValue", target.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeSetsFlagsFromSource()
        {
            var source = new TelemetryContext { Flags = 0x00100000 };
            var target = new TelemetryContext();

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual(0x00100000, target.Flags);
        }


        [TestMethod]
        public void InitializeSetsFlagsFromArgument()
        {
            var source = new TelemetryContext();
            var target = new TelemetryContext { Flags = 0x00100000 };

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual(0x00100000, target.Flags);
        }

        [TestMethod]
        public void InitializeSetsFlagsFromSourceAndArgument()
        {
            var source = new TelemetryContext { Flags = 0x00010000 };
            var target = new TelemetryContext { Flags = 0x00100000 };

            target.Initialize(source, source.InstrumentationKey);

            Assert.AreEqual(0x00110000, target.Flags);
        }

        [TestMethod]
        public void SerializeWritesCopiedDeviceContext()
        {
            var context = new TelemetryContext();
            context.Device.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.DeviceId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedComponentContext()
        {
            var context = new TelemetryContext();
            context.Component.Version = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.ApplicationVersion + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedLocationContext()
        {
            var context = new TelemetryContext();
            context.Location.Ip = "1.2.3.4";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.LocationIp + "\":\"1.2.3.4\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedUserContext()
        {
            var context = new TelemetryContext();
            context.User.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.UserId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedOperationContext()
        {
            var context = new TelemetryContext();
            context.Operation.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.OperationId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void SerializeWritesCopiedSessionContext()
        {
            var context = new TelemetryContext();
            context.Session.Id = "Test Value";
            string json = CopyAndSerialize(context);
            AssertEx.Contains("\"" + ContextTagKeys.Keys.SessionId + "\":\"Test Value\"", json, StringComparison.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void TestSanitizeGlobalProperties()
        {
            var addedKeyWithSizeAboveLimit = new string('K', Property.MaxDictionaryNameLength + 1);
            var addedValueWithSizeAboveLimit = new string('V', Property.MaxValueLength + 1);

            var expectedKeyWithSizeWithinLimit = new string('K', Property.MaxDictionaryNameLength);
            var expectedValueWithSizeWithinLimit = new string('V', Property.MaxValueLength);

            var context = new TelemetryContext();
            context.GlobalProperties.Add(addedKeyWithSizeAboveLimit, addedValueWithSizeAboveLimit);
            context.SanitizeGlobalProperties();

            Assert.IsTrue(context.GlobalProperties.ContainsKey(expectedKeyWithSizeWithinLimit));
            var value = context.GlobalProperties[expectedKeyWithSizeWithinLimit];
            Assert.AreEqual(expectedValueWithSizeWithinLimit, value);
        }

        [TestMethod]
        public void TestStoresRawObject()
        {
            const string key = "foo";
            const string detail = "bar";

            var context = new TelemetryContext();
            context.StoreRawObject(key, detail);
            Assert.IsFalse(context.TryGetRawObject("keyDontExst", out object actualDontExist));
            Assert.IsTrue(context.TryGetRawObject(key, out object actual));
            Assert.AreEqual(detail, actual);
        }

        [TestMethod]
        public void TestTelemetryContextDoesNotThrowOnInvalidKeysValuesForRawObjectStore()
        {
            const string key = "";
            const string detail = "bar";
            var context = new TelemetryContext();

            // These shouldn't throw.
            context.StoreRawObject(null, detail);
            context.StoreRawObject(null, detail, false);
            context.TryGetRawObject(null, out object actualDontExist);

            context.StoreRawObject(null, null);
            context.StoreRawObject(null, null, false);
            context.TryGetRawObject(null, out object actualDontExist1);

            context.StoreRawObject("key", null);
            Assert.IsTrue(context.TryGetRawObject("key", out object actual));
            Assert.AreSame(null, actual);

            context.StoreRawObject(key, detail);
            Assert.IsTrue(context.TryGetRawObject(key, out object actual1));
            Assert.AreSame(detail, actual1);

            context.StoreRawObject(string.Empty, detail);
            Assert.IsTrue(context.TryGetRawObject(string.Empty, out object actual2));
            Assert.AreSame(detail, actual2);
        }

        [TestMethod]
        public void TestRawObjectIsOverwritten()
        {
            const string key = "foo";
            const string detail = "bar";
            const string detailNew = "barnew";

            var context = new TelemetryContext();
            context.StoreRawObject(key, detail);
            Assert.IsTrue(context.TryGetRawObject(key, out object actual));
            Assert.AreSame(detail, actual);

            context.StoreRawObject(key, detailNew);
            Assert.IsTrue(context.TryGetRawObject(key, out object actualNew));
            Assert.AreSame(detailNew, actualNew);
        }

        [TestMethod]
        public void TestRawObjectLastWrittenValueWins()
        {
            const string key = "foo";
            const string detail = "bar";
            const string detailNew = "barnew";
            const string detailNewer = "barnewer";
            const string detailNewest = "barnewest";
            const string detailNewestFinal = "barnewestfinal";

            var context = new TelemetryContext();

            // Overwrite temp key value with new temp value
            context.StoreRawObject(key, detail, true);            
            context.StoreRawObject(key, detailNew, true);
            Assert.IsTrue(context.TryGetRawObject(key, out object actualNew));
            Assert.AreSame(detailNew, actualNew);

            // Overwrite temp key value with new perm value
            context.StoreRawObject(key, detailNewer, false);
            Assert.IsTrue(context.TryGetRawObject(key, out object actualNewer));
            Assert.AreSame(detailNewer, actualNewer);

            // Overwrite perm key value with new perm value
            context.StoreRawObject(key, detailNewest, false);
            Assert.IsTrue(context.TryGetRawObject(key, out object actualNewest));
            Assert.AreSame(detailNewest, actualNewest);

            // Overwrite perm key value with new temp value
            context.StoreRawObject(key, detailNewestFinal, true);
            Assert.IsTrue(context.TryGetRawObject(key, out object actualNewestFinal));
            Assert.AreSame(detailNewestFinal, actualNewestFinal);
        }

        [TestMethod]
        public void TestStoreRawObjectTempByDefault()
        {
            const string keyTemp = "fooTemp";
            const string detailTemp = "barTemp";
            const string keyPerm = "fooPerm";
            const string detailPerm = "barPerm";

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTemp);
            context.StoreRawObject(keyPerm, detailPerm, false);

            Assert.IsTrue(context.TryGetRawObject(keyTemp, out object temp));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object perm));

            context.ClearTempRawObjects();
            Assert.IsFalse(context.TryGetRawObject(keyTemp, out object tempAfterCleanup));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object permAfterCleanup));
        }

        [TestMethod]
        public void TestClearsTempRawObjects()
        {
            const string keyTemp = "fooTemp";
            const string detailTemp = "barTemp";
            const string keyPerm = "fooPerm";
            const string detailPerm = "barPerm";

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTemp, true);
            context.StoreRawObject(keyPerm, detailPerm, false);
            
            Assert.IsTrue(context.TryGetRawObject(keyTemp, out object temp));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object perm));

            context.ClearTempRawObjects();
            Assert.IsFalse(context.TryGetRawObject(keyTemp, out object tempAfterCleanup));
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object permAfterCleanup));
        }

        [TestMethod]
        public void TestRawObjectIsSharedOnDeepCopy()
        {
            string keyTemp = "keyTemp";
            string detailTemp = "valueTemp";
            var detailTempObj = new MyCustomObject(detailTemp);
            string keyPerm = "keyPerm";
            string detailPerm = "valuePerm";
            var detailPermObj = new MyCustomObject(detailPerm);

            var context = new TelemetryContext();
            context.StoreRawObject(keyTemp, detailTempObj);
            context.StoreRawObject(keyPerm, detailPermObj, false);
            Assert.IsTrue(context.TryGetRawObject(keyTemp, out object actualTemp));
            Assert.AreEqual(detailTempObj, actualTemp);
            Assert.IsTrue(context.TryGetRawObject(keyPerm, out object actualPerm));
            Assert.AreEqual(detailPermObj, actualPerm);

            var clonedContext = context.DeepClone();
            Assert.IsTrue(clonedContext.TryGetRawObject(keyTemp, out object actualTempFromClone));
            Assert.AreEqual(detailTempObj, actualTempFromClone);
            Assert.IsTrue(clonedContext.TryGetRawObject(keyPerm, out object actualPermFromClone));
            Assert.AreEqual(detailPermObj, actualPermFromClone);

            // Modify the object in original context
            context.TryGetRawObject(keyTemp, out object tempObjFromOriginal);
            ((MyCustomObject)tempObjFromOriginal).v = "new temp value";

            clonedContext.TryGetRawObject(keyTemp, out object tempObjFromClone);

            // validate that modifying original context affects the clone.
            Assert.AreEqual(((MyCustomObject)tempObjFromOriginal).v, ((MyCustomObject)tempObjFromClone).v);
        }

        private static string CopyAndSerialize(TelemetryContext source)
        {
            // Create a copy of the source context to verify that Serialize writes property values stored in tags 
            // dictionary even if their context objects (User, Location, etc) haven't been initialized yet.
            var target = new TelemetryContext();
            target.Initialize(source, source.InstrumentationKey);

            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                Telemetry.WriteTelemetryContext(new JsonSerializationWriter(stringWriter), source);
                return stringWriter.ToString();
            }
        }
    }

    internal class MyCustomObject
    {
        public string v;

        public MyCustomObject(string v)
        {
            this.v = v;
        }
    }
}
