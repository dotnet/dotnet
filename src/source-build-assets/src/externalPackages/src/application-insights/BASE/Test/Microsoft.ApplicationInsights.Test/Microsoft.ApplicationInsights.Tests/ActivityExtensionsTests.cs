namespace Microsoft.ApplicationInsights
{
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class ActivityExtensionsTests
    {
        [TestMethod]
        public void CanLoadDiagnosticSourceAssembly()
        {
            Assert.IsTrue(ActivityExtensions.TryRun(() => Assert.IsNull(Activity.Current)));
        }

        [TestMethod]
        public void GetOperationNameReturnsNullIfThereIsNoOperationName()
        {
            var activity = new Activity("test me");
            Assert.IsNull(activity.GetOperationName());
        }

        [TestMethod]
        public void SetOperationNameIsConsistentWithGetOperationName()
        {
            var activity = new Activity("test");
            activity.SetOperationName("test me");
            Assert.AreEqual("test me", activity.GetOperationName());
        }

        [TestMethod]
        public void GetOperationNameReturnsFirstAddedOperationName()
        {
            var activity = new Activity("test");
            activity.AddTag("OperationName", "test me 1");
            activity.AddTag("OperationName", "test me 2");
#if !REDFIELD
            Assert.AreEqual("test me 1", activity.GetOperationName());
#endif
        }
    }
}