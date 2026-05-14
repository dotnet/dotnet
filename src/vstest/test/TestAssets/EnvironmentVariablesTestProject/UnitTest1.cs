// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace EnvironmentVariablesTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var envVar = Environment.GetEnvironmentVariable("RANDOM_PATH");
            Assert.AreEqual(envVar, @"C:\temp");
        }
    }
}
