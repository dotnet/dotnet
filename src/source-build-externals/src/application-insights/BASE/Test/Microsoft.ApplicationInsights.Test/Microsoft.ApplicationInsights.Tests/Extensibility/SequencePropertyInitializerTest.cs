namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Reflection;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class SequencePropertyInitializerTest
    {
        [TestMethod]
        public void ClassIsPublicToAllowMicrosoftApplicationDevelopersToSendTelemetryToVortex()
        {
            Assert.IsTrue(typeof(SequencePropertyInitializer).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void ClassImplementsITelemetryInitializerBecauseSequenceChangesForEachTelemetry()
        {
            Assert.IsTrue(typeof(ITelemetryInitializer).GetTypeInfo().IsAssignableFrom(typeof(SequencePropertyInitializer).GetTypeInfo()));
        }

        [TestMethod]
        public void InitializeSetsSequencePropertyValue()
        {
            var telemetry = new StubTelemetry();
            new SequencePropertyInitializer().Initialize(telemetry);
            Assert.AreNotEqual(string.Empty, telemetry.Sequence);
        }

        [TestMethod]
        public void InitializePreservesExistingSequencePropertyValue()
        {
            string originalValue = Guid.NewGuid().ToString();
            var telemetry = new StubTelemetry { Sequence = originalValue };
            new SequencePropertyInitializer().Initialize(telemetry);
            Assert.AreEqual(originalValue, telemetry.Sequence);
        }

        [TestMethod]
        public void InitializeGeneratesUniqueSequenceValuesWhenCalledMultipleTimes()
        {
            var initializer = new SequencePropertyInitializer();

            var telemetry1 = new StubTelemetry();
            initializer.Initialize(telemetry1);
            var telemetry2 = new StubTelemetry();
            initializer.Initialize(telemetry2);

            Assert.AreNotEqual(telemetry1.Sequence, telemetry2.Sequence);
        }

        [TestMethod]
        public void InitializeGeneratesUniqueValuesWhenCalledOnMultipleInstances()
        {
            var telemetry1 = new StubTelemetry();
            new SequencePropertyInitializer().Initialize(telemetry1);

            var telemetry2 = new StubTelemetry();
            new SequencePropertyInitializer().Initialize(telemetry2);

            Assert.AreNotEqual(telemetry1.Sequence, telemetry2.Sequence);
        }

        [TestMethod]
        public void InitializeSeparatesStableIdAndNumberWithColonToConformWithVortexSpecification()
        {
            var telemetry = new StubTelemetry();
            new SequencePropertyInitializer().Initialize(telemetry);
            AssertEx.Contains(":", telemetry.Sequence, StringComparison.Ordinal);
        }

        [TestMethod]
        public void InitializeDoesNotIncludeBase64PaddingInSequenceToReduceDataSize()
        {
            var telemetry = new StubTelemetry();
            new SequencePropertyInitializer().Initialize(telemetry);
            AssertEx.DoesNotContain("=", telemetry.Sequence, StringComparison.Ordinal);
        }
    }
}
