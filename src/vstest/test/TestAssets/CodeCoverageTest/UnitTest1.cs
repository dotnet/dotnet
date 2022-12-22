// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCoverageTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Logic _logic;

        public UnitTest1()
        {
            _logic = new Logic();
        }

        [TestMethod]
        public void TestAbs()
        {
            Assert.AreEqual(_logic.Abs(0), 0);
            Assert.AreEqual(_logic.Abs(-5), 5);
            Assert.AreEqual(_logic.Abs(7), 7);
        }

        [TestMethod]
        public void TestSign()
        {
            Assert.AreEqual(_logic.Sign(0), 0);
            Assert.AreEqual(_logic.Sign(-5), -1);
            Assert.AreEqual(_logic.Sign(7), 1);
        }

        [TestMethod]
#pragma warning disable IDE1006 // Naming Styles
        public void __CxxPureMSILEntry_Test()
#pragma warning restore IDE1006 // Naming Styles
        {
            Assert.AreEqual(_logic.Abs(0), 0);
        }
    }
}
