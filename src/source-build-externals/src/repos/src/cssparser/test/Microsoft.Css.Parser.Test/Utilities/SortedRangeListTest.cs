// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Css.Parser.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Css.Parser.Test.Utilities
{
    [TestClass]
    public class SortedRangeListTest : CssUnitTestBase
    {
        private class TestRange : IRange
        {
            public TestRange(int start, int length)
            {
                Start = start;
                Length = length;
            }

            public int Start { get; }
            public int Length { get; }
            public int AfterEnd => Start + Length;
        }

        [TestMethod]
        public void SortedRangeListChangeStampTest()
        {
            SortedRangeList<TestRange> list = new SortedRangeList<TestRange>();

            int stamp = list.ChangeStamp;
            list.ForceUpdateChangeStamp();
            Assert.AreNotEqual(stamp, list.ChangeStamp);
            stamp = list.ChangeStamp;

            list.Clear();
            Assert.AreEqual(stamp, list.ChangeStamp);

            list.Add(new TestRange(5, 1));
            list.Add(new TestRange(6, 1));
            Assert.AreNotEqual(stamp, list.ChangeStamp);
            stamp = list.ChangeStamp;

            list.IndexOf(list[0]);
            Assert.AreEqual(stamp, list.ChangeStamp);

            list.Remove(list[0]);
            Assert.AreNotEqual(stamp, list.ChangeStamp);
            stamp = list.ChangeStamp;

            list.Clear();
            Assert.AreNotEqual(stamp, list.ChangeStamp);
        }
    }
}
