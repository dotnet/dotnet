// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Abstractions;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Logging.Tests
{
    public class TelemetryEventDetailsTests
    {
        [Fact]
        public void TestNameSetGet()
        {
            var mock = new Mock<TelemetryEventDetails>();
            mock.CallBase = true;
            const string eventName = "Create_Widget";
            mock.Object.Name = eventName;
            Assert.Equal(eventName, mock.Object.Name);
        }

        [Fact]
        public void SetPropertyArgumentNullException()
        {
            var mock = new Mock<TelemetryEventDetails>();
            mock.CallBase = true;
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, "string"));
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, false));
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, 1L));
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, DateTime.UtcNow));
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, 1.0d));
            Assert.Throws<ArgumentNullException>("key", () => mock.Object.SetProperty(null, Guid.NewGuid()));
        }

        [Fact]
        public void SetPropertyNoConflict()
        {
            Dictionary<string, object> expected = new Dictionary<string, object>()
            {
                { "string", "string" },
                { "bool", true },
                { "long", 1L },
                { "DateTime", DateTime.Now },
                { "double", 1.0d },
                { "Guid", Guid.NewGuid() }
            };

            var mock = new Mock<TelemetryEventDetails>();
            mock.CallBase = true;
            mock.Object.SetProperty("string", (string)expected["string"]);
            mock.Object.SetProperty("bool", (bool)expected["bool"]);
            mock.Object.SetProperty("long", (long)expected["long"]);
            mock.Object.SetProperty("DateTime", (DateTime)expected["DateTime"]);
            mock.Object.SetProperty("double", (double)expected["double"]);
            mock.Object.SetProperty("Guid", (Guid)expected["Guid"]);

            Assert.Equal(expected.Count, mock.Object.Properties.Count);
            foreach (var item in mock.Object.Properties)
            {
                Assert.Equal(expected[item.Key], item.Value);
            }
        }

        [Fact]
        public void SetPropertyWithConflict()
        {
            var mock = new Mock<TelemetryEventDetails>();
            mock.CallBase = true;
            mock.Object.SetProperty("key1", "value");
            mock.Object.SetProperty("key1", false);

            Assert.Equal(1, mock.Object.Properties.Count);
            Assert.False((bool)mock.Object.Properties["key1"]);
        }

        [Fact]
        public void VerifyDerivedTypesCanAddArbitraryTypes()
        {
            CustomTelemetryEventDetails eventDetails = new CustomTelemetryEventDetails();
            eventDetails.SetProperty("foo", new Foo() { Bar = "bar" });

            Assert.Equal(1, eventDetails.Properties.Count);
            Assert.Equal("bar", (eventDetails.Properties["foo"] as Foo)?.Bar);
        }

        [Fact]
        public void VerifyExtensibilityForClassification()
        {
            TelemetryEventDetailsWithClassification eventDetails = new TelemetryEventDetailsWithClassification();
            eventDetails.SetProperty("Protocol", "Bearer");
            eventDetails.SetProperty("TokenIdentity", "Bob Jones", true);
            eventDetails.SetProperty("DataCenter", "CO1", false);

            Assert.Equal(3, eventDetails.Properties.Count);
            Assert.True(eventDetails.IsPersonalData["TokenIdentity"]);
            Assert.False(eventDetails.IsPersonalData["Protocol"]);
            Assert.False(eventDetails.IsPersonalData["DataCenter"]);
        }

        #region Dummy Implementation
        internal class TelemetryEventDetailsWithClassification : TelemetryEventDetails
        {
            internal IDictionary<string, bool> PersonalDataDecoration = new Dictionary<string, bool>();

            public IReadOnlyDictionary<string, bool> IsPersonalData
            {
                get
                {
                    return (IReadOnlyDictionary<string, bool>)PersonalDataDecoration;
                }
            }

            public override void  SetProperty(
                string key,
                string value)
            {
                SetPropertyCore(key, value);
            }

            public void SetProperty(
               string key,
               string value,
               bool isPersonal)
            {
                SetPropertyCore(key, value, isPersonal);
            }

            private void SetPropertyCore(
                string key,
                object value,
                bool isPersonal = false)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                PropertyValues[key] = value;
                PersonalDataDecoration[key] = isPersonal;
            }
        }

        internal class CustomTelemetryEventDetails : TelemetryEventDetails
        {
            internal void SetProperty(
                string key,
                Foo value)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                PropertyValues[key] = value;
            }
        }

        internal class Foo
        {
            public string Bar { get; set; }
        }
        #endregion Dummy Implementation
    }
}
