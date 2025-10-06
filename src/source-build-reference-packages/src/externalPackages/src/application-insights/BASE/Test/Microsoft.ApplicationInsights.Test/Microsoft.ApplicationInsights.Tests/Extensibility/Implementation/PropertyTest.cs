namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using DataContracts;
    using External;
    using Microsoft.ApplicationInsights.TestFramework;

    [TestClass]
    public class PropertyTest
    {
        [TestMethod]
        public void SanitizeNameTrimsLeadingAndTraliningSpaces()
        {
            const string Original = " name with spaces ";

            string sanitized = Original.SanitizeName();

            Assert.AreEqual(Original.Trim(), sanitized);
        }

        [TestMethod]
        public void SanitizeNameTruncatesValuesLongerThan1024Characters()
        {
            string original = new string('A', Property.MaxNameLength + 1);
            string sanitized = original.SanitizeName();

            Assert.AreEqual(Property.MaxNameLength, sanitized.Length);
        }

        [TestMethod]
        public void SanitizeNameDontTruncatesValuesSmallerThan1024Characters()
        {
            const int ValueLength = 512;

            string original = new string('c', ValueLength);
            string sanitized = original.SanitizeName();

            Assert.AreEqual(ValueLength, sanitized.Length);
        }

        [TestMethod]
        public void SanitizeValueTruncatesValuesLongerThan1024Characters()
        {
            string original = new string('A', Property.MaxValueLength + 10);
            string sanitized = original.SanitizeValue();

            Assert.AreEqual(Property.MaxValueLength, sanitized.Length);
        }

        [TestMethod]
        public void SanitizeValueDontTruncatesValuesSmallerThan1024Characters()
        {
            const int ValueLength = 512;

            string original = new string('c', ValueLength);
            string sanitized = original.SanitizeValue();

            Assert.AreEqual(ValueLength, sanitized.Length);
        }

        [TestMethod]
        public void SanitizeValueTrimsLeadingAndTraliningSpaces()
        {
            const string Original = " name with spaces ";
            string sanitized = Original.SanitizeValue();

            Assert.AreEqual(Original.Trim(), sanitized);
        }

        [TestMethod]
        public void SanitizeMessgaeTruncatesValuesLongerThan32768Characters()
        {
            const int MaxMessageLength = 32768;

            string original = new string('M', MaxMessageLength + 10);
            string sanitized = original.SanitizeMessage();

            Assert.AreEqual(MaxMessageLength, sanitized.Length);
        }

        [TestMethod]
        public void SanitizeMessageDontTruncatesValuesSmallerThan32768Characters()
        {
            const int MessageLength = 512;

            string original = new string('m', MessageLength);
            string sanitized = original.SanitizeMessage();

            Assert.AreEqual(MessageLength, sanitized.Length);
        }
        
        [TestMethod]
        public void SanitizeMessageTrimsLeadingAndTraliningSpaces()
        {
            const string Original = " name with   spaces    ";
            string sanitized = Original.SanitizeMessage();

            Assert.AreEqual(Original.Trim(), sanitized);
        }

        [TestMethod]
        public void SanitizeUriTruncatesValuesLongerThan2048Characters()
        {
            const int MaxUrlLength = 2048;

            string original = new string('M', MaxUrlLength);
            original = string.Concat("https://test.com/", original);
            
            Uri originalUri = new Uri(original);
            Uri sanitized = originalUri.SanitizeUri();

            Assert.AreEqual(MaxUrlLength, sanitized.ToString().Length);
        }

        [TestMethod]
        public void SanitizeUriDontTruncatesValuesSmallerThan2048Characters()
        {
            const int UriLength = 512;

            string original = new string('m', UriLength);
            original = string.Concat("https://m.com/", original);
            int originalUriLength = original.Length;

            Uri originalUri = new Uri(original);
            Uri sanitized = originalUri.SanitizeUri();

            Assert.AreEqual(originalUriLength, sanitized.ToString().Length);
        }

        [TestMethod]
        public void SanitizePropertiesTrimsLeadingAndTrailingSpaceInKeyNames()
        {
            const string OriginalKey = " key with spaces ";
            const string OriginalValue = "Test Value";
            var original = new Dictionary<string, string> { { OriginalKey, OriginalValue } };

            original.SanitizeProperties();

            string sanitizedKey = OriginalKey.Trim();
            AssertEx.AreEqual(new[] { new KeyValuePair<string, string>(sanitizedKey, OriginalValue) }, original);
        }

        [TestMethod]
        public void SanitizePropertiesReplacesEmptyStringWithEmptyWordToEnsurePropertyValueWillBeSerializedWithoutExceptions()
        {
            var dictionary = new Dictionary<string, string> { { string.Empty, "value" } };
            dictionary.SanitizeProperties();
            Assert.AreEqual("required", dictionary.Single().Key);
        }

        [TestMethod]
        public void SanitizePropertiesTruncatesKeysLongerThan150Characters()
        {
            string originalKey = new string('A', Property.MaxNameLength + 1);
            const string OriginalValue = "Test Value";
            var original = new Dictionary<string, string> { { originalKey, OriginalValue } };

            original.SanitizeProperties();

            Assert.AreEqual(Property.MaxDictionaryNameLength, original.First().Key.Length);
        }

        [TestMethod]
        public void SanitizePropertiesMakesKeysUniqueAfterTruncation()
        {
            string originalKey = new string('A', Property.MaxDictionaryNameLength + 1);
            const string OriginalValue = "Test Value";
            var original = new Dictionary<string, string> 
            { 
                { originalKey + "1", OriginalValue },
                { originalKey + "2", OriginalValue },
                { originalKey + "3", OriginalValue },
            };

            original.SanitizeProperties();

            Assert.AreEqual(3, original.Count);
            Assert.AreEqual(Property.MaxDictionaryNameLength, original.Keys.Max(key => key.Length));
        }

        [TestMethod]
        public void SanitizePropertiesTruncatesValuesLongerThan1024Characters()
        {
            const string OriginalKey = "test";
            string originalValue = new string('A', Property.MaxValueLength + 10);
            var original = new Dictionary<string, string> { { OriginalKey, originalValue } };

            original.SanitizeProperties();

            string sanitizedValue = originalValue.Substring(0, Property.MaxValueLength);
            AssertEx.AreEqual(new[] { new KeyValuePair<string, string>(OriginalKey, sanitizedValue) }, original);
        }

        [TestMethod]
        public void SanitizePropertiesTrimsLeadingAndTraliningSpacesFromValues()
        {
            const string OriginalKey = "test";
            const string OriginalValue = " name with spaces ";
            var original = new Dictionary<string, string> { { OriginalKey, OriginalValue } };

            original.SanitizeProperties();

            string sanitizedValue = OriginalValue.Trim();
            AssertEx.AreEqual(new[] { new KeyValuePair<string, string>(OriginalKey, sanitizedValue) }, original);
        }

        [TestMethod]
        public void SanitizeMeasurementsTrimsLeadingAndTrailingSpaceInKeyNames()
        {
            const string OriginalKey = " key with spaces ";
            const double OriginalValue = 42.0;
            var original = new Dictionary<string, double> { { OriginalKey, OriginalValue } };

            original.SanitizeMeasurements();

            string sanitizedKey = OriginalKey.Trim();
            AssertEx.AreEqual(new[] { new KeyValuePair<string, double>(sanitizedKey, OriginalValue) }, original);
        }

        [TestMethod]
        public void SanitizeMeasurementsTruncatesKeysLongerThan150Characters()
        {
            string originalKey = new string('A', Property.MaxNameLength + 1);
            const double OriginalValue = 42.0;
            var original = new Dictionary<string, double> { { originalKey, OriginalValue } };

            original.SanitizeMeasurements();

            Assert.AreEqual(Property.MaxDictionaryNameLength, original.First().Key.Length);
        }

        [TestMethod]
        public void SanitizeMeasurementsMakesKeysUniqueAfterTruncation()
        {
            string originalKey = new string('A', Property.MaxNameLength + 1);
            const double OriginalValue = 42.0;
            var original = new Dictionary<string, double> 
            { 
                { originalKey + "1", OriginalValue },
                { originalKey + "2", OriginalValue },
                { originalKey + "3", OriginalValue },
            };

            original.SanitizeMeasurements();

            Assert.AreEqual(3, original.Count);
            Assert.AreEqual(Property.MaxDictionaryNameLength, original.Keys.Max(key => key.Length));
        }

        [TestMethod]
        public void SanitizeMeasurementsReplacesNanWith0()
        {
            var original = new Dictionary<string, double>
            {
                { "Key", double.NaN },
            };

            original.SanitizeMeasurements();

            Assert.AreEqual(0, original["Key"]);
        }

        [TestMethod]
        public void SanitizeMeasurementsReplacesPositiveInfinityWith0()
        {
            var original = new Dictionary<string, double>
            {
                { "Key", double.PositiveInfinity },
            };

            original.SanitizeMeasurements();

            Assert.AreEqual(0, original["Key"]);
        }

        [TestMethod]
        public void SanitizeMeasurementsReplacesNegativeInfinityWith0()
        {
            var original = new Dictionary<string, double>
            {
                { "Key", double.NegativeInfinity },
            };

            original.SanitizeMeasurements();

            Assert.AreEqual(0, original["Key"]);
        }

        [TestMethod]
        public void SanitizeTelemetryContextTest()
        {            
            var telemetryContext = new TelemetryContext();

            var componentContext = telemetryContext.Component;
            componentContext.Version = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.ApplicationVersion]+ 1);

            var deviceContext = telemetryContext.Device;
            deviceContext.Id = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceId] + 1);
            deviceContext.Model = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceModel] + 1);
            deviceContext.OemName = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceOEMName] + 1);
            deviceContext.OperatingSystem = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceOSVersion] + 1);
            deviceContext.Type = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceType] + 1);

            var locationContext = telemetryContext.Location;
            locationContext.Ip = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.LocationIp] + 1);

            var operationContext = telemetryContext.Operation;
            operationContext.Id = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationId] + 1);
            operationContext.Name = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationName] + 1);
            operationContext.ParentId = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationParentId] + 1);
            operationContext.SyntheticSource = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationSyntheticSource] + 1);
            operationContext.CorrelationVector = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationCorrelationVector] + 1);

            var sessionContext = telemetryContext.Session;
            sessionContext.Id = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.SessionId] + 1);

            var userContext = telemetryContext.User;
            userContext.Id = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserId] + 1);
            userContext.AccountId = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserAccountId] + 1);
            userContext.AuthenticatedUserId = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserAuthUserId] + 1);

            var cloudContext = telemetryContext.Cloud;
            cloudContext.RoleName = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.CloudRole] + 1);
            cloudContext.RoleInstance = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.CloudRoleInstance] + 1);

            var internalContext = telemetryContext.Internal;
            internalContext.SdkVersion = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalSdkVersion] + 1);
            internalContext.AgentVersion = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalAgentVersion] + 1);
            internalContext.NodeName = new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalNodeName] + 1);

            var tags = telemetryContext.SanitizedTags;

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.ApplicationVersion]), tags[ContextTagKeys.Keys.ApplicationVersion]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceId]), tags[ContextTagKeys.Keys.DeviceId]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceModel]), tags[ContextTagKeys.Keys.DeviceModel]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceOEMName]), tags[ContextTagKeys.Keys.DeviceOEMName]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceOSVersion]), tags[ContextTagKeys.Keys.DeviceOSVersion]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.DeviceType]), tags[ContextTagKeys.Keys.DeviceType]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.LocationIp]), tags[ContextTagKeys.Keys.LocationIp]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationId]), tags[ContextTagKeys.Keys.OperationId]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationName]), tags[ContextTagKeys.Keys.OperationName]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationParentId]), tags[ContextTagKeys.Keys.OperationParentId]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationSyntheticSource]), tags[ContextTagKeys.Keys.OperationSyntheticSource]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.OperationCorrelationVector]), tags[ContextTagKeys.Keys.OperationCorrelationVector]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.SessionId]), tags[ContextTagKeys.Keys.SessionId]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserId]), tags[ContextTagKeys.Keys.UserId]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserAccountId]), tags[ContextTagKeys.Keys.UserAccountId]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.UserAuthUserId]), tags[ContextTagKeys.Keys.UserAuthUserId]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.CloudRole]), tags[ContextTagKeys.Keys.CloudRole]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.CloudRoleInstance]), tags[ContextTagKeys.Keys.CloudRoleInstance]);

            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalSdkVersion]), tags[ContextTagKeys.Keys.InternalSdkVersion]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalAgentVersion]), tags[ContextTagKeys.Keys.InternalAgentVersion]);
            Assert.AreEqual(new string('Z', Property.TagSizeLimits[ContextTagKeys.Keys.InternalNodeName]), tags[ContextTagKeys.Keys.InternalNodeName]);
        }
    }
}