namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActiveSubsciptionManagerTests
    {
        [TestMethod]
        public void AttachAndDetach()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs = new TestSubscription();
            dlSubscription.Attach(subs);

            Assert.IsTrue(dlSubscription.IsActive(subs));

            dlSubscription.Detach(subs);
            Assert.IsFalse(dlSubscription.IsActive(subs));
        }

        [TestMethod]
        public void AttachMultiple()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            Assert.IsTrue(dlSubscription.IsActive(subs1));

            var subs3 = new TestSubscription();
            dlSubscription.Attach(subs3);

            Assert.IsTrue(dlSubscription.IsActive(subs1));
        }

        [TestMethod]
        public void AttachManyAndDetachInactive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            var subs3 = new TestSubscription();
            dlSubscription.Attach(subs3);

            dlSubscription.Detach(subs2);
            Assert.IsTrue(dlSubscription.IsActive(subs1));

            dlSubscription.Detach(subs3);
            Assert.IsTrue(dlSubscription.IsActive(subs1));
        }

        [TestMethod]
        public void AttachManyAndDetachActive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            var subs3 = new TestSubscription();
            dlSubscription.Attach(subs3);

            dlSubscription.Detach(subs1);
            Assert.IsTrue(dlSubscription.IsActive(subs2) ^ dlSubscription.IsActive(subs3));
        }

        [TestMethod]
        public void DetachTwiceInactive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            dlSubscription.Detach(subs2);
            dlSubscription.Detach(subs2);

            Assert.IsTrue(dlSubscription.IsActive(subs1));
        }

        [TestMethod]
        public void DetachTwiceActive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            dlSubscription.Detach(subs1);
            dlSubscription.Detach(subs1);

            Assert.IsTrue(dlSubscription.IsActive(subs2));
        }

        [TestMethod]
        public void AttachTwiceActive()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);
            dlSubscription.Attach(subs1);

            Assert.IsTrue(dlSubscription.IsActive(subs1));

            var subs2 = new TestSubscription();
            dlSubscription.Attach(subs2);

            dlSubscription.Detach(subs1);
            Assert.IsTrue(dlSubscription.IsActive(subs2));
        }

        [TestMethod]
        public void NotAttached()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            Assert.IsFalse(dlSubscription.IsActive(subs1));
        }

        [TestMethod]
        public void AllDetached()
        {
            var dlSubscription = new ActiveSubsciptionManager();

            var subs1 = new TestSubscription();
            dlSubscription.Attach(subs1);
            dlSubscription.Detach(subs1);

            Assert.IsFalse(dlSubscription.IsActive(subs1));
        }

        private class TestSubscription : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}