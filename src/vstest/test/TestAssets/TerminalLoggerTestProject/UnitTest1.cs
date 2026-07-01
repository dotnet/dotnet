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
        }

        /// <summary>
        /// The failing test.
        /// </summary>
        [TestMethod]
        public void FailingTest()
        {
            // test characters taken from https://pages.ucsd.edu/~dkjordan/chin/unitestuni.html
#pragma warning disable MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
            Assert.AreEqual("ğğğ𦮙我們剛才從𓋴𓅓𓏏𓇏𓇌𓀀", "not the same");
#pragma warning restore MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
        }

        /// <summary>
        /// Validates that ~, !, |, and % in assertion messages are not corrupted
        /// by the MSBuildLogger encoding used by the TerminalLogger.
        /// </summary>
        [TestMethod]
        public void FailingTestWithSpecialChars()
        {
            // These characters were corrupted by the old ~~~~, !!!!, |||| encoding.
            // 5 tildes, 4 bangs, 4 pipes, and percent-n which could be confused with a newline escape.
#pragma warning disable MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
            Assert.AreEqual("~~~~~!!!!||||%n", "not the same");
#pragma warning restore MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
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
