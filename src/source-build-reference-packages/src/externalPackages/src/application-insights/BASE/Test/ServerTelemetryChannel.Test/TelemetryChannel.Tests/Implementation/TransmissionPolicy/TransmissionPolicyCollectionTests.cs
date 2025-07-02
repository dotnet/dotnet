namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("TransmissionPolicy")]
    public class TransmissionPolicyCollectionTests
    {
        [TestMethod]
        public void VerifyCalcuateMinimums()
        {
            // Setup
            var policies = new TransmissionPolicyCollection(policies: new List<TransmissionPolicy>
            {
                new MockValueTransmissionPolicy(maxSenderCapacity: null, maxBufferCapacity: null, maxStorageCapacity: null),
                new MockValueTransmissionPolicy(maxSenderCapacity: 1, maxBufferCapacity: 2, maxStorageCapacity: 3),
                new MockValueTransmissionPolicy(maxSenderCapacity: 101, maxBufferCapacity: 102, maxStorageCapacity: 103),
            });

            // Act & Verify
            Assert.AreEqual(1, policies.CalculateMinimumMaxSenderCapacity());
            Assert.AreEqual(2, policies.CalculateMinimumMaxBufferCapacity());
            Assert.AreEqual(3, policies.CalculateMinimumMaxStorageCapacity());
        }


        [TestMethod]
        public void VerifyCalcuateMinimums_CanHandleNulls()
        {
            // Setup
            var policies = new TransmissionPolicyCollection(policies: new List<TransmissionPolicy>
            {
                new MockValueTransmissionPolicy(maxSenderCapacity: null, maxBufferCapacity: null, maxStorageCapacity: null),
            });

            // Act & Verify
            Assert.AreEqual(null, policies.CalculateMinimumMaxSenderCapacity());
            Assert.AreEqual(null, policies.CalculateMinimumMaxBufferCapacity());
            Assert.AreEqual(null, policies.CalculateMinimumMaxStorageCapacity());
        }

        private class MockValueTransmissionPolicy : TransmissionPolicy
        {
            public MockValueTransmissionPolicy(int? maxSenderCapacity, int? maxBufferCapacity, int? maxStorageCapacity)
            {
                this.MaxSenderCapacity = maxSenderCapacity;
                this.MaxBufferCapacity = maxBufferCapacity;
                this.MaxStorageCapacity = maxStorageCapacity;
            }
        }
    }
}
