namespace Microsoft.ApplicationInsights.Extensibility
{
    using System.Diagnostics;
    using System.Linq;
    using Implementation;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationCorrelationTelemetryInitializerTests
    {
        private static TelemetryConfiguration tc;

        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            // Constructor on TelemetryConfiguration forces Activity.IDFormat to be W3C
            // OperationCorrelationTelemetryInitializer has no responsibility to set the Activity Format.
            // It expects Activity to use W3CFormat, but falls back to use Hierrarchial Id.
            tc = new TelemetryConfiguration();
        }

        [TestInitialize]
        public void TestInit()
        {            
            ActivityFormatHelper.EnableW3CFormatInActivity();
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializerDoesNotFailOnNullCurrentActivity()
        {
            // Arrange
            // Does not start Activity.
            
            var telemetry = new DependencyTelemetry();

            // Act
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            // Validate
            // Initialize is a no-op, and no exceptions thrown.
            Assert.IsNull(telemetry.Context.Operation.Id);
            Assert.IsNull(telemetry.Context.Operation.ParentId);
            Assert.IsFalse(telemetry.Properties.Any());
        }

        [TestMethod]
        public void InitializePopulatesOperationContextFromActivity()
        {
            // Arrange
            Activity activity = new Activity("somename");
            activity.Start();
            var telemetry = new DependencyTelemetry();
            var originalTelemetryId = telemetry.Id;

            // Act
            var initializer = new OperationCorrelationTelemetryInitializer();            
            initializer.Initialize(telemetry);

            // Validate
            Assert.AreEqual(activity.TraceId.ToHexString(), telemetry.Context.Operation.Id, "OperationCorrelationTelemetryInitializer is expected to populate OperationID from Activity");
            Assert.AreEqual(activity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId,
                "OperationCorrelationTelemetryInitializer is expected to populate Operation ParentID as |traceID.SpanId. from Activity");
            Assert.AreEqual(originalTelemetryId, telemetry.Id, "OperationCorrelationTelemetryInitializer is not expected to modify Telemetry ID");
            activity.Stop();
        }

        [TestMethod]
        public void InitializePopulatesOperationContextFromActivityWhenW3CIsDisabled()
        {
            // Arrange
            ActivityFormatHelper.DisableW3CFormatInActivity();
            try
            {
                Activity parent = new Activity("parent");

                // Setting parentid like this forces Activity to use Hierarchical ID Format
                parent.SetParentId("parent");
                parent.Start();

                var telemetry = new DependencyTelemetry();
                var initializer = new OperationCorrelationTelemetryInitializer();

                // Act
                initializer.Initialize(telemetry);

                // Validate
                Assert.AreEqual("parent", telemetry.Context.Operation.Id);
                Assert.AreEqual(parent.Id, telemetry.Context.Operation.ParentId);
                parent.Stop();
            }
            finally
            {
                ActivityFormatHelper.EnableW3CFormatInActivity();
            }
        }

        [TestMethod]
        public void InitializeDoesNotOverrideOperationIdIfItExists()
        {
            // Arrange
            var telemetry = new DependencyTelemetry();
            telemetry.Context.Operation.ParentId = "OldParentOperationId";
            telemetry.Context.Operation.Id = "OldOperationId";
            var initializer = new OperationCorrelationTelemetryInitializer();
            Activity parent = new Activity("parent");
            parent.Start();

            // Act
            initializer.Initialize(telemetry);

            // Validate
            Assert.AreEqual("OldParentOperationId", telemetry.Context.Operation.ParentId);
            Assert.AreEqual("OldOperationId", telemetry.Context.Operation.Id);            
            parent.Stop();
        }

        [TestMethod]
        public void InitializeDoesNotOverrideEmptyParentIdIfOperationIdExists()
        {
            // Arrange
            var telemetry = new DependencyTelemetry();
            telemetry.Context.Operation.Id = "OldOperationId";
            // Does not set parentid and hence it'll be empty
            var initializer = new OperationCorrelationTelemetryInitializer();
            Activity parent = new Activity("parent");
            parent.Start();

            initializer.Initialize(telemetry);

            Assert.IsNull(telemetry.Context.Operation.ParentId, "Operation.ParentID should not be overwritten when Operation.ID is already present");
            Assert.AreEqual("OldOperationId", telemetry.Context.Operation.Id, "Operation should not be overwritten");
            parent.Stop();
        }

        [TestMethod]
        public void InitializeDoesntOverrideContextIfOperationIdSet()
        {
            var currentActivity = new Activity("test");
            currentActivity.AddTag("OperationName", "operation");
            currentActivity.AddBaggage("k1", "v1");
            currentActivity.AddBaggage("k2", "v2");
            currentActivity.Start();
            var telemetry = new RequestTelemetry();
            telemetry.Context.Operation.Id = "operationId";
            telemetry.Context.Operation.ParentId = null;
            telemetry.Context.Operation.Name = "operation";
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual("operationId", telemetry.Context.Operation.Id);
            Assert.IsNull(telemetry.Context.Operation.ParentId);
            Assert.AreEqual("operation", telemetry.Context.Operation.Name);
            Assert.AreEqual(0, telemetry.Properties.Count);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeOverridesContextIfOperationIdIsNotSet()
        {
            var currentActivity = new Activity("test");
            currentActivity.AddTag("OperationName", "operation");
            currentActivity.AddBaggage("k1", "v1");
            currentActivity.Start();
            var telemetry = new TraceTelemetry();

            telemetry.Context.Operation.ParentId = "parentId";
            telemetry.Context.Operation.Name = "operation";
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual(currentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.AreEqual("parentId", telemetry.Context.Operation.ParentId);
            Assert.AreEqual("operation", telemetry.Context.Operation.Name);
            Assert.AreEqual(1, telemetry.Properties.Count);
            Assert.AreEqual("v1", telemetry.Properties["k1"]);
            currentActivity.Stop();
        }

        [TestMethod]
        public void TelemetryContextIsUpdatedWithOperationNameForDependencyTelemetry()
        {
            Activity parent = new Activity("parent");
            parent.AddTag("OperationName", "OperationName");
            parent.Start();

            var telemetry = new DependencyTelemetry();
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);
            Assert.AreEqual(telemetry.Context.Operation.Name, "OperationName");
            parent.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityWithoutOperationName()
        {
            var currentActivity = new Activity("test");
            currentActivity.Start();
            var telemetry = new RequestTelemetry();

            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.IsNull(telemetry.Context.Operation.Name);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityWithOperationName()
        {
            var currentActivity = new Activity("test");
            currentActivity.AddTag("OperationName", "OperationName");
            currentActivity.Start();
            var telemetry = new RequestTelemetry();

            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual("OperationName", telemetry.Context.Operation.Name);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeDoesNotUpdateOperationNameIfItExists()
        {
            Activity parent = new Activity("parent");
            parent.AddTag("OperationName", "OperationName");
            parent.Start();

            var telemetry = new DependencyTelemetry();
            telemetry.Context.Operation.Name = "OldOperationName";
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual("OldOperationName",telemetry.Context.Operation.Name);
            parent.Stop();
        }

        [TestMethod]
        public void InitializeSetsBaggage()
        {
            var currentActivity = new Activity("test");
            currentActivity.AddTag("OperationName", "operation");
            currentActivity.AddBaggage("k1", "v1");
            currentActivity.AddBaggage("k2", "v2");
            currentActivity.AddBaggage("existingkey", "exitingvalue");
            currentActivity.Start();
            var telemetry = new RequestTelemetry();
            telemetry.Properties.Add("existingkey", "exitingvalue");
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual(currentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
            Assert.AreEqual("operation", telemetry.Context.Operation.Name);

            Assert.AreEqual(3, telemetry.Properties.Count);
            Assert.AreEqual("v1", telemetry.Properties["k1"]);
            Assert.AreEqual("v2", telemetry.Properties["k2"]);
            Assert.AreEqual("exitingvalue", telemetry.Properties["existingkey"], "OperationCorrelationTelemetryInitializer should not override existing telemetry property bag");
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityWinsOverCallContext()
        {
            CallContextHelpers.SaveOperationContext(new OperationContextForCallContext { RootOperationId = "callContextRoot" });
            var currentActivity = new Activity("test");
            currentActivity.AddTag("OperationName", "operation");
            currentActivity.AddBaggage("k1", "v1");
            currentActivity.Start();
            var telemetry = new RequestTelemetry();
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual(currentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
            Assert.AreEqual(currentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);

            Assert.AreEqual("operation", telemetry.Context.Operation.Name);
            Assert.AreEqual(1, telemetry.Properties.Count);
            Assert.AreEqual("v1", telemetry.Properties["k1"]);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityRecorded()
        {
            var currentActivity = new Activity("test");
            currentActivity.ActivityTraceFlags |= ActivityTraceFlags.Recorded;
            currentActivity.Start();
            var request = new RequestTelemetry();

            (new OperationCorrelationTelemetryInitializer()).Initialize(request);

            Assert.AreEqual(SamplingDecision.SampledIn, request.ProactiveSamplingDecision);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityRecordedOperationIdSet()
        {
            var currentActivity = new Activity("test");
            currentActivity.ActivityTraceFlags |= ActivityTraceFlags.Recorded;
            currentActivity.Start();
            var request = new RequestTelemetry();
            request.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();

            (new OperationCorrelationTelemetryInitializer()).Initialize(request);

            Assert.AreEqual(SamplingDecision.SampledIn, request.ProactiveSamplingDecision);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityNotRecorded()
        {
            var currentActivity = new Activity("test");
            currentActivity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
            currentActivity.Start();
            var telemetry = new RequestTelemetry();

            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.AreEqual(SamplingDecision.None, telemetry.ProactiveSamplingDecision);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityRecordedDoesNotOverrideSampledInIfSet()
        {
            var currentActivity = new Activity("test");
            currentActivity.ActivityTraceFlags |= ActivityTraceFlags.Recorded;
            currentActivity.Start();
            var request = new RequestTelemetry
            {
                ProactiveSamplingDecision = SamplingDecision.SampledOut
            };
            (new OperationCorrelationTelemetryInitializer()).Initialize(request);

            Assert.AreEqual(SamplingDecision.SampledOut, request.ProactiveSamplingDecision);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeWithActivityNotRecordedDoesNotOverrideSampledInIfSet()
        {
            var currentActivity = new Activity("test");
            currentActivity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
            currentActivity.Start();
            var request = new RequestTelemetry
            {
                ProactiveSamplingDecision = SamplingDecision.SampledIn
            };


            (new OperationCorrelationTelemetryInitializer()).Initialize(request);

            Assert.AreEqual(SamplingDecision.SampledIn, request.ProactiveSamplingDecision);
            currentActivity.Stop();
        }

        [TestMethod]
        public void InitializeOnActivityWithTracestate()
        {
            Activity parent = new Activity("parent")
            {
                TraceStateString = "some=state"
            };
            parent.Start();

            var telemetry = new DependencyTelemetry();
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.IsTrue(telemetry.Properties.ContainsKey("tracestate"));
            Assert.AreEqual("some=state", telemetry.Properties["tracestate"]);
        }

        [TestMethod]
        public void InitializeOnActivityWithTracestateW3COff()
        {
            ActivityFormatHelper.DisableW3CFormatInActivity();

            Activity parent = new Activity("parent")
            {
                TraceStateString = "some=state"
            };
            parent.Start();

            var telemetry = new DependencyTelemetry();
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.IsFalse(telemetry.Properties.ContainsKey("tracestate"));
        }

        [TestMethod]
        public void InitializeOnActivityWithTracestateWhenPropertyAlreadyExists()
        {
            Activity parent = new Activity("parent")
            {
                TraceStateString = "some=state"
            };
            parent.Start();

            var telemetry = new DependencyTelemetry();
            telemetry.Properties.Add("tracestate", "123");
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.IsTrue(telemetry.Properties.ContainsKey("tracestate"));
            Assert.AreEqual("123", telemetry.Properties["tracestate"]);
        }


        [TestMethod]
        public void InitializeOnActivityWithTracestateNotOperationTelemetry()
        {
            Activity parent = new Activity("parent")
            {
                TraceStateString = "some=state"
            };
            parent.Start();

            var telemetry = new TraceTelemetry();

            // does not throw
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);
        }

        [TestMethod]
        public void InitializeOnActivityWithTracestateAndOperationIdSet()
        {
            Activity parent = new Activity("parent")
            {
                TraceStateString = "some=state"
            };
            parent.Start();

            var telemetry = new DependencyTelemetry();
            telemetry.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();
            (new OperationCorrelationTelemetryInitializer()).Initialize(telemetry);

            Assert.IsTrue(telemetry.Properties.ContainsKey("tracestate"));
            Assert.AreEqual("some=state", telemetry.Properties["tracestate"]);
        }
    }
}