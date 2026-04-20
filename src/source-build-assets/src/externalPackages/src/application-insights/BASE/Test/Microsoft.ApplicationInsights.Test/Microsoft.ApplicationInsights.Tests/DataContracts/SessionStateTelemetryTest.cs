namespace Microsoft.ApplicationInsights.DataContracts
{
    using AI;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using KellermanSoftware.CompareNetObjects;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    [TestClass]
    public class SessionStateTelemetryTest
    {
#pragma warning disable 618
        [TestMethod]
        public void SessionStateTelemetryImplementsITelemetryContract()
        {
            var test = new ITelemetryTest<SessionStateTelemetry, AI.EventData>();
            test.Run();
        }

        [TestMethod]
        public void ConstructorInitializesStateWithSpecifiedValue()
        {
            var telemetry = new SessionStateTelemetry(SessionState.End);
            Assert.AreEqual(SessionState.End, telemetry.State);
        }

        [TestMethod]
        public void ParameterizedConstructorPerformsDefaultInitialization()
        {
            var telemetry = new SessionStateTelemetry(SessionState.Start);
            Assert.IsNotNull(telemetry.Context);
        }

        [TestMethod]
        public void SessionStateIsStartByDefault()
        {
            var telemetry = new SessionStateTelemetry();
            Assert.AreEqual(SessionState.Start, telemetry.State);
        }

        [TestMethod]
        public void SessionStateCanBeSetByUser()
        {
            var telemetry = new SessionStateTelemetry();
            telemetry.State = SessionState.End;
            Assert.AreEqual(SessionState.End, telemetry.State);
        }

        [TestMethod]
        public void SerializeWritesStateAsExpectedByEndpoint()
        {
            var telemetry = new SessionStateTelemetry { State = SessionState.End };
            TelemetryItem<EventData> envelope = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<EventData>(telemetry);
            Assert.AreEqual("Session ended", envelope.data.baseData.name);
            Assert.AreEqual(2, envelope.data.baseData.ver);
        }

        [TestMethod]
        public void SessionStateTelemetryDeepCloneCopiesAllProperties()
        {
            var telemetry = new SessionStateTelemetry();
            telemetry.State = SessionState.End;
            telemetry.Extension = new MyTestExtension();
            var other = telemetry.DeepClone();

            CompareLogic deepComparator = new CompareLogic();

            var result = deepComparator.Compare(telemetry, other);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod]
        public void SessionStateTelemetryDeepCloneWithNullExtensionDoesNotThrow()
        {
            var telemetry = new SessionStateTelemetry();
            // Extension is not set, means it'll be null.
            // Validate that cloning with null Extension does not throw.
            var other = telemetry.DeepClone();
        }

#pragma warning restore 618
    }
}
