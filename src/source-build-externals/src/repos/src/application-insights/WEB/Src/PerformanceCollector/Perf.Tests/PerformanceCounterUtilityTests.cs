#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// PerformanceCounterUtility tests.
    /// </summary>
    [TestClass]
    public class PerformanceCounterUtilityTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
        
        [TestMethod]
        [TestCategory("RequiresPerformanceCounters")]
        public void PerformanceCounterUtilityPlaceholderExpansionTest()
        {
            PerformanceCounterUtility.InvalidatePlaceholderCache();

            var win32Instances = PerformanceCounterUtility.GetWin32ProcessInstances();
            var clrInstances = PerformanceCounterUtility.GetClrProcessInstances();

            bool usesInstanceNamePlaceholder;
            var pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Processor(??APP_WIN32_PROC??)\% Processor Time", win32Instances, clrInstances, true, out usesInstanceNamePlaceholder);
            Assert.IsFalse(pc.InstanceName.Contains("?"));

            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Processor(??APP_CLR_PROC??)\% Processor Time", win32Instances, clrInstances, true, out usesInstanceNamePlaceholder);
            Assert.IsFalse(pc.InstanceName.Contains("?"));

            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Processor(??APP_W3SVC_PROC??)\% Processor Time", win32Instances, clrInstances, true, out usesInstanceNamePlaceholder);
            Assert.IsFalse(pc.InstanceName.Contains("?"));

            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time", win32Instances, clrInstances, true, out usesInstanceNamePlaceholder);
            Assert.IsFalse(pc.InstanceName.Contains("?"));
            
            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Processor(??NON_EXISTENT??)\% Processor Time", win32Instances, clrInstances, true, out usesInstanceNamePlaceholder);
            Assert.AreEqual("??NON_EXISTENT??", pc.InstanceName);

            // validate placeholder cache state
            var cache =
                new PrivateType(typeof(PerformanceCounterUtility)).GetStaticField(
                    "PlaceholderCache",
                    BindingFlags.NonPublic) as ConcurrentDictionary<string, string>;

            Assert.AreEqual(3, cache.Count);
            Assert.IsTrue(cache.ContainsKey("APP_WIN32_PROC"));
            Assert.IsTrue(cache.ContainsKey("APP_CLR_PROC"));
            Assert.IsTrue(cache.ContainsKey("APP_W3SVC_PROC"));

            PerformanceCounterUtility.InvalidatePlaceholderCache();

            Assert.IsFalse(cache.Any());
        }

        [TestMethod]
        public void PerformanceCounterUtilitySanityTest()
        {
            var win32Instances = PerformanceCounterUtility.GetWin32ProcessInstances();
            var clrInstances = PerformanceCounterUtility.GetClrProcessInstances();

            var win32Instance = PerformanceCounterUtility.GetInstanceForWin32Process(win32Instances);
            var clrInstance = PerformanceCounterUtility.GetInstanceForClrProcess(clrInstances);

            PerformanceCounterUtility.GetInstanceForCurrentW3SvcWorker();
        }

        [TestMethod]
        public void DomainNameParsingTest()
        {
            Assert.AreEqual("_LM_W3SVC_1_ROOT_Directory_SiteName", PerformanceCounterUtility.GetInstanceFromApplicationDomain("/LM/W3SVC/1/ROOT/Directory/SiteName-1-130560686698179161"));
            Assert.AreEqual("_LM_W3SVC_1_ROOT-1_Directory-1_SiteName-1", PerformanceCounterUtility.GetInstanceFromApplicationDomain("/LM/W3SVC/1/ROOT-1/Directory-1/SiteName-1-1-130560686698179161"));
        }

        [TestMethod]
        public void ParsePerformanceCounterTest()
        {
            PerformanceCounterStructure pc;

            bool usesInstanceNamePlaceholder;
            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Processor(_Total)\% Processor Time", null, null, true, out usesInstanceNamePlaceholder);
            Assert.AreEqual("Processor", pc.CategoryName);
            Assert.AreEqual("% Processor Time", pc.CounterName);
            Assert.AreEqual("_Total", pc.InstanceName);

            pc = PerformanceCounterUtility.ParsePerformanceCounter(@"\Memory\Available Memory", null, null, true, out usesInstanceNamePlaceholder);
            Assert.AreEqual("Memory", pc.CategoryName);
            Assert.AreEqual("Available Memory", pc.CounterName);
            Assert.AreEqual(string.Empty, pc.InstanceName);
        }
    }
}
#endif