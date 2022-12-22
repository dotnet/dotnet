// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleUnitTestProject3
{
    [TestClass]
    public class TestSessionTimeoutTest
    {
        [TestMethod]
        public void TestWhichTakeSomeTime1()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }

        [TestMethod]
        public void TestWhichTakeSomeTime2()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }

        [TestMethod]
        public void TestWhichTakeSomeTime3()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }

        [TestMethod]
        public void TestWhichTakeSomeTime4()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }

        [TestMethod]
        public void TestWhichTakeSomeTime5()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }

        [TestMethod]
        public void TestWhichTakeSomeTime6()
        {
            System.Threading.Thread.Sleep(3 * 1000);
        }
    }
}
