namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Threading;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ThreadResourceLockTest
    {
        [TestMethod]
        public void LockIsSetOnSameThread()
        {
            Assert.IsFalse(ThreadResourceLock.IsResourceLocked);
            using (var resourceLock = new ThreadResourceLock())
            {
                Assert.IsTrue(ThreadResourceLock.IsResourceLocked);
            }

            Assert.IsFalse(ThreadResourceLock.IsResourceLocked);
        }
    }
}
