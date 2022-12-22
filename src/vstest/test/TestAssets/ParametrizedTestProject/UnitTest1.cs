// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ParametrizedTestProject
{
    /// <summary>
    /// The unit test 1.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// The passing test.
        /// </summary>
        [TestMethod]
        public void CheckingParameters()
        {
            Assert.AreEqual("http://localhost//def", TestContext.Properties["weburl"]);
        }
    }
}
