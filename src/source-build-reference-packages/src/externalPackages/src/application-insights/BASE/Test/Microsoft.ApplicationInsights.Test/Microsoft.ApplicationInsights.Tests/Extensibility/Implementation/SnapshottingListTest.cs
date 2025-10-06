// <copyright file="SnapshottingListTest.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class SnapshottingListTest
    {
        [TestClass]
        public class Class : SnapshottingListTest
        {
            [TestMethod]
            public void IsInternalAndNotMeantForPublicConsumption()
            {
                Assert.IsFalse(typeof(SnapshottingList<>).GetTypeInfo().IsPublic);
            }

            [TestMethod]
            public void ClassImplementsIListForCompatibilityWithPublicApis()
            {
                Assert.IsTrue(typeof(IList<object>).IsAssignableFrom(typeof(SnapshottingList<object>)));
            }
        }

        [TestClass]
        public class Constructor : SnapshottingListTest
        {
            [TestMethod]
            public void CreatesNewList()
            {
                var target = new TestableSnapshottingList<object>();
                Assert.IsNotNull(target.Collection);
            }
        }

        [TestClass]
        public class CreateSnapshot : SnapshottingListTest
        {
            [TestMethod]
            public void CreatesCloneOfGivenList()
            {
                var target = new TestableSnapshottingList<object>();

                var item = new object();
                var input = new List<object> { item };
                IList<object> output = target.CreateSnapshot(input);

                Assert.AreSame(item, output[0]);
            }
        }

        [TestClass]
        public class IndexOf : SnapshottingListTest
        {
            [TestMethod]
            public void ReturnsIndexOfGivenItemInSnapshot()
            {
                var target = new TestableSnapshottingList<object>();
                var item = new object();
                target.Snapshot = new List<object> { item };

                Assert.AreEqual(0, target.IndexOf(item));
            }
        }

        [TestClass]
        public class Insert : SnapshottingCollectionTest
        {
            [TestMethod]
            public void InsertsItemInListAtTheSpecifiedIndex()
            {
                var target = new TestableSnapshottingList<object>();
                var item = new object();

                target.Insert(0, item);

                Assert.AreSame(item, target.Collection[0]);
            }

            [TestMethod]
            public void ResetsSnapshotSoThatItIsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingList<object>();
                target.GetSnapshot();

                target.Insert(0, null);

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void LocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var target = new TestableSnapshottingList<object>();
                lock (target.Collection)
                {
                    anotherThread = Task.Run(() => target.Insert(0, new object()));
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        [TestClass]
        public class RemoveAt : SnapshottingListTest
        {
            [TestMethod]
            public void RemovesItemAtTheSpecifiedIndexInList()
            {
                var target = new TestableSnapshottingList<object> { null };

                target.RemoveAt(0);

                Assert.AreEqual(0, target.Collection.Count);
            }

            [TestMethod]
            public void ResetsSnapshotSoThatItIsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingList<object> { null };
                target.GetSnapshot();

                target.RemoveAt(0);

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void LocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var target = new TestableSnapshottingList<object> { null };
                lock (target.Collection)
                {
                    anotherThread = Task.Run(() => target.RemoveAt(0));
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        [TestClass]
        public class Item : SnapshottingListTest
        {
            [TestMethod]
            public void GetterReturnsSnapshotItemAtSpecifiedIndex()
            {
                var target = new TestableSnapshottingList<object>();
                var item = new object();
                target.Snapshot = new List<object> { item };

                Assert.AreSame(item, target[0]);
            }

            [TestMethod]
            public void SetterReplacesItemAtTheSpecifiedIndexInList()
            {
                var target = new TestableSnapshottingList<object> { null };
                var item = new object();

                target[0] = item;

                Assert.AreSame(item, target.Collection[0]);
            }

            [TestMethod]
            public void SetterResetsSnapshotSoThatItIsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingList<object> { null };
                target.GetSnapshot();

                target[0] = new object();

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void SetterLocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var target = new TestableSnapshottingList<object> { null };
                lock (target.Collection)
                {
                    anotherThread = Task.Run(() => target[0] = new object());
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        private class TestableSnapshottingList<T> : SnapshottingList<T>
        {
            public new IList<T> Collection
            {
                get { return base.Collection; }
            }

            public IList<T> Snapshot
            {
                get { return this.snapshot; }
                set { this.snapshot = value; }
            }

            public new IList<T> CreateSnapshot(IList<T> collection)
            {
                return base.CreateSnapshot(collection);
            }

            public new IList<T> GetSnapshot()
            {
                return base.GetSnapshot();
            }
        }
    }
}
