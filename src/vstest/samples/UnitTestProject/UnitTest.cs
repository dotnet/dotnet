// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace UnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// The passing test.
        /// </summary>
        [TestMethod]
        public void PassingTest()
        {
            Assert.AreEqual(2, 2);
        }

        /// <summary>
        /// The failing test.
        /// </summary>
        [TestMethod]
        public void FailingTest()
        {
            Assert.AreEqual(2, 3);
        }

        /// <summary>
        /// The skipping test.
        /// </summary>
        [Ignore]
        [TestMethod]
        public void SkippingTest()
        {
        }

        [TestCategory("CategoryA")]
        [TestMethod]
        public void TestWithTestCategory()
        {
            Assert.AreEqual(1, 1);
        }

        [Priority(0)]
        [TestMethod]
        public void TestWithPriority()
        {
            Assert.AreEqual(1, 1);
        }

        [TestProperty("Property1", "Value1")]
        [TestProperty("Property2", "Value2")]
        [TestMethod]
        public void TestWithProperties()
        {
            Assert.AreEqual(1, 1);
        }

        [TestCategory("CategoryA")]
        [Priority(1)]
        [TestProperty("Property2", "Value2")]
        [TestMethod]
        public void FailingTestWithTraits()
        {
            Assert.Fail();
        }
    }
}
