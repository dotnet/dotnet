#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Runtime.InteropServices;

    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CounterDefinitionSampleTests
    {
        [TestMethod]
        public void CounterDefinitionSampleReadsInstanceValueCorrectlyInt32()
        {
            // ARRANGE
            var perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION()
                                  {
                                      CounterNameTitleIndex = 0,
                                      CounterType = 0,
                                      CounterOffset = 6,
                                      CounterSize = 4
                                  };

            var data = new byte[] 
            {
                0, 0, 0, 0,
                0, 0, 5, 0,
                0, 0, 0, 0
            };

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            IntPtr dataRef = handle.AddrOfPinnedObject();

            var sample = new CounterDefinitionSample(perfCounter, -1);

            // ACT
            sample.SetInstanceValue(0, dataRef);
            
            // ASSERT
            Assert.AreEqual(5, sample.GetInstanceValue(0));

            handle.Free();
        }

        [TestMethod]
        public void CounterDefinitionSampleReadsInstanceValueCorrectlyInt64()
        {
            // ARRANGE
            var perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION()
            {
                CounterNameTitleIndex = 0,
                CounterType = 0,
                CounterOffset = 6,
                CounterSize = 8
            };

            var data = new byte[]
            {
                0, 0, 0, 0,
                0, 0, 5, 0,
                0, 0, 0, 0
            };

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            IntPtr dataRef = handle.AddrOfPinnedObject();

            var sample = new CounterDefinitionSample(perfCounter, -1);

            // ACT
            sample.SetInstanceValue(0, dataRef);

            // ASSERT
            Assert.AreEqual(5, sample.GetInstanceValue(0));

            handle.Free();
        }
    }
}
#endif