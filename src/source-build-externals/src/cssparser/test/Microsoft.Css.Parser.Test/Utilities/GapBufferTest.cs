// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Utilities
{
    [TestClass]
    public class GapBufferTest
    {
        private static GapBuffer<int> CreateTestGapBuffer()
        {
            GapBuffer<int> buf = new GapBuffer<int>();

            for (int i = 100; i < 125; i++)
            {
                buf.Add(i);
            }

            buf.Insert(16, 999);

            return buf;
        }

        [TestMethod]
        public void GapBuffer_ConstructTest()
        {
            GapBuffer<int> buf = new GapBuffer<int>();

            Assert.AreEqual(0, buf.Count);
            Assert.AreEqual(0, buf.ToReal(0));
            Assert.AreEqual(0, buf.ToVirtual(0));
            Assert.AreEqual(0, buf.Gap);
            Assert.AreEqual(0, buf.AfterGap);
            Assert.AreEqual(0, buf.GapCount);
            Assert.AreEqual(0, buf.AllocatedCount);
            Assert.IsFalse(buf.IsReadOnly);
        }

        [TestMethod]
        public void GapBuffer_RealAndVirtualTest()
        {
            GapBuffer<int> buf = CreateTestGapBuffer();

            Assert.AreEqual(26, buf.Count);
            Assert.AreEqual(17, buf.Gap);
            Assert.AreEqual(23, buf.AfterGap);
            Assert.AreEqual(6, buf.GapCount);
            Assert.AreEqual(32, buf.AllocatedCount);

            Assert.AreEqual(26, buf.Count);
            Assert.AreEqual(0, buf.ToReal(0));
            Assert.AreEqual(16, buf.ToReal(16));
            Assert.AreEqual(23, buf.ToReal(17));
            Assert.AreEqual(32, buf.ToReal(26));

            Assert.AreEqual(0, buf.ToVirtual(0));
            Assert.AreEqual(16, buf.ToVirtual(16));
            Assert.AreEqual(17, buf.ToVirtual(23));
            Assert.AreEqual(26, buf.ToVirtual(32));

            buf.Clear();
            Assert.AreEqual(0, buf.Count);
            Assert.AreEqual(0, buf.ToReal(0));
            Assert.AreEqual(0, buf.ToVirtual(0));
        }

        [TestMethod]
        public void GapBuffer_IndexOfTest()
        {
            GapBuffer<int> buf = CreateTestGapBuffer();

            Assert.AreEqual(0, buf.IndexOf(100));
            Assert.AreEqual(15, buf.IndexOf(115));
            Assert.AreEqual(17, buf.IndexOf(116));
            Assert.AreEqual(25, buf.IndexOf(124));
            Assert.AreEqual(-1, buf.IndexOf(125));
            Assert.AreEqual(16, buf.IndexOf(999));

            Assert.IsTrue(buf.Contains(100));
            Assert.IsTrue(buf.Contains(999));
            Assert.IsFalse(buf.Contains(0));
        }

        [TestMethod]
        public void GapBuffer_RemoveTest()
        {
            GapBuffer<int> buf = CreateTestGapBuffer();

            Assert.IsFalse(buf.Remove(0));

            Assert.AreEqual(16, buf.IndexOf(999));
            Assert.IsTrue(buf.Remove(999));
            Assert.AreEqual(-1, buf.IndexOf(999));

            buf.RemoveAt(1);
            Assert.AreEqual(1, buf.Gap);

            buf.RemoveAt(0);
            Assert.AreEqual(23, buf.Count);
            Assert.AreEqual(0, buf.Gap);
            Assert.AreEqual(9, buf.GapCount);
            Assert.AreEqual(9, buf.AfterGap);

            buf.RemoveAt(21);
            Assert.AreEqual(21, buf.Gap);

            buf.RemoveAt(21);
            Assert.AreEqual(21, buf.Count);
            Assert.AreEqual(21, buf.Gap);
            Assert.AreEqual(11, buf.GapCount);
            Assert.AreEqual(32, buf.AfterGap);

            for (int i = 0; i < buf.Count; i++)
            {
                Assert.AreEqual(102 + i, buf[i]);
            }
        }

        [TestMethod]
        public void GapBuffer_RemoveRangeTest()
        {
            GapBuffer<int> buf = CreateTestGapBuffer();

            buf.RemoveRange(8, 16);
            Assert.AreEqual(10, buf.Count);
            Assert.AreEqual(8, buf.Gap);
            Assert.AreEqual(22, buf.GapCount);

            buf.RemoveRange(0, 5);
            Assert.AreEqual(5, buf.Count);
            Assert.AreEqual(0, buf.Gap);
            Assert.AreEqual(27, buf.GapCount);

            Assert.AreEqual(105, buf[0]);
            Assert.AreEqual(106, buf[1]);
            Assert.AreEqual(124, buf[4]);

            buf.RemoveRange(0, 5);
            Assert.AreEqual(0, buf.Count);
            Assert.AreEqual(0, buf.Gap);
            Assert.AreEqual(32, buf.GapCount);
        }

        [TestMethod]
        public void GapBuffer_ToArrayTest()
        {
            GapBuffer<int> buf = CreateTestGapBuffer();

            int[] array = buf.ToArray();
            Assert.AreEqual(buf.Count, array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(buf[i], array[i]);
            }
        }
    }
}
