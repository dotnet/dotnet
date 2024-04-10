namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests corresponding to TelemetryClientExtension methods.
    /// </summary>
    [TestClass]
    public class OperationHolderTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ActivityFormatHelper.EnableW3CFormatInActivity();
        }

        /// <summary>
        /// Tests the scenario if OperationItem throws ArgumentNullException with null telemetry client.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingOperationItemWithNullTelemetryClientThrowsArgumentNullException()
        {
            var operationItem = new OperationHolder<DependencyTelemetry>(null, new DependencyTelemetry());
        }

        /// <summary>
        /// Tests the scenario if OperationItem throws ArgumentNullException with null telemetry.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingOperationItemWithNullTelemetryThrowsArgumentNullException()
        {
            var operationItem = new OperationHolder<DependencyTelemetry>(new TelemetryClient(TelemetryConfiguration.CreateDefault()), null);
        }

        /// <summary>
        /// Tests the scenario if creating OperationItem does not throw exception if arguments are not null.
        /// </summary>
        [TestMethod]
        public void CreatingOperationItemDoesNotThrowOnPassingValidArguments()
        {
            var operationItem = new OperationHolder<DependencyTelemetry>(new TelemetryClient(TelemetryConfiguration.CreateDefault()), new DependencyTelemetry());
        }

        [TestMethod]
        public void CreatingOperationHolderWithDetachedOriginalActivityRestoresIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());

            var originalActivity = new Activity("original").Start();
            var operation = new OperationHolder<DependencyTelemetry>(client, new DependencyTelemetry(), originalActivity);

            var newActivity = new Activity("new").SetParentId("detached-parent").Start();
            operation.Telemetry.Id = newActivity.SpanId.ToHexString();

            operation.Dispose();
            Assert.AreEqual(Activity.Current, originalActivity);
        }

        [TestMethod]
        public void CreatingOperationHolderWithNullOriginalActivityDoesNotRestoreIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());

            var originalActivity = new Activity("original").Start();
            var operation = new OperationHolder<DependencyTelemetry>(client, new DependencyTelemetry(), null);

            var newActivity = new Activity("new").SetParentId("detached-parent").Start();
            operation.Telemetry.Id = newActivity.SpanId.ToHexString();

            operation.Dispose();

#if NET6_0_OR_GREATER
            Assert.IsNotNull(Activity.Current);
#else
            Assert.IsNull(Activity.Current);
#endif
        }

        [TestMethod]
        public void CreatingOperationHolderWithParentActivityRestoresIt()
        {
            var client = new TelemetryClient(TelemetryConfiguration.CreateDefault());

            var originalActivity = new Activity("original").Start();
            var operation = new OperationHolder<DependencyTelemetry>(client, new DependencyTelemetry(), originalActivity);

            // child of original
            var newActivity = new Activity("new").Start();
            operation.Telemetry.Id = newActivity.SpanId.ToHexString();
            operation.Dispose();
            Assert.AreEqual(Activity.Current, originalActivity);
        }

    }
}
