// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;
using System.IO;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TerminalLoggerUnitTests
{
    /// <summary>
    /// The unit test 1.
    /// </summary>
    [TestClass]
    public class UnitTest1
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
            // test characters taken from https://pages.ucsd.edu/~dkjordan/chin/unitestuni.html
            Assert.AreEqual("ğğğ𦮙我們剛才從𓋴𓅓𓏏𓇏𓇌𓀀", 3);
        }

        /// <summary>
        /// The skipping test.
        /// </summary>
        [Ignore]
        [TestMethod]
        public void SkippingTest()
        {
        }
    }
}
