// <copyright file="SnapshottingCollectionTest.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    [TestClass]
    public class SnapshottingCollectionTest
    {
        [TestClass]
        public class Class : SnapshottingCollectionTest
        {
            [TestMethod]
            public void IsInternalAndNotMeantForDirectPublicConsumption()
            {
                Assert.IsFalse(typeof(SnapshottingCollection<,>).GetTypeInfo().IsPublic);
            }

            [TestMethod]
            public void ImplementsICollectionInterfaceExposedInPublicProperties()
            {
                Assert.IsTrue(typeof(ICollection<object>).IsAssignableFrom(typeof(SnapshottingCollection<object, ICollection<object>>)));
            }
        }

        [TestClass]
        public class Add : SnapshottingCollectionTest
        {
            [TestMethod]
            public void AddsItemToCollection()
            {
                var collection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(collection);
                var item = new object();

                target.Add(item);

                Assert.IsTrue(collection.Contains(item));
            }

            [TestMethod]
            public void ResetsSnapshotSoThatItIsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                target.GetSnapshot();

                target.Add(new object());

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void LocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var collection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(collection);
                lock (collection)
                {
                    anotherThread = Task.Run(() => target.Add(new object()));
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        [TestClass]
        public class Clear : SnapshottingCollectionTest
        {
            [TestMethod]
            public void RemovesItemsFromCollection()
            {
                var collection = new List<object> { new object() };
                var target = new TestableSnapshottingCollection<object>(collection);

                target.Clear();

                Assert.AreEqual(0, collection.Count);
            }

            [TestMethod]
            public void ResetsSnapshotSoThatItsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                target.GetSnapshot();

                target.Clear();

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void LocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var collection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(collection);
                lock (collection)
                {
                    anotherThread = Task.Run(() => target.Clear());
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        [TestClass]
        public class Contains : SnapshottingCollectionTest
        {
            [TestMethod]
            public void ReturnsTrueIfSnapshotContainsGivenItem()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var item = new object();
                target.OnCreateSnapshot = c => new List<object> { item };

                Assert.IsTrue(target.Contains(item));
            }
        }

        [TestClass]
        public class CopyTo : SnapshottingCollectionTest
        {
            [TestMethod]
            public void CopiesItemsFromSnapshotToGivenArrayAtSpecifiedIndex()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var item = new object();
                target.OnCreateSnapshot = c => new List<object> { item };

                object[] array = new object[50];
                target.CopyTo(array, 42);

                Assert.AreSame(item, array[42]);
            }
        }

        [TestClass]
        public class Count : SnapshottingCollectionTest
        {
            [TestMethod]
            public void ReturnsNumberOfItemsInSnapshot()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var snapshot = new List<object> { new object() };
                target.OnCreateSnapshot = c => snapshot;

                Assert.AreEqual(snapshot.Count, target.Count);
            }
        }

        [TestClass]
        public class GetEnumerator : SnapshottingCollectionTest
        {
            [TestMethod]
            public void ReturnsGenericEnumeratorOfSnapshot()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var item = new object();
                target.OnCreateSnapshot = c => new List<object> { item };

                IEnumerator<object> enumerator = target.GetEnumerator();

                enumerator.MoveNext();
                Assert.AreSame(item, enumerator.Current);
            }

            [TestMethod]
            public void ReturnsEnumeratorOfSnapshot()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var item = new object();
                target.OnCreateSnapshot = c => new List<object> { item };

                IEnumerator enumerator = ((IEnumerable)target).GetEnumerator();

                enumerator.MoveNext();
                Assert.AreSame(item, enumerator.Current);
            }
        }

        [TestClass]
        public class IsReadOnly : SnapshottingCollectionTest
        {
            [TestMethod]
            public void ReturnsFalseForConsistencyWithBuiltInCollectionTypes()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                Assert.IsFalse(target.IsReadOnly);
            }
        }

        [TestClass]
        public class Remove : SnapshottingCollectionTest
        {
            [TestMethod]
            public void RemovesItemFromCollection()
            {
                var item = new object();
                var collection = new List<object> { item };
                var target = new TestableSnapshottingCollection<object>(collection);

                target.Remove(item);

                Assert.AreEqual(0, collection.Count);
            }

            [TestMethod]
            public void ReturnsTrueIfItemWasRemovedSuccessfully()
            {
                var item = new object();
                var collection = new List<object> { item };
                var target = new TestableSnapshottingCollection<object>(collection);

                bool result = target.Remove(item);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ResetsSnapshotSoThatItsRecreatedAtNextRead()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object> { null });
                target.GetSnapshot();

                target.Remove(null);

                Assert.IsNull(target.Snapshot);
            }

            [TestMethod]
            public void DoesNotResetSnapshotWhenItemWasNotRemovedToAvoidUnnecessaryRecreationOfSnapshot()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var oldSnapshot = target.Snapshot;

                target.Remove(new object());

                Assert.AreSame(oldSnapshot, target.Snapshot);
            }

            [TestMethod]
            public void LocksCollectionForThreadSafety()
            {
                Task anotherThread;
                var collection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(collection);
                lock (collection)
                {
                    anotherThread = Task.Run(() => target.Remove(null));
                    Assert.IsFalse(anotherThread.Wait(20));
                }

                Assert.IsTrue(anotherThread.Wait(20));
            }
        }

        [TestClass]
        public class GetSnapshot : SnapshottingCollectionTest
        {
            [TestMethod]
            public void InvokesCreateSnapshotMethodToLetConcreteChildrenDoItEfficiently()
            {
                IList<object> actualCollection = null;
                var expectedCollection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                target.OnCreateSnapshot = c =>
                {
                    actualCollection = expectedCollection;
                    return null;
                };

                var dummy = target.GetSnapshot();

                Assert.AreSame(expectedCollection, actualCollection);
            }

            [TestMethod]
            public void ReusesPreviouslyCreatedSnapshotToImprovePerformance()
            {
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                var previouslyCreatedSnapshot = new List<object>();
                target.Snapshot = previouslyCreatedSnapshot;

                var returnedSnapshot = target.GetSnapshot();

                Assert.AreSame(previouslyCreatedSnapshot, returnedSnapshot);
            }

            [TestMethod]
            public void LocksCollectionWhileCreatingSnapshotForThreadSafety()
            {
                bool isCollectionLocked = false;
                var target = new TestableSnapshottingCollection<object>(new List<object>());
                target.OnCreateSnapshot = c =>
                {
                    isCollectionLocked = Monitor.IsEntered(c);
                    return null;
                };

                var dummy = target.GetSnapshot();

                Assert.IsTrue(isCollectionLocked);
            }

            [TestMethod]
            public void DoesNotKeepCollectionLockedToUnblockOtherThreads()
            {
                var collection = new List<object>();
                var target = new TestableSnapshottingCollection<object>(collection);

                var dummy = target.GetSnapshot();

                Assert.IsFalse(Monitor.IsEntered(collection));
            }
        }

        private class TestableSnapshottingCollection<T> : SnapshottingCollection<T, ICollection<T>>
        {
            public Func<ICollection<T>, ICollection<T>> OnCreateSnapshot = c => new List<T>(c);

            public TestableSnapshottingCollection(ICollection<T> collection)
                : base(collection)
            {
            }

            public ICollection<T> Snapshot
            {
                get { return this.snapshot; }
                set { this.snapshot = value; }
            }

            public new ICollection<T> GetSnapshot()
            {
                return base.GetSnapshot();
            }

            protected override ICollection<T> CreateSnapshot(ICollection<T> collection)
            {
                return this.OnCreateSnapshot(collection);
            }
        }
    }
}
