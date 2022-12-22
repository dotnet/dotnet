// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;
using System.IO;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleUnitTestProject
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
        [Priority(2)]
        [TestMethod]
        public void PassingTest()
        {
            Assert.AreEqual(2, 2);
        }

        /// <summary>
        /// The failing test.
        /// </summary>
        [TestCategory("CategoryA")]
        [Priority(3)]
        [TestMethod]
        public void FailingTest()
        {
#if NETFRAMEWORK
            // current App domain should be write to file to test DisableAppDomain acceptance test.
            var appDomainFilePath = Environment.GetEnvironmentVariable("TEST_ASSET_APPDOMAIN_TEST_PATH") ?? Path.Combine(Path.GetTempPath(), "appdomain_test.txt");
            File.WriteAllText(appDomainFilePath, "AppDomain FriendlyName: " + AppDomain.CurrentDomain.FriendlyName);
#endif
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
    }
}
