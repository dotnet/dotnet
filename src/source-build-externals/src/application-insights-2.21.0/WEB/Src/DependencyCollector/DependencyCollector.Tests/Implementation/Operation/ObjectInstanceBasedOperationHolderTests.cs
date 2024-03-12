namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Net;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Shared WebRequestDependencyTrackingHelpers class tests.
    /// </summary>
    [TestClass]
    public class ObjectInstanceBasedOperationHolderTests
    {
        private Tuple<DependencyTelemetry, bool> telemetryTuple;
        private ObjectInstanceBasedOperationHolder<DependencyTelemetry> objectInstanceBasedOperationHolder;
        private Object obj = new object();

        [TestInitialize]
        public void TestInitialize()
        {
            this.telemetryTuple = new Tuple<DependencyTelemetry, bool>(new DependencyTelemetry(), true);
            this.objectInstanceBasedOperationHolder = new ObjectInstanceBasedOperationHolder<DependencyTelemetry>();
        }

        /// <summary>
        /// Tests the scenario if Store() throws Exception with null object.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoreThrowsExceptionOnNullKey()
        {
            this.objectInstanceBasedOperationHolder.Store(null, this.telemetryTuple);
        }

        /// <summary>
        /// Tests the scenario if Store() adds telemetry tuple to the cache.
        /// </summary>
        [TestMethod]
        public void StoreAddsTelemetryTupleToTheObjectInstance()
        {
            Assert.IsNull(this.objectInstanceBasedOperationHolder.Get(this.obj));
            this.objectInstanceBasedOperationHolder.Store(this.obj, this.telemetryTuple);
            Assert.AreEqual(this.telemetryTuple, this.objectInstanceBasedOperationHolder.Get(this.obj));
        }

        /// <summary>
        /// Tests the scenario if Store() adds telemetry tuple with same id.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StoreThrowsExceptionWhenAddingAlreadyExistingKey()
        {
            Assert.IsNull(this.objectInstanceBasedOperationHolder.Get(this.obj));
            var tuple = new Tuple<DependencyTelemetry, bool>(new DependencyTelemetry(), true);
            this.objectInstanceBasedOperationHolder.Store(this.obj, tuple);
            this.objectInstanceBasedOperationHolder.Store(this.obj, this.telemetryTuple);
        }

        /// <summary>
        /// Tests the scenario if Store() throws exception null tuple.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StoreThrowsExceptionForNullTelemetryTupleInObjectInstance()
        {
            this.objectInstanceBasedOperationHolder.Store(this.obj, null);
        }

        /// <summary>
        /// Tests the scenario if Remove() throws Exception with null object.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveThrowsExceptionOnNullKey()
        {
            this.objectInstanceBasedOperationHolder.Remove(null);
        }

        /// <summary>
        /// Tests the scenario if Remove() removes telemetry tuple from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveDeletesTelemetryTupleFromTheObjectInstance()
        {
            this.objectInstanceBasedOperationHolder.Store(this.obj, this.telemetryTuple);
            Assert.AreEqual(this.telemetryTuple, this.objectInstanceBasedOperationHolder.Get(this.obj));
            this.objectInstanceBasedOperationHolder.Remove(this.obj);
            Assert.IsNull(this.objectInstanceBasedOperationHolder.Get(this.obj));
        }

        /// <summary>
        /// Tests the scenario if Remove() does not throw an exception when it tries to delete a non existing id.
        /// </summary>
        [TestMethod]
        public void RemoveDoesNotThrowExceptionForNonExistingItemFromTheObjectInstance()
        {
            this.objectInstanceBasedOperationHolder.Store(this.obj, this.telemetryTuple);
            Assert.IsTrue(this.objectInstanceBasedOperationHolder.Remove(this.obj));
            Assert.IsFalse(this.objectInstanceBasedOperationHolder.Remove(this.obj));
            Assert.IsFalse(this.objectInstanceBasedOperationHolder.Remove(this.obj));
        }

        /// <summary>
        /// Tests the scenario if Get retrieves the tuple that corresponds to the given id.
        /// </summary>
        [TestMethod]
        public void GetReturnsItemIfItExistsInTheObjectInstanceTable()
        {
            this.objectInstanceBasedOperationHolder.Store(this.obj, this.telemetryTuple);
            Assert.AreEqual(this.telemetryTuple, this.objectInstanceBasedOperationHolder.Get(this.obj));
        }

        /// <summary>
        /// Tests the scenario if Get returns null for a non existing item in the table.
        /// </summary>
        [TestMethod]
        public void GetReturnsNullIfIdDoesNotExistInObjectInstance()
        {
            Assert.IsNull(this.objectInstanceBasedOperationHolder.Get(this.obj));
        }

        /// <summary>
        /// Tests the scenario if Remove() throws Exception with null object.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetThrowsExceptionOnNullKey()
        {
            this.objectInstanceBasedOperationHolder.Get(null);
        }
    }
}
