using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    /// <summary />
    [TestClass]
    public class GrowingCollectionTests
    {
        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            var collection = new GrowingCollection<string>();
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.Count);
        }

        /// <summary />
        [TestMethod]
        public void AddAndCount()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, collection.Count);
                collection.Add(i.ToString());
                Assert.AreEqual(i + 1, collection.Count);
            }

            Assert.AreEqual(5000, collection.Count);
            collection.Add(null);
            Assert.AreEqual(5001, collection.Count);

            Assert.AreEqual(5001, collection.Count);
            collection.Add("");
            Assert.AreEqual(5002, collection.Count);
        }

        /// <summary />
        [TestMethod]
        public void GetEnumerator()
        {
            var collection = new GrowingCollection<string>();
            GrowingCollection<string>.Enumerator typedEnumerator = collection.GetEnumerator();

            Assert.IsNotNull(typedEnumerator);

            {
                IEnumerator<string> enumerator = ((IEnumerable<string>) collection).GetEnumerator();
                Assert.IsNotNull(enumerator);
                Assert.IsTrue(enumerator is GrowingCollection<string>.Enumerator);
            }
            {
                IEnumerator enumerator = ((IEnumerable) collection).GetEnumerator();
                Assert.IsNotNull(enumerator);
                Assert.IsTrue(enumerator is GrowingCollection<string>.Enumerator);
            }
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_Count()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, collection.GetEnumerator().Count);
                collection.Add(i.ToString());
                Assert.AreEqual(i + 1, collection.GetEnumerator().Count);
            }

            Assert.AreEqual(5000, collection.GetEnumerator().Count);
            collection.Add(null);
            Assert.AreEqual(5001, collection.GetEnumerator().Count);

            Assert.AreEqual(5001, collection.GetEnumerator().Count);
            collection.Add("");
            Assert.AreEqual(5002, collection.GetEnumerator().Count);
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_MoveNextAndCurrent()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            }

            collection.Add(null);
            collection.Add("");

            GrowingCollection<string>.Enumerator enumerator = collection.GetEnumerator();

            Assert.ThrowsException<ArgumentOutOfRangeException>( () => enumerator.Current);
            Assert.ThrowsException<ArgumentOutOfRangeException>( () => ((IEnumerator) enumerator).Current);
            Assert.ThrowsException<ArgumentOutOfRangeException>( () => ((IEnumerator<string>) enumerator).Current);

            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual("", enumerator.Current);
                Assert.AreEqual("", ((IEnumerator) enumerator).Current);
                Assert.AreEqual("", ((IEnumerator<string>) enumerator).Current);
            }
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.IsNull(enumerator.Current);
                Assert.IsNull(((IEnumerator) enumerator).Current);
                Assert.IsNull(((IEnumerator<string>) enumerator).Current);
            }

            for (int i = 4999; i >= 0; i--)
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual(i.ToString(), enumerator.Current);
                Assert.AreEqual(i.ToString(), ((IEnumerator) enumerator).Current);
                Assert.AreEqual(i.ToString(), ((IEnumerator<string>) enumerator).Current);
            }
            
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsFalse(canMove);

                Assert.AreEqual("0", enumerator.Current);
                Assert.AreEqual("0", ((IEnumerator) enumerator).Current);
                Assert.AreEqual("0", ((IEnumerator<string>) enumerator).Current);
            }
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_Reset()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            }

            GrowingCollection<string>.Enumerator enumerator = collection.GetEnumerator();

            Assert.AreEqual(5000, enumerator.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerator.Current);

            for (int i = 4999; i >= 0; i--)
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual(i.ToString(), enumerator.Current);
            }

            {
                Assert.AreEqual(5000, enumerator.Count);

                bool canMove = enumerator.MoveNext();
                Assert.IsFalse(canMove);

                Assert.AreEqual("0", enumerator.Current);
                Assert.AreEqual(5000, enumerator.Count);
            }

            enumerator.Reset();

            Assert.AreEqual(5000, enumerator.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => enumerator.Current);

            for (int i = 4999; i >= 0; i--)
            {
                bool canMove = enumerator.MoveNext();
                Assert.IsTrue(canMove);

                Assert.AreEqual(i.ToString(), enumerator.Current);
            }

            {
                Assert.AreEqual(5000, enumerator.Count);

                bool canMove = enumerator.MoveNext();
                Assert.IsFalse(canMove);

                Assert.AreEqual("0", enumerator.Current);
                Assert.AreEqual(5000, enumerator.Count);
            }
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_Dispose()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            }

            GrowingCollection<string>.Enumerator enumerator = collection.GetEnumerator();

            for (int i = 4999; i >= 0; i--)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i.ToString(), enumerator.Current);

                enumerator.Dispose();  // Expects No-Op.
            }

            enumerator.Reset();

            for (int i = 4999; i >= 0; i--)
            {
                enumerator.MoveNext();
                Assert.AreEqual(i.ToString(), enumerator.Current);

                enumerator.Dispose();  // Expects No-Op.
            }

            Assert.AreEqual(5000, enumerator.Count);
        }

        /// <summary />
        [TestMethod]
        public void Enumerator_Foreach()
        {
            var collection = new GrowingCollection<string>();

            for (int i = 0; i < 5000; i++)
            {
                collection.Add(i.ToString());
            }
            collection.Add(null);
            collection.Add("");

            int j = 0;
            foreach (string s in collection)
            {
                Assert.AreEqual(
                            (j == 0)
                                    ? ""
                                    : (j == 1)
                                            ? null
                                            : (4999 - (j - 2)).ToString(),
                            s);
                j++;
            }
        }

        /// <summary />
        [TestMethod]
        public async Task MultipleEnumerators()
        {
            var collection = new GrowingCollection<string>();

            var tasks = new List<Task>();
            int[] workloads = new int[] { 1, 10, 20, 50, 60, 70, 75, 80, 85, 88, 90, 92, 94, 95, 96, 97, 98, 99 };
            int w = 0;

            for (int i = 0; i < 100; i++)
            {
                collection.Add(i.ToString());

                if (i == workloads[w])
                {
                    Task t = CheckEnumeratorUnderConcurrencyAsync(collection.GetEnumerator(), i + 1);
                    tasks.Add(t);
                    w++;
                }
            }

            Assert.AreEqual(workloads.Length, tasks.Count);

            await Task.WhenAll(tasks);
        }

        private static async Task CheckEnumeratorUnderConcurrencyAsync<T>(GrowingCollection<T>.Enumerator enumerator, int expectedCount)
        {
            Assert.IsNotNull(enumerator);
            Assert.IsTrue(expectedCount > 0);

            Random rnd = new Random();

            await Task.Delay(rnd.Next(10)).ConfigureAwait(continueOnCapturedContext: false);

            Assert.AreEqual(expectedCount, enumerator.Count);

            for (int i = expectedCount - 1; i >= 0; i--)
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(i.ToString(), enumerator.Current);
                await Task.Delay(rnd.Next(2)).ConfigureAwait(continueOnCapturedContext: false);
            }

            {
                Assert.IsFalse(enumerator.MoveNext());
            }

            enumerator.Reset();

            Assert.AreEqual(expectedCount, enumerator.Count);

            for (int i = expectedCount - 1; i >= 0; i--)
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(i.ToString(), enumerator.Current);
                await Task.Delay(rnd.Next(2)).ConfigureAwait(continueOnCapturedContext: false);
            }

            {
                Assert.IsFalse(enumerator.MoveNext());
            }
        }
    }
}
